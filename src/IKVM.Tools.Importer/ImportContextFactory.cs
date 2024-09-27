using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using IKVM.ByteCode;
using IKVM.CoreLib.Diagnostics;
using IKVM.Reflection;
using IKVM.Runtime;
using IKVM.Tools.Core.Diagnostics;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Processes a hierarchy set of <see cref="ImportOptions"/>s into a hierarchal set of <see cref="ImportContext"/> instances.
    /// </summary>
    class ImportContextFactory
    {

        readonly RuntimeContext runtime;
        readonly ImportAssemblyResolver resolver;
        readonly StaticCompiler compiler;
        readonly IDiagnosticHandler diagnostics;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="resolver"></param>
        /// <param name="compiler"></param>
        /// <param name="diagnostics"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImportContextFactory(RuntimeContext runtime, ImportAssemblyResolver resolver, StaticCompiler compiler, IDiagnosticHandler diagnostics)
        {
            this.runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        /// <summary>
        /// Creates a hierarchy of <see cref="ImportContexty"/> instances that represent the compilation from a root options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<ImportContext> Create(ImportOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var imports = new List<ImportContext>();

            // create root import context
            var import = new ImportContext();
            import.target = IKVM.CoreLib.Symbols.Emit.PEFileKinds.ConsoleApplication;
            import.guessFileKind = true;
            import.version = new Version(0, 0, 0, 0);
            import.apartment = ApartmentState.STA;
            import.props = new Dictionary<string, string>();
            Create(options, import, imports);
            ResolveReferences(imports);
            ResolveStrongNameKeys(imports);
            return imports;
        }

        /// <summary>
        /// Creates a hierarchy of <see cref="ImportContextFactory"/> instances that represent the compilation.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="import"></param>
        /// <param name="targets"></param>
        /// <exception cref="FatalCompilerErrorException"></exception>
        void Create(ImportOptions options, ImportContext import, List<ImportContext> targets)
        {
            if (options.Output != null)
                import.path = options.Output;

            if (options.AssemblyName != null)
                import.assembly = options.AssemblyName;

            switch (options.Target)
            {
                case ImportTarget.Exe:
                    import.target = IKVM.CoreLib.Symbols.Emit.PEFileKinds.ConsoleApplication;
                    import.guessFileKind = false;
                    break;
                case ImportTarget.WinExe:
                    import.target = IKVM.CoreLib.Symbols.Emit.PEFileKinds.WindowApplication;
                    import.guessFileKind = false;
                    break;
                case ImportTarget.Module:
                    import.targetIsModule = true;
                    import.target = IKVM.CoreLib.Symbols.Emit.PEFileKinds.Dll;
                    import.guessFileKind = false;
                    break;
                case ImportTarget.Library:
                    import.target = IKVM.CoreLib.Symbols.Emit.PEFileKinds.Dll;
                    import.guessFileKind = false;
                    break;
                default:
                    throw new FatalCompilerErrorException(DiagnosticEvent.UnrecognizedTargetType(options.Target.ToString()));
            }

            switch (options.Platform)
            {
                case ImportPlatform.X86:
                    import.pekind = System.Reflection.PortableExecutableKinds.ILOnly | System.Reflection.PortableExecutableKinds.Required32Bit;
                    import.imageFileMachine = IKVM.CoreLib.Symbols.ImageFileMachine.I386;
                    break;
                case ImportPlatform.X64:
                    import.pekind = System.Reflection.PortableExecutableKinds.ILOnly | System.Reflection.PortableExecutableKinds.PE32Plus;
                    import.imageFileMachine = IKVM.CoreLib.Symbols.ImageFileMachine.AMD64;
                    break;
                case ImportPlatform.ARM:
                    import.pekind = System.Reflection.PortableExecutableKinds.ILOnly;
                    import.imageFileMachine = IKVM.CoreLib.Symbols.ImageFileMachine.ARM;
                    break;
                case ImportPlatform.ARM64:
                    import.pekind = System.Reflection.PortableExecutableKinds.ILOnly;
                    import.imageFileMachine = IKVM.CoreLib.Symbols.ImageFileMachine.ARM64;
                    break;
                case ImportPlatform.AnyCpu32BitPreferred:
                    import.pekind = System.Reflection.PortableExecutableKinds.ILOnly | System.Reflection.PortableExecutableKinds.Preferred32Bit;
                    import.imageFileMachine = IKVM.CoreLib.Symbols.ImageFileMachine.Unknown;
                    break;
                case ImportPlatform.AnyCpu:
                    import.pekind = System.Reflection.PortableExecutableKinds.ILOnly;
                    import.imageFileMachine = IKVM.CoreLib.Symbols.ImageFileMachine.Unknown;
                    break;
                default:
                    throw new FatalCompilerErrorException(DiagnosticEvent.UnrecognizedPlatform(options.Platform.ToString()));
            }

            switch (options.Apartment)
            {
                case ImportApartment.STA:
                    import.apartment = ApartmentState.STA;
                    break;
                case ImportApartment.MTA:
                    import.apartment = ApartmentState.MTA;
                    break;
                case ImportApartment.None:
                    import.apartment = ApartmentState.Unknown;
                    break;
                default:
                    throw new FatalCompilerErrorException(DiagnosticEvent.UnrecognizedApartment(options.Apartment.ToString()));
            }

            if (options.NoGlobbing)
                import.noglobbing = true;

            if (options.Properties.Count > 0)
                foreach (var kvp in options.Properties)
                    import.props[kvp.Key] = kvp.Value;

            if (options.EnableAssertions != null)
            {
                if (options.EnableAssertions.Length == 0)
                    import.props["ikvm.assert.default"] = "true";
                else
                    import.props["ikvm.assert.enable"] = string.Join(";", options.EnableAssertions);
            }

            if (options.DisableAssertions != null)
            {
                if (options.DisableAssertions.Length == 0)
                    import.props["ikvm.assert.default"] = "false";
                else
                    import.props["ikvm.assert.disable"] = string.Join(";", options.DisableAssertions);
            }

            if (options.RemoveAssertions)
                import.codegenoptions |= CodeGenOptions.RemoveAsserts;

            if (options.Main != null)
                import.mainClass = options.Main;

            foreach (var reference in options.References)
                import.unresolvedReferences.Add(reference);

            foreach (var spec in options.Recurse)
            {
                var exists = false;

                // MONOBUG On Mono 1.0.2, Directory.Exists throws an exception if we pass an invalid directory name
                try
                {
                    exists = Directory.Exists((string)spec);
                }
                catch (IOException)
                {

                }

                var found = false;
                if (exists)
                {
                    var dir = new DirectoryInfo(spec);
                    found = Recurse(import, dir, dir, "*");
                }
                else
                {
                    try
                    {
                        var dir = new DirectoryInfo(Path.GetDirectoryName(spec));
                        if (dir.Exists)
                        {
                            found = Recurse(import, dir, dir, Path.GetFileName(spec));
                        }
                        else
                        {
                            found = RecurseJar(import, spec);
                        }
                    }
                    catch (PathTooLongException)
                    {
                        throw new FatalCompilerErrorException(DiagnosticEvent.PathTooLong(spec));
                    }
                    catch (DirectoryNotFoundException)
                    {
                        throw new FatalCompilerErrorException(DiagnosticEvent.PathNotFound(spec));
                    }
                    catch (ArgumentException)
                    {
                        throw new FatalCompilerErrorException(DiagnosticEvent.InvalidPath(spec));
                    }
                }

                if (!found)
                    throw new FatalCompilerErrorException(DiagnosticEvent.FileNotFound(spec));
            }

            foreach (var kvp in options.Resources)
            {
                var fileInfo = GetFileInfo(kvp.Value.FullName);
                var fileName = kvp.Key.TrimStart('/').TrimEnd('/');
                import.GetResourcesJar().Add(fileName, ReadAllBytes(fileInfo), fileInfo);
            }

            foreach (var kvp in options.ExternalResources)
            {
                if (!File.Exists(kvp.Value.FullName))
                    throw new FatalCompilerErrorException(DiagnosticEvent.ExternalResourceNotFound(kvp.Value.FullName));
                if (Path.GetFileName(kvp.Value.FullName) != kvp.Value.FullName)
                    throw new FatalCompilerErrorException(DiagnosticEvent.ExternalResourceNameInvalid(kvp.Value.FullName));

                // TODO resource name clashes should be tested
                import.externalResources ??= new Dictionary<string, string>();
                import.externalResources.Add(kvp.Key, kvp.Value.FullName);
            }

            if (options.NoJNI)
                import.codegenoptions |= CodeGenOptions.NoJNI;

            if (options.Exclude != null)
                ProcessExclusionFile(ref import.classesToExclude, options.Exclude.FullName);

            if (options.Version != null)
                import.version = options.Version;

            if (options.FileVersion != null)
                import.fileversion = options.FileVersion.ToString();

            if (options.Win32Icon != null)
                import.iconfile = GetFileInfo(options.Win32Icon.FullName);

            if (options.Win32Manifest != null)
                import.manifestFile = GetFileInfo(options.Win32Manifest.FullName);

            if (options.KeyFile != null)
                import.keyfile = GetFileInfo(options.KeyFile.FullName);

            if (options.Key != null)
                import.keycontainer = options.Key;

            if (options.DelaySign)
                import.delaysign = true;

            switch (options.Debug)
            {
                case ImportDebug.Full:
                    import.codegenoptions |= CodeGenOptions.EmitSymbols;
                    import.debugMode = DebugMode.Full;
                    break;
                case ImportDebug.Portable:
                    import.codegenoptions |= CodeGenOptions.EmitSymbols;
                    import.debugMode = DebugMode.Portable;
                    break;
                case ImportDebug.Embedded:
                    import.codegenoptions |= CodeGenOptions.EmitSymbols;
                    import.debugMode = DebugMode.Embedded;
                    break;
            }

            if (options.Optimize == false)
                import.codegenoptions |= CodeGenOptions.DisableOptimizations;

            if (options.SourcePath != null)
                import.sourcepath = options.SourcePath.FullName;

            if (options.Remap != null)
                import.remapfile = GetFileInfo(options.Remap.FullName);

            if (options.NoStackTraceInfo)
                import.codegenoptions |= CodeGenOptions.NoStackTraceInfo;

            if (options.RemoveUnusedPrivateFields)
                import.codegenoptions |= CodeGenOptions.RemoveUnusedFields;

            if (options.CompressResources)
                import.compressedResources = true;

            if (options.StrictFinalFieldSemantics)
                import.codegenoptions |= CodeGenOptions.StrictFinalFieldSemantics;

            if (options.PrivatePackages != null)
                foreach (var prefix in options.PrivatePackages)
                    import.privatePackages = [.. import.privatePackages, prefix];

            if (options.PublicPackages != null)
                foreach (var prefix in options.PublicPackages)
                    import.publicPackages = [.. import.privatePackages, prefix];

            if (options.ClassLoader != null)
                import.classLoader = options.ClassLoader;

            if (options.SharedClassLoader)
                import.sharedclassloader ??= new List<ImportClassLoader>();

            if (options.BaseAddress != null)
            {
                var baseAddress = options.BaseAddress;
                ulong baseAddressParsed;
                if (baseAddress.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    baseAddressParsed = ulong.Parse(baseAddress.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
                else
                    baseAddressParsed = ulong.Parse(baseAddress); // note that unlike CSC we don't support octal

                import.baseAddress = (baseAddressParsed & 0xFFFFFFFFFFFF0000UL);
            }

            if (options.FileAlign != null)
            {
                if (!uint.TryParse(options.FileAlign, out var filealign) || filealign < 512 || filealign > 8192 || (filealign & (filealign - 1)) != 0)
                    throw new FatalCompilerErrorException(DiagnosticEvent.InvalidFileAlignment(options.FileAlign));

                import.fileAlignment = filealign;
            }

            if (options.NoPeerCrossReference)
                import.crossReferenceAllPeers = false;

            if (options.NoAutoSerialization)
                import.codegenoptions |= CodeGenOptions.NoAutomagicSerialization;

            if (options.HighEntropyVA)
                import.highentropyva = true;

            if (options.Proxies != null)
            {
                foreach (var proxy in options.Proxies)
                {
                    if (import.proxies.Contains(proxy))
                        diagnostics.DuplicateProxy(proxy);

                    import.proxies.Add(proxy);
                }
            }

            if (options.AllowNonVirtualCalls)
                JVM.AllowNonVirtualCalls = true;

            if (options.Static)
            {
                // we abuse -static to also enable support for NoRefEmit scenarios
                import.codegenoptions |= CodeGenOptions.DisableDynamicBinding | CodeGenOptions.NoRefEmitHelpers;
            }

            if (options.NoJarStubs)    // undocumented temporary option to mitigate risk
                import.nojarstubs = true;

            if (options.AssemblyAttributes != null)
                foreach (var i in options.AssemblyAttributes)
                    ProcessAttributeAnnotationsClass(import.assemblyAttributeAnnotations, i.FullName);

            if (options.WarningLevel4Option) // undocumented option to always warn if a class isn't found
                import.warningLevelHigh = true;

            if (options.NoParameterReflection) // undocumented option to compile core class libraries with, to disable MethodParameter attribute
                import.noParameterReflection = true;

            if (options.Bootstrap)
                import.bootstrap = true;

            if (import.targetIsModule && import.sharedclassloader != null)
                throw new FatalCompilerErrorException(DiagnosticEvent.SharedClassLoaderCannotBeUsedOnModuleTarget());

            ReadFiles(import, options.Inputs.Select(i => i.FullName).ToList());

            // process nested options
            foreach (var nestedOptions in options.Nested)
            {
                var nestedImport = new ImportContext();
                nestedImport.manifestMainClass = import.manifestMainClass;
                nestedImport.defaultAssemblyName = import.defaultAssemblyName;
                Create(nestedOptions, nestedImport, targets);
            }

            if (import.assembly == null)
            {
                var basename = import.path == null ? import.defaultAssemblyName : import.path.Name;
                if (basename == null)
                    throw new FatalCompilerErrorException(DiagnosticEvent.NoOutputFileSpecified());

                int idx = basename.LastIndexOf('.');
                if (idx > 0)
                    import.assembly = basename.Substring(0, idx);
                else
                    import.assembly = basename;
            }

            if (import.path != null && import.guessFileKind)
            {
                if (import.path.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                    import.target = IKVM.CoreLib.Symbols.Emit.PEFileKinds.Dll;

                import.guessFileKind = false;
            }

            if (import.mainClass == null && import.manifestMainClass != null && (import.guessFileKind || import.target != IKVM.CoreLib.Symbols.Emit.PEFileKinds.Dll))
            {
                diagnostics.MainMethodFromManifest(import.manifestMainClass);
                import.mainClass = import.manifestMainClass;
            }

            // schedule run if leaf-node
            if (options.Nested == null || options.Nested.Length == 0)
                targets.Add(import);
        }

        /// <summary>
        /// Resolves the intra-peer references in the list of imports.
        /// </summary>
        /// <param name="imports"></param>
        /// <exception cref="FatalCompilerErrorException"></exception>
        void ResolveReferences(List<ImportContext> imports)
        {
            var cache = new Dictionary<string, Assembly>();

            foreach (var import in imports)
            {
                if (import.unresolvedReferences.Count > 0)
                {
                    var refs = new List<Assembly>();
                    var cont = true;

                    foreach (var reference in import.unresolvedReferences)
                    {
                        if (cont == false)
                            break;

                        foreach (var peer in imports)
                        {
                            if (peer.assembly.Equals(reference, StringComparison.OrdinalIgnoreCase))
                            {
                                import.peerReferences = [.. import.peerReferences, peer.assembly];
                                cont = false;
                                break;
                            }
                        }

                        if (resolver.ResolveReference(cache, refs, reference) == false)
                            throw new FatalCompilerErrorException(DiagnosticEvent.ReferenceNotFound(reference));
                    }

                    // transform references into symbols
                    foreach (var a in refs)
                        import.references.Add(runtime.Resolver.ImportAssembly(a));
                }

            }


            // verify that we didn't reference any secondary assemblies of a shared class loader group
            foreach (var import in imports)
            {
                if (import.references != null)
                {
                    foreach (var asm in import.references)
                    {
                        var forwarder = asm.GetType("__<MainAssembly>");
                        if (forwarder != null && forwarder.Assembly != asm)
                            diagnostics.NonPrimaryAssemblyReference(asm.Location, forwarder.Assembly.GetName().Name);
                    }
                }
            }

            // now pre-load the secondary assemblies of any shared class loader groups
            foreach (var import in imports)
                if (import.references != null)
                    foreach (var asm in import.references)
                        RuntimeAssemblyClassLoader.PreloadExportedAssemblies(compiler, asm);
        }

        /// <summary>
        /// Resolves any strong name keys.
        /// </summary>
        /// <param name="targets"></param>
        /// <exception cref="FatalCompilerErrorException"></exception>
        void ResolveStrongNameKeys(List<ImportContext> targets)
        {
            foreach (var options in targets)
            {
                if (options.keyfile != null && options.keycontainer != null)
                    throw new FatalCompilerErrorException(DiagnosticEvent.CannotSpecifyBothKeyFileAndContainer());

                if (options.keyfile == null && options.keycontainer == null && options.delaysign)
                    throw new FatalCompilerErrorException(DiagnosticEvent.DelaySignRequiresKey());

                if (options.keyfile != null)
                {
                    if (options.delaysign)
                    {
                        var buf = ReadAllBytes(options.keyfile);
                        try
                        {
                            // maybe it is a key pair, if so we need to extract just the public key
                            buf = new StrongNameKeyPair(buf).PublicKey;
                        }
                        catch
                        {

                        }

                        options.publicKey = buf;
                    }
                    else
                    {
                        SetStrongNameKeyPair(ref options.keyPair, options.keyfile, null);
                    }
                }
                else if (options.keycontainer != null)
                {
                    StrongNameKeyPair keyPair = null;
                    SetStrongNameKeyPair(ref keyPair, null, options.keycontainer);
                    if (options.delaysign)
                        options.publicKey = keyPair.PublicKey;
                    else
                        options.keyPair = keyPair;
                }
            }
        }

        static internal byte[] ReadAllBytes(FileInfo path)
        {
            try
            {
                return File.ReadAllBytes(path.FullName);
            }
            catch (Exception x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.ErrorReadingFile(path.ToString(), x.Message));
            }
        }

        static internal FileInfo GetFileInfo(string path)
        {
            try
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.Directory == null)
                {
                    // this happens with an incorrect unc path (e.g. "\\foo\bar")
                    throw new FatalCompilerErrorException(DiagnosticEvent.InvalidPath(path));
                }
                return fileInfo;
            }
            catch (ArgumentException)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.InvalidPath(path));
            }
            catch (NotSupportedException)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.InvalidPath(path));
            }
            catch (PathTooLongException)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.PathTooLong(path));
            }
            catch (UnauthorizedAccessException)
            {
                // this exception does not appear to be possible
                throw new FatalCompilerErrorException(DiagnosticEvent.InvalidPath(path));
            }
        }

        void ReadFiles(ImportContext import, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                if (import.defaultAssemblyName == null)
                {
                    try
                    {
                        import.defaultAssemblyName = new FileInfo(Path.GetFileName(fileName)).Name;
                    }
                    catch (ArgumentException)
                    {
                        // if the filename contains a wildcard (or any other invalid character), we ignore
                        // it as a potential default assembly name
                    }
                    catch (NotSupportedException)
                    {

                    }
                    catch (PathTooLongException)
                    {

                    }
                }

                string[] files = null;
                try
                {
                    var path = Path.GetDirectoryName(fileName);
                    files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(fileName));
                }
                catch
                {

                }


                if (files == null || files.Length == 0)
                {
                    diagnostics.InputFileNotFound(fileName);
                }
                else
                {
                    foreach (var f in files)
                    {
                        ProcessFile(import, null, f);
                    }
                }
            }
        }

        static internal bool TryParseVersion(string str, out Version version)
        {
            if (str.EndsWith(".*"))
            {
                str = str.Substring(0, str.Length - 1);
                int count = str.Split('.').Length;
                // NOTE this is the published algorithm for generating automatic build and revision numbers
                // (see AssemblyVersionAttribute constructor docs), but it turns out that the revision
                // number is off an hour (on my system)...
                var now = DateTime.Now;
                int seconds = (int)(now.TimeOfDay.TotalSeconds / 2);
                int days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
                if (count == 3)
                {
                    str += days + "." + seconds;
                }
                else if (count == 4)
                {
                    str += seconds;
                }
                else
                {
                    version = null;
                    return false;
                }
            }
            try
            {
                version = new Version(str);
                return version.Major <= 65535 && version.Minor <= 65535 && version.Build <= 65535 && version.Revision <= 65535;
            }
            catch (ArgumentException)
            {

            }
            catch (FormatException)
            {

            }
            catch (OverflowException)
            {

            }

            version = null;
            return false;
        }

        void SetStrongNameKeyPair(ref StrongNameKeyPair strongNameKeyPair, FileInfo keyFile, string keyContainer)
        {
            try
            {
                if (keyFile != null)
                    strongNameKeyPair = new StrongNameKeyPair(ReadAllBytes(keyFile));
                else
                    strongNameKeyPair = new StrongNameKeyPair(keyContainer);

                // FXBUG we explicitly try to access the public key force a check (the StrongNameKeyPair constructor doesn't validate the key)
                if (strongNameKeyPair.PublicKey != null)
                {

                }
            }
            catch (Exception e)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.InvalidStrongNameKeyPair(keyFile != null ? "file" : "container", e.Message));
            }
        }

        byte[] ReadFromZip(ZipArchiveEntry ze)
        {
            using MemoryStream ms = new MemoryStream();
            using Stream s = ze.Open();
            s.CopyTo(ms);
            return ms.ToArray();
        }

        bool EmitStubWarning(ImportContext import, byte[] buf)
        {
            IKVM.Runtime.ClassFile cf;

            try
            {
                cf = new IKVM.Runtime.ClassFile(runtime, diagnostics, IKVM.ByteCode.Decoding.ClassFile.Read(buf), "<unknown>", ClassFileParseOptions.None, null);
            }
            catch (ClassFormatError)
            {
                return false;
            }
            catch (ByteCodeException)
            {
                return false;
            }

            if (cf.IKVMAssemblyAttribute == null)
            {
                return false;
            }

            if (cf.IKVMAssemblyAttribute.StartsWith("[["))
            {
                var r = new Regex(@"\[([^\[\]]+)\]");
                var mc = r.Matches(cf.IKVMAssemblyAttribute);
                foreach (Match m in mc)
                {
                    import.legacyStubReferences[m.Groups[1].Value] = null;
                    diagnostics.StubsAreDeprecated(m.Groups[1].Value);
                }
            }
            else
            {
                import.legacyStubReferences[cf.IKVMAssemblyAttribute] = null;
                diagnostics.StubsAreDeprecated(cf.IKVMAssemblyAttribute);
            }

            return true;
        }

        bool IsExcludedOrStubLegacy(ImportContext import, ZipArchiveEntry ze, byte[] data)
        {
            if (ze.Name.EndsWith(".class", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var name = IKVM.Runtime.ClassFile.GetClassName(data, 0, data.Length, out var stub);
                    if (import.IsExcludedClass(name) || (stub && EmitStubWarning(import, data)))
                    {
                        // we use stubs to add references, but otherwise ignore them
                        return true;
                    }
                }
                catch (ClassFormatError)
                {

                }
            }

            return false;
        }

        void ProcessManifest(ImportContext import, ZipArchiveEntry ze)
        {
            if (import.manifestMainClass == null)
            {
                // read main class from manifest
                // TODO find out if we can use other information from manifest
                using Stream stream = ze.Open();
                using StreamReader rdr = new StreamReader(stream);
                string line;
                while ((line = rdr.ReadLine()) != null)
                {
                    if (line.StartsWith("Main-Class: "))
                    {
                        line = line.Substring(12);
                        string continuation;
                        while ((continuation = rdr.ReadLine()) != null
                            && continuation.StartsWith(" ", StringComparison.Ordinal))
                        {
                            line += continuation.Substring(1);
                        }

                        import.manifestMainClass = line.Replace('/', '.');
                        break;
                    }
                }
            }
        }

        bool ProcessZipFile(ImportContext import, string file, Predicate<ZipArchiveEntry> filter)
        {
            try
            {
                using var zf = ZipFile.OpenRead(file);

                bool found = false;
                Jar jar = null;
                foreach (var ze in zf.Entries)
                {
                    if (filter != null && !filter(ze))
                    {
                        // skip
                    }
                    else
                    {
                        found = true;
                        var data = ReadFromZip(ze);
                        if (IsExcludedOrStubLegacy(import, ze, data))
                        {
                            continue;
                        }
                        if (jar == null)
                        {
                            jar = import.GetJar(file);
                        }
                        jar.Add(ze.FullName, data);
                        if (string.Equals(ze.FullName, "META-INF/MANIFEST.MF", StringComparison.OrdinalIgnoreCase))
                        {
                            ProcessManifest(import, ze);
                        }
                    }
                }

                // include empty zip file
                if (!found)
                {
                    import.GetJar(file);
                }

                return found;
            }
            catch (InvalidDataException x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.ErrorReadingFile(file, x.Message));
            }
        }

        void ProcessFile(ImportContext import, DirectoryInfo baseDir, string file)
        {
            var fileInfo = GetFileInfo(file);
            if (fileInfo.Extension.Equals(".jar", StringComparison.OrdinalIgnoreCase) || fileInfo.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                ProcessZipFile(import, file, null);
            }
            else
            {
                if (fileInfo.Extension.Equals(".class", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] data = ReadAllBytes(fileInfo);
                    try
                    {
                        var name = IKVM.Runtime.ClassFile.GetClassName(data, 0, data.Length, out var stub);
                        if (import.IsExcludedClass(name))
                            return;

                        // we use stubs to add references, but otherwise ignore them
                        if (stub && EmitStubWarning(import, data))
                            return;

                        import.GetClassesJar().Add(name.Replace('.', '/') + ".class", data, fileInfo);
                        return;
                    }
                    catch (ClassFormatError x)
                    {
                        diagnostics.ClassFormatError(file, x.Message);
                    }
                }

                if (baseDir == null)
                {
                    diagnostics.UnknownFileType(file);
                }
                else
                {
                    // include as resource
                    // extract the resource name by chopping off the base directory
                    var name = file.Substring(baseDir.FullName.Length);
                    name = name.TrimStart(Path.DirectorySeparatorChar).Replace('\\', '/');
                    import.GetResourcesJar().Add(name, ReadAllBytes(fileInfo), fileInfo);
                }
            }
        }

        bool Recurse(ImportContext import, DirectoryInfo baseDir, DirectoryInfo dir, string spec)
        {
            bool found = false;

            foreach (var file in dir.GetFiles(spec))
            {
                found = true;
                ProcessFile(import, baseDir, file.FullName);
            }

            foreach (var sub in dir.GetDirectories())
            {
                found |= Recurse(import, baseDir, sub, spec);
            }

            return found;
        }

        bool RecurseJar(ImportContext import, string path)
        {
            var file = "";
            for (; ; )
            {
                file = Path.Combine(Path.GetFileName(path), file);
                path = Path.GetDirectoryName(path);
                if (Directory.Exists(path))
                {
                    throw new DirectoryNotFoundException();
                }
                else if (File.Exists(path))
                {
                    var pathFilter = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar;
                    var fileFilter = "^" + Regex.Escape(Path.GetFileName(file)).Replace("\\*", ".*").Replace("\\?", ".") + "$";

                    return ProcessZipFile(import, path, delegate (ZipArchiveEntry ze)
                    {
                        // MONOBUG Path.GetDirectoryName() doesn't normalize / to \ on Windows
                        var name = ze.FullName.Replace('/', Path.DirectorySeparatorChar);
                        return (Path.GetDirectoryName(name) + Path.DirectorySeparatorChar).StartsWith(pathFilter) && Regex.IsMatch(Path.GetFileName(ze.FullName), fileFilter);
                    });
                }
            }
        }

        //This processes an exclusion file with a single regular expression per line
        void ProcessExclusionFile(ref string[] classesToExclude, string filename)
        {
            try
            {
                var list = classesToExclude == null ? new List<string>() : new List<string>(classesToExclude);
                using (var file = new StreamReader(filename))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (!line.StartsWith("//") && line.Length != 0)
                        {
                            list.Add(line);
                        }
                    }
                }

                classesToExclude = list.ToArray();
            }
            catch (Exception x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.ErrorReadingFile(filename, x.Message));
            }
        }

        void ProcessAttributeAnnotationsClass(List<object> annotations, string filename)
        {
            try
            {
                using var file = File.OpenRead(filename);
                var cf = new IKVM.Runtime.ClassFile(runtime, diagnostics, IKVM.ByteCode.Decoding.ClassFile.Read(file), null, ClassFileParseOptions.None, null);
                annotations.AddRange(cf.Annotations);
            }
            catch (Exception x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.ErrorReadingFile(filename, x.Message));
            }
        }

        void HandleWarnArg(ICollection<string> target, string arg)
        {
            foreach (var w in arg.Split(','))
            {
                // Strip IKVM prefix
                int prefixStart = w.StartsWith("IKVM", StringComparison.OrdinalIgnoreCase) ? 4 : 0;
                int contextIndex = w.IndexOf(':', prefixStart);
                string context = string.Empty;
                string parse;
                if (contextIndex != -1)
                {
                    // context includes ':' separator
                    context = w.Substring(contextIndex);
                    parse = w.Substring(prefixStart, contextIndex - prefixStart);
                }
                else
                {
                    parse = w.Substring(prefixStart);
                }

                if (!int.TryParse(parse, out var intResult))
                {
                    // NamedResults aren't supported
                    continue; // silently continue
                }

                target.Add($"{intResult}{context}");
            }
        }

    }

}
