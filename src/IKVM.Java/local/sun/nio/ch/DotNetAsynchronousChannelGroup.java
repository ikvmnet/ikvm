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
 * .NET implementation of AsynchronousChannelGroup.
 */
class DotNetAsynchronousChannelGroup extends AsynchronousChannelGroupImpl {

    private boolean closed;

    DotNetAsynchronousChannelGroup(AsynchronousChannelProvider provider, ThreadPool pool) throws IOException {
        super(provider, pool);
    }

    // release all resources
    synchronized void implClose() {
        closed = true;
    }

    @Override
    boolean isEmpty() {
        return true;
    }

    @Override
    final Object attachForeignChannel(final Channel channel, FileDescriptor fdObj) throws IOException {
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

}
