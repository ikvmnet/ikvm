using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Xml;

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

        class ManagedResolver : IManagedTypeResolver
        {

            readonly StaticCompiler compiler;
            readonly Assembly baseAssembly;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="compiler"></param>
            /// <param name="baseAssembly"></param>
            public ManagedResolver(StaticCompiler compiler, Assembly baseAssembly)
            {
                this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
                this.baseAssembly = baseAssembly;
            }

            /// <summary>
            /// Attempts to resolve the base Java assembly.
            /// </summary>
            /// <returns></returns>
            public Assembly ResolveBaseAssembly()
            {
                return baseAssembly;
            }

            /// <summary>
            /// Attempts to resolve an assembly from one of the assembly sources.
            /// </summary>
            /// <param name="assemblyName"></param>
            /// <returns></returns>
            public Assembly ResolveAssembly(string assemblyName)
            {
                return compiler.Load(assemblyName);
            }

            /// <summary>
            /// Attempts to resolve a type from one of the assembly sources.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            public Type ResolveCoreType(string typeName)
            {
                foreach (var assembly in compiler.Universe.GetAssemblies())
                    if (assembly.GetType(typeName) is Type t)
                        return t;

                return null;
            }

            /// <summary>
            /// Attempts to resolve a type from the IKVM runtime assembly.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            public Type ResolveRuntimeType(string typeName)
            {
                return compiler.GetRuntimeType(typeName);
            }
        }

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

            // discover the core lib from the references
            var coreLibName = FindCoreLibName(references, libpaths);
            if (coreLibName == null)
            {
                Console.Error.WriteLine("Error: core library not found");
                return 1;
            }

            // build universe and resolver against universe and references
            var universe = new Universe(coreLibName);
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Warning += new AssemblyResolver.WarningEvent(Resolver_Warning);
            assemblyResolver.Init(universe, options.NoStdLib, references, libpaths);

            var cache = new Dictionary<string, Assembly>();
            foreach (var reference in references)
            {
                Assembly[] dummy = null;
                if (assemblyResolver.ResolveReference(cache, ref dummy, reference) == false)
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
                assembly = assemblyResolver.LoadFile(options.Assembly);
            }
            else
            {
                assembly = assemblyResolver.LoadWithPartialName(options.Assembly);
            }

            StaticCompiler compiler;
            RuntimeContext context;

            int rc = 0;
            if (assembly == null)
            {
                rc = 1;
                Console.Error.WriteLine("Error: Assembly \"{0}\" not found", options.Assembly);
            }
            else
            {
                Assembly runtimeAssembly = null;
                Assembly baseAssembly = null;

                if (options.Bootstrap)
                {
                    var runtimeAssemblyPath = references.FirstOrDefault(i => Path.GetFileNameWithoutExtension(i) == "IKVM.Runtime");
                    if (runtimeAssemblyPath != null)
                        runtimeAssembly = assemblyResolver.LoadFile(runtimeAssemblyPath);

                    if (runtimeAssembly == null || runtimeAssembly.__IsMissing)
                    {
                        Console.Error.WriteLine("Error: IKVM.Runtime not found.");
                        return 1;
                    }

                    compiler = new StaticCompiler(universe, assemblyResolver, runtimeAssembly);
                    context = new RuntimeContext(new RuntimeContextOptions(), new ManagedResolver(compiler, null), true, compiler);
                    context.ClassLoaderFactory.SetBootstrapClassLoader(new RuntimeBootstrapClassLoader(context));
                }
                else
                {

                    var runtimeAssemblyPath = references.FirstOrDefault(i => Path.GetFileNameWithoutExtension(i) == "IKVM.Runtime");
                    if (runtimeAssemblyPath != null)
                        runtimeAssembly = assemblyResolver.LoadFile(runtimeAssemblyPath);

                    var baseAssemblyPath = references.FirstOrDefault(i => Path.GetFileNameWithoutExtension(i) == "IKVM.Java");
                    if (baseAssemblyPath != null)
                        baseAssembly = assemblyResolver.LoadFile(baseAssemblyPath);

                    if (runtimeAssembly == null || runtimeAssembly.__IsMissing)
                    {
                        Console.Error.WriteLine("Error: IKVM.Runtime not found.");
                        return 1;
                    }

                    if (baseAssembly == null || runtimeAssembly.__IsMissing)
                    {
                        Console.Error.WriteLine("Error: IKVM.Java not found.");
                        return 1;
                    }

                    compiler = new StaticCompiler(universe, assemblyResolver, runtimeAssembly);
                    context = new RuntimeContext(new RuntimeContextOptions(), new ManagedResolver(compiler, baseAssembly), false, compiler);
                }

                if (context.AttributeHelper.IsJavaModule(assembly.ManifestModule))
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
                                LoadSharedClassLoaderAssemblies(compiler, assembly, assemblies);

                            foreach (var asm in assemblies)
                            {
                                if (ProcessTypes(context, options, asm.GetTypes()) != 0)
                                {
                                    rc = 1;
                                    if (options.ContinueOnError == false)
                                        break;
                                }

                                if (options.Forwarders && ProcessTypes(context, options, asm.ManifestModule.__GetExportedTypes()) != 0)
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

        /// <summary>
        /// Finds the first potential core library in the reference set.
        /// </summary>
        /// <param name="references"></param>
        /// <param name="libpaths"></param>
        /// <returns></returns>
        static string FindCoreLibName(List<string> references, List<string> libpaths)
        {
            foreach (var reference in references)
                if (GetAssemblyNameIfCoreLib(reference) is string coreLibName)
                    return coreLibName;

            return null;
        }

        /// <summary>
        /// Returns <c>true</c> if the given assembly is a core library.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string GetAssemblyNameIfCoreLib(string path)
        {
            if (File.Exists(path) == false)
                return null;

            try
            {
                using var st = File.OpenRead(path);
                using var pe = new PEReader(st);
                var mr = pe.GetMetadataReader();

                foreach (var handle in mr.TypeDefinitions)
                    if (IsSystemObject(mr, handle))
                        return mr.GetString(mr.GetAssemblyDefinition().Name);

                return null;
            }
            catch (System.BadImageFormatException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the given type definition handle refers to "System.Object".
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="th"></param>
        /// <returns></returns>
        static bool IsSystemObject(MetadataReader reader, TypeDefinitionHandle th)
        {
            var td = reader.GetTypeDefinition(th);
            var ns = reader.GetString(td.Namespace);
            var nm = reader.GetString(td.Name);

            return ns == "System" && nm == "Object";
        }

        static void Resolver_Warning(AssemblyResolver.WarningId warning, string message, string[] parameters)
        {
            if (warning != AssemblyResolver.WarningId.HigherVersion)
                Console.Error.WriteLine("Warning: " + message, parameters);
        }

        static void LoadSharedClassLoaderAssemblies(StaticCompiler compiler, Assembly assembly, List<Assembly> assemblies)
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
                                assemblies.Add(compiler.Load(name));
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

            tw.Context.StubGenerator.WriteClass(stream, tw, options.IncludeNonPublicTypes, options.IncludeNonPublicInterfaces, options.IncludeNonPublicMembers, options.IncludeParameterNames, options.SerialVersionUID);
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

        private static int ProcessTypes(RuntimeContext context, IkvmExporterOptions options, Type[] types)
        {
            int rc = 0;
            foreach (var t in types)
            {
                if ((t.IsPublic || options.IncludeNonPublicTypes) && ExportNamespace(options.Namespaces, t) && !t.IsGenericTypeDefinition && !context.AttributeHelper.IsHideFromJava(t) && (!t.IsGenericType || !context.AttributeHelper.IsJavaModule(t.Module)))
                {
                    RuntimeJavaType c;
                    if (context.ClassLoaderFactory.IsRemappedType(t) || t.IsPrimitive || t == context.Types.Void)
                        c = context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(t);
                    else
                        c = context.ClassLoaderFactory.GetJavaTypeFromType(t);

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
