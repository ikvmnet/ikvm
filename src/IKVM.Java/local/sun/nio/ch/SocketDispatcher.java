/*
 * Copyright (c) 2000, 2013, Oracle and/or its affiliates. All rights reserved.
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

import java.io.*;
import java.net.SocketException;
import java.net.SocketUtil;
import java.nio.ByteBuffer;

import cli.System.Net.Sockets.SocketError;
import cli.System.Net.Sockets.SocketFlags;

/**
 * Allows different platforms to call different native methods
 * for read and write operations.
 */
class SocketDispatcher extends NativeDispatcher
{

    int read(FileDescriptor fd, byte[] buf, int offset, int length) throws IOException
    {
        if (length == 0)
            return 0;

        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            int read = fd.getSocket().Receive(buf, offset, length, SocketFlags.wrap(SocketFlags.None));
            return read == 0 ? IOStatus.EOF : read;
        }
        catch (cli.System.Net.Sockets.SocketException e)
        {
            switch (e.get_SocketErrorCode().Value)
            {
                case SocketError.Shutdown:
                    // the socket was shutdown, so we have to return EOF
                    return IOStatus.EOF;
                case SocketError.WouldBlock:
                    // nothing to read and would block
                    return IOStatus.UNAVAILABLE;
                default:
                    throw SocketUtil.convertSocketExceptionToIOException(e);
            }
        }
        catch (cli.System.ObjectDisposedException e)
        {
            throw new SocketException("Socket is closed.");
        }
    }

    int write(FileDescriptor fd, byte[] buf, int offset, int length) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return fd.getSocket().Send(buf, offset, length, SocketFlags.wrap(SocketFlags.None));
        }
        catch (cli.System.Net.Sockets.SocketException e)
        {
            switch (e.get_SocketErrorCode().Value)
            {
                case SocketError.WouldBlock:
                    return IOStatus.UNAVAILABLE;
                default:
                    throw SocketUtil.convertSocketExceptionToIOException(e);
            }
        }
        catch (cli.System.ObjectDisposedException e)
        {
            throw new SocketException("Socket is closed.");
        }
    }

    native long read(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length) throws IOException;

    native long write(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length) throws IOException;

    void close(FileDescriptor fd) throws IOException
    {

    }

    void preClose(FileDescriptor fd) throws IOException
    {
        closeImpl(fd);
    }

    static void closeImpl(FileDescriptor fd) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            fd.getSocket().Close();
        }
        catch (cli.System.Net.Sockets.SocketException e)
        {
            throw java.net.SocketUtil.convertSocketExceptionToIOException(e);
        }
        catch (cli.System.ObjectDisposedException e)
        {
            throw new java.net.SocketException("Socket is closed");
        }

    }

}
