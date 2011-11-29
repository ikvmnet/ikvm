/*
  Copyright (C) 2008-2011 Jeroen Frijters

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
package ikvm.extensions;

import cli.System.Runtime.CompilerServices.ExtensionAttribute;
import java.io.PrintStream;
import java.io.PrintWriter;
import java.io.UnsupportedEncodingException;
import java.nio.charset.Charset;
import java.util.Locale;

@ExtensionAttribute.Annotation
public final class ExtensionMethods
{
    private ExtensionMethods() { }

    /* java.lang.Throwable methods */

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static void addSuppressed(Throwable t, Throwable exception)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            t.addSuppressed(exception);
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static Throwable fillInStackTrace(Throwable t)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            return t.fillInStackTrace();
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static Throwable getCause(Throwable t)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            return t.getCause();
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static String getLocalizedMessage(Throwable t)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            return t.getLocalizedMessage();
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static String getMessage(Throwable t)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            return t.getMessage();
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static StackTraceElement[] getStackTrace(Throwable t)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            return t.getStackTrace();
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static Throwable[] getSuppressed(Throwable t)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            return t.getSuppressed();
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static Throwable initCause(Throwable t, Throwable cause)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            return t.initCause(cause);
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static void printStackTrace(Throwable t)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            t.printStackTrace();
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static void printStackTrace(Throwable t, PrintStream s)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            t.printStackTrace(s);
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static void printStackTrace(Throwable t, PrintWriter s)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            t.printStackTrace(s);
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    @ExtensionAttribute.Annotation
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static void setStackTrace(Throwable t, StackTraceElement[] stackTrace)
    {
        t = ikvm.runtime.Util.mapException(t);
        try
        {
            t.setStackTrace(stackTrace);
        }
        finally
        {
            ikvm.runtime.Util.unmapException(t);
        }
    }

    /* java.lang.Object methods */

    @ExtensionAttribute.Annotation
    public static Class getClass(Object obj)
    {
        return obj.getClass();
    }

    @ExtensionAttribute.Annotation
    public static void notify(Object obj)
    {
        obj.notify();
    }

    @ExtensionAttribute.Annotation
    public static void notifyAll(Object obj)
    {
        obj.notifyAll();
    }

    @ExtensionAttribute.Annotation
    public static String toString(Object obj)
    {
        return obj.toString();
    }

    @ExtensionAttribute.Annotation
    public static void wait(Object obj) throws InterruptedException
    {
        obj.wait();
    }

    @ExtensionAttribute.Annotation
    public static void wait(Object obj, long timeout, int nanos) throws InterruptedException
    {
        obj.wait(timeout, nanos);
    }

    @ExtensionAttribute.Annotation
    public static void wait(Object obj, long timeout) throws InterruptedException
    {
        obj.wait(timeout);
    }

    /* java.lang.String methods */

    @ExtensionAttribute.Annotation
    public static int hashCode(String _this)
    {
        return _this.hashCode();
    }

    @ExtensionAttribute.Annotation
    public static String substring(String _this, int beginIndex)
    {
        return _this.substring(beginIndex);
    }

    @ExtensionAttribute.Annotation
    public static int length(String _this)
    {
        return _this.length();
    }

    @ExtensionAttribute.Annotation
    public static char charAt(String _this, int index)
    {
        return _this.charAt(index);
    }

    @ExtensionAttribute.Annotation
    public static String substring(String _this, int beginIndex, int endIndex)
    {
        return _this.substring(beginIndex, endIndex);
    }

    @ExtensionAttribute.Annotation
    public static int indexOf(String _this, int ch)
    {
        return _this.indexOf(ch);
    }

    @ExtensionAttribute.Annotation
    public static int indexOf(String _this, int ch, int fromIndex)
    {
        return _this.indexOf(ch, fromIndex);
    }

    @ExtensionAttribute.Annotation
    public static int indexOf(String _this, String str)
    {
        return _this.indexOf(str);
    }

    @ExtensionAttribute.Annotation
    public static int indexOf(String _this, String str, int fromIndex)
    {
        return _this.indexOf(str, fromIndex);
    }

    @ExtensionAttribute.Annotation
    public static int lastIndexOf(String _this, int ch)
    {
        return _this.lastIndexOf(ch);
    }

    @ExtensionAttribute.Annotation
    public static int lastIndexOf(String _this, int ch, int fromIndex)
    {
        return _this.lastIndexOf(ch, fromIndex);
    }

    @ExtensionAttribute.Annotation
    public static int lastIndexOf(String _this, String str)
    {
        return _this.lastIndexOf(str);
    }

    @ExtensionAttribute.Annotation
    public static int lastIndexOf(String _this, String str, int fromIndex)
    {
        return _this.lastIndexOf(str, fromIndex);
    }

    @ExtensionAttribute.Annotation
    public static char[] toCharArray(String _this)
    {
        return _this.toCharArray();
    }

    @ExtensionAttribute.Annotation
    public static void getChars(String _this, int srcBegin, int srcEnd, char[] dst, int dstBegin)
    {
        _this.getChars(srcBegin, srcEnd, dst, dstBegin);
    }

    @ExtensionAttribute.Annotation
    public static boolean startsWith(String _this, String prefix)
    {
        return _this.startsWith(prefix);
    }

    @ExtensionAttribute.Annotation
    public static boolean startsWith(String _this, String prefix, int toffset)
    {
        return _this.startsWith(prefix, toffset);
    }

    @ExtensionAttribute.Annotation
    public static boolean endsWith(String _this, String suffix)
    {
        return _this.endsWith(suffix);
    }

    @ExtensionAttribute.Annotation
    public static String toUpperCase(String _this)
    {
        return _this.toUpperCase();
    }

    @ExtensionAttribute.Annotation
    public static String toUpperCase(String _this, Locale locale)
    {
        return _this.toUpperCase(locale);
    }

    @ExtensionAttribute.Annotation
    public static String toLowerCase(String _this)
    {
        return _this.toLowerCase();
    }

    @ExtensionAttribute.Annotation
    public static String toLowerCase(String _this, Locale locale)
    {
        return _this.toLowerCase(locale);
    }

    @ExtensionAttribute.Annotation
    public static int compareToIgnoreCase(String _this, String str)
    {
        return _this.compareToIgnoreCase(str);
    }

    @ExtensionAttribute.Annotation
    public static boolean equalsIgnoreCase(String _this, String anotherString)
    {
        return _this.equalsIgnoreCase(anotherString);
    }

    @ExtensionAttribute.Annotation
    public static String intern(String _this)
    {
        return _this.intern();
    }

    @ExtensionAttribute.Annotation
    public static int compareTo(String _this, String anotherString)
    {
        return _this.compareTo(anotherString);
    }

    @ExtensionAttribute.Annotation
    public static String replace(String _this, char oldChar, char newChar)
    {
        return _this.replace(oldChar, newChar);
    }

    @ExtensionAttribute.Annotation
    public static byte[] getBytes(String _this)
    {
        return _this.getBytes();
    }

    @ExtensionAttribute.Annotation
    public static byte[] getBytes(String _this, String charsetName) throws UnsupportedEncodingException
    {
        return _this.getBytes(charsetName);
    }

    @ExtensionAttribute.Annotation
    public static CharSequence subSequence(String _this, int beginIndex, int endIndex)
    {
        return _this.subSequence(beginIndex, endIndex);
    }

    @ExtensionAttribute.Annotation
    public static String trim(String _this)
    {
        return _this.trim();
    }

    @ExtensionAttribute.Annotation
    public static boolean regionMatches(String _this, boolean ignoreCase, int toffset, String other, int ooffset, int len)
    {
        return _this.regionMatches(ignoreCase, toffset, other, ooffset, len);
    }

    @ExtensionAttribute.Annotation
    public static boolean regionMatches(String _this, int toffset, String other, int ooffset, int len)
    {
        return _this.regionMatches(toffset, other, ooffset, len);
    }

    @ExtensionAttribute.Annotation
    public static void getBytes(String _this, int srcBegin, int srcEnd, byte[] dst, int dstBegin)
    {
        _this.getBytes(srcBegin, srcEnd, dst, dstBegin);
    }

    @ExtensionAttribute.Annotation
    public static String concat(String _this, String str)
    {
        return _this.concat(str);
    }

    @ExtensionAttribute.Annotation
    public static boolean contains(String _this, CharSequence s)
    {
        return _this.contains(s);
    }

    @ExtensionAttribute.Annotation
    public static int codePointAt(String _this, int index)
    {
        return _this.codePointAt(index);
    }

    @ExtensionAttribute.Annotation
    public static int codePointBefore(String _this, int index)
    {
        return _this.codePointBefore(index);
    }

    @ExtensionAttribute.Annotation
    public static int codePointCount(String _this, int beginIndex, int endIndex)
    {
        return _this.codePointCount(beginIndex, endIndex);
    }

    @ExtensionAttribute.Annotation
    public static int offsetByCodePoints(String _this, int index, int codePointOffset)
    {
        return _this.offsetByCodePoints(index, codePointOffset);
    }

    @ExtensionAttribute.Annotation
    public static boolean contentEquals(String _this, CharSequence cs)
    {
        return _this.contentEquals(cs);
    }

    @ExtensionAttribute.Annotation
    public static boolean contentEquals(String _this, StringBuffer sb)
    {
        return _this.contentEquals(sb);
    }

    @ExtensionAttribute.Annotation
    public static String replace(String _this, CharSequence target, CharSequence replacement)
    {
        return _this.replace(target, replacement);
    }

    @ExtensionAttribute.Annotation
    public static boolean matches(String _this, String regex)
    {
        return _this.matches(regex);
    }

    @ExtensionAttribute.Annotation
    public static String replaceAll(String _this, String regex, String replacement)
    {
        return _this.replaceAll(regex, replacement);
    }

    @ExtensionAttribute.Annotation
    public static String replaceFirst(String _this, String regex, String replacement)
    {
        return _this.replaceFirst(regex, replacement);
    }

    @ExtensionAttribute.Annotation
    public static String[] split(String _this, String regex)
    {
        return _this.split(regex);
    }

    @ExtensionAttribute.Annotation
    public static String[] split(String _this, String regex, int limit)
    {
        return _this.split(regex, limit);
    }

    @ExtensionAttribute.Annotation
    public static boolean isEmpty(String _this)
    {
        return _this.isEmpty();
    }

    @ExtensionAttribute.Annotation
    public static byte[] getBytes(String _this, Charset charset)
    {
        return _this.getBytes(charset);
    }
}
