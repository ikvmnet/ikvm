/*
 * Copyright (c) 2008, 2013, Oracle and/or its affiliates. All rights reserved.
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

import java.nio.channels.*;
import java.net.InetSocketAddress;
import java.util.concurrent.Future;
import java.util.concurrent.atomic.AtomicBoolean;
import java.io.FileDescriptor;
import java.io.IOException;
import java.security.AccessControlContext;
import java.security.AccessController;
import java.security.PrivilegedAction;

/**
 * Windows implementation of AsynchronousServerSocketChannel using overlapped I/O.
 */

class WindowsAsynchronousServerSocketChannelImpl
    extends AsynchronousServerSocketChannelImpl
{
    private final Iocp iocp;

    // flag to indicate that an accept operation is outstanding
    private AtomicBoolean accepting = new AtomicBoolean();


    WindowsAsynchronousServerSocketChannelImpl(Iocp iocp) throws IOException {
        super(iocp);

        this.iocp = iocp;
    }

    @Override
    void implClose() throws IOException {
        // close socket (which may cause outstanding accept to be aborted).
        SocketDispatcher.closeImpl(fd);
    }

    @Override
    public AsynchronousChannelGroupImpl group() {
        return iocp;
    }

    /**
     * Task to initiate accept operation and to handle result.
     */
    private class AcceptTask implements Runnable, Iocp.ResultHandler {
        private final WindowsAsynchronousSocketChannelImpl channel;
        private final AccessControlContext acc;
        private final PendingFuture<AsynchronousSocketChannel,Object> result;

        AcceptTask(WindowsAsynchronousSocketChannelImpl channel,
                   AccessControlContext acc,
                   PendingFuture<AsynchronousSocketChannel,Object> result)
        {
            this.channel = channel;
            this.acc = acc;
            this.result = result;
        }

        void enableAccept() {
            accepting.set(false);
        }

        void closeChildChannel() {
            try {
                channel.close();
            } catch (IOException ignore) { }
        }

        // caller must have acquired read lock for the listener and child channel.
        void finishAccept() throws IOException {
            /**
             * Set local/remote addresses. This is currently very inefficient
             * in that it requires 2 calls to getsockname and 2 calls to getpeername.
             * (should change this to use GetAcceptExSockaddrs)
             */
            updateAcceptContext(fd, channel.fd);

            InetSocketAddress local = Net.localAddress(channel.fd);
            final InetSocketAddress remote = Net.remoteAddress(channel.fd);
            channel.setConnected(local, remote);

            // permission check (in context of initiating thread)
            if (acc != null) {
                AccessController.doPrivileged(new PrivilegedAction<Void>() {
                    public Void run() {
                        SecurityManager sm = System.getSecurityManager();
                        sm.checkAccept(remote.getAddress().getHostAddress(),
                                       remote.getPort());
                        return null;
                    }
                }, acc);
            }
        }

        /**
         * Initiates the accept operation.
         */
        @Override
        public void run() {

            try {
                // begin usage of listener socket
                begin();
                try {
                    // begin usage of child socket (as it is registered with
                    // completion port and so may be closed in the event that
                    // the group is forcefully closed).
                    channel.begin();

                    synchronized (result) {

                        int n = accept0(fd, channel.fd, this);
                        if (n == IOStatus.UNAVAILABLE) {
                            return;
                        }

                        // connection accepted immediately
                        finishAccept();

                        // allow another accept before the result is set
                        enableAccept();
                        result.setResult(channel);
                    }
                } finally {
                    // end usage on child socket
                    channel.end();
                }
            } catch (Throwable x) {
                // failed to initiate accept so release resources
                closeChildChannel();
                if (x instanceof ClosedChannelException)
                    x = new AsynchronousCloseException();
                if (!(x instanceof IOException) && !(x instanceof SecurityException))
                    x = new IOException(x);
                enableAccept();
                result.setFailure(x);
            } finally {
                // end of usage of listener socket
                end();
            }

            // accept completed immediately but may not have executed on
            // initiating thread in which case the operation may have been
            // cancelled.
            if (result.isCancelled()) {
                closeChildChannel();
            }

            // invoke completion handler
            Invoker.invokeIndirectly(result);
        }

        /**
         * Executed when the I/O has completed
         */
        @Override
        public void completed(int bytesTransferred, boolean canInvokeDirect) {
            try {
                // connection accept after group has shutdown
                if (iocp.isShutdown()) {
                    throw new IOException(new ShutdownChannelGroupException());
                }

                // finish the accept
                try {
                    begin();
                    try {
                        channel.begin();
                        finishAccept();
                    } finally {
                        channel.end();
                    }
                } finally {
                    end();
                }

                // allow another accept before the result is set
                enableAccept();
                result.setResult(channel);
            } catch (Throwable x) {
                enableAccept();
                closeChildChannel();
                if (x instanceof ClosedChannelException)
                    x = new AsynchronousCloseException();
                if (!(x instanceof IOException) && !(x instanceof SecurityException))
                    x = new IOException(x);
                result.setFailure(x);
            }

            // if an async cancel has already cancelled the operation then
            // close the new channel so as to free resources
            if (result.isCancelled()) {
                closeChildChannel();
            }

            // invoke handler (but not directly)
            Invoker.invokeIndirectly(result);
        }

        @Override
        public void failed(int error, IOException x) {
            enableAccept();
            closeChildChannel();

            // release waiters
            if (isOpen()) {
                result.setFailure(x);
            } else {
                result.setFailure(new AsynchronousCloseException());
            }
            Invoker.invokeIndirectly(result);
        }
    }

    @Override
    Future<AsynchronousSocketChannel> implAccept(Object attachment,
        final CompletionHandler<AsynchronousSocketChannel,Object> handler)
    {
        if (!isOpen()) {
            Throwable exc = new ClosedChannelException();
            if (handler == null)
                return CompletedFuture.withFailure(exc);
            Invoker.invokeIndirectly(this, handler, attachment, null, exc);
            return null;
        }
        if (isAcceptKilled())
            throw new RuntimeException("Accept not allowed due to cancellation");

        // ensure channel is bound to local address
        if (localAddress == null)
            throw new NotYetBoundException();

        // create the socket that will be accepted. The creation of the socket
        // is enclosed by a begin/end for the listener socket to ensure that
        // we check that the listener is open and also to prevent the I/O
        // port from being closed as the new socket is registered.
        WindowsAsynchronousSocketChannelImpl ch = null;
        IOException ioe = null;
        try {
            begin();
            ch = new WindowsAsynchronousSocketChannelImpl(iocp, false);
        } catch (IOException x) {
            ioe = x;
        } finally {
            end();
        }
        if (ioe != null) {
            if (handler == null)
                return CompletedFuture.withFailure(ioe);
            Invoker.invokeIndirectly(this, handler, attachment, null, ioe);
            return null;
        }

        // need calling context when there is security manager as
        // permission check may be done in a different thread without
        // any application call frames on the stack
        AccessControlContext acc = (System.getSecurityManager() == null) ?
            null : AccessController.getContext();

        PendingFuture<AsynchronousSocketChannel,Object> result =
            new PendingFuture<AsynchronousSocketChannel,Object>(this, handler, attachment);
        AcceptTask task = new AcceptTask(ch, acc, result);
        result.setContext(task);

        // check and set flag to prevent concurrent accepting
        if (!accepting.compareAndSet(false, true))
            throw new AcceptPendingException();

        // initiate I/O
        if (Iocp.supportsThreadAgnosticIo()) {
            task.run();
        } else {
            Invoker.invokeOnThreadInThreadPool(this, task);
        }
        return result;
    }

    // -- Native methods --

    private static native void initIDs();

    private static native int accept0(FileDescriptor listenSocket, FileDescriptor acceptSocket,
        Iocp.ResultHandler handler) throws IOException;

    private static native void updateAcceptContext(FileDescriptor listenSocket,
        FileDescriptor acceptSocket) throws IOException;

    private static native void closesocket0(long socket) throws IOException;

    static {
        IOUtil.load();
        initIDs();
    }
}
