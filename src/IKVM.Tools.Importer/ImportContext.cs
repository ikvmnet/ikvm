/*
  Copyright (C) 2002-2014 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
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
using IKVM.Reflection.Emit;
using IKVM.Runtime;
using IKVM.Tools.Core.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Represents the context of an import operation outputing a single assembly.
    /// </summary>
    class ImportContext
    {

        /// <summary>
        /// <see cref="IDiagnosticHandler"/> for the importer.
        /// </summary>
        class CompilerOptionsDiagnosticHandler : FormattedDiagnosticHandler
        {

            readonly ImportState _options;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="options"></param>
            /// <param name="spec"></param>
            /// <param name="formatters"></param>
            public CompilerOptionsDiagnosticHandler(ImportState options, string spec, DiagnosticFormatterProvider formatters) :
                base(spec, formatters)
            {
                _options = options ?? throw new ArgumentNullException(nameof(options));
            }

            /// <inheritdoc />
            public override bool IsEnabled(Diagnostic diagnostic)
            {
                return diagnostic.Level is not DiagnosticLevel.Trace and not DiagnosticLevel.Info;
            }

            /// <inheritdoc />
            public override void Report(in DiagnosticEvent @event)
            {
                if (IsEnabled(@event.Diagnostic) == false)
                    return;

                var key = @event.Diagnostic.Id.ToString();
                for (int i = 0; ; i++)
                {
                    if (_options.suppressWarnings.Contains(key))
                        return;

                    if (i == @event.Args.Length)
                        break;

                    key += ":" + @event.Args[i];
                }

                _options.suppressWarnings.Add(key);

                base.Report(@event);
            }

        }

        readonly ImportOptions _options;
        string manifestMainClass;
        string defaultAssemblyName;
        static bool time;
        static string runtimeAssembly;
        static bool nostdlib;
        static bool nonDeterministicOutput;
        static DebugMode debugMode;
        static readonly List<string> libpaths = new List<string>();
        internal static readonly AssemblyResolver resolver = new AssemblyResolver();

        public static int Execute(ImportOptions options)
        {
            DateTime start = DateTime.Now;
            Thread.CurrentThread.Name = "compiler";

            try
            {
                try
                {
                    return Compile(options);
                }
                catch (TypeInitializationException x)
                {
                    if (x.InnerException is FatalCompilerErrorException)
                        throw x.InnerException;

                    throw;
                }
            }
            catch (FatalCompilerErrorException x)
            {
                Console.Error.WriteLine(x.Message);
                return 1;
            }
            catch (Exception x)
            {
                Console.Error.WriteLine();
                Console.Error.WriteLine("*** COMPILER ERROR ***");
                Console.Error.WriteLine();
                Console.Error.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().FullName);
                Console.Error.WriteLine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
                Console.Error.WriteLine("{0} {1}-bit", Environment.Version, IntPtr.Size * 8);
                Console.Error.WriteLine();
                Console.Error.WriteLine(x);
                return 2;
            }
            finally
            {
                if (time)
                {
                    Console.WriteLine("Total cpu time: {0}", System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime);
                    Console.WriteLine("User cpu time: {0}", System.Diagnostics.Process.GetCurrentProcess().UserProcessorTime);
                    Console.WriteLine("Total wall clock time: {0}", DateTime.Now - start);
                    Console.WriteLine("Peak virtual memory: {0}", System.Diagnostics.Process.GetCurrentProcess().PeakVirtualMemorySize64);
                    for (int i = 0; i <= GC.MaxGeneration; i++)
                    {
                        Console.WriteLine("GC({0}) count: {1}", i, GC.CollectionCount(i));
                    }
                }
            }
        }

        /// <summary>
        /// Generates a diagnostic instance from the diagnostics options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        static IDiagnosticHandler GetDiagnostics(IServiceProvider services, ImportState options, string spec)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrWhiteSpace(spec))
                throw new ArgumentException($"'{nameof(spec)}' cannot be null or whitespace.", nameof(spec));

            return ActivatorUtilities.CreateInstance<CompilerOptionsDiagnosticHandler>(services, options, spec);
        }

        static int Compile(ImportOptions options)
        {
            var rootTarget = new ImportState();
            var services = new ServiceCollection();
            services.AddToolsDiagnostics();
            services.AddSingleton(p => GetDiagnostics(p, rootTarget, options.Log));
            services.AddSingleton<IManagedTypeResolver, ManagedResolver>();
            services.AddSingleton<StaticCompiler>();
            using var provider = services.BuildServiceProvider();

            var diagnostics = provider.GetRequiredService<IDiagnosticHandler>();
            var compiler = provider.GetRequiredService<StaticCompiler>();
            var targets = new List<ImportState>();
            var context = new RuntimeContext(new RuntimeContextOptions(), diagnostics, provider.GetRequiredService<IManagedTypeResolver>(), options.Bootstrap, compiler);

            compiler.rootTarget = rootTarget;
            var importer = new ImportContext();
            importer.ParseCommandLine(context, compiler, diagnostics, options, targets, rootTarget);
            compiler.Init(nonDeterministicOutput, rootTarget.debugMode, libpaths);
            resolver.Warning += (warning, message, parameters) => loader_Warning(compiler, diagnostics, warning, message, parameters);
            resolver.Init(compiler.Universe, nostdlib, rootTarget.unresolvedReferences, libpaths);
            ResolveReferences(compiler, diagnostics, targets);
            ResolveStrongNameKeys(targets);

            if (targets.Count == 0)
                throw new FatalCompilerErrorException(DiagnosticEvent.NoTargetsFound());

            if (compiler.errorCount != 0)
                return 1;

            try
            {
                return ImportClassLoader.Compile(importer, context, compiler, diagnostics, runtimeAssembly, targets);
            }
            catch (FileFormatLimitationExceededException x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.FileFormatLimitationExceeded(x.Message));
            }
        }

        static void loader_Warning(StaticCompiler compiler, IDiagnosticHandler diagnostics, AssemblyResolver.WarningId warning, string message, string[] parameters)
        {
            switch (warning)
            {
                case AssemblyResolver.WarningId.HigherVersion:
                    diagnostics.AssumeAssemblyVersionMatch(parameters[0], parameters[1]);
                    break;
                case AssemblyResolver.WarningId.InvalidLibDirectoryOption:
                    diagnostics.InvalidDirectoryInLibOptionPath(parameters[0]);
                    break;
                case AssemblyResolver.WarningId.InvalidLibDirectoryEnvironment:
                    diagnostics.InvalidDirectoryInLibEnvironmentPath(parameters[0]);
                    break;
                case AssemblyResolver.WarningId.LegacySearchRule:
                    diagnostics.LegacySearchRule(parameters[0]);
                    break;
                case AssemblyResolver.WarningId.LocationIgnored:
                    diagnostics.AssemblyLocationIgnored(parameters[0], parameters[1], parameters[2]);
                    break;
                default:
                    diagnostics.UnknownWarning(string.Format(message, parameters));
                    break;
            }
        }

        static void ResolveStrongNameKeys(List<ImportState> targets)
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

        internal static byte[] ReadAllBytes(FileInfo path)
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

        void ParseCommandLine(RuntimeContext context, StaticCompiler compiler, IDiagnosticHandler diagnostics, ImportOptions options, List<ImportState> targets, ImportState compilerOptions)
        {
            compilerOptions.target = PEFileKinds.ConsoleApplication;
            compilerOptions.guessFileKind = true;
            compilerOptions.version = new Version(0, 0, 0, 0);
            compilerOptions.apartment = ApartmentState.STA;
            compilerOptions.props = new Dictionary<string, string>();
            ContinueParseCommandLine(context, compiler, diagnostics, options, targets, compilerOptions);
        }

        void ContinueParseCommandLine(RuntimeContext context, StaticCompiler compiler, IDiagnosticHandler diagnostics, ImportOptions options, List<ImportState> targets, ImportState compilerOptions)
        {
            if (options.Output != null)
                compilerOptions.path = options.Output;

            if (options.AssemblyName != null)
                compilerOptions.assembly = options.AssemblyName;

            switch (options.Target)
            {
                case ImportTarget.Exe:
                    compilerOptions.target = PEFileKinds.ConsoleApplication;
                    compilerOptions.guessFileKind = false;
                    break;
                case ImportTarget.WinExe:
                    compilerOptions.target = PEFileKinds.WindowApplication;
                    compilerOptions.guessFileKind = false;
                    break;
                case ImportTarget.Module:
                    compilerOptions.targetIsModule = true;
                    compilerOptions.target = PEFileKinds.Dll;
                    compilerOptions.guessFileKind = false;
                    nonDeterministicOutput = true;
                    break;
                case ImportTarget.Library:
                    compilerOptions.target = PEFileKinds.Dll;
                    compilerOptions.guessFileKind = false;
                    break;
                default:
                    throw new FatalCompilerErrorException(DiagnosticEvent.UnrecognizedTargetType(options.Target.ToString()));
            }

            switch (options.Platform)
            {
                case ImportPlatform.X86:
                    compilerOptions.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit;
                    compilerOptions.imageFileMachine = ImageFileMachine.I386;
                    break;
                case ImportPlatform.X64:
                    compilerOptions.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus;
                    compilerOptions.imageFileMachine = ImageFileMachine.AMD64;
                    break;
                case ImportPlatform.ARM:
                    compilerOptions.pekind = PortableExecutableKinds.ILOnly;
                    compilerOptions.imageFileMachine = ImageFileMachine.ARM;
                    break;
                case ImportPlatform.ARM64:
                    compilerOptions.pekind = PortableExecutableKinds.ILOnly;
                    compilerOptions.imageFileMachine = ImageFileMachine.ARM64;
                    break;
                case ImportPlatform.AnyCpu32BitPreferred:
                    compilerOptions.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Preferred32Bit;
                    compilerOptions.imageFileMachine = ImageFileMachine.UNKNOWN;
                    break;
                case ImportPlatform.AnyCpu:
                    compilerOptions.pekind = PortableExecutableKinds.ILOnly;
                    compilerOptions.imageFileMachine = ImageFileMachine.UNKNOWN;
                    break;
                default:
                    throw new FatalCompilerErrorException(DiagnosticEvent.UnrecognizedPlatform(options.Platform.ToString()));
            }

            switch (options.Apartment)
            {
                case ImportApartment.STA:
                    compilerOptions.apartment = ApartmentState.STA;
                    break;
                case ImportApartment.MTA:
                    compilerOptions.apartment = ApartmentState.MTA;
                    break;
                case ImportApartment.None:
                    compilerOptions.apartment = ApartmentState.Unknown;
                    break;
                default:
                    throw new FatalCompilerErrorException(DiagnosticEvent.UnrecognizedApartment(options.Apartment.ToString()));
            }

            if (options.NoGlobbing)
                compilerOptions.noglobbing = true;

            if (options.Properties.Count > 0)
                foreach (var kvp in options.Properties)
                    compilerOptions.props[kvp.Key] = kvp.Value;

            if (options.EnableAssertions != null)
            {
                if (options.EnableAssertions.Length == 0)
                    compilerOptions.props["ikvm.assert.default"] = "true";
                else
                    compilerOptions.props["ikvm.assert.enable"] = string.Join(";", options.EnableAssertions);
            }

            if (options.DisableAssertions != null)
            {
                if (options.DisableAssertions.Length == 0)
                    compilerOptions.props["ikvm.assert.default"] = "false";
                else
                    compilerOptions.props["ikvm.assert.disable"] = string.Join(";", options.DisableAssertions);
            }

            if (options.RemoveAssertions)
                compilerOptions.codegenoptions |= CodeGenOptions.RemoveAsserts;

            if (options.Main != null)
                compilerOptions.mainClass = options.Main;

            foreach (var reference in options.References)
                ArrayAppend(ref compilerOptions.unresolvedReferences, reference);

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
                    found = Recurse(context, compiler, compilerOptions, diagnostics, dir, dir, "*");
                }
                else
                {
                    try
                    {
                        var dir = new DirectoryInfo(Path.GetDirectoryName(spec));
                        if (dir.Exists)
                        {
                            found = Recurse(context, compiler, compilerOptions, diagnostics, dir, dir, Path.GetFileName(spec));
                        }
                        else
                        {
                            found = RecurseJar(context, compiler, compilerOptions, diagnostics, spec);
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
                compilerOptions.GetResourcesJar().Add(fileName, ReadAllBytes(fileInfo), fileInfo);
            }

            foreach (var kvp in options.ExternalResources)
            {
                if (!File.Exists(kvp.Value.FullName))
                    throw new FatalCompilerErrorException(DiagnosticEvent.ExternalResourceNotFound(kvp.Value.FullName));
                if (Path.GetFileName(kvp.Value.FullName) != kvp.Value.FullName)
                    throw new FatalCompilerErrorException(DiagnosticEvent.ExternalResourceNameInvalid(kvp.Value.FullName));

                // TODO resource name clashes should be tested
                compilerOptions.externalResources ??= new Dictionary<string, string>();
                compilerOptions.externalResources.Add(kvp.Key, kvp.Value.FullName);
            }

            if (options.NoJNI)
                compilerOptions.codegenoptions |= CodeGenOptions.NoJNI;

            if (options.Exclude != null)
                ProcessExclusionFile(ref compilerOptions.classesToExclude, options.Exclude.FullName);

            if (options.Version != null)
                compilerOptions.version = options.Version;

            if (options.FileVersion != null)
                compilerOptions.fileversion = options.FileVersion.ToString();

            if (options.Win32Icon != null)
            {
                compilerOptions.iconfile = GetFileInfo(options.Win32Icon.FullName);
            }

            if (options.Win32Manifest != null)
                compilerOptions.manifestFile = GetFileInfo(options.Win32Manifest.FullName);

            if (options.KeyFile != null)
                compilerOptions.keyfile = GetFileInfo(options.KeyFile.FullName);

            if (options.Key != null)
                compilerOptions.keycontainer = options.Key;

            if (options.DelaySign)
                compilerOptions.delaysign = true;

            switch (options.Debug)
            {
                case ImportDebug.Full:
                    compilerOptions.codegenoptions |= CodeGenOptions.EmitSymbols;
                    compilerOptions.debugMode = DebugMode.Full;
                    break;
                case ImportDebug.Portable:
                    compilerOptions.codegenoptions |= CodeGenOptions.EmitSymbols;
                    compilerOptions.debugMode = DebugMode.Portable;
                    break;
                case ImportDebug.Embedded:
                    compilerOptions.codegenoptions |= CodeGenOptions.EmitSymbols;
                    compilerOptions.debugMode = DebugMode.Embedded;
                    break;
            }

            if (options.Deterministic == false)
                nonDeterministicOutput = true;

            if (options.Optimize == false)
                compilerOptions.codegenoptions |= CodeGenOptions.DisableOptimizations;

            if (options.SourcePath != null)
                compilerOptions.sourcepath = options.SourcePath.FullName;

            if (options.Remap != null)
                compilerOptions.remapfile = GetFileInfo(options.Remap.FullName);

            if (options.NoStackTraceInfo)
                compilerOptions.codegenoptions |= CodeGenOptions.NoStackTraceInfo;

            if (options.RemoveUnusedPrivateFields)
                compilerOptions.codegenoptions |= CodeGenOptions.RemoveUnusedFields;

            if (options.CompressResources)
                compilerOptions.compressedResources = true;

            if (options.StrictFinalFieldSemantics)
                compilerOptions.codegenoptions |= CodeGenOptions.StrictFinalFieldSemantics;

            if (options.PrivatePackages != null)
                foreach (var prefix in options.PrivatePackages)
                    ArrayAppend(ref compilerOptions.privatePackages, prefix);

            if (options.PublicPackages != null)
                foreach (var prefix in options.PublicPackages)
                    ArrayAppend(ref compilerOptions.publicPackages, prefix);

            if (options.NoWarn != null)
                foreach (var diagnostic in options.NoWarn)
                    compilerOptions.suppressWarnings.Add($"IKVM{diagnostic.Id:D4}");

            // TODO handle specific diagnostic IDs
            if (options.WarnAsError != null)
            {
                if (options.WarnAsError.Length == 0)
                    compilerOptions.warnaserror = true;
                else
                    foreach (var i in options.WarnAsError)
                        compilerOptions.errorWarnings.Add($"IKVM{i.Id:D4}");
            }

            if (options.Runtime != null)
                runtimeAssembly = options.Runtime.FullName;

            if (options.Time)
                time = true;

            if (options.ClassLoader != null)
                compilerOptions.classLoader = options.ClassLoader;

            if (options.SharedClassLoader)
                compilerOptions.sharedclassloader ??= new List<ImportClassLoader>();

            if (options.BaseAddress != null)
            {
                var baseAddress = options.BaseAddress;
                ulong baseAddressParsed;
                if (baseAddress.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    baseAddressParsed = ulong.Parse(baseAddress.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
                else
                    baseAddressParsed = ulong.Parse(baseAddress); // note that unlike CSC we don't support octal

                compilerOptions.baseAddress = (baseAddressParsed & 0xFFFFFFFFFFFF0000UL);
            }

            if (options.FileAlign != null)
            {
                if (!uint.TryParse(options.FileAlign, out var filealign) || filealign < 512 || filealign > 8192 || (filealign & (filealign - 1)) != 0)
                    throw new FatalCompilerErrorException(DiagnosticEvent.InvalidFileAlignment(options.FileAlign));

                compilerOptions.fileAlignment = filealign;
            }

            if (options.NoPeerCrossReference)
                compilerOptions.crossReferenceAllPeers = false;

            if (options.NoStdLib)
                nostdlib = true;

            if (options.Libraries != null)
                foreach (var lib in options.Libraries)
                    libpaths.Add(lib.FullName);

            if (options.NoAutoSerialization)
                compilerOptions.codegenoptions |= CodeGenOptions.NoAutomagicSerialization;

            if (options.HighEntropyVA)
            {
                compilerOptions.highentropyva = true;
            }

            if (options.Proxies != null)
            {
                foreach (var proxy in options.Proxies)
                {
                    if (compilerOptions.proxies.Contains(proxy))
                        diagnostics.DuplicateProxy(proxy);

                    compilerOptions.proxies.Add(proxy);
                }
            }

            if (options.AllowNonVirtualCalls)
                JVM.AllowNonVirtualCalls = true;

            if (options.Static)
            {
                // we abuse -static to also enable support for NoRefEmit scenarios
                compilerOptions.codegenoptions |= CodeGenOptions.DisableDynamicBinding | CodeGenOptions.NoRefEmitHelpers;
            }

            if (options.NoJarStubs)    // undocumented temporary option to mitigate risk
            {
                compilerOptions.nojarstubs = true;
            }

            if (options.AssemblyAttributes != null)
                foreach (var i in options.AssemblyAttributes)
                    ProcessAttributeAnnotationsClass(context, diagnostics, ref compilerOptions.assemblyAttributeAnnotations, i.FullName);

            if (options.WarningLevel4Option) // undocumented option to always warn if a class isn't found
                compilerOptions.warningLevelHigh = true;

            if (options.NoParameterReflection) // undocumented option to compile core class libraries with, to disable MethodParameter attribute
                compilerOptions.noParameterReflection = true;

            if (options.Bootstrap)
                compilerOptions.bootstrap = true;

            if (compilerOptions.targetIsModule && compilerOptions.sharedclassloader != null)
                throw new FatalCompilerErrorException(DiagnosticEvent.SharedClassLoaderCannotBeUsedOnModuleTarget());

            ReadFiles(context, compiler, compilerOptions, diagnostics, options.Inputs.Select(i => i.FullName).ToList());

            foreach (var nested in options.Nested)
            {
                var nestedLevel = new ImportContext();
                nestedLevel.manifestMainClass = manifestMainClass;
                nestedLevel.defaultAssemblyName = defaultAssemblyName;
                nestedLevel.ContinueParseCommandLine(context, compiler, diagnostics, nested, targets, compilerOptions.Copy());
            }

            if (compilerOptions.assembly == null)
            {
                var basename = compilerOptions.path == null ? defaultAssemblyName : compilerOptions.path.Name;
                if (basename == null)
                    throw new FatalCompilerErrorException(DiagnosticEvent.NoOutputFileSpecified());

                int idx = basename.LastIndexOf('.');
                if (idx > 0)
                    compilerOptions.assembly = basename.Substring(0, idx);
                else
                    compilerOptions.assembly = basename;
            }

            if (compilerOptions.path != null && compilerOptions.guessFileKind)
            {
                if (compilerOptions.path.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                    compilerOptions.target = PEFileKinds.Dll;

                compilerOptions.guessFileKind = false;
            }

            if (compilerOptions.mainClass == null && manifestMainClass != null && (compilerOptions.guessFileKind || compilerOptions.target != PEFileKinds.Dll))
            {
                diagnostics.MainMethodFromManifest(manifestMainClass);
                compilerOptions.mainClass = manifestMainClass;
            }

            // schedule run if leaf-node
            if (options.Nested == null || options.Nested.Length == 0)
                targets.Add(compilerOptions);
        }

        internal static FileInfo GetFileInfo(string path)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
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

        void ReadFiles(RuntimeContext context, StaticCompiler compiler, ImportState options, IDiagnosticHandler diagnostics, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                if (defaultAssemblyName == null)
                {
                    try
                    {
                        defaultAssemblyName = new FileInfo(Path.GetFileName(fileName)).Name;
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
                        ProcessFile(context, compiler, options, diagnostics, null, f);
                    }
                }
            }
        }

        internal static bool TryParseVersion(string str, out Version version)
        {
            if (str.EndsWith(".*"))
            {
                str = str.Substring(0, str.Length - 1);
                int count = str.Split('.').Length;
                // NOTE this is the published algorithm for generating automatic build and revision numbers
                // (see AssemblyVersionAttribute constructor docs), but it turns out that the revision
                // number is off an hour (on my system)...
                DateTime now = DateTime.Now;
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
            catch (ArgumentException) { }
            catch (FormatException) { }
            catch (OverflowException) { }
            version = null;
            return false;
        }

        static void SetStrongNameKeyPair(ref StrongNameKeyPair strongNameKeyPair, FileInfo keyFile, string keyContainer)
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
            catch (Exception x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.InvalidStrongNameKeyPair(keyFile != null ? "file" : "container", x.Message));
            }
        }

        static void ResolveReferences(StaticCompiler compiler, IDiagnosticHandler diagnostics, List<ImportState> targets)
        {
            var cache = new Dictionary<string, IKVM.Reflection.Assembly>();

            foreach (var target in targets)
            {
                if (target.unresolvedReferences != null)
                {
                    foreach (string reference in target.unresolvedReferences)
                    {
                        foreach (var peer in targets)
                        {
                            if (peer.assembly.Equals(reference, StringComparison.OrdinalIgnoreCase))
                            {
                                ArrayAppend(ref target.peerReferences, peer.assembly);
                                goto next_reference;
                            }
                        }
                        if (!resolver.ResolveReference(cache, ref target.references, reference))
                        {
                            throw new FatalCompilerErrorException(DiagnosticEvent.ReferenceNotFound(reference));
                        }
                    next_reference:;
                    }
                }
            }

            // verify that we didn't reference any secondary assemblies of a shared class loader group
            foreach (var target in targets)
            {
                if (target.references != null)
                {
                    foreach (var asm in target.references)
                    {
                        var forwarder = asm.GetType("__<MainAssembly>");
                        if (forwarder != null && forwarder.Assembly != asm)
                            diagnostics.NonPrimaryAssemblyReference(asm.Location, forwarder.Assembly.GetName().Name);
                    }
                }
            }

            // add legacy references (from stub files)
            foreach (var target in targets)
                foreach (var assemblyName in target.legacyStubReferences.Keys)
                    ArrayAppend(ref target.references, resolver.LegacyLoad(new AssemblyName(assemblyName), null));

            // now pre-load the secondary assemblies of any shared class loader groups
            foreach (var target in targets)
                if (target.references != null)
                    foreach (var asm in target.references)
                        RuntimeAssemblyClassLoader.PreloadExportedAssemblies(compiler, asm);
        }

        private static void ArrayAppend<T>(ref T[] array, T element)
        {
            if (array == null)
                array = [element];
            else
                array = ArrayUtil.Concat(array, element);
        }

        private static void ArrayAppend<T>(ref T[] array, T[] append)
        {
            if (array == null)
            {
                array = append;
            }
            else if (append != null)
            {
                T[] tmp = new T[array.Length + append.Length];
                Array.Copy(array, tmp, array.Length);
                Array.Copy(append, 0, tmp, array.Length, append.Length);
                array = tmp;
            }
        }

        static byte[] ReadFromZip(ZipArchiveEntry ze)
        {
            using MemoryStream ms = new MemoryStream();
            using Stream s = ze.Open();
            s.CopyTo(ms);
            return ms.ToArray();
        }

        static bool EmitStubWarning(RuntimeContext context, StaticCompiler compiler, ImportState options, IDiagnosticHandler diagnostics, byte[] buf)
        {
            IKVM.Runtime.ClassFile cf;

            try
            {
                cf = new IKVM.Runtime.ClassFile(context, diagnostics, IKVM.ByteCode.Decoding.ClassFile.Read(buf), "<unknown>", ClassFileParseOptions.None, null);
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
                    options.legacyStubReferences[m.Groups[1].Value] = null;
                    diagnostics.StubsAreDeprecated(m.Groups[1].Value);
                }
            }
            else
            {
                options.legacyStubReferences[cf.IKVMAssemblyAttribute] = null;
                diagnostics.StubsAreDeprecated(cf.IKVMAssemblyAttribute);
            }

            return true;
        }

        static bool IsExcludedOrStubLegacy(RuntimeContext context, StaticCompiler compiler, ImportState options, IDiagnosticHandler diagnostics, ZipArchiveEntry ze, byte[] data)
        {
            if (ze.Name.EndsWith(".class", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var name = IKVM.Runtime.ClassFile.GetClassName(data, 0, data.Length, out var stub);
                    if (options.IsExcludedClass(name) || (stub && EmitStubWarning(context, compiler, options, diagnostics, data)))
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

        void ProcessManifest(StaticCompiler compiler, ImportState options, ZipArchiveEntry ze)
        {
            if (manifestMainClass == null)
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
                        manifestMainClass = line.Replace('/', '.');
                        break;
                    }
                }
            }
        }

        bool ProcessZipFile(RuntimeContext context, StaticCompiler compiler, ImportState options, IDiagnosticHandler diagnostics, string file, Predicate<ZipArchiveEntry> filter)
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
                        if (IsExcludedOrStubLegacy(context, compiler, options, diagnostics, ze, data))
                        {
                            continue;
                        }
                        if (jar == null)
                        {
                            jar = options.GetJar(file);
                        }
                        jar.Add(ze.FullName, data);
                        if (string.Equals(ze.FullName, "META-INF/MANIFEST.MF", StringComparison.OrdinalIgnoreCase))
                        {
                            ProcessManifest(compiler, options, ze);
                        }
                    }
                }

                // include empty zip file
                if (!found)
                {
                    options.GetJar(file);
                }

                return found;
            }
            catch (InvalidDataException x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.ErrorReadingFile(file, x.Message));
            }
        }

        void ProcessFile(RuntimeContext context, StaticCompiler compiler, ImportState options, IDiagnosticHandler diagnostics, DirectoryInfo baseDir, string file)
        {
            var fileInfo = GetFileInfo(file);
            if (fileInfo.Extension.Equals(".jar", StringComparison.OrdinalIgnoreCase) || fileInfo.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                ProcessZipFile(context, compiler, options, diagnostics, file, null);
            }
            else
            {
                if (fileInfo.Extension.Equals(".class", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] data = ReadAllBytes(fileInfo);
                    try
                    {
                        var name = IKVM.Runtime.ClassFile.GetClassName(data, 0, data.Length, out var stub);
                        if (options.IsExcludedClass(name))
                            return;

                        // we use stubs to add references, but otherwise ignore them
                        if (stub && EmitStubWarning(context, compiler, options, diagnostics, data))
                            return;

                        options.GetClassesJar().Add(name.Replace('.', '/') + ".class", data, fileInfo);
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
                    options.GetResourcesJar().Add(name, ReadAllBytes(fileInfo), fileInfo);
                }
            }
        }

        bool Recurse(RuntimeContext context, StaticCompiler compiler, ImportState options, IDiagnosticHandler diagnostics, DirectoryInfo baseDir, DirectoryInfo dir, string spec)
        {
            bool found = false;

            foreach (var file in dir.GetFiles(spec))
            {
                found = true;
                ProcessFile(context, compiler, options, diagnostics, baseDir, file.FullName);
            }

            foreach (var sub in dir.GetDirectories())
            {
                found |= Recurse(context, compiler, options, diagnostics, baseDir, sub, spec);
            }

            return found;
        }

        bool RecurseJar(RuntimeContext context, StaticCompiler compiler, ImportState options, IDiagnosticHandler diagnostics, string path)
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

                    return ProcessZipFile(context, compiler, options, diagnostics, path, delegate (ZipArchiveEntry ze)
                    {
                        // MONOBUG Path.GetDirectoryName() doesn't normalize / to \ on Windows
                        var name = ze.FullName.Replace('/', Path.DirectorySeparatorChar);
                        return (Path.GetDirectoryName(name) + Path.DirectorySeparatorChar).StartsWith(pathFilter) && Regex.IsMatch(Path.GetFileName(ze.FullName), fileFilter);
                    });
                }
            }
        }

        //This processes an exclusion file with a single regular expression per line
        private static void ProcessExclusionFile(ref string[] classesToExclude, string filename)
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

        static void ProcessAttributeAnnotationsClass(RuntimeContext context, IDiagnosticHandler diagnostics, ref object[] annotations, string filename)
        {
            try
            {
                using var file = File.OpenRead(filename);
                var cf = new IKVM.Runtime.ClassFile(context, diagnostics, IKVM.ByteCode.Decoding.ClassFile.Read(file), null, ClassFileParseOptions.None, null);
                ArrayAppend(ref annotations, cf.Annotations);
            }
            catch (Exception x)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.ErrorReadingFile(filename, x.Message));
            }
        }

        internal static void HandleWarnArg(ICollection<string> target, string arg)
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
