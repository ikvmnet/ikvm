/* RandomAccessFile.java -- Class supporting random file I/O
   Copyright (C) 1998 Free Software Foundation, Inc.

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


package java.io;

import gnu.classpath.Configuration;
import java.nio.channels.FileChannel;
import gnu.java.nio.FileChannelImpl;

/**
  * This class allows reading and writing of files at random locations.
  * Most Java I/O classes are either pure sequential input or output.  This
  * class fulfills the need to be able to read the bytes of a file in an
  * arbitrary order.  In addition, this class implements the
  * <code>DataInput</code> and <code>DataOutput</code> interfaces to allow
  * the reading and writing of Java primitives.
  *
  * @version 0.0
  *
  * @author Aaron M. Renn (arenn@urbanophile.com)
  */
public class RandomAccessFile implements DataOutput, DataInput
{

  static
  {
    if (Configuration.INIT_LOAD_LIBRARY)
      {
        System.loadLibrary ("javaio");
      }
  }

/*************************************************************************/

/*
 * Instance Variables
 */

/**
  * The native file descriptor for this file
  */
private FileDescriptor fd;

/**
  * Whether or not this file is open in read only mode
  */
private boolean read_only;

/*************************************************************************/

/*
 * Constructors
 */

/**
  * This method initializes a new instance of <code>RandomAccessFile</code>
  * to read from the specified file name with the specified access mode.
  * The access mode is either "r" for read only access or "rw" for read
  * write access.
  * <p>
  * Note that a <code>SecurityManager</code> check is made prior to
  * opening the file to determine whether or not this file is allowed to
  * be read or written.
  *
  * @param name The name of the file to read and/or write
  * @param mode "r" for read only or "rw" for read-write access to the file
  *
  * @exception IllegalArgumentException If <code>mode</code> has an illegal value
  * @exception SecurityException If the requested access to the file is not allowed
  * @exception IOException If any other error occurs
  */
public
RandomAccessFile(String name, String mode) throws IllegalArgumentException,
                                                  SecurityException,
                                                  IOException
{
  this(new File(name), mode);
}

/*************************************************************************/

/**
  * This method initializes a new instance of <code>RandomAccessFile</code>
  * to read from the specified <code>File</code> object with the specified 
  * access mode.   The access mode is either "r" for read only access or "rw" 
  * for read-write access.
  * <p>
  * Note that a <code>SecurityManager</code> check is made prior to
  * opening the file to determine whether or not this file is allowed to
  * be read or written.
  *
  * @param file The <code>File</code> object to read and/or write.
  * @param mode "r" for read only or "rw" for read-write access to the file
  *
  * @exception IllegalArgumentException If <code>mode</code> has an illegal value
  * @exception SecurityException If the requested access to the file is not allowed
  * @exception IOException If any other error occurs
  */
public
RandomAccessFile(File file, String mode) throws IllegalArgumentException,
                                                  SecurityException,
                                                  IOException
{
  // Check the mode
  if (!mode.equals("r") && !mode.equals("rw"))
    throw new IllegalArgumentException("Bad mode value: " + mode);

  // The obligatory SecurityManager stuff
  SecurityManager sm = System.getSecurityManager();
  if (sm != null)
    {
      if (mode.equals("r"))
        sm.checkRead(file.getPath());
      else if (mode.equals("rw"))
        sm.checkWrite(file.getPath());
    }

  if (mode.equals("r"))
    read_only = true;

	fd = FileDescriptor.open(file.getPath(), read_only ? FileDescriptor.Read : FileDescriptor.ReadWrite);
}

/*************************************************************************/

/*
 * Instance Methods
 */

/**
  * This method closes the file and frees up all file related system
  * resources.  Since most operating systems put a limit on how many files
  * may be opened at any given time, it is a good idea to close all files
  * when no longer needed to avoid hitting this limit
  */
public void
close() throws IOException
{
  fd.close();
}

/*************************************************************************/

/**
  * This method returns a <code>FileDescriptor</code> object that 
  * represents the native file handle for this file.
  *
  * @return The <code>FileDescriptor</code> object for this file
  *
  * @exception IOException If an error occurs
  */
public final FileDescriptor
getFD() throws IOException
{
  return fd;
}

/*************************************************************************/

/**
  * This method returns the current offset in the file at which the next
  * read or write will occur
  *
  * @return The current file position
  *
  * @exception IOException If an error occurs
  */
public long
getFilePointer() throws IOException
{
  return fd.getFilePointer();
}

/*************************************************************************/

/**
  * This method returns the length of the file in bytes
  *
  * @return The length of the file
  *
  * @exception IOException If an error occurs
  */
public long
length() throws IOException
{
  return fd.getLength();
}

/*************************************************************************/

/**
  * This method sets the current file position to the specified offset 
  * from the beginning of the file.  Note that some operating systems will
  * allow the file pointer to be set past the current end of the file.
  *
  * @param pos The offset from the beginning of the file at which to set the file pointer
  *
  * @exception IOException If an error occurs
  */
public void
seek(long pos) throws IOException
{
  fd.seek(pos);
}

/*************************************************************************/

/**
  * This method sets the length of the file to the specified length.  If
  * the currently length of the file is longer than the specified length,
  * then the file is truncated to the specified length.  If the current
  * length of the file is shorter than the specified length, the file
  * is extended with bytes of an undefined value.
  *  <p>
  * The file must be open for write access for this operation to succeed.
  *
  * @param newlen The new length of the file
  *
  * @exception IOException If an error occurs
  */
public void
setLength(long newlen) throws IOException
{
  if (read_only)
    throw new IOException("File is open read only");

  fd.setLength(newlen);
}

/*************************************************************************/

/**
  * This method reads a single byte of data from the file and returns it
  * as an integer.
  *
  * @return The byte read as an int, or -1 if the end of the file was reached.
  *
  * @exception IOException If an error occurs
  */
public int
read() throws IOException
{
  byte[] buf = new byte[1];

  int rc = fd.read(buf, 0, buf.length);
  if (rc == 0)
    return(-1);

  return(buf[0] & 0xFF);
}

/*************************************************************************/

/**
  * This method reads bytes from the file into the specified array.  The
  * bytes are stored starting at the beginning of the array and up to 
  * <code>buf.length</code> bytes can be read.
  *
  * @param buf The buffer to read bytes from the file into
  *
  * @return The actual number of bytes read or -1 if end of file
  *
  * @exception IOException If an error occurs
  */
public int
read(byte[] buf) throws IOException
{
  int rc = fd.read(buf, 0, buf.length);
  if (rc == 0)
    return(-1);
  else
    return(rc);
}

/*************************************************************************/

/**
  * This methods reads up to <code>len</code> bytes from the file into the s
  * pecified array starting at position <code>offset</code> into the array.
  *
  * @param buf The array to read the bytes into
  * @param offset The index into the array to start storing bytes
  * @param len The requested number of bytes to read
  *
  * @param len The actual number of bytes read, or -1 if end of file
  *
  * @exception IOException If an error occurs
  */
public int
read(byte[] buf, int offset, int len) throws IOException
{
  int rc = fd.read(buf, offset, len);
  if (rc == 0)
    return(-1);
  else
    return(rc);
}

/*************************************************************************/

/**
  * This method reads a Java boolean value from an input stream.  It does
  * so by reading a single byte of data.  If that byte is zero, then the
  * value returned is <code>false</code>  If the byte is non-zero, then
  * the value returned is <code>true</code>
  * <p>
  * This method can read a <code>boolean</code> written by an object implementing the
  * <code>writeBoolean()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>boolean</code> value read
  *
  * @exception EOFException If end of file is reached before reading the boolean
  * @exception IOException If any other error occurs
  */
public final boolean
readBoolean() throws EOFException, IOException
{
  int byte_read = read();

  if (byte_read == -1)
    throw new EOFException("Unexpected end of stream");

  return(DataInputStream.convertToBoolean(byte_read));
}

/*************************************************************************/

/**
  * This method reads a Java byte value from an input stream.  The value
  * is in the range of -128 to 127.
  * <p>
  * This method can read a <code>byte</code> written by an object implementing the 
  * <code>writeByte()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>byte</code> value read
  *
  * @exception EOFException If end of file is reached before reading the byte
  * @exception IOException If any other error occurs
  *
  * @see DataOutput
  */
public final byte
readByte() throws EOFException, IOException
{
  int byte_read = read();

  if (byte_read == -1)
    throw new EOFException("Unexpected end of stream");

  return(DataInputStream.convertToByte(byte_read));
}

/*************************************************************************/

/**
  * This method reads 8 unsigned bits into a Java <code>int</code> value from the 
  * stream. The value returned is in the range of 0 to 255.
  * <p>
  * This method can read an unsigned byte written by an object implementing the
  * <code>writeUnsignedByte()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The unsigned bytes value read as a Java <code>int</code>
  *
  * @exception EOFException If end of file is reached before reading the value
  * @exception IOException If any other error occurs
  *
  * @see DataOutput
  */
public final int
readUnsignedByte() throws EOFException, IOException
{
  int byte_read = read();

  if (byte_read == -1)
    throw new EOFException("Unexpected end of stream");

  return(DataInputStream.convertToUnsignedByte(byte_read));
}

/*************************************************************************/

/**
  * This method reads a Java <code>char</code> value from an input stream.  
  * It operates by reading two bytes from the stream and converting them to 
  * a single 16-bit Java <code>char</code>  The two bytes are stored most
  * significant byte first (i.e., "big endian") regardless of the native
  * host byte ordering. 
  * <p>
  * As an example, if <code>byte1</code> and code{byte2</code> represent the first
  * and second byte read from the stream respectively, they will be
  * transformed to a <code>char</code> in the following manner:
  * <p>
  * <code>(char)(((byte1 & 0xFF) << 8) | (byte2 & 0xFF)</code>
  * <p>
  * This method can read a <code>char</code> written by an object implementing the
  * <code>writeChar()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>char</code> value read 
  *
  * @exception EOFException If end of file is reached before reading the char
  * @exception IOException If any other error occurs
  *
  * @see DataOutput
  */
public final char
readChar() throws EOFException, IOException
{
  byte[] buf = new byte[2];

  readFully(buf);

  return(DataInputStream.convertToChar(buf));
}

/*************************************************************************/

/**
  * This method reads a signed 16-bit value into a Java in from the stream.
  * It operates by reading two bytes from the stream and converting them to 
  * a single 16-bit Java <code>short</code>  The two bytes are stored most
  * significant byte first (i.e., "big endian") regardless of the native
  * host byte ordering. 
  * <p>
  * As an example, if <code>byte1</code> and code{byte2</code> represent the first
  * and second byte read from the stream respectively, they will be
  * transformed to a <code>short</code> in the following manner:
  * <p>
  * <code>(short)(((byte1 & 0xFF) << 8) | (byte2 & 0xFF)</code>
  * <p>
  * The value returned is in the range of -32768 to 32767.
  * <p>
  * This method can read a <code>short</code> written by an object implementing the
  * <code>writeShort()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>short</code> value read
  *
  * @exception EOFException If end of file is reached before reading the value
  * @exception IOException If any other error occurs
  *
  * @see DataOutput
  */
public final short
readShort() throws EOFException, IOException
{
  byte[] buf = new byte[2];

  readFully(buf);

  return(DataInputStream.convertToShort(buf));
}

/*************************************************************************/

/**
  * This method reads 16 unsigned bits into a Java int value from the stream.
  * It operates by reading two bytes from the stream and converting them to 
  * a single Java <code>int</code>  The two bytes are stored most
  * significant byte first (i.e., "big endian") regardless of the native
  * host byte ordering. 
  * <p>
  * As an example, if <code>byte1</code> and code{byte2</code> represent the first
  * and second byte read from the stream respectively, they will be
  * transformed to an <code>int</code> in the following manner:
  * <p>
  * <code>(int)(((byte1 & 0xFF) << 8) + (byte2 & 0xFF))</code>
  * <p>
  * The value returned is in the range of 0 to 65535.
  * <p>
  * This method can read an unsigned short written by an object implementing
  * the <code>writeUnsignedShort()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The unsigned short value read as a Java <code>int</code>
  *
  * @exception EOFException If end of file is reached before reading the value
  * @exception IOException If any other error occurs
  */
public final int
readUnsignedShort() throws EOFException, IOException
{
  byte[] buf = new byte[2];

  readFully(buf);

  return(DataInputStream.convertToUnsignedShort(buf));
}

/*************************************************************************/

/**
  * This method reads a Java <code>int</code> value from an input stream
  * It operates by reading four bytes from the stream and converting them to 
  * a single Java <code>int</code>  The bytes are stored most
  * significant byte first (i.e., "big endian") regardless of the native
  * host byte ordering. 
  * <p>
  * As an example, if <code>byte1</code> through <code>byte4</code> represent the first
  * four bytes read from the stream, they will be
  * transformed to an <code>int</code> in the following manner:
  * <p>
  * <code>(int)(((byte1 & 0xFF) << 24) + ((byte2 & 0xFF) << 16) + 
  * ((byte3 & 0xFF) << 8) + (byte4 & 0xFF)))</code>
  * <p>
  * The value returned is in the range of 0 to 65535.
  * <p>
  * This method can read an <code>int</code> written by an object implementing the
  * <code>writeInt()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>int</code> value read
  *
  * @exception EOFException If end of file is reached before reading the int
  * @exception IOException If any other error occurs
  *
  * @see DataOutput
  */
public final int
readInt() throws EOFException, IOException
{
  byte[] buf = new byte[4];

  readFully(buf);

  return(DataInputStream.convertToInt(buf));
}

/*************************************************************************/

/**
  * This method reads a Java long value from an input stream
  * It operates by reading eight bytes from the stream and converting them to 
  * a single Java <code>long</code>  The bytes are stored most
  * significant byte first (i.e., "big endian") regardless of the native
  * host byte ordering. 
  * <p>
  * As an example, if <code>byte1</code> through <code>byte8</code> represent the first
  * eight bytes read from the stream, they will be
  * transformed to an <code>long</code> in the following manner:
  * <p>
  * <code>(long)((((long)byte1 & 0xFF) << 56) + (((long)byte2 & 0xFF) << 48) + 
  * (((long)byte3 & 0xFF) << 40) + (((long)byte4 & 0xFF) << 32) + 
  * (((long)byte5 & 0xFF) << 24) + (((long)byte6 & 0xFF) << 16) + 
  * (((long)byte7 & 0xFF) << 8) + ((long)byte9 & 0xFF)))</code>
  * <p>
  * The value returned is in the range of 0 to 65535.
  * <p>
  * This method can read an <code>long</code> written by an object implementing the
  * <code>writeLong()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>long</code> value read
  *
  * @exception EOFException If end of file is reached before reading the long
  * @exception IOException If any other error occurs
  *
  * @see DataOutput
  */
public final long
readLong() throws EOFException, IOException
{
  byte[] buf = new byte[8];

  readFully(buf);

  return(DataInputStream.convertToLong(buf));
}

/*************************************************************************/

/**
  * This method reads a Java float value from an input stream.  It operates
  * by first reading an <code>int</code> value from the stream by calling the
  * <code>readInt()</code> method in this interface, then converts that <code>int</code>
  * to a <code>float</code> using the <code>intBitsToFloat</code> method in 
  * the class <code>java.lang.Float</code>
  * <p>
  * This method can read a <code>float</code> written by an object implementing the
  * <code>writeFloat()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>float</code> value read
  *
  * @exception EOFException If end of file is reached before reading the float
  * @exception IOException If any other error occurs
  *
  * @see java.lang.Float
  * @see DataOutput
  */
public final float
readFloat() throws EOFException, IOException
{
  int val = readInt();

  return(Float.intBitsToFloat(val));
}

/*************************************************************************/

/**
  * This method reads a Java double value from an input stream.  It operates
  * by first reading a <code>logn</code> value from the stream by calling the
  * <code>readLong()</code> method in this interface, then converts that <code>long</code>
  * to a <code>double</code> using the <code>longBitsToDouble</code> method in 
  * the class <code>java.lang.Double</code>
  * <p>
  * This method can read a <code>double</code> written by an object implementing the
  * <code>writeDouble()</code> method in the <code>DataOutput</code> interface.
  *
  * @return The <code>double</code> value read
  *
  * @exception EOFException If end of file is reached before reading the double
  * @exception IOException If any other error occurs
  *
  * @see java.lang.Double
  * @see DataOutput
  */
public final double
readDouble() throws EOFException, IOException
{
  long val = readLong();

  return(Double.longBitsToDouble(val));
}

/*************************************************************************/

/**
  * This method reads the next line of text data from an input stream.
  * It operates by reading bytes and converting those bytes to <code>char</code>
  * values by treating the byte read as the low eight bits of the <code>char</code>
  * and using <code>0</code> as the high eight bits.  Because of this, it does
  * not support the full 16-bit Unicode character set.
  * <p>
  * The reading of bytes ends when either the end of file or a line terminator
  * is encountered.  The bytes read are then returned as a <code>String</code>
  * A line terminator is a byte sequence consisting of either 
  * <code>\r</code> <code>\n</code> or <code>\r\n</code>  These termination charaters are
  * discarded and are not returned as part of the string.
  * <p>
  * This method can read data that was written by an object implementing the
  * <code>writeLine()</code> method in <code>DataOutput</code>
  *
  * @return The line read as a <code>String</code>
  *
  * @exception IOException If an error occurs
  *
  * @see DataOutput
  *
  * @deprecated
  */
public final String
readLine() throws IOException
{
  StringBuffer sb = new StringBuffer("");

  for (;;)
    {
      int byte_read = read();
 
      if (byte_read == -1)
        return(sb.toString());

      char c = (char)byte_read;

      if (c == '\r')
        {
          byte_read = read();
          if (((char)byte_read) != '\n')
            seek(getFilePointer() - 1);

          return(sb.toString());
        }

      if (c == '\n')
        return(sb.toString());

      sb.append(c);
    }
}

/*************************************************************************/

/**
  * This method reads a <code>String</code> from an input stream that is encoded in
  * a modified UTF-8 format.  This format has a leading two byte sequence
  * that contains the remaining number of bytes to read.  This two byte
  * sequence is read using the <code>readUnsignedShort()</code> method of this
  * interface.
  * <p>
  * After the number of remaining bytes have been determined, these bytes
  * are read an transformed into <code>char</code> values.  These <code>char</code> values
  * are encoded in the stream using either a one, two, or three byte format.
  * The particular format in use can be determined by examining the first
  * byte read.  
  * <p>
  * If the first byte has a high order bit of 0 then
  * that character consists on only one byte.  This character value consists
  * of seven bits that are at positions 0 through 6 of the byte.  As an
  * example, if <code>byte1</code> is the byte read from the stream, it would
  * be converted to a <code>char</code> like so:
  * <p>
  * <code>(char)byte1</code>
  * <p>
  * If the first byte has <code>110</code> as its high order bits, then the 
  * character consists of two bytes.  The bits that make up the character
  * value are in positions 0 through 4 of the first byte and bit positions
  * 0 through 5 of the second byte.  (The second byte should have 
  * 10 as its high order bits).  These values are in most significant
  * byte first (i.e., "big endian") order.
  * <p>
  * As an example, if <code>byte1</code> and <code>byte2</code> are the first two bytes
  * read respectively, and the high order bits of them match the patterns
  * which indicate a two byte character encoding, then they would be
  * converted to a Java <code>char</code> like so:
  * <p>
  * <code>(char)(((byte1 & 0x1F) << 6) | (byte2 & 0x3F))</code>
  * <p>
  * If the first byte has a <code>1110</code> as its high order bits, then the
  * character consists of three bytes.  The bits that make up the character
  * value are in positions 0 through 3 of the first byte and bit positions
  * 0 through 5 of the other two bytes.  (The second and third bytes should
  * have <code>10</code> as their high order bits).  These values are in most
  * significant byte first (i.e., "big endian") order.
  * <p>
  * As an example, if <code>byte1</code> <code>byte2</code> and <code>byte3</code> are the
  * three bytes read, and the high order bits of them match the patterns
  * which indicate a three byte character encoding, then they would be
  * converted to a Java <code>char</code> like so:
  * <p>
  * <code>(char)(((byte1 & 0x0F) << 12) | ((byte2 & 0x3F) << 6) | (byte3 & 0x3F))</code>
  * <p>
  * Note that all characters are encoded in the method that requires the
  * fewest number of bytes with the exception of the character with the
  * value of <code>&#92;u0000</code> which is encoded as two bytes.  This is a 
  * modification of the UTF standard used to prevent C language style
  * <code>NUL</code> values from appearing in the byte stream.
  * <p>
  * This method can read data that was written by an object implementing the
  * <code>writeUTF()</code> method in <code>DataOutput</code>
  * 
  * @return The <code>String</code> read
  *
  * @exception EOFException If end of file is reached before reading the String
  * @exception UTFDataFormatException If the data is not in UTF-8 format
  * @exception IOException If any other error occurs
  *
  * @see DataOutput
  */
public final String
readUTF() throws EOFException, UTFDataFormatException, IOException
{
  StringBuffer sb = new StringBuffer("");

  int num_bytes = readUnsignedShort();
  byte[] buf = new byte[num_bytes];
  readFully(buf);

  return(DataInputStream.convertFromUTF(buf));
}

/*************************************************************************/

/**
  * This method reads raw bytes into the passed array until the array is
  * full.  Note that this method blocks until the data is available and
  * throws an exception if there is not enough data left in the stream to
  * fill the buffer
  *
  * @param buf The buffer into which to read the data
  *
  * @exception EOFException If end of file is reached before filling the buffer
  * @exception IOException If any other error occurs
  */
public final void
readFully(byte[] buf) throws EOFException, IOException
{
  readFully(buf, 0, buf.length);
}

/*************************************************************************/

/**
  * This method reads raw bytes into the passed array <code>buf</code> starting
  * <code>offset</code> bytes into the buffer.  The number of bytes read will be
  * exactly <code>len</code>  Note that this method blocks until the data is 
  * available and * throws an exception if there is not enough data left in 
  * the stream to read <code>len</code> bytes.
  *
  * @param buf The buffer into which to read the data
  * @param offset The offset into the buffer to start storing data
  * @param len The number of bytes to read into the buffer
  *
  * @exception EOFException If end of file is reached before filling the buffer
  * @exception IOException If any other error occurs
  */
public final void
readFully(byte[] buf, int offset, int len) throws EOFException, IOException
{
  int total_read = 0;

  while (total_read < len)
    {
      int bytes_read = read(buf, offset + total_read, len - total_read);
      if (bytes_read == -1)
        throw new EOFException("Unexpected end of stream");

      total_read += bytes_read;
    }
}

/*************************************************************************/

/**
  * This method attempts to skip and discard the specified number of bytes 
  * in the input stream.  It may actually skip fewer bytes than requested. 
  * The actual number of bytes skipped is returned.  This method will not
  * skip any bytes if passed a negative number of bytes to skip.
  *
  * @param num_bytes The requested number of bytes to skip.
  *
  * @return The number of bytes actually skipped.
  *
  * @exception IOException If an error occurs.
  */
public int
skipBytes(int n) throws EOFException, IOException
{
  if (n <= 0)
    return(0);

  long total_skipped = fd.skip(n);

  return((int)n);
}

/*************************************************************************/

/**
  * This method writes a single byte of data to the file. The file must
  * be open for read-write in order for this operation to succeed.
  *
  * @param The byte of data to write, passed as an int.
  *
  * @exception IOException If an error occurs
  */
public void
write(int b) throws IOException
{
  if (read_only)
    throw new IOException("File is open read only");

  byte[] buf = new byte[1];
  buf[0] = (byte)b;
  
  fd.write(buf, 0, buf.length);
}

/*************************************************************************/

/**
  * This method writes all the bytes in the specified array to the file.
  * The file must be open read-write in order for this operation to succeed.
  *
  * @param buf The array of bytes to write to the file
  */
public void
write(byte[] buf) throws IOException
{
  if (read_only)
    throw new IOException("File is open read only");

  fd.write(buf, 0, buf.length);
}
  
/*************************************************************************/

/**
  * This method writes <code>len</code> bytes to the file from the specified
  * array starting at index <code>offset</code> into the array.
  *
  * @param buf The array of bytes to write to the file
  * @param offset The index into the array to start writing file
  * @param len The number of bytes to write
  *
  * @exception IOException If an error occurs
  */
public void
write(byte[] buf, int offset, int len) throws IOException
{
  if (read_only)
    throw new IOException("File is open read only");

  fd.write(buf, offset, len);
}

/*************************************************************************/

/**
  * This method writes a Java <code>boolean</code> to the underlying output 
  * stream. For a value of <code>true</code>, 1 is written to the stream.
  * For a value of <code>false</code>, 0 is written.
  *
  * @param b The <code>boolean</code> value to write to the stream
  *
  * @exception IOException If an error occurs
  */
public final void
writeBoolean(boolean b) throws IOException
{
  int bool = DataOutputStream.convertFromBoolean(b);
  write(bool);
}

/*************************************************************************/

/**
  * This method writes a Java <code>byte</code> value to the underlying
  * output stream.
  *
  * @param b The <code>byte</code> to write to the stream, passed as an <code>int</code>.
  *
  * @exception IOException If an error occurs
  */
public final void
writeByte(int b) throws IOException
{
  write(b);
}

/*************************************************************************/

/**
  * This method writes all the bytes in a <code>String</code> out to the
  * stream.  One byte is written for each character in the <code>String</code>.
  * The high eight bits of each character are discarded.
  *
  * @param s The <code>String</code> to write to the stream
  *
  * @exception IOException If an error occurs
  */
public final void
writeBytes(String s) throws IOException
{
  if (s.length() == 0)
    return;

  byte[] buf = new byte[s.length()];

  for (int i = 0; i < s.length(); i++)
    buf[i] = (byte)(s.charAt(i) & 0xFF);

  write(buf);
}

/*************************************************************************/

/**
  * This method writes a single <code>char</code> value to the stream,
  * high byte first.
  *
  * @param c The <code>char</code> value to write, passed as an <code>int</code>.
  *
  * @exception IOException If an error occurs
  */
public final void
writeChar(int c) throws IOException
{
  write(DataOutputStream.convertFromChar(c));
}

/*************************************************************************/

/**
  * This method writes all the characters in a <code>String</code> to the
  * stream.  There will be two bytes for each character value.  The high
  * byte of the character will be written first.
  *
  * @param s The <code>String</code> to write to the stream.
  *
  * @exception IOException If an error occurs
  */
public final void
writeChars(String s) throws IOException
{
  if (s.length() == 0)
    return;

  byte[] buf = DataOutputStream.getConvertedStringChars(s);
  write(buf);
}

/*************************************************************************/

/**
  * This method writes a Java <code>short</code> to the stream, high byte
  * first.  This method requires two bytes to encode the value.
  *
  * @param s The <code>short</code> value to write to the stream, passed as an <code>int</code>.
  *
  * @exception IOException If an error occurs
  */
public final void
writeShort(int s) throws IOException
{
  write(DataOutputStream.convertFromShort(s));
}

/*************************************************************************/

/**
  * This method writes a Java <code>int</code> to the stream, high bytes
  * first.  This method requires four bytes to encode the value.
  *
  * @param i The <code>int</code> value to write to the stream.
  *
  * @exception IOException If an error occurs
  */
public final void
writeInt(int i) throws IOException
{
  write(DataOutputStream.convertFromInt(i));
}

/*************************************************************************/

/**
  * This method writes a Java <code>long</code> to the stream, high bytes
  * first.  This method requires eight bytes to encode the value.
  *
  * @param l The <code>long</code> value to write to the stream.
  *
  * @exception IOException If an error occurs
  */
public final void
writeLong(long l) throws IOException
{
  write(DataOutputStream.convertFromLong(l));
}

/*************************************************************************/

/**
  * This method writes a Java <code>float</code> value to the stream.  This
  * value is written by first calling the method <code>Float.floatToIntBits</code>
  * to retrieve an <code>int</code> representing the floating point number,
  * then writing this <code>int</code> value to the stream exactly the same
  * as the <code>writeInt()</code> method does.
  *
  * @param f The floating point number to write to the stream.
  *
  * @exception IOException If an error occurs
  *
  * @see writeInt
  */
public final void
writeFloat(float f) throws IOException
{
  int i = Float.floatToIntBits(f);
  writeInt(i);
}

/*************************************************************************/

/**
  * This method writes a Java <code>double</code> value to the stream.  This
  * value is written by first calling the method <code>Double.doubleToLongBits</code>
  * to retrieve an <code>long</code> representing the floating point number,
  * then writing this <code>long</code> value to the stream exactly the same
  * as the <code>writeLong()</code> method does.
  *
  * @param d The double precision floating point number to write to the stream.
  *
  * @exception IOException If an error occurs
  *
  * @see writeLong
  */
public final void
writeDouble(double d) throws IOException
{
  long l = Double.doubleToLongBits(d);
  writeLong(l);
}

/*************************************************************************/

/**
  * This method writes a Java <code>String</code> to the stream in a modified
  * UTF-8 format.  First, two bytes are written to the stream indicating the
  * number of bytes to follow.  Note that this is the number of bytes in the
  * encoded <code>String</code> not the <code>String</code> length.  Next
  * come the encoded characters.  Each character in the <code>String</code>
  * is encoded as either one, two or three bytes.  For characters in the
  * range of <code>&#92;u0001</code> to <code>&#92;u007F</code>, one byte is used.  The character
  * value goes into bits 0-7 and bit eight is 0.  For characters in the range
  * of <code>&#92;u0080</code> to <code>&#92;u007FF</code>, two bytes are used.  Bits
  * 6-10 of the character value are encoded bits 0-4 of the first byte, with
  * the high bytes having a value of "110".  Bits 0-5 of the character value
  * are stored in bits 0-5 of the second byte, with the high bits set to
  * "10".  This type of encoding is also done for the null character
  * <code>&#92;u0000</code>.  This eliminates any C style NUL character values
  * in the output.  All remaining characters are stored as three bytes.
  * Bits 12-15 of the character value are stored in bits 0-3 of the first
  * byte.  The high bits of the first bytes are set to "1110".  Bits 6-11
  * of the character value are stored in bits 0-5 of the second byte.  The
  * high bits of the second byte are set to "10".  And bits 0-5 of the
  * character value are stored in bits 0-5 of byte three, with the high bits
  * of that byte set to "10".
  *
  * @param s The <code>String</code> to write to the output in UTF format
  *
  * @exception IOException If an error occurs
  */
public final void
writeUTF(String s) throws IOException
{
  byte[] buf = DataOutputStream.convertToUTF(s);

  writeShort(buf.length);
  write(buf);
}

/*************************************************************************/

/**
 *  This method creates a java.nio.channels.FileChannel.
 * Nio does not allow one to create a file channel directly.
 * A file channel must be created by first creating an instance of
 * Input/Output/RandomAccessFile and invoking the getChannel() method on it.
 */

private FileChannel ch; /* cached associated file-channel */

public FileChannel 
getChannel() 
{
    synchronized (this) 
	{
//	    if (ch == null)
//		ch = new gnu.java.nio.FileChannelImpl(native_fd,
//						      this);
	}
    return ch;
}

/*************************************************************************/

/**
  * This method ensures that the underlying file is closed when this object
  * is garbage collected.  Since there is a system dependent limit on how
  * many files a single process can have open, it is a good idea to close
  * streams when they are no longer needed rather than waiting for garbage
  * collectio to automatically close them.
  *
  * @exception IOException If an error occurs
  */
protected void finalize() throws IOException
{
	if(fd != null)
	{
		close();
	}
}


} // class RandomAccessFile

