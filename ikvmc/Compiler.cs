/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using System.Collections;
using System.IO;
using System.Reflection;
using ICSharpCode.SharpZipLib.Zip;

class Compiler
{
	private static string manifestMainClass;
	private static int itemsProcessed;
	private static ArrayList classes = new ArrayList();
	private static Hashtable resources = new Hashtable();

	private static ArrayList GetArgs(string[] args)
	{
		ArrayList arglist = new ArrayList();
		foreach(string s in args)
		{
			if(s.StartsWith("@"))
			{
				using(StreamReader sr = new StreamReader(s.Substring(1)))
				{
					string line;
					while((line = sr.ReadLine()) != null)
					{
						arglist.Add(line);
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

	static void Main(string[] args)
	{
		// FXBUG if we run a static initializer that starts a thread, we would never end,
		// so we force an exit here
		Environment.Exit(RealMain(args));
	}

	static int RealMain(string[] args)
	{
		AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		System.Threading.Thread.CurrentThread.Name = "compiler";
		Tracer.EnableTraceForDebug();
		System.Reflection.Emit.PEFileKinds target = System.Reflection.Emit.PEFileKinds.ConsoleApplication;
		bool guessFileKind = true;
		string assemblyname = null;
		string outputfile = null;
		string version = "0.0.0.0";
		string keyfile = null;
		bool targetIsModule = false;
		string main = null;
		string defaultAssemblyName = null;
		string remapfile = null;
		bool nojni = false;
		ArrayList classesToExclude = new ArrayList();
		ArrayList references = new ArrayList();
		ArrayList arglist = GetArgs(args);
		if(arglist.Count == 0)
		{
			Console.Error.WriteLine("usage: ikvmc [-options] <classOrJar1> ... <classOrJarN>");
			Console.Error.WriteLine();
			Console.Error.WriteLine("options:");
			Console.Error.WriteLine("    -out:<outputfile>       Specify the output filename");
			Console.Error.WriteLine("    -assembly:<name>        Specify assembly name");
			Console.Error.WriteLine("    -target:exe             Build a console executable");
			Console.Error.WriteLine("    -target:winexe          Build a windows executable");
			Console.Error.WriteLine("    -target:library         Build a library");
			Console.Error.WriteLine("    -target:module          Build a module for use by the linker");
			Console.Error.WriteLine("    -keyfile:keyfilename    Use keyfile to sign the assembly");
			Console.Error.WriteLine("    -version:M.m.b.r        Assembly version");
			Console.Error.WriteLine("    -main:<class>           Specify the class containing the main method");
			Console.Error.WriteLine("    -reference:<filespec>   Reference an assembly");
			Console.Error.WriteLine("    -recurse:<filespec>     Recurse directory and include matching files");
			Console.Error.WriteLine("    -nojni                  Do not generate JNI stub for native methods");
			Console.Error.WriteLine("    -resource:<name>=<path> Include file as Java resource");
			Console.Error.WriteLine("    -exclude:<filename>     A file containing a list of classes to exclude");
			Console.Error.WriteLine("    -debug                  Creates debugging information for the output file");
			Console.Error.WriteLine("    -srcpath:<path>         Prepend path and package name to source file");
			Console.Error.WriteLine("    -Xtrace:<string>        Displays all tracepoints with the given name");
			Console.Error.WriteLine("    -Xmethodtrace:<string>  Builds method trace into the specified output methods");
			return 1;
		}
		foreach(string s in arglist)
		{
			if(s[0] == '-')
			{
				if(s.StartsWith("-out:"))
				{
					outputfile = s.Substring(5);
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
					assemblyname = s.Substring(10);
				}
				else if(s.StartsWith("-target:"))
				{
					switch(s)
					{
						case "-target:exe":
							target = System.Reflection.Emit.PEFileKinds.ConsoleApplication;
							guessFileKind = false;
							break;
						case "-target:winexe":
							target = System.Reflection.Emit.PEFileKinds.WindowApplication;
							guessFileKind = false;
							break;
						case "-target:module":
							targetIsModule = true;
							target = System.Reflection.Emit.PEFileKinds.Dll;
							guessFileKind = false;
							break;
						case "-target:library":
							target = System.Reflection.Emit.PEFileKinds.Dll;
							guessFileKind = false;
							break;
						default:
							Console.Error.WriteLine("Warning: unrecognized option: {0}", s);
							break;
					}
				}
				else if(s.StartsWith("-main:"))
				{
					main = s.Substring(6);
				}
				else if(s.StartsWith("-reference:"))
				{
					string r = s.Substring(11);
					string path = Path.GetDirectoryName(r);
					string[] files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(r));
					if(files.Length == 0)
					{
						Console.Error.WriteLine("Error: reference not found: {0}", r);
						return 1;
					}
					foreach(string f in files)
					{
						references.Add(f);
					}
				}
				else if(s.StartsWith("-recurse:"))
				{
					string spec = s.Substring(9);
					if(Directory.Exists(spec))
					{
						// NOTE we're appending a DirectorySeparatorChar to make sure that dir.FullName always
						// ends with a separator (multiple separators are automatically collapsed into one).
						// This is important because later on we will be using baseDir.FullName to make
						// an absolute path relative again.
						DirectoryInfo dir = new DirectoryInfo(spec + Path.DirectorySeparatorChar);
						Recurse(dir, dir, "*");
					}
					else
					{
						// see comment above about Path.DirectorySeparatorChar
						DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(spec) + Path.DirectorySeparatorChar);
						Recurse(dir, dir, Path.GetFileName(spec));
					}
				}
				else if(s.StartsWith("-resource:"))
				{
					string[] spec = s.Substring(10).Split('=');
					if(resources.ContainsKey(spec[0]))
					{
						Console.Error.WriteLine("Warning: skipping resource (name clash): " + spec[0]);
					}
					else
					{
						using(FileStream fs = new FileStream(spec[1], FileMode.Open))
						{
							byte[] b = new byte[fs.Length];
							fs.Read(b, 0, b.Length);
							resources.Add(spec[0], b);
						}
					}
				}
				else if(s == "-nojni")
				{
					nojni = true;
				}
				else if(s.StartsWith("-exclude:"))
				{
					ProcessExclusionFile(classesToExclude, s.Substring(9));
				}
				else if(s.StartsWith("-version:"))
				{
					version = s.Substring(9);
				}
				else if(s.StartsWith("-keyfile:"))
				{
					keyfile = s.Substring(9);
				}
				else if(s == "-debug")
				{
					JVM.Debug = true;
				}
				else if(s.StartsWith("-srcpath:"))
				{
					JVM.SourcePath = s.Substring(9);
				}
				else if(s.StartsWith("-remap:"))
				{
					remapfile = s.Substring(7);
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
				string path = Path.GetDirectoryName(s);
				string[] files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(s));
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
		if(itemsProcessed == 0)
		{
			Console.Error.WriteLine("Error: at least one class or jar must be specified");
			return 1;
		}
		if(assemblyname == null)
		{
			string basename = outputfile == null ? defaultAssemblyName : new FileInfo(outputfile).Name;
			int idx = basename.LastIndexOf('.');
			if(idx > 0)
			{
				assemblyname = basename.Substring(0, idx);
			}
			else
			{
				assemblyname = basename;
			}
		}
		if(outputfile != null && guessFileKind)
		{
			if(outputfile.ToLower().EndsWith(".dll"))
			{
				target = System.Reflection.Emit.PEFileKinds.Dll;
			}
			guessFileKind = false;
		}
		if(main == null && manifestMainClass != null && (guessFileKind || target != System.Reflection.Emit.PEFileKinds.Dll))
		{
			Console.Error.WriteLine("Note: using main class {0} based on jar manifest", manifestMainClass);
			main = manifestMainClass;
		}
		try
		{
			JVM.Compile(outputfile, keyfile, version, targetIsModule, assemblyname, main, target, guessFileKind, (byte[][])classes.ToArray(typeof(byte[])), (string[])references.ToArray(typeof(string)), nojni, resources, (string[])classesToExclude.ToArray(typeof(string)), remapfile);
			return 0;
		}
		catch(Exception x)
		{
			Console.Error.WriteLine(x);
			return 1;
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

	private static void ProcessZipFile(string file)
	{
		ZipFile zf = new ZipFile(file);
		try
		{
			foreach(ZipEntry ze in zf)
			{
				if(ze.Name.ToLower().EndsWith(".class"))
				{
					classes.Add(ReadFromZip(zf, ze));
					itemsProcessed++;
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
								manifestMainClass = line.Substring(12);
								break;
							}
						}
					}
					if(resources.ContainsKey(ze.Name))
					{
						Console.Error.WriteLine("Warning: skipping resource (name clash): " + ze.Name);
					}
					else
					{
						resources.Add(ze.Name, ReadFromZip(zf, ze));
						itemsProcessed++;
					}
				}
			}
		}
		finally
		{
			zf.Close();
		}
	}

	private static void ProcessFile(DirectoryInfo baseDir, string file)
	{
		switch(new FileInfo(file).Extension.ToLower())
		{
			case ".class":
				using(FileStream fs = new FileStream(file, FileMode.Open))
				{
					byte[] b = new byte[fs.Length];
					fs.Read(b, 0, b.Length);
					itemsProcessed++;
					classes.Add(b);
				}
				break;
			case ".jar":
			case ".zip":
				try
				{
					ProcessZipFile(file);
				}
				catch(ICSharpCode.SharpZipLib.ZipException x)
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
						using(FileStream fs = new FileStream(file, FileMode.Open))
						{
							byte[] b = new byte[fs.Length];
							fs.Read(b, 0, b.Length);
							// HACK very lame way to extract the resource name (by chopping off the base directory)
							string name = file.Substring(baseDir.FullName.Length);
							name = name.Replace('\\', '/');
							resources.Add(name, b);
							itemsProcessed++;
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

	private static void Recurse(DirectoryInfo baseDir, DirectoryInfo dir, string spec)
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
	private static void ProcessExclusionFile(ArrayList classesToExclude, String filename)
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
		foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			if(a.FullName == args.Name)
			{
				return a;
			}
		}
		return null;
	}
}
