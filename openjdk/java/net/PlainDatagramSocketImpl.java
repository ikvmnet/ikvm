/*
 * Copyright 1996-2003 Sun Microsystems, Inc.  All Rights Reserved.
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
import cli.System.Net.Sockets.SelectMode;
import cli.System.Net.Sockets.SocketOptionName;
import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.MulticastOption;
import cli.System.Net.Sockets.SocketFlags;
import cli.System.Net.Sockets.SocketType;
import cli.System.Net.Sockets.ProtocolType;
import cli.System.Net.Sockets.AddressFamily;
import ikvm.lang.CIL;
import java.io.FileDescriptor;
import java.io.IOException;
import java.io.InterruptedIOException;
import java.util.Enumeration;

/**
 * Concrete datagram and multicast socket implementation base class.
 * Note: This is not a public class, so that applets cannot call
 * into the implementation directly and hence cannot bypass the
 * security checks present in the DatagramSocket and MulticastSocket
 * classes.
 *
 * @author Pavani Diwanji
 */

class PlainDatagramSocketImpl extends DatagramSocketImpl
{
    // Windows 2000 introduced a "feature" that causes it to return WSAECONNRESET from receive,
    // if a previous send resulted in an ICMP port unreachable. We disable this feature by using
    // this ioctl.
    private static final int IOC_IN = (int)0x80000000;
    private static final int IOC_VENDOR = 0x18000000;
    private static final int SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;

    // Winsock Error Codes
    private static final int WSAEMSGSIZE = 10040;
    private static final int WSAECONNRESET = 10054;

    private cli.System.Net.Sockets.Socket netSocket;
    /* timeout value for receive() */
    private int timeout = 0;
    private int trafficClass = 0;
    private boolean connected = false;
    private InetAddress connectedAddress = null;
    private int connectedPort = -1;

    /* cached socket options */
    private int multicastInterface = 0;
    private boolean loopbackMode = true;
    private int ttl = -1;

    /* Used for IPv6 on Windows only */
    private FileDescriptor fd1;
    private int fduse=-1; /* saved between peek() and receive() calls */

    /* saved between successive calls to receive, if data is detected
     * on both sockets at same time. To ensure that one socket is not
     * starved, they rotate using this field
     */
    private int lastfd=-1; 

    /*
     * Needed for ipv6 on windows because we need to know
     * if the socket was bound to ::0 or 0.0.0.0, when a caller
     * asks for it. In this case, both sockets are used, but we
     * don't know whether the caller requested ::0 or 0.0.0.0
     * and need to remember it here.
     */
    private InetAddress anyLocalBoundAddr=null;

    /**
     * Creates a datagram socket
     */
    protected synchronized void create() throws SocketException {
        fd = new FileDescriptor();
        fd1 = new FileDescriptor();
        datagramSocketCreate();
    }

    /**
     * Binds a datagram socket to a local port.
     */
    protected synchronized void bind(int lport, InetAddress laddr) 
        throws SocketException {
        
        bind0(lport, laddr);
        if (laddr.isAnyLocalAddress()) {
            anyLocalBoundAddr = laddr;
        }
    }

    protected synchronized void bind0(int lport, InetAddress laddr) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            netSocket.Bind(new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(laddr), lport));
            localPort = ((IPEndPoint)netSocket.get_LocalEndPoint()).get_Port();
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

    /**
     * Sends a datagram packet. The packet contains the data and the
     * destination address to send the packet to.
     * @param packet to be sent.
     */
    protected void send(DatagramPacket p) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            int len = p.getLength();
            int port = p.getPort();
            if (port < 1 || port > 65535)
            {
                throw new SocketException("Invalid port");
            }
            if (netSocket.SendTo(p.getData(), p.getOffset(), len, SocketFlags.wrap(SocketFlags.None), new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(p.getAddress()), port)) != len)
            {
                throw new SocketException("Not all data was sent");
            }
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

    /**
     * Connects a datagram socket to a remote destination. This associates the remote
     * address with the local socket so that datagrams may only be sent to this destination
     * and received from this destination.
     * @param address the remote InetAddress to connect to
     * @param port the remote port number
     */
    protected void connect(InetAddress address, int port) throws SocketException {
        connect0(address, port);
        connectedAddress = address;
        connectedPort = port;
        connected = true;
    }

    /**
     * Disconnects a previously connected socket. Does nothing if the socket was
     * not connected already.
     */
    protected void disconnect() {
        disconnect0(connectedAddress.family);
        connected = false;
        connectedAddress = null;
        connectedPort = -1;
    }

    /**
     * Peek at the packet to see who it is from.
     * @param return the address which the packet came from.
     */
    protected synchronized int peek(InetAddress i) throws IOException
    {
        DatagramPacket p = new DatagramPacket(new byte[1], 1);
        receiveImpl(p, SocketFlags.Peek);
        i.address = p.getAddress().address;
        i.family = InetAddress.IPv4;
        return p.getPort();
    }

    protected synchronized int peekData(DatagramPacket p) throws IOException
    {
        receiveImpl(p, SocketFlags.Peek);
        return p.getPort();
    }

    /**
     * Receive the datagram packet.
     * @param Packet Received.
     */
    protected synchronized void receive(DatagramPacket p) 
        throws IOException {
        try {
            receive0(p);
        } finally {
            fduse = -1;
        }
    }

    protected synchronized void receive0(DatagramPacket p) throws IOException
    {
        receiveImpl(p, SocketFlags.None);
    }

    private void receiveImpl(DatagramPacket p, int socketFlags) throws IOException
    {
        cli.System.Net.EndPoint[] remoteEP = new cli.System.Net.EndPoint[] 
            {
                new cli.System.Net.IPEndPoint(0, 0)
            };
        int length;
        for (; ; )
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
                length = netSocket.ReceiveFrom(p.buf, p.offset, p.bufLength, SocketFlags.wrap(socketFlags), remoteEP);
                break;
            }
            catch (cli.System.Net.Sockets.SocketException x)
            {
                if (x.get_ErrorCode() == WSAECONNRESET)
                {
                    // A previous send failed (i.e. the remote host responded with a ICMP that the port is closed) and
                    // the winsock stack helpfully lets us know this, but we only care about this when we're connected,
                    // otherwise we'll simply retry the receive (note that we use SIO_UDP_CONNRESET to prevent these
                    // WSAECONNRESET exceptions, but when switching from connected to disconnected, some can slip through).
                    if ((socketFlags & SocketFlags.Peek) != 0)
                    {
                        // We did a peek, so we still need to remove the error result.
                        try
                        {
                            if (false) throw new cli.System.Net.Sockets.SocketException();
                            if (false) throw new cli.System.ObjectDisposedException("");
                            netSocket.ReceiveFrom(p.buf, 0, 0, SocketFlags.wrap(SocketFlags.None), remoteEP);
                        }
                        catch (cli.System.Net.Sockets.SocketException _)
                        {
                        }
                        catch (cli.System.ObjectDisposedException _)
                        {
                        }
                    }
                    if (connected)
                    {
                        throw new PortUnreachableException("ICMP Port Unreachable");
                    }
                    continue;
                }
                if (x.get_ErrorCode() == WSAEMSGSIZE)
                {
                    // The buffer size was too small for the packet, ReceiveFrom receives the part of the packet
                    // that fits in the buffer and then throws an exception, so we have to ignore the exception in this case.
                    length = p.bufLength;
                    break;
                }
                throw PlainSocketImpl.convertSocketExceptionToIOException(x);
            }
            catch (cli.System.ObjectDisposedException x1)
            {
                throw new SocketException("Socket is closed");
            }
        }
        IPEndPoint endpoint = (IPEndPoint)remoteEP[0];
        p.address = PlainSocketImpl.getInetAddressFromIPEndPoint(endpoint);
        p.port = endpoint.get_Port();
        p.length = length;
    }

    /**
     * Set the TTL (time-to-live) option.
     * @param TTL to be set.
     */
    protected void setTimeToLive(int ttl) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.IpTimeToLive), ttl);
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /**
     * Get the TTL (time-to-live) option.
     */
    protected int getTimeToLive() throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.IpTimeToLive)));
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch (cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /**
     * Set the TTL (time-to-live) option.
     * @param TTL to be set.
     */
    protected void setTTL(byte ttl) throws IOException
    {
        setTimeToLive(ttl & 0xFF);
    }

    /**
     * Get the TTL (time-to-live) option.
     */
    protected byte getTTL() throws IOException
    {
        return (byte)getTimeToLive();
    }

    /**
     * Join the multicast group.
     * @param multicast address to join.
     */
    protected void join(InetAddress inetaddr) throws IOException {
        join(inetaddr, null);
    }

    /**
     * Leave the multicast group.
     * @param multicast address to leave.
     */
    protected void leave(InetAddress inetaddr) throws IOException {
        leave(inetaddr, null);
    }
    /**
     * Join the multicast group.
     * @param multicast address to join.
     * @param netIf specifies the local interface to receive multicast
     *        datagram packets
     * @throws  IllegalArgumentException if mcastaddr is null or is a
     *          SocketAddress subclass not supported by this socket
     * @since 1.4
     */

    protected void joinGroup(SocketAddress mcastaddr, NetworkInterface netIf)
        throws IOException {
        if (mcastaddr == null || !(mcastaddr instanceof InetSocketAddress))
            throw new IllegalArgumentException("Unsupported address type");
        join(((InetSocketAddress)mcastaddr).getAddress(), netIf);
    }

    private void join(InetAddress inetaddr, NetworkInterface netIf) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.ObjectDisposedException("");
            IPAddress mcastAddr = PlainSocketImpl.getAddressFromInetAddress(inetaddr);
            if (netIf == null)
            {
                netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.AddMembership), new MulticastOption(mcastAddr));
            }
            else
            {
                Enumeration e = netIf.getInetAddresses();
                if (e.hasMoreElements())
                {
                    IPAddress bindAddr = PlainSocketImpl.getAddressFromInetAddress((InetAddress)e.nextElement());
                    MulticastOption mcastOption = new MulticastOption(mcastAddr, bindAddr);
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.AddMembership), mcastOption);
                }
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ArgumentException x1)
        {
            throw new IOException(x1.getMessage());
        }
        catch (cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /**
     * Leave the multicast group.
     * @param multicast address to leave.
     * @param netIf specified the local interface to leave the group at
     * @throws  IllegalArgumentException if mcastaddr is null or is a
     *          SocketAddress subclass not supported by this socket
     * @since 1.4
     */
    protected void leaveGroup(SocketAddress mcastaddr, NetworkInterface netIf)
        throws IOException {
        if (mcastaddr == null || !(mcastaddr instanceof InetSocketAddress))
            throw new IllegalArgumentException("Unsupported address type");
        leave(((InetSocketAddress)mcastaddr).getAddress(), netIf);
    }

    private void leave(InetAddress inetaddr, NetworkInterface netIf) throws IOException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.ObjectDisposedException("");
            IPAddress mcastAddr = PlainSocketImpl.getAddressFromInetAddress(inetaddr);
            if (netIf == null)
            {
                netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.DropMembership), new MulticastOption(mcastAddr));
            }
            else
            {
                Enumeration e = netIf.getInetAddresses();
                if (e.hasMoreElements())
                {
                    IPAddress bindAddr = PlainSocketImpl.getAddressFromInetAddress((InetAddress)e.nextElement());
                    MulticastOption mcastOption = new MulticastOption(mcastAddr, bindAddr);
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.DropMembership), mcastOption);
                }
            }
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch (cli.System.ArgumentException x1)
        {
            throw new IOException(x1.getMessage());
        }
        catch (cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /**
     * Close the socket.
     */
    protected void close() {
        if (fd != null || fd1 != null) {
            datagramSocketClose();
            fd = null;
            fd1 = null;
        }
    }

    /**
     * set a value - since we only support (setting) binary options
     * here, o must be a Boolean
     */

     public void setOption(int optID, Object o) throws SocketException {
         if (fd == null && fd1 == null) {
            throw new SocketException("Socket Closed");
         }
         switch (optID) {
            /* check type safety b4 going native.  These should never
             * fail, since only java.Socket* has access to
             * PlainSocketImpl.setOption().
             */
         case SO_TIMEOUT:
             if (o == null || !(o instanceof Integer)) {
                 throw new SocketException("bad argument for SO_TIMEOUT");
             }
             int tmp = ((Integer) o).intValue();
             if (tmp < 0)
                 throw new IllegalArgumentException("timeout < 0");
             timeout = tmp;
             return;
         case IP_TOS:
             if (o == null || !(o instanceof Integer)) {
                 throw new SocketException("bad argument for IP_TOS");
             }
             trafficClass = ((Integer)o).intValue();
             break;
         case SO_REUSEADDR:
             if (o == null || !(o instanceof Boolean)) {
                 throw new SocketException("bad argument for SO_REUSEADDR");
             }
             break;
         case SO_BROADCAST:
             if (o == null || !(o instanceof Boolean)) {
                 throw new SocketException("bad argument for SO_BROADCAST");
             }
             break;
         case SO_BINDADDR:
             throw new SocketException("Cannot re-bind Socket");
         case SO_RCVBUF:
         case SO_SNDBUF:
             if (o == null || !(o instanceof Integer) ||
                 ((Integer)o).intValue() < 0) {
                 throw new SocketException("bad argument for SO_SNDBUF or " +
                                           "SO_RCVBUF");
             }
             break;
         case IP_MULTICAST_IF:
             if (o == null || !(o instanceof InetAddress))
                 throw new SocketException("bad argument for IP_MULTICAST_IF");
             break;
         case IP_MULTICAST_IF2:
             if (o == null || !(o instanceof NetworkInterface))
                 throw new SocketException("bad argument for IP_MULTICAST_IF2");
             break;
         case IP_MULTICAST_LOOP:
             if (o == null || !(o instanceof Boolean))
                 throw new SocketException("bad argument for IP_MULTICAST_LOOP");
             break;
         default:
             throw new SocketException("invalid option: " + optID);
         }
         socketSetOption(optID, o);
     }

    /*
     * get option's state - set or not
     */

    public Object getOption(int optID) throws SocketException {
        if (fd == null && fd1 == null) {
            throw new SocketException("Socket Closed");
        }

        Object result;

        switch (optID) {
            case SO_TIMEOUT:
                result = new Integer(timeout);
                break;
        
            case IP_TOS:
                result = socketGetOption(optID);
                if ( ((Integer)result).intValue() == -1) {
                    result = new Integer(trafficClass);
                }
                break;

            case SO_BINDADDR:
                if (fd != null && fd1 != null) {
                    return anyLocalBoundAddr;   
                }
                /* fall through */
            case IP_MULTICAST_IF:
            case IP_MULTICAST_IF2:
            case SO_RCVBUF:
            case SO_SNDBUF:
            case IP_MULTICAST_LOOP:
            case SO_REUSEADDR:
            case SO_BROADCAST:
                result = socketGetOption(optID);
                break;

            default:
                throw new SocketException("invalid option: " + optID);
        }

        return result;
    }

    private void datagramSocketCreate() throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            netSocket = new cli.System.Net.Sockets.Socket(
                AddressFamily.wrap(AddressFamily.InterNetwork),
                SocketType.wrap(SocketType.Dgram),
                ProtocolType.wrap(ProtocolType.Udp));
            netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Broadcast), 1);
            netSocket.IOControl(SIO_UDP_CONNRESET, new byte[] { 0 }, null);
            fd1 = null;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
    }

    private void datagramSocketClose()
    {
        netSocket.Close();
    }

    private void socketSetOption(int opt, Object val) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            switch (opt)
            {
                case SocketOptions.SO_BROADCAST:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Broadcast), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.IP_MULTICAST_IF:
                    {
                        InetAddress addr = (InetAddress)val;
                        netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastInterface), (int)PlainSocketImpl.getAddressFromInetAddress(addr).get_Address());
                        break;
                    }
                case SocketOptions.IP_MULTICAST_IF2:
                    {
                        NetworkInterface netIf = (NetworkInterface)val;
                        Enumeration e = netIf.getInetAddresses();
                        while (e.hasMoreElements())
                        {
                            InetAddress addr = (InetAddress)e.nextElement();
                            if (addr.getAddress().length == 4)
                            {
                                netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastInterface), (int)PlainSocketImpl.getAddressFromInetAddress(addr).get_Address());
                                return;
                            }
                        }
                        throw new SocketException("No IPv4 address found on interface");
                    }
                case SocketOptions.IP_MULTICAST_LOOP:
                    netSocket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastLoopback), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.SO_REUSEADDR:
                    PlainSocketImpl.setCommonSocketOption(netSocket, opt, ((Boolean)val).booleanValue(), null);
                    break;
                default:
                    PlainSocketImpl.setCommonSocketOption(netSocket, opt, false, val);
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
    
    private static InetAddress getInetAddressFromInt(int addr) throws SocketException
    {
        try
        {
            return InetAddress.getByAddress(cli.System.BitConverter.GetBytes(addr));
        }
        catch (UnknownHostException x)
        {
            throw new SocketException(x.getMessage());
        }
    }

    private Object socketGetOption(int opt) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            switch (opt)
            {
                case SocketOptions.SO_BROADCAST:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Broadcast))) != 0;
                case SocketOptions.IP_MULTICAST_IF:
                    return getInetAddressFromInt(CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastInterface))));
                case SocketOptions.IP_MULTICAST_IF2:
                    {
                        NetworkInterface inf = NetworkInterface.getByInetAddress(getInetAddressFromInt(CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastInterface)))));
                        return inf != null ? inf : new NetworkInterface(null, -1, new InetAddress[] { new Inet4Address() });
                    }
                case SocketOptions.IP_MULTICAST_LOOP:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastLoopback))) != 0;
                case SocketOptions.SO_REUSEADDR:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress))) != 0;
                case SocketOptions.SO_SNDBUF:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendBuffer)));
                case SocketOptions.SO_RCVBUF:
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveBuffer)));
                case SocketOptions.IP_TOS:
                    // TODO handle IPv6 here
                    return CIL.unbox_int(netSocket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.TypeOfService)));
                case SocketOptions.SO_BINDADDR:
                    return PlainSocketImpl.getInetAddressFromIPEndPoint((IPEndPoint)netSocket.get_LocalEndPoint());
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

    private void connect0(InetAddress address, int port) throws SocketException
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            IPEndPoint ep = new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(address), port);
            // NOTE we use async connect to work around the issue that the .NET Socket class disallows sync Connect after the socket has received WSAECONNRESET
            netSocket.EndConnect(netSocket.BeginConnect(ep, null, null));
            netSocket.IOControl(SIO_UDP_CONNRESET, new byte[] { 1 }, null);
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

    private void disconnect0(int family)
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            // NOTE we use async connect to work around the issue that the .NET Socket class disallows sync Connect after the socket has received WSAECONNRESET
            netSocket.EndConnect(netSocket.BeginConnect(new IPEndPoint(IPAddress.Any, 0), null, null));
            netSocket.IOControl(SIO_UDP_CONNRESET, new byte[] { 0 }, null);
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
        }
        catch (cli.System.ObjectDisposedException x1)
        {
        }
    }
}
