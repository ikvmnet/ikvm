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

import gnu.classpath.SystemProperties;
import gnu.java.lang.CharData;
import java.io.UnsupportedEncodingException;
import java.nio.charset.Charset;
import java.nio.charset.CharsetEncoder;
import java.nio.charset.CharsetDecoder;
import java.nio.charset.CoderResult;
import java.nio.charset.CodingErrorAction;
import java.nio.charset.CharacterCodingException;
import java.nio.charset.IllegalCharsetNameException;
import java.nio.charset.UnsupportedCharsetException;
import java.nio.ByteBuffer;
import java.nio.CharBuffer;
import java.util.Locale;

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
		char[] newStr = s.toCharArray();
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
        return Character.direction[0][Character.readCodePoint((int)ch) >> 7] & 3;
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
	if (offset < 0 || count < 0 || ascii.length - offset < count)
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

    private static Charset getCharset(String encoding) throws UnsupportedEncodingException
    {
        try
        {
            return Charset.forName(encoding);
        }
        catch(IllegalCharsetNameException e)
        {
            throw (UnsupportedEncodingException)new UnsupportedEncodingException("Encoding: " + encoding + " not found.").initCause(e);
        }
        catch(UnsupportedCharsetException e)
        {
            throw (UnsupportedEncodingException)new UnsupportedEncodingException("Encoding: " + encoding + " not found.").initCause(e);
        }
    }

    static String NewString(byte[] data, int offset, int count, String encoding)
	throws UnsupportedEncodingException
    {
	if (offset < 0 || count < 0 || data.length - offset < count)
	    throw new StringIndexOutOfBoundsException();

        CharsetDecoder csd = getCharset(encoding).newDecoder();
        csd.onMalformedInput(CodingErrorAction.REPLACE);
        csd.onUnmappableCharacter(CodingErrorAction.REPLACE);
        CharBuffer out = CharBuffer.allocate(count * (int)csd.maxCharsPerByte());
        csd.decode(ByteBuffer.wrap(data, offset, count), out, true);
        csd.flush(out);
        return out.flip().toString();
    }

    static String NewString(byte[] data, String encoding)
	throws UnsupportedEncodingException
    {
	return NewString(data, 0, data.length, encoding);
    }

    static String NewString(byte[] data, int offset, int count)
    {
        try
        {
            return NewString(data, offset, count, SystemProperties.getProperty("file.encoding"));
        }
        catch(UnsupportedEncodingException e)
        {
            throw new Error(e);
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

    static String NewString(int[] codePoints, int offset, int count)
    {
        cli.System.Text.StringBuilder sb = new cli.System.Text.StringBuilder(count);
        for(int i = 0; i < count; i++)
        {
            int ch = codePoints[i + offset];
            if(ch < 0 || ch > 0x10ffff)
                throw new IllegalArgumentException();
            if(ch < 0x10000)
            {
                sb.Append((char)ch);
            }
            else
            {
                sb.Append((char)(0xd800 + ((ch - 0x10000) >> 10)));
                sb.Append((char)(0xdc00 + (ch & 0x3ff)));
            }
        }
        return sb.ToString();
    }

    static String substring(cli.System.String s, int off, int end)
    {
	return s.Substring(off, end - off);
    }

    static void getChars(cli.System.String s, int srcBegin, int srcEnd, char[] dst, int dstBegin) 
    {
	s.CopyTo(srcBegin, dst, dstBegin, srcEnd - srcBegin);
    }

    // this exposes the package accessible "count" field (for use by StringBuffer)
    static int GetCountField(cli.System.String s)
    {
	return s.get_Length();
    }

    // this exposes the package accessible "value" field (for use by StringBuffer)
    static char[] GetValueField(cli.System.String s)
    {
	return s.ToCharArray();
    }

    // this exposes the package accessible "offset" field (for use by StringBuffer)
    static int GetOffsetField(cli.System.String s)
    {
	return 0;
    }

    static int indexOf(String s, String o)
    {
        return indexOf(s, o, 0);
    }

    static int indexOf(String s, String o, int fromIndex)
    {
        // start by dereferencing s, to make sure we throw a NullPointerException if s is null
        int slen = s.length();
        int olen = o.length();
        if(olen == 0)
        {
            return Math.max(0, Math.min(fromIndex, slen));
        }
        if(olen > slen)
        {
            return -1;
        }
        char firstChar = o.charAt(0);
        // Java allows fromIndex to both below zero or above the length of the string, .NET doesn't
        int index = Math.max(0, Math.min(slen, fromIndex));
        int end = slen - olen;
        while(index >= 0 && index <= end)
        {
            if(cli.System.String.CompareOrdinal(s, index, o, 0, olen) == 0)
            {
                return index;
            }
            index = s.indexOf(firstChar, index + 1);
        }
        return -1;
    }

    static int lastIndexOf(cli.System.String s, int ch, int fromIndex)
    {
	// start by dereferencing s, to make sure we throw a NullPointerException if s is null
	int len = s.get_Length();
	if(fromIndex  < 0)
	{
	    return -1;
	}
	if(ch < 0 || ch > Character.MAX_VALUE)
	{
	    return -1;
	}
	// Java allows fromIndex to be above the length of the string, .NET doesn't
	return s.LastIndexOf((char)ch, Math.min(len - 1, fromIndex));
    }

    static int lastIndexOf(String s, String o)
    {
	return lastIndexOf(s, o, Integer.MAX_VALUE);
    }

    static int lastIndexOf(String s, String o, int fromIndex)
    {
	// start by dereferencing s, to make sure we throw a NullPointerException if s is null
	int slen = s.length();
	if(fromIndex < 0)
	{
	    return -1;
	}
        int olen = o.length();
	if(olen == 0)
	{
	    return Math.min(slen, fromIndex);
	}
        if(olen > slen)
        {
            return -1;
        }
        cli.System.String cliStr = (cli.System.String)(Object)s;
        char firstChar = o.charAt(0);
        // Java allows fromIndex to both below zero or above the length of the string, .NET doesn't
        int index = Math.max(0, Math.min(slen - olen, fromIndex));
        while(index > 0)
        {
            if(cli.System.String.CompareOrdinal(s, index, o, 0, olen) == 0)
            {
                return index;
            }
            index = cliStr.LastIndexOf(firstChar, index - 1);
        }
        return cli.System.String.CompareOrdinal(s, 0, o, 0, olen) == 0 ? 0 : -1;
    }

    static String concat(String s1, String s2)
    {
	if(s1.length() == 0)
	{
	    return s2;
	}
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
            CharsetEncoder cse = getCharset(enc).newEncoder();
            cse.onMalformedInput(CodingErrorAction.REPLACE);
            cse.onUnmappableCharacter(CodingErrorAction.REPLACE);
            char[] value = s.toCharArray();
            ByteBuffer bbuf = cse.encode(CharBuffer.wrap(value, 0, value.length));
            if(bbuf.hasArray())
                return bbuf.array();

            // Doubt this will happen. But just in case.
            byte[] bytes = new byte[bbuf.remaining()];
            bbuf.get(bytes);
            return bytes;
        }
        catch(CharacterCodingException e)
        {
            throw new Error(e);
        }
    }

    static byte[] getBytes(String s)
    {
        try 
        {
            return getBytes(s, SystemProperties.getProperty("file.encoding"));
        }
        catch(UnsupportedEncodingException e)
        {
            throw new Error(e);
        }
    }

    static boolean regionMatches(String s, int toffset, String other, int ooffset, int len)
    {
	return regionMatches(s, false, toffset, other, ooffset, len);
    }

    static boolean regionMatches(String s, boolean ignoreCase, int toffset,
	String other, int ooffset, int len)
    {
	// this explicit test is needed, because Integer.MIN_VALUE will underflow the while
	if (len < 0)
	{
	    return true;
	}
	// be careful to avoid integer overflow
	if (toffset < 0 || ooffset < 0 || s.length() - toffset < len || other.length() - ooffset < len)
	{
	    return false;
	}
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

    static cli.System.String valueOf(char c)
    {
	return new cli.System.String(c, 1);
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

    static int hashCode(cli.System.String s)
    {
	int h = 0;
	// NOTE having the get_Length in the for condition is actually faster than hoisting it,
	// the CLR JIT recognizes this pattern and optimizes the array bounds check in get_Chars.
	for(int i = 0; i < s.get_Length(); i++)
	{
	    h = h * 31 + s.get_Chars(i);
	}
	return h;
    }

    static String replace(String s, CharSequence oldValue, CharSequence newValue)
    {
        String s1 = oldValue.toString();
        String s2 = newValue.toString();
        int prev = 0;
        int pos = s.indexOf(s1);
        if(pos == -1)
        {
            return s;
        }
        int slen = s.length();
        int s1len = s1.length();
        if(s1len == 0)
        {
            cli.System.Text.StringBuilder sb = new cli.System.Text.StringBuilder((slen + 1) * (s2.length() + 1));
            for(int i = 0; i < slen; i++)
            {
                sb.Append(s2).Append(s.charAt(i));
            }
            sb.Append(s2);
            return sb.ToString();
        }
        cli.System.Text.StringBuilder sb = new cli.System.Text.StringBuilder();
        while (pos != -1)
        {
            sb.Append(s, prev, pos - prev);
            sb.Append(s2);
            prev = pos + s1len;
            pos = s.indexOf(s1, prev);
        }
        sb.Append(s, prev, slen - prev);
        return sb.ToString();
    }

    static boolean contains(String s, CharSequence seq)
    {
        return s.indexOf(seq.toString()) != -1;
    }

    static boolean contentEquals(String s, CharSequence seq)
    {
        return s.equals(seq.toString());
    }

    static int codePointAt(String s, int index)
    {
        char c1 = s.charAt(index++);
        if(c1 >= 0xd800 && c1 <= 0xdbff && index < s.length())
        {
            char c2 = s.charAt(index);
            if(c2 >= 0xdc00 && c2 <= 0xdfff)
            {
                return 0x10000 + ((c1 - 0xdb800) << 10) + c2 - 0xdc00;
            }
        }
        return c1;
    }

    static int codePointBefore(String s, int index)
    {
        char c2 = s.charAt(--index);
        if(c2 >= 0xdc00 && c2 <= 0xdfff && index > 0)
        {
            char c1 = s.charAt(--index);
            if(c1 >= 0xd800 && c1 <= 0xdbff)
            {
                return 0x10000 + ((c1 - 0xdb800) << 10) + c2 - 0xdc00;
            }
        }
        return c2;
    }

    static int codePointCount(String s, int offset, int count)
    {
        int cpc = 0;
        char prev = 0;
        for(int i = 0; i < count; i++)
        {
            char c = s.charAt(i + offset);
            if(c >= 0xdc00 && c <= 0xdfff && prev >= 0xd800 && prev <= 0xdbff)
            {
                cpc--;
            }
            prev = c;
            cpc++;
        }
        return cpc;
    }

    static int offsetByCodePoints(String s, int index, int codePointOffset)
    {
        while(codePointOffset < 0)
        {
            char c2 = s.charAt(--index);
            char c1 = index > 0 ? s.charAt(index - 1) : (char)0;
            if(c1 >= 0xd800 && c1 <= 0xdbff && c2 >= 0xdc00 && c2 <= 0xdfff)
            {
                index--;
            }
            codePointOffset++;
        }
        while(codePointOffset > 0)
        {
            char c1 = s.charAt(index++);
            char c2 = index < s.length() ? s.charAt(index) : (char)0;
            if(c1 >= 0xd800 && c1 <= 0xdbff && c2 >= 0xdc00 && c2 <= 0xdfff)
            {
                index++;
            }
            codePointOffset--;
        }
        return index;
    }
}
