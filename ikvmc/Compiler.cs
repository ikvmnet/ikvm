/*
  Copyright (C) 2002 Jeroen Frijters

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
using java.io;
using java.util.zip;

class Compiler : MarshalByRefObject
{
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

	static int Main(string[] args)
	{
		System.Reflection.Emit.PEFileKinds target = System.Reflection.Emit.PEFileKinds.ConsoleApplication;
		string assemblyname = null;
		string outputfile = null;
		string main = null;
		bool nojni = false;
		ArrayList references = new ArrayList();
		ArrayList arglist = GetArgs(args);
		if(arglist.Count == 0)
		{
			Console.Error.WriteLine("usage: ikvmc [-options] <classOrJar1> ... <classOrJarN>");
			Console.Error.WriteLine();
			Console.Error.WriteLine("options:");
			Console.Error.WriteLine("    -out:<outputfile>       Required");
			Console.Error.WriteLine("    -assembly:<outputfile>  Optionally used to specify assembly name");
			Console.Error.WriteLine("    -target:exe             Build a console executable");
			Console.Error.WriteLine("    -target:winexe          Build a windows executable");
			Console.Error.WriteLine("    -target:library         Build a library");
			Console.Error.WriteLine("    -main:<class>           Required (for executables)");
			Console.Error.WriteLine("    -reference:<path>       Reference an assembly");
			Console.Error.WriteLine("    -recurse:<filespec>     Recurse directory and include matching files");
			Console.Error.WriteLine("    -nojni                  Do not generate JNI stub for native methods");
			return 1;
		}
		ArrayList classes = new ArrayList();
		Hashtable resources = new Hashtable();
		foreach(string s in arglist)
		{
			if(s[0] == '-')
			{
				if(s.StartsWith("-out:"))
				{
					outputfile = s.Substring(5);
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
							break;
						case "-target:winexe":
							target = System.Reflection.Emit.PEFileKinds.WindowApplication;
							break;
						case "-target:library":
							target = System.Reflection.Emit.PEFileKinds.Dll;
							break;
					}
				}
				else if(s.StartsWith("-main:"))
				{
					main = s.Substring(6);
				}
				else if(s.StartsWith("-reference:"))
				{
					references.Add(s.Substring(11));
				}
				else if(s.StartsWith("-recurse:"))
				{
					string spec = s.Substring(9);
					Recurse(classes, resources, new DirectoryInfo(Path.GetDirectoryName(spec)), Path.GetFileName(spec));
				}
				else if(s == "-nojni")
				{
					nojni = true;
				}
				else
				{
					Console.Error.WriteLine("Warning: Unrecognized option: {0}", s);
				}
			}
			else
			{
				ProcessFile(classes, resources, s);
			}
		}
		if(outputfile == null)
		{
			Console.Error.WriteLine("Error: -out:<outputfile> must be specified");
			return 1;
		}
		if(assemblyname == null)
		{
			int idx = outputfile.LastIndexOf('.');
			if(idx > 0)
			{
				assemblyname = outputfile.Substring(0, idx);
			}
			else
			{
				assemblyname = outputfile;
			}
		}
		if(target != System.Reflection.Emit.PEFileKinds.Dll && main == null)
		{
			Console.Error.WriteLine("Error: -main:<class> must be specified when creating an executable");
			return 1;
		}
		// HACK since we use Classpath's zip code, our VM is already running in this AppDomain, which means that
		// it cannot be (ab)used to statically compile anymore, so we create a new AppDomain to run the compile in.
		Compiler c = (Compiler)AppDomain.CreateDomain("Compiler").CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, "Compiler");
		return c.Compile(outputfile, assemblyname, main, target, (byte[][])classes.ToArray(typeof(byte[])), (string[])references.ToArray(typeof(string)), nojni, resources);
	}

	private static void ProcessFile(ArrayList classes, Hashtable resources, string file)
	{
		if(file.ToLower().EndsWith(".class"))
		{
			using(FileStream fs = new FileStream(file, FileMode.Open))
			{
				byte[] b = new byte[fs.Length];
				fs.Read(b, 0, b.Length);
				classes.Add(b);
			}
		}
		else if(file.ToLower().EndsWith(".jar") || file.ToLower().EndsWith(".zip"))
		{
			ZipFile zf = new ZipFile(file);
			java.util.Enumeration e = zf.entries();
			while(e.hasMoreElements())
			{
				ZipEntry ze = (ZipEntry)e.nextElement();
				if(ze.getName().ToLower().EndsWith(".class"))
				{
					sbyte[] sbuf = new sbyte[ze.getSize()];
					DataInputStream dis = new DataInputStream(zf.getInputStream(ze));
					dis.readFully(sbuf);
					dis.close();
					byte[] buf = new byte[sbuf.Length];
					for(int i = 0; i < buf.Length; i++)
					{
						buf[i] = (byte)sbuf[i];
					}
					classes.Add(buf);
				}
				else
				{
					// if it's not a class, we treat it as a resource
					sbyte[] sbuf = new sbyte[ze.getSize()];
					DataInputStream dis = new DataInputStream(zf.getInputStream(ze));
					dis.readFully(sbuf);
					dis.close();
					byte[] buf = new byte[sbuf.Length];
					for(int i = 0; i < buf.Length; i++)
					{
						buf[i] = (byte)sbuf[i];
					}
					resources.Add(ze.getName(), buf);
				}
			}
		}
		else
		{
			Console.Error.WriteLine("Warning: Unknown file type: {0}", file);
		}
	}

	private static void Recurse(ArrayList classes, Hashtable resources, DirectoryInfo dir, string spec)
	{
		foreach(FileInfo file in dir.GetFiles(spec))
		{
			ProcessFile(classes, resources, file.FullName);
		}
		foreach(DirectoryInfo sub in dir.GetDirectories())
		{
			Recurse(classes, resources, sub, spec);
		}
	}

	public int Compile(string fileName, string assemblyName, string mainClass, System.Reflection.Emit.PEFileKinds target, byte[][] classes, string[] references, bool nojni, Hashtable resources)
	{
		try
		{
			JVM.Compile(fileName, assemblyName, mainClass, target, classes, references, nojni, resources);
			return 0;
		}
		catch(Exception x)
		{
			Console.Error.WriteLine(x);
			return 1;
		}
	}
}
