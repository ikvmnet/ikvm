/*
  Copyright (C) 2010-2013 Jeroen Frijters

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

		internal enum WarningId
		{
			HigherVersion = 1,
			LocationIgnored = 2,
			InvalidLibDirectoryOption = 3,
			InvalidLibDirectoryEnvironment = 4,
			LegacySearchRule = 5,
		}

		internal delegate void WarningEvent(WarningId warning, string message, string[] parameters);
		internal event WarningEvent Warning;

		private void EmitWarning(WarningId warning, string message, params string[] parameters)
		{
			if (Warning != null)
			{
				Warning(warning, message, parameters);
			}
			else
			{
				Console.Error.WriteLine("Warning: " + message, parameters);
			}
		}

		internal void Init(Universe universe, bool nostdlib, IList<string> references, IList<string> userLibPaths)
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
				AddLibraryPaths(str, true);
			}
			AddLibraryPaths(Environment.GetEnvironmentVariable("LIB") ?? "", false);
			if (nostdlib)
			{
				 mscorlibVersion = LoadMscorlib(references).GetName().Version;
			}
			else
			{
				mscorlibVersion = universe.Load("mscorlib").GetName().Version;
			}
#if STATIC_COMPILER
			universe.AssemblyResolve += AssemblyResolve;
#else
			universe.AssemblyResolve += LegacyAssemblyResolve;
#endif
		}

		internal Assembly LoadFile(string path)
		{
			string ex = null;
			try
			{
				using (RawModule module = universe.OpenRawModule(path))
				{
					if (mscorlibVersion != null)
					{
						// to avoid problems (i.e. weird exceptions), we don't allow assemblies to load that reference a newer version of mscorlib
						foreach (AssemblyName asmref in module.GetReferencedAssemblies())
						{
							if (asmref.Name == "mscorlib" && asmref.Version > mscorlibVersion)
							{
								Console.Error.WriteLine("Error: unable to load assembly '{0}' as it depends on a higher version of mscorlib than the one currently loaded", path);
								Environment.Exit(1);
							}
						}
					}
					Assembly asm = universe.LoadAssembly(module);
					if (asm.Location != module.Location && CanonicalizePath(asm.Location) != CanonicalizePath(module.Location))
					{
						EmitWarning(WarningId.LocationIgnored, "assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identity \"{2}\"", path, asm.Location, asm.FullName);
					}
					return asm;
				}
			}
			catch (IOException x)
			{
				ex = x.Message;
			}
			catch (UnauthorizedAccessException x)
			{
				ex = x.Message;
			}
			catch (IKVM.Reflection.BadImageFormatException x)
			{
				ex = x.Message;
			}
			Console.Error.WriteLine("Error: unable to load assembly '{0}'" + Environment.NewLine + "    ({1})", path, ex);
			Environment.Exit(1);
			return null;
		}

		private static string CanonicalizePath(string path)
		{
			try
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(path);
				if (fi.DirectoryName == null)
				{
					return path.Length > 1 && path[1] == ':' ? path.ToUpper() : path;
				}
				string dir = CanonicalizePath(fi.DirectoryName);
				string name = fi.Name;
				try
				{
					string[] arr = System.IO.Directory.GetFileSystemEntries(dir, name);
					if (arr.Length == 1)
					{
						name = arr[0];
					}
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				return System.IO.Path.Combine(dir, name);
			}
			catch (System.UnauthorizedAccessException)
			{
			}
			catch (System.IO.IOException)
			{
			}
			catch (System.Security.SecurityException)
			{
			}
			catch (System.NotSupportedException)
			{
			}
			return path;
		}

		internal Assembly LoadWithPartialName(string name)
		{
			foreach (string path in FindAssemblyPath(name + ".dll"))
			{
				return LoadFile(path);
			}
			return null;
		}

		internal bool ResolveReference(Dictionary<string, Assembly> cache, ref Assembly[] references, string reference)
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
				if (asm == null)
				{
					foreach (string found in FindAssemblyPath(reference))
					{
						asm = LoadFile(found);
						cache.Add(reference, asm);
						break;
					}
				}
				if (asm == null)
				{
					return false;
				}
				ArrayAppend(ref references, asm);
			}
			else
			{
				foreach (string file in files)
				{
					Assembly asm;
					if (!cache.TryGetValue(file, out asm))
					{
						asm = LoadFile(file);
					}
					ArrayAppend(ref references, asm);
				}
			}
			return true;
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

		private Assembly AssemblyResolve(object sender, IKVM.Reflection.ResolveEventArgs args)
		{
			AssemblyName name = new AssemblyName(args.Name);
			AssemblyName previousMatch = null;
			int previousMatchLevel = 0;
			foreach (Assembly asm in universe.GetAssemblies())
			{
				if (Match(asm.GetName(), name, ref previousMatch, ref previousMatchLevel))
				{
					return asm;
				}
			}
			if (previousMatch != null)
			{
				if (previousMatchLevel == 2)
				{
					EmitWarning(WarningId.HigherVersion, "assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime policy", previousMatch.FullName, name.FullName);
					return universe.Load(previousMatch.FullName);
				}
				else if (args.RequestingAssembly != null)
				{
					Console.Error.WriteLine("Error: Assembly '{0}' uses '{1}' which has a higher version than referenced assembly '{2}'", args.RequestingAssembly.FullName, name.FullName, previousMatch.FullName);
					Environment.Exit(1);
					return null;
				}
				else
				{
					Console.Error.WriteLine("Error: Assembly '{0}' was requested which is a higher version than referenced assembly '{1}'", name.FullName, previousMatch.FullName);
					Environment.Exit(1);
					return null;
				}
			}
			else if (args.RequestingAssembly != null)
			{
				return universe.CreateMissingAssembly(args.Name);
			}
			else
			{
				return null;
			}
		}

		private Assembly LegacyAssemblyResolve(object sender, IKVM.Reflection.ResolveEventArgs args)
		{
			return LegacyLoad(new AssemblyName(args.Name), args.RequestingAssembly);
		}
	
		internal Assembly LegacyLoad(AssemblyName name, Assembly requestingAssembly)
		{
			AssemblyName previousMatch = null;
			int previousMatchLevel = 0;
			foreach (Assembly asm in universe.GetAssemblies())
			{
				if (Match(asm.GetName(), name, ref previousMatch, ref previousMatchLevel))
				{
					return asm;
				}
			}
			foreach (string file in FindAssemblyPath(name.Name + ".dll"))
			{
				if (Match(AssemblyName.GetAssemblyName(file), name, ref previousMatch, ref previousMatchLevel))
				{
					return LoadFile(file);
				}
			}
			if (requestingAssembly != null)
			{
				string path = Path.Combine(Path.GetDirectoryName(requestingAssembly.Location), name.Name + ".dll");
				if (File.Exists(path) && Match(AssemblyName.GetAssemblyName(path), name, ref previousMatch, ref previousMatchLevel))
				{
					return LoadFile(path);
				}
			}
			if (previousMatch != null)
			{
				if (previousMatchLevel == 2)
				{
					EmitWarning(WarningId.HigherVersion, "assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime policy", previousMatch.FullName, name.FullName);
					return LoadFile(new Uri(previousMatch.CodeBase).LocalPath);
				}
				else if (requestingAssembly != null)
				{
					Console.Error.WriteLine("Error: Assembly '{0}' uses '{1}' which has a higher version than referenced assembly '{2}'", requestingAssembly.FullName, name.FullName, previousMatch.FullName);
				}
				else
				{
					Console.Error.WriteLine("Error: Assembly '{0}' was requested which is a higher version than referenced assembly '{1}'", name.FullName, previousMatch.FullName);
				}
			}
			else
			{
#if STUB_GENERATOR
				return universe.CreateMissingAssembly(name.FullName);
#else
				Console.Error.WriteLine("Error: unable to find assembly '{0}'", name.FullName);
				if (requestingAssembly != null)
				{
					Console.Error.WriteLine("    (a dependency of '{0}')", requestingAssembly.FullName);
				}
#endif
			}
			Environment.Exit(1);
			return null;
		}

		private bool Match(AssemblyName assemblyDef, AssemblyName assemblyRef, ref AssemblyName bestMatch, ref int bestMatchLevel)
		{
			// Match levels:
			//   0 = no match
			//   1 = lower version match (i.e. not a suitable match, but used in error reporting: something was found but the version was too low)
			//   2 = higher version potential match (i.e. we can use this version, but if it is available the exact match will be preferred)
			//
			// If we find a perfect match, bestMatch is not changed but we return true to signal that the search can end right now.
			AssemblyComparisonResult result;
			universe.CompareAssemblyIdentity(assemblyRef.FullName, false, assemblyDef.FullName, true, out result);
			switch (result)
			{
				case AssemblyComparisonResult.EquivalentFullMatch:
				case AssemblyComparisonResult.EquivalentPartialMatch:
				case AssemblyComparisonResult.EquivalentFXUnified:
				case AssemblyComparisonResult.EquivalentPartialFXUnified:
				case AssemblyComparisonResult.EquivalentPartialWeakNamed:
				case AssemblyComparisonResult.EquivalentWeakNamed:
					return true;
				case AssemblyComparisonResult.NonEquivalentPartialVersion:
				case AssemblyComparisonResult.NonEquivalentVersion:
					if (bestMatchLevel < 1)
					{
						bestMatchLevel = 1;
						bestMatch = assemblyDef;
					}
					return false;
				case AssemblyComparisonResult.EquivalentUnified:
				case AssemblyComparisonResult.EquivalentPartialUnified:
					if (bestMatchLevel < 2)
					{
						bestMatchLevel = 2;
						bestMatch = assemblyDef;
					}
					return false;
				case AssemblyComparisonResult.NonEquivalent:
				case AssemblyComparisonResult.Unknown:
					return false;
				default:
					throw new NotImplementedException();
			}
		}

		private void AddLibraryPaths(string str, bool option)
		{
			foreach (string dir in str.Split(Path.PathSeparator))
			{
				if (Directory.Exists(dir))
				{
					libpath.Add(dir);
				}
				else if (dir != "")
				{
					if (option)
					{
						EmitWarning(WarningId.InvalidLibDirectoryOption, "directory \"{0}\" specified in -lib option is not valid", dir);
					}
					else
					{
						EmitWarning(WarningId.InvalidLibDirectoryEnvironment, "directory \"{0}\" specified in LIB environment is not valid", dir);
					}
				}
			}
		}

		private Assembly LoadMscorlib(IList<string> references)
		{
			if (references != null)
			{
				foreach (string r in references)
				{
					try
					{
						if (AssemblyName.GetAssemblyName(r).Name == "mscorlib")
						{
							return LoadFile(r);
						}
					}
					catch
					{
					}
				}
			}
			foreach (string mscorlib in FindAssemblyPath("mscorlib.dll"))
			{
				return LoadFile(mscorlib);
			}
			Console.Error.WriteLine("Error: unable to find mscorlib.dll");
			Environment.Exit(1);
			return null;
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
						EmitWarning(WarningId.LegacySearchRule, "found assembly \"{0}\" using legacy search rule, please append '.dll' to the reference", file);
						yield return path;
					}
				}
			}
		}
	}
}
