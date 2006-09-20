package gnu.java.nio;

import java.io.IOException;
import java.nio.channels.DatagramChannel;
import java.nio.channels.Pipe;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.nio.channels.spi.AbstractSelector;
import java.nio.channels.spi.SelectorProvider;

public final class SelectorProviderImpl extends SelectorProvider
{
    public DatagramChannel openDatagramChannel() throws IOException
    {
        return new DatagramChannelImpl(this);
    }

    public Pipe openPipe() throws IOException
    {
        return new PipeImpl(this);
    }
    
    public AbstractSelector openSelector() throws IOException
    {
        return new SelectorImpl(this);
    }

    public ServerSocketChannel openServerSocketChannel() throws IOException
    {
        return new ServerSocketChannelImpl(this);
    }

    public SocketChannel openSocketChannel() throws IOException
    {
        return new SocketChannelImpl(this);
    }
}
