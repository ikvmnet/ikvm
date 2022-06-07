/*
  Copyright (C) 2007-2013 Jeroen Frijters

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
using System.Globalization;
using IKVM.Internal;

namespace IKVM.Runtime
{
	public static class Assertions
	{
		private static bool sysAsserts;
		private static bool userAsserts;
		private static OptionNode classes;
		private static OptionNode packages;

		private sealed class OptionNode
		{
			internal readonly string name;
			internal readonly bool enabled;
			internal readonly OptionNode next;

			internal OptionNode(string name, bool enabled, OptionNode next)
			{
				this.name = name;
				this.enabled = enabled;
				this.next = next;
			}
		}

		private static void AddOption(string classOrPackage, bool enabled)
		{
			if (classOrPackage == null)
			{
				throw new ArgumentNullException("classOrPackage");
			}

			if (classOrPackage.EndsWith("..."))
			{
				packages = new OptionNode(classOrPackage.Substring(0, classOrPackage.Length - 3), enabled, packages);
			}
			else
			{
				classes = new OptionNode(classOrPackage, enabled, classes);
			}
		}

		public static void EnableAssertions(string classOrPackage)
		{
			AddOption(classOrPackage, true);
		}

		public static void DisableAssertions(string classOrPackage)
		{
			AddOption(classOrPackage, false);
		}

		public static void EnableAssertions()
		{
			userAsserts = true;
		}

		public static void DisableAssertions()
		{
			userAsserts = false;
		}

		public static void EnableSystemAssertions()
		{
			sysAsserts = true;
		}

		public static void DisableSystemAssertions()
		{
			sysAsserts = false;
		}

		internal static bool IsEnabled(TypeWrapper tw)
		{
			string className = tw.Name;

			// match class name
			for (OptionNode n = classes; n != null; n = n.next)
			{
				if (n.name == className)
				{
					return n.enabled;
				}
			}

			// match package name
			if (packages != null)
			{
				int len = className.Length;
				while (len > 0 && className[--len] != '.') ;

				do
				{
					for (OptionNode n = packages; n != null; n = n.next)
					{
						if (String.Compare(n.name, 0, className, 0, len, false, CultureInfo.InvariantCulture) == 0 && len == n.name.Length)
						{
							return n.enabled;
						}
					}
					while (len > 0 && className[--len] != '.') ;
				} while (len > 0);
			}

			return tw.GetClassLoader() == ClassLoaderWrapper.GetBootstrapClassLoader() ? sysAsserts : userAsserts;
		}

		private static int Count(OptionNode n)
		{
			int count = 0;
			while (n != null)
			{
				count++;
				n = n.next;
			}
			return count;
		}

		internal static object RetrieveDirectives()
		{
#if FIRST_PASS
			return null;
#else
			java.lang.AssertionStatusDirectives asd = new java.lang.AssertionStatusDirectives();
			string[] arrStrings = new string[Count(classes)];
			bool[] arrBools = new bool[arrStrings.Length];
			OptionNode n = classes;
			for (int i = 0; i < arrStrings.Length; i++)
			{
				arrStrings[i] = n.name;
				arrBools[i] = n.enabled;
				n = n.next;
			}
			asd.classes = arrStrings;
			asd.classEnabled = arrBools;
			arrStrings = new string[Count(packages)];
			arrBools = new bool[arrStrings.Length];
			n = packages;
			for (int i = 0; i < arrStrings.Length; i++)
			{
				arrStrings[i] = n.name;
				arrBools[i] = n.enabled;
				n = n.next;
			}
			asd.packages = arrStrings;
			asd.packageEnabled = arrBools;
			asd.deflt = userAsserts;
			return asd;
#endif
		}
	}
}
