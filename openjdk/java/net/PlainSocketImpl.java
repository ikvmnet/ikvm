/*
 * Copyright 1995-2006 Sun Microsystems, Inc.  All Rights Reserved.
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

package java.net;

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
import ikvm.lang.CIL;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.InterruptedIOException;
import java.io.FileDescriptor;
import java.io.ByteArrayOutputStream;

import sun.net.ConnectionResetException;

/**
 * Default Socket Implementation. This implementation does
 * not implement any security checks.
 * Note this class should <b>NOT</b> be public.
 *
 * @author  Steven B. Byrne
 * @version 1.73, 05/05/07
 */
@ikvm.lang.Internal
public class PlainSocketImpl extends SocketImpl
{
    // Winsock Error Codes
    public static final int WSAEWOULDBLOCK = 10035;
    private static final int WSAEADDRINUSE = 10048;
    private static final int WSAENETUNREACH = 10051;
    public static final int WSAESHUTDOWN = 10058;
    private static final int WSAETIMEDOUT = 10060;
    private static final int WSAECONNREFUSED = 10061;
    private static final int WSAEHOSTUNREACH = 10065;
    private static final int WSAHOST_NOT_FOUND = 11001;

    public static IOException convertSocketExceptionToIOException(cli.System.Net.Sockets.SocketException x) throws IOException
    {
        switch (x.get_ErrorCode())
        {
            case WSAEADDRINUSE:
                return new BindException(x.getMessage());
            case WSAENETUNREACH:
            case WSAEHOSTUNREACH:
                return new NoRouteToHostException(x.getMessage());
            case WSAETIMEDOUT:
                return new SocketTimeoutException(x.getMessage());
            case WSAECONNREFUSED:
                return new PortUnreachableException(x.getMessage());
            case WSAHOST_NOT_FOUND:
                return new UnknownHostException(x.getMessage());
            default:
                return new SocketException(x.getMessage() + "\nError Code: " + x.get_ErrorCode());
        }
    }

    public static IPAddress getAddressFromInetAddress(InetAddress addr)
    {
        byte[] b = addr.getAddress();
        if (b.length == 16)
        {
            // FXBUG in .NET 1.1 you can only construct IPv6 addresses (not IPv4) with this constructor
            // (according to the documentation this was fixed in .NET 2.0)
            return new IPAddress(b);
        }
        else
        {
            return new IPAddress((((b[3] & 0xff) << 24) + ((b[2] & 0xff) << 16) + ((b[1] & 0xff) << 8) + (b[0] & 0xff)) & 0xffffffffL);
        }
    }

    public static InetAddress getInetAddressFromIPEndPoint(IPEndPoint endpoint)
    {
        try
        {
            return InetAddress.getByAddress(endpoint.get_Address().GetAddressBytes());
        }
        catch (UnknownHostException x)
        {
            // this exception only happens if the address byte array is of invalid length, which cannot happen unless
            // the .NET socket returns a bogus address
            throw (InternalError)new InternalError().initCause(x);
        }
    }

    static void setCommonSocketOption(cli.System.Net.Sockets.Socket netSocket, int cmd, boolean on, Object value) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            switch (cmd)
            {
                case SocketOptions.SO_REUSEADDR:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress), on ? 1 : 0);
                    break;
                case SocketOptions.SO_SNDBUF:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendBuffer), ((Integer)value).intValue());
                    break;
                case SocketOptions.SO_RCVBUF:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveBuffer), ((Integer)value).intValue());
                    break;
                case SocketOptions.IP_TOS:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.TypeOfService), ((Integer)value).intValue());
                    break;
                case SocketOptions.SO_BINDADDR: // read-only
                default:
                    throw new SocketException("Invalid socket option: " + cmd);
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    static int getCommonSocketOption(cli.System.Net.Sockets.Socket netSocket, int opt, Object iaContainerObj) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            switch (opt)
            {
                case SocketOptions.SO_REUSEADDR:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress))) == 0 ? -1 : 1;
                case SocketOptions.SO_SNDBUF:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendBuffer)));
                case SocketOptions.SO_RCVBUF:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveBuffer)));
                case SocketOptions.IP_TOS:
                    // TODO handle IPv6 here
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.TypeOfService)));
                case SocketOptions.SO_BINDADDR:
                    ((InetAddressContainer)iaContainerObj).addr = getInetAddressFromIPEndPoint((IPEndPoint)netSocket.get_LocalEndPoint());
                    return 0;
                default:
                    throw new SocketException("Invalid socket option: " + opt);
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private cli.System.Net.Sockets.Socket netSocket;
    /* instance variable for SO_TIMEOUT */
    int timeout;   // timeout in millisec
    // traffic class
    private int trafficClass;

    private boolean shut_rd = false;
    private boolean shut_wr = false;
    
    private SocketInputStream socketInputStream = null;

    /* number of threads using the FileDescriptor */
    private int fdUseCount = 0;

    /* lock when increment/decrementing fdUseCount */
    private Object fdLock = new Object();

    /* indicates a close is pending on the file descriptor */
    private boolean closePending = false;

    /* indicates connection reset state */
    private int CONNECTION_NOT_RESET = 0;
    private int CONNECTION_RESET_PENDING = 1;
    private int CONNECTION_RESET = 2;
    private int resetState;
    private Object resetLock = new Object();

    /* second fd, used for ipv6 on windows only.
     * fd1 is used for listeners and for client sockets at initialization
     * until the socket is connected. Up to this point fd always refers
     * to the ipv4 socket and fd1 to the ipv6 socket. After the socket
     * becomes connected, fd always refers to the connected socket 
     * (either v4 or v6) and fd1 is closed.
     *
     * For ServerSockets, fd always refers to the v4 listener and 
     * fd1 the v6 listener.
     */
    private FileDescriptor fd1;
    /*
     * Needed for ipv6 on windows because we need to know
     * if the socket is bound to ::0 or 0.0.0.0, when a caller
     * asks for it. Otherwise we don't know which socket to ask.
     */
    private InetAddress anyLocalBoundAddr=null;
 
    /* to prevent starvation when listening on two sockets, this is
     * is used to hold the id of the last socket we accepted on.
     */
    private int lastfd = -1;

    /**
     * Constructs an empty instance.
     */
    PlainSocketImpl() { }

    /**
     * Creates a socket with a boolean that specifies whether this
     * is a stream socket (true) or an unconnected UDP socket (false).
     */
    protected synchronized void create(boolean stream) throws IOException {
        fd = new FileDescriptor();
        fd1 = new FileDescriptor();
        socketCreate(stream);
        if (socket != null)
            socket.setCreated();
        if (serverSocket != null)
            serverSocket.setCreated();
    }

    /**
     * Creates a socket and connects it to the specified port on
     * the specified host.
     * @param host the specified host
     * @param port the specified port
     */
    protected void connect(String host, int port)
        throws UnknownHostException, IOException
    {
        IOException pending = null;
        try {
            InetAddress address = InetAddress.getByName(host);

            try {
                connectToAddress(address, port, timeout);
                return;
            } catch (IOException e) {
                pending = e;
            }
        } catch (UnknownHostException e) {
            pending = e;
        }

        // everything failed
        close();
        throw pending;
    }

    /**
     * Creates a socket and connects it to the specified address on
     * the specified port.
     * @param address the address
     * @param port the specified port
     */
    protected void connect(InetAddress address, int port) throws IOException {
        this.port = port;
        this.address = address;

        try {
            connectToAddress(address, port, timeout);
            return;
        } catch (IOException e) {
            // everything failed
            close();
            throw e;
        }
    }

    /**
     * Creates a socket and connects it to the specified address on
     * the specified port.
     * @param address the address
     * @param timeout the timeout value in milliseconds, or zero for no timeout.
     * @throws IOException if connection fails
     * @throws  IllegalArgumentException if address is null or is a
     *          SocketAddress subclass not supported by this socket
     * @since 1.4
     */
    protected void connect(SocketAddress address, int timeout) throws IOException {
        if (address == null || !(address instanceof InetSocketAddress))
            throw new IllegalArgumentException("unsupported address type");
        InetSocketAddress addr = (InetSocketAddress) address;
        if (addr.isUnresolved())
            throw new UnknownHostException(addr.getHostName());
        this.port = addr.getPort();
        this.address = addr.getAddress();

        try {
            connectToAddress(this.address, port, timeout);
            return;
        } catch (IOException e) {
            // everything failed
            close();
            throw e;
        }
    }

    private void connectToAddress(InetAddress address, int port, int timeout) throws IOException {
        if (address.isAnyLocalAddress()) {
            doConnect(InetAddress.getLocalHost(), port, timeout);
        } else {
            doConnect(address, port, timeout);
        }
    }

    public void setOption(int opt, Object val) throws SocketException {
        if (isClosedOrPending()) {
            throw new SocketException("Socket Closed");
        }
        boolean on = true;
        switch (opt) {
            /* check type safety b4 going native.  These should never
             * fail, since only java.Socket* has access to
             * PlainSocketImpl.setOption().
             */
        case SO_LINGER:
            if (val == null || (!(val instanceof Integer) && !(val instanceof Boolean)))
                throw new SocketException("Bad parameter for option");
            if (val instanceof Boolean) {
                /* true only if disabling - enabling should be Integer */
                on = false;
            }
            break;
        case SO_TIMEOUT:
            if (val == null || (!(val instanceof Integer)))
                throw new SocketException("Bad parameter for SO_TIMEOUT");
            int tmp = ((Integer) val).intValue();
            if (tmp < 0)
                throw new IllegalArgumentException("timeout < 0");
            timeout = tmp;
            break;
        case IP_TOS:
             if (val == null || !(val instanceof Integer)) {
                 throw new SocketException("bad argument for IP_TOS");
             }
             trafficClass = ((Integer)val).intValue();
             break;
        case SO_BINDADDR:
            throw new SocketException("Cannot re-bind socket");
        case TCP_NODELAY:
            if (val == null || !(val instanceof Boolean))
                throw new SocketException("bad parameter for TCP_NODELAY");
            on = ((Boolean)val).booleanValue();
            break;
        case SO_SNDBUF:
        case SO_RCVBUF:
            if (val == null || !(val instanceof Integer) ||
                !(((Integer)val).intValue() > 0)) {
                throw new SocketException("bad parameter for SO_SNDBUF " +
                                          "or SO_RCVBUF");
            }
            break;
        case SO_KEEPALIVE:
            if (val == null || !(val instanceof Boolean))
                throw new SocketException("bad parameter for SO_KEEPALIVE");
            on = ((Boolean)val).booleanValue();
            break;
        case SO_OOBINLINE:
            if (val == null || !(val instanceof Boolean))
                throw new SocketException("bad parameter for SO_OOBINLINE");
            on = ((Boolean)val).booleanValue();
            break;
        case SO_REUSEADDR:
            if (val == null || !(val instanceof Boolean)) 
                throw new SocketException("bad parameter for SO_REUSEADDR");
            on = ((Boolean)val).booleanValue();
            break;
        default:
            throw new SocketException("unrecognized TCP option: " + opt);
        }
        socketSetOption(opt, on, val);
    }
    public Object getOption(int opt) throws SocketException {
        if (isClosedOrPending()) {
            throw new SocketException("Socket Closed");
        }
        if (opt == SO_TIMEOUT) {
            return new Integer(timeout);
        }
        int ret = 0;
        /*
         * The native socketGetOption() knows about 3 options.
         * The 32 bit value it returns will be interpreted according
         * to what we're asking.  A return of -1 means it understands
         * the option but its turned off.  It will raise a SocketException
         * if "opt" isn't one it understands.
         */

        switch (opt) {
        case TCP_NODELAY:
            ret = socketGetOption(opt, null);
            return Boolean.valueOf(ret != -1);
        case SO_OOBINLINE:
            ret = socketGetOption(opt, null);
            return Boolean.valueOf(ret != -1);
        case SO_LINGER:
            ret = socketGetOption(opt, null);
            return (ret == -1) ? Boolean.FALSE: (Object)(new Integer(ret));
        case SO_REUSEADDR:
            ret = socketGetOption(opt, null);
            return Boolean.valueOf(ret != -1);
        case SO_BINDADDR:
            if (fd != null && fd1 != null ) {
                /* must be unbound or else bound to anyLocal */
                return anyLocalBoundAddr;
            }
            InetAddressContainer in = new InetAddressContainer();
            ret = socketGetOption(opt, in); 
            return in.addr;
        case SO_SNDBUF:
        case SO_RCVBUF:
            ret = socketGetOption(opt, null);
            return new Integer(ret);
        case IP_TOS:
            ret = socketGetOption(opt, null);
            if (ret == -1) { // ipv6 tos
                return new Integer(trafficClass);
            } else {
                return new Integer(ret);
            }
        case SO_KEEPALIVE:
            ret = socketGetOption(opt, null);
            return Boolean.valueOf(ret != -1);
        // should never get here
        default:
            return null;
        }
    }

    /**
     * The workhorse of the connection operation.  Tries several times to
     * establish a connection to the given <host, port>.  If unsuccessful,
     * throws an IOException indicating what went wrong.
     */

    private synchronized void doConnect(InetAddress address, int port, int timeout) throws IOException {
        try {
            FileDescriptor fd = acquireFD();
            try {
                socketConnect(address, port, timeout);
                // If we have a ref. to the Socket, then sets the flags
                // created, bound & connected to true.
                // This is normally done in Socket.connect() but some
                // subclasses of Socket may call impl.connect() directly!
                if (socket != null) {
                    socket.setBound();
                    socket.setConnected();
                }
            } finally {
                releaseFD();
            }
        } catch (IOException e) {       
            close();
            throw e;
        }
    }

    /**
     * Binds the socket to the specified address of the specified local port.
     * @param address the address
     * @param port the port
     */
    protected synchronized void bind(InetAddress address, int lport)
        throws IOException
    {
        socketBind(address, lport);
        if (socket != null)
            socket.setBound();
        if (serverSocket != null)
            serverSocket.setBound();
        if (address.isAnyLocalAddress()) {
            anyLocalBoundAddr = address;
        }
    }

    /**
     * Listens, for a specified amount of time, for connections.
     * @param count the amount of time to listen for connections
     */
    protected synchronized void listen(int count) throws IOException {
        socketListen(count);
    }

    /**
     * Accepts connections.
     * @param s the connection
     */
    protected synchronized void accept(SocketImpl s) throws IOException {
        FileDescriptor fd = acquireFD();
        try {
            socketAccept(s);
        } finally {
            releaseFD();
        }
    }

    void setFileDescriptor(FileDescriptor fd) {
        this.netSocket = fd.getSocket();
    }

    void setAddress(InetAddress address) {
        this.address = address;
    }

    void setPort(int port) {
        this.port = port;
    }

    void setLocalPort(int localPort) {
        this.localport = localPort;
    }

    /**
     * Gets an InputStream for this socket.
     */
    protected synchronized InputStream getInputStream() throws IOException {
        if (isClosedOrPending()) {
            throw new IOException("Socket Closed");
        }
        if (shut_rd) {
            throw new IOException("Socket input is shutdown");
        }
        if (socketInputStream == null) {
            socketInputStream = new SocketInputStream(this);
        }
        return socketInputStream;
    }

    void setInputStream(SocketInputStream in) {
        socketInputStream = in;
    }

    /**
     * Gets an OutputStream for this socket.
     */
    protected synchronized OutputStream getOutputStream() throws IOException {
        if (isClosedOrPending()) {
            throw new IOException("Socket Closed");
        }
        if (shut_wr) {
            throw new IOException("Socket output is shutdown");
        }
        return new SocketOutputStream(this);
    }

    /**
     * Returns the number of bytes that can be read without blocking.
     */
    protected synchronized int available() throws IOException {
        if (isClosedOrPending()) {
            throw new IOException("Stream closed.");
        }

        /*
         * If connection has been reset then return 0 to indicate
         * there are no buffered bytes.
         */
        if (isConnectionReset()) {
            return 0;
        }

        /*
         * If no bytes available and we were previously notified
         * of a connection reset then we move to the reset state.
         *
         * If are notified of a connection reset then check
         * again if there are bytes buffered on the socket. 
         */
        int n = 0;
        try { 
            n = socketAvailable();
            if (n == 0 && isConnectionResetPending()) {
                setConnectionReset();
            }
        } catch (ConnectionResetException exc1) {
            setConnectionResetPending();
            try {
                n = socketAvailable();
                if (n == 0) {
                    setConnectionReset();
                }
            } catch (ConnectionResetException exc2) {
            }
        }
        return n;
    }

    /**
     * Closes the socket.
     */
    protected void close() throws IOException {
        synchronized(fdLock) {
            if (fd != null || fd1 != null) {
                if (fdUseCount == 0) {
                    if (closePending) {
                        return;
                    }
                    closePending = true;
                    /*
                     * We close the FileDescriptor in two-steps - first the
                     * "pre-close" which closes the socket but doesn't
                     * release the underlying file descriptor. This operation
                     * may be lengthy due to untransmitted data and a long
                     * linger interval. Once the pre-close is done we do the
                     * actual socket to release the fd.
                     */
                    try {
                        socketPreClose();
                    } finally {
                        socketClose();
                    }
                    fd = null;
                    fd1 = null;
                    return;
                } else {
                    /*
                     * If a thread has acquired the fd and a close
                     * isn't pending then use a deferred close.
                     * Also decrement fdUseCount to signal the last
                     * thread that releases the fd to close it.
                     */
                    if (!closePending) {
                        closePending = true;
                        fdUseCount--;
                        socketPreClose();
                    }
                }
            }
        }
    }
    
    void reset() throws IOException {
        if (fd != null || fd1 != null) {
            socketClose();
        }
        fd = null;
        fd1 = null;
        super.reset();
    }


    /**
     * Shutdown read-half of the socket connection;
     */
    protected void shutdownInput() throws IOException {
      if (fd != null) {
          socketShutdown(SHUT_RD);
          if (socketInputStream != null) {
              socketInputStream.setEOF(true);
          }
          shut_rd = true;
      }
    } 

    /**
     * Shutdown write-half of the socket connection;
     */
    protected void shutdownOutput() throws IOException {
      if (fd != null) {
          socketShutdown(SHUT_WR);
          shut_wr = true;
      }
    } 

    protected boolean supportsUrgentData () {
        return true;
    }

    protected void sendUrgentData (int data) throws IOException {
        if (fd == null) {
            throw new IOException("Socket Closed");
        }
        socketSendUrgentData (data);
    }

    /*
     * "Acquires" and returns the FileDescriptor for this impl
     *
     * A corresponding releaseFD is required to "release" the
     * FileDescriptor.
     */
    public final FileDescriptor acquireFD() {
        synchronized (fdLock) {
            fdUseCount++;
            return fd;
        }
    }

    /*
     * "Release" the FileDescriptor for this impl. 
     *
     * If the use count goes to -1 then the socket is closed.
     */
    public final void releaseFD() {
        synchronized (fdLock) {
            fdUseCount--;
            if (fdUseCount == -1) {
                if (fd != null) {
                    try {
                        socketClose();
                    } catch (IOException e) { 
                    } finally {
                        fd = null;
                    }
                }
            }
        }
    }

    public boolean isConnectionReset() {
        synchronized (resetLock) {
            return (resetState == CONNECTION_RESET);
        }
    }

    public boolean isConnectionResetPending() {
        synchronized (resetLock) {
            return (resetState == CONNECTION_RESET_PENDING);
        }
    }

    public void setConnectionReset() {
        synchronized (resetLock) {
            resetState = CONNECTION_RESET;
        }
    }

    public void setConnectionResetPending() {
        synchronized (resetLock) {
            if (resetState == CONNECTION_NOT_RESET) {
                resetState = CONNECTION_RESET_PENDING;
            }
        }

    }

    /*
     * Return true if already closed or close is pending
     */
    public boolean isClosedOrPending() {
        /*
         * Lock on fdLock to ensure that we wait if a
         * close is in progress.
         */
        synchronized (fdLock) {
            if (closePending || (fd == null && fd1 == null)) {
                return true;
            } else {
                return false;
            }
        }
    }

    /*
     * Return the current value of SO_TIMEOUT
     */
    public int getTimeout() {
        return timeout;
    }

    /*
     * "Pre-close" a socket by dup'ing the file descriptor - this enables
     * the socket to be closed without releasing the file descriptor.
     */
    private void socketPreClose() throws IOException {
        socketClose0(true);
    }

    /*
     * Close the socket (and release the file descriptor).
     */
    private void socketClose() throws IOException {
        socketClose0(false);
    }

    private void socketCreate(boolean stream) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            if (stream)
            {
                netSocket = new cli.System.Net.Sockets.Socket(AddressFamily.wrap(AddressFamily.InterNetwork), SocketType.wrap(SocketType.Stream), ProtocolType.wrap(ProtocolType.Tcp));
            }
            else
            {
                netSocket = new cli.System.Net.Sockets.Socket(AddressFamily.wrap(AddressFamily.InterNetwork), SocketType.wrap(SocketType.Dgram), ProtocolType.wrap(ProtocolType.Udp));
            }
            fd1 = null;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketConnect(InetAddress address, int port, int timeout) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            IPEndPoint ep = new IPEndPoint(getAddressFromInetAddress(address), port);
            if (timeout <= 0)
            {
                netSocket.Connect(ep);
            }
            else
            {
                cli.System.IAsyncResult result = netSocket.BeginConnect(ep, null, null);
                if (!result.get_AsyncWaitHandle().WaitOne(timeout, false))
                {
                    netSocket.Close();
                    throw new SocketTimeoutException();
                }
                netSocket.EndConnect(result);
            }
            this.address = address;
            this.port = port;
            if (this.localport == 0)
            {
                this.localport = ((IPEndPoint)netSocket.get_LocalEndPoint()).get_Port();
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new ConnectException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketBind(InetAddress address, int localport) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            netSocket.Bind(new IPEndPoint(getAddressFromInetAddress(address), localport));
            this.address = address;
            if (localport == 0)
            {
                this.localport = ((IPEndPoint)netSocket.get_LocalEndPoint()).get_Port();
            }
            else
            {
                this.localport = localport;
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new BindException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketListen(int count) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            netSocket.Listen(count);
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketAccept(SocketImpl s) throws IOException
    {
        PlainSocketImpl impl = (PlainSocketImpl)s;
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            if (timeout > 0 && !netSocket.Poll(Math.min(timeout, Integer.MAX_VALUE / 1000) * 1000,
                SelectMode.wrap(SelectMode.SelectRead)))
            {
                throw new SocketTimeoutException("Accept timed out");
            }
            cli.System.Net.Sockets.Socket accept = netSocket.Accept();
            impl.netSocket = accept;
            IPEndPoint remoteEndPoint = ((IPEndPoint)accept.get_RemoteEndPoint());
            impl.address = getInetAddressFromIPEndPoint(remoteEndPoint);
            impl.port = remoteEndPoint.get_Port();
            impl.localport = ((IPEndPoint)accept.get_LocalEndPoint()).get_Port();
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            // TODO we may have to throw java.io.InterruptedIOException here
            throw new SocketException("Socket is closed");
        }
    }

    private int socketAvailable() throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return netSocket.get_Available();
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketClose0(boolean useDeferredClose) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            if (netSocket != null)
            {
                netSocket.Close();
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketShutdown(int howto) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            netSocket.Shutdown(SocketShutdown.wrap(howto == SHUT_RD ? SocketShutdown.Receive : SocketShutdown.Send));
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketSetOption(int cmd, boolean on, Object value) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            switch (cmd)
            {
                case SocketOptions.SO_LINGER:
                    if (on)
                    {
                        netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Linger), new LingerOption(true, ((Integer)value).intValue()));
                    }
                    else
                    {
                        netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Linger), new LingerOption(false, 0));
                    }
                    break;
                case SocketOptions.SO_TIMEOUT:
                    if (serverSocket == null)
                    {
                        netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveTimeout), timeout <= 5000 ? 0 : timeout);
                    }
                    break;
                case SocketOptions.TCP_NODELAY:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Tcp), SocketOptionName.wrap(SocketOptionName.NoDelay), on ? 1 : 0);
                    break;
                case SocketOptions.SO_KEEPALIVE:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.KeepAlive), on ? 1 : 0);
                    break;
                case SocketOptions.SO_OOBINLINE:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.OutOfBandInline), on ? 1 : 0);
                    break;
                default:
                    setCommonSocketOption(netSocket, cmd, on, value);
                    break;
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private int socketGetOption(int opt, Object iaContainerObj) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            switch (opt)
            {
                case SocketOptions.TCP_NODELAY:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Tcp), SocketOptionName.wrap(SocketOptionName.NoDelay))) == 0 ? -1 : 1;
                case SocketOptions.SO_KEEPALIVE:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.KeepAlive))) == 0 ? -1 : 1;
                case SocketOptions.SO_LINGER:
                    {
                        LingerOption linger = (LingerOption)netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Linger));
                        if (linger.get_Enabled())
                        {
                            return linger.get_LingerTime();
                        }
                        return -1;
                    }
                case SocketOptions.SO_OOBINLINE:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.OutOfBandInline))) == 0 ? -1 : 1;
                default:
                    return getCommonSocketOption(netSocket, opt, iaContainerObj);
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private void socketSendUrgentData(int data) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            byte[] oob = { (byte)data };
            netSocket.Send(oob, SocketFlags.wrap(SocketFlags.OutOfBand));
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // used by SocketInputStream
    int read(byte[] buf, int offset, int len, int timeout) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            if (timeout > 0 && !netSocket.Poll(Math.min(timeout, Integer.MAX_VALUE / 1000) * 1000,
                SelectMode.wrap(SelectMode.SelectRead)))
            {
                throw new SocketTimeoutException();
            }
            int read = netSocket.Receive(buf, offset, len, SocketFlags.wrap(SocketFlags.None));
            return read == 0 ? -1 : read;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            if (x.get_ErrorCode() == WSAESHUTDOWN)
            {
                // the socket was shutdown, so we have to return EOF
                return -1;
            }
            else if (x.get_ErrorCode() == WSAEWOULDBLOCK)
            {
                // nothing to read and would block
                return 0;
            }
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // used by SocketOutputStream
    int write(byte[] buf, int offset, int len) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return netSocket.Send(buf, offset, len, SocketFlags.wrap(SocketFlags.None));
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    public final static int SHUT_RD = 0;
    public final static int SHUT_WR = 1;
}

class InetAddressContainer {
    InetAddress addr;
}
