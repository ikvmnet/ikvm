/*
 * Copyright 2000-2006 Sun Microsystems, Inc.  All Rights Reserved.
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

import cli.System.Net.IPAddress;
import cli.System.Net.IPEndPoint;
import cli.System.Net.Sockets.LingerOption;
import cli.System.Net.Sockets.SelectMode;
import cli.System.Net.Sockets.SocketOptionName;
import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.SocketFlags;
import cli.System.Net.Sockets.SocketType;
import cli.System.Net.Sockets.ProtocolType;
import cli.System.Net.Sockets.AddressFamily;
import cli.System.Net.Sockets.SocketShutdown;
import java.io.FileDescriptor;
import java.io.IOException;
import java.lang.reflect.*;
import java.net.*;
import java.nio.channels.*;
import java.nio.channels.spi.*;
import java.security.AccessController;
import java.security.PrivilegedAction;
import java.util.HashSet;
import java.util.Iterator;


/**
 * An implementation of ServerSocketChannels
 */

class ServerSocketChannelImpl
    extends ServerSocketChannel
    implements SelChImpl
{
    private final cli.System.Net.Sockets.Socket netSocket;

    // ID of native thread currently blocked in this channel, for signalling
    private volatile long thread = 0;

    // Lock held by thread currently blocked in this channel
    private final Object lock = new Object();

    // Lock held by any thread that modifies the state fields declared below
    // DO NOT invoke a blocking I/O operation while holding this lock!
    private final Object stateLock = new Object();

    // -- The following fields are protected by stateLock

    // Channel state, increases monotonically
    private static final int ST_UNINITIALIZED = -1;
    private static final int ST_INUSE = 0;
    private static final int ST_KILLED = 1;
    private int state = ST_UNINITIALIZED;
    
    // Binding
    private SocketAddress localAddress = null; // null => unbound

    // Options, created on demand
    private SocketOpts.IP.TCP options = null;

    // Our socket adaptor, if any
    ServerSocket socket;

    // -- End of fields protected by stateLock


    public ServerSocketChannelImpl(SelectorProvider sp) throws IOException {
	super(sp);
	this.state = ST_INUSE;
	try
	{
	    if (false) throw new cli.System.Net.Sockets.SocketException();
	    netSocket = new cli.System.Net.Sockets.Socket(AddressFamily.wrap(AddressFamily.InterNetwork), SocketType.wrap(SocketType.Stream), ProtocolType.wrap(ProtocolType.Tcp));
	}
	catch (cli.System.Net.Sockets.SocketException x)
	{
	    throw PlainSocketImpl.convertSocketExceptionToIOException(x);
	}
    }

    public ServerSocket socket() {
	synchronized (stateLock) {
	    if (socket == null)
		socket = ServerSocketAdaptor.create(this);
	    return socket;
	}
    }

    public boolean isBound() {
	synchronized (stateLock) {
	    return localAddress != null;
	}
    }

    public SocketAddress localAddress() {
	synchronized (stateLock) {
	    return localAddress;
	}
    }

    public void bind(SocketAddress local, int backlog) throws IOException {
	synchronized (lock) {
	    if (!isOpen())
		throw new ClosedChannelException();
	    if (isBound())
		throw new AlreadyBoundException();
	    InetSocketAddress isa = Net.checkAddress(local);
	    SecurityManager sm = System.getSecurityManager();
	    if (sm != null)
		sm.checkListen(isa.getPort());
	    bindImpl(isa.getAddress(), isa.getPort());
	    listen(backlog < 1 ? 50 : backlog);
	    synchronized (stateLock) {
		IPEndPoint ep = (IPEndPoint)netSocket.get_LocalEndPoint();
		localAddress = new InetSocketAddress(PlainSocketImpl.getInetAddressFromIPEndPoint(ep), ep.get_Port());
	    }
	}
    }

    public SocketChannel accept() throws IOException {
	synchronized (lock) {
	    if (!isOpen())
		throw new ClosedChannelException();
	    if (!isBound())
		throw new NotYetBoundException();
	    SocketChannel sc = null;

	    int n = 0;
	    InetSocketAddress[] isaa = new InetSocketAddress[1];
	    cli.System.Net.Sockets.Socket[] accsock = new cli.System.Net.Sockets.Socket[1];

	    try {
		begin();
		if (!isOpen())
		    return null;
		thread = NativeThread.current();
		for (;;) {
		    n = accept0(accsock, isaa);
		    if ((n == IOStatus.INTERRUPTED) && isOpen())
			continue;
		    break;
		}
	    } finally {
		thread = 0;
		end(n > 0);
		assert IOStatus.check(n);
	    }

	    if (n < 1)
		return null;

	    accsock[0].set_Blocking(true);
	    InetSocketAddress isa = isaa[0];
	    sc = new SocketChannelImpl(provider(), accsock[0], isa);
	    SecurityManager sm = System.getSecurityManager();
	    if (sm != null) {
		try {
		    sm.checkAccept(isa.getAddress().getHostAddress(),
				   isa.getPort());
		} catch (SecurityException x) {
		    sc.close();
		    throw x;
		}
	    }
	    return sc;

	}
    }

    protected void implConfigureBlocking(boolean block) throws IOException {
	try
	{
	    if (false) throw new cli.System.Net.Sockets.SocketException();
	    if (false) throw new cli.System.ObjectDisposedException("");
	    netSocket.set_Blocking(block);
	}
	catch (cli.System.Net.Sockets.SocketException x)
	{
	    throw PlainSocketImpl.convertSocketExceptionToIOException(x);
	}
	catch (cli.System.ObjectDisposedException _)
	{
	    throw new SocketException("Socket is closed");
	}
    }

    public SocketOpts options() {
	synchronized (stateLock) {
	    if (options == null) {
		SocketOptsImpl.Dispatcher d
		    = new SocketOptsImpl.Dispatcher() {
			    int getInt(int opt) throws IOException {
				// TODO
				throw new Error("TODO");
			    }
			    void setInt(int opt, int arg) throws IOException {
				// TODO
				throw new Error("TODO");
			    }
			};
		options = new SocketOptsImpl.IP.TCP(d);
	    }
	    return options;
	}
    }

    protected void implCloseSelectableChannel() throws IOException {
	synchronized (stateLock) {
	    closeImpl();
	    long th = thread;
	    if (th != 0)
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
            ((intOps & SelectionKey.OP_ACCEPT) != 0))
                newOps |= SelectionKey.OP_ACCEPT;

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

        // Translate ops
        if ((ops & SelectionKey.OP_ACCEPT) != 0)
            newOps |= PollArrayWrapper.POLLIN;
        // Place ops into pollfd array
        sk.selector.putEventOps(sk, newOps);
    }

    public FileDescriptor getFD() {
	FileDescriptor fd = new FileDescriptor();
	fd.setSocket(netSocket);
	return fd;
    }

    public int getFDVal() {
	throw new Error();
    }

    public String toString() {
	StringBuffer sb = new StringBuffer();
	sb.append(this.getClass().getName());
	sb.append('[');
	if (!isOpen())
	    sb.append("closed");
	else {
	    synchronized (stateLock) {
		if (localAddress() == null) {
		    sb.append("unbound");
		} else {
		    sb.append(localAddress().toString());
		}
	    }
	}
	sb.append(']');
	return sb.toString();
    }

    // -- Native methods --

    private void listen(int backlog) throws IOException
    {
	try
	{
	    if (false) throw new cli.System.Net.Sockets.SocketException();
	    if (false) throw new cli.System.ObjectDisposedException("");
	    netSocket.Listen(backlog);
	}
	catch (cli.System.Net.Sockets.SocketException x)
	{
	    throw PlainSocketImpl.convertSocketExceptionToIOException(x);
	}
	catch (cli.System.ObjectDisposedException _)
	{
	    throw new SocketException("Socket is closed");
	}
    }

    // Accepts a new connection, setting the given file descriptor to refer to
    // the new socket and setting isaa[0] to the socket's remote address.
    // Returns 1 on success, or IOStatus.UNAVAILABLE (if non-blocking and no
    // connections are pending) or IOStatus.INTERRUPTED.
    //
    private int accept0(cli.System.Net.Sockets.Socket[] accsock, InetSocketAddress[] isaa) throws IOException
    {
	try
	{
	    if (false) throw new cli.System.Net.Sockets.SocketException();
	    if (false) throw new cli.System.ObjectDisposedException("");
	    if (netSocket.get_Blocking() || netSocket.Poll(0, SelectMode.wrap(SelectMode.SelectRead)))
	    {
		accsock[0] = netSocket.Accept();
		IPEndPoint ep = (IPEndPoint)accsock[0].get_RemoteEndPoint();
		isaa[0] = new InetSocketAddress(PlainSocketImpl.getInetAddressFromIPEndPoint(ep), ep.get_Port());
		return 1;
	    }
	    else
	    {
		return IOStatus.UNAVAILABLE;
	    }
	}
	catch (cli.System.Net.Sockets.SocketException x)
	{
	    throw PlainSocketImpl.convertSocketExceptionToIOException(x);
	}
	catch (cli.System.ObjectDisposedException _)
	{
	    throw new SocketException("Socket is closed");
	}
    }

    private void bindImpl(InetAddress addr, int port) throws IOException
    {
	try
	{
	    if (false) throw new cli.System.Net.Sockets.SocketException();
	    if (false) throw new cli.System.ObjectDisposedException("");
	    netSocket.Bind(new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(addr), port));
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

    private void closeImpl() throws IOException
    {
	try
	{
	    if (false) throw new cli.System.Net.Sockets.SocketException();
	    if (false) throw new cli.System.ObjectDisposedException("");
	    netSocket.Close();
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
}
