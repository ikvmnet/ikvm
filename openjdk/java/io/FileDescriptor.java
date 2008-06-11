/*
 * Copyright 2003-2006 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
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
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

package java.io;

import cli.System.IO.FileAccess;
import cli.System.IO.FileMode;
import cli.System.IO.FileShare;
import cli.System.IO.FileStream;
import cli.System.IO.SeekOrigin;
import cli.System.Runtime.InteropServices.DllImportAttribute;
import java.util.concurrent.atomic.AtomicInteger;

/**
 * Instances of the file descriptor class serve as an opaque handle
 * to the underlying machine-specific structure representing an open
 * file, an open socket, or another source or sink of bytes. The
 * main practical use for a file descriptor is to create a
 * <code>FileInputStream</code> or <code>FileOutputStream</code> to
 * contain it.
 * <p>
 * Applications should not create their own file descriptors.
 *
 * @author  Pavani Diwanji
 * @version 1.10, 05/05/07
 * @see     java.io.FileInputStream
 * @see     java.io.FileOutputStream
 * @since   JDK1.0
 */
public final class FileDescriptor {
 
    private volatile cli.System.IO.Stream stream;
    private volatile cli.System.Net.Sockets.Socket socket;

    /**
     * A use counter for tracking the FIS/FOS/RAF instances that
     * use this FileDescriptor. The FIS/FOS.finalize() will not release
     * the FileDescriptor if it is still under use by any stream.
     */
    private AtomicInteger useCount;


    /**
     * Constructs an (invalid) FileDescriptor
     * object.
     */
    public /**/ FileDescriptor() {
        useCount = new AtomicInteger(1);
    }

    /**
     * A handle to the standard input stream. Usually, this file
     * descriptor is not used directly, but rather via the input stream
     * known as <code>System.in</code>.
     *
     * @see     java.lang.System#in
     */
    public static final FileDescriptor in = standardStream(0);

    /**
     * A handle to the standard output stream. Usually, this file
     * descriptor is not used directly, but rather via the output stream
     * known as <code>System.out</code>.
     * @see     java.lang.System#out
     */
    public static final FileDescriptor out = standardStream(1);

    /**
     * A handle to the standard error stream. Usually, this file
     * descriptor is not used directly, but rather via the output stream
     * known as <code>System.err</code>.
     *
     * @see     java.lang.System#err
     */
    public static final FileDescriptor err = standardStream(2);

    /**
     * Tests if this file descriptor object is valid.
     *
     * @return  <code>true</code> if the file descriptor object represents a
     *          valid, open file, socket, or other active I/O connection;
     *          <code>false</code> otherwise.
     */
    public boolean valid() {
        return stream != null;
    }

    /**
     * Force all system buffers to synchronize with the underlying
     * device.  This method returns after all modified data and
     * attributes of this FileDescriptor have been written to the
     * relevant device(s).  In particular, if this FileDescriptor
     * refers to a physical storage medium, such as a file in a file
     * system, sync will not return until all in-memory modified copies
     * of buffers associated with this FileDesecriptor have been
     * written to the physical medium.
     *
     * sync is meant to be used by code that requires physical
     * storage (such as a file) to be in a known state  For
     * example, a class that provided a simple transaction facility
     * might use sync to ensure that all changes to a file caused
     * by a given transaction were recorded on a storage medium.
     *
     * sync only affects buffers downstream of this FileDescriptor.  If
     * any in-memory buffering is being done by the application (for
     * example, by a BufferedOutputStream object), those buffers must
     * be flushed into the FileDescriptor (for example, by invoking
     * OutputStream.flush) before that data will be affected by sync.
     *
     * @exception SyncFailedException
     *        Thrown when the buffers cannot be flushed,
     *        or because the system cannot guarantee that all the
     *        buffers have been synchronized with physical media.
     * @since     JDK1.1
     */
    public void sync() throws SyncFailedException
    {
        if (stream == null)
        {
            throw new SyncFailedException("sync failed");
        }

        if (!stream.get_CanWrite())
        {
            return;
        }

        try
        {
            if (false) throw new cli.System.IO.IOException();
            stream.Flush();
        }
        catch (cli.System.IO.IOException x)
        {
            throw new SyncFailedException(x.getMessage());
        }

        if (stream instanceof FileStream)
        {
            FileStream fs = (FileStream)stream;
            boolean ok = ikvm.internal.Util.WINDOWS ? flushWin32(fs) : ikvm.internal.MonoUtils.fsync(fs);
            if (!ok)
            {
                throw new SyncFailedException("sync failed");
            }
        }
    }

    private static boolean flushWin32(FileStream fs)
    {
        return FlushFileBuffers(fs.get_SafeFileHandle()) != 0;
    }

    @DllImportAttribute.Annotation("kernel32")
    private static native int FlushFileBuffers(cli.Microsoft.Win32.SafeHandles.SafeFileHandle handle);

    private static FileDescriptor standardStream(int fd) {
        FileDescriptor desc = new FileDescriptor();
        try
        {
            desc.stream = getStandardStream(fd);
        }
        catch (cli.System.MissingMethodException _)
        {
            desc.stream = cli.System.IO.Stream.Null;
        }
        return desc;
    }

    private static cli.System.IO.Stream getStandardStream(int fd) throws cli.System.MissingMethodException
    {
        switch (fd)
        {
            case 0:
                return cli.System.Console.OpenStandardInput();
            case 1:
                return cli.System.Console.OpenStandardOutput();
            case 2:
                return cli.System.Console.OpenStandardError();
            default:
                throw new Error();
        }
    }

    // package private methods used by FIS, FOS and RAF. 

    int incrementAndGetUseCount() {
        return useCount.incrementAndGet();
    }
 
    int decrementAndGetUseCount() {
        return useCount.decrementAndGet();
    }

    void openReadOnly(String name) throws FileNotFoundException
    {
        open(name, FileMode.Open, FileAccess.Read);
    }

    void openWriteOnly(String name) throws FileNotFoundException
    {
        open(name, FileMode.Create, FileAccess.Write);
    }

    void openReadWrite(String name) throws FileNotFoundException
    {
        open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    void openAppend(String name) throws FileNotFoundException
    {
        open(name, FileMode.Append, FileAccess.Write);
    }

    private static native cli.System.IO.Stream open(String name, FileMode fileMode, FileAccess fileAccess)
        throws cli.System.IO.IOException,
            cli.System.Security.SecurityException,
            cli.System.UnauthorizedAccessException,
            cli.System.ArgumentException,
            cli.System.NotSupportedException;
    
    private void open(String name, int fileMode, int fileAccess) throws FileNotFoundException
    {
        try
        {
            stream = open(name, FileMode.wrap(fileMode), FileAccess.wrap(fileAccess));
        }
        catch (cli.System.Security.SecurityException x1)
        {
            throw new SecurityException(x1.getMessage());
        }
        catch (cli.System.IO.IOException x2)
        {
            throw new FileNotFoundException(x2.getMessage());
        }
        catch (cli.System.UnauthorizedAccessException x3)
        {
            // this is caused by "name" being a directory instead of a file
            throw new FileNotFoundException(x3.getMessage());
        }
        catch (cli.System.ArgumentException x4)
        {
            throw new FileNotFoundException(x4.getMessage());
        }
        catch (cli.System.NotSupportedException x5)
        {
            throw new FileNotFoundException(x5.getMessage());
        }
    }

    private void checkOpen() throws IOException
    {
        if (stream == null)
        {
            throw new IOException("Stream Closed");
        }
    }

    int read() throws IOException
    {
        checkOpen();
        try
        {
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            return stream.ReadByte();
        }
        catch (cli.System.NotSupportedException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public int readBytes(byte buf[], int offset, int len) throws IOException
    {
        checkOpen();

        if (len == 0)
        {
            return 0;
        }

        if ((offset < 0) || (offset > buf.length))
        {
            throw new IllegalArgumentException("Offset invalid: " + offset);
        }

        if ((len < 0) || (len > (buf.length - offset)))
        {
            throw new IllegalArgumentException("Length invalid: " + len);
        }

        try
        {
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            int count = stream.Read(buf, offset, len);
            if (count == 0)
            {
                count = -1;
            }
            return count;
        }
        catch (cli.System.NotSupportedException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    long skip(long n) throws IOException
    {
        checkOpen();
        if (!stream.get_CanSeek())
        {
            // in a somewhat bizar twist, for non-seekable streams the JDK throws an exception
            throw new IOException("The handle is invalid");
        }
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            long cur = stream.get_Position();
            long end = stream.Seek(n, SeekOrigin.wrap(SeekOrigin.Current));
            return end - cur;
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.NotSupportedException x1)
        {
            // this means we have a broken Stream, because if CanSeek returns true, it must
            // support Length and Position
            throw new IOException(x1);
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public int available() throws IOException
    {
        checkOpen();
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            if (stream.get_CanSeek())
            {
                return (int)Math.min(Integer.MAX_VALUE, Math.max(0, stream.get_Length() - stream.get_Position()));
            }
            return 0;
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.NotSupportedException x1)
        {
            // this means we have a broken Stream, because if CanSeek returns true, it must
            // support Length and Position
            throw new IOException(x1);
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    void write(int b) throws IOException
    {
        checkOpen();
        try
        {
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            stream.WriteByte((byte)b);
            // NOTE FileStream buffers the output, so we have to flush explicitly
            stream.Flush();
        }
        catch (cli.System.NotSupportedException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public void writeBytes(byte buf[], int offset, int len) throws IOException
    {
        checkOpen();
        if (len == 0)
        {
            return;
        }

        if ((offset < 0) || (offset > buf.length))
        {
            throw new IllegalArgumentException("Offset invalid: " + offset);
        }

        if ((len < 0) || (len > (buf.length - offset)))
        {
            throw new IllegalArgumentException("Length invalid: " + len);
        }

        try
        {
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            stream.Write(buf, offset, len);
            // NOTE FileStream buffers the output, so we have to flush explicitly
            stream.Flush();
        }
        catch (cli.System.NotSupportedException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public long getFilePointer() throws IOException
    {
        checkOpen();
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            return stream.get_Position();
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.NotSupportedException x1)
        {
            throw new IOException(x1);
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public void seek(long newPosition) throws IOException
    {
        checkOpen();
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            stream.set_Position(newPosition);
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.NotSupportedException x1)
        {
            throw new IOException(x1);
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public long length() throws IOException
    {
        checkOpen();
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            return stream.get_Length();
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.NotSupportedException x1)
        {
            throw new IOException(x1);
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public void setLength(long newLength) throws IOException
    {
        checkOpen();
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            stream.SetLength(newLength);
        }
        catch (cli.System.IO.IOException x)
        {
            throw new IOException(x.getMessage());
        }
        catch (cli.System.NotSupportedException x1)
        {
            throw new IOException(x1);
        }
        catch (cli.System.ObjectDisposedException x)
        {
            throw new java.nio.channels.ClosedChannelException();
        }
    }

    @ikvm.lang.Internal
    public void close() throws IOException
    {
        cli.System.IO.Stream s = stream;
        stream = null;
        if (s != null)
        {
            s.Close();
        }
    }

    @ikvm.lang.Internal
    public cli.System.IO.Stream getStream()
    {
        return stream;
    }

    @ikvm.lang.Internal
    public static FileDescriptor fromStream(cli.System.IO.Stream stream)
    {
        FileDescriptor desc = new FileDescriptor();
        desc.stream = stream;
        return desc;
    }

    @ikvm.lang.Internal
    public cli.System.Net.Sockets.Socket getSocket()
    {
        return socket;
    }

    @ikvm.lang.Internal
    public void setSocket(cli.System.Net.Sockets.Socket socket)
    {
        this.socket = socket;
    }
}
