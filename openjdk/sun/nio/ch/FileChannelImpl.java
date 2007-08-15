/* FileChannelImpl.java -- 
   Copyright (C) 2002, 2004, 2005, 2006 Free Software Foundation, Inc.

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


package sun.nio.ch;

import gnu.classpath.Configuration;

import java.io.File;
import java.io.FileDescriptor;
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
import cli.System.IntPtr;
import cli.System.Type;
import cli.System.Reflection.MethodInfo;
import cli.System.Reflection.ParameterModifier;
import cli.System.Reflection.BindingFlags;
import cli.System.Runtime.InteropServices.DllImportAttribute;

@ikvm.lang.Internal
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

    private static final boolean win32 = runningOnWindows();

    private volatile Stream stream;
    private int mode;

    private static boolean runningOnWindows()
    {
        cli.System.OperatingSystem os = cli.System.Environment.get_OSVersion();
        int platform = os.get_Platform().Value;
        return platform == cli.System.PlatformID.Win32NT || 
            platform == cli.System.PlatformID.Win32Windows;
    }

    void release(FileLockImpl lock)
    {
	// TODO
    }

    public static java.nio.channels.FileChannel open(FileDescriptor fd, boolean readable, boolean writable, Object parent)
    {
	return create(fd.getStream());
    }

    public static java.nio.channels.FileChannel open(FileDescriptor fd, boolean readable, boolean writable, Object parent, boolean append)
    {
	return create(fd.getStream());
    }

    private static IntPtr mapViewOfFile(FileStream fs, boolean writeable, boolean copy_on_write, long position, int size) throws IOException
    {
        if (win32)
            return mapViewOfFileWin32(fs, writeable, copy_on_write, position, size);
        else
            return mapViewOfFilePosix(fs, writeable, copy_on_write, position, size);
    }

    static void unmapViewOfFile(IntPtr ptr, int size)
    {
        if (win32)
            UnmapViewOfFile(ptr);
        else
            ikvm_munmap(ptr, size);
    }

    public static void flushViewOfFile(IntPtr ptr, int size)
    {
        if (win32)
            FlushViewOfFile(ptr, IntPtr.Zero);
        else
            ikvm_msync(ptr, size);
    }

    private static IntPtr mapViewOfFileWin32(FileStream fs, boolean writeable, boolean copy_on_write, long position, int size) throws IOException
    {
        try
        {
            int PAGE_READONLY = 2;
            int PAGE_READWRITE = 4;
            int PAGE_WRITECOPY = 8;
            IntPtr hFileMapping = CreateFileMapping(fs.get_Handle(), IntPtr.Zero, 
                copy_on_write ? PAGE_WRITECOPY : (writeable ? PAGE_READWRITE : PAGE_READONLY),
                0, size, null);
            int err = cli.System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            if(hFileMapping.Equals(IntPtr.Zero))
            {
                throw new IOException("Win32 error " + err);
            }
            int FILE_MAP_WRITE = 2;
            int FILE_MAP_READ = 4;
            int FILE_MAP_COPY = 1;
            IntPtr p = MapViewOfFile(hFileMapping,
                copy_on_write ? FILE_MAP_COPY : (writeable ? FILE_MAP_WRITE : FILE_MAP_READ),
                (int)(position >> 32), (int)position, new IntPtr(size));
            err = cli.System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            CloseHandle(hFileMapping);
            if(p.Equals(IntPtr.Zero))
            {
                throw new IOException("Win32 error " + err);
            }
            return p;                
        }
        finally
        {
            cli.System.GC.KeepAlive(fs);
        }
    }
        
    private static IntPtr mapViewOfFilePosix(FileStream fs, boolean writeable, boolean copy_on_write, long position, int size) throws IOException
    {
        IntPtr p = ikvm_mmap(fs.get_Handle(), (byte)(writeable ? 1 : 0), (byte)(copy_on_write ? 1 : 0), position, size);
        cli.System.GC.KeepAlive(fs);
        // HACK ikvm_mmap should really be changed to return a null pointer on failure,
        // instead of whatever MAP_FAILED is defined to on the particular system we're running on,
        // common values for MAP_FAILED are 0 and -1, so we test for these.
        if(p.Equals(IntPtr.Zero) || p.Equals(new IntPtr(-1)))
        {
            throw new IOException("file mapping failed");
        }
        return p;
    }

    private static boolean flush(FileStream fs)
    {
        if (win32)
            return flushWin32(fs);
        else
            return flushPosix(fs);
    }

    private static boolean flushWin32(FileStream fs)
    {
        int rc = FlushFileBuffers(fs.get_Handle());
        cli.System.GC.KeepAlive(fs);
        return rc != 0;
    }

    private static boolean flushPosix(FileStream fs)
    {
        Type t = Type.GetType("Mono.Posix.Syscall, Mono.Posix");
        if(t != null)
        {
            BindingFlags flags = BindingFlags.wrap(BindingFlags.Public | BindingFlags.Static);
            MethodInfo mono_1_1_Flush = t.GetMethod("fsync", flags, null, new Type[] { Type.GetType("System.Int32") }, new ParameterModifier[0]);
            if(mono_1_1_Flush != null)
            {
                Object[] args = new Object[] { ikvm.lang.CIL.box_int(fs.get_Handle().ToInt32()) };
                return ikvm.lang.CIL.unbox_int(mono_1_1_Flush.Invoke(null, args)) == 0;
            }
        }
        return true;
    }

    @DllImportAttribute.Annotation("kernel32")
    private static native int FlushFileBuffers(IntPtr handle);

    @DllImportAttribute.Annotation("kernel32")
    private static native int CloseHandle(IntPtr handle);

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpAttributes, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, String lpName);

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native IntPtr MapViewOfFile(IntPtr hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);

    @DllImportAttribute.Annotation("kernel32")
    private static native int FlushViewOfFile(IntPtr lpBaseAddress, IntPtr dwNumberOfBytesToFlush);

    @DllImportAttribute.Annotation("kernel32")
    private static native int UnmapViewOfFile(IntPtr lpBaseAddress);

    @DllImportAttribute.Annotation("ikvm-native")
    private static native int ikvm_munmap(IntPtr address, int size);

    @DllImportAttribute.Annotation("ikvm-native")
    private static native int ikvm_msync(IntPtr address, int size);

    @DllImportAttribute.Annotation("ikvm-native")
    private static native IntPtr ikvm_mmap(IntPtr handle, byte writeable, byte copy_on_write, long position, int size);

    /* Open a file.  MODE is a combination of the above mode flags. */
    public static FileChannelImpl create(File file, int mode)
        throws FileNotFoundException
    {
        return new FileChannelImpl(file, mode);
    }

    FileChannelImpl(File file, int mode)
        throws FileNotFoundException
    {
	stream = open(file.getPath(), mode);
	this.mode = mode;
    }

    public static FileChannelImpl create(Stream stream)
    {
        return new FileChannelImpl(stream);
    }

    private FileChannelImpl(Stream stream)
    {
	this.stream = stream;
	mode = (stream.get_CanRead() ? READ : 0) | (stream.get_CanWrite() ? WRITE : 0);
    }

    public static final FileChannelImpl in;
    public static final FileChannelImpl out;
    public static final FileChannelImpl err;

    static
    {
        FileChannelImpl _in;
        FileChannelImpl _out;
        FileChannelImpl _err;
        try
        {
            _in = FileChannelImpl.create(getStandardStream(0));
            _out = FileChannelImpl.create(getStandardStream(1));
            _err = FileChannelImpl.create(getStandardStream(2));
        }
        catch(cli.System.MissingMethodException _)
        {
            _in = FileChannelImpl.create(Stream.Null);
            _out = FileChannelImpl.create(Stream.Null);
            _err = FileChannelImpl.create(Stream.Null);
        }
        in = _in;
        out = _out;
        err = _err;
    }

    private static Stream getStandardStream(int id)
        throws cli.System.MissingMethodException
    {
        switch(id)
        {
            case 0:
                return Console.OpenStandardInput();
            case 1:
                return Console.OpenStandardOutput();
            case 2:
                return Console.OpenStandardError();
            default:
                throw new Error();
        }
    }

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
	    return new FileStream(demanglePath(path), FileMode.wrap(fileMode), FileAccess.wrap(fileAccess), FileShare.wrap(FileShare.ReadWrite), 1, false);
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
            Stream local = stream;
            stream = null;
	    try
	    {
		local.Close();
                if(false) throw new cli.System.IO.IOException();
                if(false) throw new cli.System.NotSupportedException();
	    }
            catch(cli.System.NotSupportedException _)
            {
                // FXBUG ignore this, there's a bug in System.IO.__ConsoleStream,
                // it throws this exception when you try to close it.
            }
            catch(cli.System.IO.IOException x)
	    {
		throw new IOException(x.getMessage());
	    }
	}
    }

    public int read(ByteBuffer dst) throws IOException
    {
        if (dst.hasArray())
        {
            byte[] buffer = dst.array();
            int result = read(buffer, dst.arrayOffset() + dst.position(), dst.remaining());
            if (result > 0)
            {
                dst.position(dst.position() + result);
            }
            return result;
        }
        else
        {
	    byte[] buffer = new byte[dst.remaining()];
	    int result = read(buffer, 0, buffer.length);
	    if (result > 0)
            {
	        dst.put(buffer, 0, result);
            }
	    return result;
        }
    }

    public int read(ByteBuffer dst, long position) throws IOException
    {
	if (position < 0)
            throw new IllegalArgumentException ("position: " + position);

        long oldPosition = implPosition();
	seek(position);
        try
        {
	    return read(dst);
        }
        finally
        {
	    seek(oldPosition);
        }
    }

    public int read() throws IOException
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

    public long read(ByteBuffer[] dsts, int offset, int length) throws IOException
    {
	long result = 0;

	for (int i = offset; i < offset + length; i++)
	{
	    result += read(dsts [i]);
	}

	return result;
    }

    public int write(ByteBuffer src) throws IOException
    {
	int len = src.remaining();
	if (src.hasArray() && !src.isReadOnly())
	{
	    byte[] buffer = src.array();
	    write(buffer, src.arrayOffset() + src.position(), len);
            src.position(src.position() + len);
	}
	else
	{
	    // Use a more efficient native method! FIXME!
	    byte[] buffer = new byte[len];
	    src.get(buffer, 0, len);
	    write(buffer, 0, len);
	}
	return len;
    }
    
    public int write(ByteBuffer src, long position) throws IOException
    {
	if (position < 0)
            throw new IllegalArgumentException ("position: " + position);

	if (!isOpen ())
	    throw new ClosedChannelException ();
    
	if ((mode & WRITE) == 0)
	    throw new NonWritableChannelException ();

	long oldPosition = implPosition();
	seek(position);
        try
        {
	    return write(src);
        }
        finally
        {
	    seek(oldPosition);
        }
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
				   
    private MappedByteBuffer mapImpl (char mode, long position, final int size) throws IOException
    {
        if (! (stream instanceof FileStream))
            throw new IllegalArgumentException("only file streams can be mapped");

        final IntPtr ptr = mapViewOfFile((FileStream)stream, mode != 'r', mode == 'c', position, size);
	Runnable um = new Runnable() {
	    public void run() {
		unmapViewOfFile(ptr, size);
	    }
	};
	return mode != 'r'
	    ? Util.newMappedByteBuffer(size, ptr.ToInt64(), um)
	    : Util.newMappedByteBufferR(size, ptr.ToInt64(), um);
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
            if ((this.mode & READ) == 0)
                throw new NonReadableChannelException();
            if ((this.mode & WRITE) == 0)
                throw new NonWritableChannelException();
	}
	else
            throw new IllegalArgumentException ("mode: " + mode);
    
	if (position < 0 || size < 0 || size > Integer.MAX_VALUE)
            throw new IllegalArgumentException ("position: " + position
                                                + ", size: " + size);
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

	if (stream instanceof FileStream)
	{
	    if(!flush(((FileStream)stream)))
	    {
		throw new IOException();
	    }
	}
    }

    public long transferTo(long position, long count, WritableByteChannel target) throws IOException
    {
        if (position < 0 || count < 0)
            throw new IllegalArgumentException ("position: " + position + ", count: " + count);

        if (!isOpen())
            throw new ClosedChannelException ();

        if ((mode & READ) == 0)
            throw new NonReadableChannelException ();
   
        long total = 0;
        ByteBuffer buf = ByteBuffer.allocate((int)Math.min(4096, count));

        while (count > 0)
        {
            buf.clear();
            buf.limit((int)Math.min(buf.capacity(), count));
            int bytesRead = read(buf, position);
            if (bytesRead <= 0)
            {
                break;
            }
            buf.flip();
            int bytesWritten = target.write(buf);
            total += bytesWritten;
            position += bytesWritten;
            count -= bytesWritten;
            if (bytesWritten != bytesRead)
            {
                break;
            }
        }

        return total;
    }

    public long transferFrom(ReadableByteChannel src, long position, long count) throws IOException
    {
        if (position < 0 || count < 0)
            throw new IllegalArgumentException ("position: " + position + ", count: " + count);

        if (!isOpen())
            throw new ClosedChannelException ();

        if ((mode & WRITE) == 0)
            throw new NonWritableChannelException ();

        long total = 0;
        ByteBuffer buf = ByteBuffer.allocate((int)Math.min(4096, count));

        while (count > 0)
        {
            buf.clear();
            buf.limit((int)Math.min(buf.capacity(), count));
            long transferred = src.read(buf);
            if (transferred <= 0)
            {
                break;
            }
            buf.flip();
            write(buf, position);
            total += transferred;
            position += transferred;
            count -= transferred;
        }

        return total;
    }

    // Shared sanity checks between lock and tryLock methods.
    private void lockCheck(long position, long size, boolean shared)
        throws IOException
    {
        if (position < 0
            || size < 0)
            throw new IllegalArgumentException ("position: " + position
                + ", size: " + size);

        if (!isOpen ())
            throw new ClosedChannelException();

        if (shared && ((mode & READ) == 0))
            throw new NonReadableChannelException();
	
        if (!shared && ((mode & WRITE) == 0))
            throw new NonWritableChannelException();
    }

    public FileLock tryLock (long position, long size, boolean shared)
	throws IOException
    {
        lockCheck(position, size, shared);
	
	boolean completed = false;
    
	try
	{
	    begin();
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
	    if(false) throw new cli.System.ArgumentOutOfRangeException();
            int backoff = 5;
            for(;;)
            {
                try
                {
                    if(false) throw new cli.System.IO.IOException();
                    ((FileStream)stream).Lock(position, size);
                    return true;
                }
                catch(cli.System.IO.IOException x)
                {
                    if(!wait)
                    {
                        return false;
                    }
                    try
                    {
                        Thread.sleep(backoff);
                        backoff *= 2;
                        if(backoff > 1000)
                        {
                            backoff = 40;
                        }
                    }
                    catch(InterruptedException _)
                    {
                        // SPECNOTE The API spec says that an interrupt lock
                        // throws a FileLockInterruptedException, but in
                        // reality Sun returns null from lock (at least on Windows).
                        return false;
                    }
                }
            }
	}
	catch(ClassCastException c)
	{
	    throw new IOException("Locking not supported");
	}
	catch(cli.System.ArgumentOutOfRangeException x1)
	{
	    throw new IOException(x1.getMessage());
	}
	// TODO map al the other exceptions as well...	
    }
  
    public FileLock lock (long position, long size, boolean shared)
	throws IOException
    {
        lockCheck(position, size, shared);

	boolean completed = false;

	try
	{
	    boolean lockable = lock(position, size, shared, true);
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
            throw new IllegalArgumentException ("newPostition: " + newPosition);

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
            throw new IllegalArgumentException ("size: " + size);

	if (!isOpen ())
	    throw new ClosedChannelException ();

	if ((mode & WRITE) == 0)
	    throw new NonWritableChannelException ();

        if (size < size ())
            implTruncate (size);
	return this;
    }

    public Stream getStream()
    {
	return stream;
    }
}
