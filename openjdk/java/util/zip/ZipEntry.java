/* ZipEntry.java --
   Copyright (C) 2001, 2002, 2004, 2005, 2011 Free Software Foundation, Inc.

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
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
02110-1301 USA.

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


package java.util.zip;

import java.util.Date;

/**
 * This class represents a member of a zip archive.  ZipFile and
 * ZipInputStream will give you instances of this class as information
 * about the members in an archive.  On the other hand ZipOutputStream
 * needs an instance of this class to create a new member.
 *
 * @author Jochen Hoenicke 
 */
public class ZipEntry implements ZipConstants, Cloneable
{
    String name;
    long time = -1;
    long crc = -1;
    long size = -1;
    long csize = -1;
    int method = -1;
    int flag;
    byte[] extra;
    String comment;

    long offset;             /* used by ZipFile */

    /**
     * Compression method.  This method doesn't compress at all.
     */
    public static final int STORED = 0;
    /**
     * Compression method.  This method uses the Deflater.
     */
    public static final int DEFLATED = 8;

    /**
     * Creates a zip entry with the given name.
     * @param name the name. May include directory components separated
     * by '/'.
     *
     * @exception NullPointerException when name is null.
     * @exception IllegalArgumentException when name is bigger then 65535 chars.
     */
    public ZipEntry(String name)
    {
        int length = name.length();
        if (length > 65535)
            throw new IllegalArgumentException("name length is " + length);
        this.name = name;
    }

    /**
     * Creates a copy of the given zip entry.
     * @param e the entry to copy.
     */
    public ZipEntry(ZipEntry e)
    {
        name = e.name;
        time = e.time;
        crc = e.crc;
        size = e.size;
        csize = e.csize;
        method = e.method;
        flag = e.flag;
        extra = e.extra;
        comment = e.comment;
    }

    ZipEntry()
    {
    }

    /**
     * Creates a copy of this zip entry.
     */
    /**
     * Clones the entry.
     */
    public Object clone()
    {
        try
        {
            // The JCL says that the `extra' field is also copied.
            ZipEntry clone = (ZipEntry)super.clone();
            if (extra != null)
                clone.extra = (byte[])extra.clone();
            return clone;
        }
        catch (CloneNotSupportedException ex)
        {
            throw new InternalError();
        }
    }

    /**
     * Returns the entry name.  The path components in the entry are
     * always separated by slashes ('/').  
     */
    public String getName()
    {
        return name;
    }

    /**
     * Sets the time of last modification of the entry.
     * @time the time of last modification of the entry.
     */
    public void setTime(long time)
    {
        Date d = new Date(time);
        if (d.getYear() < 80)
        {
            d = new Date(80, 0, 1);
        }
        this.time = ((d.getYear() - 80) << 25)
            | ((d.getMonth() + 1) << 21)
            | (d.getDate() << 16)
            | (d.getHours() << 11)
            | (d.getMinutes() << 5)
            | (d.getSeconds() >> 1);
    }

    /**
     * Gets the time of last modification of the entry.
     * @return the time of last modification of the entry, or -1 if unknown.
     */
    public long getTime()
    {
        if (time == -1)
        {
            return -1;
        }
        Date d = new Date((int)(((time >> 25) & 0x7f) + 80),
                  (int)(((time >> 21) & 0x0f) - 1),
                  (int)((time >> 16) & 0x1f),
                  (int)((time >> 11) & 0x1f),
                  (int)((time >> 5) & 0x3f),
                  (int)((time << 1) & 0x3e));
        return d.getTime();
    }

    /**
     * Sets the size of the uncompressed data.
     * @exception IllegalArgumentException if size is negative.
     */
    public void setSize(long size)
    {
        if (size < 0)
            throw new IllegalArgumentException();
        this.size = size;
    }

    /**
     * Gets the size of the uncompressed data.
     * @return the size or -1 if unknown.
     */
    public long getSize()
    {
        return size;
    }

    /**
     * Sets the size of the compressed data.
     */
    public void setCompressedSize(long csize)
    {
        this.csize = csize;
    }

    /**
     * Gets the size of the compressed data.
     * @return the size or -1 if unknown.
     */
    public long getCompressedSize()
    {
        return csize;
    }

    /**
     * Sets the crc of the uncompressed data.
     * @exception IllegalArgumentException if crc is not in 0..0xffffffffL
     */
    public void setCrc(long crc)
    {
        if ((crc & 0xffffffff00000000L) != 0)
            throw new IllegalArgumentException();
        this.crc = crc;
    }

    /**
     * Gets the crc of the uncompressed data.
     * @return the crc or -1 if unknown.
     */
    public long getCrc()
    {
        return crc;
    }

    /**
     * Sets the compression method.  Only DEFLATED and STORED are
     * supported.
     * @exception IllegalArgumentException if method is not supported.
     * @see ZipOutputStream#DEFLATED
     * @see ZipOutputStream#STORED 
     */
    public void setMethod(int method)
    {
        if (method != ZipOutputStream.STORED
            && method != ZipOutputStream.DEFLATED)
            throw new IllegalArgumentException();
        this.method = method;
    }

    /**
     * Gets the compression method.  
     * @return the compression method or -1 if unknown.
     */
    public int getMethod()
    {
        return method;
    }

    /**
     * Sets the extra data.
     * @exception IllegalArgumentException if extra is longer than 0xffff bytes.
     */
    public void setExtra(byte[] extra)
    {
        if (extra == null)
        {
            this.extra = null;
            return;
        }
        if (extra.length > 0xffff)
            throw new IllegalArgumentException();
        this.extra = extra;
    }

    /**
     * Gets the extra data.
     * @return the extra data or null if not set.
     */
    public byte[] getExtra()
    {
        return extra;
    }

    /**
     * Sets the entry comment.
     */
    public void setComment(String comment)
    {
        this.comment = comment;
    }

    /**
     * Gets the comment.
     * @return the comment or null if not set.
     */
    public String getComment()
    {
        return comment;
    }

    /**
     * Gets true, if the entry is a directory.  This is solely
     * determined by the name, a trailing slash '/' marks a directory.  
     */
    public boolean isDirectory()
    {
        int nlen = name.length();
        return nlen > 0 && name.charAt(nlen - 1) == '/';
    }

    /**
     * Gets the string representation of this ZipEntry.  This is just
     * the name as returned by getName().
     */
    public String toString()
    {
        return getName();
    }

    /**
     * Gets the hashCode of this ZipEntry.  This is just the hashCode
     * of the name.  Note that the equals method isn't changed, though.
     */
    public int hashCode()
    {
        return name.hashCode();
    }
}
