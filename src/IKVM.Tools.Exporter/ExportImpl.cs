using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using IKVM.CoreLib.Diagnostics;
using IKVM.Reflection;
using IKVM.Runtime;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    class ExportImpl
    {

        readonly ExportOptions options;
        readonly IDiagnosticHandler diagnostics;

        readonly Dictionary<string, string> done = new Dictionary<string, string>();
        readonly Dictionary<string, RuntimeJavaType> todo = new Dictionary<string, RuntimeJavaType>();
        ZipArchive zipFile;
        FileInfo file;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public ExportImpl(ExportOptions options, IDiagnosticHandler diagnostics)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        public int Execute()
        {
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
                diagnostics.CoreClassesMissing();
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
                    diagnostics.ReferenceNotFound(reference);
                    return 1;
                }
            }

            Assembly assembly = null;
            try
            {
                file = new FileInfo(options.Assembly);
            }
            catch (FileNotFoundException)
            {
                diagnostics.InputFileNotFound(options.Assembly);
                return 1;
            }
            catch (Exception x)
            {
                diagnostics.ErrorReadingFile(options.Assembly, x.ToString());
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
                diagnostics.ReferenceNotFound(options.Assembly);
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
                        diagnostics.RuntimeNotFound();
                        return 1;
                    }

                    compiler = new StaticCompiler(universe, assemblyResolver, runtimeAssembly);
                    context = new RuntimeContext(new RuntimeContextOptions(), diagnostics, new ManagedTypeResolver(compiler, null), true, compiler);
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
                        diagnostics.RuntimeNotFound();
                        return 1;
                    }

                    if (baseAssembly == null || runtimeAssembly.__IsMissing)
                    {
                        diagnostics.CoreClassesMissing();
                        return 1;
                    }

                    compiler = new StaticCompiler(universe, assemblyResolver, runtimeAssembly);
                    context = new RuntimeContext(new RuntimeContextOptions(), diagnostics, new ManagedTypeResolver(compiler, baseAssembly), false, compiler);
                }

                if (context.AttributeHelper.IsJavaModule(assembly.ManifestModule))
                {
                    diagnostics.ExportingImportsNotSupported();
                    return 1;
                }

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
                                if (ProcessTypes(context, asm.GetTypes()) != 0)
                                {
                                    rc = 1;
                                    if (options.ContinueOnError == false)
                                        break;
                                }

                                if (options.Forwarders && ProcessTypes(context, asm.ManifestModule.__GetExportedTypes()) != 0)
                                {
                                    rc = 1;
                                    if (options.ContinueOnError == false)
                                        break;
                                }
                            }
                        }
                        catch (Exception x)
                        {
                            if (options.ContinueOnError == false)
                                diagnostics.UnknownWarning($"Assembly reflection encountered an error. Resultant JAR may be incomplete. ({x.Message})");

                            rc = 1;
                        }
                    }
                }
                catch (InvalidDataException e)
                {
                    rc = 1;
                    diagnostics.InvalidZip(options.Output);
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
        /// Returns the assembly name if the given assembly is a core library.
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
                using (var stream = assembly.GetManifestResourceStream("ikvm.exports"))
                {
                    var rdr = new BinaryReader(stream);
                    var assemblyCount = rdr.ReadInt32();
                    for (int i = 0; i < assemblyCount; i++)
                    {
                        var name = rdr.ReadString();
                        var typeCount = rdr.ReadInt32();
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

        void WriteClass(RuntimeJavaType javaType)
        {
            var entry = zipFile.CreateEntry(javaType.Name.Replace('.', '/') + ".class");
            entry.LastWriteTime = new DateTime(1980, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            using Stream stream = entry.Open();

            javaType.Context.StubGenerator.Write(stream, javaType, options.IncludeNonPublicTypes, options.IncludeNonPublicInterfaces, options.IncludeNonPublicMembers, options.IncludeParameterNames, options.SerialVersionUID);
        }

        bool ExportNamespace(IList<string> namespaces, Type type)
        {
            if (namespaces.Count == 0)
                return true;

            var name = type.FullName;
            foreach (string ns in namespaces)
                if (name.StartsWith(ns, StringComparison.Ordinal))
                    return true;

            return false;
        }

        int ProcessTypes(RuntimeContext context, Type[] types)
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
                            rc = ProcessClass(c);
                            if (rc != 0)
                                return rc;

                            WriteClass(c);
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

        void AddToExportList(RuntimeJavaType c)
        {
            todo[c.Name] = c;
        }

        bool IsNonVectorArray(RuntimeJavaType tw)
        {
            return !tw.IsArray && tw.TypeAsBaseType.IsArray;
        }

        int AddToExportListIfNeeded(RuntimeJavaType javaType)
        {
            while (javaType.IsArray)
                javaType = javaType.ElementTypeWrapper;

            if (javaType.IsUnloadable && javaType is RuntimeUnloadableJavaType unloadableJavaType)
            {
                javaType.Diagnostics.MissingType(unloadableJavaType.MissingType.Name, unloadableJavaType.MissingType.Assembly.FullName);
                return 1;
            }

            if (javaType is RuntimeStubJavaType)
            {
                // skip
            }
            else if ((javaType.TypeAsTBD != null && javaType.TypeAsTBD.IsGenericType) || IsNonVectorArray(javaType) || !javaType.IsPublic)
            {
                AddToExportList(javaType);
            }

            return 0;
        }

        int AddToExportListIfNeeded(RuntimeJavaType[] types)
        {
            foreach (var tw in types)
            {
                var rc = AddToExportListIfNeeded(tw);
                if (rc != 0)
                    return rc;
            }

            return 0;
        }

        int ProcessClass(RuntimeJavaType tw)
        {
            int rc = 0;

            var superclass = tw.BaseTypeWrapper;
            if (superclass != null)
            {
                rc = AddToExportListIfNeeded(superclass);
                if (rc != 0)
                    return rc;
            }

            rc = AddToExportListIfNeeded(tw.Interfaces);
            if (rc != 0)
                return rc;

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

                    rc = AddToExportListIfNeeded(mw.ReturnType);
                    if (rc != 0)
                        return rc;

                    rc = AddToExportListIfNeeded(mw.GetParameters());
                    if (rc != 0)
                        return rc;
                }
            }

            foreach (var fw in tw.GetFields())
            {
                if (fw.IsPublic || fw.IsProtected)
                {
                    fw.Link();
                    rc = AddToExportListIfNeeded(fw.FieldTypeWrapper);
                    if (rc != 0)
                        return rc;
                }
            }

            return 0;
        }

    }

}
