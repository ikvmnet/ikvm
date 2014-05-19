/*
 * Copyright (c) 2003, 2011, Oracle and/or its affiliates. All rights reserved.
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

package java.io;

import cli.System.IO.FileAccess;
import cli.System.IO.FileMode;
import cli.System.IO.FileShare;
import cli.System.IO.FileStream;
import cli.System.IO.SeekOrigin;
import cli.System.Runtime.InteropServices.DllImportAttribute;
import java.util.ArrayList;
import java.util.List;

/**
 * Instances of the file descriptor class serve as an opaque handle
 * to the underlying machine-specific structure representing an
 * open file, an open socket, or another source or sink of bytes.
 * The main practical use for a file descriptor is to create a
 * {@link FileInputStream} or {@link FileOutputStream} to contain it.
 *
 * <p>Applications should not create their own file descriptors.
 *
 * @author  Pavani Diwanji
 * @since   JDK1.0
 */
public final class FileDescriptor {

    private volatile cli.System.IO.Stream stream;
    private volatile cli.System.Net.Sockets.Socket socket;
    private volatile boolean nonBlockingSocket;
    private volatile cli.System.IAsyncResult asyncResult;

    /**
     * HACK
     *   JRuby uses reflection to get at the handle (on Windows) and fd (on non-Windows)
     *   fields to convert it into a native handle and query if it is a tty.
     */
    @ikvm.lang.Property(get = "get_fd")
    private int fd;

    @ikvm.lang.Property(get = "get_handle")
    private long handle;

    private Closeable parent;
    private List<Closeable> otherParents;
    private boolean closed;
    
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private long get_handle()
    {
        if (ikvm.internal.Util.WINDOWS)
        {
            if (stream instanceof cli.System.IO.FileStream)
            {
                cli.System.IO.FileStream fs = (cli.System.IO.FileStream)stream;
                return fs.get_Handle().ToInt64();
            }
            else if (this == in)
            {
                return GetStdHandle(-10).ToInt64();
            }
            else if (this == out)
            {
                return GetStdHandle(-11).ToInt64();
            }
            else if (this == err)
            {
                return GetStdHandle(-12).ToInt64();
            }
        }
        return -1;
    }
    
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private int get_fd()
    {
        if (!ikvm.internal.Util.WINDOWS)
        {
            if (stream instanceof cli.System.IO.FileStream)
            {
                cli.System.IO.FileStream fs = (cli.System.IO.FileStream)stream;
                return fs.get_Handle().ToInt32();
            }
            else if (this == in)
            {
                return 0;
            }
            else if (this == out)
            {
                return 1;
            }
            else if (this == err)
            {
                return 2;
            }
        }
        return -1;
    }
    
    @DllImportAttribute.Annotation("kernel32")
    private static native cli.System.IntPtr GetStdHandle(int nStdHandle);

    /**
     * Constructs an (invalid) FileDescriptor
     * object.
     */
    public /**/ FileDescriptor() {
    }

    /**
     * A handle to the standard input stream. Usually, this file
     * descriptor is not used directly, but rather via the input stream
     * known as {@code System.in}.
     *
     * @see     java.lang.System#in
     */
    public static final FileDescriptor in = standardStream(0);

    /**
     * A handle to the standard output stream. Usually, this file
     * descriptor is not used directly, but rather via the output stream
     * known as {@code System.out}.
     * @see     java.lang.System#out
     */
    public static final FileDescriptor out = standardStream(1);

    /**
     * A handle to the standard error stream. Usually, this file
     * descriptor is not used directly, but rather via the output stream
     * known as {@code System.err}.
     *
     * @see     java.lang.System#err
     */
    public static final FileDescriptor err = standardStream(2);

    /**
     * Tests if this file descriptor object is valid.
     *
     * @return  {@code true} if the file descriptor object represents a
     *          valid, open file, socket, or other active I/O connection;
     *          {@code false} otherwise.
     */
    public boolean valid() {
        return stream != null || socket != null;
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
            boolean ok = ikvm.internal.Util.WINDOWS ? flushWin32(fs) : flushPosix(fs);
            if (!ok)
            {
                throw new SyncFailedException("sync failed");
            }
        }
    }

    private static native boolean flushPosix(FileStream fs);

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
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

    /*
     * Package private methods to track referents.
     * If multiple streams point to the same FileDescriptor, we cycle
     * through the list of all referents and call close()
     */

    /**
     * Attach a Closeable to this FD for tracking.
     * parent reference is added to otherParents when
     * needed to make closeAll simpler.
     */
    synchronized void attach(Closeable c) {
        if (parent == null) {
            // first caller gets to do this
            parent = c;
        } else if (otherParents == null) {
            otherParents = new ArrayList<>();
            otherParents.add(parent);
            otherParents.add(c);
        } else {
            otherParents.add(c);
        }
    }

    /**
     * Cycle through all Closeables sharing this FD and call
     * close() on each one.
     *
     * The caller closeable gets to call close0().
     */
    @SuppressWarnings("try")
    synchronized void closeAll(Closeable releaser) throws IOException {
        if (!closed) {
            closed = true;
            IOException ioe = null;
            try (Closeable c = releaser) {
                if (otherParents != null) {
                    for (Closeable referent : otherParents) {
                        try {
                            referent.close();
                        } catch(IOException x) {
                            if (ioe == null) {
                                ioe = x;
                            } else {
                                ioe.addSuppressed(x);
                            }
                        }
                    }
                }
            } catch(IOException ex) {
                /*
                 * If releaser close() throws IOException
                 * add other exceptions as suppressed.
                 */
                if (ioe != null)
                    ex.addSuppressed(ioe);
                ioe = ex;
            } finally {
                if (ioe != null)
                    throw ioe;
            }
        }
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
            // or by name being a read-only file
            throw new FileNotFoundException(name + " (Access is denied)");
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
        // NOTE we start by dereferencing buf, to make sure you get a NullPointerException first if you pass a null reference.
        int bufLen = buf.length;
        if ((offset < 0) || (offset > bufLen) || (len < 0) || (len > (bufLen - offset)))
        {
            throw new IndexOutOfBoundsException();
        }

        if (len == 0)
        {
            return 0;
        }

        checkOpen();

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
        // NOTE we start by dereferencing buf, to make sure you get a NullPointerException first if you pass a null reference.
        int bufLen = buf.length;
        if ((offset < 0) || (offset > bufLen) || (len < 0) || (len > (bufLen - offset)))
        {
            throw new IndexOutOfBoundsException();
        }

        if (len == 0)
        {
            return;
        }

        checkOpen();

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
            if (false) throw new cli.System.ArgumentOutOfRangeException();
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
        catch (cli.System.ArgumentOutOfRangeException _)
        {
            throw new IOException("Negative seek offset");
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

    @ikvm.lang.Internal
    public void setSocketBlocking(boolean blocking) throws IOException
    {
        this.nonBlockingSocket = !blocking;
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            socket.set_Blocking(blocking);
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            if (x.get_ErrorCode() == java.net.SocketUtil.WSAEINVAL)
            {
                // Work around for winsock issue. You can't set a socket to blocking if a connection request is pending,
                // so we'll have to set the blocking again in SocketChannelImpl.checkConnect().
                return;
            }
            throw java.net.SocketUtil.convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException _)
        {
            throw new java.net.SocketException("Socket is closed");
        }
    }
    
    @ikvm.lang.Internal
    public boolean isSocketBlocking()
    {
        return !nonBlockingSocket;
    }

    @ikvm.lang.Internal
    public cli.System.IAsyncResult getAsyncResult()
    {
        return asyncResult;
    }

    @ikvm.lang.Internal
    public void setAsyncResult(cli.System.IAsyncResult asyncResult)
    {
        this.asyncResult = asyncResult;
    }
}
