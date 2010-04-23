/*
  Copyright (C) 2010 Jeroen Frijters

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
using System.Text;
using System.IO;
using IKVM.Reflection;

namespace IKVM.Internal
{
	sealed class AssemblyResolver
	{
		private readonly List<string> libpath = new List<string>();
		private Universe universe;

		internal int Init(Universe universe, bool nostdlib, IList<string> references, IList<string> userLibPaths)
		{
			this.universe = universe;
			// like the C# compiler, the references are loaded from:
			// current directory, CLR directory, -lib: option, %LIB% environment
			// (note that, unlike the C# compiler, we don't add the CLR directory if -nostdlib has been specified)
			libpath.Add(Environment.CurrentDirectory);
			if (!nostdlib)
			{
				libpath.Add(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
			}
			foreach (string str in userLibPaths)
			{
				AddLibraryPaths(str, "-lib option");
			}
			AddLibraryPaths(Environment.GetEnvironmentVariable("LIB") ?? "", "LIB environment");
			int rc = 0;
			if (nostdlib)
			{
				rc = LoadMscorlib(references);
			}
			if (rc == 0)
			{
				universe.AssemblyResolve += new IKVM.Reflection.ResolveEventHandler(universe_AssemblyResolve);
			}
			return rc;
		}

		internal Assembly LoadWithPartialName(string name)
		{
			foreach (string path in FindAssemblyPath(name + ".dll"))
			{
				return universe.LoadFile(path);
			}
			return null;
		}

		internal int ResolveReference(Dictionary<string, Assembly> cache, ref Assembly[] references, string reference)
		{
			string[] files = new string[0];
			try
			{
				string path = Path.GetDirectoryName(reference);
				files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(reference));
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			if (files.Length == 0)
			{
				Assembly asm = null;
				cache.TryGetValue(reference, out asm);
				try
				{
					if (asm == null)
					{
						foreach (string found in FindAssemblyPath(reference))
						{
							asm = StaticCompiler.LoadFile(found);
							cache.Add(reference, asm);
							break;
						}
					}
				}
				catch (FileLoadException)
				{
				}
				if (asm == null)
				{
					Console.Error.WriteLine("Error: reference not found: {0}", reference);
					return 1;
				}
				ArrayAppend(ref references, asm);
			}
			else
			{
				foreach (string file in files)
				{
					try
					{
						Assembly asm;
						if (!cache.TryGetValue(file, out asm))
						{
							asm = StaticCompiler.LoadFile(file);
						}
						ArrayAppend(ref references, asm);
					}
					catch (FileLoadException)
					{
						Console.Error.WriteLine("Error: reference not found: {0}", file);
						return 1;
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

		private Assembly universe_AssemblyResolve(object sender, IKVM.Reflection.ResolveEventArgs args)
		{
			// to support Universe.GetType("System.Object, mscorlib"), we have to support partial names
			// (the map.xml file contains such type names)
			bool partialName = !args.Name.Contains(",");
			AssemblyName name = new AssemblyName(args.Name);
			foreach (string file in FindAssemblyPath(name.Name + ".dll"))
			{
				Assembly asm = StaticCompiler.LoadFile(file);
				if (Matches(asm.GetName(), name) || partialName)
				{
					return asm;
				}
			}
			if (args.RequestingAssembly != null)
			{
				string path = Path.Combine(Path.GetDirectoryName(args.RequestingAssembly.Location), name.Name + ".dll");
				if (File.Exists(path) && Matches(AssemblyName.GetAssemblyName(path), name))
				{
					return StaticCompiler.LoadFile(path);
				}
			}
			Console.Error.WriteLine("Error: unable to find assembly '{0}'", args.Name);
			if (args.RequestingAssembly != null)
			{
				Console.Error.WriteLine("    (a dependency of '{0}')", args.RequestingAssembly.FullName);
			}
			Environment.Exit(1);
			return null;
		}

		private static bool Matches(AssemblyName assemblyDef, AssemblyName assemblyRef)
		{
			if (assemblyDef.Name != assemblyRef.Name)
			{
				return false;
			}
			bool strongNamed = IsStrongNamed(assemblyDef);
			if (strongNamed != IsStrongNamed(assemblyRef))
			{
				return false;
			}
			if (strongNamed)
			{
				return IsEqual(assemblyDef.GetPublicKeyToken(), assemblyRef.GetPublicKeyToken()) && assemblyDef.Version >= assemblyRef.Version;
			}
			return true;
		}

		private static bool IsStrongNamed(AssemblyName name)
		{
			byte[] key = name.GetPublicKeyToken();
			return key != null && key.Length != 0;
		}

		private static bool IsEqual(byte[] b1, byte[] b2)
		{
			if (b1.Length != b2.Length)
			{
				return false;
			}
			for (int i = 0; i < b1.Length; i++)
			{
				if (b1[i] != b2[i])
				{
					return false;
				}
			}
			return true;
		}

		private void AddLibraryPaths(string str, string msg)
		{
			foreach (string dir in str.Split(Path.PathSeparator))
			{
				if (Directory.Exists(dir))
				{
					libpath.Add(dir);
				}
				else if (dir != "")
				{
					Console.Error.WriteLine("Warning: directory '{0}' specified in {1} is not valid", dir, msg);
				}
			}
		}

		private int LoadMscorlib(IList<string> references)
		{
			if (references != null)
			{
				Universe dummy = new Universe();
				foreach (string r in references)
				{
					try
					{
						Assembly asm = dummy.LoadFile(r);
						if (asm.GetType("System.Object") != null)
						{
							StaticCompiler.Universe.LoadMscorlib(r);
							return 0;
						}
					}
					catch
					{
					}
				}
			}
			foreach (string mscorlib in FindAssemblyPath("mscorlib.dll"))
			{
				StaticCompiler.Universe.LoadMscorlib(mscorlib);
				return 0;
			}
			Console.Error.WriteLine("Error: unable to find mscorlib.dll");
			return 1;
		}

		private IEnumerable<string> FindAssemblyPath(string file)
		{
			if (Path.IsPathRooted(file))
			{
				if (File.Exists(file))
				{
					yield return file;
				}
			}
			else
			{
				foreach (string dir in libpath)
				{
					string path = Path.Combine(dir, file);
					if (File.Exists(path))
					{
						yield return path;
					}
					// for legacy compat, we try again after appending .dll
					path = Path.Combine(dir, file + ".dll");
					if (File.Exists(path))
					{
						Console.WriteLine("Warning: Found assembly '{0}' using legacy search rule. Please append '.dll' to the reference.", file);
						yield return path;
					}
				}
			}
		}
	}
}
