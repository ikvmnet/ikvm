/* FileOutputStream.java -- Writes to a file on disk.
   Copyright (C) 1998, 2001 Free Software Foundation, Inc.

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
  * This classes allows a stream of data to be written to a disk file or
  * any open <code>FileDescriptor</code>.
  *
  * @version 0.0
  *
  * @author Aaron M. Renn (arenn@urbanophile.com)
  */
public class FileOutputStream extends OutputStream
{

/*************************************************************************/

/*
 * Class Variables and Initializers
 */

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
  * This is the native file handle
  */
private FileDescriptor fd;

/*************************************************************************/

/*
 * Constructors
 */

/**
  * This method initializes a <code>FileOutputStream</code> object to write
  * to the named file.  The file is created if it does not exist, and
  * the bytes written are written starting at the beginning of the file.
  * <p>
  * Before opening a file, a security check is performed by calling the
  * <code>checkWrite</code> method of the <code>SecurityManager</code> (if
  * one exists) with the name of the file to be opened.  An exception is
  * thrown if writing is not allowed. 
  *
  * @param name The name of the file this stream should write to
  *
  * @exception SecurityException If write access to the file is not allowed
  * @exception IOException If a non-security error occurs
  */
public
FileOutputStream(String name) throws SecurityException, IOException
{
  this(name, false);
}

/*************************************************************************/

/**
  * This method initializes a <code>FileOutputStream</code> object to write
  * to the specified <code>File</code> object.  The file is created if it 
  * does not exist, and the bytes written are written starting at the 
  * beginning of the file.
  * <p>
  * Before opening a file, a security check is performed by calling the
  * <code>checkWrite</code> method of the <code>SecurityManager</code> (if
  * one exists) with the name of the file to be opened.  An exception is
  * thrown if writing is not allowed. 
  *
  * @param file The <code>File</code> object this stream should write to
  *
  * @exception SecurityException If write access to the file is not allowed
  * @exception IOException If a non-security error occurs
  */
public
FileOutputStream(File file) throws SecurityException, IOException
{
  this(file.getPath(), false);
}

/*************************************************************************/

/**
  * This method initializes a <code>FileOutputStream</code> object to write
  * to the named file.  The file is created if it does not exist, and
  * the bytes written are written starting at the beginning of the file if
  * the <code>append</code> argument is <code>false</code> or at the end
  * of the file if the <code>append</code> argument is true.
  * <p>
  * Before opening a file, a security check is performed by calling the
  * <code>checkWrite</code> method of the <code>SecurityManager</code> (if
  * one exists) with the name of the file to be opened.  An exception is
  * thrown if writing is not allowed. 
  *
  * @param name The name of the file this stream should write to
  * @param append <code>true</code> to append bytes to the end of the file, or <code>false</code> to write bytes to the beginning
  *
  * @exception SecurityException If write access to the file is not allowed
  * @exception IOException If a non-security error occurs
  */
public
FileOutputStream(String name, boolean append) throws SecurityException, 
                                                     IOException
{
  SecurityManager sm = System.getSecurityManager();
  if (sm != null)
    {
//      try
//        {
          sm.checkWrite(name);
//        }
//      catch(AccessControlException e)
//        {
//          throw new SecurityException(e.getMessage());
//        }
    }

  fd = FileDescriptor.open((new File(name)).getAbsolutePath(), append ? FileDescriptor.Append : FileDescriptor.Write);
}

/*************************************************************************/

/**
  * This method initializes a <code>FileOutputStream</code> object to write
  * to the file represented by the specified <code>FileDescriptor</code>
  * object.  This method does not create any underlying disk file or
  * reposition the file pointer of the given descriptor.  It assumes that
  * this descriptor is ready for writing as is.
  * <p>
  * Before opening a file, a security check is performed by calling the
  * <code>checkWrite</code> method of the <code>SecurityManager</code> (if
  * one exists) with the specified <code>FileDescriptor</code> as an argument.
  * An exception is thrown if writing is not allowed. 
  *
  * @param file The <code>FileDescriptor</code> this stream should write to
  *
  * @exception SecurityException If write access to the file is not allowed
  */
public
FileOutputStream(FileDescriptor fd) throws SecurityException
{
  // Hmm, no other exception but this one to throw, but if the descriptor
  // isn't valid, we surely don't have "permission" to write to it.
  if (!fd.valid())
    throw new SecurityException("Invalid FileDescriptor");

  SecurityManager sm = System.getSecurityManager();
  if (sm != null)
    {
//      try
//        {
         // sm.checkWrite(fd);
//        }
//      catch(AccessControlException e)
//        {
//          throw new SecurityException(e.getMessage());
//        }
    }

  this.fd = fd;
}

/*************************************************************************/

/**
  * This method returns a <code>FileDescriptor</code> object representing
  * the file that is currently being written to
  *
  * @return A <code>FileDescriptor</code> object for this stream
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
  * This method writes a single byte of data to the file.  
  *
  * @param b The byte of data to write, passed as an <code>int</code>
  *
  * @exception IOException If an error occurs
  */
public void
write(int b) throws IOException
{
  byte[] buf = new byte[1];

  buf[0] = (byte)(b & 0xFF);
  fd.write(buf, 0, buf.length);
}

/*************************************************************************/

/**
  * This method writes all the bytes in the specified array to the
  * file.
  *
  * @param buf The array of bytes to write to the file
  *
  * @exception IOException If an error occurs
  */
public void
write(byte[] buf) throws IOException
{
  fd.write(buf, 0, buf.length);
}

/*************************************************************************/

/**
  * This method writes <code>len</code> bytes from the byte array 
  * <code>buf</code> to the file starting at index <code>offset</code>.
  *
  * @param buf The array of bytes to write to the file
  * @param offset The offset into the array to start writing bytes from
  * @param len The number of bytes to write to the file
  *
  * @exception IOException If an error occurs
  */
public void
write(byte[] buf, int offset, int len) throws IOException
{
  fd.write(buf, offset, len);
}

/*************************************************************************/

/**
  * This method closes the underlying file.  Any further attempts to
  * write to this stream will likely generate an exception since the
  * file is closed.
  *
  * @exception IOException If an error occurs
  */
public void
close() throws IOException
{
	fd.close();
}

/*************************************************************************/

/**
  * This method closes the stream when this object is being garbage
  * collected.
  *
  * @exception IOException If an error occurs (ignored by the Java runtime)
  */
protected void
finalize() throws IOException
{
	if(fd != null)
	{
		close();
	}
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


} // class FileOutputStream

