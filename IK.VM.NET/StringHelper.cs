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
using System.Text;

public class StringHelper
{
	public static string substring(string s, int off, int end)
	{
		return s.Substring(off, end - off);
	}

	public static bool startsWith(string s, string prefix, int toffset)
	{
		if(toffset < 0)
		{
			return false;
		}
		return s.Substring(Math.Min(s.Length, toffset)).StartsWith(prefix);
	}

	public static char charAt(string s, int index)
	{
		try {
			return s[index];
		}
		catch (IndexOutOfRangeException) {
			throw JavaException.StringIndexOutOfBoundsException("");
		}
	}

	public static void getChars(string s, int srcBegin, int srcEnd, char[] dst, int dstBegin) 
	{
		s.CopyTo(srcBegin, dst, dstBegin, srcEnd - srcBegin);
	}

	public static int GetCountField(string s)
	{
		return s.Length;
	}

	public static char[] GetValueField(string s)
	{
		return s.ToCharArray();
	}

	public static int GetOffsetField(string s)
	{
		return 0;
	}

	public static int indexOf(string s, char ch, int fromIndex)
	{
		// Java allow fromIndex to both below zero or above the length of the string, .NET doesn't
		return s.IndexOf(ch, Math.Max(0, Math.Min(s.Length, fromIndex)));
	}

	public static int indexOf(string s, string o, int fromIndex)
	{
		// Java allow fromIndex to both below zero or above the length of the string, .NET doesn't
		return s.IndexOf(o, Math.Max(0, Math.Min(s.Length, fromIndex)));
	}

	public static int lastIndexOf(string s, char ch, int fromIndex)
	{
		// start by dereferencing s, to make sure we throw a NullPointerException if s is null
		int len = s.Length;
		if(fromIndex  < 0)
		{
			return -1;
		}
		// Java allow fromIndex to be above the length of the string, .NET doesn't
		return s.LastIndexOf(ch, Math.Min(len - 1, fromIndex));
	}

	public static int lastIndexOf(string s, string o)
	{
		return lastIndexOf(s, o, s.Length);
	}

	public static int lastIndexOf(string s, string o, int fromIndex)
	{
		// start by dereferencing s, to make sure we throw a NullPointerException if s is null
		int len = s.Length;
		if(fromIndex  < 0)
		{
			return -1;
		}
		if(o.Length == 0)
		{
			return Math.Min(len, fromIndex);
		}
		// Java allow fromIndex to be above the length of the string, .NET doesn't
		return s.LastIndexOf(o, Math.Min(len - 1, fromIndex + o.Length - 1));
	}

	public static string concat(string s1, string s2)
	{
		s1 = s1.ToString();
		if(s2.Length == 0)
		{
			return s1;
		}
		return String.Concat(s1, s2);
	}
}
