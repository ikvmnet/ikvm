/*
  Copyright (C) 2002-2010 Jeroen Frijters

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
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
using ResolveEventHandler = IKVM.Reflection.ResolveEventHandler;
using ResolveEventArgs = IKVM.Reflection.ResolveEventArgs;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using IKVM.Internal;
using System.Text.RegularExpressions;

class IkvmcCompiler
{
	private bool nonleaf;
	private string manifestMainClass;
	private Dictionary<string, byte[]> classes = new Dictionary<string, byte[]>();
	private Dictionary<string, List<ResourceItem>> resources = new Dictionary<string, List<ResourceItem>>();
	private string defaultAssemblyName;
	private List<string> classesToExclude = new List<string>();
	private static bool time;
	private static string runtimeAssembly;
	private static bool nostdlib;
	private static readonly List<string> libpaths = new List<string>();
	internal static readonly AssemblyResolver resolver = new AssemblyResolver();

	private static void AddArg(List<string> arglist, string s, int depth)
	{
		if (s.StartsWith("@"))
		{
			if (depth++ > 16)
			{
				Console.Error.WriteLine("Error: response file nesting depth exceeded");
				Environment.Exit(1);
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
			catch (Exception x)
			{
				Console.Error.WriteLine("Error: unable to read response file: {0}{1}\t({2})", s.Substring(1), Environment.NewLine, x.Message);
				Environment.Exit(1);
			}
		}
		else
		{
			arglist.Add(s);
		}
	}

	private static List<string> GetArgs(string[] args)
	{
		List<string> arglist = new List<string>();
		foreach(string s in args)
		{
			AddArg(arglist, s, 0);
		}
		return arglist;
	}

	static int Main(string[] args)
	{
		DateTime start = DateTime.Now;
		System.Threading.Thread.CurrentThread.Name = "compiler";
		Tracer.EnableTraceConsoleListener();
		Tracer.EnableTraceForDebug();
		List<string> argList = GetArgs(args);
		if (argList.Count == 0)
		{
			PrintHelp();
			return 1;
		}
		IkvmcCompiler comp = new IkvmcCompiler();
		List<CompilerOptions> targets = new List<CompilerOptions>();
		CompilerOptions toplevel = new CompilerOptions();
		StaticCompiler.toplevel = toplevel;
		int rc = comp.ParseCommandLine(argList.GetEnumerator(), targets, toplevel);
		if (rc == 0)
		{
			resolver.Warning += new AssemblyResolver.WarningEvent(loader_Warning);
			resolver.Init(StaticCompiler.Universe, nostdlib, toplevel.unresolvedReferences, libpaths);
		}
		if (rc == 0)
		{
			rc = ResolveReferences(targets);
		}
		if (rc == 0)
		{
			rc = ResolveStrongNameKeys(targets);
		}
		if (rc == 0)
		{
			try
			{
				if (targets.Count == 0)
				{
					Console.Error.WriteLine("Error: no target founds");
					rc = 1;
				}
				else
				{
					try
					{
						rc = CompilerClassLoader.Compile(runtimeAssembly, targets);
					}
					catch(FileFormatLimitationExceededException x)
					{
						Console.Error.WriteLine("Error: {0}", x.Message);
						rc = 1;
					}
				}
			}
			catch(Exception x)
			{
				Console.Error.WriteLine(x);
				rc = 1;
			}
		}
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
		return rc;
	}

	static void loader_Warning(AssemblyResolver.WarningId warning, string message, string[] parameters)
	{
		switch (warning)
		{
			case AssemblyResolver.WarningId.HigherVersion:
				StaticCompiler.IssueMessage(Message.AssumeAssemblyVersionMatch, parameters);
				break;
			case AssemblyResolver.WarningId.InvalidLibDirectoryOption:
				StaticCompiler.IssueMessage(Message.InvalidDirectoryInLibOptionPath, parameters);
				break;
			case AssemblyResolver.WarningId.InvalidLibDirectoryEnvironment:
				StaticCompiler.IssueMessage(Message.InvalidDirectoryInLibEnvironmentPath, parameters);
				break;
			case AssemblyResolver.WarningId.LegacySearchRule:
				StaticCompiler.IssueMessage(Message.LegacySearchRule, parameters);
				break;
			case AssemblyResolver.WarningId.LocationIgnored:
				StaticCompiler.IssueMessage(Message.AssemblyLocationIgnored, parameters);
				break;
			default:
				StaticCompiler.IssueMessage(Message.UnknownWarning, string.Format(message, parameters));
				break;
		}
	}

	private static int ResolveStrongNameKeys(List<CompilerOptions> targets)
	{
		foreach (CompilerOptions options in targets)
		{
			if (options.keyfile != null && options.keycontainer != null)
			{
				Console.Error.WriteLine("Error: you cannot specify both a key file and container");
				return 1;
			}
			if (options.keyfile == null && options.keycontainer == null && options.delaysign)
			{
				Console.Error.WriteLine("Error: you cannot delay sign without a key file or container");
				return 1;
			}
			if (options.keyfile != null)
			{
				if (options.delaysign)
				{
					byte[] buf;
					try
					{
						buf = File.ReadAllBytes(options.keyfile);
					}
					catch (Exception x)
					{
						Console.Error.WriteLine("Error: unable to read key file: {0}", x.Message);
						return 1;
					}
					try
					{
						// maybe it is a key pair, if so we need to extract just the public key
						buf = new StrongNameKeyPair(buf).PublicKey;
					}
					catch { }
					options.publicKey = buf;
				}
				else
				{
					if (!SetStrongNameKeyPair(ref options.keyPair, options.keyfile, true))
					{
						return 1;
					}
				}
			}
			else if (options.keycontainer != null)
			{
				StrongNameKeyPair keyPair = null;
				if (!SetStrongNameKeyPair(ref keyPair, options.keycontainer, false))
				{
					return 1;
				}
				if (options.delaysign)
				{
					options.publicKey = keyPair.PublicKey;
				}
				else
				{
					options.keyPair = keyPair;
				}
			}
		}
		return 0;
	}

	static string GetVersionAndCopyrightInfo()
	{
		System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
		object[] desc = asm.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
		if (desc.Length == 1)
		{
			object[] copyright = asm.GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
			if (copyright.Length == 1)
			{
				return string.Format("{0} version {1}{2}{3}{2}http://www.ikvm.net/",
					((System.Reflection.AssemblyTitleAttribute)desc[0]).Title,
					asm.GetName().Version,
					Environment.NewLine,
					((System.Reflection.AssemblyCopyrightAttribute)copyright[0]).Copyright);
			}
		}
		return "";
	}

	private static void PrintHelp()
	{
		Console.Error.WriteLine(GetVersionAndCopyrightInfo());
		Console.Error.WriteLine();
		Console.Error.WriteLine("usage: ikvmc [-options] <classOrJar1> ... <classOrJarN>");
		Console.Error.WriteLine();
		Console.Error.WriteLine("options:");
		Console.Error.WriteLine("    @<filename>                Read more options from file");
		Console.Error.WriteLine("    -out:<outputfile>          Specify the output filename");
		Console.Error.WriteLine("    -assembly:<name>           Specify assembly name");
		Console.Error.WriteLine("    -target:exe                Build a console executable");
		Console.Error.WriteLine("    -target:winexe             Build a windows executable");
		Console.Error.WriteLine("    -target:library            Build a library");
		Console.Error.WriteLine("    -target:module             Build a module for use by the linker");
		Console.Error.WriteLine("    -platform:<string>         Limit which platforms this code can run on: x86,");
		Console.Error.WriteLine("                               Itanium, x64, or anycpu. The default is anycpu.");
		Console.Error.WriteLine("    -keyfile:<keyfilename>     Use keyfile to sign the assembly");
		Console.Error.WriteLine("    -key:<keycontainer>        Use keycontainer to sign the assembly");
		Console.Error.WriteLine("    -delaysign                 Delay-sign the assembly");
		Console.Error.WriteLine("    -version:<M.m.b.r>         Assembly version");
		Console.Error.WriteLine("    -fileversion:<version>     File version");
		Console.Error.WriteLine("    -win32icon:<file>          Embed specified icon in output");
		Console.Error.WriteLine("    -main:<class>              Specify the class containing the main method");
		Console.Error.WriteLine("    -reference:<filespec>      Reference an assembly (short form -r:<filespec>)");
		Console.Error.WriteLine("    -recurse:<filespec>        Recurse directory and include matching files");
		Console.Error.WriteLine("    -nojni                     Do not generate JNI stub for native methods");
		Console.Error.WriteLine("    -resource:<name>=<path>    Include file as Java resource");
		Console.Error.WriteLine("    -externalresource:<name>=<path>");
		Console.Error.WriteLine("                               Reference file as Java resource");
		Console.Error.WriteLine("    -exclude:<filename>        A file containing a list of classes to exclude");
		Console.Error.WriteLine("    -debug                     Generate debug info for the output file");
		Console.Error.WriteLine("                               (Note that this also causes the compiler to");
		Console.Error.WriteLine("                               generated somewhat less efficient CIL code.)");
		Console.Error.WriteLine("    -srcpath:<path>            Prepend path and package name to source file");
		Console.Error.WriteLine("    -apartment:sta             (default) Apply STAThreadAttribute to main");
		Console.Error.WriteLine("    -apartment:mta             Apply MTAThreadAttribute to main");
		Console.Error.WriteLine("    -apartment:none            Don't apply STAThreadAttribute to main");
		Console.Error.WriteLine("    -noglobbing                Don't glob the arguments");
		Console.Error.WriteLine("    -D<name>=<value>           Set system property (at runtime)");
		Console.Error.WriteLine("    -ea[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("    -enableassertions[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("                               Set system property to enable assertions");
		Console.Error.WriteLine("    -da[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("    -disableassertions[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("                               Set system property to disable assertions");
		Console.Error.WriteLine("    -removeassertions          Remove all assert statements");
		Console.Error.WriteLine("    -nostacktraceinfo          Don't create metadata to emit rich stack traces");
		Console.Error.WriteLine("    -opt:fields                Remove unused private fields");
		Console.Error.WriteLine("    -Xtrace:<string>           Displays all tracepoints with the given name");
		Console.Error.WriteLine("    -Xmethodtrace:<string>     Build tracing into the specified output methods");
		Console.Error.WriteLine("    -compressresources         Compress resources");
		Console.Error.WriteLine("    -strictfinalfieldsemantics Don't allow final fields to be modified outside");
		Console.Error.WriteLine("                               of initializer methods");
		Console.Error.WriteLine("    -privatepackage:<prefix>   Mark all classes with a package name starting");
		Console.Error.WriteLine("                               with <prefix> as internal to the assembly");
		Console.Error.WriteLine("    -nowarn:<warning[:key]>    Suppress specified warnings");
		Console.Error.WriteLine("    -warnaserror:<warning[:key]>");
		Console.Error.WriteLine("                               Treat specified warnings as errors");
		Console.Error.WriteLine("    -writeSuppressWarningsFile:<file>");
		Console.Error.WriteLine("                               Write response file with -nowarn:<warning[:key]>");
		Console.Error.WriteLine("                               options to suppress all encountered warnings");
		Console.Error.WriteLine("    -time                      Display timing statistics");
		Console.Error.WriteLine("    -classloader:<class>       Set custom class loader class for assembly");
		Console.Error.WriteLine("    -sharedclassloader         All targets below this level share a common");
		Console.Error.WriteLine("                               class loader");
		Console.Error.WriteLine("    -baseaddress:<address>     Base address for the library to be built");
		Console.Error.WriteLine("    -nopeercrossreference      Do not automatically cross reference all peers");
		Console.Error.WriteLine("    -nostdlib                  Do not reference standard libraries");
		Console.Error.WriteLine("    -lib:<dir>                 Additional directories to search for references");
		Console.Error.WriteLine("    -noautoserialization       Disable automatic .NET serialization support");
	}

	int ParseCommandLine(IEnumerator<string> arglist, List<CompilerOptions> targets, CompilerOptions options)
	{
		options.target = PEFileKinds.ConsoleApplication;
		options.guessFileKind = true;
		options.version = new Version(0, 0, 0, 0);
		options.apartment = ApartmentState.STA;
		options.props = new Dictionary<string, string>();
		return ContinueParseCommandLine(arglist, targets, options);
	}

	int ContinueParseCommandLine(IEnumerator<string> arglist, List<CompilerOptions> targets, CompilerOptions options)
	{
		while(arglist.MoveNext())
		{
			string s = arglist.Current;
			if(s == "{")
			{
				nonleaf = true;
				IkvmcCompiler nestedLevel = new IkvmcCompiler();
				nestedLevel.manifestMainClass = manifestMainClass;
				nestedLevel.classes = new Dictionary<string, byte[]>(classes);
				nestedLevel.resources = CompilerOptions.Copy(resources);
				nestedLevel.defaultAssemblyName = defaultAssemblyName;
				nestedLevel.classesToExclude = new List<string>(classesToExclude);
				int rc = nestedLevel.ContinueParseCommandLine(arglist, targets, options.Copy());
				if(rc != 0)
				{
					return rc;
				}
			}
			else if(s == "}")
			{
				break;
			}
			else if(nonleaf)
			{
				Console.Error.WriteLine("Error: you can only specify options before any child levels");
				return 1;
			}
			else if(s[0] == '-')
			{
				if(s.StartsWith("-out:"))
				{
					options.path = s.Substring(5);
				}
				else if(s.StartsWith("-Xtrace:"))
				{
					Tracer.SetTraceLevel(s.Substring(8));
				}
				else if(s.StartsWith("-Xmethodtrace:"))
				{
					Tracer.HandleMethodTrace(s.Substring(14));
				}
				else if(s.StartsWith("-assembly:"))
				{
					options.assembly = s.Substring(10);
				}
				else if(s.StartsWith("-target:"))
				{
					switch(s)
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
							break;
						case "-target:library":
							options.target = PEFileKinds.Dll;
							options.guessFileKind = false;
							break;
						default:
							Console.Error.WriteLine("Warning: unrecognized option: {0}", s);
							break;
					}
				}
				else if(s.StartsWith("-platform:"))
				{
					switch(s)
					{
						case "-platform:x86":
							options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit;
							options.imageFileMachine = ImageFileMachine.I386;
							break;
						case "-platform:Itanium":
							options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus;
							options.imageFileMachine = ImageFileMachine.IA64;
							break;
						case "-platform:x64":
							options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus;
							options.imageFileMachine = ImageFileMachine.AMD64;
							break;
						case "-platform:anycpu":
							options.pekind = PortableExecutableKinds.ILOnly;
							options.imageFileMachine = ImageFileMachine.I386;
							break;
						default:
							Console.Error.WriteLine("Warning: unrecognized option: {0}", s);
							break;
					}
				}
				else if(s.StartsWith("-apartment:"))
				{
					switch(s)
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
							Console.Error.WriteLine("Warning: unrecognized option: {0}", s);
							break;
					}
				}
				else if(s == "-noglobbing")
				{
					options.noglobbing = true;
				}
				else if(s.StartsWith("-D"))
				{
					string[] keyvalue = s.Substring(2).Split('=');
					if(keyvalue.Length != 2)
					{
						keyvalue = new string[] { keyvalue[0], "" };
					}
					options.props[keyvalue[0]] = keyvalue[1];
				}
				else if(s == "-ea" || s == "-enableassertions")
				{
					options.props["ikvm.assert.default"] = "true";
				}
				else if(s == "-da" || s == "-disableassertions")
				{
					options.props["ikvm.assert.default"] = "false";
				}
				else if(s.StartsWith("-ea:") || s.StartsWith("-enableassertions:"))
				{
					options.props["ikvm.assert.enable"] = s.Substring(s.IndexOf(':') + 1);
				}
				else if(s.StartsWith("-da:") || s.StartsWith("-disableassertions:"))
				{
					options.props["ikvm.assert.disable"] = s.Substring(s.IndexOf(':') + 1);
				}
				else if(s == "-removeassertions")
				{
					options.codegenoptions |= CodeGenOptions.RemoveAsserts;
				}
				else if(s.StartsWith("-main:"))
				{
					options.mainClass = s.Substring(6);
				}
				else if(s.StartsWith("-reference:") || s.StartsWith("-r:"))
				{
					string r = s.Substring(s.IndexOf(':') + 1);
					if(r == "")
					{
						Console.Error.WriteLine("Error: missing file specification for '{0}' option", s);
						return 1;
					}
					ArrayAppend(ref options.unresolvedReferences, r);
				}
				else if(s.StartsWith("-recurse:"))
				{
					string spec = s.Substring(9);
					bool exists = false;
					// MONOBUG On Mono 1.0.2, Directory.Exists throws an exception if we pass an invalid directory name
					try
					{
						exists = Directory.Exists(spec);
					}
					catch(IOException)
					{
					}
					if(exists)
					{
						DirectoryInfo dir = new DirectoryInfo(spec);
						Recurse(dir, dir, "*");
					}
					else
					{
						try
						{
							DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(spec));
							if(dir.Exists)
							{
								Recurse(dir, dir, Path.GetFileName(spec));
							}
							else
							{
								RecurseJar(spec);
							}
						}
						catch(PathTooLongException)
						{
							Console.Error.WriteLine("Error: path too long: {0}", spec);
							return 1;
						}
						catch(DirectoryNotFoundException)
						{
							Console.Error.WriteLine("Error: path not found: {0}", spec);
							return 1;
						}
						catch(ArgumentException)
						{
							Console.Error.WriteLine("Error: invalid path: {0}", spec);
							return 1;
						}
					}
				}
				else if(s.StartsWith("-resource:"))
				{
					string[] spec = s.Substring(10).Split('=');
					try
					{
						using(FileStream fs = new FileStream(spec[1], FileMode.Open, FileAccess.Read))
						{
							byte[] b = new byte[fs.Length];
							fs.Read(b, 0, b.Length);
							string name = spec[0];
							if(name.StartsWith("/"))
							{
								// a leading slash is not required, so strip it
								name = name.Substring(1);
							}
							AddResource(null, name, b, null);
						}
					}
					catch(Exception x)
					{
						Console.Error.WriteLine("Error: {0}: {1}", x.Message, spec[1]);
						return 1;
					}
				}
				else if(s.StartsWith("-externalresource:"))
				{
					string[] spec = s.Substring(18).Split('=');
					if(!File.Exists(spec[1]))
					{
						Console.Error.WriteLine("Error: external resource file does not exist: {0}", spec[1]);
						return 1;
					}
					if(Path.GetFileName(spec[1]) != spec[1])
					{
						Console.Error.WriteLine("Error: external resource file may not include path specification: {0}", spec[1]);
						return 1;
					}
					if(options.externalResources == null)
					{
						options.externalResources = new Dictionary<string, string>();
					}
					// TODO resource name clashes should be tested
					options.externalResources.Add(spec[0], spec[1]);
				}
				else if(s == "-nojni")
				{
					options.codegenoptions |= CodeGenOptions.NoJNI;
				}
				else if(s.StartsWith("-exclude:"))
				{
					ProcessExclusionFile(classesToExclude, s.Substring(9));
				}
				else if(s.StartsWith("-version:"))
				{
					string str = s.Substring(9);
					if(!TryParseVersion(s.Substring(9), out options.version))
					{
						Console.Error.WriteLine("Error: Invalid version specified: {0}", str);
						return 1;
					}
				}
				else if(s.StartsWith("-fileversion:"))
				{
					options.fileversion = s.Substring(13);
				}
				else if(s.StartsWith("-win32icon:"))
				{
					options.iconfile = s.Substring(11);
				}
				else if(s.StartsWith("-keyfile:"))
				{
					options.keyfile = s.Substring(9);
				}
				else if(s.StartsWith("-key:"))
				{
					options.keycontainer = s.Substring(5);
				}
				else if(s == "-delaysign")
				{
					options.delaysign = true;
				}
				else if(s == "-debug")
				{
					options.codegenoptions |= CodeGenOptions.Debug;
				}
				else if(s.StartsWith("-srcpath:"))
				{
					options.sourcepath = s.Substring(9);
				}
				else if(s.StartsWith("-remap:"))
				{
					options.remapfile = s.Substring(7);
				}
				else if(s == "-nostacktraceinfo")
				{
					options.codegenoptions |= CodeGenOptions.NoStackTraceInfo;
				}
				else if(s == "-opt:fields")
				{
					options.removeUnusedFields = true;
				}
				else if(s == "-compressresources")
				{
					options.compressedResources = true;
				}
				else if(s == "-strictfinalfieldsemantics")
				{
					options.codegenoptions |= CodeGenOptions.StrictFinalFieldSemantics;
				}
				else if(s.StartsWith("-privatepackage:"))
				{
					string prefix = s.Substring(16);
					ArrayAppend(ref options.privatePackages, prefix);
				}
				else if(s.StartsWith("-publicpackage:"))
				{
					string prefix = s.Substring(15);
					ArrayAppend(ref options.publicPackages, prefix);
				}
				else if(s.StartsWith("-nowarn:"))
				{
					foreach(string w in s.Substring(8).Split(','))
					{
						string ws = w;
						// lame way to chop off the leading zeroes
						while(ws.StartsWith("0"))
						{
							ws = ws.Substring(1);
						}
						options.suppressWarnings[ws] = ws;
					}
				}
				else if(s.StartsWith("-warnaserror:"))
				{
					foreach(string w in s.Substring(13).Split(','))
					{
						string ws = w;
						// lame way to chop off the leading zeroes
						while(ws.StartsWith("0"))
						{
							ws = ws.Substring(1);
						}
						options.errorWarnings[ws] = ws;
					}
				}
				else if(s.StartsWith("-runtime:"))
				{
					// NOTE this is an undocumented option
					runtimeAssembly = s.Substring(9);
				}
				else if(s == "-time")
				{
					time = true;
				}
				else if(s.StartsWith("-classloader:"))
				{
					options.classLoader = s.Substring(13);
				}
				else if(s == "-sharedclassloader")
				{
					if(options.sharedclassloader == null)
					{
						options.sharedclassloader = new List<CompilerClassLoader>();
					}
				}
				else if(s.StartsWith("-baseaddress:"))
				{
					string baseAddress = s.Substring(13);
					ulong baseAddressParsed;
					if (baseAddress.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
					{
						baseAddressParsed = UInt64.Parse(baseAddress.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
					}
					else
					{
						// note that unlike CSC we don't support octal
						baseAddressParsed = UInt64.Parse(baseAddress);
					}
					options.baseAddress = (long)(baseAddressParsed & 0xFFFFFFFFFFFF0000UL);
				}
				else if(s == "-nopeercrossreference")
				{
					options.crossReferenceAllPeers = false;
				}
				else if(s=="-nostdlib")
				{
					// this is a global option
					nostdlib = true;
				}
				else if(s.StartsWith("-lib:"))
				{
					// this is a global option
					libpaths.Add(s.Substring(5));
				}
				else if(s == "-noautoserialization")
				{
					options.codegenoptions |= CodeGenOptions.NoAutomagicSerialization;
				}
				else if(s.StartsWith("-writeSuppressWarningsFile:"))
				{
					options.writeSuppressWarningsFile = s.Substring(27);
					try
					{
						File.Delete(options.writeSuppressWarningsFile);
					}
					catch(Exception x)
					{
						Console.Error.WriteLine("Error: invalid option: {0}{1}\t({2})", s, Environment.NewLine, x.Message);
						return 1;
					}
				}
				else
				{
					Console.Error.WriteLine("Error: unrecognized option: {0}", s);
					return 1;
				}
			}
			else
			{
				if(defaultAssemblyName == null)
				{
					try
					{
						defaultAssemblyName = new FileInfo(Path.GetFileName(s)).Name;
					}
					catch(ArgumentException)
					{
						// if the filename contains a wildcard (or any other invalid character), we ignore
						// it as a potential default assembly name
					}
				}
				string[] files;
				try
				{
					string path = Path.GetDirectoryName(s);
					files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(s));
				}
				catch(Exception)
				{
					Console.Error.WriteLine("Error: invalid filename: {0}", s);
					return 1;
				}
				if(files.Length == 0)
				{
					Console.Error.WriteLine("Error: file not found: {0}", s);
					return 1;
				}
				foreach(string f in files)
				{
					ProcessFile(null, f);
				}
			}
			if(options.targetIsModule && options.sharedclassloader != null)
			{
				Console.Error.WriteLine("Error: -target:module and -sharedclassloader options cannot be combined.");
				return 1;
			}
		}
		if(nonleaf)
		{
			return 0;
		}
		if(options.assembly == null)
		{
			string basename = options.path == null ? defaultAssemblyName : new FileInfo(options.path).Name;
			if(basename == null)
			{
				Console.Error.WriteLine("Error: no output file specified");
				return 1;
			}
			int idx = basename.LastIndexOf('.');
			if(idx > 0)
			{
				options.assembly = basename.Substring(0, idx);
			}
			else
			{
				options.assembly = basename;
			}
		}
		if(options.path != null && options.guessFileKind)
		{
			if(options.path.ToLower().EndsWith(".dll"))
			{
				options.target = PEFileKinds.Dll;
			}
			options.guessFileKind = false;
		}
		if(options.mainClass == null && manifestMainClass != null && (options.guessFileKind || options.target != PEFileKinds.Dll))
		{
			StaticCompiler.IssueMessage(options, Message.MainMethodFromManifest, manifestMainClass);
			options.mainClass = manifestMainClass;
		}
		options.classes = classes;
		options.resources = resources;
		options.classesToExclude = classesToExclude.ToArray();
		targets.Add(options);
		return 0;
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

	private static bool SetStrongNameKeyPair(ref StrongNameKeyPair strongNameKeyPair, string fileNameOrKeyContainer, bool file)
	{
		try
		{
			if (file)
			{
				strongNameKeyPair = new StrongNameKeyPair(File.ReadAllBytes(fileNameOrKeyContainer));
			}
			else
			{
				strongNameKeyPair = new StrongNameKeyPair(fileNameOrKeyContainer);
			}
			// FXBUG we explicitly try to access the public key force a check (the StrongNameKeyPair constructor doesn't validate the key)
			return strongNameKeyPair.PublicKey != null;
		}
		catch (Exception x)
		{
			Console.Error.WriteLine("Error: Invalid key {0} specified.\n\t(\"{1}\")", file ? "file" : "container", x.Message);
			return false;
		}
	}

	private static int ResolveReferences(List<CompilerOptions> targets)
	{
		Dictionary<string, Assembly> cache = new Dictionary<string, Assembly>();
		foreach (CompilerOptions target in targets)
		{
			if (target.unresolvedReferences != null)
			{
				foreach (string reference in target.unresolvedReferences)
				{
					foreach (CompilerOptions peer in targets)
					{
						if (peer.assembly.Equals(reference, StringComparison.InvariantCultureIgnoreCase))
						{
							ArrayAppend(ref target.peerReferences, peer.assembly);
							goto next_reference;
						}
					}
					int rc = resolver.ResolveReference(cache, ref target.references, reference);
					if (rc != 0)
					{
						return rc;
					}
				next_reference: ;
				}
			}
		}
		return 0;
	}

	private static void ArrayAppend<T>(ref T[] array, T element)
	{
		if (array == null)
		{
			array = new T[] { element };
		}
		else
		{
			T[] temp = new T[array.Length + 1];
			Array.Copy(array, 0, temp, 0, array.Length);
			temp[temp.Length - 1] = element;
			array = temp;
		}
	}

	private static byte[] ReadFromZip(ZipFile zf, ZipEntry ze)
	{
		byte[] buf = new byte[ze.Size];
		int pos = 0;
		Stream s = zf.GetInputStream(ze);
		while(pos < buf.Length)
		{
			pos += s.Read(buf, pos, buf.Length - pos);
		}
		return buf;
	}

	private void AddClassFile(ZipEntry zipEntry, string filename, byte[] buf, bool addResourceFallback, string jar)
	{
		try
		{
			string name = ClassFile.GetClassName(buf, 0, buf.Length);
			if(classes.ContainsKey(name))
			{
				StaticCompiler.IssueMessage(Message.DuplicateClassName, name);
			}
			else
			{
				classes.Add(name, buf);
			}
		}
		catch(ClassFormatError x)
		{
			if(addResourceFallback)
			{
				// not a class file, so we include it as a resource
				// (IBM's db2os390/sqlj jars apparantly contain such files)
				StaticCompiler.IssueMessage(Message.NotAClassFile, filename, x.Message);
				AddResource(zipEntry, filename, buf, jar);
			}
			else
			{
				StaticCompiler.IssueMessage(Message.ClassFormatError, filename, x.Message);
			}
		}
	}

	private void ProcessZipFile(string file, Predicate<ZipEntry> filter)
	{
		string jar = Path.GetFileName(file);
		ZipFile zf = new ZipFile(file);
		try
		{
			foreach(ZipEntry ze in zf)
			{
				if(filter != null && !filter(ze))
				{
					// skip
				}
				else if(ze.IsDirectory)
				{
					AddResource(ze, ze.Name, null, jar);
				}
				else if(ze.Name.ToLower().EndsWith(".class"))
				{
					AddClassFile(ze, ze.Name, ReadFromZip(zf, ze), true, jar);
				}
				else
				{
					// if it's not a class, we treat it as a resource and the manifest
					// is examined to find the Main-Class
					if(ze.Name == "META-INF/MANIFEST.MF" && manifestMainClass == null)
					{
						// read main class from manifest
						// TODO find out if we can use other information from manifest
						StreamReader rdr = new StreamReader(zf.GetInputStream(ze));
						string line;
						while((line = rdr.ReadLine()) != null)
						{
							if(line.StartsWith("Main-Class: "))
							{
								manifestMainClass = line.Substring(12).Replace('/', '.');
								break;
							}
						}
					}
					AddResource(ze, ze.Name, ReadFromZip(zf, ze), jar);
				}
			}
		}
		finally
		{
			zf.Close();
		}
	}

	private void AddResource(ZipEntry zipEntry, string name, byte[] buf, string jar)
	{
		List<ResourceItem> list;
		if (!resources.TryGetValue(name, out list))
		{
			list = new List<ResourceItem>();
			resources.Add(name, list);
		}
		ResourceItem item = new ResourceItem();
		item.zipEntry = zipEntry;
		item.data = buf;
		item.jar = jar ?? "resources.jar";
		list.Add(item);
	}

	private void ProcessFile(DirectoryInfo baseDir, string file)
	{
		switch(new FileInfo(file).Extension.ToLower())
		{
			case ".class":
				using(FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					byte[] buf = new byte[fs.Length];
					fs.Read(buf, 0, buf.Length);
					AddClassFile(null, file, buf, false, null);
				}
				break;
			case ".jar":
			case ".zip":
				try
				{
					ProcessZipFile(file, null);
				}
				catch(ICSharpCode.SharpZipLib.SharpZipBaseException x)
				{
					Console.Error.WriteLine("Warning: error reading {0}: {1}", file, x.Message);
				}
				break;
			default:
			{
				if(baseDir == null)
				{
					Console.Error.WriteLine("Warning: unknown file type: {0}", file);
				}
				else
				{
					// include as resource
					try 
					{
						using(FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
						{
							byte[] b = new byte[fs.Length];
							fs.Read(b, 0, b.Length);
							// extract the resource name by chopping off the base directory
							string name = file.Substring(baseDir.FullName.Length);
							if(name.Length > 0 && name[0] == Path.DirectorySeparatorChar)
							{
								name = name.Substring(1);
							}
							name = name.Replace('\\', '/');
							AddResource(null, name, b, null);
						}
					}
					catch(UnauthorizedAccessException)
					{
						Console.Error.WriteLine("Warning: error reading file {0}: Access Denied", file);
					}
				}
				break;
			}
		}
	}

	private void Recurse(DirectoryInfo baseDir, DirectoryInfo dir, string spec)
	{
		foreach(FileInfo file in dir.GetFiles(spec))
		{
			ProcessFile(baseDir, file.FullName);
		}
		foreach(DirectoryInfo sub in dir.GetDirectories())
		{
			Recurse(baseDir, sub, spec);
		}
	}

	private void RecurseJar(string path)
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
				ProcessZipFile(path, delegate(ZipEntry ze) {
					// MONOBUG Path.GetDirectoryName() doesn't normalize / to \ on Windows
					string name = ze.Name.Replace('/', Path.DirectorySeparatorChar);
					return (Path.GetDirectoryName(name) + Path.DirectorySeparatorChar).StartsWith(pathFilter)
						&& Regex.IsMatch(Path.GetFileName(ze.Name), fileFilter);
				});
				return;
			}
		}
	}

	//This processes an exclusion file with a single regular expression per line
	private static void ProcessExclusionFile(List<string> classesToExclude, String filename)
	{
		try 
		{
			using(StreamReader file = new StreamReader(filename))
			{
				String line;
				while((line = file.ReadLine()) != null)
				{
					line = line.Trim();
					if(!line.StartsWith("//") && line.Length != 0)
					{
						classesToExclude.Add(line);
					}
				}
			}
		} 
		catch(FileNotFoundException) 
		{
			Console.Error.WriteLine("Warning: could not find exclusion file '{0}'", filename);
		}
	}
}
