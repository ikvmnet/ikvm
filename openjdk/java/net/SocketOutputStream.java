/*
 * Copyright (c) 1995, 2013, Oracle and/or its affiliates. All rights reserved.
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

package java.net;

import java.io.FileDescriptor;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.channels.FileChannel;
import static ikvm.internal.Winsock.*;
import static java.net.net_util_md.*;

/**
 * This stream extends FileOutputStream to implement a
 * SocketOutputStream. Note that this class should <b>NOT</b> be
 * public.
 *
 * @author      Jonathan Payne
 * @author      Arthur van Hoff
 */
class SocketOutputStream extends FileOutputStream
{
    private AbstractPlainSocketImpl impl = null;
    private byte temp[] = new byte[1];
    private Socket socket = null;

    /**
     * Creates a new SocketOutputStream. Can only be called
     * by a Socket. This method needs to hang on to the owner Socket so
     * that the fd will not be closed.
     * @param impl the socket output stream inplemented
     */
    SocketOutputStream(AbstractPlainSocketImpl impl) throws IOException {
        super(impl.getFileDescriptor());
        this.impl = impl;
        socket = impl.getSocket();
    }

    /**
     * Returns the unique {@link java.nio.channels.FileChannel FileChannel}
     * object associated with this file output stream. </p>
     *
     * The {@code getChannel} method of {@code SocketOutputStream}
     * returns {@code null} since it is a socket based stream.</p>
     *
     * @return  the file channel associated with this file output stream
     *
     * @since 1.4
     * @spec JSR-51
     */
    public final FileChannel getChannel() {
        return null;
    }

    /**
     * Writes to the socket.
     * @param fd the FileDescriptor
     * @param b the data to be written
     * @param off the start offset in the data
     * @param len the number of bytes that are written
     * @exception IOException If an I/O error has occurred.
     */
    private void socketWrite0(FileDescriptor fdObj, byte[] data, int off, int len) throws IOException
    {
        // [IKVM] this method is a direct port of the native code in openjdk6-b18\jdk\src\windows\native\java\net\SocketOutputStream.c
        final int MAX_BUFFER_LEN = 2048;
        cli.System.Net.Sockets.Socket fd;
        int buflen = 65536; // MAX_HEAP_BUFFER_LEN
        int n;

        if (IS_NULL(fdObj)) {
            throw new SocketException("socket closed");
        } else {
            fd = fdObj.getSocket();
        }
        if (IS_NULL(data)) {
            throw new NullPointerException("data argument");
        }
        
        while(len > 0) {
            int loff = 0;
            int chunkLen = Math.min(buflen, len);
            int llen = chunkLen;
            int retry = 0;

            while(llen > 0) {
                n = send(fd, data, off + loff, llen, 0);
                if (n > 0) {
                    llen -= n;
                    loff += n;
                    continue;
                }

                /*
                 * Due to a bug in Windows Sockets (observed on NT and Windows
                 * 2000) it may be necessary to retry the send. The issue is that
                 * on blocking sockets send/WSASend is supposed to block if there
                 * is insufficient buffer space available. If there are a large
                 * number of threads blocked on write due to congestion then it's
                 * possile to hit the NT/2000 bug whereby send returns WSAENOBUFS.
                 * The workaround we use is to retry the send. If we have a
                 * large buffer to send (>2k) then we retry with a maximum of
                 * 2k buffer. If we hit the issue with <=2k buffer then we backoff
                 * for 1 second and retry again. We repeat this up to a reasonable
                 * limit before bailing out and throwing an exception. In load
                 * conditions we've observed that the send will succeed after 2-3
                 * attempts but this depends on network buffers associated with
                 * other sockets draining.
                 */
                if (WSAGetLastError() == WSAENOBUFS) {
                    if (llen > MAX_BUFFER_LEN) {
                        buflen = MAX_BUFFER_LEN;
                        chunkLen = MAX_BUFFER_LEN;
                        llen = MAX_BUFFER_LEN;
                        continue;
                    }
                    if (retry >= 30) {
                        throw new SocketException("No buffer space available - exhausted attempts to queue buffer");
                    }
                    cli.System.Threading.Thread.Sleep(1000);
                    retry++;
                    continue;
                }

                /*
                 * Send failed - can be caused by close or write error.
                 */
                if (WSAGetLastError() == WSAENOTSOCK) {
                    throw new SocketException("Socket closed");
                } else {
                    throw NET_ThrowCurrent("socket write error");
                }
            }
            len -= chunkLen;
            off += chunkLen;
        }
    }

    /**
     * Writes to the socket with appropriate locking of the
     * FileDescriptor.
     * @param b the data to be written
     * @param off the start offset in the data
     * @param len the number of bytes that are written
     * @exception IOException If an I/O error has occurred.
     */
    private void socketWrite(byte b[], int off, int len) throws IOException {

        if (len <= 0 || off < 0 || off + len > b.length) {
            if (len == 0) {
                return;
            }
            throw new ArrayIndexOutOfBoundsException();
        }

        FileDescriptor fd = impl.acquireFD();
        try {
            socketWrite0(fd, b, off, len);
        } catch (SocketException se) {
            if (se instanceof sun.net.ConnectionResetException) {
                impl.setConnectionResetPending();
                se = new SocketException("Connection reset");
            }
            if (impl.isClosedOrPending()) {
                throw new SocketException("Socket closed");
            } else {
                throw se;
            }
        } finally {
            impl.releaseFD();
        }
    }

    /**
     * Writes a byte to the socket.
     * @param b the data to be written
     * @exception IOException If an I/O error has occurred.
     */
    public void write(int b) throws IOException {
        temp[0] = (byte)b;
        socketWrite(temp, 0, 1);
    }

    /**
     * Writes the contents of the buffer <i>b</i> to the socket.
     * @param b the data to be written
     * @exception SocketException If an I/O error has occurred.
     */
    public void write(byte b[]) throws IOException {
        socketWrite(b, 0, b.length);
    }

    /**
     * Writes <i>length</i> bytes from buffer <i>b</i> starting at
     * offset <i>len</i>.
     * @param b the data to be written
     * @param off the start offset in the data
     * @param len the number of bytes that are written
     * @exception SocketException If an I/O error has occurred.
     */
    public void write(byte b[], int off, int len) throws IOException {
        socketWrite(b, off, len);
    }

    /**
     * Closes the stream.
     */
    private boolean closing = false;
    public void close() throws IOException {
        // Prevent recursion. See BugId 4484411
        if (closing)
            return;
        closing = true;
        if (socket != null) {
            if (!socket.isClosed())
                socket.close();
        } else
            impl.close();
        closing = false;
    }

    /**
     * Overrides finalize, the fd is closed by the Socket.
     */
    protected void finalize() {}
}
