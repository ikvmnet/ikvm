package sun.nio.ch;

import java.io.IOException;
import java.nio.channels.*;
import java.util.concurrent.Future;
import java.util.concurrent.atomic.AtomicBoolean;

/**
 * .NET implementation of AsynchronousServerSocketChannel using overlapped I/O.
 */
class DotNetAsynchronousServerSocketChannelImpl extends AsynchronousServerSocketChannelImpl {

    protected final DotNetAsynchronousChannelGroup group;
    protected AtomicBoolean accepting = new AtomicBoolean();

    DotNetAsynchronousServerSocketChannelImpl(DotNetAsynchronousChannelGroup group) throws IOException {
        super(group);
        this.group = group;
    }

    @Override
    public AsynchronousChannelGroupImpl group() {
        return group;
    }

    @Override
    void implClose() throws IOException {
        implClose0();
    }

    @Override
    Future<AsynchronousSocketChannel> implAccept(Object attachment, final CompletionHandler<AsynchronousSocketChannel, Object> handler) {
        return implAccept0(attachment, handler);
    }

    private native Future<AsynchronousSocketChannel> implAccept0(Object attachment, final CompletionHandler<AsynchronousSocketChannel, Object> handler);

    private native void implClose0() throws IOException;

}
