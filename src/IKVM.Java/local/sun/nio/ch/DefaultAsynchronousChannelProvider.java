package sun.nio.ch;

import java.nio.channels.spi.AsynchronousChannelProvider;

public class DefaultAsynchronousChannelProvider {

    private DefaultAsynchronousChannelProvider() {
        
    }

    public static AsynchronousChannelProvider create() {
        return new DotNetAsynchronousChannelProvider();
    }

}
