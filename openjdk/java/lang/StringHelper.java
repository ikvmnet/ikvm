/*
 * Copyright (c) 1994, 2013, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

package java.lang;

import java.io.ObjectStreamField;
import java.io.UnsupportedEncodingException;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.Formatter;
import java.util.Locale;
import java.util.Objects;
import java.util.StringJoiner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.regex.PatternSyntaxException;

/**
 * The {@code String} class represents character strings. All
 * string literals in Java programs, such as {@code "abc"}, are
 * implemented as instances of this class.
 * <p>
 * Strings are constant; their values cannot be changed after they
 * are created. String buffers support mutable strings.
 * Because String objects are immutable they can be shared. For example:
 * <blockquote><pre>
 *     String str = "abc";
 * </pre></blockquote><p>
 * is equivalent to:
 * <blockquote><pre>
 *     char data[] = {'a', 'b', 'c'};
 *     String str = new String(data);
 * </pre></blockquote><p>
 * Here are some more examples of how strings can be used:
 * <blockquote><pre>
 *     System.out.println("abc");
 *     String cde = "cde";
 *     System.out.println("abc" + cde);
 *     String c = "abc".substring(2,3);
 *     String d = cde.substring(1, 2);
 * </pre></blockquote>
 * <p>
 * The class {@code String} includes methods for examining
 * individual characters of the sequence, for comparing strings, for
 * searching strings, for extracting substrings, and for creating a
 * copy of a string with all characters translated to uppercase or to
 * lowercase. Case mapping is based on the Unicode Standard version
 * specified by the {@link java.lang.Character Character} class.
 * <p>
 * The Java language provides special support for the string
 * concatenation operator (&nbsp;+&nbsp;), and for conversion of
 * other objects to strings. String concatenation is implemented
 * through the {@code StringBuilder}(or {@code StringBuffer})
 * class and its {@code append} method.
 * String conversions are implemented through the method
 * {@code toString}, defined by {@code Object} and
 * inherited by all classes in Java. For additional information on
 * string concatenation and conversion, see Gosling, Joy, and Steele,
 * <i>The Java Language Specification</i>.
 *
 * <p> Unless otherwise noted, passing a <tt>null</tt> argument to a constructor
 * or method in this class will cause a {@link NullPointerException} to be
 * thrown.
 *
 * <p>A {@code String} represents a string in the UTF-16 format
 * in which <em>supplementary characters</em> are represented by <em>surrogate
 * pairs</em> (see the section <a href="Character.html#unicode">Unicode
 * Character Representations</a> in the {@code Character} class for
 * more information).
 * Index values refer to {@code char} code units, so a supplementary
 * character uses two positions in a {@code String}.
 * <p>The {@code String} class provides methods for dealing with
 * Unicode code points (i.e., characters), in addition to those for
 * dealing with Unicode code units (i.e., {@code char} values).
 *
 * @author  Lee Boynton
 * @author  Arthur van Hoff
 * @author  Martin Buchholz
 * @author  Ulf Zibis
 * @see     java.lang.Object#toString()
 * @see     java.lang.StringBuffer
 * @see     java.lang.StringBuilder
 * @see     java.nio.charset.Charset
 * @since   JDK1.0
 */

final class StringHelper
{
    /**
     * Allocates a new {@code String} that contains characters from a subarray
     * of the <a href="Character.html#unicode">Unicode code point</a> array
     * argument.  The {@code offset} argument is the index of the first code
     * point of the subarray and the {@code count} argument specifies the
     * length of the subarray.  The contents of the subarray are converted to
     * {@code char}s; subsequent modification of the {@code int} array does not
     * affect the newly created string.
     *
     * @param  codePoints
     *         Array that is the source of Unicode code points
     *
     * @param  offset
     *         The initial offset
     *
     * @param  count
     *         The length
     *
     * @throws  IllegalArgumentException
     *          If any invalid Unicode code point is found in {@code
     *          codePoints}
     *
     * @throws  IndexOutOfBoundsException
     *          If the {@code offset} and {@code count} arguments index
     *          characters outside the bounds of the {@code codePoints} array
     *
     * @since  1.5
     */
    static String NewString(int[] codePoints, int offset, int count) {
        if (offset < 0) {
            throw new StringIndexOutOfBoundsException(offset);
        }
        if (count < 0) {
            throw new StringIndexOutOfBoundsException(count);
        }
        // Note: offset or count might be near -1>>>1.
        if (offset > codePoints.length - count) {
            throw new StringIndexOutOfBoundsException(offset + count);
        }

        final int end = offset + count;

        // Pass 1: Compute precise size of char[]
        int n = count;
        for (int i = offset; i < end; i++) {
            int c = codePoints[i];
            if (Character.isBmpCodePoint(c))
                continue;
            else if (Character.isValidCodePoint(c))
                n++;
            else throw new IllegalArgumentException(Integer.toString(c));
        }

        // Pass 2: Allocate and fill in char[]
        final char[] v = new char[n];

        for (int i = offset, j = 0; i < end; i++, j++) {
            int c = codePoints[i];
            if (Character.isBmpCodePoint(c))
                v[j] = (char)c;
            else
                Character.toSurrogates(c, v, j++);
        }

        return new String(v);
    }

    /**
     * Allocates a new {@code String} constructed from a subarray of an array
     * of 8-bit integer values.
     *
     * <p> The {@code offset} argument is the index of the first byte of the
     * subarray, and the {@code count} argument specifies the length of the
     * subarray.
     *
     * <p> Each {@code byte} in the subarray is converted to a {@code char} as
     * specified in the method above.
     *
     * @deprecated This method does not properly convert bytes into characters.
     * As of JDK&nbsp;1.1, the preferred way to do this is via the
     * {@code String} constructors that take a {@link
     * java.nio.charset.Charset}, charset name, or that use the platform's
     * default charset.
     *
     * @param  ascii
     *         The bytes to be converted to characters
     *
     * @param  hibyte
     *         The top 8 bits of each 16-bit Unicode code unit
     *
     * @param  offset
     *         The initial offset
     * @param  count
     *         The length
     *
     * @throws  IndexOutOfBoundsException
     *          If the {@code offset} or {@code count} argument is invalid
     *
     * @see  #String(byte[], int)
     * @see  #String(byte[], int, int, java.lang.String)
     * @see  #String(byte[], int, int, java.nio.charset.Charset)
     * @see  #String(byte[], int, int)
     * @see  #String(byte[], java.lang.String)
     * @see  #String(byte[], java.nio.charset.Charset)
     * @see  #String(byte[])
     */
    @Deprecated
    static String NewString(byte ascii[], int hibyte, int offset, int count) {
        checkBounds(ascii, offset, count);
        char value[] = new char[count];

        if (hibyte == 0) {
            for (int i = count; i-- > 0;) {
                value[i] = (char)(ascii[i + offset] & 0xff);
            }
        } else {
            hibyte <<= 8;
            for (int i = count; i-- > 0;) {
                value[i] = (char)(hibyte | (ascii[i + offset] & 0xff));
            }
        }
        return new String(value, 0, count);
    }

    /**
     * Allocates a new {@code String} containing characters constructed from
     * an array of 8-bit integer values. Each character <i>c</i>in the
     * resulting string is constructed from the corresponding component
     * <i>b</i> in the byte array such that:
     *
     * <blockquote><pre>
     *     <b><i>c</i></b> == (char)(((hibyte &amp; 0xff) &lt;&lt; 8)
     *                         | (<b><i>b</i></b> &amp; 0xff))
     * </pre></blockquote>
     *
     * @deprecated  This method does not properly convert bytes into
     * characters.  As of JDK&nbsp;1.1, the preferred way to do this is via the
     * {@code String} constructors that take a {@link
     * java.nio.charset.Charset}, charset name, or that use the platform's
     * default charset.
     *
     * @param  ascii
     *         The bytes to be converted to characters
     *
     * @param  hibyte
     *         The top 8 bits of each 16-bit Unicode code unit
     *
     * @see  #String(byte[], int, int, java.lang.String)
     * @see  #String(byte[], int, int, java.nio.charset.Charset)
     * @see  #String(byte[], int, int)
     * @see  #String(byte[], java.lang.String)
     * @see  #String(byte[], java.nio.charset.Charset)
     * @see  #String(byte[])
     */
    @Deprecated
    static String NewString(byte ascii[], int hibyte) {
        return NewString(ascii, hibyte, 0, ascii.length);
    }

    /* Common private utility method used to bounds check the byte array
     * and requested offset & length values used by the String(byte[],..)
     * constructors.
     */
    private static void checkBounds(byte[] bytes, int offset, int length) {
        if (length < 0)
            throw new StringIndexOutOfBoundsException(length);
        if (offset < 0)
            throw new StringIndexOutOfBoundsException(offset);
        if (offset > bytes.length - length)
            throw new StringIndexOutOfBoundsException(offset + length);
    }

    /**
     * Constructs a new {@code String} by decoding the specified subarray of
     * bytes using the specified charset.  The length of the new {@code String}
     * is a function of the charset, and hence may not be equal to the length
     * of the subarray.
     *
     * <p> The behavior of this constructor when the given bytes are not valid
     * in the given charset is unspecified.  The {@link
     * java.nio.charset.CharsetDecoder} class should be used when more control
     * over the decoding process is required.
     *
     * @param  bytes
     *         The bytes to be decoded into characters
     *
     * @param  offset
     *         The index of the first byte to decode
     *
     * @param  length
     *         The number of bytes to decode

     * @param  charsetName
     *         The name of a supported {@linkplain java.nio.charset.Charset
     *         charset}
     *
     * @throws  UnsupportedEncodingException
     *          If the named charset is not supported
     *
     * @throws  IndexOutOfBoundsException
     *          If the {@code offset} and {@code length} arguments index
     *          characters outside the bounds of the {@code bytes} array
     *
     * @since  JDK1.1
     */
    static String NewString(byte bytes[], int offset, int length, String charsetName)
            throws UnsupportedEncodingException {
        if (charsetName == null)
            throw new NullPointerException("charsetName");
        checkBounds(bytes, offset, length);
        char[] v = StringCoding.decode(charsetName, bytes, offset, length);
        return new String(v);
    }

    /**
     * Constructs a new {@code String} by decoding the specified subarray of
     * bytes using the specified {@linkplain java.nio.charset.Charset charset}.
     * The length of the new {@code String} is a function of the charset, and
     * hence may not be equal to the length of the subarray.
     *
     * <p> This method always replaces malformed-input and unmappable-character
     * sequences with this charset's default replacement string.  The {@link
     * java.nio.charset.CharsetDecoder} class should be used when more control
     * over the decoding process is required.
     *
     * @param  bytes
     *         The bytes to be decoded into characters
     *
     * @param  offset
     *         The index of the first byte to decode
     *
     * @param  length
     *         The number of bytes to decode
     *
     * @param  charset
     *         The {@linkplain java.nio.charset.Charset charset} to be used to
     *         decode the {@code bytes}
     *
     * @throws  IndexOutOfBoundsException
     *          If the {@code offset} and {@code length} arguments index
     *          characters outside the bounds of the {@code bytes} array
     *
     * @since  1.6
     */
    static String NewString(byte bytes[], int offset, int length, Charset charset) {
        if (charset == null)
            throw new NullPointerException("charset");
        checkBounds(bytes, offset, length);
        char[] v = StringCoding.decode(charset, bytes, offset, length);
        return new String(v);
    }

    /**
     * Constructs a new {@code String} by decoding the specified array of bytes
     * using the specified {@linkplain java.nio.charset.Charset charset}.  The
     * length of the new {@code String} is a function of the charset, and hence
     * may not be equal to the length of the byte array.
     *
     * <p> The behavior of this constructor when the given bytes are not valid
     * in the given charset is unspecified.  The {@link
     * java.nio.charset.CharsetDecoder} class should be used when more control
     * over the decoding process is required.
     *
     * @param  bytes
     *         The bytes to be decoded into characters
     *
     * @param  charsetName
     *         The name of a supported {@linkplain java.nio.charset.Charset
     *         charset}
     *
     * @throws  UnsupportedEncodingException
     *          If the named charset is not supported
     *
     * @since  JDK1.1
     */
    static String NewString(byte bytes[], String charsetName)
            throws UnsupportedEncodingException {
        return NewString(bytes, 0, bytes.length, charsetName);
    }

    /**
     * Constructs a new {@code String} by decoding the specified array of
     * bytes using the specified {@linkplain java.nio.charset.Charset charset}.
     * The length of the new {@code String} is a function of the charset, and
     * hence may not be equal to the length of the byte array.
     *
     * <p> This method always replaces malformed-input and unmappable-character
     * sequences with this charset's default replacement string.  The {@link
     * java.nio.charset.CharsetDecoder} class should be used when more control
     * over the decoding process is required.
     *
     * @param  bytes
     *         The bytes to be decoded into characters
     *
     * @param  charset
     *         The {@linkplain java.nio.charset.Charset charset} to be used to
     *         decode the {@code bytes}
     *
     * @since  1.6
     */
    static String NewString(byte bytes[], Charset charset) {
        return NewString(bytes, 0, bytes.length, charset);
    }

    /**
     * Constructs a new {@code String} by decoding the specified subarray of
     * bytes using the platform's default charset.  The length of the new
     * {@code String} is a function of the charset, and hence may not be equal
     * to the length of the subarray.
     *
     * <p> The behavior of this constructor when the given bytes are not valid
     * in the default charset is unspecified.  The {@link
     * java.nio.charset.CharsetDecoder} class should be used when more control
     * over the decoding process is required.
     *
     * @param  bytes
     *         The bytes to be decoded into characters
     *
     * @param  offset
     *         The index of the first byte to decode
     *
     * @param  length
     *         The number of bytes to decode
     *
     * @throws  IndexOutOfBoundsException
     *          If the {@code offset} and the {@code length} arguments index
     *          characters outside the bounds of the {@code bytes} array
     *
     * @since  JDK1.1
     */
    static String NewString(byte bytes[], int offset, int length) {
        checkBounds(bytes, offset, length);
        char[] v  = StringCoding.decode(bytes, offset, length);
        return new String(v);
    }

    /**
     * Constructs a new {@code String} by decoding the specified array of bytes
     * using the platform's default charset.  The length of the new {@code
     * String} is a function of the charset, and hence may not be equal to the
     * length of the byte array.
     *
     * <p> The behavior of this constructor when the given bytes are not valid
     * in the default charset is unspecified.  The {@link
     * java.nio.charset.CharsetDecoder} class should be used when more control
     * over the decoding process is required.
     *
     * @param  bytes
     *         The bytes to be decoded into characters
     *
     * @since  JDK1.1
     */
    static String NewString(byte bytes[]) {
        return NewString(bytes, 0, bytes.length);
    }

    /**
     * Allocates a new string that contains the sequence of characters
     * currently contained in the string buffer argument. The contents of the
     * string buffer are copied; subsequent modification of the string buffer
     * does not affect the newly created string.
     *
     * @param  buffer
     *         A {@code StringBuffer}
     */
    static String NewString(StringBuffer buffer) {
        return buffer.toString();
    }

    /**
     * Allocates a new string that contains the sequence of characters
     * currently contained in the string builder argument. The contents of the
     * string builder are copied; subsequent modification of the string builder
     * does not affect the newly created string.
     *
     * <p> This constructor is provided to ease migration to {@code
     * StringBuilder}. Obtaining a string from a string builder via the {@code
     * toString} method is likely to run faster and is generally preferred.
     *
     * @param   builder
     *          A {@code StringBuilder}
     *
     * @since  1.5
     */
    static String NewString(StringBuilder builder) {
        return builder.toString();
    }

    /**
     * Returns the character (Unicode code point) at the specified
     * index. The index refers to {@code char} values
     * (Unicode code units) and ranges from {@code 0} to
     * {@link #length()}{@code  - 1}.
     *
     * <p> If the {@code char} value specified at the given index
     * is in the high-surrogate range, the following index is less
     * than the length of this {@code String}, and the
     * {@code char} value at the following index is in the
     * low-surrogate range, then the supplementary code point
     * corresponding to this surrogate pair is returned. Otherwise,
     * the {@code char} value at the given index is returned.
     *
     * @param      index the index to the {@code char} values
     * @return     the code point value of the character at the
     *             {@code index}
     * @exception  IndexOutOfBoundsException  if the {@code index}
     *             argument is negative or not less than the length of this
     *             string.
     * @since      1.5
     */
    static int codePointAt(String _this, int index) {
        if ((index < 0) || (index >= _this.length())) {
            throw new StringIndexOutOfBoundsException(index);
        }
        char c1 = _this.charAt(index++);
        if (Character.isHighSurrogate(c1)) {
            if (index < _this.length()) {
                char c2 = _this.charAt(index);
                if (Character.isLowSurrogate(c2)) {
                    return Character.toCodePoint(c1, c2);
                }
            }
        }
        return c1;
    }

    /**
     * Returns the character (Unicode code point) before the specified
     * index. The index refers to {@code char} values
     * (Unicode code units) and ranges from {@code 1} to {@link
     * CharSequence#length() length}.
     *
     * <p> If the {@code char} value at {@code (index - 1)}
     * is in the low-surrogate range, {@code (index - 2)} is not
     * negative, and the {@code char} value at {@code (index -
     * 2)} is in the high-surrogate range, then the
     * supplementary code point value of the surrogate pair is
     * returned. If the {@code char} value at {@code index -
     * 1} is an unpaired low-surrogate or a high-surrogate, the
     * surrogate value is returned.
     *
     * @param     index the index following the code point that should be returned
     * @return    the Unicode code point value before the given index.
     * @exception IndexOutOfBoundsException if the {@code index}
     *            argument is less than 1 or greater than the length
     *            of this string.
     * @since     1.5
     */
    static int codePointBefore(String _this, int index) {
        int i = index - 1;
        if ((i < 0) || (i >= _this.length())) {
            throw new StringIndexOutOfBoundsException(index);
        }
        char c2 = _this.charAt(--index);
        if (Character.isLowSurrogate(c2)) {
            if (index > 0) {
                char c1 = _this.charAt(--index);
                if (Character.isHighSurrogate(c1)) {
                    return Character.toCodePoint(c1, c2);
                }
            }
        }
        return c2;
    }

    /**
     * Returns the number of Unicode code points in the specified text
     * range of this {@code String}. The text range begins at the
     * specified {@code beginIndex} and extends to the
     * {@code char} at index {@code endIndex - 1}. Thus the
     * length (in {@code char}s) of the text range is
     * {@code endIndex-beginIndex}. Unpaired surrogates within
     * the text range count as one code point each.
     *
     * @param beginIndex the index to the first {@code char} of
     * the text range.
     * @param endIndex the index after the last {@code char} of
     * the text range.
     * @return the number of Unicode code points in the specified text
     * range
     * @exception IndexOutOfBoundsException if the
     * {@code beginIndex} is negative, or {@code endIndex}
     * is larger than the length of this {@code String}, or
     * {@code beginIndex} is larger than {@code endIndex}.
     * @since  1.5
     */
    static int codePointCount(String _this, int beginIndex, int endIndex) {
        if (beginIndex < 0 || endIndex > _this.length() || beginIndex > endIndex) {
            throw new IndexOutOfBoundsException();
        }
        int n = 0;
        for (int i = beginIndex; i < endIndex; ) {
            n++;
            if (Character.isHighSurrogate(_this.charAt(i++))) {
                if (i < endIndex && Character.isLowSurrogate(_this.charAt(i))) {
                    i++;
                }
            }
        }
        return n;
    }

    /**
     * Returns the index within this {@code String} that is
     * offset from the given {@code index} by
     * {@code codePointOffset} code points. Unpaired surrogates
     * within the text range given by {@code index} and
     * {@code codePointOffset} count as one code point each.
     *
     * @param index the index to be offset
     * @param codePointOffset the offset in code points
     * @return the index within this {@code String}
     * @exception IndexOutOfBoundsException if {@code index}
     *   is negative or larger then the length of this
     *   {@code String}, or if {@code codePointOffset} is positive
     *   and the substring starting with {@code index} has fewer
     *   than {@code codePointOffset} code points,
     *   or if {@code codePointOffset} is negative and the substring
     *   before {@code index} has fewer than the absolute value
     *   of {@code codePointOffset} code points.
     * @since 1.5
     */
    static int offsetByCodePoints(String _this, int index, int codePointOffset) {
        int count = _this.length();
        if (index < 0 || index > count) {
            throw new IndexOutOfBoundsException();
        }
        int x = index;
        if (codePointOffset >= 0) {
            int limit = count;
            int i;
            for (i = 0; x < limit && i < codePointOffset; i++) {
                if (Character.isHighSurrogate(_this.charAt(x++))) {
                    if (x < limit && Character.isLowSurrogate(_this.charAt(x))) {
                        x++;
                    }
                }
            }
            if (i < codePointOffset) {
                throw new IndexOutOfBoundsException();
            }
        } else {
            int i;
            for (i = codePointOffset; x > 0 && i < 0; i++) {
                if (Character.isLowSurrogate(_this.charAt(--x))) {
                    if (x > 0 && Character.isHighSurrogate(_this.charAt(x-1))) {
                        x--;
                    }
                }
            } 
            if (i < 0) {
                throw new IndexOutOfBoundsException();
            }
        }
        return x;
    }

    /**
     * Copy characters from this string into dst starting at dstBegin.
     * This method doesn't perform any range checking.
     */
    static void getChars(String _this, char dst[], int dstBegin) {
        _this.getChars(0, _this.length(), dst, dstBegin);
    }

    /**
     * Copies characters from this string into the destination character
     * array.
     * <p>
     * The first character to be copied is at index {@code srcBegin};
     * the last character to be copied is at index {@code srcEnd-1}
     * (thus the total number of characters to be copied is
     * {@code srcEnd-srcBegin}). The characters are copied into the
     * subarray of {@code dst} starting at index {@code dstBegin}
     * and ending at index:
     * <blockquote><pre>
     *     dstbegin + (srcEnd-srcBegin) - 1
     * </pre></blockquote>
     *
     * @param      srcBegin   index of the first character in the string
     *                        to copy.
     * @param      srcEnd     index after the last character in the string
     *                        to copy.
     * @param      dst        the destination array.
     * @param      dstBegin   the start offset in the destination array.
     * @exception IndexOutOfBoundsException If any of the following
     *            is true:
     *            <ul><li>{@code srcBegin} is negative.
     *            <li>{@code srcBegin} is greater than {@code srcEnd}
     *            <li>{@code srcEnd} is greater than the length of this
     *                string
     *            <li>{@code dstBegin} is negative
     *            <li>{@code dstBegin+(srcEnd-srcBegin)} is larger than
     *                {@code dst.length}</ul>
     */
    static void getChars(cli.System.String _this, int srcBegin, int srcEnd, char dst[], int dstBegin) {
        if (srcBegin < 0) {
            throw new StringIndexOutOfBoundsException(srcBegin);
        }
        if (srcEnd > _this.get_Length()) {
            throw new StringIndexOutOfBoundsException(srcEnd);
        }
        if (srcBegin > srcEnd) {
            throw new StringIndexOutOfBoundsException(srcEnd - srcBegin);
        }
        _this.CopyTo(srcBegin, dst, dstBegin, srcEnd - srcBegin);
    }

    /**
     * Copies characters from this string into the destination byte array. Each
     * byte receives the 8 low-order bits of the corresponding character. The
     * eight high-order bits of each character are not copied and do not
     * participate in the transfer in any way.
     *
     * <p> The first character to be copied is at index {@code srcBegin}; the
     * last character to be copied is at index {@code srcEnd-1}.  The total
     * number of characters to be copied is {@code srcEnd-srcBegin}. The
     * characters, converted to bytes, are copied into the subarray of {@code
     * dst} starting at index {@code dstBegin} and ending at index:
     *
     * <blockquote><pre>
     *     dstbegin + (srcEnd-srcBegin) - 1
     * </pre></blockquote>
     *
     * @deprecated  This method does not properly convert characters into
     * bytes.  As of JDK&nbsp;1.1, the preferred way to do this is via the
     * {@link #getBytes()} method, which uses the platform's default charset.
     *
     * @param  srcBegin
     *         Index of the first character in the string to copy
     *
     * @param  srcEnd
     *         Index after the last character in the string to copy
     *
     * @param  dst
     *         The destination array
     *
     * @param  dstBegin
     *         The start offset in the destination array
     *
     * @throws  IndexOutOfBoundsException
     *          If any of the following is true:
     *          <ul>
     *            <li> {@code srcBegin} is negative
     *            <li> {@code srcBegin} is greater than {@code srcEnd}
     *            <li> {@code srcEnd} is greater than the length of this String
     *            <li> {@code dstBegin} is negative
     *            <li> {@code dstBegin+(srcEnd-srcBegin)} is larger than {@code
     *                 dst.length}
     *          </ul>
     */
    @Deprecated
    static void getBytes(String _this, int srcBegin, int srcEnd, byte dst[], int dstBegin) {
        if (srcBegin < 0) {
            throw new StringIndexOutOfBoundsException(srcBegin);
        }
        if (srcEnd > _this.length()) {
            throw new StringIndexOutOfBoundsException(srcEnd);
        }
        if (srcBegin > srcEnd) {
            throw new StringIndexOutOfBoundsException(srcEnd - srcBegin);
        }
        Objects.requireNonNull(dst);

        int j = dstBegin;
        int n = srcEnd;
        int i = srcBegin;

        while (i < n) {
            dst[j++] = (byte)_this.charAt(i++);
        }
    }

    /**
     * Encodes this {@code String} into a sequence of bytes using the named
     * charset, storing the result into a new byte array.
     *
     * <p> The behavior of this method when this string cannot be encoded in
     * the given charset is unspecified.  The {@link
     * java.nio.charset.CharsetEncoder} class should be used when more control
     * over the encoding process is required.
     *
     * @param  charsetName
     *         The name of a supported {@linkplain java.nio.charset.Charset
     *         charset}
     *
     * @return  The resultant byte array
     *
     * @throws  UnsupportedEncodingException
     *          If the named charset is not supported
     *
     * @since  JDK1.1
     */
    static byte[] getBytes(String _this, String charsetName)
            throws UnsupportedEncodingException {
        if (charsetName == null) throw new NullPointerException();
        char[] value = _this.toCharArray();
        return StringCoding.encode(charsetName, value, 0, value.length);
    }

    /**
     * Encodes this {@code String} into a sequence of bytes using the given
     * {@linkplain java.nio.charset.Charset charset}, storing the result into a
     * new byte array.
     *
     * <p> This method always replaces malformed-input and unmappable-character
     * sequences with this charset's default replacement byte array.  The
     * {@link java.nio.charset.CharsetEncoder} class should be used when more
     * control over the encoding process is required.
     *
     * @param  charset
     *         The {@linkplain java.nio.charset.Charset} to be used to encode
     *         the {@code String}
     *
     * @return  The resultant byte array
     *
     * @since  1.6
     */
    static byte[] getBytes(String _this, Charset charset) {
        if (charset == null) throw new NullPointerException();
        char[] value = _this.toCharArray();
        return StringCoding.encode(charset, value, 0, value.length);
    }

    /**
     * Encodes this {@code String} into a sequence of bytes using the
     * platform's default charset, storing the result into a new byte array.
     *
     * <p> The behavior of this method when this string cannot be encoded in
     * the default charset is unspecified.  The {@link
     * java.nio.charset.CharsetEncoder} class should be used when more control
     * over the encoding process is required.
     *
     * @return  The resultant byte array
     *
     * @since      JDK1.1
     */
    static byte[] getBytes(String _this) {
        char[] value = _this.toCharArray();
        return StringCoding.encode(value, 0, value.length);
    }

    /**
     * Compares this string to the specified {@code StringBuffer}.  The result
     * is {@code true} if and only if this {@code String} represents the same
     * sequence of characters as the specified {@code StringBuffer}. This method
     * synchronizes on the {@code StringBuffer}.
     *
     * @param  sb
     *         The {@code StringBuffer} to compare this {@code String} against
     *
     * @return  {@code true} if this {@code String} represents the same
     *          sequence of characters as the specified {@code StringBuffer},
     *          {@code false} otherwise
     *
     * @since  1.4
     */
    static boolean contentEquals(String _this, StringBuffer sb) {
        return contentEquals(_this, (CharSequence)sb);
    }

    private static boolean nonSyncContentEquals(String _this, AbstractStringBuilder sb) {
        char v2[] = sb.getValue();
        int n = _this.length();
        if (n != sb.length()) {
            return false;
        }
        for (int i = 0; i < n; i++) {
            if (_this.charAt(i) != v2[i]) {
                return false;
            }
        }
        return true;
    }

    /**
     * Compares this string to the specified {@code CharSequence}.  The
     * result is {@code true} if and only if this {@code String} represents the
     * same sequence of char values as the specified sequence. Note that if the
     * {@code CharSequence} is a {@code StringBuffer} then the method
     * synchronizes on it.
     *
     * @param  cs
     *         The sequence to compare this {@code String} against
     *
     * @return  {@code true} if this {@code String} represents the same
     *          sequence of char values as the specified sequence, {@code
     *          false} otherwise
     *
     * @since  1.5
     */
    static boolean contentEquals(String _this, CharSequence cs) {
        // Argument is a StringBuffer, StringBuilder
        if (cs instanceof AbstractStringBuilder) {
            if (cs instanceof StringBuffer) {
                synchronized(cs) {
                   return nonSyncContentEquals(_this, (AbstractStringBuilder)cs);
                }
            } else {
                return nonSyncContentEquals(_this, (AbstractStringBuilder)cs);
            }
        }
        // Argument is a String
        if (cs instanceof String) {
            return _this.equals(cs);
        }
        // Argument is a generic CharSequence
        int n = _this.length();
        if (n != cs.length()) {
            return false;
        }
        for (int i = 0; i < n; i++) {
            if (_this.charAt(i) != cs.charAt(i)) {
                return false;
            }
        }
        return true;
    }

    /**
     * Compares this {@code String} to another {@code String}, ignoring case
     * considerations.  Two strings are considered equal ignoring case if they
     * are of the same length and corresponding characters in the two strings
     * are equal ignoring case.
     *
     * <p> Two characters {@code c1} and {@code c2} are considered the same
     * ignoring case if at least one of the following is true:
     * <ul>
     *   <li> The two characters are the same (as compared by the
     *        {@code ==} operator)
     *   <li> Applying the method {@link
     *        java.lang.Character#toUpperCase(char)} to each character
     *        produces the same result
     *   <li> Applying the method {@link
     *        java.lang.Character#toLowerCase(char)} to each character
     *        produces the same result
     * </ul>
     *
     * @param  anotherString
     *         The {@code String} to compare this {@code String} against
     *
     * @return  {@code true} if the argument is not {@code null} and it
     *          represents an equivalent {@code String} ignoring case; {@code
     *          false} otherwise
     *
     * @see  #equals(Object)
     */
    static boolean equalsIgnoreCase(String _this, String anotherString) {
        return (_this == anotherString) ? true
                : (anotherString != null)
                && (anotherString.length() == _this.length())
                && regionMatches(_this, true, 0, anotherString, 0, _this.length());
    }

    /**
     * Compares two strings lexicographically.
     * The comparison is based on the Unicode value of each character in
     * the strings. The character sequence represented by this
     * {@code String} object is compared lexicographically to the
     * character sequence represented by the argument string. The result is
     * a negative integer if this {@code String} object
     * lexicographically precedes the argument string. The result is a
     * positive integer if this {@code String} object lexicographically
     * follows the argument string. The result is zero if the strings
     * are equal; {@code compareTo} returns {@code 0} exactly when
     * the {@link #equals(Object)} method would return {@code true}.
     * <p>
     * This is the definition of lexicographic ordering. If two strings are
     * different, then either they have different characters at some index
     * that is a valid index for both strings, or their lengths are different,
     * or both. If they have different characters at one or more index
     * positions, let <i>k</i> be the smallest such index; then the string
     * whose character at position <i>k</i> has the smaller value, as
     * determined by using the &lt; operator, lexicographically precedes the
     * other string. In this case, {@code compareTo} returns the
     * difference of the two character values at position {@code k} in
     * the two string -- that is, the value:
     * <blockquote><pre>
     * this.charAt(k)-anotherString.charAt(k)
     * </pre></blockquote>
     * If there is no index position at which they differ, then the shorter
     * string lexicographically precedes the longer string. In this case,
     * {@code compareTo} returns the difference of the lengths of the
     * strings -- that is, the value:
     * <blockquote><pre>
     * this.length()-anotherString.length()
     * </pre></blockquote>
     *
     * @param   anotherString   the {@code String} to be compared.
     * @return  the value {@code 0} if the argument string is equal to
     *          this string; a value less than {@code 0} if this string
     *          is lexicographically less than the string argument; and a
     *          value greater than {@code 0} if this string is
     *          lexicographically greater than the string argument.
     */
    static int compareTo(String _this, String anotherString) {
        int len = Math.min(_this.length(), anotherString.length());
        for (int i = 0; i < len; i++)
        {
            int diff = _this.charAt(i) - anotherString.charAt(i);
            if (diff != 0)
            {
                return diff;
            }
        }
        return _this.length() - anotherString.length();
    }

    /**
     * Compares two strings lexicographically, ignoring case
     * differences. This method returns an integer whose sign is that of
     * calling {@code compareTo} with normalized versions of the strings
     * where case differences have been eliminated by calling
     * {@code Character.toLowerCase(Character.toUpperCase(character))} on
     * each character.
     * <p>
     * Note that this method does <em>not</em> take locale into account,
     * and will result in an unsatisfactory ordering for certain locales.
     * The java.text package provides <em>collators</em> to allow
     * locale-sensitive ordering.
     *
     * @param   str   the {@code String} to be compared.
     * @return  a negative integer, zero, or a positive integer as the
     *          specified String is greater than, equal to, or less
     *          than this String, ignoring case considerations.
     * @see     java.text.Collator#compare(String, String)
     * @since   1.2
     */
    static int compareToIgnoreCase(String _this, String str) {
        return String.CASE_INSENSITIVE_ORDER.compare(_this, str);
    }

    /**
     * Tests if two string regions are equal.
     * <p>
     * A substring of this {@code String} object is compared to a substring
     * of the argument other. The result is true if these substrings
     * represent identical character sequences. The substring of this
     * {@code String} object to be compared begins at index {@code toffset}
     * and has length {@code len}. The substring of other to be compared
     * begins at index {@code ooffset} and has length {@code len}. The
     * result is {@code false} if and only if at least one of the following
     * is true:
     * <ul><li>{@code toffset} is negative.
     * <li>{@code ooffset} is negative.
     * <li>{@code toffset+len} is greater than the length of this
     * {@code String} object.
     * <li>{@code ooffset+len} is greater than the length of the other
     * argument.
     * <li>There is some nonnegative integer <i>k</i> less than {@code len}
     * such that:
     * {@code this.charAt(toffset + }<i>k</i>{@code ) != other.charAt(ooffset + }
     * <i>k</i>{@code )}
     * </ul>
     *
     * @param   toffset   the starting offset of the subregion in this string.
     * @param   other     the string argument.
     * @param   ooffset   the starting offset of the subregion in the string
     *                    argument.
     * @param   len       the number of characters to compare.
     * @return  {@code true} if the specified subregion of this string
     *          exactly matches the specified subregion of the string argument;
     *          {@code false} otherwise.
     */
    static boolean regionMatches(String _this, int toffset, String other, int ooffset,
            int len) {
        int to = toffset;
        int po = ooffset;
        // Note: toffset, ooffset, or len might be near -1>>>1.
        if ((ooffset < 0) || (toffset < 0)
            || (toffset > (long)_this.length() - len)
            || (ooffset > (long)other.length() - len)) {
            return false;
        }
        while (len-- > 0) {
            if (_this.charAt(to++) != other.charAt(po++)) {
                return false;
            }
        }
        return true;
    }

    /**
     * Tests if two string regions are equal.
     * <p>
     * A substring of this {@code String} object is compared to a substring
     * of the argument {@code other}. The result is {@code true} if these
     * substrings represent character sequences that are the same, ignoring
     * case if and only if {@code ignoreCase} is true. The substring of
     * this {@code String} object to be compared begins at index
     * {@code toffset} and has length {@code len}. The substring of
     * {@code other} to be compared begins at index {@code ooffset} and
     * has length {@code len}. The result is {@code false} if and only if
     * at least one of the following is true:
     * <ul><li>{@code toffset} is negative.
     * <li>{@code ooffset} is negative.
     * <li>{@code toffset+len} is greater than the length of this
     * {@code String} object.
     * <li>{@code ooffset+len} is greater than the length of the other
     * argument.
     * <li>{@code ignoreCase} is {@code false} and there is some nonnegative
     * integer <i>k</i> less than {@code len} such that:
     * <blockquote><pre>
     * this.charAt(toffset+k) != other.charAt(ooffset+k)
     * </pre></blockquote>
     * <li>{@code ignoreCase} is {@code true} and there is some nonnegative
     * integer <i>k</i> less than {@code len} such that:
     * <blockquote><pre>
     * Character.toLowerCase(this.charAt(toffset+k)) !=
     Character.toLowerCase(other.charAt(ooffset+k))
     * </pre></blockquote>
     * and:
     * <blockquote><pre>
     * Character.toUpperCase(this.charAt(toffset+k)) !=
     *         Character.toUpperCase(other.charAt(ooffset+k))
     * </pre></blockquote>
     * </ul>
     *
     * @param   ignoreCase   if {@code true}, ignore case when comparing
     *                       characters.
     * @param   toffset      the starting offset of the subregion in this
     *                       string.
     * @param   other        the string argument.
     * @param   ooffset      the starting offset of the subregion in the string
     *                       argument.
     * @param   len          the number of characters to compare.
     * @return  {@code true} if the specified subregion of this string
     *          matches the specified subregion of the string argument;
     *          {@code false} otherwise. Whether the matching is exact
     *          or case insensitive depends on the {@code ignoreCase}
     *          argument.
     */
    static boolean regionMatches(String _this, boolean ignoreCase, int toffset,
            String other, int ooffset, int len) {
        int to = toffset;
        int po = ooffset;
        // Note: toffset, ooffset, or len might be near -1>>>1.
        if ((ooffset < 0) || (toffset < 0)
               || (toffset > (long)_this.length() - len)
               || (ooffset > (long)other.length() - len)) {
            return false;
        }
        while (len-- > 0) {
            char c1 = _this.charAt(to++);
            char c2 = other.charAt(po++);
            if (c1 == c2) {
                continue;
            }
            if (ignoreCase) {
                // If characters don't match but case may be ignored,
                // try converting both characters to uppercase.
                // If the results match, then the comparison scan should
                // continue.
                char u1 = Character.toUpperCase(c1);
                char u2 = Character.toUpperCase(c2);
                if (u1 == u2) {
                    continue;
                }
                // Unfortunately, conversion to uppercase does not work properly
                // for the Georgian alphabet, which has strange rules about case
                // conversion.  So we need to make one last check before
                // exiting.
                if (Character.toLowerCase(u1) == Character.toLowerCase(u2)) {
                    continue;
                }
            }
            return false;
        }
        return true;
    }

    /**
     * Returns a hash code for this string. The hash code for a
     * {@code String} object is computed as
     * <blockquote><pre>
     * s[0]*31^(n-1) + s[1]*31^(n-2) + ... + s[n-1]
     * </pre></blockquote>
     * using {@code int} arithmetic, where {@code s[i]} is the
     * <i>i</i>th character of the string, {@code n} is the length of
     * the string, and {@code ^} indicates exponentiation.
     * (The hash value of the empty string is zero.)
     *
     * @return  a hash code value for this object.
     */
    static int hashCode(cli.System.String _this) {
        int h = 0;
        // NOTE having the get_Length in the for condition is actually faster than hoisting it,
        // the CLR JIT recognizes this pattern and optimizes the array bounds check in get_Chars.
        for (int i = 0; i < _this.get_Length(); i++)
        {
            h = h * 31 + _this.get_Chars(i);
        }
        return h;
    }

    /**
     * Returns the index within this string of the first occurrence of
     * the specified character. If a character with value
     * {@code ch} occurs in the character sequence represented by
     * this {@code String} object, then the index (in Unicode
     * code units) of the first such occurrence is returned. For
     * values of {@code ch} in the range from 0 to 0xFFFF
     * (inclusive), this is the smallest value <i>k</i> such that:
     * <blockquote><pre>
     * this.charAt(<i>k</i>) == ch
     * </pre></blockquote>
     * is true. For other values of {@code ch}, it is the
     * smallest value <i>k</i> such that:
     * <blockquote><pre>
     * this.codePointAt(<i>k</i>) == ch
     * </pre></blockquote>
     * is true. In either case, if no such character occurs in this
     * string, then {@code -1} is returned.
     *
     * @param   ch   a character (Unicode code point).
     * @return  the index of the first occurrence of the character in the
     *          character sequence represented by this object, or
     *          {@code -1} if the character does not occur.
     */
    static int indexOf(cli.System.String _this, int ch) {
        return indexOf(_this, ch, 0);
    }

    /**
     * Returns the index within this string of the first occurrence of the
     * specified character, starting the search at the specified index.
     * <p>
     * If a character with value {@code ch} occurs in the
     * character sequence represented by this {@code String}
     * object at an index no smaller than {@code fromIndex}, then
     * the index of the first such occurrence is returned. For values
     * of {@code ch} in the range from 0 to 0xFFFF (inclusive),
     * this is the smallest value <i>k</i> such that:
     * <blockquote><pre>
     * (this.charAt(<i>k</i>) == ch) {@code &&} (<i>k</i> &gt;= fromIndex)
     * </pre></blockquote>
     * is true. For other values of {@code ch}, it is the
     * smallest value <i>k</i> such that:
     * <blockquote><pre>
     * (this.codePointAt(<i>k</i>) == ch) {@code &&} (<i>k</i> &gt;= fromIndex)
     * </pre></blockquote>
     * is true. In either case, if no such character occurs in this
     * string at or after position {@code fromIndex}, then
     * {@code -1} is returned.
     *
     * <p>
     * There is no restriction on the value of {@code fromIndex}. If it
     * is negative, it has the same effect as if it were zero: this entire
     * string may be searched. If it is greater than the length of this
     * string, it has the same effect as if it were equal to the length of
     * this string: {@code -1} is returned.
     *
     * <p>All indices are specified in {@code char} values
     * (Unicode code units).
     *
     * @param   ch          a character (Unicode code point).
     * @param   fromIndex   the index to start the search from.
     * @return  the index of the first occurrence of the character in the
     *          character sequence represented by this object that is greater
     *          than or equal to {@code fromIndex}, or {@code -1}
     *          if the character does not occur.
     */
    static int indexOf(cli.System.String _this, int ch, int fromIndex) {
        int max = _this.get_Length();

        if (fromIndex < 0) {
            fromIndex = 0;
        } else if (fromIndex >= max) {
            // Note: fromIndex might be near -1>>>1.
            return -1;
        }

        int i = fromIndex;
        if (ch < Character.MIN_SUPPLEMENTARY_CODE_POINT) {
            // handle most cases here (ch is a BMP code point or a
            // negative value (invalid code point))
            for (; i < max ; i++) {
                if (_this.get_Chars(i) == ch) {
                    return i;
                }
            }
            return -1;
        } else {
            return indexOfSupplementary(_this, ch, fromIndex);
        }
    }

    /**
     * Handles (rare) calls of indexOf with a supplementary character.
     */
    private static int indexOfSupplementary(cli.System.String _this, int ch, int fromIndex) {
        if (Character.isValidCodePoint(ch)) {
            final char hi = Character.highSurrogate(ch);
            final char lo = Character.lowSurrogate(ch);
            final int max = _this.get_Length() - 1;
            for (int i = fromIndex; i < max; i++) {
                if (_this.get_Chars(i) == hi && _this.get_Chars(i+1) == lo) {
                    return i;
                }
            }
        }
        return -1;
    }

    /**
     * Returns the index within this string of the last occurrence of
     * the specified character. For values of {@code ch} in the
     * range from 0 to 0xFFFF (inclusive), the index (in Unicode code
     * units) returned is the largest value <i>k</i> such that:
     * <blockquote><pre>
     * this.charAt(<i>k</i>) == ch
     * </pre></blockquote>
     * is true. For other values of {@code ch}, it is the
     * largest value <i>k</i> such that:
     * <blockquote><pre>
     * this.codePointAt(<i>k</i>) == ch
     * </pre></blockquote>
     * is true.  In either case, if no such character occurs in this
     * string, then {@code -1} is returned.  The
     * {@code String} is searched backwards starting at the last
     * character.
     *
     * @param   ch   a character (Unicode code point).
     * @return  the index of the last occurrence of the character in the
     *          character sequence represented by this object, or
     *          {@code -1} if the character does not occur.
     */
    static int lastIndexOf(cli.System.String _this, int ch) {
        return lastIndexOf(_this, ch, _this.get_Length() - 1);
    }

    /**
     * Returns the index within this string of the last occurrence of
     * the specified character, searching backward starting at the
     * specified index. For values of {@code ch} in the range
     * from 0 to 0xFFFF (inclusive), the index returned is the largest
     * value <i>k</i> such that:
     * <blockquote><pre>
     * (this.charAt(<i>k</i>) == ch) {@code &&} (<i>k</i> &lt;= fromIndex)
     * </pre></blockquote>
     * is true. For other values of {@code ch}, it is the
     * largest value <i>k</i> such that:
     * <blockquote><pre>
     * (this.codePointAt(<i>k</i>) == ch) {@code &&} (<i>k</i> &lt;= fromIndex)
     * </pre></blockquote>
     * is true. In either case, if no such character occurs in this
     * string at or before position {@code fromIndex}, then
     * {@code -1} is returned.
     *
     * <p>All indices are specified in {@code char} values
     * (Unicode code units).
     *
     * @param   ch          a character (Unicode code point).
     * @param   fromIndex   the index to start the search from. There is no
     *          restriction on the value of {@code fromIndex}. If it is
     *          greater than or equal to the length of this string, it has
     *          the same effect as if it were equal to one less than the
     *          length of this string: this entire string may be searched.
     *          If it is negative, it has the same effect as if it were -1:
     *          -1 is returned.
     * @return  the index of the last occurrence of the character in the
     *          character sequence represented by this object that is less
     *          than or equal to {@code fromIndex}, or {@code -1}
     *          if the character does not occur before that point.
     */
    static int lastIndexOf(cli.System.String _this, int ch, int fromIndex) {
        if (ch < Character.MIN_SUPPLEMENTARY_CODE_POINT) {
            // handle most cases here (ch is a BMP code point or a
            // negative value (invalid code point))
            int i = Math.min(fromIndex, _this.get_Length() - 1);
            for (; i >= 0; i--) {
                if (_this.get_Chars(i) == ch) {
                    return i;
                }
            }
            return -1;
        } else {
            return lastIndexOfSupplementary(_this, ch, fromIndex);
        }
    }

    /**
     * Handles (rare) calls of lastIndexOf with a supplementary character.
     */
    private static int lastIndexOfSupplementary(cli.System.String _this, int ch, int fromIndex) {
        if (Character.isValidCodePoint(ch)) {
            char hi = Character.highSurrogate(ch);
            char lo = Character.lowSurrogate(ch);
            int i = Math.min(fromIndex, _this.get_Length() - 2);
            for (; i >= 0; i--) {
                if (_this.get_Chars(i) == hi && _this.get_Chars(i+1) == lo) {
                    return i;
                }
            }
        }
        return -1;
    }

    /**
     * Returns the index within this string of the first occurrence of the
     * specified substring.
     *
     * <p>The returned index is the smallest value <i>k</i> for which:
     * <blockquote><pre>
     * this.startsWith(str, <i>k</i>)
     * </pre></blockquote>
     * If no such value of <i>k</i> exists, then {@code -1} is returned.
     *
     * @param   str   the substring to search for.
     * @return  the index of the first occurrence of the specified substring,
     *          or {@code -1} if there is no such occurrence.
     */
    static int indexOf(String _this, String str) {
        return indexOf(_this, str, 0);
    }

    /**
     * Returns the index within this string of the first occurrence of the
     * specified substring, starting at the specified index.
     *
     * <p>The returned index is the smallest value <i>k</i> for which:
     * <blockquote><pre>
     * <i>k</i> &gt;= fromIndex {@code &&} this.startsWith(str, <i>k</i>)
     * </pre></blockquote>
     * If no such value of <i>k</i> exists, then {@code -1} is returned.
     *
     * @param   str         the substring to search for.
     * @param   fromIndex   the index from which to start the search.
     * @return  the index of the first occurrence of the specified substring,
     *          starting at the specified index,
     *          or {@code -1} if there is no such occurrence.
     */
    static int indexOf(String _this, String str, int fromIndex) {
        // start by dereferencing _this, to make sure we throw a NullPointerException if _this is null
        int slen = _this.length();
        int olen = str.length();
        if (olen == 0)
        {
            return Math.max(0, Math.min(fromIndex, slen));
        }
        if (olen > slen)
        {
            return -1;
        }
        char firstChar = str.charAt(0);
        // Java allows fromIndex to both below zero or above the length of the string, .NET doesn't
        int index = Math.max(0, Math.min(slen, fromIndex));
        int end = slen - olen;
        while (index >= 0 && index <= end)
        {
            if (cli.System.String.CompareOrdinal(_this, index, str, 0, olen) == 0)
            {
                return index;
            }
            index = _this.indexOf(firstChar, index + 1);
        }
        return -1;
    }

    /**
     * Code shared by String and AbstractStringBuilder to do searches. The
     * source is the character array being searched, and the target
     * is the string being searched for.
     *
     * @param   source       the characters being searched.
     * @param   sourceOffset offset of the source string.
     * @param   sourceCount  count of the source string.
     * @param   target       the characters being searched for.
     * @param   fromIndex    the index to begin searching from.
     */
    static int indexOf(char[] source, int sourceOffset, int sourceCount,
            String target, int fromIndex) {
        final int targetOffset = 0;
        final int targetCount = target.length();
        if (fromIndex >= sourceCount) {
            return (targetCount == 0 ? sourceCount : -1);
        }
        if (fromIndex < 0) {
            fromIndex = 0;
        }
        if (targetCount == 0) {
            return fromIndex;
        }

        char first = target.charAt(targetOffset);
        int max = sourceOffset + (sourceCount - targetCount);

        for (int i = sourceOffset + fromIndex; i <= max; i++) {
            /* Look for first character. */
            if (source[i] != first) {
                while (++i <= max && source[i] != first);
            }

            /* Found first character, now look at the rest of v2 */
            if (i <= max) {
                int j = i + 1;
                int end = j + targetCount - 1;
                for (int k = targetOffset + 1; j < end && source[j]
                        == target.charAt(k); j++, k++);

                if (j == end) {
                    /* Found whole string. */
                    return i - sourceOffset;
                }
            }
        }
        return -1;
    }

    /**
     * Code shared by String and StringBuffer to do searches. The
     * source is the character array being searched, and the target
     * is the string being searched for.
     *
     * @param   source       the characters being searched.
     * @param   sourceOffset offset of the source string.
     * @param   sourceCount  count of the source string.
     * @param   target       the characters being searched for.
     * @param   targetOffset offset of the target string.
     * @param   targetCount  count of the target string.
     * @param   fromIndex    the index to begin searching from.
     */
    static int indexOf(char[] source, int sourceOffset, int sourceCount,
            char[] target, int targetOffset, int targetCount,
            int fromIndex) {
        if (fromIndex >= sourceCount) {
            return (targetCount == 0 ? sourceCount : -1);
        }
        if (fromIndex < 0) {
            fromIndex = 0;
        }
        if (targetCount == 0) {
            return fromIndex;
        }

        char first = target[targetOffset];
        int max = sourceOffset + (sourceCount - targetCount);

        for (int i = sourceOffset + fromIndex; i <= max; i++) {
            /* Look for first character. */
            if (source[i] != first) {
                while (++i <= max && source[i] != first);
            }

            /* Found first character, now look at the rest of v2 */
            if (i <= max) {
                int j = i + 1;
                int end = j + targetCount - 1;
                for (int k = targetOffset + 1; j < end && source[j]
                        == target[k]; j++, k++);

                if (j == end) {
                    /* Found whole string. */
                    return i - sourceOffset;
                }
            }
        }
        return -1;
    }

    /**
     * Returns the index within this string of the last occurrence of the
     * specified substring.  The last occurrence of the empty string ""
     * is considered to occur at the index value {@code this.length()}.
     *
     * <p>The returned index is the largest value <i>k</i> for which:
     * <blockquote><pre>
     * this.startsWith(str, <i>k</i>)
     * </pre></blockquote>
     * If no such value of <i>k</i> exists, then {@code -1} is returned.
     *
     * @param   str   the substring to search for.
     * @return  the index of the last occurrence of the specified substring,
     *          or {@code -1} if there is no such occurrence.
     */
    static int lastIndexOf(String _this, String str) {
        return lastIndexOf(_this, str, _this.length());
    }

    /**
     * Returns the index within this string of the last occurrence of the
     * specified substring, searching backward starting at the specified index.
     *
     * <p>The returned index is the largest value <i>k</i> for which:
     * <blockquote><pre>
     * <i>k</i> {@code <=} fromIndex {@code &&} this.startsWith(str, <i>k</i>)
     * </pre></blockquote>
     * If no such value of <i>k</i> exists, then {@code -1} is returned.
     *
     * @param   str         the substring to search for.
     * @param   fromIndex   the index to start the search from.
     * @return  the index of the last occurrence of the specified substring,
     *          searching backward from the specified index,
     *          or {@code -1} if there is no such occurrence.
     */
    static int lastIndexOf(String _this, String str, int fromIndex) {
        // start by dereferencing s, to make sure we throw a NullPointerException if s is null
        int slen = _this.length();
        if (fromIndex < 0)
        {
            return -1;
        }
        int olen = str.length();
        if (olen == 0)
        {
            return Math.min(slen, fromIndex);
        }
        if (olen > slen)
        {
            return -1;
        }
        cli.System.String cliStr = (cli.System.String)(Object)_this;
        char firstChar = str.charAt(0);
        // Java allows fromIndex to both below zero or above the length of the string, .NET doesn't
        int index = Math.max(0, Math.min(slen - olen, fromIndex));
        while (index > 0)
        {
            if (cli.System.String.CompareOrdinal(_this, index, str, 0, olen) == 0)
            {
                return index;
            }
            index = cliStr.LastIndexOf(firstChar, index - 1);
        }
        return cli.System.String.CompareOrdinal(_this, 0, str, 0, olen) == 0 ? 0 : -1;
    }


    /**
     * Code shared by String and AbstractStringBuilder to do searches. The
     * source is the character array being searched, and the target
     * is the string being searched for.
     *
     * @param   source       the characters being searched.
     * @param   sourceOffset offset of the source string.
     * @param   sourceCount  count of the source string.
     * @param   target       the characters being searched for.
     * @param   fromIndex    the index to begin searching from.
     */
    static int lastIndexOf(char[] source, int sourceOffset, int sourceCount,
            String target, int fromIndex) {
        final int targetOffset = 0;
        final int targetCount = target.length();
        /*
         * Check arguments; return immediately where possible. For
         * consistency, don't check for null str.
         */
        int rightIndex = sourceCount - targetCount;
        if (fromIndex < 0) {
            return -1;
        }
        if (fromIndex > rightIndex) {
            fromIndex = rightIndex;
        }
        /* Empty string always matches. */
        if (targetCount == 0) {
            return fromIndex;
        }

        int strLastIndex = targetOffset + targetCount - 1;
        char strLastChar = target.charAt(strLastIndex);
        int min = sourceOffset + targetCount - 1;
        int i = min + fromIndex;

    startSearchForLastChar:
        while (true) {
            while (i >= min && source[i] != strLastChar) {
                i--;
            }
            if (i < min) {
                return -1;
            }
            int j = i - 1;
            int start = j - (targetCount - 1);
            int k = strLastIndex - 1;

            while (j > start) {
                if (source[j--] != target.charAt(k--)) {
                    i--;
                    continue startSearchForLastChar;
                }
            }
            return start - sourceOffset + 1;
        }
    }

    /**
     * Code shared by String and StringBuffer to do searches. The
     * source is the character array being searched, and the target
     * is the string being searched for.
     *
     * @param   source       the characters being searched.
     * @param   sourceOffset offset of the source string.
     * @param   sourceCount  count of the source string.
     * @param   target       the characters being searched for.
     * @param   targetOffset offset of the target string.
     * @param   targetCount  count of the target string.
     * @param   fromIndex    the index to begin searching from.
     */
    static int lastIndexOf(char[] source, int sourceOffset, int sourceCount,
            char[] target, int targetOffset, int targetCount,
            int fromIndex) {
        /*
         * Check arguments; return immediately where possible. For
         * consistency, don't check for null str.
         */
        int rightIndex = sourceCount - targetCount;
        if (fromIndex < 0) {
            return -1;
        }
        if (fromIndex > rightIndex) {
            fromIndex = rightIndex;
        }
        /* Empty string always matches. */
        if (targetCount == 0) {
            return fromIndex;
        }

        int strLastIndex = targetOffset + targetCount - 1;
        char strLastChar = target[strLastIndex];
        int min = sourceOffset + targetCount - 1;
        int i = min + fromIndex;

    startSearchForLastChar:
        while (true) {
            while (i >= min && source[i] != strLastChar) {
                i--;
            }
            if (i < min) {
                return -1;
            }
            int j = i - 1;
            int start = j - (targetCount - 1);
            int k = strLastIndex - 1;

            while (j > start) {
                if (source[j--] != target[k--]) {
                    i--;
                    continue startSearchForLastChar;
                }
            }
            return start - sourceOffset + 1;
        }
    }

    /**
     * Returns a string that is a substring of this string. The
     * substring begins at the specified {@code beginIndex} and
     * extends to the character at index {@code endIndex - 1}.
     * Thus the length of the substring is {@code endIndex-beginIndex}.
     * <p>
     * Examples:
     * <blockquote><pre>
     * "hamburger".substring(4, 8) returns "urge"
     * "smiles".substring(1, 5) returns "mile"
     * </pre></blockquote>
     *
     * @param      beginIndex   the beginning index, inclusive.
     * @param      endIndex     the ending index, exclusive.
     * @return     the specified substring.
     * @exception  IndexOutOfBoundsException  if the
     *             {@code beginIndex} is negative, or
     *             {@code endIndex} is larger than the length of
     *             this {@code String} object, or
     *             {@code beginIndex} is larger than
     *             {@code endIndex}.
     */
    static String substring(cli.System.String _this, int beginIndex, int endIndex) {
        if (beginIndex < 0) {
            throw new StringIndexOutOfBoundsException(beginIndex);
        }
        if (endIndex > _this.get_Length()) {
            throw new StringIndexOutOfBoundsException(endIndex);
        }
        int subLen = endIndex - beginIndex;
        if (subLen < 0) {
            throw new StringIndexOutOfBoundsException(subLen);
        }
        return ((beginIndex == 0) && (endIndex == _this.get_Length())) ? (String)(Object)_this
                : _this.Substring(beginIndex, subLen);
    }

    /**
     * Concatenates the specified string to the end of this string.
     * <p>
     * If the length of the argument string is {@code 0}, then this
     * {@code String} object is returned. Otherwise, a
     * {@code String} object is returned that represents a character
     * sequence that is the concatenation of the character sequence
     * represented by this {@code String} object and the character
     * sequence represented by the argument string.<p>
     * Examples:
     * <blockquote><pre>
     * "cares".concat("s") returns "caress"
     * "to".concat("get").concat("her") returns "together"
     * </pre></blockquote>
     *
     * @param   str   the {@code String} that is concatenated to the end
     *                of this {@code String}.
     * @return  a string that represents the concatenation of this object's
     *          characters followed by the string argument's characters.
     */
    static String concat(String _this, String str) {
        int otherLen = str.length();
        if (otherLen == 0) {
            return _this;
        }
        return cli.System.String.Concat(_this, str);
    }

    /**
     * Returns a string resulting from replacing all occurrences of
     * {@code oldChar} in this string with {@code newChar}.
     * <p>
     * If the character {@code oldChar} does not occur in the
     * character sequence represented by this {@code String} object,
     * then a reference to this {@code String} object is returned.
     * Otherwise, a {@code String} object is returned that
     * represents a character sequence identical to the character sequence
     * represented by this {@code String} object, except that every
     * occurrence of {@code oldChar} is replaced by an occurrence
     * of {@code newChar}.
     * <p>
     * Examples:
     * <blockquote><pre>
     * "mesquite in your cellar".replace('e', 'o')
     *         returns "mosquito in your collar"
     * "the war of baronets".replace('r', 'y')
     *         returns "the way of bayonets"
     * "sparring with a purple porpoise".replace('p', 't')
     *         returns "starring with a turtle tortoise"
     * "JonL".replace('q', 'x') returns "JonL" (no change)
     * </pre></blockquote>
     *
     * @param   oldChar   the old character.
     * @param   newChar   the new character.
     * @return  a string derived from this string by replacing every
     *          occurrence of {@code oldChar} with {@code newChar}.
     */
    static String replace(String _this, char oldChar, char newChar) {
        if (oldChar != newChar) {
            int len = _this.length();
            int i = -1;

            while (++i < len) {
                if (_this.charAt(i) == oldChar) {
                    break;
                }
            }
            if (i < len) {
                char buf[] = new char[len];
                for (int j = 0 ; j < i ; j++) {
                    buf[j] = _this.charAt(j);
                }
                while (i < len) {
                    char c = _this.charAt(i);
                    buf[i] = (c == oldChar) ? newChar : c;
                    i++;
                }
                return new String(buf, true);
            }
        }
        return _this;
    }

    /**
     * Returns true if and only if this string contains the specified
     * sequence of char values.
     *
     * @param s the sequence to search for
     * @return true if this string contains {@code s}, false otherwise
     * @since 1.5
     */
    static boolean contains(String _this, CharSequence s) {
        return indexOf(_this, s.toString()) > -1;
    }

    /**
     * Replaces each substring of this string that matches the literal target
     * sequence with the specified literal replacement sequence. The
     * replacement proceeds from the beginning of the string to the end, for
     * example, replacing "aa" with "b" in the string "aaa" will result in
     * "ba" rather than "ab".
     *
     * @param  target The sequence of char values to be replaced
     * @param  replacement The replacement sequence of char values
     * @return  The resulting string
     * @since 1.5
     */
    static String replace(String _this, CharSequence target, CharSequence replacement) {
        return Pattern.compile(target.toString(), Pattern.LITERAL).matcher(
            _this).replaceAll(Matcher.quoteReplacement(replacement.toString()));
    }

    /**
     * Splits this string around matches of the given
     * <a href="../util/regex/Pattern.html#sum">regular expression</a>.
     *
     * <p> The array returned by this method contains each substring of this
     * string that is terminated by another substring that matches the given
     * expression or is terminated by the end of the string.  The substrings in
     * the array are in the order in which they occur in this string.  If the
     * expression does not match any part of the input then the resulting array
     * has just one element, namely this string.
     *
     * <p> When there is a positive-width match at the beginning of this
     * string then an empty leading substring is included at the beginning
     * of the resulting array. A zero-width match at the beginning however
     * never produces such empty leading substring.
     *
     * <p> The {@code limit} parameter controls the number of times the
     * pattern is applied and therefore affects the length of the resulting
     * array.  If the limit <i>n</i> is greater than zero then the pattern
     * will be applied at most <i>n</i>&nbsp;-&nbsp;1 times, the array's
     * length will be no greater than <i>n</i>, and the array's last entry
     * will contain all input beyond the last matched delimiter.  If <i>n</i>
     * is non-positive then the pattern will be applied as many times as
     * possible and the array can have any length.  If <i>n</i> is zero then
     * the pattern will be applied as many times as possible, the array can
     * have any length, and trailing empty strings will be discarded.
     *
     * <p> The string {@code "boo:and:foo"}, for example, yields the
     * following results with these parameters:
     *
     * <blockquote><table cellpadding=1 cellspacing=0 summary="Split example showing regex, limit, and result">
     * <tr>
     *     <th>Regex</th>
     *     <th>Limit</th>
     *     <th>Result</th>
     * </tr>
     * <tr><td align=center>:</td>
     *     <td align=center>2</td>
     *     <td>{@code { "boo", "and:foo" }}</td></tr>
     * <tr><td align=center>:</td>
     *     <td align=center>5</td>
     *     <td>{@code { "boo", "and", "foo" }}</td></tr>
     * <tr><td align=center>:</td>
     *     <td align=center>-2</td>
     *     <td>{@code { "boo", "and", "foo" }}</td></tr>
     * <tr><td align=center>o</td>
     *     <td align=center>5</td>
     *     <td>{@code { "b", "", ":and:f", "", "" }}</td></tr>
     * <tr><td align=center>o</td>
     *     <td align=center>-2</td>
     *     <td>{@code { "b", "", ":and:f", "", "" }}</td></tr>
     * <tr><td align=center>o</td>
     *     <td align=center>0</td>
     *     <td>{@code { "b", "", ":and:f" }}</td></tr>
     * </table></blockquote>
     *
     * <p> An invocation of this method of the form
     * <i>str.</i>{@code split(}<i>regex</i>{@code ,}&nbsp;<i>n</i>{@code )}
     * yields the same result as the expression
     *
     * <blockquote>
     * <code>
     * {@link java.util.regex.Pattern}.{@link
     * java.util.regex.Pattern#compile compile}(<i>regex</i>).{@link
     * java.util.regex.Pattern#split(java.lang.CharSequence,int) split}(<i>str</i>,&nbsp;<i>n</i>)
     * </code>
     * </blockquote>
     *
     *
     * @param  regex
     *         the delimiting regular expression
     *
     * @param  limit
     *         the result threshold, as described above
     *
     * @return  the array of strings computed by splitting this string
     *          around matches of the given regular expression
     *
     * @throws  PatternSyntaxException
     *          if the regular expression's syntax is invalid
     *
     * @see java.util.regex.Pattern
     *
     * @since 1.4
     * @spec JSR-51
     */
    static String[] split(String _this, String regex, int limit) {
        /* fastpath if the regex is a
         (1)one-char String and this character is not one of the
            RegEx's meta characters ".$|()[{^?*+\\", or
         (2)two-char String and the first char is the backslash and
            the second is not the ascii digit or ascii letter.
         */
        char ch = 0;
        if (((regex.length() == 1 &&
             ".$|()[{^?*+\\".indexOf(ch = regex.charAt(0)) == -1) ||
             (regex.length() == 2 &&
              regex.charAt(0) == '\\' &&
              (((ch = regex.charAt(1))-'0')|('9'-ch)) < 0 &&
              ((ch-'a')|('z'-ch)) < 0 &&
              ((ch-'A')|('Z'-ch)) < 0)) &&
            (ch < Character.MIN_HIGH_SURROGATE ||
             ch > Character.MAX_LOW_SURROGATE))
        {
            int off = 0;
            int next = 0;
            boolean limited = limit > 0;
            ArrayList<String> list = new ArrayList<>();
            while ((next = _this.indexOf(ch, off)) != -1) {
                if (!limited || list.size() < limit - 1) {
                    list.add(_this.substring(off, next));
                    off = next + 1;
                } else {    // last one
                    //assert (list.size() == limit - 1);
                    list.add(_this.substring(off, _this.length()));
                    off = _this.length();
                    break;
                }
            }
            // If no match was found, return this
            if (off == 0)
                return new String[]{_this};

            // Add remaining segment
            if (!limited || list.size() < limit)
                list.add(_this.substring(off, _this.length()));

            // Construct result
            int resultSize = list.size();
            if (limit == 0) {
                while (resultSize > 0 && list.get(resultSize - 1).length() == 0) {
                    resultSize--;
                }
            }
            String[] result = new String[resultSize];
            return list.subList(0, resultSize).toArray(result);
        }
        return Pattern.compile(regex).split(_this, limit);
    }

    /**
     * Returns a new String composed of copies of the
     * {@code CharSequence elements} joined together with a copy of
     * the specified {@code delimiter}.
     *
     * <blockquote>For example,
     * <pre>{@code
     *     String message = String.join("-", "Java", "is", "cool");
     *     // message returned is: "Java-is-cool"
     * }</pre></blockquote>
     *
     * Note that if an element is null, then {@code "null"} is added.
     *
     * @param  delimiter the delimiter that separates each element
     * @param  elements the elements to join together.
     *
     * @return a new {@code String} that is composed of the {@code elements}
     *         separated by the {@code delimiter}
     *
     * @throws NullPointerException If {@code delimiter} or {@code elements}
     *         is {@code null}
     *
     * @see java.util.StringJoiner
     * @since 1.8
     */
    public static String join(CharSequence delimiter, CharSequence... elements) {
        Objects.requireNonNull(delimiter);
        Objects.requireNonNull(elements);
        // Number of elements not likely worth Arrays.stream overhead.
        StringJoiner joiner = new StringJoiner(delimiter);
        for (CharSequence cs: elements) {
            joiner.add(cs);
        }
        return joiner.toString();
    }

    /**
     * Returns a new {@code String} composed of copies of the
     * {@code CharSequence elements} joined together with a copy of the
     * specified {@code delimiter}.
     *
     * <blockquote>For example,
     * <pre>{@code
     *     List<String> strings = new LinkedList<>();
     *     strings.add("Java");strings.add("is");
     *     strings.add("cool");
     *     String message = String.join(" ", strings);
     *     //message returned is: "Java is cool"
     *
     *     Set<String> strings = new LinkedHashSet<>();
     *     strings.add("Java"); strings.add("is");
     *     strings.add("very"); strings.add("cool");
     *     String message = String.join("-", strings);
     *     //message returned is: "Java-is-very-cool"
     * }</pre></blockquote>
     *
     * Note that if an individual element is {@code null}, then {@code "null"} is added.
     *
     * @param  delimiter a sequence of characters that is used to separate each
     *         of the {@code elements} in the resulting {@code String}
     * @param  elements an {@code Iterable} that will have its {@code elements}
     *         joined together.
     *
     * @return a new {@code String} that is composed from the {@code elements}
     *         argument
     *
     * @throws NullPointerException If {@code delimiter} or {@code elements}
     *         is {@code null}
     *
     * @see    #join(CharSequence,CharSequence...)
     * @see    java.util.StringJoiner
     * @since 1.8
     */
    public static String join(CharSequence delimiter,
            Iterable<? extends CharSequence> elements) {
        Objects.requireNonNull(delimiter);
        Objects.requireNonNull(elements);
        StringJoiner joiner = new StringJoiner(delimiter);
        for (CharSequence cs: elements) {
            joiner.add(cs);
        }
        return joiner.toString();
    }

    /**
     * Converts all of the characters in this {@code String} to lower
     * case using the rules of the given {@code Locale}.  Case mapping is based
     * on the Unicode Standard version specified by the {@link java.lang.Character Character}
     * class. Since case mappings are not always 1:1 char mappings, the resulting
     * {@code String} may be a different length than the original {@code String}.
     * <p>
     * Examples of lowercase  mappings are in the following table:
     * <table border="1" summary="Lowercase mapping examples showing language code of locale, upper case, lower case, and description">
     * <tr>
     *   <th>Language Code of Locale</th>
     *   <th>Upper Case</th>
     *   <th>Lower Case</th>
     *   <th>Description</th>
     * </tr>
     * <tr>
     *   <td>tr (Turkish)</td>
     *   <td>&#92;u0130</td>
     *   <td>&#92;u0069</td>
     *   <td>capital letter I with dot above -&gt; small letter i</td>
     * </tr>
     * <tr>
     *   <td>tr (Turkish)</td>
     *   <td>&#92;u0049</td>
     *   <td>&#92;u0131</td>
     *   <td>capital letter I -&gt; small letter dotless i </td>
     * </tr>
     * <tr>
     *   <td>(all)</td>
     *   <td>French Fries</td>
     *   <td>french fries</td>
     *   <td>lowercased all chars in String</td>
     * </tr>
     * <tr>
     *   <td>(all)</td>
     *   <td><img src="doc-files/capiota.gif" alt="capiota"><img src="doc-files/capchi.gif" alt="capchi">
     *       <img src="doc-files/captheta.gif" alt="captheta"><img src="doc-files/capupsil.gif" alt="capupsil">
     *       <img src="doc-files/capsigma.gif" alt="capsigma"></td>
     *   <td><img src="doc-files/iota.gif" alt="iota"><img src="doc-files/chi.gif" alt="chi">
     *       <img src="doc-files/theta.gif" alt="theta"><img src="doc-files/upsilon.gif" alt="upsilon">
     *       <img src="doc-files/sigma1.gif" alt="sigma"></td>
     *   <td>lowercased all chars in String</td>
     * </tr>
     * </table>
     *
     * @param locale use the case transformation rules for this locale
     * @return the {@code String}, converted to lowercase.
     * @see     java.lang.String#toLowerCase()
     * @see     java.lang.String#toUpperCase()
     * @see     java.lang.String#toUpperCase(Locale)
     * @since   1.1
     */
    static String toLowerCase(String _this, Locale locale) {
        if (locale == null) {
            throw new NullPointerException();
        }

        int firstUpper;
        final int len = _this.length();

        /* Now check if there are any characters that need to be changed. */
        scan: {
            for (firstUpper = 0 ; firstUpper < len; ) {
                char c = _this.charAt(firstUpper);
                if ((c >= Character.MIN_HIGH_SURROGATE)
                        && (c <= Character.MAX_HIGH_SURROGATE)) {
                    int supplChar = _this.codePointAt(firstUpper);
                    if (supplChar != Character.toLowerCase(supplChar)) {
                        break scan;
                    }
                    firstUpper += Character.charCount(supplChar);
                } else {
                    if (c != Character.toLowerCase(c)) {
                        break scan;
                    }
                    firstUpper++;
                }
            }
            return _this;
        }

        char[] result = new char[len];
        int resultOffset = 0;  /* result may grow, so i+resultOffset
                                * is the write location in result */

        /* Just copy the first few lowerCase characters. */
        _this.getChars(0, firstUpper, result, 0);

        String lang = locale.getLanguage();
        boolean localeDependent =
                (lang == "tr" || lang == "az" || lang == "lt");
        char[] lowerCharArray;
        int lowerChar;
        int srcChar;
        int srcCount;
        for (int i = firstUpper; i < len; i += srcCount) {
            srcChar = (int)_this.charAt(i);
            if ((char)srcChar >= Character.MIN_HIGH_SURROGATE
                    && (char)srcChar <= Character.MAX_HIGH_SURROGATE) {
                srcChar = _this.codePointAt(i);
                srcCount = Character.charCount(srcChar);
            } else {
                srcCount = 1;
            }
            if (localeDependent ||
                srcChar == '\u03A3' || // GREEK CAPITAL LETTER SIGMA
                srcChar == '\u0130') { // LATIN CAPITAL LETTER I WITH DOT ABOVE
                lowerChar = ConditionalSpecialCasing.toLowerCaseEx(_this, i, locale);
            } else {
                lowerChar = Character.toLowerCase(srcChar);
            }
            if ((lowerChar == Character.ERROR)
                    || (lowerChar >= Character.MIN_SUPPLEMENTARY_CODE_POINT)) {
                if (lowerChar == Character.ERROR) {
                    lowerCharArray =
                            ConditionalSpecialCasing.toLowerCaseCharArray(_this, i, locale);
                } else if (srcCount == 2) {
                    resultOffset += Character.toChars(lowerChar, result, i + resultOffset) - srcCount;
                    continue;
                } else {
                    lowerCharArray = Character.toChars(lowerChar);
                }

                /* Grow result if needed */
                int mapLen = lowerCharArray.length;
                if (mapLen > srcCount) {
                    char[] result2 = new char[result.length + mapLen - srcCount];
                    System.arraycopy(result, 0, result2, 0, i + resultOffset);
                    result = result2;
                }
                for (int x = 0; x < mapLen; ++x) {
                    result[i + resultOffset + x] = lowerCharArray[x];
                }
                resultOffset += (mapLen - srcCount);
            } else {
                result[i + resultOffset] = (char)lowerChar;
            }
        }
        return new String(result, 0, len + resultOffset);
    }

    /**
     * Converts all of the characters in this {@code String} to lower
     * case using the rules of the default locale. This is equivalent to calling
     * {@code toLowerCase(Locale.getDefault())}.
     * <p>
     * <b>Note:</b> This method is locale sensitive, and may produce unexpected
     * results if used for strings that are intended to be interpreted locale
     * independently.
     * Examples are programming language identifiers, protocol keys, and HTML
     * tags.
     * For instance, {@code "TITLE".toLowerCase()} in a Turkish locale
     * returns {@code "t\u005Cu0131tle"}, where '\u005Cu0131' is the
     * LATIN SMALL LETTER DOTLESS I character.
     * To obtain correct results for locale insensitive strings, use
     * {@code toLowerCase(Locale.ROOT)}.
     * <p>
     * @return  the {@code String}, converted to lowercase.
     * @see     java.lang.String#toLowerCase(Locale)
     */
    static String toLowerCase(String _this) {
        return toLowerCase(_this, Locale.getDefault());
    }

    /**
     * Converts all of the characters in this {@code String} to upper
     * case using the rules of the given {@code Locale}. Case mapping is based
     * on the Unicode Standard version specified by the {@link java.lang.Character Character}
     * class. Since case mappings are not always 1:1 char mappings, the resulting
     * {@code String} may be a different length than the original {@code String}.
     * <p>
     * Examples of locale-sensitive and 1:M case mappings are in the following table.
     *
     * <table border="1" summary="Examples of locale-sensitive and 1:M case mappings. Shows Language code of locale, lower case, upper case, and description.">
     * <tr>
     *   <th>Language Code of Locale</th>
     *   <th>Lower Case</th>
     *   <th>Upper Case</th>
     *   <th>Description</th>
     * </tr>
     * <tr>
     *   <td>tr (Turkish)</td>
     *   <td>&#92;u0069</td>
     *   <td>&#92;u0130</td>
     *   <td>small letter i -&gt; capital letter I with dot above</td>
     * </tr>
     * <tr>
     *   <td>tr (Turkish)</td>
     *   <td>&#92;u0131</td>
     *   <td>&#92;u0049</td>
     *   <td>small letter dotless i -&gt; capital letter I</td>
     * </tr>
     * <tr>
     *   <td>(all)</td>
     *   <td>&#92;u00df</td>
     *   <td>&#92;u0053 &#92;u0053</td>
     *   <td>small letter sharp s -&gt; two letters: SS</td>
     * </tr>
     * <tr>
     *   <td>(all)</td>
     *   <td>Fahrvergn&uuml;gen</td>
     *   <td>FAHRVERGN&Uuml;GEN</td>
     *   <td></td>
     * </tr>
     * </table>
     * @param locale use the case transformation rules for this locale
     * @return the {@code String}, converted to uppercase.
     * @see     java.lang.String#toUpperCase()
     * @see     java.lang.String#toLowerCase()
     * @see     java.lang.String#toLowerCase(Locale)
     * @since   1.1
     */
    static String toUpperCase(String _this, Locale locale) {
        if (locale == null) {
            throw new NullPointerException();
        }

        int firstLower;
        final int len = _this.length();

        /* Now check if there are any characters that need to be changed. */
        scan: {
            for (firstLower = 0 ; firstLower < len; ) {
                int c = (int)_this.charAt(firstLower);
                int srcCount;
                if ((c >= Character.MIN_HIGH_SURROGATE)
                        && (c <= Character.MAX_HIGH_SURROGATE)) {
                    c = _this.codePointAt(firstLower);
                    srcCount = Character.charCount(c);
                } else {
                    srcCount = 1;
                }
                int upperCaseChar = Character.toUpperCaseEx(c);
                if ((upperCaseChar == Character.ERROR)
                        || (c != upperCaseChar)) {
                    break scan;
                }
                firstLower += srcCount;
            }
            return _this;
        }

        /* result may grow, so i+resultOffset is the write location in result */
        int resultOffset = 0;
        char[] result = new char[len]; /* may grow */

        /* Just copy the first few upperCase characters. */
        _this.getChars(0, firstLower, result, 0);

        String lang = locale.getLanguage();
        boolean localeDependent =
                (lang == "tr" || lang == "az" || lang == "lt");
        char[] upperCharArray;
        int upperChar;
        int srcChar;
        int srcCount;
        for (int i = firstLower; i < len; i += srcCount) {
            srcChar = (int)_this.charAt(i);
            if ((char)srcChar >= Character.MIN_HIGH_SURROGATE &&
                (char)srcChar <= Character.MAX_HIGH_SURROGATE) {
                srcChar = _this.codePointAt(i);
                srcCount = Character.charCount(srcChar);
            } else {
                srcCount = 1;
            }
            if (localeDependent) {
                upperChar = ConditionalSpecialCasing.toUpperCaseEx(_this, i, locale);
            } else {
                upperChar = Character.toUpperCaseEx(srcChar);
            }
            if ((upperChar == Character.ERROR)
                    || (upperChar >= Character.MIN_SUPPLEMENTARY_CODE_POINT)) {
                if (upperChar == Character.ERROR) {
                    if (localeDependent) {
                        upperCharArray =
                                ConditionalSpecialCasing.toUpperCaseCharArray(_this, i, locale);
                    } else {
                        upperCharArray = Character.toUpperCaseCharArray(srcChar);
                    }
                } else if (srcCount == 2) {
                    resultOffset += Character.toChars(upperChar, result, i + resultOffset) - srcCount;
                    continue;
                } else {
                    upperCharArray = Character.toChars(upperChar);
                }

                /* Grow result if needed */
                int mapLen = upperCharArray.length;
                if (mapLen > srcCount) {
                    char[] result2 = new char[result.length + mapLen - srcCount];
                    System.arraycopy(result, 0, result2, 0, i + resultOffset);
                    result = result2;
                }
                for (int x = 0; x < mapLen; ++x) {
                    result[i + resultOffset + x] = upperCharArray[x];
                }
                resultOffset += (mapLen - srcCount);
            } else {
                result[i + resultOffset] = (char)upperChar;
            }
        }
        return new String(result, 0, len + resultOffset);
    }

    /**
     * Converts all of the characters in this {@code String} to upper
     * case using the rules of the default locale. This method is equivalent to
     * {@code toUpperCase(Locale.getDefault())}.
     * <p>
     * <b>Note:</b> This method is locale sensitive, and may produce unexpected
     * results if used for strings that are intended to be interpreted locale
     * independently.
     * Examples are programming language identifiers, protocol keys, and HTML
     * tags.
     * For instance, {@code "title".toUpperCase()} in a Turkish locale
     * returns {@code "T\u005Cu0130TLE"}, where '\u005Cu0130' is the
     * LATIN CAPITAL LETTER I WITH DOT ABOVE character.
     * To obtain correct results for locale insensitive strings, use
     * {@code toUpperCase(Locale.ROOT)}.
     * <p>
     * @return  the {@code String}, converted to uppercase.
     * @see     java.lang.String#toUpperCase(Locale)
     */
    static String toUpperCase(String _this) {
        return toUpperCase(_this, Locale.getDefault());
    }

    /**
     * Returns a string whose value is this string, with any leading and trailing
     * whitespace removed.
     * <p>
     * If this {@code String} object represents an empty character
     * sequence, or the first and last characters of character sequence
     * represented by this {@code String} object both have codes
     * greater than {@code '\u005Cu0020'} (the space character), then a
     * reference to this {@code String} object is returned.
     * <p>
     * Otherwise, if there is no character with a code greater than
     * {@code '\u005Cu0020'} in the string, then a
     * {@code String} object representing an empty string is
     * returned.
     * <p>
     * Otherwise, let <i>k</i> be the index of the first character in the
     * string whose code is greater than {@code '\u005Cu0020'}, and let
     * <i>m</i> be the index of the last character in the string whose code
     * is greater than {@code '\u005Cu0020'}. A {@code String}
     * object is returned, representing the substring of this string that
     * begins with the character at index <i>k</i> and ends with the
     * character at index <i>m</i>-that is, the result of
     * {@code this.substring(k, m + 1)}.
     * <p>
     * This method may be used to trim whitespace (as defined above) from
     * the beginning and end of a string.
     *
     * @return  A string whose value is this string, with any leading and trailing white
     *          space removed, or this string if it has no leading or
     *          trailing white space.
     */
    static String trim(String _this) {
        int len = _this.length();
        int st = 0;

        while ((st < len) && (_this.charAt(st) <= ' ')) {
            st++;
        }
        while ((st < len) && (_this.charAt(len - 1) <= ' ')) {
            len--;
        }
        return ((st > 0) || (len < _this.length())) ? _this.substring(st, len) : _this;
    }

    /**
     * Returns a formatted string using the specified format string and
     * arguments.
     *
     * <p> The locale always used is the one returned by {@link
     * java.util.Locale#getDefault() Locale.getDefault()}.
     *
     * @param  format
     *         A <a href="../util/Formatter.html#syntax">format string</a>
     *
     * @param  args
     *         Arguments referenced by the format specifiers in the format
     *         string.  If there are more arguments than format specifiers, the
     *         extra arguments are ignored.  The number of arguments is
     *         variable and may be zero.  The maximum number of arguments is
     *         limited by the maximum dimension of a Java array as defined by
     *         <cite>The Java&trade; Virtual Machine Specification</cite>.
     *         The behaviour on a
     *         {@code null} argument depends on the <a
     *         href="../util/Formatter.html#syntax">conversion</a>.
     *
     * @throws  java.util.IllegalFormatException
     *          If a format string contains an illegal syntax, a format
     *          specifier that is incompatible with the given arguments,
     *          insufficient arguments given the format string, or other
     *          illegal conditions.  For specification of all possible
     *          formatting errors, see the <a
     *          href="../util/Formatter.html#detail">Details</a> section of the
     *          formatter class specification.
     *
     * @return  A formatted string
     *
     * @see  java.util.Formatter
     * @since  1.5
     */
    public static String format(String format, Object... args) {
        return new Formatter().format(format, args).toString();
    }

    /**
     * Returns a formatted string using the specified locale, format string,
     * and arguments.
     *
     * @param  l
     *         The {@linkplain java.util.Locale locale} to apply during
     *         formatting.  If {@code l} is {@code null} then no localization
     *         is applied.
     *
     * @param  format
     *         A <a href="../util/Formatter.html#syntax">format string</a>
     *
     * @param  args
     *         Arguments referenced by the format specifiers in the format
     *         string.  If there are more arguments than format specifiers, the
     *         extra arguments are ignored.  The number of arguments is
     *         variable and may be zero.  The maximum number of arguments is
     *         limited by the maximum dimension of a Java array as defined by
     *         <cite>The Java&trade; Virtual Machine Specification</cite>.
     *         The behaviour on a
     *         {@code null} argument depends on the
     *         <a href="../util/Formatter.html#syntax">conversion</a>.
     *
     * @throws  java.util.IllegalFormatException
     *          If a format string contains an illegal syntax, a format
     *          specifier that is incompatible with the given arguments,
     *          insufficient arguments given the format string, or other
     *          illegal conditions.  For specification of all possible
     *          formatting errors, see the <a
     *          href="../util/Formatter.html#detail">Details</a> section of the
     *          formatter class specification
     *
     * @return  A formatted string
     *
     * @see  java.util.Formatter
     * @since  1.5
     */
    public static String format(Locale l, String format, Object... args) {
        return new Formatter(l).format(format, args).toString();
    }

    /**
     * Returns the string representation of the {@code Object} argument.
     *
     * @param   obj   an {@code Object}.
     * @return  if the argument is {@code null}, then a string equal to
     *          {@code "null"}; otherwise, the value of
     *          {@code obj.toString()} is returned.
     * @see     java.lang.Object#toString()
     */
    public static String valueOf(Object obj) {
        return (obj == null) ? "null" : obj.toString();
    }

    /**
     * Returns the string representation of the {@code char} array
     * argument. The contents of the character array are copied; subsequent
     * modification of the character array does not affect the returned
     * string.
     *
     * @param   data     the character array.
     * @return  a {@code String} that contains the characters of the
     *          character array.
     */
    public static String valueOf(char data[]) {
        return new String(data);
    }

    /**
     * Returns the string representation of a specific subarray of the
     * {@code char} array argument.
     * <p>
     * The {@code offset} argument is the index of the first
     * character of the subarray. The {@code count} argument
     * specifies the length of the subarray. The contents of the subarray
     * are copied; subsequent modification of the character array does not
     * affect the returned string.
     *
     * @param   data     the character array.
     * @param   offset   initial offset of the subarray.
     * @param   count    length of the subarray.
     * @return  a {@code String} that contains the characters of the
     *          specified subarray of the character array.
     * @exception IndexOutOfBoundsException if {@code offset} is
     *          negative, or {@code count} is negative, or
     *          {@code offset+count} is larger than
     *          {@code data.length}.
     */
    public static String valueOf(char data[], int offset, int count) {
        return new String(data, offset, count);
    }

    /**
     * Equivalent to {@link #valueOf(char[], int, int)}.
     *
     * @param   data     the character array.
     * @param   offset   initial offset of the subarray.
     * @param   count    length of the subarray.
     * @return  a {@code String} that contains the characters of the
     *          specified subarray of the character array.
     * @exception IndexOutOfBoundsException if {@code offset} is
     *          negative, or {@code count} is negative, or
     *          {@code offset+count} is larger than
     *          {@code data.length}.
     */
    public static String copyValueOf(char data[], int offset, int count) {
        return new String(data, offset, count);
    }

    /**
     * Equivalent to {@link #valueOf(char[])}.
     *
     * @param   data   the character array.
     * @return  a {@code String} that contains the characters of the
     *          character array.
     */
    public static String copyValueOf(char data[]) {
        return new String(data);
    }

    /**
     * Returns the string representation of the {@code boolean} argument.
     *
     * @param   b   a {@code boolean}.
     * @return  if the argument is {@code true}, a string equal to
     *          {@code "true"} is returned; otherwise, a string equal to
     *          {@code "false"} is returned.
     */
    public static String valueOf(boolean b) {
        return b ? "true" : "false";
    }

    /**
     * Returns the string representation of the {@code int} argument.
     * <p>
     * The representation is exactly the one returned by the
     * {@code Integer.toString} method of one argument.
     *
     * @param   i   an {@code int}.
     * @return  a string representation of the {@code int} argument.
     * @see     java.lang.Integer#toString(int, int)
     */
    public static String valueOf(int i) {
        return Integer.toString(i);
    }

    /**
     * Returns the string representation of the {@code long} argument.
     * <p>
     * The representation is exactly the one returned by the
     * {@code Long.toString} method of one argument.
     *
     * @param   l   a {@code long}.
     * @return  a string representation of the {@code long} argument.
     * @see     java.lang.Long#toString(long)
     */
    public static String valueOf(long l) {
        return Long.toString(l);
    }

    /**
     * Returns the string representation of the {@code float} argument.
     * <p>
     * The representation is exactly the one returned by the
     * {@code Float.toString} method of one argument.
     *
     * @param   f   a {@code float}.
     * @return  a string representation of the {@code float} argument.
     * @see     java.lang.Float#toString(float)
     */
    public static String valueOf(float f) {
        return Float.toString(f);
    }

    /**
     * Returns the string representation of the {@code double} argument.
     * <p>
     * The representation is exactly the one returned by the
     * {@code Double.toString} method of one argument.
     *
     * @param   d   a {@code double}.
     * @return  a  string representation of the {@code double} argument.
     * @see     java.lang.Double#toString(double)
     */
    public static String valueOf(double d) {
        return Double.toString(d);
    }
}
