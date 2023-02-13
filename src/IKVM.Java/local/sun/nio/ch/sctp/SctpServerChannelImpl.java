package sun.nio.ch.sctp;

import java.net.SocketAddress;
import java.net.InetAddress;
import java.io.IOException;
import java.util.Set;
import java.nio.channels.spi.SelectorProvider;

import com.sun.nio.sctp.SctpChannel;
import com.sun.nio.sctp.SctpServerChannel;
import com.sun.nio.sctp.SctpSocketOption;

public class SctpServerChannelImpl extends SctpServerChannel {

    private static final String message = "SCTP not supported on this platform";

    public SctpServerChannelImpl(SelectorProvider provider) {
        super(provider);
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpChannel accept() throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpServerChannel bind(SocketAddress local, int backlog) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpServerChannel bindAddress(InetAddress address) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpServerChannel unbindAddress(InetAddress address) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SocketAddress> getAllLocalAddresses() throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> T getOption(SctpSocketOption<T> name) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> SctpServerChannel setOption(SctpSocketOption<T> name, T value) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SctpSocketOption<?>> supportedOptions() {
        throw new UnsupportedOperationException(message);
    }

    @Override
    protected void implConfigureBlocking(boolean block) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public void implCloseSelectableChannel() throws IOException {
        throw new UnsupportedOperationException(message);
    }

}
