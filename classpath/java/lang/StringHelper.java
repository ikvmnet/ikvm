/* StringHelper.java -- helper class adapted from java/lang/String.java
   Copyright (C) 1998, 1999, 2000, 2001, 2002 Free Software Foundation, Inc.

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
02111-1307 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */

package java.lang;

import java.util.Locale;
import java.io.UnsupportedEncodingException;
import java.io.CharConversionException;
import gnu.java.io.EncodingManager;
import gnu.java.lang.CharData;

final class StringHelper
{
    private StringHelper() {}

    static boolean equalsIgnoreCase(String s1, String s2)
    {
	int len = s1.length();
	if(s2 == null || len != s2.length())
	{
	    return false;
	}
	for(int i = 0; i < len; i++)
	{
	    char c1 = s1.charAt(i);
	    char c2 = s2.charAt(i);
	    if(c1 != c2 && (Character.toUpperCase(c1) != Character.toUpperCase(c2)) &&
		(Character.toLowerCase(c1) != Character.toLowerCase(c2)))
	    {
		return false;
	    }
	}
	return true;
    }

    static int compareTo(String s1, String s2)
    {
	int len = Math.min(s1.length(), s2.length());
	for(int i = 0; i < len; i++)
	{
	    int diff = s1.charAt(i) - s2.charAt(i);
	    if(diff != 0)
	    {
		return diff;
	    }
	}
	return s1.length() - s2.length();
    }

    static int compareToIgnoreCase(String s1, String s2)
    {
	int len = Math.min(s1.length(), s2.length());
	for(int i = 0; i < len; i++)
	{
	    int result = Character.toLowerCase(Character.toUpperCase(s1.charAt(i)))
		- Character.toLowerCase(Character.toUpperCase(s2.charAt(i)));
	    if (result != 0)
		return result;
	}
	return s1.length() - s2.length();
    }

    static String toLowerCase(String s)
    {
	return toLowerCase(s, Locale.getDefault());
    }

    static String toLowerCase(String s, Locale loc)
    {
	// First, see if the current string is already lower case.
	boolean turkish = "tr".equals(loc.getLanguage());
	int len = s.length();
	for(int i = 0; i < len; i++)
	{
	    char ch = s.charAt(i);
	    if((turkish && ch == '\u0049') || ch != Character.toLowerCase(ch))
	    {
		// Now we perform the conversion. Fortunately, there are no multi-character
		// lowercase expansions in Unicode 3.0.0.
		char[] newStr = (char[])s.toCharArray();
		for(; i < len; i++)
		{
		    ch = newStr[i];
		    // Hardcoded special case.
		    newStr[i] = (turkish && ch == '\u0049') ? '\u0131' : Character.toLowerCase(ch);
		}
		return new String(newStr);
	    }
	}
	return s;
    }

    static String toUpperCase(String s)
    {
	return toUpperCase(s, Locale.getDefault());
    }

    static String toUpperCase(String s, Locale loc)
    {
	// First, see how many characters we have to grow by, as well as if the
	// current string is already upper case.
	boolean turkish = "tr".equals(loc.getLanguage());
	int expand = 0;
	boolean unchanged = true;
	int i = s.length();
	int x = i;
	while (--i >= 0)
	{
	    char ch = s.charAt(--x);
	    expand += upperCaseExpansion(ch);
	    unchanged = (unchanged && expand == 0
		&& ! (turkish && ch == '\u0069')
		&& ch == Character.toUpperCase(ch));
	}
	if (unchanged)
	    return s;

	// Now we perform the conversion.
	i = s.length();
	if (expand == 0)
	{
	    char[] newStr = s.toCharArray();
	    while (--i >= 0)
	    {
		char ch = s.charAt(x);
		// Hardcoded special case.
		newStr[x++] = (turkish && ch == '\u0069') ? '\u0130'
		    : Character.toUpperCase(ch);
	    }
	    return new String(newStr);
	}

	// Expansion is necessary.
	char[] newStr = new char[s.length() + expand];
	int j = 0;
	while (--i >= 0)
	{
	    char ch = s.charAt(x++);
	    // Hardcoded special case.
	    if (turkish && ch == '\u0069')
	    {
		newStr[j++] = '\u0130';
		continue;
	    }
	    expand = upperCaseExpansion(ch);
	    if (expand > 0)
	    {
		int index = upperCaseIndex(ch);
		while (expand-- >= 0)
		    newStr[j++] = upperExpand[index++];
	    }
	    else
		newStr[j++] = Character.toUpperCase(ch);
	}
	return new String(newStr);
    }

    private static int upperCaseExpansion(char ch)
    {
	return Character.direction[Character.readChar(ch) >> 7] & 3;
    }

    private static int upperCaseIndex(char ch)
    {
	// Simple binary search for the correct character.
	int low = 0;
	int hi = upperSpecial.length - 2;
	int mid = ((low + hi) >> 2) << 1;
	char c = upperSpecial[mid];
	while (ch != c)
	{
	    if (ch < c)
		hi = mid - 2;
	    else
		low = mid + 2;
	    mid = ((low + hi) >> 2) << 1;
	    c = upperSpecial[mid];
	}
	return upperSpecial[mid + 1];
    }

    private static final char[] upperExpand = CharData.UPPER_EXPAND.toCharArray();
    private static final char[] upperSpecial = CharData.UPPER_SPECIAL.toCharArray();

    static String NewString(byte[] ascii, int hibyte, int offset, int count)
    {
	if (offset < 0 || count < 0 || offset + count > ascii.length)
	    throw new StringIndexOutOfBoundsException();
	char[] value = new char[count];
	hibyte <<= 8;
	offset += count;
	while (--count >= 0)
	    value[count] = (char) (hibyte | (ascii[--offset] & 0xff));
	return new String(value);
    }

    static String NewString(byte[] ascii, int hibyte)
    {
	return NewString(ascii, hibyte, 0, ascii.length);
    }

    static String NewString(byte[] data, int offset, int count, String encoding)
	throws UnsupportedEncodingException
    {
	if (offset < 0 || count < 0 || offset + count > data.length)
	    throw new StringIndexOutOfBoundsException();
	try
	{
	    // XXX Consider using java.nio here.
	    return new String(EncodingManager.getDecoder(encoding)
		.convertToChars(data, offset, count));
	}
	catch (CharConversionException cce)
	{
	    throw new Error(cce);
	}
    }

    static String NewString(byte[] data, String encoding)
	throws UnsupportedEncodingException
    {
	return NewString(data, 0, data.length, encoding);
    }

    static String NewString(byte[] data, int offset, int count)
    {
	if (offset < 0 || count < 0 || offset + count > data.length)
	    throw new StringIndexOutOfBoundsException();
	try
	{
	    // XXX Consider using java.nio here.
	    return new String(EncodingManager.getDecoder()
		.convertToChars(data, offset, count));
	}
	catch (CharConversionException cce)
	{
	    throw new Error(cce);
	}
    }

    static String NewString(byte[] data)
    {
	return NewString(data, 0, data.length);
    }

    static String NewString(char[] data, int offset, int count, boolean dont_copy)
    {
	return new String(data, offset, count);
    }

    static String NewString(StringBuffer sb)
    {
	synchronized(sb)
	{
	    return new String(sb.value, 0, sb.count);
	}
    }

    static String substring(String s, int off, int end)
    {
	return cli.System.String.Substring(s, off, end - off);
    }

    static boolean startsWith(String s, String prefix, int toffset)
    {
	if(toffset < 0)
	{
	    return false;
	}
	s = cli.System.String.Substring(s, Math.min(s.length(), toffset));
	return cli.System.String.StartsWith(s, prefix);
    }

    static char charAt(String s, int index)
    {
	try 
	{
	    return cli.System.String.get_Chars(s, index);
	}
	// NOTE the System.IndexOutOfRangeException thrown by get_Chars, is translated by our
	// exception handling code to an ArrayIndexOutOfBoundsException, so we catch that.
	catch (ArrayIndexOutOfBoundsException x) 
	{
	    throw new StringIndexOutOfBoundsException();
	}
    }

    static void getChars(String s, int srcBegin, int srcEnd, char[] dst, int dstBegin) 
    {
	cli.System.String.CopyTo(s, srcBegin, dst, dstBegin, srcEnd - srcBegin);
    }

    // this exposes the package accessible "count" field (for use by StringBuffer)
    static int GetCountField(String s)
    {
	return s.length();
    }

    // this exposes the package accessible "value" field (for use by StringBuffer)
    static char[] GetValueField(String s)
    {
	return s.toCharArray();
    }

    // this exposes the package accessible "offset" field (for use by StringBuffer)
    static int GetOffsetField(String s)
    {
	return 0;
    }

    static int indexOf(String s, char ch, int fromIndex)
    {
	// Java allow fromIndex to both below zero or above the length of the string, .NET doesn't
	return cli.System.String.IndexOf(s, ch, Math.max(0, Math.min(s.length(), fromIndex)));
    }

    static int indexOf(String s, String o, int fromIndex)
    {
	// Java allow fromIndex to both below zero or above the length of the string, .NET doesn't
	return cli.System.String.IndexOf(s, o, Math.max(0, Math.min(s.length(), fromIndex)));
    }

    static int lastIndexOf(String s, char ch, int fromIndex)
    {
	// start by dereferencing s, to make sure we throw a NullPointerException if s is null
	int len = s.length();
	if(fromIndex  < 0)
	{
	    return -1;
	}
	// Java allow fromIndex to be above the length of the string, .NET doesn't
	return cli.System.String.LastIndexOf(s, ch, Math.min(len - 1, fromIndex));
    }

    static int lastIndexOf(String s, String o)
    {
	return lastIndexOf(s, o, s.length());
    }

    static int lastIndexOf(String s, String o, int fromIndex)
    {
	// start by dereferencing s, to make sure we throw a NullPointerException if s is null
	int len = s.length();
	if(fromIndex  < 0)
	{
	    return -1;
	}
	if(o.length() == 0)
	{
	    return Math.min(len, fromIndex);
	}
	// Java allow fromIndex to be above the length of the string, .NET doesn't
	return cli.System.String.LastIndexOf(s, o, Math.min(len - 1, fromIndex + o.length() - 1));
    }

    static String concat(String s1, String s2)
    {
	// null check
	s1 = s1.toString();
	if(s2.length() == 0)
	{
	    return s1;
	}
	return cli.System.String.Concat(s1, s2);
    }

    static void getBytes(String s, int srcBegin, int srcEnd, byte dst[], int dstBegin)
    {
	if (srcBegin < 0 || srcBegin > srcEnd || srcEnd > s.length())
	    throw new StringIndexOutOfBoundsException();
	int i = srcEnd - srcBegin;
	while (--i >= 0)
	    dst[dstBegin++] = (byte)s.charAt(srcBegin++);
    }

    static byte[] getBytes(String s, String enc) throws UnsupportedEncodingException
    {
	try
	{
	    // XXX Consider using java.nio here.
	    return EncodingManager.getEncoder(enc)
		.convertToBytes(s.toCharArray());
	}
	catch (CharConversionException e)
	{
	    return null;
	}
    }

    static byte[] getBytes(String s)
    {
	try
	{
	    // XXX Consider using java.nio here.
	    return EncodingManager.getEncoder()
		.convertToBytes(s.toCharArray());
	}
	catch (CharConversionException e)
	{
	    return null;
	}
    }

    static boolean regionMatches(String s, int toffset, String other, int ooffset, int len)
    {
	return regionMatches(s, false, toffset, other, ooffset, len);
    }

    static boolean regionMatches(String s, boolean ignoreCase, int toffset,
	String other, int ooffset, int len)
    {
	if (toffset < 0 || ooffset < 0 || toffset + len > s.length()
	    || ooffset + len > other.length())
	    return false;
	while (--len >= 0)
	{
	    char c1 = s.charAt(toffset++);
	    char c2 = other.charAt(ooffset++);
	    // Note that checking c1 != c2 is redundant when ignoreCase is true,
	    // but it avoids method calls.
	    if (c1 != c2
		&& (! ignoreCase
		|| (Character.toLowerCase(c1) != Character.toLowerCase(c2)
		&& (Character.toUpperCase(c1)
		!= Character.toUpperCase(c2)))))
		return false;
	}
	return true;
    }

    static String trim(String s)
    {
	int limit = s.length();
	if (limit == 0 || (s.charAt(0) > '\u0020'
	    && s.charAt(limit - 1) > '\u0020'))
	    return s;
	int begin = 0;
	do
	    if (begin == limit)
		return "";
	while (s.charAt(begin++) <= '\u0020');
	int end = limit;
	while (s.charAt(--end) <= '\u0020');
	return s.substring(begin - 1, end + 1);
    }

    static String valueOf(boolean b)
    {
	return b ? "true" : "false";
    }

    static String valueOf(int i)
    {
	return Integer.toString(i, 10);
    }

    static String valueOf(long l)
    {
	return Long.toString(l);
    }

    static String valueOf(char c)
    {
	return cli.System.String.__new(c, 1);
    }

    static String valueOf(float f)
    {
	return Float.toString(f);
    }

    static String valueOf(double d)
    {
	return Double.toString(d);
    }

    static String valueOf(char[] c)
    {
	return new String(c);
    }

    static String valueOf(char[] c, int offset, int count)
    {
	return new String(c, offset, count);
    }

    static String valueOf(Object o)
    {
	return o == null ? "null" : o.toString();
    }

    static int hashCode(String s)
    {
	int h = 0;
	int len = s.length();
	for(int i = 0; i < len; i++)
	{
	    h = h *31 + cli.System.String.get_Chars(s, i);
	}
	return h;
    }
}
