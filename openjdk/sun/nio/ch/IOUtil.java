/*
 * Copyright (c) 2000, 2002, Oracle and/or its affiliates. All rights reserved.
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

package sun.nio.ch;

import java.io.FileDescriptor;
import java.io.IOException;
import java.net.*;
import java.nio.ByteBuffer;
import java.nio.channels.*;
import java.nio.channels.spi.*;
import cli.System.Net.Sockets.SocketFlags;


/**
 * File-descriptor based I/O utilities that are shared by NIO classes.
 */

class IOUtil {

    private IOUtil() { }                // No instantiation

    static boolean randomBytes(byte[] someBytes)
    {
        try
        {
            if (false) throw new cli.System.Security.Cryptography.CryptographicException();
            cli.System.Security.Cryptography.RNGCryptoServiceProvider csp = new cli.System.Security.Cryptography.RNGCryptoServiceProvider();
            csp.GetBytes(someBytes);
            return true;
        }
        catch (cli.System.Security.Cryptography.CryptographicException _)
        {
            return false;
        }
    }

    static void configureBlocking(FileDescriptor fd, boolean blocking) throws IOException
    {
        fd.setSocketBlocking(blocking);
    }

    // this is a dummy method to allow us to use unmodified socket channel impls
    static int fdVal(FileDescriptor fd)
    {
        return 0xbadc0de;
    }

    static int read(FileDescriptor fd, ByteBuffer dst, long position,
                    NativeDispatcher nd, Object lock)
        throws IOException
    {
        if (dst.isReadOnly())
            throw new IllegalArgumentException("Read-only buffer");
        if (position != -1)
            throw new ikvm.internal.NotYetImplementedError();

        if (dst.hasArray())
        {
            byte[] buf = dst.array();
            int len = readImpl(fd, buf, dst.arrayOffset() + dst.position(), dst.remaining());
            if (len > 0)
            {
                dst.position(dst.position() + len);
            }
            return len;
        }
        else
        {
            byte[] buf = new byte[dst.remaining()];
            int len = readImpl(fd, buf, 0, buf.length);
            if (len > 0)
            {
                dst.put(buf, 0, len);
            }
            return len;
        }
    }

    private static int readImpl(FileDescriptor fd, byte[] buf, int offset, int length) throws IOException
    {
        if (length == 0)
        {
            return 0;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            int read = fd.getSocket().Receive(buf, offset, length, SocketFlags.wrap(SocketFlags.None));
            return read == 0 ? IOStatus.EOF : read;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            if (x.get_ErrorCode() == SocketUtil.WSAESHUTDOWN)
            {
                // the socket was shutdown, so we have to return EOF
                return IOStatus.EOF;
            }
            else if (x.get_ErrorCode() == SocketUtil.WSAEWOULDBLOCK)
            {
                // nothing to read and would block
                return IOStatus.UNAVAILABLE;
            }
            throw SocketUtil.convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    static native long read(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length, NativeDispatcher nd)
        throws IOException;

    static int write(FileDescriptor fd, ByteBuffer src, long position,
                     NativeDispatcher nd, Object lock)
        throws IOException
    {
        if (src.hasArray())
        {
            byte[] buf = src.array();
            int len = writeImpl(fd, buf, src.arrayOffset() + src.position(), src.remaining());
            if (len > 0)
            {
                src.position(src.position() + len);
            }
            return len;
        }
        else
        {
            int pos = src.position();
            byte[] buf = new byte[src.remaining()];
            src.get(buf);
            int len = writeImpl(fd, buf, 0, buf.length);
            if (len > 0)
            {
                src.position(pos + len);
            }
            return len;
        }
    }

    private static int writeImpl(FileDescriptor fd, byte[] buf, int offset, int length) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return fd.getSocket().Send(buf, offset, length, SocketFlags.wrap(SocketFlags.None));
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            if (x.get_ErrorCode() == SocketUtil.WSAEWOULDBLOCK)
            {
                return IOStatus.UNAVAILABLE;
            }
            throw SocketUtil.convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    static long write(FileDescriptor fd, ByteBuffer[] bufs, NativeDispatcher nd)
        throws IOException
    {
        return write(fd, bufs, 0, bufs.length, nd);
    }

    static native long write(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length, NativeDispatcher nd)
        throws IOException;
}
