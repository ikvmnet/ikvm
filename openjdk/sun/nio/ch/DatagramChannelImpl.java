/*
 * Copyright 2001-2006 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
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
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

package sun.nio.ch;

import java.io.FileDescriptor;
import java.io.IOException;
import java.net.*;
import java.nio.ByteBuffer;
import java.nio.channels.*;
import java.nio.channels.spi.*;
import java.lang.ref.SoftReference;


/**
 * An implementation of DatagramChannels.
 */

class DatagramChannelImpl
    extends DatagramChannel
    implements SelChImpl
{
    // Our file descriptor
    FileDescriptor fd = null;   

    // IDs of native threads doing reads and writes, for signalling
    private volatile long readerThread = 0;
    private volatile long writerThread = 0;

    // Cached InetAddress and port for unconnected DatagramChannels
    // used by receive0
    private InetAddress cachedSenderInetAddress = null;
    private int cachedSenderPort = 0;

    // Lock held by current reading or connecting thread
    private final Object readLock = new Object();

    // Lock held by current writing or connecting thread
    private final Object writeLock = new Object();

    // Lock held by any thread that modifies the state fields declared below
    // DO NOT invoke a blocking I/O operation while holding this lock!
    private final Object stateLock = new Object();

    // -- The following fields are protected by stateLock

    // State (does not necessarily increase monotonically)
    private static final int ST_UNINITIALIZED = -1;
    private static int ST_UNCONNECTED = 0;
    private static int ST_CONNECTED = 1;
    private static final int ST_KILLED = 2;
    private int state = ST_UNINITIALIZED;

    // Binding
    private SocketAddress localAddress = null;
    SocketAddress remoteAddress = null;

    // Options
    private SocketOpts.IP options = null;

    // Our socket adaptor, if any
    private DatagramSocket socket = null;

    // -- End of fields protected by stateLock


    public DatagramChannelImpl(SelectorProvider sp)
        throws IOException
    {
        super(sp);
        this.fd = Net.socket(false);
        this.state = ST_UNCONNECTED;
    }

    public DatagramChannelImpl(SelectorProvider sp, FileDescriptor fd)
        throws IOException
    {
        super(sp);
        this.fd = fd;
        this.state = ST_UNCONNECTED;
    }

    public DatagramSocket socket() {
        synchronized (stateLock) {
            if (socket == null)
                socket = DatagramSocketAdaptor.create(this);
            return socket;
        }
    }

    private void ensureOpen() throws ClosedChannelException {
        if (!isOpen())
            throw new ClosedChannelException();
    }

    private SocketAddress sender;       // Set by receive0 (## ugh)

    public SocketAddress receive(ByteBuffer dst) throws IOException {
        if (dst.isReadOnly())
            throw new IllegalArgumentException("Read-only buffer");
        if (dst == null)
            throw new NullPointerException();
        synchronized (readLock) {
            ensureOpen();
            // If socket is not bound then behave as if nothing received
            if (!isBound())             // ## NotYetBoundException ??
                return null;
            int n = 0;
            ByteBuffer bb = null;
            try {
                begin();
                if (!isOpen())
                    return null;
                SecurityManager security = System.getSecurityManager();
                readerThread = NativeThread.current();
                if (isConnected() || (security == null)) {
                    do {
                        n = receive0(dst);
                    } while ((n == IOStatus.INTERRUPTED) && isOpen());
                    if (n == IOStatus.UNAVAILABLE)
                        return null;
                } else {
                    bb = ByteBuffer.allocate(dst.remaining());
                    for (;;) {
                        do {
                            n = receive0(bb);
                        } while ((n == IOStatus.INTERRUPTED) && isOpen());
                        if (n == IOStatus.UNAVAILABLE)
                            return null;
                        InetSocketAddress isa = (InetSocketAddress)sender;
                        try {
                            security.checkAccept(
                                isa.getAddress().getHostAddress(),
                                isa.getPort());
                        } catch (SecurityException se) {
                            // Ignore packet
                            bb.clear();
                            n = 0;
                            continue;
                        }
                        bb.flip();
                        dst.put(bb);
                        break;
                    }
                }
                return sender;
            } finally {
                readerThread = 0;
                end((n > 0) || (n == IOStatus.UNAVAILABLE));
                assert IOStatus.check(n);
            }
        }
    }

    public int send(ByteBuffer src, SocketAddress target)
        throws IOException
    {
        if (src == null)
            throw new NullPointerException();

        synchronized (writeLock) {
            ensureOpen();
            InetSocketAddress isa = (InetSocketAddress)target;
            InetAddress ia = isa.getAddress();
            if (ia == null)
                throw new IOException("Target address not resolved");
            synchronized (stateLock) {
                if (!isConnected()) {
                    if (target == null)
                        throw new NullPointerException();
                    SecurityManager sm = System.getSecurityManager();
                    if (sm != null) {
                        if (ia.isMulticastAddress()) {
                            sm.checkMulticast(isa.getAddress());
                        } else {
                            sm.checkConnect(isa.getAddress().getHostAddress(),
                                            isa.getPort());
                        }
                    }
                } else { // Connected case; Check address then write
                    if (!target.equals(remoteAddress)) {
                        throw new IllegalArgumentException(
                            "Connected address not equal to target address");
                    }
                    return write(src);
                }
            }

            int n = 0;
            try {
                begin();
                if (!isOpen())
                    return 0;
                writerThread = NativeThread.current();
                do {
                    n = sendImpl(src, isa);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                writerThread = 0;
                end((n > 0) || (n == IOStatus.UNAVAILABLE));
                assert IOStatus.check(n);
            }
        }
    }

    public int read(ByteBuffer buf) throws IOException {
        if (buf == null)
            throw new NullPointerException();
        synchronized (readLock) {
            synchronized (stateLock) {
                ensureOpen();
                if (!isConnected())
                    throw new NotYetConnectedException();
            }
            int n = 0;
            try {
                begin();
                if (!isOpen())
                    return 0;
                readerThread = NativeThread.current();
                do {
                    n = readImpl(buf);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                readerThread = 0;
                end((n > 0) || (n == IOStatus.UNAVAILABLE));
                assert IOStatus.check(n);
            }
        }
    }

    private long read0(ByteBuffer[] bufs) throws IOException {
        if (bufs == null)
            throw new NullPointerException();
        synchronized (readLock) {
            synchronized (stateLock) {
                ensureOpen();
                if (!isConnected())
                    throw new NotYetConnectedException();
            }
            long n = 0;
            try {
                begin();
                if (!isOpen())
                    return 0;
                readerThread = NativeThread.current();
                do {
                    n = readImpl(bufs);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                readerThread = 0;
                end((n > 0) || (n == IOStatus.UNAVAILABLE));
                assert IOStatus.check(n);
            }
        }
    }

    public long read(ByteBuffer[] dsts, int offset, int length)
        throws IOException
    {
        if ((offset < 0) || (length < 0) || (offset > dsts.length - length))
           throw new IndexOutOfBoundsException();
        // ## Fix IOUtil.write so that we can avoid this array copy
        return read0(Util.subsequence(dsts, offset, length));
    }

    public int write(ByteBuffer buf) throws IOException {
        if (buf == null)
            throw new NullPointerException();
        synchronized (writeLock) {
            synchronized (stateLock) {
                ensureOpen();
                if (!isConnected())
                    throw new NotYetConnectedException();
            }
            int n = 0;
            try {
                begin();
                if (!isOpen())
                    return 0;
                writerThread = NativeThread.current();
                do {
                    n = writeImpl(buf);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                writerThread = 0;
                end((n > 0) || (n == IOStatus.UNAVAILABLE));
                assert IOStatus.check(n);
            }
        }
    }

    private long write0(ByteBuffer[] bufs) throws IOException {
        if (bufs == null)
            throw new NullPointerException();
        synchronized (writeLock) {
            synchronized (stateLock) {
                ensureOpen();
                if (!isConnected())
                    throw new NotYetConnectedException();
            }
            long n = 0;
            try {
                begin();
                if (!isOpen())
                    return 0;
                writerThread = NativeThread.current();
                do {
                    n = writeImpl(bufs);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                writerThread = 0;
                end((n > 0) || (n == IOStatus.UNAVAILABLE));
                assert IOStatus.check(n);
            }
        }
    }

    public long write(ByteBuffer[] srcs, int offset, int length)
        throws IOException
    {
        if ((offset < 0) || (length < 0) || (offset > srcs.length - length))
            throw new IndexOutOfBoundsException();
        // ## Fix IOUtil.write so that we can avoid this array copy
        return write0(Util.subsequence(srcs, offset, length));
    }

    protected void implConfigureBlocking(boolean block) throws IOException {
        IOUtil.configureBlocking(fd, block);
    }

    public SocketOpts options() {
        synchronized (stateLock) {
            if (options == null) {
                SocketOptsImpl.Dispatcher d
                    = new SocketOptsImpl.Dispatcher() {
                            int getInt(int opt) throws IOException {
                                return Net.getIntOption(fd, opt);
                            }
                            void setInt(int opt, int arg)
                                throws IOException
                            {
                                Net.setIntOption(fd, opt, arg);
                            }
                        };
                options = new SocketOptsImpl.IP(d);
            }
            return options;
        }
    }

    public boolean isBound() {
        return Net.localPortNumber(fd) != 0;
    }

    public SocketAddress localAddress() {
        synchronized (stateLock) {
            if (isConnected() && (localAddress == null)) {
                // Socket was not bound before connecting,
                // so ask what the address turned out to be
                localAddress = Net.localAddress(fd);
            }
            SecurityManager sm = System.getSecurityManager();
            if (sm != null) {
                InetSocketAddress isa = (InetSocketAddress)localAddress;
                sm.checkConnect(isa.getAddress().getHostAddress(), -1);
            }
            return localAddress;
        }
    }

    public SocketAddress remoteAddress() {
        synchronized (stateLock) {
            return remoteAddress;
        }
    }

    public void bind(SocketAddress local) throws IOException {
        synchronized (readLock) {
            synchronized (writeLock) {
                synchronized (stateLock) {
                    ensureOpen();
                    if (isBound())
                        throw new AlreadyBoundException();
                    InetSocketAddress isa = Net.checkAddress(local);
                    SecurityManager sm = System.getSecurityManager();
                    if (sm != null)
                        sm.checkListen(isa.getPort());
                    Net.bind(fd, isa.getAddress(), isa.getPort());
                    localAddress = Net.localAddress(fd);
                }
            }
        }
    }

    public boolean isConnected() {
        synchronized (stateLock) {
            return (state == ST_CONNECTED);
        }
    }

    void ensureOpenAndUnconnected() throws IOException { // package-private
        synchronized (stateLock) {
            if (!isOpen())
                throw new ClosedChannelException();
            if (state != ST_UNCONNECTED)
                throw new IllegalStateException("Connect already invoked");
        }
    }

    public DatagramChannel connect(SocketAddress sa) throws IOException {
        int trafficClass = 0;
        int localPort = 0;

        synchronized(readLock) {
            synchronized(writeLock) {
                synchronized (stateLock) {
                    ensureOpenAndUnconnected();
                    InetSocketAddress isa = Net.checkAddress(sa);
                    SecurityManager sm = System.getSecurityManager();
                    if (sm != null)
                        sm.checkConnect(isa.getAddress().getHostAddress(),
                                        isa.getPort());
                    // We simulate connectedness, so we don't call connect here,
                    // but if we're not yet bound, we should bind here.
                    if (!isBound())
                    {
                        socket().bind(null);
                    }
                    //int n = Net.connect(fd,
                    //                    isa.getAddress(),
                    //                    isa.getPort(),
                    //                    trafficClass);
                    //if (n <= 0)
                    //    throw new Error();    // Can't happen

                    // Connection succeeded; disallow further invocation
                    state = ST_CONNECTED;
                    remoteAddress = sa;
                    sender = isa;
                    cachedSenderInetAddress = isa.getAddress();
                    cachedSenderPort = isa.getPort();
                }
            }
        }
        return this;
    }

    public DatagramChannel disconnect() throws IOException {
        synchronized(readLock) {
            synchronized(writeLock) {
                synchronized (stateLock) {
                    if (!isConnected() || !isOpen())
                        return this;
                    InetSocketAddress isa = (InetSocketAddress)remoteAddress;
                    SecurityManager sm = System.getSecurityManager();
                    if (sm != null)
                        sm.checkConnect(isa.getAddress().getHostAddress(),
                                        isa.getPort());
                    disconnect0(fd);
                    remoteAddress = null;
                    state = ST_UNCONNECTED;
                }
            }
        }
        return this;
    }

    protected void implCloseSelectableChannel() throws IOException {
        synchronized (stateLock) {
            closeImpl();
            long th;
            if ((th = readerThread) != 0)
                NativeThread.signal(th);
            if ((th = writerThread) != 0)
                NativeThread.signal(th);
            if (!isRegistered())
                kill();
        }
    }

    public void kill() throws IOException {
        synchronized (stateLock) {
            if (state == ST_KILLED)
                return;
            if (state == ST_UNINITIALIZED) {
                state = ST_KILLED;
                return;
            }
            assert !isOpen() && !isRegistered();
            closeImpl();
            state = ST_KILLED;
        }
    }

    /**
     * Translates native poll revent set into a ready operation set
     */
    public boolean translateReadyOps(int ops, int initialOps,
                                     SelectionKeyImpl sk) {
        int intOps = sk.nioInterestOps(); // Do this just once, it synchronizes
        int oldOps = sk.nioReadyOps();
        int newOps = initialOps;

        if ((ops & PollArrayWrapper.POLLNVAL) != 0) {
            // This should only happen if this channel is pre-closed while a
            // selection operation is in progress
            // ## Throw an error if this channel has not been pre-closed
            return false;
        }

        if ((ops & (PollArrayWrapper.POLLERR
                    | PollArrayWrapper.POLLHUP)) != 0) {
            newOps = intOps;
            sk.nioReadyOps(newOps);
            return (newOps & ~oldOps) != 0;
        }

        if (((ops & PollArrayWrapper.POLLIN) != 0) &&
            ((intOps & SelectionKey.OP_READ) != 0))
            newOps |= SelectionKey.OP_READ;

        if (((ops & PollArrayWrapper.POLLOUT) != 0) &&
            ((intOps & SelectionKey.OP_WRITE) != 0))
            newOps |= SelectionKey.OP_WRITE;

        sk.nioReadyOps(newOps);
        return (newOps & ~oldOps) != 0;
    }

    public boolean translateAndUpdateReadyOps(int ops, SelectionKeyImpl sk) {
        return translateReadyOps(ops, sk.nioReadyOps(), sk);
    }

    public boolean translateAndSetReadyOps(int ops, SelectionKeyImpl sk) {
        return translateReadyOps(ops, 0, sk);
    }

    /**
     * Translates an interest operation set into a native poll event set
     */
    public void translateAndSetInterestOps(int ops, SelectionKeyImpl sk) {
        int newOps = 0;

        if ((ops & SelectionKey.OP_READ) != 0)
            newOps |= PollArrayWrapper.POLLIN;
        if ((ops & SelectionKey.OP_WRITE) != 0)
            newOps |= PollArrayWrapper.POLLOUT;
        if ((ops & SelectionKey.OP_CONNECT) != 0)
            newOps |= PollArrayWrapper.POLLIN;
        sk.selector.putEventOps(sk, newOps);
    }

    public FileDescriptor getFD() {
        return fd;
    }

    public int getFDVal() {
        throw new Error();
    }


    // -- Native methods --

    private void closeImpl() throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            fd.getSocket().Close();
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private static void disconnect0(FileDescriptor fd) throws IOException
    {
        // since we simulate connectedness, we don't need to do anything here
    }

    private int receive0(ByteBuffer bb) throws IOException
    {
        byte[] buf = new byte[bb.remaining()];
        cli.System.Net.EndPoint[] remoteEP = new cli.System.Net.EndPoint[] 
            {
                new cli.System.Net.IPEndPoint(0, 0)
            };
        InetSocketAddress addr;
        int length;
        do
        {
            for (; ; )
            {
                try
                {
                    if (false) throw new cli.System.Net.Sockets.SocketException();
                    if (false) throw new cli.System.ObjectDisposedException("");
                    length = fd.getSocket().ReceiveFrom(buf, 0, buf.length, cli.System.Net.Sockets.SocketFlags.wrap(cli.System.Net.Sockets.SocketFlags.None), remoteEP);
                    break;
                }
                catch (cli.System.Net.Sockets.SocketException x)
                {
                    if (x.get_ErrorCode() == Net.WSAECONNRESET)
                    {
                        // A previous send failed (i.e. the remote host responded with a ICMP that the port is closed) and
                        // the winsock stack helpfully lets us know this, but we don't care so we just retry the receive.
                        continue;
                    }
                    if (x.get_ErrorCode() == Net.WSAEMSGSIZE)
                    {
                        // The buffer size was too small for the packet, ReceiveFrom receives the part of the packet
                        // that fits in the buffer and then throws an exception, so we have to ignore the exception in this case.
                        length = buf.length;
                        break;
                    }
                    if (x.get_ErrorCode() == Net.WSAEWOULDBLOCK)
                    {
                        return IOStatus.UNAVAILABLE;
                    }
                    throw PlainSocketImpl.convertSocketExceptionToIOException(x);
                }
                catch (cli.System.ObjectDisposedException x1)
                {
                    throw new SocketException("Socket is closed");
                }
            }
            cli.System.Net.IPEndPoint ep = (cli.System.Net.IPEndPoint)remoteEP[0];
            addr = new InetSocketAddress(PlainSocketImpl.getInetAddressFromIPEndPoint(ep), ep.get_Port());
        } while (remoteAddress != null && !addr.equals(remoteAddress));
        sender = addr;
        bb.put(buf, 0, length);
        return length;
    }

    private int sendImpl(ByteBuffer bb, InetSocketAddress addr) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            int position = bb.position();
            byte[] buf;
            int offset;
            int length;
            if (bb.hasArray())
            {
                buf = bb.array();
                offset = bb.arrayOffset() + bb.position();
                length = bb.remaining();
            }
            else
            {
                buf = new byte[bb.remaining()];
                offset = 0;
                length = buf.length;
                bb.get(buf);
                bb.position(position);
            }
            int sent = fd.getSocket().SendTo(buf, offset, length, cli.System.Net.Sockets.SocketFlags.wrap(cli.System.Net.Sockets.SocketFlags.None), new cli.System.Net.IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(addr.getAddress()), addr.getPort()));
            if (bb.hasArray())
            {
                bb.position(position + sent);
            }
            else
            {
                bb.put(buf, 0, sent);
            }
            return sent;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            if (x.get_ErrorCode() == Net.WSAEWOULDBLOCK)
            {
                return IOStatus.UNAVAILABLE;
            }
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private int readImpl(ByteBuffer bb) throws IOException
    {
        return receive0(bb);
    }

    private long readImpl(ByteBuffer[] bb) throws IOException
    {
        // This is a rather lame implementation. On .NET 2.0 we could make this more
        // efficient by using the IList<ArraySegment<byte>> overload of Socket.Send()
        long size = 0;
        for (int i = 0; i < bb.length; i++)
        {
            size += bb[i].remaining();
        }
        // UDP has a maximum packet size of 64KB
        byte[] buf = new byte[(int)Math.min(65536, size)];
        int n = receive0(ByteBuffer.wrap(buf));
        if (n <= 0)
        {
            return n;
        }
        for (int i = 0, pos = 0; i < bb.length && pos < buf.length; i++)
        {
            int len = Math.min(bb[i].remaining(), buf.length - pos);
            bb[i].put(buf, pos, len);
            pos += len;
        }
        return n;
    }

    private int writeImpl(ByteBuffer bb) throws IOException
    {
        return sendImpl(bb, (InetSocketAddress)remoteAddress);
    }

    private long writeImpl(ByteBuffer[] bb) throws IOException
    {
        // This is a rather lame implementation. On .NET 2.0 we could make this more
        // efficient by using the IList<ArraySegment<byte>> overload of Socket.Send()
        long totalWritten = 0;
        for (int i = 0; i < bb.length; i++)
        {
            try
            {
                int len = writeImpl(bb[i]);
                if (len < 0)
                {
                    return totalWritten > 0 ? totalWritten : len;
                }
                totalWritten += len;
            }
            catch (IOException x)
            {
                if (totalWritten > 0)
                {
                    return totalWritten;
                }
                throw x;
            }
        }
        return totalWritten;
    }
}
