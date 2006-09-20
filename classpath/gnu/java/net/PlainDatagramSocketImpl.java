/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
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

public class PlainDatagramSocketImpl extends DatagramSocketImpl
{
    private cli.System.Net.Sockets.Socket socket;

    public PlainDatagramSocketImpl() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            socket = new cli.System.Net.Sockets.Socket(
                AddressFamily.wrap(AddressFamily.InterNetwork), 
                SocketType.wrap(SocketType.Dgram),
                ProtocolType.wrap(ProtocolType.Udp));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new SocketException(x.getMessage());
        }
    }

    protected void create() throws SocketException
    {
    }

    protected void close()
    {
        if(socket != null)
        {
            socket.Close();
            socket = null;
        }
    }

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

    public void send(DatagramPacket packet) throws IOException
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

    protected int peek(InetAddress addr) throws IOException
    {
        throw new IOException("Not Implemented Yet");
    }

    public void receive(DatagramPacket packet) throws IOException
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

    protected void join(InetAddress addr) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.AddMembership), new MulticastOption(PlainSocketImpl.getAddressFromInetAddress(addr)));
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

    protected void leave(InetAddress addr) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.DropMembership), new MulticastOption(PlainSocketImpl.getAddressFromInetAddress(addr)));
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

    public Object getOption(int option_id) throws SocketException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            switch(option_id)
            {
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

    public void setOption(int option_id, Object val) throws SocketException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            switch(option_id)
            {
                case SocketOptions.SO_BROADCAST:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Broadcast), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.IP_MULTICAST_IF:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastInterface), (int)PlainSocketImpl.getAddressFromInetAddress((InetAddress)val).get_Address());
                    break;
                case SocketOptions.IP_MULTICAST_IF2:
                    throw new SocketException("SocketOptions.IP_MULTICAST_IF2 not implemented");
                case SocketOptions.IP_MULTICAST_LOOP:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.MulticastLoopback), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.SO_TIMEOUT:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveTimeout), ((Integer)val).intValue());
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
        throw new InternalError("PlainDatagramSocketImpl::peekData is not implemented");
    }

    public void joinGroup(SocketAddress address, NetworkInterface netIf) throws IOException
    {
        try
        {
            if(!(address instanceof InetSocketAddress)) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");

            if(((InetSocketAddress)address).isUnresolved())
            {
                throw new UnknownHostException(((InetSocketAddress)address).getHostName());
            }

            InetAddress inetAddr = ((InetSocketAddress)address).getAddress();
            IPAddress mcastAddr = PlainSocketImpl.getAddressFromInetAddress(inetAddr);

            Enumeration e = netIf.getInetAddresses();
            if(e.hasMoreElements())
            {
                IPAddress bindAddr  = PlainSocketImpl.getAddressFromInetAddress((InetAddress)e.nextElement());
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

    public void leaveGroup(SocketAddress address, NetworkInterface netIf) throws IOException
    {
        try
        {
            if(!(address instanceof InetSocketAddress)) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");

            if(((InetSocketAddress)address).isUnresolved())
            {
                throw new UnknownHostException(((InetSocketAddress)address).getHostName());
            }

            InetAddress inetAddr = ((InetSocketAddress)address).getAddress();
            IPAddress mcastAddr = PlainSocketImpl.getAddressFromInetAddress(inetAddr);

            Enumeration e = netIf.getInetAddresses();
            if(e.hasMoreElements())
            {
                IPAddress bindAddr  = PlainSocketImpl.getAddressFromInetAddress((InetAddress)e.nextElement());
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
}
