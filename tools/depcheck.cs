/*
  Copyright (C) 2009 Jeroen Frijters

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
using System.IO;
using System.Collections.Generic;
using System.Reflection;

class DependencyChecker
{
	static int Main(string[] args)
	{
		Dictionary<string, List<string>> deps = new Dictionary<string, List<string>>();
		string dep = null;
		foreach (string line in File.ReadAllLines(args[1]))
		{
			if (line.Trim().Length == 0 || line.StartsWith("#"))
			{
				// comment
			}
			else if (line.StartsWith("->"))
			{
				deps[dep].Add(line.Substring(2));
			}
			else
			{
				dep = line;
				deps.Add(dep, new List<string>());
			}
		}
		List<string> whitelist = new List<string>(new string[] { "mscorlib", "System", "IKVM.Runtime", "IKVM.OpenJDK.Core" });
		bool fail = false;
		foreach (string line in File.ReadAllLines(args[0]))
		{
			if (line.Contains("-out:"))
			{
				string file = line.Trim().Substring(5);
				Assembly asm = Assembly.ReflectionOnlyLoadFrom(Path.Combine(Path.GetDirectoryName(args[0]), file));
				if (!deps.ContainsKey(asm.GetName().Name))
				{
					fail = true;
					Console.WriteLine(asm.GetName().Name);
					foreach (AssemblyName asmdep in asm.GetReferencedAssemblies())
					{
						if (!whitelist.Contains(asmdep.Name))
						{
							Console.WriteLine("->{0}", asmdep.Name);
						}
					}
				}
				else
				{
					foreach (AssemblyName asmdep in asm.GetReferencedAssemblies())
					{
						if (!whitelist.Contains(asmdep.Name))
						{
							if (!deps[asm.GetName().Name].Contains(asmdep.Name))
							{
								fail = true;
								Console.WriteLine("Error: Assembly {0} has an undeclared dependency on {1}", asm.GetName().Name, asmdep.Name);
							}
						}
					}
				}
			}
		}
		return fail ? 1 : 0;
	}
}
