package sun.nio.ch;

import java.nio.channels.*;
import java.nio.channels.spi.AsynchronousChannelProvider;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.ThreadFactory;
import java.io.IOException;

public class DotNetAsynchronousChannelProvider extends AsynchronousChannelProvider {

    private static volatile DotNetAsynchronousChannelGroup defaultGroup;

    public DotNetAsynchronousChannelProvider() {
        // nothing to do
    }

    private DotNetAsynchronousChannelGroup defaultGroup() throws IOException {
        if (defaultGroup == null) {
            synchronized (DotNetAsynchronousChannelProvider.class) {
                if (defaultGroup == null) {
                    defaultGroup = new DotNetAsynchronousChannelGroup(this, ThreadPool.getDefault());
                }
            }
        }

        return defaultGroup;
    }

    @Override
    public AsynchronousChannelGroup openAsynchronousChannelGroup(int nThreads, ThreadFactory factory) throws IOException {
        return new DotNetAsynchronousChannelGroup(this, ThreadPool.create(nThreads, factory));
    }

    @Override
    public AsynchronousChannelGroup openAsynchronousChannelGroup(ExecutorService executor, int initialSize) throws IOException {
        return new DotNetAsynchronousChannelGroup(this, ThreadPool.wrap(executor, initialSize));
    }

    private DotNetAsynchronousChannelGroup toDotNet(AsynchronousChannelGroup group) throws IOException {
        if (group == null) {
            return defaultGroup();
        } else {
            if (!(group instanceof DotNetAsynchronousChannelGroup))
                throw new IllegalChannelGroupException();

            return (DotNetAsynchronousChannelGroup)group;
        }
    }

    @Override
    public AsynchronousServerSocketChannel openAsynchronousServerSocketChannel(AsynchronousChannelGroup group) throws IOException {
        return new DotNetAsynchronousServerSocketChannelImpl(toDotNet(group));
    }

    @Override
    public AsynchronousSocketChannel openAsynchronousSocketChannel(AsynchronousChannelGroup group) throws IOException {
        return new DotNetAsynchronousSocketChannelImpl(toDotNet(group));
    }

}
