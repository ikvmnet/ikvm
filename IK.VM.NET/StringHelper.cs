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
	public static string NewString(char[] data, int offset, int count, bool dont_copy)
	{
		return new String(data, offset, count);
	}

	public static string NewString(sbyte[] sdata)
	{
		return NewString(sdata, 0, sdata.Length);
	}

	public static string NewString(sbyte[] sdata, int hibyte)
	{
		return NewString(sdata, hibyte, 0, sdata.Length);
	}

	public static string NewString(sbyte[] sdata, int offset, int count)
	{
		// TODO what encoding should this use?
		// TODO could use the unsafe constructor that takes sbyte*, but I don't know if that is worthwhile to be unsafe for
		byte[] data = new byte[sdata.Length];
		for(int i = 0; i < data.Length; i++)
		{
			data[i] = (byte)sdata[i];
		}
		return System.Text.Encoding.ASCII.GetString(data, offset, count);
	}

	public static string NewString(sbyte[] sdata, int hibyte, int offset, int count)
	{
		// TODO benchmark this versus using a stringbuilder instead of a char[]
		hibyte <<= 8;
		char[] data = new char[count];
		for(int i = 0; i < count; i++)
		{
			// TODO what happens for negative bytes?
			data[i] = (char)(((byte)sdata[i + offset]) | hibyte);
		}
		return new String(data);
	}

	public static string NewString(sbyte[] sdata, string charsetName)
	{
		return NewString(sdata, 0, sdata.Length, charsetName);
	}

	public static string NewString(sbyte[] sdata, int offset, int count, string charsetName)
	{
		// HACK special case for UTF8, I really need to implement this by
		// redirecting to the classpath character encoding support
		if(charsetName == "UTF8")
		{
			char[] ch = new Char[count];
			int l = 0;
			for(int i = 0; i < count; i++)
			{
				int c = (byte)sdata[offset + i];
				int char2, char3;
				switch (c >> 4)
				{
					case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
						// 0xxxxxxx
						break;
					case 12: case 13:
						// 110x xxxx   10xx xxxx
						char2 = (byte)sdata[offset + ++i];
						c = (((c & 0x1F) << 6) | (char2 & 0x3F));
						break;
					case 14:
						// 1110 xxxx  10xx xxxx  10xx xxxx
						char2 = (byte)sdata[offset + ++i];
						char3 = (byte)sdata[offset + ++i];
						c = (((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0));
						break;
				}
				ch[l++] = (char)c;
			}
			return new String(ch, 0, l);
		}
		// TODO don't use reflection, but write a Java helper class and redirect this method there
		Type t = ClassLoaderWrapper.GetType("gnu.java.io.EncodingManager");
		object decoder = t.InvokeMember("getDecoder", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new object[] { charsetName });
		return new String((char[])decoder.GetType().InvokeMember("convertToChars", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, decoder, new object[] { sdata, offset, count }));
	}

	public static string valueOf(bool b)
	{
		return b ? "true" : "false";
	}

	public static string valueOf(int i)
	{
		return i.ToString();
	}

	public static string valueOf(long l)
	{
		return l.ToString();
	}

	public static string valueOf(char c)
	{
		return c.ToString();
	}

	public static string valueOf(float f)
	{
		StringBuilder sb = new StringBuilder();
		return StringBufferHelper.append(sb, f).ToString();
	}

	public static string valueOf(double d)
	{
		StringBuilder sb = new StringBuilder();
		return StringBufferHelper.append(sb, d).ToString();
	}

	public static string valueOf(char[] c)
	{
		return new String(c);
	}

	public static string valueOf(object o)
	{
		if(o == null)
		{
			return "null";
		}
		return ObjectHelper.toStringVirtual(o);
	}

	public static string substring(string s, int off, int end)
	{
		return s.Substring(off, end - off);
	}

	public static bool startsWith(string s, string prefix, int toffset)
	{
		// TODO
		throw new NotImplementedException();
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

	public static bool equalsIgnoreCase(string s1, string s2)
	{
		return String.Compare(s1, s2, true) == 0;
	}

	public static int compareToIgnoreCase(string s1, string s2)
	{
		return String.Compare(s1, s2, true);
	}

	public static sbyte[] getBytes(string s)
	{
		byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
		sbyte[] sdata = new sbyte[data.Length];
		for(int i = 0; i < data.Length; i++)
		{
			sdata[i] = (sbyte)data[i];
		}
		return sdata;
	}

	public static sbyte[] getBytes(string s, string charsetName)
	{
		// TODO don't use reflection, but write a Java helper class and redirect this method there
		char[] ch = s.ToCharArray();
		Type t = ClassLoaderWrapper.GetType("gnu.java.io.EncodingManager");
		object encoder = t.InvokeMember("getEncoder", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new object[] { charsetName });
		return (sbyte[])encoder.GetType().InvokeMember("convertToBytes", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, encoder, new object[] { ch, 0, ch.Length });
	}

	public static void getBytes(string s, int srcBegin, int srcEnd, sbyte[] dst, int dstBegin)
	{
		for(int i = 0; i < (srcEnd - srcBegin); i++)
		{
			dst[i + dstBegin] = (sbyte)s[i + srcBegin];
		}
	}

	public static object subSequence(string s, int offset, int count)
	{
		// TODO
		throw new NotImplementedException();
	}

	public static bool regionMatches(string s, int toffset, string other, int ooffset, int len)
	{
		return regionMatches(s, false, toffset, other, ooffset, len);
	}

	public static bool regionMatches(string s, bool ignoreCase, int toffset, string other, int ooffset, int len)
	{
		if(toffset < 0 || ooffset < 0 || toffset + len > s.Length || ooffset + len > other.Length)
		{
			return false;
		}
		while(--len >= 0)
		{
			char c1 = s[toffset++];
			char c2 = other[ooffset++];
			if(c1 != c2 && (!ignoreCase || (Char.ToLower(c1) != Char.ToLower(c2) && (Char.ToUpper(c1) != Char.ToUpper(c2)))))
			{
				return false;
			}
		}
		return true;
	}

	// NOTE argument is of type object, because otherwise the code that calls this function
	// has to be much more complex
	public static int hashCode(object s)
	{
		int h = 0;
		foreach(char c in (string)s)
		{
			h = h * 31 + c;
		}
		return h;
	}

	public static string toUpperCase(string s, object locale)
	{
		// TODO
		return s.ToUpper();
	}

	public static string toLowerCase(string s, object locale)
	{
		// TODO
		return s.ToLower();
	}
}

public class StringBufferHelper
{
	public static StringBuilder append(StringBuilder thiz, object o)
	{
		if(o == null)
		{
			o = "null";
		}
		return thiz.Append(ObjectHelper.toStringVirtual(o));
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

	public static void getChars(StringBuilder thiz, int srcBegin, int srcEnd, char[] dst, int dstBegin)
	{
		string s = thiz.ToString(srcBegin, srcEnd - srcBegin);
		s.CopyTo(0, dst, dstBegin, s.Length);
	}
}
