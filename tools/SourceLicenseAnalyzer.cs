/*
  Copyright (C) 2013-2014 Jeroen Frijters

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
using System.Text;
using System.Text.RegularExpressions;

namespace SourceLicenseAnalyzer
{
	class Years
	{
		internal static Years Dummy = new Years();
		internal int min = Int32.MaxValue;
		internal int max = Int32.MinValue;
		internal string name;
	}

	class Program
	{
		static Dictionary<string, string> aliases = new Dictionary<string, string>();
		static Dictionary<string, Years> copyrights = new Dictionary<string, Years>();
		static int errorCount;

		static void Def(string name, params string[] aliasesList)
		{
			Years y = new Years();
			y.name = name;
			copyrights.Add(name, y);
			aliases.Add(name, name);
			foreach (string s in aliasesList)
			{
				aliases.Add(s, name);
			}
		}

		static int Main(string[] args)
		{
			Def("Free Software Foundation", "Free Software   Foundation", "Free Software    Foundation", "Free Software Fonudation, Inc.");
			Def("Sun Microsystems, Inc.", "Sun Microsystems Inc");
			Def("Jeroen Frijters");
			Def("Thai Open Source Software Center Ltd");
			Def("World Wide Web Consortium");
			Def("International Business Machines, Inc.", "IBM Corp.", "IBM Corporation", "International Business Machines", "International Business Machines Corporation");
			Def("Wily Technology, Inc.");
			Def("Unicode, Inc.");
			Def("Colin Plumb");
			Def("Taligent, Inc.");
			Def("Red Hat, Inc.");
			Def("The Open Group Research Institute");
			Def("FundsXpress, INC.");
			Def("AT&T");
			Def("The Apache Software Foundation");
			Def("freebxml.org");
			Def("The Cryptix Foundation Limited");
			Def("Visual Numerics Inc.");
			Def("INRIA, France Telecom");
			Def("Oracle and/or its affiliates", "Oracle Corporation");
			Def("i-net software");
			Def("Google Inc.");
			Def("Stephen Colebourne & Michael Nascimento Santos");
			Def("Attila Szegedi");


			// these are false positives
			copyrights.Add("dummy", Years.Dummy);
			aliases.Add("icSigCopyrightTag", "dummy");
			aliases.Add("Copyright notice to stick into built-in-profile files.", "dummy");
			aliases.Add("AssemblyCopyrightAttribute", "dummy");
			aliases.Add("getVersionAndCopyrightInfo()", "dummy");
			aliases.Add("Copyright by IBM and others and distributed under the * distributed under MIT/X", "dummy");
			aliases.Add("*  Copyright Office. *", "dummy");
			aliases.Add("* Copyright office. *", "dummy");
			aliases.Add("Your Corporation", "dummy");
			aliases.Add("but I wrote that code so I co-own the copyright", "dummy");
			aliases.Add("identifying information: \"Portions Copyrighted [year] * [name of copyright owner]\"", "dummy");
			aliases.Add("* \"Portions Copyright [year] [name of copyright owner]\" *", "dummy");

			using (StreamReader rdr = new StreamReader("allsources.gen.lst"))
			{
				string file;
				while ((file = rdr.ReadLine()) != null)
				{
					if (file != "AssemblyInfo.java")
					{
						ProcessFile(file);
					}
				}
			}

			Years[] years = new Years[copyrights.Count];
			copyrights.Values.CopyTo(years, 0);

			Array.Sort(years, delegate(Years x, Years y) { return x.name == null ? 0 : x.name.CompareTo(y.name); });

			bool first = true;
			foreach (Years y in years)
			{
				if (y != Years.Dummy)
				{
					if (!first)
					{
						Console.WriteLine("\\r\\n\" +");
					}
					first = false;
					Console.Write("    \"");
					if (y.min != y.max)
					{
						Console.Write("{0}-{1}  {2}", y.min, y.max, y.name);
					}
					else
					{
						Console.Write("{0}       {1}", y.min, y.name);
					}
				}
			}
			Console.WriteLine("\"");

			return errorCount;
		}

		static void ProcessFile(string filePath)
		{
			bool gpl = false;
			bool classpathException = false;
			if (!File.Exists(filePath) && File.Exists(filePath + ".in"))
			{
				filePath += ".in";
			}
			using (StreamReader rdr = new StreamReader(filePath))
			{
				string line;
				string nextline = null;
				while ((line = rdr.ReadLine()) != null)
				{
					gpl |= line.Contains("GNU General Public License");
					classpathException |= line.Contains("subject to the \"Classpath\" exception") || line.Contains("permission to link this library with independent modules");
					while (line != null && line.IndexOf("Copyright") != -1)
					{
						Years y = null;
						foreach (KeyValuePair<string, string> kv in aliases)
						{
							if (line.IndexOf(kv.Key) != -1)
							{
								y = copyrights[kv.Value];
								break;
							}
						}
						if (y == null)
						{
							if (nextline == null)
							{
								nextline = rdr.ReadLine();
								if (nextline.IndexOf("Copyright") == -1)
								{
									line += nextline;
									continue;
								}
							}
							if (filePath.Contains("/jaxws/src/share/jaxws_classes/com/sun/xml/internal/rngom/")
								&& (line.Contains("* Copyright (C) 2004-2011 *") || line.Contains("* Copyright (C) 2004-2012 *")))
							{
								// HACK ignore bogus copyright line
							}
							else
							{
								Error(filePath + ":" + Environment.NewLine + line);
							}
						}
						else
						{
							foreach (Match m in Regex.Matches(line, "[0-9][0-9][0-9][0-9]"))
							{
								int v = Int32.Parse(m.Value);
								y.min = Math.Min(y.min, v);
								y.max = Math.Max(y.max, v);
							}
						}
						line = nextline;
						nextline = null;
					}
				}
			}
			if (gpl && !classpathException)
			{
				Error("GPL without Classpath exception: {0}", filePath);
			}
		}

		static void Error(string message, params object[] args)
		{
			errorCount++;
			Console.Error.WriteLine(message, args);
		}
	}
}
