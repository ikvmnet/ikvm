using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

using IKVM.Reflection;
using IKVM.Runtime;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Internal implementation of the IkvmExporter.
    /// </summary>
    static class IkvmExporterInternal
    {

        static ZipArchive zipFile;
        static Dictionary<string, string> done = new Dictionary<string, string>();
        static Dictionary<string, RuntimeJavaType> todo = new Dictionary<string, RuntimeJavaType>();
        static FileInfo file;

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="options"></param>
        public static int Execute(IkvmExporterOptions options)
        {
            IKVM.Runtime.Tracer.EnableTraceConsoleListener();
            IKVM.Runtime.Tracer.EnableTraceForDebug();

            var references = new List<string>();
            if (options.References != null)
                foreach (var reference in options.References)
                    references.Add(reference);

            var libpaths = new List<string>();
            if (options.Libraries != null)
                foreach (var library in options.Libraries)
                    libpaths.Add(library);

            var namespaces = new List<string>();
            if (options.Namespaces != null)
                foreach (var ns in options.Namespaces)
                    namespaces.Add(ns);

            if (File.Exists(options.Assembly) && options.NoStdLib)
            {
                // Add the target assembly to the references list, to allow it to be considered as "mscorlib".
                // This allows "ikvmstub -nostdlib \...\mscorlib.dll" to work.
                references.Add(options.Assembly);
            }

            StaticCompiler.Resolver.Warning += new AssemblyResolver.WarningEvent(Resolver_Warning);
            StaticCompiler.Resolver.Init(StaticCompiler.Universe, options.NoStdLib, references, libpaths);

            var cache = new Dictionary<string, Assembly>();
            foreach (var reference in references)
            {
                Assembly[] dummy = null;
                if (!StaticCompiler.Resolver.ResolveReference(cache, ref dummy, reference))
                {
                    Console.Error.WriteLine("Error: reference not found {0}", reference);
                    return 1;
                }
            }

            Assembly assembly = null;
            try
            {
                file = new FileInfo(options.Assembly);
            }
            catch (Exception x)
            {
                Console.Error.WriteLine("Error: unable to load \"{0}\"\n  {1}", options.Assembly, x.Message);
                return 1;
            }
            if (file != null && file.Exists)
            {
                assembly = StaticCompiler.LoadFile(options.Assembly);
            }
            else
            {
                assembly = StaticCompiler.Resolver.LoadWithPartialName(options.Assembly);
            }
            int rc = 0;
            if (assembly == null)
            {
                rc = 1;
                Console.Error.WriteLine("Error: Assembly \"{0}\" not found", options.Assembly);
            }
            else
            {
                if (options.Boostrap)
                {
                    StaticCompiler.runtimeAssembly = StaticCompiler.LoadFile(typeof(IkvmExporterTool).Assembly.Location);
                    RuntimeClassLoaderFactory.SetBootstrapClassLoader(new RuntimeBootstrapClassLoader());
                }
                else
                {
                    StaticCompiler.LoadFile(typeof(IkvmExporterInternal).Assembly.Location);

                    var runtimeAssemblyPath = references.FirstOrDefault(i => Path.GetFileNameWithoutExtension(i) == "IKVM.Runtime");
                    if (runtimeAssemblyPath != null)
                        StaticCompiler.runtimeAssembly = StaticCompiler.LoadFile(runtimeAssemblyPath);

                    var baseAssemblyPath = references.FirstOrDefault(i => Path.GetFileNameWithoutExtension(i) == "IKVM.Java");
                    if (baseAssemblyPath != null)
                        JVM.BaseAssembly = StaticCompiler.LoadFile(baseAssemblyPath);

                    if (StaticCompiler.runtimeAssembly == null || StaticCompiler.runtimeAssembly.__IsMissing)
                    {
                        Console.Error.WriteLine("Error: IKVM.Runtime not found.");
                        return 1;
                    }

                    if (JVM.BaseAssembly == null || StaticCompiler.runtimeAssembly.__IsMissing)
                    {
                        Console.Error.WriteLine("Error: IKVM.Java not found.");
                        return 1;
                    }
                }

                if (AttributeHelper.IsJavaModule(assembly.ManifestModule))
                    Console.Error.WriteLine("Warning: Running ikvmstub on ikvmc compiled assemblies is not supported.");

                if (options.Output == null)
                    options.Output = assembly.GetName().Name + ".jar";

                try
                {
                    using (zipFile = new ZipArchive(new FileStream(options.Output, FileMode.Create), ZipArchiveMode.Create))
                    {
                        try
                        {
                            var assemblies = new List<Assembly>();
                            assemblies.Add(assembly);

                            if (options.Shared)
                                LoadSharedClassLoaderAssemblies(assembly, assemblies);

                            foreach (var asm in assemblies)
                            {
                                if (ProcessTypes(options, asm.GetTypes()) != 0)
                                {
                                    rc = 1;
                                    if (options.ContinueOnError == false)
                                        break;
                                }

                                if (options.Forwarders && ProcessTypes(options, asm.ManifestModule.__GetExportedTypes()) != 0)
                                {
                                    rc = 1;
                                    if (options.ContinueOnError == false)
                                        break;
                                }
                            }
                        }
                        catch (System.Exception x)
                        {
                            Console.Error.WriteLine(x);

                            if (options.ContinueOnError == false)
                                Console.Error.WriteLine("Warning: Assembly reflection encountered an error. Resultant JAR may be incomplete.");

                            rc = 1;
                        }
                    }
                }
                catch (InvalidDataException x)
                {
                    rc = 1;
                    Console.Error.WriteLine("Error: {0}", x.Message);
                }
            }

            return rc;
        }

        static void Resolver_Warning(AssemblyResolver.WarningId warning, string message, string[] parameters)
        {
            if (warning != AssemblyResolver.WarningId.HigherVersion)
            {
                Console.Error.WriteLine("Warning: " + message, parameters);
            }
        }

        private static void LoadSharedClassLoaderAssemblies(Assembly assembly, List<Assembly> assemblies)
        {
            if (assembly.GetManifestResourceInfo("ikvm.exports") != null)
            {
                using (Stream stream = assembly.GetManifestResourceStream("ikvm.exports"))
                {
                    BinaryReader rdr = new BinaryReader(stream);
                    int assemblyCount = rdr.ReadInt32();
                    for (int i = 0; i < assemblyCount; i++)
                    {
                        string name = rdr.ReadString();
                        int typeCount = rdr.ReadInt32();
                        if (typeCount > 0)
                        {
                            for (int j = 0; j < typeCount; j++)
                            {
                                rdr.ReadInt32();
                            }
                            try
                            {
                                assemblies.Add(StaticCompiler.Load(name));
                            }
                            catch
                            {
                                Console.WriteLine("Warning: Unable to load shared class loader assembly: {0}", name);
                            }
                        }
                    }
                }
            }
        }

        static void WriteClass(IkvmExporterOptions options, RuntimeJavaType tw)
        {
            var entry = zipFile.CreateEntry(tw.Name.Replace('.', '/') + ".class");
            entry.LastWriteTime = new DateTime(1980, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            using Stream stream = entry.Open();

            IKVM.StubGen.StubGenerator.WriteClass(stream, tw, options.IncludeNonPublicTypes, options.IncludeNonPublicInterfaces, options.IncludeNonPublicMembers, options.IncludeParameterNames, options.SerialVersionUID);
        }

        static bool ExportNamespace(IList<string> namespaces, Type type)
        {
            if (namespaces.Count == 0)
                return true;

            var name = type.FullName;
            foreach (string ns in namespaces)
                if (name.StartsWith(ns, StringComparison.Ordinal))
                    return true;

            return false;
        }

        private static int ProcessTypes(IkvmExporterOptions options, Type[] types)
        {
            int rc = 0;
            foreach (var t in types)
            {
                if ((t.IsPublic || options.IncludeNonPublicTypes) && ExportNamespace(options.Namespaces, t) && !t.IsGenericTypeDefinition && !AttributeHelper.IsHideFromJava(t) && (!t.IsGenericType || !AttributeHelper.IsJavaModule(t.Module)))
                {
                    RuntimeJavaType c;
                    if (RuntimeClassLoaderFactory.IsRemappedType(t) || t.IsPrimitive || t == Types.Void)
                        c = RuntimeManagedJavaType.GetWrapperFromDotNetType(t);
                    else
                        c = RuntimeClassLoaderFactory.GetWrapperFromType(t);

                    if (c != null)
                        AddToExportList(c);
                }
            }

            bool keepGoing;
            do
            {
                keepGoing = false;
                foreach (var c in new List<RuntimeJavaType>(todo.Values).OrderBy(i => i.Name))
                {
                    if (!done.ContainsKey(c.Name))
                    {
                        keepGoing = true;
                        done.Add(c.Name, null);

                        try
                        {
                            ProcessClass(c);
                            WriteClass(options, c);
                        }
                        catch (Exception x)
                        {
                            if (options.ContinueOnError)
                            {
                                rc = 1;
                                Console.WriteLine(x);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            while (keepGoing);

            return rc;
        }

        private static void AddToExportList(RuntimeJavaType c)
        {
            todo[c.Name] = c;
        }

        private static bool IsNonVectorArray(RuntimeJavaType tw)
        {
            return !tw.IsArray && tw.TypeAsBaseType.IsArray;
        }

        private static void AddToExportListIfNeeded(RuntimeJavaType tw)
        {
            while (tw.IsArray)
                tw = tw.ElementTypeWrapper;

            if (tw.IsUnloadable && tw.Name.StartsWith("Missing/"))
            {
                Console.Error.WriteLine("Error: unable to find assembly '{0}'", tw.Name.Substring(8));
                Environment.Exit(1);
                return;
            }
            if (tw is RuntimeStubJavaType)
            {
                // skip
            }
            else if ((tw.TypeAsTBD != null && tw.TypeAsTBD.IsGenericType) || IsNonVectorArray(tw) || !tw.IsPublic)
            {
                AddToExportList(tw);
            }
        }

        private static void AddToExportListIfNeeded(RuntimeJavaType[] types)
        {
            foreach (var tw in types)
                AddToExportListIfNeeded(tw);
        }

        private static void ProcessClass(RuntimeJavaType tw)
        {
            RuntimeJavaType superclass = tw.BaseTypeWrapper;
            if (superclass != null)
                AddToExportListIfNeeded(superclass);

            AddToExportListIfNeeded(tw.Interfaces);

            var outerClass = tw.DeclaringTypeWrapper;
            if (outerClass != null)
                AddToExportList(outerClass);

            foreach (var innerClass in tw.InnerClasses)
                if (innerClass.IsPublic)
                    AddToExportList(innerClass);

            foreach (var mw in tw.GetMethods())
            {
                if (mw.IsPublic || mw.IsProtected)
                {
                    mw.Link();
                    AddToExportListIfNeeded(mw.ReturnType);
                    AddToExportListIfNeeded(mw.GetParameters());
                }
            }

            foreach (var fw in tw.GetFields())
            {
                if (fw.IsPublic || fw.IsProtected)
                {
                    fw.Link();
                    AddToExportListIfNeeded(fw.FieldTypeWrapper);
                }
            }
        }

    }

}
