package sun.nio.ch.sctp;

import java.net.SocketAddress;
import java.net.InetAddress;
import java.io.IOException;
import java.util.Set;
import java.nio.ByteBuffer;
import java.nio.channels.spi.SelectorProvider;

import com.sun.nio.sctp.Association;
import com.sun.nio.sctp.MessageInfo;
import com.sun.nio.sctp.NotificationHandler;
import com.sun.nio.sctp.SctpChannel;
import com.sun.nio.sctp.SctpSocketOption;

public class SctpChannelImpl extends SctpChannel {

    private static final String message = "SCTP not supported on this platform";

    public SctpChannelImpl(SelectorProvider provider) {
        super(provider);
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Association association() {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpChannel bind(SocketAddress local) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpChannel bindAddress(InetAddress address) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpChannel unbindAddress(InetAddress address) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public boolean connect(SocketAddress remote) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public boolean connect(SocketAddress remote, int maxOutStreams, int maxInStreams) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public boolean isConnectionPending() {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public boolean finishConnect() throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SocketAddress> getAllLocalAddresses() throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SocketAddress> getRemoteAddresses() throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public SctpChannel shutdown() throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> T getOption(SctpSocketOption<T> name) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> SctpChannel setOption(SctpSocketOption<T> name, T value) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public Set<SctpSocketOption<?>> supportedOptions() {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public <T> MessageInfo receive(ByteBuffer dst, T attachment, NotificationHandler<T> handler) throws IOException {
        throw new UnsupportedOperationException(message);
    }

    @Override
    public int send(ByteBuffer src, MessageInfo messageInfo) throws IOException {
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
