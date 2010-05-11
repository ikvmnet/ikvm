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
		private Version mscorlibVersion;

		internal delegate void HigherVersionEvent(AssemblyName assemblyDef, AssemblyName assemblyRef);
		internal event HigherVersionEvent HigherVersion;

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
				mscorlibVersion = universe.Load("mscorlib").GetName().Version;
				universe.AssemblyResolve += new IKVM.Reflection.ResolveEventHandler(universe_AssemblyResolve);
			}
			return rc;
		}

		internal Assembly LoadFile(string path)
		{
			Assembly asm = universe.LoadFile(path);
			// to avoid problems (i.e. weird exceptions), we don't allow assemblies to load that reference a newer version of mscorlib
			foreach (AssemblyName asmref in asm.GetReferencedAssemblies())
			{
				if (asmref.Name == "mscorlib" && asmref.Version > mscorlibVersion)
				{
					Console.Error.WriteLine("Error: unable to load assembly '{0}' as it depends on a higher version of mscorlib than the one currently loaded", path);
					Environment.Exit(1);
				}
			}
			return asm;
		}

		internal Assembly LoadWithPartialName(string name)
		{
			foreach (string path in FindAssemblyPath(name + ".dll"))
			{
				return LoadFile(path);
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
							asm = LoadFile(found);
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
							asm = LoadFile(file);
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
			AssemblyName previousMatch = null;
			int previousMatchLevel = 0;
			foreach (Assembly asm in universe.GetAssemblies())
			{
				if (partialName && asm.GetName().Name == name.Name)
				{
					return asm;
				}
				if (Match(asm.GetName(), name, ref previousMatch, ref previousMatchLevel))
				{
					return asm;
				}
			}
			foreach (string file in FindAssemblyPath(name.Name + ".dll"))
			{
				if (partialName || Match(AssemblyName.GetAssemblyName(file), name, ref previousMatch, ref previousMatchLevel))
				{
					return LoadFile(file);
				}
			}
			if (args.RequestingAssembly != null)
			{
				string path = Path.Combine(Path.GetDirectoryName(args.RequestingAssembly.Location), name.Name + ".dll");
				if (File.Exists(path) && Match(AssemblyName.GetAssemblyName(path), name, ref previousMatch, ref previousMatchLevel))
				{
					return LoadFile(path);
				}
			}
			if (previousMatch != null)
			{
				if (previousMatchLevel == 2)
				{
					if (HigherVersion != null)
					{
						HigherVersion(previousMatch, name);
					}
					return LoadFile(new Uri(previousMatch.CodeBase).LocalPath);
				}
				else if (args.RequestingAssembly != null)
				{
					Console.Error.WriteLine("Error: Assembly '{0}' uses '{1}' which has a higher version than referenced assembly '{2}'", args.RequestingAssembly.FullName, name.FullName, previousMatch.FullName);
				}
				else
				{
					Console.Error.WriteLine("Error: Assembly '{0}' was requested which is a higher version than referenced assembly '{1}'", name.FullName, previousMatch.FullName);
				}
			}
			else
			{
				Console.Error.WriteLine("Error: unable to find assembly '{0}'", args.Name);
				if (args.RequestingAssembly != null)
				{
					Console.Error.WriteLine("    (a dependency of '{0}')", args.RequestingAssembly.FullName);
				}
			}
			Environment.Exit(1);
			return null;
		}

		// TODO this method should be based on Fusion's CompareAssemblyIdentity (which we should have an equivalent of in Universe)
		private static bool Match(AssemblyName assemblyDef, AssemblyName assemblyRef, ref AssemblyName bestMatch, ref int bestMatchLevel)
		{
			// Match levels:
			//   0 = no match
			//   1 = lower version match (i.e. not a suitable match, but used in error reporting: something was found but the version was too low)
			//   2 = higher version potential match (i.e. we can use this version, but if it is available the exact match will be preferred)
			//
			// If we find a perfect match, bestMatch is not changed but we return true to signal that the search can end right now. 
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
				if (!IsEqual(assemblyDef.GetPublicKeyToken(), assemblyRef.GetPublicKeyToken()))
				{
					return false;
				}
				if (assemblyDef.Version < assemblyRef.Version)
				{
					if (bestMatchLevel < 1)
					{
						bestMatchLevel = 1;
						bestMatch = assemblyDef;
					}
					return false;
				}
				else if (assemblyDef.Version > assemblyRef.Version)
				{
					if (bestMatchLevel < 2)
					{
						bestMatchLevel = 2;
						bestMatch = assemblyDef;
					}
					return false;
				}
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
				foreach (string r in references)
				{
					try
					{
						if (AssemblyName.GetAssemblyName(r).Name == "mscorlib")
						{
							universe.LoadMscorlib(r);
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
				universe.LoadMscorlib(mscorlib);
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
