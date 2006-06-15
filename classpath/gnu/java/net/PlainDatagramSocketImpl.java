/* PlainDatagramSocketImpl.java -- Default DatagramSocket implementation
   Copyright (C) 1998, 1999, 2001 Free Software Foundation, Inc.

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.
 
GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
02111-1307 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */


package gnu.java.net;

import java.io.IOException;
import java.net.*;
import java.util.*;
import cli.System.Net.IPAddress;
import cli.System.Net.IPEndPoint;
import cli.System.Net.Sockets.SocketOptionName;
import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.MulticastOption;
import cli.System.Net.Sockets.SocketFlags;
import cli.System.Net.Sockets.SocketType;
import cli.System.Net.Sockets.ProtocolType;
import cli.System.Net.Sockets.AddressFamily;
import ikvm.lang.CIL;

/**
* This is the default socket implementation for datagram sockets.
* It makes native calls to C routines that implement BSD style
* SOCK_DGRAM sockets in the AF_INET family.
*
* @version 0.1
*
* @author Aaron M. Renn (arenn@urbanophile.com)
*/
public class PlainDatagramSocketImpl extends DatagramSocketImpl
{
    /*
     * Static Variables
     */

    /**
     * This is the actual underlying socket
     */
    private cli.System.Net.Sockets.Socket socket = new cli.System.Net.Sockets.Socket(
        AddressFamily.wrap(AddressFamily.InterNetwork), 
        SocketType.wrap(SocketType.Dgram),
        ProtocolType.wrap(ProtocolType.Udp));

    /*************************************************************************/

    /*
     * Constructors
     */

    /**
     * Default do nothing constructor
     */
    public PlainDatagramSocketImpl()
    {
    }

    /*************************************************************************/

    /*
     * Instance Methods
     */

    /**
     * Creates a new datagram socket
     *
     * @exception SocketException If an error occurs
     */
    protected void create() throws SocketException
    {
    }

    /*************************************************************************/

    /**
     * Closes the socket
     */
    protected void close()
    {
        if(socket != null)
        {
            socket.Close();
            socket = null;
        }
    }

    /*************************************************************************/

    /**
     * Binds this socket to a particular port and interface
     *
     * @param port The port to bind to
     * @param addr The address to bind to
     *
     * @exception SocketException If an error occurs
     */
    protected void bind(int port, InetAddress addr) throws SocketException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.Bind(new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(addr), port));
            localPort = ((IPEndPoint)socket.get_LocalEndPoint()).get_Port();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new BindException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * Sends a packet of data to a remote host
     *
     * @param packet The packet to send
     *
     * @exception IOException If an error occurs
     */
    protected void send(DatagramPacket packet) throws IOException
    {
        try		
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            int len = packet.getLength();
            int port = packet.getPort();
            if(port < 1 || port > 65535)
            {
                throw new SocketException("Invalid port");
            }
            if(socket.SendTo(packet.getData(), packet.getOffset(), len, SocketFlags.wrap(SocketFlags.None), new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(packet.getAddress()), port)) != len)
            {
                throw new SocketException("Not all data was sent");
            }
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * What does this method really do?
     */
    protected int peek(InetAddress addr) throws IOException
    {
        throw new IOException("Not Implemented Yet");
    }

    /*************************************************************************/

    /**
     * Receives a UDP packet from the network
     *
     * @param packet The packet to fill in with the data received
     *
     * @exception IOException IOException If an error occurs
     */
    protected void receive(DatagramPacket packet) throws IOException
    {
        byte[] data = packet.getData();
        cli.System.Net.EndPoint[] remoteEP = new cli.System.Net.EndPoint[] 
            {
                new cli.System.Net.IPEndPoint(0, 0)
            };
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            int length = socket.ReceiveFrom(data, packet.getOffset(), getDatagramPacketBufferLength(packet),
                            cli.System.Net.Sockets.SocketFlags.wrap(cli.System.Net.Sockets.SocketFlags.None), remoteEP);
            setDatagramPacketLength(packet, length);
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            // If the buffer size was too small for the packet, ReceiveFrom receives the part of the packet
            // that fits in the buffer and then throws an exception, so we have to ignore the exception in this case.
            if(x.get_ErrorCode() != 10040) //WSAEMSGSIZE
            {
                throw PlainSocketImpl.convertSocketExceptionToIOException(x);
            }
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
        int remoteIP = (int)((cli.System.Net.IPEndPoint)remoteEP[0]).get_Address().get_Address();
        byte[] ipv4 = new byte[] { (byte)remoteIP, (byte)(remoteIP >> 8), (byte)(remoteIP >> 16), (byte)(remoteIP >> 24) };
        InetAddress remoteAddress = InetAddress.getByAddress(ipv4);
        packet.setAddress(remoteAddress);
        packet.setPort(((cli.System.Net.IPEndPoint)remoteEP[0]).get_Port());
    }

    // these methods live in map.xml, because we need to access the package accessible fields in DatagramPacket
    private static native void setDatagramPacketLength(DatagramPacket packet, int length);
    private static native int getDatagramPacketBufferLength(DatagramPacket packet);

    /*************************************************************************/

    /**
     * Joins a multicast group
     *
     * @param addr The group to join
     *
     * @exception IOException If an error occurs
     */
    protected void join(InetAddress addr) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.AddMembership), new MulticastOption(new cli.System.Net.IPAddress(PlainSocketImpl.getAddressFromInetAddress(addr))));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ArgumentException x1)
        {
            throw new IOException(x1.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * Leaves a multicast group
     *
     * @param addr The group to leave
     *
     * @exception IOException If an error occurs
     */
    protected void leave(InetAddress addr) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.DropMembership), new MulticastOption(new cli.System.Net.IPAddress(PlainSocketImpl.getAddressFromInetAddress(addr))));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ArgumentException x1)
        {
            throw new IOException(x1.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * Gets the Time to Live value for the socket
     *
     * @return The TTL value
     *
     * @exception IOException If an error occurs
     */
    protected byte getTTL() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            return (byte)CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.IpTimeToLive)));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * Sets the Time to Live value for the socket
     *
     * @param ttl The new TTL value
     *
     * @exception IOException If an error occurs
     */
    protected void setTTL(byte ttl) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.IpTimeToLive), ttl & 0xff);
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * Gets the Time to Live value for the socket
     *
     * @return The TTL value
     *
     * @exception IOException If an error occurs
     */
    protected int getTimeToLive() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            return CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.IpTimeToLive)));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * Sets the Time to Live value for the socket
     *
     * @param ttl The new TTL value
     *
     * @exception IOException If an error occurs
     */
    protected void setTimeToLive(int ttl) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.IpTimeToLive), ttl);
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /*************************************************************************/

    /**
     * Retrieves the value of an option on the socket
     *
     * @param option_id The identifier of the option to retrieve
     *
     * @return The value of the option
     *
     * @exception SocketException If an error occurs
     */
    public Object getOption(int option_id) throws SocketException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            switch(option_id)
            {
                case SocketOptions.SO_REUSEADDR:
                    return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress))) != 0);
                case SocketOptions.SO_BROADCAST:
                    return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Broadcast))) != 0);
                case SocketOptions.IP_MULTICAST_IF:
                    return getInetAddressFromInt(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastInterface))));
                case SocketOptions.IP_MULTICAST_IF2:
                    throw new SocketException("SocketOptions.IP_MULTICAST_IF2 not implemented");
                case SocketOptions.IP_MULTICAST_LOOP:
                    return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastLoopback))) != 0);
                case SocketOptions.SO_TIMEOUT:
                    return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveTimeout))));
                default:
                    return PlainSocketImpl.getCommonSocketOption(socket, option_id);
            }
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    private static InetAddress getInetAddressFromInt(int i) throws SocketException
    {
        byte[] b = new byte[4];
        b[0] = (byte)(i >>  0);
        b[1] = (byte)(i >>  8);
        b[2] = (byte)(i >> 16);
        b[3] = (byte)(i >> 24);
        try
        {
            return InetAddress.getByAddress(b);
        }
        catch(UnknownHostException x)
        {
            throw new SocketException(x.getMessage());
        }
    }

    /*************************************************************************/

    /**
     * Sets the value of an option on the socket
     *
     * @param option_id The identifier of the option to set
     * @param val The value of the option to set
     *
     * @exception SocketException If an error occurs
     */
    public void setOption(int option_id, Object val) throws SocketException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            switch(option_id)
            {
                case SocketOptions.SO_REUSEADDR:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.SO_BROADCAST:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Broadcast), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.IP_MULTICAST_IF:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastInterface), (int)PlainSocketImpl.getAddressFromInetAddress((InetAddress)val));
                    break;
                case SocketOptions.IP_MULTICAST_IF2:
                    throw new SocketException("SocketOptions.IP_MULTICAST_IF2 not implemented");
                case SocketOptions.IP_MULTICAST_LOOP:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastLoopback), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.SO_TIMEOUT:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveTimeout), ((Integer)val).intValue());
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendTimeout), ((Integer)val).intValue());
                    break;
                default:
                    PlainSocketImpl.setCommonSocketOption(socket, option_id, val);
                    break;
            }
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    public int peekData(DatagramPacket packet)
    {
        // TODO
        throw new InternalError ("PlainDatagramSocketImpl::peekData is not implemented");
    }

    /**
     * Joins a multicast group
     *
     * @param address The group to join
     * @param netIf The interface to bind to
     *
     * @exception IOException If an error occurs
     */
    public void joinGroup(SocketAddress address, NetworkInterface netIf) throws IOException
    {
        try
        {
            if(!(address instanceof InetSocketAddress)) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");

            InetAddress inetAddr = ((InetSocketAddress)address).getAddress();
            IPAddress mcastAddr = new IPAddress(PlainSocketImpl.getAddressFromInetAddress(inetAddr));

            Enumeration e=netIf.getInetAddresses();
            if (e.hasMoreElements())
            {
                IPAddress bindAddr  = new IPAddress(PlainSocketImpl.getAddressFromInetAddress((InetAddress)e.nextElement()));
                MulticastOption mcastOption = new MulticastOption(mcastAddr, bindAddr);
                socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.AddMembership), mcastOption);
            }
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ArgumentException x1)
        {
            throw new IOException(x1.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    /**
     * Leaves a multicast group
     *
     * @param address The group to join
     * @param netIf The interface to bind to
     *
     * @exception IOException If an error occurs
     */
    public void leaveGroup(SocketAddress address, NetworkInterface netIf) throws IOException
    {
        try
        {
            if(!(address instanceof InetSocketAddress)) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");

            InetAddress inetAddr = ((InetSocketAddress)address).getAddress();
            IPAddress mcastAddr = new IPAddress(PlainSocketImpl.getAddressFromInetAddress(inetAddr));

            Enumeration e=netIf.getInetAddresses();
            if (e.hasMoreElements())
            {
                IPAddress bindAddr  = new IPAddress(PlainSocketImpl.getAddressFromInetAddress((InetAddress)e.nextElement()));
                MulticastOption mcastOption = new MulticastOption(mcastAddr, bindAddr);
                socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.DropMembership), mcastOption);
            }
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw PlainSocketImpl.convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ArgumentException x1)
        {
            throw new IOException(x1.getMessage());
        }
        catch(cli.System.ObjectDisposedException x2)
        {
            throw new SocketException("Socket is closed");
        }
    }

    public cli.System.Net.Sockets.Socket getSocket()
    {
        return socket;
    }
} // class PlainDatagramSocketImpl
