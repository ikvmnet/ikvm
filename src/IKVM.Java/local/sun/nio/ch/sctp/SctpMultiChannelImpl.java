package sun.nio.ch.sctp;

import java.net.SocketAddress;
import java.net.InetAddress;
import java.io.IOException;
import java.util.Set;
import java.nio.ByteBuffer;
import java.nio.channels.spi.SelectorProvider;

import com.sun.nio.sctp.Association;
import com.sun.nio.sctp.SctpChannel;
import com.sun.nio.sctp.MessageInfo;
import com.sun.nio.sctp.NotificationHandler;
import com.sun.nio.sctp.SctpMultiChannel;
import com.sun.nio.sctp.SctpSocketOption;

public class SctpMultiChannelImpl extends SctpMultiChannel {

    private static final String message = "SCTP not supported on this platform";

    public SctpMultiChannelImpl(SelectorProvider provider) {
        super(provider);
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<Association> associations() {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpMultiChannel bind(SocketAddress local, int backlog) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpMultiChannel bindAddress(InetAddress address) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpMultiChannel unbindAddress(InetAddress address) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SocketAddress> getAllLocalAddresses() throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SocketAddress> getRemoteAddresses (Association association) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpMultiChannel shutdown(Association association) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> T getOption(SctpSocketOption<T> name, Association association) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> SctpMultiChannel setOption(SctpSocketOption<T> name, T value, Association association) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SctpSocketOption<?>> supportedOptions() {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> MessageInfo receive(ByteBuffer buffer, T attachment, NotificationHandler<T> handler) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public int send(ByteBuffer buffer, MessageInfo messageInfo) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpChannel branch(Association association) throws IOException {
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
