package sun.nio.ch;

import java.io.*;
import java.net.*;
import java.nio.*;
import java.nio.channels.*;
import java.util.concurrent.*;

/**
 * .NET implementation of AsynchronousSocketChannelImpl.
 */
class DotNetAsynchronousSocketChannelImpl extends AsynchronousSocketChannelImpl {

    private final DotNetAsynchronousChannelGroup group;

    DotNetAsynchronousSocketChannelImpl(DotNetAsynchronousChannelGroup group, boolean failIfGroupShutdown) throws IOException {
        super(group);
        this.group = group;
    }

    DotNetAsynchronousSocketChannelImpl(DotNetAsynchronousChannelGroup group) throws IOException {
        this(group, true);
    }

    @Override
    public AsynchronousChannelGroupImpl group() {
        return group;
    }

    @Override
    public void onCancel(PendingFuture<?, ?> task) {
        onCancel0(task);
    }

    @Override
    void implClose() throws IOException {
        implClose0();
    }

    @Override
    <A> Future<Void> implConnect(SocketAddress remote, A attachment, CompletionHandler<Void, ? super A> handler) {
        return implConnect0(remote, attachment, handler);
    }

    @Override
    <V extends Number, A> Future<V> implRead(boolean isScatteringRead, ByteBuffer dst, ByteBuffer[] dsts, long timeout, TimeUnit unit, A attachment, CompletionHandler<V, ? super A> handler) {
        return implRead0(isScatteringRead, dst, dsts, timeout, unit, attachment, handler);
    }

    @Override
    <V extends Number, A> Future<V> implWrite(boolean gatheringWrite, ByteBuffer src, ByteBuffer[] srcs, long timeout, TimeUnit unit, A attachment, CompletionHandler<V, ? super A> handler) {
        return implWrite0(gatheringWrite, src, srcs, timeout, unit, attachment, handler);
    }

    private native void onCancel0(PendingFuture<?, ?> task);

    private native void implClose0();

    private native <A> Future<Void> implConnect0(SocketAddress remote, A attachment, CompletionHandler<Void, ? super A> handler);

    private native <V extends Number, A> Future<V> implRead0(boolean isScatteringRead, ByteBuffer dst, ByteBuffer[] dsts, long timeout, TimeUnit unit, A attachment, CompletionHandler<V, ? super A> handler);

    private native <V extends Number, A> Future<V> implWrite0(boolean gatheringWrite, ByteBuffer src, ByteBuffer[] srcs, long timeout, TimeUnit unit, A attachment, CompletionHandler<V, ? super A> handler);

}
