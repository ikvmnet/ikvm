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
import java.nio.ByteBuffer;
import java.nio.BufferOverflowException;
import java.net.*;
import java.util.concurrent.*;
import java.io.FileDescriptor;
import java.io.IOException;
import java.security.AccessController;
import java.security.PrivilegedActionException;
import java.security.PrivilegedExceptionAction;

/**
 * Windows implementation of AsynchronousSocketChannel using overlapped I/O.
 */

class WindowsAsynchronousSocketChannelImpl
    extends AsynchronousSocketChannelImpl
{
    // maximum vector size for scatter/gather I/O
    private static final int MAX_WSABUF     = 16;

    // I/O completion port that the socket is associated with
    private final Iocp iocp;


    WindowsAsynchronousSocketChannelImpl(Iocp iocp, boolean failIfGroupShutdown)
        throws IOException
    {
        super(iocp);

        this.iocp = iocp;
    }

    WindowsAsynchronousSocketChannelImpl(Iocp iocp) throws IOException {
        this(iocp, true);
    }

    @Override
    public AsynchronousChannelGroupImpl group() {
        return iocp;
    }

    // invoked by WindowsAsynchronousServerSocketChannelImpl when new connection
    // accept
    void setConnected(InetSocketAddress localAddress,
                      InetSocketAddress remoteAddress)
    {
        synchronized (stateLock) {
            state = ST_CONNECTED;
            this.localAddress = localAddress;
            this.remoteAddress = remoteAddress;
        }
    }

    @Override
    void implClose() throws IOException {
        // close socket (may cause outstanding async I/O operations to fail).
        SocketDispatcher.closeImpl(fd);
    }

    @Override
    public void onCancel(PendingFuture<?,?> task) {
        if (task.getContext() instanceof ConnectTask)
            killConnect();
        if (task.getContext() instanceof ReadTask)
            killReading();
        if (task.getContext() instanceof WriteTask)
            killWriting();
    }

    /**
     * Implements the task to initiate a connection and the handler to
     * consume the result when the connection is established (or fails).
     */
    private class ConnectTask<A> implements Runnable, Iocp.ResultHandler {
        private final InetSocketAddress remote;
        private final PendingFuture<Void,A> result;

        ConnectTask(InetSocketAddress remote, PendingFuture<Void,A> result) {
            this.remote = remote;
            this.result = result;
        }

        private void closeChannel() {
            try {
                close();
            } catch (IOException ignore) { }
        }

        private IOException toIOException(Throwable x) {
            if (x instanceof IOException) {
                if (x instanceof ClosedChannelException)
                    x = new AsynchronousCloseException();
                return (IOException)x;
            }
            return new IOException(x);
        }

        /**
         * Invoke after a connection is successfully established.
         */
        private void afterConnect() throws IOException {
            updateConnectContext(fd);
            synchronized (stateLock) {
                state = ST_CONNECTED;
                remoteAddress = remote;
            }
        }

        /**
         * Task to initiate a connection.
         */
        @Override
        public void run() {
            Throwable exc = null;
            try {
                begin();

                // synchronize on result to allow this thread handle the case
                // where the connection is established immediately.
                synchronized (result) {
                    // initiate the connection
                    int n = connect0(fd, Net.isIPv6Available(), remote.getAddress(),
                                     remote.getPort(), this);
                    if (n == IOStatus.UNAVAILABLE) {
                        // connection is pending
                        return;
                    }

                    // connection established immediately
                    afterConnect();
                    result.setResult(null);
                }
            } catch (Throwable x) {
                exc = x;
            } finally {
                end();
            }

            if (exc != null) {
                closeChannel();
                result.setFailure(toIOException(exc));
            }
            Invoker.invoke(result);
        }

        /**
         * Invoked by handler thread when connection established.
         */
        @Override
        public void completed(int bytesTransferred, boolean canInvokeDirect) {
            Throwable exc = null;
            try {
                begin();
                afterConnect();
                result.setResult(null);
            } catch (Throwable x) {
                // channel is closed or unable to finish connect
                exc = x;
            } finally {
                end();
            }

            // can't close channel while in begin/end block
            if (exc != null) {
                closeChannel();
                result.setFailure(toIOException(exc));
            }

            if (canInvokeDirect) {
                Invoker.invokeUnchecked(result);
            } else {
                Invoker.invoke(result);
            }
        }

        /**
         * Invoked by handler thread when failed to establish connection.
         */
        @Override
        public void failed(int error, IOException x) {
            if (isOpen()) {
                closeChannel();
                result.setFailure(x);
            } else {
                result.setFailure(new AsynchronousCloseException());
            }
            Invoker.invoke(result);
        }
    }

    private void doPrivilegedBind(final SocketAddress sa) throws IOException {
        try {
            AccessController.doPrivileged(new PrivilegedExceptionAction<Void>() {
                public Void run() throws IOException {
                    bind(sa);
                    return null;
                }
            });
        } catch (PrivilegedActionException e) {
            throw (IOException) e.getException();
        }
    }

    @Override
    <A> Future<Void> implConnect(SocketAddress remote,
                                 A attachment,
                                 CompletionHandler<Void,? super A> handler)
    {
        if (!isOpen()) {
            Throwable exc = new ClosedChannelException();
            if (handler == null)
                return CompletedFuture.withFailure(exc);
            Invoker.invoke(this, handler, attachment, null, exc);
            return null;
        }

        InetSocketAddress isa = Net.checkAddress(remote);

        // permission check
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
            sm.checkConnect(isa.getAddress().getHostAddress(), isa.getPort());

        // check and update state
        // ConnectEx requires the socket to be bound to a local address
        IOException bindException = null;
        synchronized (stateLock) {
            if (state == ST_CONNECTED)
                throw new AlreadyConnectedException();
            if (state == ST_PENDING)
                throw new ConnectionPendingException();
            if (localAddress == null) {
                try {
                    SocketAddress any = new InetSocketAddress(0);
                    if (sm == null) {
                        bind(any);
                    } else {
                        doPrivilegedBind(any);
                    }
                } catch (IOException x) {
                    bindException = x;
                }
            }
            if (bindException == null)
                state = ST_PENDING;
        }

        // handle bind failure
        if (bindException != null) {
            try {
                close();
            } catch (IOException ignore) { }
            if (handler == null)
                return CompletedFuture.withFailure(bindException);
            Invoker.invoke(this, handler, attachment, null, bindException);
            return null;
        }

        // setup task
        PendingFuture<Void,A> result =
            new PendingFuture<Void,A>(this, handler, attachment);
        ConnectTask<A> task = new ConnectTask<A>(isa, result);
        result.setContext(task);

        // initiate I/O
        if (Iocp.supportsThreadAgnosticIo()) {
            task.run();
        } else {
            Invoker.invokeOnThreadInThreadPool(this, task);
        }
        return result;
    }

    /**
     * Implements the task to initiate a read and the handler to consume the
     * result when the read completes.
     */
    private class ReadTask<V,A> implements Runnable, Iocp.ResultHandler {
        private final ByteBuffer[] bufs;
        private final int numBufs;
        private final boolean scatteringRead;
        private final PendingFuture<V,A> result;

        // set by run method
        private ByteBuffer[] shadow;

        ReadTask(ByteBuffer[] bufs,
                 boolean scatteringRead,
                 PendingFuture<V,A> result)
        {
            this.bufs = bufs;
            this.numBufs = (bufs.length > MAX_WSABUF) ? MAX_WSABUF : bufs.length;
            this.scatteringRead = scatteringRead;
            this.result = result;
        }

        /**
         * Invoked prior to read to prepare the WSABUF array. Where necessary,
         * it substitutes direct buffers with managed buffers.
         */
        void prepareBuffers() {
            shadow = new ByteBuffer[numBufs];
            for (int i=0; i<numBufs; i++) {
                ByteBuffer dst = bufs[i];
                int pos = dst.position();
                int lim = dst.limit();
                assert (pos <= lim);
                int rem = (pos <= lim ? lim - pos : 0);
                if (!dst.hasArray()) {
                    // substitute with direct buffer
                    ByteBuffer bb = ByteBuffer.allocate(rem);
                    shadow[i] = bb;
                } else {
                    shadow[i] = dst;
                }
            }
        }

        /**
         * Invoked after a read has completed to update the buffer positions
         * and release any substituted buffers.
         */
        void updateBuffers(int bytesRead) {
            for (int i=0; i<numBufs; i++) {
                ByteBuffer nextBuffer = shadow[i];
                int pos = nextBuffer.position();
                int len = nextBuffer.remaining();
                if (bytesRead >= len) {
                    bytesRead -= len;
                    int newPosition = pos + len;
                    try {
                        nextBuffer.position(newPosition);
                    } catch (IllegalArgumentException x) {
                        // position changed by another
                    }
                } else { // Buffers not completely filled
                    if (bytesRead > 0) {
                        assert(pos + bytesRead < (long)Integer.MAX_VALUE);
                        int newPosition = pos + bytesRead;
                        try {
                            nextBuffer.position(newPosition);
                        } catch (IllegalArgumentException x) {
                            // position changed by another
                        }
                    }
                    break;
                }
            }

            // Put results from shadow into the slow buffers
            for (int i=0; i<numBufs; i++) {
                if (!bufs[i].hasArray()) {
                    shadow[i].flip();
                    try {
                        bufs[i].put(shadow[i]);
                    } catch (BufferOverflowException x) {
                        // position changed by another
                    }
                }
            }
        }

        void releaseBuffers() {
        }

        @Override
        @SuppressWarnings("unchecked")
        public void run() {
            boolean prepared = false;
            boolean pending = false;

            try {
                begin();

                // substitute direct buffers
                prepareBuffers();
                prepared = true;

                // initiate read
                int n = read0(fd, shadow, this);
                if (n == IOStatus.UNAVAILABLE) {
                    // I/O is pending
                    pending = true;
                    return;
                }
                if (n == IOStatus.EOF) {
                    // input shutdown
                    enableReading();
                    if (scatteringRead) {
                        result.setResult((V)Long.valueOf(-1L));
                    } else {
                        result.setResult((V)Integer.valueOf(-1));
                    }
                }
                // read completed immediately
                if (n == 0) {
                    n = -1; // EOF
                } else {
                    updateBuffers(n);
                }
                releaseBuffers();
                enableReading();
                if (scatteringRead) {
                    result.setResult((V)Long.valueOf(n));
                } else {
                    result.setResult((V)Integer.valueOf(n));
                }
            } catch (Throwable x) {
                // failed to initiate read
                // reset read flag before releasing waiters
                enableReading();
                if (x instanceof ClosedChannelException)
                    x = new AsynchronousCloseException();
                if (!(x instanceof IOException))
                    x = new IOException(x);
                result.setFailure(x);
            } finally {
                // release resources if I/O not pending
                if (!pending) {
                    if (prepared)
                        releaseBuffers();
                }
                end();
            }

            // invoke completion handler
            Invoker.invoke(result);
        }

        /**
         * Executed when the I/O has completed
         */
        @Override
        @SuppressWarnings("unchecked")
        public void completed(int bytesTransferred, boolean canInvokeDirect) {
            if (bytesTransferred == 0) {
                bytesTransferred = -1;  // EOF
            } else {
                updateBuffers(bytesTransferred);
            }

            // return direct buffer to cache if substituted
            releaseBuffers();

            // release waiters if not already released by timeout
            synchronized (result) {
                if (result.isDone())
                    return;
                enableReading();
                if (scatteringRead) {
                    result.setResult((V)Long.valueOf(bytesTransferred));
                } else {
                    result.setResult((V)Integer.valueOf(bytesTransferred));
                }
            }
            if (canInvokeDirect) {
                Invoker.invokeUnchecked(result);
            } else {
                Invoker.invoke(result);
            }
        }

        @Override
        public void failed(int error, IOException x) {
            // return direct buffer to cache if substituted
            releaseBuffers();

            // release waiters if not already released by timeout
            if (!isOpen())
                x = new AsynchronousCloseException();

            synchronized (result) {
                if (result.isDone())
                    return;
                enableReading();
                result.setFailure(x);
            }
            Invoker.invoke(result);
        }

        /**
         * Invoked if timeout expires before it is cancelled
         */
        void timeout() {
            // synchronize on result as the I/O could complete/fail
            synchronized (result) {
                if (result.isDone())
                    return;

                // kill further reading before releasing waiters
                enableReading(true);
                result.setFailure(new InterruptedByTimeoutException());
            }

            // invoke handler without any locks
            Invoker.invoke(result);
        }
    }

    @Override
    <V extends Number,A> Future<V> implRead(boolean isScatteringRead,
                                            ByteBuffer dst,
                                            ByteBuffer[] dsts,
                                            long timeout,
                                            TimeUnit unit,
                                            A attachment,
                                            CompletionHandler<V,? super A> handler)
    {
        // setup task
        PendingFuture<V,A> result =
            new PendingFuture<V,A>(this, handler, attachment);
        ByteBuffer[] bufs;
        if (isScatteringRead) {
            bufs = dsts;
        } else {
            bufs = new ByteBuffer[1];
            bufs[0] = dst;
        }
        final ReadTask<V,A> readTask =
                new ReadTask<V,A>(bufs, isScatteringRead, result);
        result.setContext(readTask);

        // schedule timeout
        if (timeout > 0L) {
            Future<?> timeoutTask = iocp.schedule(new Runnable() {
                public void run() {
                    readTask.timeout();
                }
            }, timeout, unit);
            result.setTimeoutTask(timeoutTask);
        }

        // initiate I/O
        if (Iocp.supportsThreadAgnosticIo()) {
            readTask.run();
        } else {
            Invoker.invokeOnThreadInThreadPool(this, readTask);
        }
        return result;
    }

    /**
     * Implements the task to initiate a write and the handler to consume the
     * result when the write completes.
     */
    private class WriteTask<V,A> implements Runnable, Iocp.ResultHandler {
        private final ByteBuffer[] bufs;
        private final int numBufs;
        private final boolean gatheringWrite;
        private final PendingFuture<V,A> result;

        // set by run method
        private ByteBuffer[] shadow;

        WriteTask(ByteBuffer[] bufs,
                  boolean gatheringWrite,
                  PendingFuture<V,A> result)
        {
            this.bufs = bufs;
            this.numBufs = (bufs.length > MAX_WSABUF) ? MAX_WSABUF : bufs.length;
            this.gatheringWrite = gatheringWrite;
            this.result = result;
        }

        /**
         * Invoked prior to write to prepare the WSABUF array. Where necessary,
         * it substitutes direct buffers with managed buffers.
         */
        void prepareBuffers() {
            shadow = new ByteBuffer[numBufs];
            for (int i=0; i<numBufs; i++) {
                ByteBuffer src = bufs[i];
                int pos = src.position();
                int lim = src.limit();
                assert (pos <= lim);
                int rem = (pos <= lim ? lim - pos : 0);
                if (!src.hasArray()) {
                    // substitute with direct buffer
                    ByteBuffer bb = ByteBuffer.allocate(rem);
                    bb.put(src);
                    bb.flip();
                    src.position(pos);  // leave heap buffer untouched for now
                    shadow[i] = bb;
                } else {
                    shadow[i] = src;
                }
            }
        }

        /**
         * Invoked after a write has completed to update the buffer positions
         * and release any substituted buffers.
         */
        void updateBuffers(int bytesWritten) {
            // Notify the buffers how many bytes were taken
            for (int i=0; i<numBufs; i++) {
                ByteBuffer nextBuffer = bufs[i];
                int pos = nextBuffer.position();
                int lim = nextBuffer.limit();
                int len = (pos <= lim ? lim - pos : lim);
                if (bytesWritten >= len) {
                    bytesWritten -= len;
                    int newPosition = pos + len;
                    try {
                        nextBuffer.position(newPosition);
                    } catch (IllegalArgumentException x) {
                        // position changed by someone else
                    }
                } else { // Buffers not completely filled
                    if (bytesWritten > 0) {
                        assert(pos + bytesWritten < (long)Integer.MAX_VALUE);
                        int newPosition = pos + bytesWritten;
                        try {
                            nextBuffer.position(newPosition);
                        } catch (IllegalArgumentException x) {
                            // position changed by someone else
                        }
                    }
                    break;
                }
            }
        }

        void releaseBuffers() {
        }

        @Override
        //@SuppressWarnings("unchecked")
        public void run() {
            boolean prepared = false;
            boolean pending = false;
            boolean shutdown = false;

            try {
                begin();

                // substitute direct buffers
                prepareBuffers();
                prepared = true;

                int n = write0(fd, shadow, this);
                if (n == IOStatus.UNAVAILABLE) {
                    // I/O is pending
                    pending = true;
                    return;
                }
                if (n == IOStatus.EOF) {
                    // special case for shutdown output
                    shutdown = true;
                    throw new ClosedChannelException();
                }
                // write completed immediately
                updateBuffers(n);
                releaseBuffers();
                enableWriting();
                if (gatheringWrite) {
                    result.setResult((V)Long.valueOf(n));
                } else {
                    result.setResult((V)Integer.valueOf(n));
                }
            } catch (Throwable x) {
                // write failed. Enable writing before releasing waiters.
                enableWriting();
                if (!shutdown && (x instanceof ClosedChannelException))
                    x = new AsynchronousCloseException();
                if (!(x instanceof IOException))
                    x = new IOException(x);
                result.setFailure(x);
            } finally {
                // release resources if I/O not pending
                if (!pending) {
                    if (prepared)
                        releaseBuffers();
                }
                end();
            }

            // invoke completion handler
            Invoker.invoke(result);
        }

        /**
         * Executed when the I/O has completed
         */
        @Override
        @SuppressWarnings("unchecked")
        public void completed(int bytesTransferred, boolean canInvokeDirect) {
            updateBuffers(bytesTransferred);

            // return direct buffer to cache if substituted
            releaseBuffers();

            // release waiters if not already released by timeout
            synchronized (result) {
                if (result.isDone())
                    return;
                enableWriting();
                if (gatheringWrite) {
                    result.setResult((V)Long.valueOf(bytesTransferred));
                } else {
                    result.setResult((V)Integer.valueOf(bytesTransferred));
                }
            }
            if (canInvokeDirect) {
                Invoker.invokeUnchecked(result);
            } else {
                Invoker.invoke(result);
            }
        }

        @Override
        public void failed(int error, IOException x) {
            // return direct buffer to cache if substituted
            releaseBuffers();

            // release waiters if not already released by timeout
            if (!isOpen())
                x = new AsynchronousCloseException();

            synchronized (result) {
                if (result.isDone())
                    return;
                enableWriting();
                result.setFailure(x);
            }
            Invoker.invoke(result);
        }

        /**
         * Invoked if timeout expires before it is cancelled
         */
        void timeout() {
            // synchronize on result as the I/O could complete/fail
            synchronized (result) {
                if (result.isDone())
                    return;

                // kill further writing before releasing waiters
                enableWriting(true);
                result.setFailure(new InterruptedByTimeoutException());
            }

            // invoke handler without any locks
            Invoker.invoke(result);
        }
    }

    @Override
    <V extends Number,A> Future<V> implWrite(boolean gatheringWrite,
                                             ByteBuffer src,
                                             ByteBuffer[] srcs,
                                             long timeout,
                                             TimeUnit unit,
                                             A attachment,
                                             CompletionHandler<V,? super A> handler)
    {
        // setup task
        PendingFuture<V,A> result =
            new PendingFuture<V,A>(this, handler, attachment);
        ByteBuffer[] bufs;
        if (gatheringWrite) {
            bufs = srcs;
        } else {
            bufs = new ByteBuffer[1];
            bufs[0] = src;
        }
        final WriteTask<V,A> writeTask =
                new WriteTask<V,A>(bufs, gatheringWrite, result);
        result.setContext(writeTask);

        // schedule timeout
        if (timeout > 0L) {
            Future<?> timeoutTask = iocp.schedule(new Runnable() {
                public void run() {
                    writeTask.timeout();
                }
            }, timeout, unit);
            result.setTimeoutTask(timeoutTask);
        }

        // initiate I/O (can only be done from thread in thread pool)
        // initiate I/O
        if (Iocp.supportsThreadAgnosticIo()) {
            writeTask.run();
        } else {
            Invoker.invokeOnThreadInThreadPool(this, writeTask);
        }
        return result;
    }

    // -- Native methods --

    private static native void initIDs();

    private static native int connect0(FileDescriptor fd, boolean preferIPv6,
        InetAddress remote, int remotePort, Iocp.ResultHandler handler) throws IOException;

    private static native void updateConnectContext(FileDescriptor fd) throws IOException;

    private static native int read0(FileDescriptor fd, ByteBuffer[] bufs, Iocp.ResultHandler handler)
        throws IOException;

    private static native int write0(FileDescriptor fd, ByteBuffer[] bufs, Iocp.ResultHandler handler)
        throws IOException;

    private static native void shutdown0(long socket, int how) throws IOException;

    private static native void closesocket0(long socket) throws IOException;

    static {
        IOUtil.load();
        initIDs();
    }
}
