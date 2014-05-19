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
import java.nio.channels.spi.AsynchronousChannelProvider;
import java.io.Closeable;
import java.io.IOException;
import java.io.FileDescriptor;
import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;
import ikvm.internal.NotYetImplementedError;

/**
 * Windows implementation of AsynchronousChannelGroup encapsulating an I/O
 * completion port.
 */

class Iocp extends AsynchronousChannelGroupImpl {
    private static final boolean supportsThreadAgnosticIo;

    // true if port has been closed
    private boolean closed;

    // the set of "stale" OVERLAPPED structures. These OVERLAPPED structures
    // relate to I/O operations where the completion notification was not
    // received in a timely manner after the channel is closed.
    private final Set<Long> staleIoSet = new HashSet<Long>();

    Iocp(AsynchronousChannelProvider provider, ThreadPool pool)
        throws IOException
    {
        super(provider, pool);
    }

    Iocp start() {
        return this;
    }

    /*
     * Channels implements this interface support overlapped I/O and can be
     * associated with a completion port.
     */
    static interface OverlappedChannel extends Closeable {
        /**
         * Returns a reference to the pending I/O result.
         */
        <V,A> PendingFuture<V,A> getByOverlapped(long overlapped);
    }

    /**
     * Indicates if this operating system supports thread agnostic I/O.
     */
    static boolean supportsThreadAgnosticIo() {
        return supportsThreadAgnosticIo;
    }

    // release all resources
    void implClose() {
        synchronized (this) {
            if (closed)
                return;
            closed = true;
        }
    }

    @Override
    boolean isEmpty() {
        return true;
    }

    @Override
    final Object attachForeignChannel(final Channel channel, FileDescriptor fdObj)
        throws IOException
    {
        throw new NotYetImplementedError();
    }

    @Override
    final void detachForeignChannel(Object key) {
        throw new NotYetImplementedError();
    }

    @Override
    void closeAllChannels() {
    }

    @Override
    void executeOnHandlerTask(Runnable task) {
        throw new NotYetImplementedError();
    }

    @Override
    void shutdownHandlerTasks() {
    }

    /**
     * The handler for consuming the result of an asynchronous I/O operation.
     */
    static interface ResultHandler {
        /**
         * Invoked if the I/O operation completes successfully.
         */
        public void completed(int bytesTransferred, boolean canInvokeDirect);

        /**
         * Invoked if the I/O operation fails.
         */
        public void failed(int error, IOException ioe);
    }

    static {
        supportsThreadAgnosticIo = true;
    }
}
