/* ZipFile.java --
   Copyright (C) 2001, 2014
   Free Software Foundation, Inc.

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

import java.io.Closeable;
import java.io.EOFException;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.io.RandomAccessFile;
import java.io.UnsupportedEncodingException;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.Enumeration;
import java.util.Iterator;
import java.util.LinkedHashMap;
import java.util.stream.Stream;
import static java.util.zip.ZipConstants64.*;

/**
 * This class represents a Zip archive.  You can ask for the contained
 * entries, or get an input stream for a file entry.  The entry is
 * automatically decompressed.
 *
 * This class is thread safe:  You can open input streams for arbitrary
 * entries in different threads.
 *
 * @author Jochen Hoenicke
 * @author Artur Biesiadowski
 */
public class ZipFile implements ZipConstants, Closeable
{

  /**
   * Mode flag to open a zip file for reading.
   */
  public static final int OPEN_READ = 0x1;

  /**
   * Mode flag to delete a zip file after reading.
   */
  public static final int OPEN_DELETE = 0x4;

  /**
   * This field isn't defined in the JDK's ZipConstants, but should be.
   */
  static final int ENDNRD =  4;

  // Name of this zip file.
  private final String name;

  // Encoding to use for name and comment strings
  private final Charset charset;

  // File from which zip entries are read.
  private final RandomAccessFile raf;

  // The entries of this zip file when initialized and not yet closed.
  private LinkedHashMap<String, ZipEntry> entries;

  private boolean closed = false;
  final boolean hasLocHeader;

  /**
   * Opens a Zip file with the given name for reading.
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the file doesn't contain a valid zip
   * archive.  
   */
  public ZipFile(String name) throws ZipException, IOException
  {
    this(new File(name), OPEN_READ);
  }

  /**
   * Opens a Zip file reading the given File.
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the file doesn't contain a valid zip
   * archive.  
   */
  public ZipFile(File file) throws ZipException, IOException
  {
    this(file, OPEN_READ);
  }

  /**
   * Opens a Zip file reading the given File.
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the file doesn't contain a valid zip
   * archive.  
   */
  public ZipFile(File file, Charset charset) throws IOException
  {
    this(file, OPEN_READ, charset);
  }

  /**
   * Opens a Zip file reading the given File.
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the file doesn't contain a valid zip
   * archive.  
   */
  public ZipFile(String name, Charset charset) throws IOException
  {
    this(new File(name), OPEN_READ, charset);
  }

  /**
   * Opens a Zip file reading the given File in the given mode.
   *
   * If the OPEN_DELETE mode is specified, the zip file will be deleted at
   * some time moment after it is opened. It will be deleted before the zip
   * file is closed or the Virtual Machine exits.
   * 
   * The contents of the zip file will be accessible until it is closed.
   *
   * @since JDK1.3
   * @param mode Must be one of OPEN_READ or OPEN_READ | OPEN_DELETE
   *
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the file doesn't contain a valid zip
   * archive.  
   */
  public ZipFile(File file, int mode) throws IOException
  {
    this(file, mode, StandardCharsets.UTF_8);
  }

  /**
   * Opens a Zip file reading the given File in the given mode.
   *
   * If the OPEN_DELETE mode is specified, the zip file will be deleted at
   * some time moment after it is opened. It will be deleted before the zip
   * file is closed or the Virtual Machine exits.
   * 
   * The contents of the zip file will be accessible until it is closed.
   *
   * @since JDK1.7
   * @param mode Must be one of OPEN_READ or OPEN_READ | OPEN_DELETE
   * @param charset Character encoding to use for names and comments
   *
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the file doesn't contain a valid zip
   * archive.  
   */
  public ZipFile(File file, int mode, Charset charset) throws IOException
  {
    if (mode != OPEN_READ && mode != (OPEN_READ | OPEN_DELETE))
      throw new IllegalArgumentException("Illegal mode: 0x" + Integer.toHexString(mode));
    if (charset == null)
      throw new NullPointerException("charset is null");
    if ((mode & OPEN_DELETE) != 0)
      file.deleteOnExit();
    this.raf = new RandomAccessFile(file, "r");
    this.name = file.getPath();
    this.charset = charset;
    this.hasLocHeader = raf.length() >= 4 && raf.readInt() == (int)((LOCSIG << 24) | ((LOCSIG & 0xFF00) << 8) | ((LOCSIG & 0xFF0000) >> 8) | (LOCSIG >> 24));

    boolean valid = false;

    try 
      {
        readEntries();
        ClassStubZipEntry.expandIkvmClasses(this, entries);
        valid = true;
      }
    catch (EOFException _)
      {
        throw new ZipException("invalid CEN header (bad header size)");
      } 
    finally
      {
        if (!valid)
          {
            try
              {
                raf.close();
              }
            catch (IOException _)
              {
              }
          }
      }
  }

  /**
   * Checks if file is closed and throws an exception.
   */
  private void checkClosed()
  {
    if (closed)
      throw new IllegalStateException("zip file closed");
  }

  /**
   * Read the central directory of a zip file and fill the entries
   * array.  This is called exactly once when first needed. It is called
   * while holding the lock on <code>raf</code>.
   *
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the central directory is malformed 
   */
  private void readEntries() throws IOException
  {
    PartialInputStream inp = new PartialInputStream(4096);
    long pos = inp.seekEndOfCentralDirectory();
    inp.skip(6);
    int count = inp.readLeUnsignedShort();
    long centralSize = inp.readLeUnsignedInt();    
    long centralOffset = inp.readLeUnsignedInt();

    if (centralSize == ZIP64_MAGICVAL
        || centralOffset == ZIP64_MAGICVAL
        || count == ZIP64_MAGICCOUNT)
      {
        inp.seek(pos - ZIP64_LOCHDR);
        if (inp.readLeInt() == ZIP64_LOCSIG)
          {
            inp.skip(4);
            long zip64end = inp.readLeLong();
            inp.seek(zip64end);
            if (inp.readLeInt() == ZIP64_ENDSIG)
              {
                inp.skip(ZIP64_ENDSIZ - 4);
                centralSize = inp.readLeLong();
                centralOffset = inp.readLeLong();
                pos = zip64end;
              }
          }
      }

    if (centralSize > pos)
      throw new ZipException("invalid END header (bad central directory size)");

    if (centralOffset > pos - centralSize)
      throw new ZipException("invalid END header (bad central directory offset)");

    entries = new LinkedHashMap<String, ZipEntry> (count+count/2);
    inp.seek(pos - centralSize);
    
    while (inp.position() <= pos - CENHDR)
      {
        if (inp.readLeInt() != CENSIG)
          throw new ZipException("invalid CEN header (bad signature)");

        inp.skip(4);
        int flags = inp.readLeUnsignedShort();
        if ((flags & 1) != 0)
          throw new ZipException("invalid CEN header (encrypted entry)");
        int method = inp.readLeUnsignedShort();
        if (method != ZipEntry.STORED && method != ZipEntry.DEFLATED)
          throw new ZipException("invalid CEN header (bad compression method)");
        ZipEntry entry = new ZipEntry();
        entry.flag = flags;
        entry.method = method;
        entry.dostime = inp.readLeUnsignedInt();
        entry.crc = inp.readLeUnsignedInt();
        entry.csize = inp.readLeUnsignedInt();
        entry.size = inp.readLeUnsignedInt();
        int nameLen = inp.readLeUnsignedShort();
        int extraLen = inp.readLeUnsignedShort();
        int commentLen = inp.readLeUnsignedShort();
        inp.skip(8);
        entry.offset = inp.readLeUnsignedInt();
        entry.name = inp.readString(nameLen, (flags & EFS) != 0);

        if (extraLen > 0)
          {
            byte[] extra = new byte[extraLen];
            inp.readFully(extra);
            entry.setExtra0(extra, false);
            readZip64ExtraField(entry, extra);
          }
        if (commentLen > 0)
          {
            entry.comment = inp.readString(commentLen, (flags & EFS) != 0);
          }
        entries.put(entry.name, entry);
      }

    if (inp.position() != pos)
      throw new ZipException("invalid CEN header (bad header size)");
  }

  private static void readZip64ExtraField(ZipEntry entry, byte[] extra)
  {
    if (entry.csize == ZIP64_MAGICVAL || entry.size == ZIP64_MAGICVAL
        || entry.offset == ZIP64_MAGICVAL)
      {
        for (int pos = 0; pos < extra.length - 4; )
          {
            int headerID = decodeLeUnsignedShort(extra, pos);
            int dataSize = decodeLeUnsignedShort(extra, pos + 2);
            pos += 4;
            if (headerID == ZIP64_EXTID)
              {
                if (entry.size == ZIP64_MAGICVAL)
                  {
                    entry.size = decodeLeLong(extra, pos);
                    pos += 8;
                  }
                if (entry.csize == ZIP64_MAGICVAL)
                  {
                    entry.csize = decodeLeLong(extra, pos);
                    pos += 8;
                  }
                if (entry.offset == ZIP64_MAGICVAL)
                  {
                    entry.offset = decodeLeLong(extra, pos);
                    pos += 8;
                  }
                break;
              }
            pos += dataSize;
          }
      }
  }

  private static int decodeLeUnsignedShort(byte[] b, int off)
  {
    return (b[off] & 0xFF) | ((b[off + 1] & 0xFF) << 8);
  }

  private static long decodeLeLong(byte[] b, int off)
  {
    return 0
        | ((b[off + 0] & 0xFFL) << 0)
        | ((b[off + 1] & 0xFFL) << 8)
        | ((b[off + 2] & 0xFFL) << 16)
        | ((b[off + 3] & 0xFFL) << 24)
        | ((b[off + 4] & 0xFFL) << 32)
        | ((b[off + 5] & 0xFFL) << 40)
        | ((b[off + 6] & 0xFFL) << 48)
        | ((b[off + 7] & 0xFFL) << 56);
  }

  /**
   * Closes the ZipFile.  This also closes all input streams given by
   * this class.  After this is called, no further method should be
   * called.
   * 
   * @exception IOException if a i/o error occured.
   */
  public void close() throws IOException
  {
    RandomAccessFile raf = this.raf;
    if (raf == null)
      return;

    synchronized (raf)
      {
        closed = true;
        entries = null;
        raf.close();
      }
  }

  /**
   * Calls the <code>close()</code> method when this ZipFile has not yet
   * been explicitly closed.
   */
  protected void finalize() throws IOException
  {
    if (!closed && raf != null) close();
  }

  /**
   * Returns an enumeration of all Zip entries in this Zip file.
   *
   * @exception IllegalStateException when the ZipFile has already been closed
   */
  public Enumeration<? extends ZipEntry> entries()
  {
    checkClosed();
    return new ZipEntryEnumeration(entries.values().iterator());
  }

  public Stream<? extends ZipEntry> stream()
  {
    checkClosed();
    return entries.values().stream();
  }

  /**
   * Searches for a zip entry in this archive with the given name.
   *
   * @param name the name. May contain directory components separated by
   * slashes ('/').
   * @return the zip entry, or null if no entry with that name exists.
   *
   * @exception IllegalStateException when the ZipFile has already been closed
   */
  public ZipEntry getEntry(String name)
  {
    checkClosed();
    ZipEntry entry = entries.get(name);
    // If we didn't find it, maybe it's a directory.
    if (entry == null && !name.endsWith("/"))
      entry = entries.get(name + '/');
    return entry != null ? (ZipEntry)entry.clone() : null;
  }

  /**
   * Creates an input stream reading the given zip entry as
   * uncompressed data.  Normally zip entry should be an entry
   * returned by getEntry() or entries().
   *
   * This implementation returns null if the requested entry does not
   * exist.  This decision is not obviously correct, however, it does
   * appear to mirror Sun's implementation, and it is consistant with
   * their javadoc.  On the other hand, the old JCL book, 2nd Edition,
   * claims that this should return a "non-null ZIP entry".  We have
   * chosen for now ignore the old book, as modern versions of Ant (an
   * important application) depend on this behaviour.  See discussion
   * in this thread:
   * http://gcc.gnu.org/ml/java-patches/2004-q2/msg00602.html
   *
   * @param entry the entry to create an InputStream for.
   * @return the input stream, or null if the requested entry does not exist.
   *
   * @exception IllegalStateException when the ZipFile has already been closed
   * @exception IOException if a i/o error occured.
   * @exception ZipException if the Zip archive is malformed.  
   */
  public InputStream getInputStream(ZipEntry entry) throws IOException
  {
    checkClosed();

    final ZipEntry zipEntry = entries.get(entry.getName());
    if (zipEntry == null)
      return null;

    if (zipEntry instanceof ClassStubZipEntry)
      return ((ClassStubZipEntry)zipEntry).getInputStream();

    PartialInputStream inp = new PartialInputStream(1024) {
        void lazyInitialSeek() throws IOException {
            seek(zipEntry.offset);

            if (readLeInt() != LOCSIG)
              throw new ZipException("invalid LOC header (bad signature)");

            skip(22);

            int nameLen = readLeUnsignedShort();
            int extraLen = readLeUnsignedShort();
            skip(nameLen + extraLen);

            setLength(zipEntry.getCompressedSize());
        }
    };

    switch (zipEntry.getMethod())
      {
      case ZipOutputStream.STORED:
        return inp;
      case ZipOutputStream.DEFLATED:
        inp.addDummyByte();
        final Inflater inf = new Inflater(true);
        final int sz = (int) zipEntry.getSize();
        return new InflaterInputStream(inp, inf)
        {
          private boolean closed;
          public void close() throws IOException
          {
            closed = true;
            super.close();
          }
          public int available() throws IOException
          {
            if (closed)
              return 0;
            if (sz == -1)
              return super.available();
            if (super.available() != 0)
              return sz - inf.getTotalOut();
            return 0;
          }
        };
      default:
        throw new ZipException("invalid compression method");
      }
  }
  
  /**
   * Returns the (path) name of this zip file.
   */
  public String getName()
  {
    return name;
  }

  /**
   * Returns the number of entries in this zip file.
   *
   * @exception IllegalStateException when the ZipFile has already been closed
   */
  public int size()
  {
    checkClosed();
    return entries.size();
  }

  /**
   * Returns the zip file comment.
   *
   * @exception IllegalStateException when the ZipFile has already been closed
   */
  public synchronized String getComment()
  {
    checkClosed();
    try
      {
        PartialInputStream inp = new PartialInputStream(4096);
        long pos = inp.seekEndOfCentralDirectory();
        inp.skip(16);
        int commentLength = inp.readLeUnsignedShort();
        if (commentLength == 0)
          return null;
        return inp.readString(commentLength, false);
      }
    catch (IOException _)
      {
        return null;
      }
  }

  private static class ZipEntryEnumeration implements Enumeration<ZipEntry>
  {
    private final Iterator<ZipEntry> elements;

    public ZipEntryEnumeration(Iterator<ZipEntry> elements)
    {
      this.elements = elements;
    }

    public boolean hasMoreElements()
    {
      return elements.hasNext();
    }

    public ZipEntry nextElement()
    {
      /* We return a clone, just to be safe that the user doesn't
       * change the entry.  
       */
      return (ZipEntry) (elements.next().clone());
    }
  }

  private class PartialInputStream extends InputStream
  {
    private final byte[] buffer;
    private long bufferOffset;
    private int pos;
    private long end;
    private boolean lazy;
    // We may need to supply an extra dummy byte to our reader.
    // See Inflater.  We use a count here to simplify the logic
    // elsewhere in this class.  Note that we ignore the dummy
    // byte in methods where we know it is not needed.
    private int dummyByteCount;

    public PartialInputStream(int bufferSize)
      throws IOException
    {
      buffer = new byte[bufferSize];
      bufferOffset = -buffer.length;
      pos = buffer.length;
      end = raf.length();
      lazy = true;
    }

    // Seek to the "end of central directory record" and position
    // the stream after the ENDSIG and return the file offset
    // where the record starts.
    long seekEndOfCentralDirectory() throws IOException
    {
      /* Search for the End Of Central Directory.  When a zip comment is 
       * present the directory may start earlier.
       * Note that a comment has a maximum length of 64K, so that is the
       * maximum we search backwards.
       */
      long length = raf.length();
      if (length == 0)
        throw new ZipException("zip file is empty");
      long pos = length - ENDHDR;
      long top = Math.max(0, pos - 65536);
      do
        {
          if (pos < top)
            throw new ZipException("error in opening zip file");
          seek(pos--);
        }
      while (readLeInt() != ENDSIG);
      return pos + 1;
    }

    void lazyInitialSeek() throws IOException
    {
    }

    void setLength(long length)
    {
      end = bufferOffset + pos + length;
    }

    private void fillBuffer() throws IOException
    {
      if (closed)
        throw new ZipException("ZipFile closed");

      synchronized (raf)
        {
          long len = end - bufferOffset;
          if (len == 0 && dummyByteCount > 0)
            {
              buffer[0] = 0;
              dummyByteCount = 0;
            }
          else
            {
              raf.seek(bufferOffset);
              raf.readFully(buffer, 0, (int) Math.min(buffer.length, len));
            }
        }
    }
    
    public int available() throws IOException
    {
      if (lazy)
        {
          lazy = false;
          lazyInitialSeek();
        }
      long amount = end - (bufferOffset + pos);
      if (amount > Integer.MAX_VALUE)
        return Integer.MAX_VALUE;
      return (int) amount;
    }
    
    public int read() throws IOException
    {
      if (bufferOffset + pos >= end + dummyByteCount)
        return -1;
      if (pos == buffer.length)
        {
          if (lazy)
            {
              lazy = false;
              lazyInitialSeek();
              return read();
            }
          bufferOffset += buffer.length;
          pos = 0;
          fillBuffer();
        }
      
      return buffer[pos++] & 0xFF;
    }

    public int read(byte[] b, int off, int len) throws IOException
    {
      if (lazy)
        {
          lazy = false;
          lazyInitialSeek();
        }

      if (len > end + dummyByteCount - (bufferOffset + pos))
        {
          len = (int) (end + dummyByteCount - (bufferOffset + pos));
          if (len == 0)
            return -1;
        }

      int totalBytesRead = Math.min(buffer.length - pos, len);
      System.arraycopy(buffer, pos, b, off, totalBytesRead);
      pos += totalBytesRead;
      off += totalBytesRead;
      len -= totalBytesRead;

      while (len > 0)
        {
          bufferOffset += buffer.length;
          pos = 0;
          fillBuffer();
          int remain = Math.min(buffer.length, len);
          System.arraycopy(buffer, pos, b, off, remain);
          pos += remain;
          off += remain;
          len -= remain;
          totalBytesRead += remain;
        }
      
      return totalBytesRead;
    }

    public long skip(long amount) throws IOException
    {
      if (lazy)
        {
          lazy = false;
          lazyInitialSeek();
        }
      if (amount < 0)
        return 0;
      if (amount > end - (bufferOffset + pos))
        amount = end - (bufferOffset + pos);
      seek(bufferOffset + pos + amount);
      return amount;
    }

    void seek(long newpos) throws IOException
    {
      long offset = newpos - bufferOffset;
      if (offset >= 0 && offset <= buffer.length)
        {
          pos = (int) offset;
        }
      else
        {
          bufferOffset = newpos;
          pos = 0;
          fillBuffer();
        }
    }

    long position()
    {
      return bufferOffset + pos;
    }

    void readFully(byte[] buf) throws IOException
    {
      if (read(buf, 0, buf.length) != buf.length)
        throw new EOFException();
    }

    void readFully(byte[] buf, int off, int len) throws IOException
    {
      if (read(buf, off, len) != len)
        throw new EOFException();
    }

    int readLeUnsignedShort() throws IOException
    {
      int result;
      if(pos + 1 < buffer.length)
        {
          result = ((buffer[pos + 0] & 0xff) | (buffer[pos + 1] & 0xff) << 8);
          pos += 2;
        }
      else
        {
          int b0 = read();
          int b1 = read();
          if (b1 == -1)
            throw new EOFException();
          result = (b0 & 0xff) | (b1 & 0xff) << 8;
        }
      return result;
    }

    int readLeInt() throws IOException
    {
      int result;
      if(pos + 3 < buffer.length)
        {
          result = (((buffer[pos + 0] & 0xff) | (buffer[pos + 1] & 0xff) << 8)
                   | ((buffer[pos + 2] & 0xff)
                       | (buffer[pos + 3] & 0xff) << 8) << 16);
          pos += 4;
        }
      else
        {
          int b0 = read();
          int b1 = read();
          int b2 = read();
          int b3 = read();
          if (b3 == -1)
            throw new EOFException();
          result =  (((b0 & 0xff) | (b1 & 0xff) << 8) | ((b2 & 0xff)
                    | (b3 & 0xff) << 8) << 16);
        }
      return result;
    }

    final long readLeUnsignedInt() throws IOException
    {
      return readLeInt() & 0xffffffffL;
    }
    
    final long readLeLong() throws IOException
    {
      return readLeUnsignedInt() | (readLeUnsignedInt() << 32);
    }

    /**
     * Decode chars from byte buffer using charset encoding.  This
     * operation is performance-critical since a jar file contains a
     * large number of strings for the name of each file in the
     * archive.  This routine therefore avoids using the expensive
     * utf8Decoder when decoding is straightforward.
     *
     * @param buffer the buffer that contains the encoded character
     *        data
     * @param pos the index in buffer of the first byte of the encoded
     *        data
     * @param length the length of the encoded data in number of
     *        bytes.
     *
     * @return a String that contains the decoded characters.
     */
    private String decodeChars(byte[] buffer, int pos, int length, boolean utf8)
      throws IOException
    {
      if (!utf8 && charset != StandardCharsets.UTF_8)
        return new String(buffer, pos, length, charset);

      for (int i = pos; i < pos + length; i++)
        {
          if (buffer[i] <= 0)
            return new String(buffer, pos, length, "UTF-8");
        }
      return new String(buffer, 0, pos, length);
    }

    String readString(int length, boolean utf8) throws IOException
    {
      if (length > end - (bufferOffset + pos))
        throw new EOFException();

      String result = null;
      try
        {
          if (buffer.length - pos >= length)
            {
              result = decodeChars(buffer, pos, length, utf8);
              pos += length;
            }
          else
            {
              byte[] b = new byte[length];
              readFully(b);
              result = decodeChars(b, 0, length, utf8);
            }
        }
      catch (UnsupportedEncodingException uee)
        {
          throw new AssertionError(uee);
        }
      return result;
    }

    public void addDummyByte()
    {
      dummyByteCount = 1;
    }
  }

  static {
    sun.misc.SharedSecrets.setJavaUtilZipFileAccess(
      new sun.misc.JavaUtilZipFileAccess() {
        public boolean startsWithLocHeader(ZipFile zip) {
          return zip.hasLocHeader;
        }
      }
    );
  }
}
