/*
  Copyright (C) 2002-2008 Jeroen Frijters

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
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using IKVM.Internal;

class IkvmcCompiler
{
#if MULTI_TARGET
	private bool nonleaf;
#endif
	private string manifestMainClass;
	private Dictionary<string, byte[]> classes = new Dictionary<string, byte[]>();
	private Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>();
	private string defaultAssemblyName;
	private List<string> classesToExclude = new List<string>();
	private List<string> references = new List<string>();
	private static bool time;

	private static List<string> GetArgs(string[] args)
	{
		List<string> arglist = new List<string>();
		foreach(string s in args)
		{
			if(s.StartsWith("@"))
			{
				using(StreamReader sr = new StreamReader(s.Substring(1)))
				{
					string line;
					while((line = sr.ReadLine()) != null)
					{
						arglist.Add(line.Trim());
					}
				}
			}
			else
			{
				arglist.Add(s);
			}
		}
		return arglist;
	}

	static int Main(string[] args)
	{
		DateTime start = DateTime.Now;
		AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		System.Threading.Thread.CurrentThread.Name = "compiler";
		Tracer.EnableTraceForDebug();
		List<string> argList = GetArgs(args);
		if (argList.Count == 0)
		{
			PrintHelp();
			return 1;
		}
		IkvmcCompiler comp = new IkvmcCompiler();
		List<CompilerOptions> targets = new List<CompilerOptions>();
		int rc = comp.ParseCommandLine(argList.GetEnumerator(), targets, new CompilerOptions());
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
					rc = CompilerClassLoader.Compile(targets);
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

	static string GetVersionAndCopyrightInfo()
	{
		Assembly asm = Assembly.GetEntryAssembly();
		object[] desc = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
		if (desc.Length == 1)
		{
			object[] copyright = asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			if (copyright.Length == 1)
			{
				return string.Format("{0} version {1}{2}{3}{2}http://www.ikvm.net/",
					((AssemblyTitleAttribute)desc[0]).Title,
					asm.GetName().Version,
					Environment.NewLine,
					((AssemblyCopyrightAttribute)copyright[0]).Copyright);
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
		Console.Error.WriteLine("    -version:<M.m.b.r>         Assembly version");
		Console.Error.WriteLine("    -fileversion:<version>     File version");
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
		Console.Error.WriteLine("    -warnaserror:<warning[:key]>  Treat specified warnings as errors");
		Console.Error.WriteLine("    -time                      Display timing statistics");
		Console.Error.WriteLine("    -classloader:<class>       Set custom class loader class for assembly");
	}

	int ParseCommandLine(IEnumerator<string> arglist, List<CompilerOptions> targets, CompilerOptions options)
	{
		options.target = PEFileKinds.ConsoleApplication;
		options.guessFileKind = true;
		options.version = "0.0.0.0";
		options.apartment = ApartmentState.STA;
		options.props = new Dictionary<string, string>();
		while(arglist.MoveNext())
		{
			string s = arglist.Current;
#if MULTI_TARGET
			if(s == "{")
			{
				nonleaf = true;
				IkvmcCompiler nestedLevel = new IkvmcCompiler();
				nestedLevel.manifestMainClass = manifestMainClass;
				nestedLevel.classes = new Dictionary<string, byte[]>(classes);
				nestedLevel.resources = new Dictionary<string, byte[]>(resources);
				nestedLevel.defaultAssemblyName = defaultAssemblyName;
				nestedLevel.classesToExclude = new List<string>(classesToExclude);
				nestedLevel.references = new List<string>(references);
				int rc = nestedLevel.ParseCommandLine(arglist, targets, options.Copy());
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
			else
#endif
			if(s[0] == '-')
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
					string[] files = new string[0];
					try
					{
						string path = Path.GetDirectoryName(r);
						files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(r));
					}
					catch (ArgumentException)
					{
					}
					catch (IOException)
					{
					}
					if(files.Length == 0)
					{
						Assembly asm = null;
						try
						{
#pragma warning disable 618
							// Assembly.LoadWithPartialName is obsolete
							asm = Assembly.LoadWithPartialName(r);
#pragma warning restore
						}
						catch (FileLoadException)
						{
						}
						if(asm == null)
						{
							Console.Error.WriteLine("Error: reference not found: {0}", r);
							return 1;
						}
						files = new string[] { asm.Location };
					}
					foreach(string f in files)
					{
						references.Add(f);
					}
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
							Recurse(dir, dir, Path.GetFileName(spec));
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
							AddResource(name, b);
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
					options.version = s.Substring(9);
					if(options.version.EndsWith(".*"))
					{
						options.version = options.version.Substring(0, options.version.Length - 1);
						int count = options.version.Split('.').Length;
						// NOTE this is the published algorithm for generating automatic build and revision numbers
						// (see AssemblyVersionAttribute constructor docs), but it turns out that the revision
						// number is off an hour (on my system)...
						DateTime now = DateTime.Now;
						int seconds = (int)(now.TimeOfDay.TotalSeconds / 2);
						int days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
						if(count == 3)
						{
							options.version += days + "." + seconds;
						}
						else if(count == 4)
						{
							options.version += seconds;
						}
						else
						{
							Console.Error.WriteLine("Error: Invalid version specified: {0}*", options.version);
							return 1;
						}
					}
				}
				else if(s.StartsWith("-fileversion:"))
				{
					options.fileversion = s.Substring(13);
				}
				else if(s.StartsWith("-keyfile:"))
				{
					options.keyfilename = s.Substring(9);
				}
				else if(s.StartsWith("-key:"))
				{
					options.keycontainer = s.Substring(5);
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
					if(options.privatePackages == null)
					{
						options.privatePackages = new string[] { prefix };
					}
					else
					{
						string[] temp = new string[options.privatePackages.Length + 1];
						Array.Copy(options.privatePackages, 0, temp, 0, options.privatePackages.Length);
						temp[temp.Length - 1] = prefix;
						options.privatePackages = temp;
					}
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
						StaticCompiler.SuppressWarning(ws);
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
						StaticCompiler.WarnAsError(ws);
					}
				}
				else if(s.StartsWith("-runtime:"))
				{
					// NOTE this is an undocumented option
					options.runtimeAssembly = s.Substring(9);
				}
				else if(s == "-time")
				{
					time = true;
				}
				else if(s.StartsWith("-classloader:"))
				{
					options.classLoader = s.Substring(13);
				}
				else
				{
					Console.Error.WriteLine("Warning: unrecognized option: {0}", s);
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
		}
#if MULTI_TARGET
		if(nonleaf)
		{
			return 0;
		}
#endif
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
			StaticCompiler.IssueMessage(Message.MainMethodFromManifest, manifestMainClass);
			options.mainClass = manifestMainClass;
		}
		options.classes = classes;
		options.references = references.ToArray();
		options.resources = resources;
		options.classesToExclude = classesToExclude.ToArray();
		targets.Add(options);
		return 0;
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

	private void AddClassFile(string filename, byte[] buf, bool addResourceFallback)
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
				AddResource(filename, buf);
			}
			else
			{
				StaticCompiler.IssueMessage(Message.ClassFormatError, filename, x.Message);
			}
		}
	}

	private void ProcessZipFile(string file)
	{
		ZipFile zf = new ZipFile(file);
		try
		{
			foreach(ZipEntry ze in zf)
			{
				if(ze.IsDirectory)
				{
					// skip
				}
				else if(ze.Name.ToLower().EndsWith(".class"))
				{
					AddClassFile(ze.Name, ReadFromZip(zf, ze), true);
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
					AddResource(ze.Name, ReadFromZip(zf, ze));
				}
			}
		}
		finally
		{
			zf.Close();
		}
	}

	private void AddResource(string name, byte[] buf)
	{
		if(resources.ContainsKey(name))
		{
			StaticCompiler.IssueMessage(Message.DuplicateResourceName, name);
		}
		else
		{
			resources.Add(name, buf);
		}
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
					AddClassFile(file, buf, false);
				}
				break;
			case ".jar":
			case ".zip":
				try
				{
					ProcessZipFile(file);
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
							AddResource(name, b);
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

	private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		// make sure all the referenced assemblies are visible (they are loaded with LoadFrom, so
		// they end up in the LoadFrom context [unless they happen to be available in one of the probe paths])
		foreach(Assembly a in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
		{
			if(args.Name.StartsWith(a.GetName().Name + ", "))
			{
				return a;
			}
		}
		return Assembly.ReflectionOnlyLoad(args.Name);
	}
}
