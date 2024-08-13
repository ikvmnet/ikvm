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
using System.Text.RegularExpressions;
using System.Threading;

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    class IkvmImporterInternal
    {

        bool nonleaf;
        string manifestMainClass;
        string defaultAssemblyName;
        static bool time;
        static string runtimeAssembly;
        static bool nostdlib;
        static bool nonDeterministicOutput;
        static DebugMode debugMode;
        static readonly List<string> libpaths = new List<string>();
        internal static readonly AssemblyResolver resolver = new AssemblyResolver();

        static void AddArg(List<string> arglist, string s, int depth)
        {
            if (s.StartsWith("@"))
            {
                if (depth++ > 16)
                {
                    throw new FatalCompilerErrorException(Message.ResponseFileDepthExceeded);
                }
                try
                {
                    using (StreamReader sr = new StreamReader(s.Substring(1)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string arg = line.Trim();
                            if (arg != "" && !arg.StartsWith("#"))
                            {
                                AddArg(arglist, arg, depth);
                            }
                        }
                    }
                }
                catch (FatalCompilerErrorException)
                {
                    throw;
                }
                catch (Exception x)
                {
                    throw new FatalCompilerErrorException(Message.ErrorReadingFile, s.Substring(1), x.Message);
                }
            }
            else
            {
                arglist.Add(s);
            }
        }

        static List<string> GetArgs(string[] args)
        {
            var arglist = new List<string>();
            foreach (string s in args)
                AddArg(arglist, s, 0);
            return arglist;
        }

        public static int Execute(string[] args)
        {
            DateTime start = DateTime.Now;
            System.Threading.Thread.CurrentThread.Name = "compiler";
            Tracer.EnableTraceConsoleListener();
            Tracer.EnableTraceForDebug();

            try
            {
                try
                {
                    return Compile(args);
                }
                catch (TypeInitializationException x)
                {
                    if (x.InnerException is FatalCompilerErrorException)
                    {
                        throw x.InnerException;
                    }
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

        static int Compile(string[] args)
        {
            var argList = GetArgs(args);
            if (argList.Count == 0 || argList.Contains("-?") || argList.Contains("-help"))
            {
                PrintHelp();
                return 0;
            }

            if (!argList.Contains("-nologo"))
            {
                PrintHeader();
            }

            var compiler = new StaticCompiler();
            var importer = new IkvmImporterInternal();
            var targets = new List<CompilerOptions>();
            var rootTarget = new CompilerOptions();
            var context = new RuntimeContext(new RuntimeContextOptions(), new ManagedResolver(compiler), argList.Contains("-bootstrap"), compiler);

            compiler.rootTarget = rootTarget;
            importer.ParseCommandLine(context, compiler, argList.GetEnumerator(), targets, rootTarget);
            compiler.Init(nonDeterministicOutput, rootTarget.debugMode, libpaths);
            resolver.Warning += (warning, message, parameters) => loader_Warning(compiler, warning, message, parameters);
            resolver.Init(compiler.Universe, nostdlib, rootTarget.unresolvedReferences, libpaths);
            ResolveReferences(compiler, targets);
            ResolveStrongNameKeys(targets);

            if (targets.Count == 0)
            {
                throw new FatalCompilerErrorException(Message.NoTargetsFound);
            }
            if (compiler.errorCount != 0)
            {
                return 1;
            }

            try
            {
                return CompilerClassLoader.Compile(context, compiler, runtimeAssembly, targets);
            }
            catch (FileFormatLimitationExceededException x)
            {
                throw new FatalCompilerErrorException(Message.FileFormatLimitationExceeded, x.Message);
            }
        }

        static void loader_Warning(StaticCompiler compiler, AssemblyResolver.WarningId warning, string message, string[] parameters)
        {
            switch (warning)
            {
                case AssemblyResolver.WarningId.HigherVersion:
                    IssueMessage(compiler, Message.AssumeAssemblyVersionMatch, parameters);
                    break;
                case AssemblyResolver.WarningId.InvalidLibDirectoryOption:
                    IssueMessage(compiler, Message.InvalidDirectoryInLibOptionPath, parameters);
                    break;
                case AssemblyResolver.WarningId.InvalidLibDirectoryEnvironment:
                    IssueMessage(compiler, Message.InvalidDirectoryInLibEnvironmentPath, parameters);
                    break;
                case AssemblyResolver.WarningId.LegacySearchRule:
                    IssueMessage(compiler, Message.LegacySearchRule, parameters);
                    break;
                case AssemblyResolver.WarningId.LocationIgnored:
                    IssueMessage(compiler, Message.AssemblyLocationIgnored, parameters);
                    break;
                default:
                    IssueMessage(compiler, Message.UnknownWarning, string.Format(message, parameters));
                    break;
            }
        }

        static void ResolveStrongNameKeys(List<CompilerOptions> targets)
        {
            foreach (var options in targets)
            {
                if (options.keyfile != null && options.keycontainer != null)
                    throw new FatalCompilerErrorException(Message.CannotSpecifyBothKeyFileAndContainer);

                if (options.keyfile == null && options.keycontainer == null && options.delaysign)
                    throw new FatalCompilerErrorException(Message.DelaySignRequiresKey);

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
                throw new FatalCompilerErrorException(Message.ErrorReadingFile, path.ToString(), x.Message);
            }
        }

        static string GetVersionAndCopyrightInfo()
        {
            var asm = typeof(IkvmImporterInternal).Assembly;
            var desc = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<System.Reflection.AssemblyTitleAttribute>(asm);
            var copy = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<System.Reflection.AssemblyCopyrightAttribute>(asm);
            var info = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>(asm);
            return $"{desc.Title} ({info.InformationalVersion}){Environment.NewLine}{copy.Copyright}"; // TODO: Add domain once we get one {Environment.NewLine}http://www.ikvm.org/
        }

        static void PrintHeader()
        {
            Console.Error.WriteLine(GetVersionAndCopyrightInfo());
            Console.Error.WriteLine();
        }

        static void PrintHelp()
        {
            PrintHeader();
            Console.Error.WriteLine("Usage: ikvmc [-options] <classOrJar1> ... <classOrJarN>");
            Console.Error.WriteLine();
            Console.Error.WriteLine("Compiler Options:");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      - OUTPUT FILES -");
            Console.Error.WriteLine("-out:<outputfile>              Specify the output filename");
            Console.Error.WriteLine("-assembly:<name>               Specify assembly name");
            Console.Error.WriteLine("-version:<M.m.b.r>             Specify assembly version");
            Console.Error.WriteLine("-target:exe                    Build a console executable");
            Console.Error.WriteLine("-target:winexe                 Build a windows executable");
            Console.Error.WriteLine("-target:library                Build a library");
            Console.Error.WriteLine("-target:module                 Build a module for use by the linker");
            Console.Error.WriteLine("-platform:<string>             Limit which platforms this code can run on:");
            Console.Error.WriteLine("                               x86, x64, arm, anycpu32bitpreferred, or");
            Console.Error.WriteLine("                               anycpu. The default is anycpu.");
            Console.Error.WriteLine("-runtime:<filespec>            Use the specified IKVM runtime assembly.");
            Console.Error.WriteLine("-keyfile:<keyfilename>         Use keyfile to sign the assembly");
            Console.Error.WriteLine("-key:<keycontainer>            Use keycontainer to sign the assembly");
            Console.Error.WriteLine("-delaysign                     Delay-sign the assembly");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      - INPUT FILES -");
            Console.Error.WriteLine("-reference:<filespec>          Reference an assembly (short form -r:<filespec>)");
            Console.Error.WriteLine("-recurse:<filespec>            Recurse directory and include matching files");
            Console.Error.WriteLine("-exclude:<filename>            A file containing a list of classes to exclude");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      - RESOURCES -");
            Console.Error.WriteLine("-fileversion:<version>         File version");
            Console.Error.WriteLine("-win32icon:<file>              Embed specified icon in output");
            Console.Error.WriteLine("-win32manifest:<file>          Specify a Win32 manifest file (.xml)");
            Console.Error.WriteLine("-resource:<name>=<path>        Include file as Java resource");
            Console.Error.WriteLine("-externalresource:<name>=<path>");
            Console.Error.WriteLine("                               Reference file as Java resource");
            Console.Error.WriteLine("-compressresources             Compress resources");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      - CODE GENERATION -");
            Console.Error.WriteLine();
            Console.Error.WriteLine("-debug[+|-]                    Emit debugging information");
            Console.Error.WriteLine("-debug:{full|portable|embedded}");
            Console.Error.WriteLine("                               Specify debugging type('portable' is default,");
            Console.Error.WriteLine("                               'portable' is a cross - platform format,");
            Console.Error.WriteLine("                               'embedded' is a cross - platform format embedded into");
            Console.Error.WriteLine("                               the target.dll or.exe");
            Console.Error.WriteLine("-deterministic                 Produce a deterministic assembly");
            Console.Error.WriteLine("                               (including module version GUID and timestamp)");
            Console.Error.WriteLine("-noautoserialization           Disable automatic .NET serialization support");
            Console.Error.WriteLine("-noglobbing                    Don't glob the arguments passed to main");
            Console.Error.WriteLine("-nojni                         Do not generate JNI stub for native methods");
            Console.Error.WriteLine("-opt:fields                    Remove unused private fields");
            Console.Error.WriteLine("-removeassertions              Remove all assert statements");
            Console.Error.WriteLine("-strictfinalfieldsemantics     Don't allow final fields to be modified outside");
            Console.Error.WriteLine("                               of initializer methods");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      - ERRORS AND WARNINGS -");
            Console.Error.WriteLine("-nowarn:<warning[:key]>        Suppress specified warnings");
            Console.Error.WriteLine("-warnaserror                   Treat all warnings as errors");
            Console.Error.WriteLine("-warnaserror:<warning[:key]>   Treat specified warnings as errors");
            Console.Error.WriteLine("-writeSuppressWarningsFile:<file>");
            Console.Error.WriteLine("                               Write response file with -nowarn:<warning[:key]>");
            Console.Error.WriteLine("                               options to suppress all encountered warnings");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      - MISCELLANEOUS -");
            Console.Error.WriteLine("@<filename>                    Read more options from file");
            Console.Error.WriteLine("-help                          Display this usage message (Short form: -?)");
            Console.Error.WriteLine("-nologo                        Suppress compiler copyright message");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      - ADVANCED -");
            Console.Error.WriteLine("-main:<class>                  Specify the class containing the main method");
            Console.Error.WriteLine("-srcpath:<path>                Prepend path and package name to source file");
            Console.Error.WriteLine("-apartment:sta                 (default) Apply STAThreadAttribute to main");
            Console.Error.WriteLine("-apartment:mta                 Apply MTAThreadAttribute to main");
            Console.Error.WriteLine("-apartment:none                Don't apply STAThreadAttribute to main");
            Console.Error.WriteLine("-D<name>=<value>               Set system property (at runtime)");
            Console.Error.WriteLine("-ea[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("-enableassertions[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("                               Set system property to enable assertions");
            Console.Error.WriteLine("-da[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("-disableassertions[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("                               Set system property to disable assertions");
            Console.Error.WriteLine("-nostacktraceinfo              Don't create metadata to emit rich stack traces");
            Console.Error.WriteLine("-Xtrace:<string>               Displays all tracepoints with the given name");
            Console.Error.WriteLine("-Xmethodtrace:<string>         Build tracing into the specified output methods");
            Console.Error.WriteLine("-privatepackage:<prefix>       Mark all classes with a package name starting");
            Console.Error.WriteLine("                               with <prefix> as internal to the assembly");
            Console.Error.WriteLine("-time                          Display timing statistics");
            Console.Error.WriteLine("-classloader:<class>           Set custom class loader class for assembly");
            Console.Error.WriteLine("-sharedclassloader             All targets below this level share a common");
            Console.Error.WriteLine("                               class loader");
            Console.Error.WriteLine("-baseaddress:<address>         Base address for the library to be built");
            Console.Error.WriteLine("-filealign:<n>                 Specify the alignment used for output file");
            Console.Error.WriteLine("-nopeercrossreference          Do not automatically cross reference all peers");
            Console.Error.WriteLine("-nostdlib                      Do not reference standard libraries");
            Console.Error.WriteLine("-lib:<dir>                     Additional directories to search for references");
            Console.Error.WriteLine("-highentropyva                 Enable high entropy ASLR");
            Console.Error.WriteLine("-static                        Disable dynamic binding");
            Console.Error.WriteLine("-assemblyattributes:<file>     Read assembly custom attributes from specified");
            Console.Error.WriteLine("                               class file.");
        }

        void ParseCommandLine(RuntimeContext context, StaticCompiler compiler, IEnumerator<string> arglist, List<CompilerOptions> targets, CompilerOptions options)
        {
            options.target = PEFileKinds.ConsoleApplication;
            options.guessFileKind = true;
            options.version = new Version(0, 0, 0, 0);
            options.apartment = ApartmentState.STA;
            options.props = new Dictionary<string, string>();
            ContinueParseCommandLine(context, compiler, arglist, targets, options);
        }

        void ContinueParseCommandLine(RuntimeContext context, StaticCompiler compiler, IEnumerator<string> arglist, List<CompilerOptions> targets, CompilerOptions options)
        {
            var fileNames = new List<string>();
            while (arglist.MoveNext())
            {
                var s = arglist.Current;
                if (s == "{")
                {
                    if (!nonleaf)
                    {
                        ReadFiles(context, compiler, options, fileNames);
                        nonleaf = true;
                    }
                    IkvmImporterInternal nestedLevel = new IkvmImporterInternal();
                    nestedLevel.manifestMainClass = manifestMainClass;
                    nestedLevel.defaultAssemblyName = defaultAssemblyName;
                    nestedLevel.ContinueParseCommandLine(context, compiler, arglist, targets, options.Copy());
                }
                else if (s == "}")
                {
                    break;
                }
                else if (nonleaf)
                {
                    throw new FatalCompilerErrorException(Message.OptionsMustPreceedChildLevels);
                }
                else if (s[0] == '-')
                {
                    if (s.StartsWith("-out:"))
                    {
                        options.path = GetFileInfo(s.Substring(5));
                    }
                    else if (s.StartsWith("-Xtrace:"))
                    {
                        Tracer.SetTraceLevel(s.Substring(8));
                    }
                    else if (s.StartsWith("-Xmethodtrace:"))
                    {
                        Tracer.HandleMethodTrace(s.Substring(14));
                    }
                    else if (s.StartsWith("-assembly:"))
                    {
                        options.assembly = s.Substring(10);
                    }
                    else if (s.StartsWith("-target:"))
                    {
                        switch (s)
                        {
                            case "-target:exe":
                                options.target = PEFileKinds.ConsoleApplication;
                                options.guessFileKind = false;
                                break;
                            case "-target:winexe":
                                options.target = PEFileKinds.WindowApplication;
                                options.guessFileKind = false;
                                break;
                            case "-target:module":
                                options.targetIsModule = true;
                                options.target = PEFileKinds.Dll;
                                options.guessFileKind = false;
                                nonDeterministicOutput = true;
                                break;
                            case "-target:library":
                                options.target = PEFileKinds.Dll;
                                options.guessFileKind = false;
                                break;
                            default:
                                throw new FatalCompilerErrorException(Message.UnrecognizedTargetType, s.Substring(8));
                        }
                    }
                    else if (s.StartsWith("-platform:"))
                    {
                        switch (s)
                        {
                            case "-platform:x86":
                                options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit;
                                options.imageFileMachine = ImageFileMachine.I386;
                                break;
                            case "-platform:x64":
                                options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus;
                                options.imageFileMachine = ImageFileMachine.AMD64;
                                break;
                            case "-platform:arm":
                                options.pekind = PortableExecutableKinds.ILOnly;
                                options.imageFileMachine = ImageFileMachine.ARM;
                                break;
                            case "-platform:arm64":
                                options.pekind = PortableExecutableKinds.ILOnly;
                                options.imageFileMachine = ImageFileMachine.ARM64;
                                break;
                            case "-platform:anycpu32bitpreferred":
                                options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Preferred32Bit;
                                options.imageFileMachine = ImageFileMachine.UNKNOWN;
                                break;
                            case "-platform:anycpu":
                                options.pekind = PortableExecutableKinds.ILOnly;
                                options.imageFileMachine = ImageFileMachine.UNKNOWN;
                                break;
                            default:
                                throw new FatalCompilerErrorException(Message.UnrecognizedPlatform, s.Substring(10));
                        }
                    }
                    else if (s.StartsWith("-apartment:"))
                    {
                        switch (s)
                        {
                            case "-apartment:sta":
                                options.apartment = ApartmentState.STA;
                                break;
                            case "-apartment:mta":
                                options.apartment = ApartmentState.MTA;
                                break;
                            case "-apartment:none":
                                options.apartment = ApartmentState.Unknown;
                                break;
                            default:
                                throw new FatalCompilerErrorException(Message.UnrecognizedApartment, s.Substring(11));
                        }
                    }
                    else if (s == "-noglobbing")
                    {
                        options.noglobbing = true;
                    }
                    else if (s.StartsWith("-D"))
                    {
                        var keyvalue = s.Substring(2).Split('=');
                        if (keyvalue.Length != 2)
                            keyvalue = new string[] { keyvalue[0], "" };

                        options.props[keyvalue[0]] = keyvalue[1];
                    }
                    else if (s == "-ea" || s == "-enableassertions")
                    {
                        options.props["ikvm.assert.default"] = "true";
                    }
                    else if (s == "-da" || s == "-disableassertions")
                    {
                        options.props["ikvm.assert.default"] = "false";
                    }
                    else if (s.StartsWith("-ea:") || s.StartsWith("-enableassertions:"))
                    {
                        options.props["ikvm.assert.enable"] = s.Substring(s.IndexOf(':') + 1);
                    }
                    else if (s.StartsWith("-da:") || s.StartsWith("-disableassertions:"))
                    {
                        options.props["ikvm.assert.disable"] = s.Substring(s.IndexOf(':') + 1);
                    }
                    else if (s == "-removeassertions")
                    {
                        options.codegenoptions |= CodeGenOptions.RemoveAsserts;
                    }
                    else if (s.StartsWith("-main:"))
                    {
                        options.mainClass = s.Substring(6);
                    }
                    else if (s.StartsWith("-reference:") || s.StartsWith("-r:"))
                    {
                        var r = s.Substring(s.IndexOf(':') + 1);
                        if (r == "")
                            throw new FatalCompilerErrorException(Message.MissingFileSpecification, s);

                        ArrayAppend(ref options.unresolvedReferences, r);
                    }
                    else if (s.StartsWith("-recurse:"))
                    {
                        var spec = s.Substring(9);
                        var exists = false;

                        // MONOBUG On Mono 1.0.2, Directory.Exists throws an exception if we pass an invalid directory name
                        try
                        {
                            exists = Directory.Exists(spec);
                        }
                        catch (IOException)
                        {

                        }

                        bool found;
                        if (exists)
                        {
                            var dir = new DirectoryInfo(spec);
                            found = Recurse(context, compiler, options, dir, dir, "*");
                        }
                        else
                        {
                            try
                            {
                                var dir = new DirectoryInfo(Path.GetDirectoryName(spec));
                                if (dir.Exists)
                                {
                                    found = Recurse(context, compiler, options, dir, dir, Path.GetFileName(spec));
                                }
                                else
                                {
                                    found = RecurseJar(context, compiler, options, spec);
                                }
                            }
                            catch (PathTooLongException)
                            {
                                throw new FatalCompilerErrorException(Message.PathTooLong, spec);
                            }
                            catch (DirectoryNotFoundException)
                            {
                                throw new FatalCompilerErrorException(Message.PathNotFound, spec);
                            }
                            catch (ArgumentException)
                            {
                                throw new FatalCompilerErrorException(Message.InvalidPath, spec);
                            }
                        }

                        if (!found)
                            throw new FatalCompilerErrorException(Message.FileNotFound, spec);
                    }
                    else if (s.StartsWith("-resource:"))
                    {
                        var spec = s.Substring(10).Split('=');
                        if (spec.Length != 2)
                            throw new FatalCompilerErrorException(Message.InvalidOptionSyntax, s);

                        var fileInfo = GetFileInfo(spec[1]);
                        var fileName = spec[0].TrimStart('/').TrimEnd('/');
                        options.GetResourcesJar().Add(fileName, ReadAllBytes(fileInfo), fileInfo);
                    }
                    else if (s.StartsWith("-externalresource:"))
                    {
                        var spec = s.Substring(18).Split('=');
                        if (spec.Length != 2)
                            throw new FatalCompilerErrorException(Message.InvalidOptionSyntax, s);
                        if (!File.Exists(spec[1]))
                            throw new FatalCompilerErrorException(Message.ExternalResourceNotFound, spec[1]);
                        if (Path.GetFileName(spec[1]) != spec[1])
                            throw new FatalCompilerErrorException(Message.ExternalResourceNameInvalid, spec[1]);

                        // TODO resource name clashes should be tested
                        options.externalResources ??= new Dictionary<string, string>();
                        options.externalResources.Add(spec[0], spec[1]);
                    }
                    else if (s == "-nojni")
                    {
                        options.codegenoptions |= CodeGenOptions.NoJNI;
                    }
                    else if (s.StartsWith("-exclude:"))
                    {
                        ProcessExclusionFile(ref options.classesToExclude, s.Substring(9));
                    }
                    else if (s.StartsWith("-version:"))
                    {
                        var str = s.Substring(9);
                        if (!TryParseVersion(s.Substring(9), out options.version))
                            throw new FatalCompilerErrorException(Message.InvalidVersionFormat, str);
                    }
                    else if (s.StartsWith("-fileversion:"))
                    {
                        options.fileversion = s.Substring(13);
                    }
                    else if (s.StartsWith("-win32icon:"))
                    {
                        options.iconfile = GetFileInfo(s.Substring(11));
                    }
                    else if (s.StartsWith("-win32manifest:"))
                    {
                        options.manifestFile = GetFileInfo(s.Substring(15));
                    }
                    else if (s.StartsWith("-keyfile:"))
                    {
                        options.keyfile = GetFileInfo(s.Substring(9));
                    }
                    else if (s.StartsWith("-key:"))
                    {
                        options.keycontainer = s.Substring(5);
                    }
                    else if (s == "-delaysign")
                    {
                        options.delaysign = true;
                    }
                    else if (s.StartsWith("-debug"))
                    {
                        var mode = s.Substring(6);
                        if (mode == "" || mode == "+" || mode == ":full")
                        {
                            options.codegenoptions |= CodeGenOptions.EmitSymbols;
                            options.debugMode = DebugMode.Full;
                        }
                        else if (mode == ":portable")
                        {
                            options.codegenoptions |= CodeGenOptions.EmitSymbols;
                            options.debugMode = DebugMode.Portable;
                        }
                        else if (mode == ":embedded")
                        {
                            options.codegenoptions |= CodeGenOptions.EmitSymbols;
                            options.debugMode = DebugMode.Embedded;
                        }
                    }
                    else if (s == "-deterministic-")
                    {
                        nonDeterministicOutput = true;
                    }
                    else if (s == "-optimize-")
                    {
                        options.codegenoptions |= CodeGenOptions.DisableOptimizations;
                    }
                    else if (s.StartsWith("-srcpath:"))
                    {
                        options.sourcepath = s.Substring(9);
                    }
                    else if (s.StartsWith("-remap:"))
                    {
                        options.remapfile = GetFileInfo(s.Substring(7));
                    }
                    else if (s == "-nostacktraceinfo")
                    {
                        options.codegenoptions |= CodeGenOptions.NoStackTraceInfo;
                    }
                    else if (s == "-opt:fields")
                    {
                        options.codegenoptions |= CodeGenOptions.RemoveUnusedFields;
                    }
                    else if (s == "-compressresources")
                    {
                        options.compressedResources = true;
                    }
                    else if (s == "-strictfinalfieldsemantics")
                    {
                        options.codegenoptions |= CodeGenOptions.StrictFinalFieldSemantics;
                    }
                    else if (s.StartsWith("-privatepackage:"))
                    {
                        var prefix = s.Substring(16);
                        ArrayAppend(ref options.privatePackages, prefix);
                    }
                    else if (s.StartsWith("-publicpackage:"))
                    {
                        var prefix = s.Substring(15);
                        ArrayAppend(ref options.publicPackages, prefix);
                    }
                    else if (s.StartsWith("-nowarn:"))
                    {
                        foreach (var w in s.Substring(8).Split(','))
                        {
                            // lame way to chop off the leading zeroes
                            string ws = w;
                            while (ws.StartsWith("0"))
                                ws = ws.Substring(1);

                            options.suppressWarnings[ws] = ws;
                        }
                    }
                    else if (s == "-warnaserror")
                    {
                        options.warnaserror = true;
                    }
                    else if (s.StartsWith("-warnaserror:"))
                    {
                        foreach (string w in s.Substring(13).Split(','))
                        {
                            // lame way to chop off the leading zeroes
                            string ws = w;
                            while (ws.StartsWith("0"))
                                ws = ws.Substring(1);

                            options.errorWarnings[ws] = ws;
                        }
                    }
                    else if (s.StartsWith("-runtime:"))
                    {
                        // NOTE this is an undocumented option
                        runtimeAssembly = s.Substring(9);
                    }
                    else if (s == "-time")
                    {
                        time = true;
                    }
                    else if (s.StartsWith("-classloader:"))
                    {
                        options.classLoader = s.Substring(13);
                    }
                    else if (s == "-sharedclassloader")
                    {
                        options.sharedclassloader ??= new List<CompilerClassLoader>();
                    }
                    else if (s.StartsWith("-baseaddress:"))
                    {
                        var baseAddress = s.Substring(13);
                        ulong baseAddressParsed;
                        if (baseAddress.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            baseAddressParsed = ulong.Parse(baseAddress.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
                        else
                            baseAddressParsed = ulong.Parse(baseAddress); // note that unlike CSC we don't support octal

                        options.baseAddress = (baseAddressParsed & 0xFFFFFFFFFFFF0000UL);
                    }
                    else if (s.StartsWith("-filealign:"))
                    {
                        if (!uint.TryParse(s.Substring(11), out var filealign) || filealign < 512 || filealign > 8192 || (filealign & (filealign - 1)) != 0)
                            throw new FatalCompilerErrorException(Message.InvalidFileAlignment, s.Substring(11));

                        options.fileAlignment = filealign;
                    }
                    else if (s == "-nopeercrossreference")
                    {
                        options.crossReferenceAllPeers = false;
                    }
                    else if (s == "-nostdlib")
                    {
                        // this is a global option
                        nostdlib = true;
                    }
                    else if (s.StartsWith("-lib:"))
                    {
                        // this is a global option
                        libpaths.Add(s.Substring(5));
                    }
                    else if (s == "-noautoserialization")
                    {
                        options.codegenoptions |= CodeGenOptions.NoAutomagicSerialization;
                    }
                    else if (s == "-highentropyva")
                    {
                        options.highentropyva = true;
                    }
                    else if (s.StartsWith("-writeSuppressWarningsFile:"))
                    {
                        options.writeSuppressWarningsFile = GetFileInfo(s.Substring(27));

                        try
                        {
                            options.writeSuppressWarningsFile.Delete();
                        }
                        catch (Exception x)
                        {
                            throw new FatalCompilerErrorException(Message.ErrorWritingFile, options.writeSuppressWarningsFile, x.Message);
                        }
                    }
                    else if (s.StartsWith("-proxy:")) // currently undocumented!
                    {
                        var proxy = s.Substring(7);
                        if (options.proxies.Contains(proxy))
                            IssueMessage(compiler, Message.DuplicateProxy, proxy);

                        options.proxies.Add(proxy);
                    }
                    else if (s == "-nologo")
                    {
                        // Ignore. This is handled earlier.
                    }
                    else if (s == "-XX:+AllowNonVirtualCalls")
                    {
                        JVM.AllowNonVirtualCalls = true;
                    }
                    else if (s == "-static")
                    {
                        // we abuse -static to also enable support for NoRefEmit scenarios
                        options.codegenoptions |= CodeGenOptions.DisableDynamicBinding | CodeGenOptions.NoRefEmitHelpers;
                    }
                    else if (s == "-nojarstubs")    // undocumented temporary option to mitigate risk
                    {
                        options.nojarstubs = true;
                    }
                    else if (s.StartsWith("-assemblyattributes:", StringComparison.Ordinal))
                    {
                        ProcessAttributeAnnotationsClass(context, ref options.assemblyAttributeAnnotations, s.Substring(20));
                    }
                    else if (s == "-w4") // undocumented option to always warn if a class isn't found
                    {
                        options.warningLevelHigh = true;
                    }
                    else if (s == "-noparameterreflection") // undocumented option to compile core class libraries with, to disable MethodParameter attribute
                    {
                        options.noParameterReflection = true;
                    }
                    else if (s == "-bootstrap")
                    {
                        options.bootstrap = true;
                    }
                    else
                    {
                        throw new FatalCompilerErrorException(Message.UnrecognizedOption, s);
                    }
                }
                else
                {
                    fileNames.Add(s);
                }
                if (options.targetIsModule && options.sharedclassloader != null)
                {
                    throw new FatalCompilerErrorException(Message.SharedClassLoaderCannotBeUsedOnModuleTarget);
                }
            }

            if (nonleaf)
            {
                return;
            }

            ReadFiles(context, compiler, options, fileNames);

            if (options.assembly == null)
            {
                var basename = options.path == null ? defaultAssemblyName : options.path.Name;
                if (basename == null)
                    throw new FatalCompilerErrorException(Message.NoOutputFileSpecified);

                int idx = basename.LastIndexOf('.');
                if (idx > 0)
                    options.assembly = basename.Substring(0, idx);
                else
                    options.assembly = basename;
            }

            if (options.path != null && options.guessFileKind)
            {
                if (options.path.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                    options.target = PEFileKinds.Dll;

                options.guessFileKind = false;
            }

            if (options.mainClass == null && manifestMainClass != null && (options.guessFileKind || options.target != PEFileKinds.Dll))
            {
                IssueMessage(compiler, options, Message.MainMethodFromManifest, manifestMainClass);
                options.mainClass = manifestMainClass;
            }

            targets.Add(options);
        }

        internal static FileInfo GetFileInfo(string path)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Directory == null)
                {
                    // this happens with an incorrect unc path (e.g. "\\foo\bar")
                    throw new FatalCompilerErrorException(Message.InvalidPath, path);
                }
                return fileInfo;
            }
            catch (ArgumentException)
            {
                throw new FatalCompilerErrorException(Message.InvalidPath, path);
            }
            catch (NotSupportedException)
            {
                throw new FatalCompilerErrorException(Message.InvalidPath, path);
            }
            catch (PathTooLongException)
            {
                throw new FatalCompilerErrorException(Message.PathTooLong, path);
            }
            catch (UnauthorizedAccessException)
            {
                // this exception does not appear to be possible
                throw new FatalCompilerErrorException(Message.InvalidPath, path);
            }
        }

        private void ReadFiles(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, List<string> fileNames)
        {
            foreach (string fileName in fileNames)
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
                    string path = Path.GetDirectoryName(fileName);
                    files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(fileName));
                }
                catch { }
                if (files == null || files.Length == 0)
                {
                    IssueMessage(compiler, Message.InputFileNotFound, fileName);
                }
                else
                {
                    foreach (string f in files)
                    {
                        ProcessFile(context, compiler, options, null, f);
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
                throw new FatalCompilerErrorException(Message.InvalidStrongNameKeyPair, keyFile != null ? "file" : "container", x.Message);
            }
        }

        static void ResolveReferences(StaticCompiler compiler, List<CompilerOptions> targets)
        {
            var cache = new Dictionary<string, IKVM.Reflection.Assembly>();
            foreach (CompilerOptions target in targets)
            {
                if (target.unresolvedReferences != null)
                {
                    foreach (string reference in target.unresolvedReferences)
                    {
                        foreach (CompilerOptions peer in targets)
                        {
                            if (peer.assembly.Equals(reference, StringComparison.OrdinalIgnoreCase))
                            {
                                ArrayAppend(ref target.peerReferences, peer.assembly);
                                goto next_reference;
                            }
                        }
                        if (!resolver.ResolveReference(cache, ref target.references, reference))
                        {
                            throw new FatalCompilerErrorException(Message.ReferenceNotFound, reference);
                        }
                    next_reference:;
                    }
                }
            }

            // verify that we didn't reference any secondary assemblies of a shared class loader group
            foreach (CompilerOptions target in targets)
            {
                if (target.references != null)
                {
                    foreach (Assembly asm in target.references)
                    {
                        Type forwarder = asm.GetType("__<MainAssembly>");
                        if (forwarder != null && forwarder.Assembly != asm)
                        {
                            IssueMessage(compiler, Message.NonPrimaryAssemblyReference, asm.Location, forwarder.Assembly.GetName().Name);
                        }
                    }
                }
            }

            // add legacy references (from stub files)
            foreach (CompilerOptions target in targets)
            {
                foreach (string assemblyName in target.legacyStubReferences.Keys)
                {
                    ArrayAppend(ref target.references, resolver.LegacyLoad(new AssemblyName(assemblyName), null));
                }
            }

            // now pre-load the secondary assemblies of any shared class loader groups
            foreach (CompilerOptions target in targets)
            {
                if (target.references != null)
                {
                    foreach (Assembly asm in target.references)
                    {
                        RuntimeAssemblyClassLoader.PreloadExportedAssemblies(compiler, asm);
                    }
                }
            }
        }

        private static void ArrayAppend<T>(ref T[] array, T element)
        {
            if (array == null)
            {
                array = new T[] { element };
            }
            else
            {
                array = ArrayUtil.Concat(array, element);
            }
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

        static bool EmitStubWarning(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, byte[] buf)
        {
            IKVM.Runtime.ClassFile cf;

            try
            {
                cf = new IKVM.Runtime.ClassFile(context, IKVM.ByteCode.Decoding.ClassFile.Read(buf), "<unknown>", ClassFileParseOptions.None, null);
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
                    IssueMessage(compiler, options, Message.StubsAreDeprecated, m.Groups[1].Value);
                }
            }
            else
            {
                options.legacyStubReferences[cf.IKVMAssemblyAttribute] = null;
                IssueMessage(compiler, options, Message.StubsAreDeprecated, cf.IKVMAssemblyAttribute);
            }

            return true;
        }

        static bool IsExcludedOrStubLegacy(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, ZipArchiveEntry ze, byte[] data)
        {
            if (ze.Name.EndsWith(".class", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var name = IKVM.Runtime.ClassFile.GetClassName(data, 0, data.Length, out var stub);
                    if (options.IsExcludedClass(name) || (stub && EmitStubWarning(context, compiler, options, data)))
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

        void ProcessManifest(StaticCompiler compiler, CompilerOptions options, ZipArchiveEntry ze)
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

        bool ProcessZipFile(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, string file, Predicate<ZipArchiveEntry> filter)
        {
            try
            {
                using ZipArchive zf = ZipFile.OpenRead(file);

                bool found = false;
                Jar jar = null;
                foreach (ZipArchiveEntry ze in zf.Entries)
                {
                    if (filter != null && !filter(ze))
                    {
                        // skip
                    }
                    else
                    {
                        found = true;
                        byte[] data = ReadFromZip(ze);
                        if (IsExcludedOrStubLegacy(context, compiler, options, ze, data))
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
                throw new FatalCompilerErrorException(Message.ErrorReadingFile, file, x.Message);
            }
        }

        void ProcessFile(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, DirectoryInfo baseDir, string file)
        {
            var fileInfo = GetFileInfo(file);
            if (fileInfo.Extension.Equals(".jar", StringComparison.OrdinalIgnoreCase) || fileInfo.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                ProcessZipFile(context, compiler, options, file, null);
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
                        if (stub && EmitStubWarning(context, compiler, options, data))
                            return;

                        options.GetClassesJar().Add(name.Replace('.', '/') + ".class", data, fileInfo);
                        return;
                    }
                    catch (ClassFormatError x)
                    {
                        IssueMessage(compiler, Message.ClassFormatError, file, x.Message);
                    }
                }

                if (baseDir == null)
                {
                    IssueMessage(compiler, Message.UnknownFileType, file);
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

        bool Recurse(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, DirectoryInfo baseDir, DirectoryInfo dir, string spec)
        {
            bool found = false;
            foreach (FileInfo file in dir.GetFiles(spec))
            {
                found = true;
                ProcessFile(context, compiler, options, baseDir, file.FullName);
            }
            foreach (DirectoryInfo sub in dir.GetDirectories())
            {
                found |= Recurse(context, compiler, options, baseDir, sub, spec);
            }
            return found;
        }

        bool RecurseJar(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, string path)
        {
            string file = "";
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
                    string pathFilter = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar;
                    string fileFilter = "^" + Regex.Escape(Path.GetFileName(file)).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                    return ProcessZipFile(context, compiler, options, path, delegate (ZipArchiveEntry ze)
                    {
                        // MONOBUG Path.GetDirectoryName() doesn't normalize / to \ on Windows
                        string name = ze.FullName.Replace('/', Path.DirectorySeparatorChar);
                        return (Path.GetDirectoryName(name) + Path.DirectorySeparatorChar).StartsWith(pathFilter)
                            && Regex.IsMatch(Path.GetFileName(ze.FullName), fileFilter);
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
                using (StreamReader file = new StreamReader(filename))
                {
                    String line;
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
                throw new FatalCompilerErrorException(Message.ErrorReadingFile, filename, x.Message);
            }
        }

        static void ProcessAttributeAnnotationsClass(RuntimeContext context, ref object[] annotations, string filename)
        {
            try
            {
                using var file = File.OpenRead(filename);
                var cf = new IKVM.Runtime.ClassFile(context, IKVM.ByteCode.Decoding.ClassFile.Read(file), null, ClassFileParseOptions.None, null);
                ArrayAppend(ref annotations, cf.Annotations);
            }
            catch (Exception x)
            {
                throw new FatalCompilerErrorException(Message.ErrorReadingFile, filename, x.Message);
            }
        }

        internal static void IssueMessage(StaticCompiler compiler, Message msgId, params string[] values)
        {
            IssueMessage(compiler, compiler.rootTarget, msgId, values);
        }

        internal static void IssueMessage(StaticCompiler compiler, CompilerOptions options, Message msgId, params string[] values)
        {
            if (compiler.errorCount != 0 && msgId < Message.StartErrors && !options.warnaserror)
            {
                // don't display any warnings after we've emitted an error message
                return;
            }

            string key = ((int)msgId).ToString();
            for (int i = 0; ; i++)
            {
                if (options.suppressWarnings.ContainsKey(key))
                {
                    return;
                }
                if (i == values.Length)
                {
                    break;
                }
                key += ":" + values[i];
            }
            options.suppressWarnings.Add(key, key);
            if (options.writeSuppressWarningsFile != null)
            {
                File.AppendAllText(options.writeSuppressWarningsFile.FullName, "-nowarn:" + key + Environment.NewLine);
            }
            string msg;
            switch (msgId)
            {
                case Message.MainMethodFound:
                    msg = "Found main method in class \"{0}\"";
                    break;
                case Message.OutputFileIs:
                    msg = "Output file is \"{0}\"";
                    break;
                case Message.AutoAddRef:
                    msg = "Automatically adding reference to \"{0}\"";
                    break;
                case Message.MainMethodFromManifest:
                    msg = "Using main class \"{0}\" based on jar manifest";
                    break;
                case Message.ClassNotFound:
                    msg = "Class \"{0}\" not found";
                    break;
                case Message.ClassFormatError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (class format error \"{1}\")";
                    break;
                case Message.DuplicateClassName:
                    msg = "Duplicate class name: \"{0}\"";
                    break;
                case Message.IllegalAccessError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (illegal access error \"{1}\")";
                    break;
                case Message.VerificationError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (verification error \"{1}\")";
                    break;
                case Message.NoClassDefFoundError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (missing class \"{1}\")";
                    break;
                case Message.GenericUnableToCompileError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\": \"{2}\")";
                    break;
                case Message.DuplicateResourceName:
                    msg = "Skipping resource (name clash): \"{0}\"";
                    break;
                case Message.SkippingReferencedClass:
                    msg = "Skipping class: \"{0}\"" + Environment.NewLine +
                        "    (class is already available in referenced assembly \"{1}\")";
                    break;
                case Message.NoJniRuntime:
                    msg = "Unable to load runtime JNI assembly";
                    break;
                case Message.EmittedNoClassDefFoundError:
                    msg = "Emitted java.lang.NoClassDefFoundError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedIllegalAccessError:
                    msg = "Emitted java.lang.IllegalAccessError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedInstantiationError:
                    msg = "Emitted java.lang.InstantiationError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedIncompatibleClassChangeError:
                    msg = "Emitted java.lang.IncompatibleClassChangeError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedNoSuchFieldError:
                    msg = "Emitted java.lang.NoSuchFieldError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedAbstractMethodError:
                    msg = "Emitted java.lang.AbstractMethodError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedNoSuchMethodError:
                    msg = "Emitted java.lang.NoSuchMethodError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedLinkageError:
                    msg = "Emitted java.lang.LinkageError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedVerificationError:
                    msg = "Emitted java.lang.VerificationError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedClassFormatError:
                    msg = "Emitted java.lang.ClassFormatError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.InvalidCustomAttribute:
                    msg = "Error emitting \"{0}\" custom attribute" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.IgnoredCustomAttribute:
                    msg = "Custom attribute \"{0}\" was ignored" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.AssumeAssemblyVersionMatch:
                    msg = "Assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime policy";
                    break;
                case Message.InvalidDirectoryInLibOptionPath:
                    msg = "Directory \"{0}\" specified in -lib option is not valid";
                    break;
                case Message.InvalidDirectoryInLibEnvironmentPath:
                    msg = "Directory \"{0}\" specified in LIB environment is not valid";
                    break;
                case Message.LegacySearchRule:
                    msg = "Found assembly \"{0}\" using legacy search rule, please append '.dll' to the reference";
                    break;
                case Message.AssemblyLocationIgnored:
                    msg = "Assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identity \"{2}\"";
                    break;
                case Message.InterfaceMethodCantBeInternal:
                    msg = "Ignoring @ikvm.lang.Internal annotation on interface method" + Environment.NewLine +
                        "    (\"{0}.{1}{2}\")";
                    break;
                case Message.NonPrimaryAssemblyReference:
                    msg = "Referenced assembly \"{0}\" is not the primary assembly of a shared class loader group, please reference primary assembly \"{1}\" instead";
                    break;
                case Message.MissingType:
                    msg = "Reference to type \"{0}\" claims it is defined in \"{1}\", but it could not be found";
                    break;
                case Message.MissingReference:
                    msg = "The type '{0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{1}'";
                    break;
                case Message.DuplicateAssemblyReference:
                    msg = "Duplicate assembly reference \"{0}\"";
                    break;
                case Message.UnableToResolveType:
                    msg = "Reference in \"{0}\" to type \"{1}\" claims it is defined in \"{2}\", but it could not be found";
                    break;
                case Message.StubsAreDeprecated:
                    msg = "Compiling stubs is deprecated. Please add a reference to assembly \"{0}\" instead.";
                    break;
                case Message.WrongClassName:
                    msg = "Unable to compile \"{0}\" (wrong name: \"{1}\")";
                    break;
                case Message.ReflectionCallerClassRequiresCallerID:
                    msg = "Reflection.getCallerClass() called from non-CallerID method" + Environment.NewLine +
                        "    (\"{0}.{1}{2}\")";
                    break;
                case Message.LegacyAssemblyAttributesFound:
                    msg = "Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.";
                    break;
                case Message.UnableToCreateLambdaFactory:
                    msg = "Unable to create static lambda factory.";
                    break;
                case Message.UnableToCreateProxy:
                    msg = "Unable to create proxy \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.DuplicateProxy:
                    msg = "Duplicate proxy \"{0}\"";
                    break;
                case Message.MapXmlUnableToResolveOpCode:
                    msg = "Unable to resolve opcode in remap file: {0}";
                    break;
                case Message.MapXmlError:
                    msg = "Error in remap file: {0}";
                    break;
                case Message.InputFileNotFound:
                    msg = "Source file '{0}' not found";
                    break;
                case Message.UnknownFileType:
                    msg = "Unknown file type: {0}";
                    break;
                case Message.UnknownElementInMapFile:
                    msg = "Unknown element {0} in remap file, line {1}, column {2}";
                    break;
                case Message.UnknownAttributeInMapFile:
                    msg = "Unknown attribute {0} in remap file, line {1}, column {2}";
                    break;
                case Message.InvalidMemberNameInMapFile:
                    msg = "Invalid {0} name '{1}' in remap file in class {2}";
                    break;
                case Message.InvalidMemberSignatureInMapFile:
                    msg = "Invalid {0} signature '{3}' in remap file for {0} {1}.{2}";
                    break;
                case Message.InvalidPropertyNameInMapFile:
                    msg = "Invalid property {0} name '{3}' in remap file for property {1}.{2}";
                    break;
                case Message.InvalidPropertySignatureInMapFile:
                    msg = "Invalid property {0} signature '{3}' in remap file for property {1}.{2}";
                    break;
                case Message.UnknownWarning:
                    msg = "{0}";
                    break;
                case Message.CallerSensitiveOnUnsupportedMethod:
                    msg = "CallerSensitive annotation on unsupported method" + Environment.NewLine +
                        "    (\"{0}.{1}{2}\")";
                    break;
                case Message.RemappedTypeMissingDefaultInterfaceMethod:
                    msg = "{0} does not implement default interface method {1}";
                    break;
                default:
                    throw new InvalidProgramException();
            }
            bool error = msgId >= Message.StartErrors
                || (options.warnaserror && msgId >= Message.StartWarnings)
                || options.errorWarnings.ContainsKey(key)
                || options.errorWarnings.ContainsKey(((int)msgId).ToString());
            Console.Error.Write("{0} IKVMC{1:D4}: ", error ? "error" : msgId < Message.StartWarnings ? "note" : "warning", (int)msgId);
            if (error && Message.StartWarnings <= msgId && msgId < Message.StartErrors)
            {
                Console.Error.Write("Warning as Error: ");
            }
            Console.Error.WriteLine(msg, values);
            if (options != compiler.rootTarget && options.path != null)
            {
                Console.Error.WriteLine("    (in {0})", options.path);
            }
            if (error)
            {
                if (++compiler.errorCount == 100)
                {
                    throw new FatalCompilerErrorException(Message.MaximumErrorCountReached);
                }
            }
        }

    }

}
