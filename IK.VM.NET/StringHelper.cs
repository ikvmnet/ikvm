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
using System.Reflection;

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

	public static object subSequence(string s, int offset, int count)
	{
		// TODO
		throw new NotImplementedException();
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

public class StringBufferHelper
{
	private delegate string toHexStringDelegate(int i);
	private static toHexStringDelegate toHexString;

	public static int indexOf(StringBuilder thiz, string s)
	{
		return thiz.ToString().IndexOf(s);
	}

	public static int indexOf(StringBuilder thiz, string s, int off)
	{
		return thiz.ToString().IndexOf(s, off);
	}

	public static int lastIndexOf(StringBuilder thiz, string s)
	{
		// TODO make sure this is correct
		return thiz.ToString().LastIndexOf(s);
	}

	public static int lastIndexOf(StringBuilder thiz, string s, int fromIndex)
	{
		// TODO make sure this is correct
		return thiz.ToString().LastIndexOf(s, fromIndex);
	}

	public static StringBuilder append(StringBuilder thiz, object o)
	{
		if(o == null)
		{
			return thiz.Append("null");
		}
		if(o is Array)
		{
			if(toHexString == null)
			{
				toHexString = (toHexStringDelegate)Delegate.CreateDelegate(typeof(toHexStringDelegate), ClassLoaderWrapper.GetType("java.lang.Integer").GetMethod("toHexString"));
			}
			return thiz.Append(NativeCode.java.lang.VMClass.getName(o.GetType()) + "@" + toHexString(o.GetHashCode()));
		}
		return thiz.Append(o);
	}

	public static StringBuilder append(StringBuilder thiz, string s)
	{
		if(s == null)
		{
			s = "null";
		}
		return thiz.Append(s);
	}

	public static StringBuilder append(StringBuilder thiz, bool b)
	{
		if(b)
		{
			return thiz.Append("true");
		}
		else
		{
			return thiz.Append("false");
		}
	}

	public static StringBuilder append(StringBuilder thiz, float f)
	{
		// TODO this is not correct, we need to use the Java algorithm of converting a float to string
		if(float.IsNaN(f))
		{
			thiz.Append("NaN");
			return thiz;
		}
		if(float.IsNegativeInfinity(f))
		{
			thiz.Append("-Infinity");
			return thiz;
		}
		if(float.IsPositiveInfinity(f))
		{
			thiz.Append("Infinity");
			return thiz;
		}
		// HACK really lame hack to apprioximate the Java behavior a little bit
		string s = f.ToString(System.Globalization.CultureInfo.InvariantCulture);
		thiz.Append(s);
		if(s.IndexOf('.') == -1)
		{
			thiz.Append(".0");
		}
		return thiz;
	}

	public static StringBuilder append(StringBuilder thiz, double d)
	{
		DoubleToString.append(thiz, d);
		return thiz;
	}

	public static StringBuilder insert(StringBuilder thiz, int index, string s)
	{
		if(s == null)
		{
			s = "null";
		}
		return thiz.Insert(index, s);
	}

	public static StringBuilder insert(StringBuilder thiz, int index, object o)
	{
		if(o == null)
		{
			o = "null";
		}
		return thiz.Insert(index, o);
	}

	public static string substring(StringBuilder thiz, int start, int end)
	{
		return thiz.ToString(start, end - start);
	}

	public static string substring(StringBuilder thiz, int start)
	{
		return thiz.ToString(start, thiz.Length - start);
	}

	public static StringBuilder replace(StringBuilder thiz, int start, int end, string str)
	{
		// OPTIMIZE this could be done a little more efficient
		thiz.Remove(start, end - start);
		thiz.Insert(start, str);
		return thiz;
	}

	public static StringBuilder delete(StringBuilder thiz, int start, int end)
	{
		return thiz.Remove(start, end - start);
	}

	public static StringBuilder deleteCharAt(StringBuilder thiz, int pos)
	{
		return thiz.Remove(pos, 1);
	}

	public static StringBuilder reverse(StringBuilder thiz)
	{
		for(int i = (thiz.Length / 2) - 1; i >=0; i--)
		{
			char c = thiz[i];
			thiz[i] = thiz[thiz.Length - 1 - i];
			thiz[thiz.Length - 1 - i] = c;
		}
		return thiz;
	}

	public static void getChars(StringBuilder thiz, int srcBegin, int srcEnd, char[] dst, int dstBegin)
	{
		string s = thiz.ToString(srcBegin, srcEnd - srcBegin);
		s.CopyTo(0, dst, dstBegin, s.Length);
	}
}
