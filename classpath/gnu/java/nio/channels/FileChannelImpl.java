/* FileChannelImpl.java -- 
   Copyright (C) 2002, 2004 Free Software Foundation, Inc.

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


package gnu.java.nio.channels;

import gnu.classpath.Configuration;
import gnu.java.nio.FileLockImpl;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.MappedByteBuffer;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.FileChannel;
import java.nio.channels.FileLock;
import java.nio.channels.NonReadableChannelException;
import java.nio.channels.NonWritableChannelException;
import java.nio.channels.ReadableByteChannel;
import java.nio.channels.WritableByteChannel;

import cli.System.Console;
import cli.System.IO.*;
import ikvm.lang.CIL;

/**
 * This file is not user visible !
 * But alas, Java does not have a concept of friendly packages
 * so this class is public. 
 * Instances of this class are created by invoking getChannel
 * Upon a Input/Output/RandomAccessFile object.
 */

public final class FileChannelImpl extends FileChannel
{
    // These are mode values for open().
    public static final int READ   = 1;
    public static final int WRITE  = 2;
    public static final int APPEND = 4;

    // EXCL is used only when making a temp file.
    public static final int EXCL   = 8;
    public static final int SYNC   = 16;
    public static final int DSYNC  = 32;

    private Stream stream;
    private int mode;

    public FileChannelImpl ()
    {
    }

    /* Open a file.  MODE is a combination of the above mode flags. */
    public FileChannelImpl (String path, int mode) throws FileNotFoundException
    {
	stream = open (path, mode);
	this.mode = mode;
    }

    public FileChannelImpl(Stream stream)
    {
	this.stream = stream;
	mode = (stream.get_CanRead() ? READ : 0) | (stream.get_CanWrite() ? WRITE : 0);
    }

    public static final FileChannelImpl in = new FileChannelImpl(Console.OpenStandardInput());
    public static final FileChannelImpl out = new FileChannelImpl(Console.OpenStandardOutput());
    public static final FileChannelImpl err = new FileChannelImpl(Console.OpenStandardError());

    private Stream open (String path, int mode) throws FileNotFoundException
    {
	if(stream != null)
	    throw new InternalError("FileChannelImpl already open");
	try
	{
	    int fileMode;
	    int fileAccess;
	    int fileShare;
	    // NOTE we don't support SYNC or DSYNC
	    switch(mode & (READ|WRITE|APPEND))
	    {
		case READ:
		    fileMode = FileMode.Open;
		    fileAccess = FileAccess.Read;
		    break;
		case READ|WRITE:
		    fileMode = FileMode.OpenOrCreate;
		    fileAccess = FileAccess.ReadWrite;
		    break;
		case WRITE:
		    fileMode = FileMode.Create;
		    fileAccess = FileAccess.Write;
		    break;
		case APPEND:
		case APPEND|WRITE:
		    fileMode = FileMode.Append;
		    fileAccess = FileAccess.Write;
		    break;
		default:
		    throw new IllegalArgumentException("Invalid mode value: " + mode);
	    }
	    if(false) throw new cli.System.IO.IOException();
	    if(false) throw new cli.System.Security.SecurityException();
	    if(false) throw new cli.System.UnauthorizedAccessException();
	    if(false) throw new cli.System.ArgumentException();
	    if(false) throw new cli.System.NotSupportedException();
	    return new cli.System.IO.FileStream(demanglePath(path), FileMode.wrap(fileMode), FileAccess.wrap(fileAccess), FileShare.wrap(FileShare.ReadWrite), 1, false);
	}
	catch(cli.System.Security.SecurityException x1)
	{
	    throw new SecurityException(x1.getMessage());
	}
	catch(cli.System.IO.IOException x2)
	{
	    throw new FileNotFoundException(x2.getMessage());
	}
	catch(cli.System.UnauthorizedAccessException x3)
	{
	    // this is caused by "name" being a directory instead of a file
	    throw new FileNotFoundException(x3.getMessage());
	}
	catch(cli.System.ArgumentException x4)
	{
	    throw new FileNotFoundException(x4.getMessage());
	}
	catch(cli.System.NotSupportedException x5)
	{
	    throw new FileNotFoundException(x5.getMessage());
	}
    }

    private static String demanglePath(String path)
    {
	// HACK for some reason Java accepts: \c:\foo.txt
	// I don't know what else, but for now lets just support this
	if(path.length() > 3 && (path.charAt(0) == '\\' || path.charAt(0) == '/') && path.charAt(2) == ':')
	{
	    path = path.substring(1);
	}
	return path;
    }

    public int available () throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    if(false) throw new cli.System.NotSupportedException();
	    if(stream.get_CanSeek())
		return (int)Math.min(Integer.MAX_VALUE, Math.max(0, stream.get_Length() - stream.get_Position()));
	    return 0;
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	catch(cli.System.NotSupportedException x1)
	{
	    // this means we have a broken Stream, because if CanSeek returns true, it must
	    // support Length and Position
	    return 0;
	}
    }

    private long implPosition () throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    return stream.get_Position();
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...
    }

    private void seek (long newPosition) throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    stream.Seek(newPosition, SeekOrigin.wrap(SeekOrigin.Begin));
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...
    }

    private void implTruncate (long size) throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	if (size < 0)
	    throw new IllegalArgumentException("Length cannot be less than zero " +
		size);

	try
	{
	    if(false) throw new cli.System.IO.IOException();
            if(size < stream.get_Length())
            {
	        stream.SetLength(size);
            }
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...
    }
  
    public void unlock (long pos, long len) throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    if(false) throw new cli.System.ArgumentOutOfRangeException();
	    ((FileStream)stream).Unlock(pos, len);
	}
	catch(ClassCastException c)
	{
	    throw new IOException("Locking not supported");
	}
	catch(cli.System.ArgumentOutOfRangeException x1)
	{
	    throw new IOException(x1.getMessage());
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...
    }

    public long size () throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    return stream.get_Length();
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...
    }
    
    protected void implCloseChannel() throws IOException
    {
	if(stream != null)
	{
	    // HACK don't close stdin because that throws a NotSupportedException (bug in System.IO.__ConsoleStream)
	    if(stream != in.stream)
	    {
		try
		{
		    stream.Close();
		    if(false) throw new cli.System.IO.IOException();
		}
		catch(cli.System.IO.IOException x)
		{
		    throw new IOException(x.getMessage());
		}
	    }
	    stream = null;
	}
    }

    public int read (ByteBuffer dst) throws IOException
    {
	int result;
	byte[] buffer = new byte [dst.remaining ()];
    
	result = read (buffer, 0, buffer.length);

	if (result > 0)
	    dst.put (buffer, 0, result);

	return result;
    }

    public int read (ByteBuffer dst, long position)
	throws IOException
    {
	if (position < 0)
	    throw new IllegalArgumentException ();
	long oldPosition = implPosition ();
	position (position);
	int result = read(dst);
	position (oldPosition);
    
	return result;
    }

    public int read () throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    return stream.ReadByte();
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...
    }

    public int read (byte[] buf, int offset, int len) throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	if (len == 0)
	    return(0);

	if ((offset < 0) || (offset > buf.length))
	    throw new IllegalArgumentException("Offset invalid: " + offset);

	if ((len < 0) || (len > (buf.length - offset)))
	    throw new IllegalArgumentException("Length invalid: " + len);

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    int count = stream.Read(buf, offset, len);
	    if(count == 0)
	    {
		count = -1;
	    }
	    return count;
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...
    }

    public long read (ByteBuffer[] dsts, int offset, int length)
	throws IOException
    {
	long result = 0;

	for (int i = offset; i < offset + length; i++)
	{
	    result += read (dsts [i]);
	}

	return result;
    }

    public int write (ByteBuffer src) throws IOException
    {
	int len = src.remaining ();
	if (src.hasArray())
	{
	    byte[] buffer = src.array();
	    write(buffer, src.arrayOffset() + src.position(), len);
            src.position(src.position() + len);
	}
	else
	{
	    // Use a more efficient native method! FIXME!
	    byte[] buffer = new byte [len];
	    src.get (buffer, 0, len);
	    write (buffer, 0, len);
	}
	return len;
    }
    
    public int write (ByteBuffer src, long position)
	throws IOException
    {
	if (position < 0)
	    throw new IllegalArgumentException ();

	if (!isOpen ())
	    throw new ClosedChannelException ();
    
	if ((mode & WRITE) == 0)
	    throw new NonWritableChannelException ();

	int result;
	long oldPosition;

	oldPosition = implPosition ();
	seek (position);
	result = write(src);
	seek (oldPosition);
    
	return result;
    }

    public void write(byte[] buf, int offset, int len) throws IOException
    {
	if(stream == null)
	    throw new ClosedChannelException();

	if (len == 0)
	    return;

	if ((offset < 0) || (offset > buf.length))
	    throw new IllegalArgumentException("Offset invalid: " + offset);

	if ((len < 0) || (len > (buf.length - offset)))
	    throw new IllegalArgumentException("Length invalid: " + len);

	try
	{
            if(false) throw new cli.System.IO.IOException();
            if(false) throw new cli.System.ObjectDisposedException(null);
	    stream.Write(buf, offset, len);
	    // NOTE FileStream buffers the output, so we have to flush explicitly
	    stream.Flush();
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new ClosedChannelException();
        }
    }
  
    public void write(int b) throws IOException
    {
        if(stream == null)
            throw new ClosedChannelException();

        try
        {
            if(false) throw new cli.System.IO.IOException();
            if(false) throw new cli.System.ObjectDisposedException(null);
            stream.WriteByte((byte)b);
            // NOTE FileStream buffers the output, so we have to flush explicitly
            stream.Flush();
        }
        catch(cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new ClosedChannelException();
        }
    }

    public long write(ByteBuffer[] srcs, int offset, int length)
	throws IOException
    {
	long result = 0;

	for (int i = offset;i < offset + length;i++)
	{
	    result += write (srcs[i]);
	}
    
	return result;
    }
				   
    public MappedByteBuffer mapImpl (char mode, long position, int size) throws IOException
    {
	// TODO
	throw new Error("not implemented");
    }

    public MappedByteBuffer map (FileChannel.MapMode mode,
	long position, long size)
	throws IOException
    {
	char nmode = 0;
	if (mode == MapMode.READ_ONLY)
	{
	    nmode = 'r';
	    if ((this.mode & READ) == 0)
		throw new NonReadableChannelException();
	}
	else if (mode == MapMode.READ_WRITE || mode == MapMode.PRIVATE)
	{
	    nmode = mode == MapMode.READ_WRITE ? '+' : 'c';
	    if ((this.mode & (READ|WRITE)) != (READ|WRITE))
		throw new NonWritableChannelException();
	}
	else
	    throw new IllegalArgumentException ();
    
	if (position < 0 || size < 0 || size > Integer.MAX_VALUE)
	    throw new IllegalArgumentException ();
	return mapImpl(nmode, position, (int) size);
    }

    /**
     * msync with the disk
     */
    public void force (boolean metaData) throws IOException
    {
	if (!isOpen ())
	    throw new ClosedChannelException ();

	if (!stream.get_CanWrite())
	    return;

	try
	{
	    if (false) throw new cli.System.IO.IOException();
	    stream.Flush();
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}

	if (stream instanceof cli.System.IO.FileStream)
	{
	    if(!flush(((cli.System.IO.FileStream)stream)))
	    {
		throw new IOException();
	    }
	}
    }

    private static native boolean flush(cli.System.IO.FileStream fs);

    public long transferTo (long position, long count, WritableByteChannel target)
	throws IOException
    {
	if (position < 0
	    || count < 0)
	    throw new IllegalArgumentException ();

	if (!isOpen ())
	    throw new ClosedChannelException ();

	if ((mode & READ) == 0)
	    throw new NonReadableChannelException ();
   
	// XXX: count needs to be casted from long to int. Dataloss ?
	ByteBuffer buffer = ByteBuffer.allocate ((int) count);
	read (buffer, position);
	buffer.flip();
	return target.write (buffer);
    }

    public long transferFrom (ReadableByteChannel src, long position, long count)
	throws IOException
    {
	if (position < 0
	    || count < 0)
	    throw new IllegalArgumentException ();

	if (!isOpen ())
	    throw new ClosedChannelException ();

	if ((mode & WRITE) == 0)
	    throw new NonWritableChannelException ();

	// XXX: count needs to be casted from long to int. Dataloss ?
	ByteBuffer buffer = ByteBuffer.allocate ((int) count);
	src.read (buffer);
	buffer.flip();
	return write (buffer, position);
    }

    public FileLock tryLock (long position, long size, boolean shared)
	throws IOException
    {
	if (position < 0
	    || size < 0)
	    throw new IllegalArgumentException ();

	if (!isOpen ())
	    throw new ClosedChannelException ();

	if (shared && (mode & READ) == 0)
	    throw new NonReadableChannelException ();
	
	if (!shared && (mode & WRITE) == 0)
	    throw new NonWritableChannelException ();
	
	boolean completed = false;
    
	try
	{
	    begin();
	    lock(position, size, shared, true);
	    completed = true;
	    return new FileLockImpl(this, position, size, shared);
	}
	finally
	{
	    end(completed);
	}
    }

    /** Try to acquire a lock at the given position and size.
     * On success return true.
     * If wait as specified, block until we can get it.
     * Otherwise return false.
     */
    private boolean lock(long position, long size, boolean shared, boolean wait) throws IOException
    {
	if(stream == null)
	    throw new IOException("Invalid FileChannelImpl");

	try
	{
	    if(false) throw new cli.System.IO.IOException();
	    if(false) throw new cli.System.ArgumentOutOfRangeException();
	    // TODO if wait is false, we shouldn't block...
	    ((FileStream)stream).Lock(position, size);
	    return true;
	}
	catch(ClassCastException c)
	{
	    throw new IOException("Locking not supported");
	}
	catch(cli.System.ArgumentOutOfRangeException x1)
	{
	    throw new IOException(x1.getMessage());
	}
	catch(cli.System.IO.IOException x)
	{
	    throw new IOException(x.getMessage());
	}
	// TODO map al the other exceptions as well...	
    }
  
    public FileLock lock (long position, long size, boolean shared)
	throws IOException
    {
	if (position < 0
	    || size < 0)
	    throw new IllegalArgumentException ();

	if (!isOpen ())
	    throw new ClosedChannelException ();

	boolean completed = false;

	try
	{
	    boolean lockable = lock(position, size, shared, false);
	    completed = true;
	    return (lockable
		? new FileLockImpl(this, position, size, shared)
		: null);
	}
	finally
	{
	    end(completed);
	}
    }

    public long position ()
	throws IOException
    {
	if (!isOpen ())
	    throw new ClosedChannelException ();

	return implPosition ();
    }
  
    public FileChannel position (long newPosition)
	throws IOException
    {
	if (newPosition < 0)
	    throw new IllegalArgumentException ();

	if (!isOpen ())
	    throw new ClosedChannelException ();

	// FIXME note semantics if seeking beyond eof.
	// We should seek lazily - only on a write.
	seek (newPosition);
	return this;
    }
  
    public FileChannel truncate (long size)
	throws IOException
    {
	if (size < 0)
	    throw new IllegalArgumentException ();

	if (!isOpen ())
	    throw new ClosedChannelException ();

	if ((mode & WRITE) == 0)
	    throw new NonWritableChannelException ();

	implTruncate (size);
	return this;
    }
}
