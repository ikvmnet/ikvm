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

import java.io.InputStream;
import java.io.OutputStream;
import java.io.IOException;
import java.net.*;
import cli.System.Net.IPAddress;
import cli.System.Net.IPEndPoint;
import cli.System.Net.Sockets.SelectMode;
import cli.System.Net.Sockets.SocketOptionName;
import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.SocketFlags;
import cli.System.Net.Sockets.SocketType;
import cli.System.Net.Sockets.ProtocolType;
import cli.System.Net.Sockets.AddressFamily;
import cli.System.Net.Sockets.SocketShutdown;
import ikvm.lang.CIL;

public final class PlainSocketImpl extends SocketImpl
{
    // Winsock Error Codes
    private static final int WSAEWOULDBLOCK    = 10035;
    private static final int WSAEADDRINUSE     = 10048;
    private static final int WSAENETUNREACH    = 10051;
    private static final int WSAESHUTDOWN      = 10058;
    private static final int WSAETIMEDOUT      = 10060;
    private static final int WSAECONNREFUSED   = 10061;
    private static final int WSAEHOSTUNREACH   = 10065;
    private static final int WSAHOST_NOT_FOUND = 11001;

    static IOException convertSocketExceptionToIOException(cli.System.Net.Sockets.SocketException x) throws IOException
    {
        switch(x.get_ErrorCode())
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

    private cli.System.Net.Sockets.Socket socket;
    private int timeout;
    private cli.System.IAsyncResult asyncConnect;
    private InetSocketAddress asyncAddress;

    public PlainSocketImpl()
    {
    }

    // public for use by ServerSocketChannelImpl
    public void accept(SocketImpl _impl) throws IOException
    {
        PlainSocketImpl impl = (PlainSocketImpl)_impl;
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            if(timeout > 0 && !socket.Poll(Math.min(timeout, Integer.MAX_VALUE / 1000) * 1000,
                SelectMode.wrap(SelectMode.SelectRead)))
            {
                throw new SocketTimeoutException("Accept timed out");
            }
            cli.System.Net.Sockets.Socket accept = socket.Accept();
            impl.socket = accept;
            IPEndPoint remoteEndPoint = ((IPEndPoint)accept.get_RemoteEndPoint());
            impl.address = getInetAddressFromIPEndPoint(remoteEndPoint);
            impl.port = remoteEndPoint.get_Port();
            impl.localport = ((IPEndPoint)accept.get_LocalEndPoint()).get_Port();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // for use by ServerSocketChannelImpl
    public boolean pollAccept() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            return socket.Poll(0, SelectMode.wrap(SelectMode.SelectRead));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }        
    }

    // public for use by SocketChannelImpl
    public int available() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            return socket.get_Available();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    protected void bind(InetAddress addr, int port) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.Bind(new IPEndPoint(getAddressFromInetAddress(addr), port));
            this.address = addr;
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

    static IPAddress getAddressFromInetAddress(InetAddress addr)
    {
        byte[] b = addr.getAddress();
        if (b.length == 16)
        {
            // FXBUG in .NET 1.1 you can only construct IPv6 addresses with this constructor
            // (according to the documentation this was fixed in .NET 2.0)
            return new IPAddress(b);
        }
        else
        {
            return new IPAddress((((b[3] & 0xff) << 24) + ((b[2] & 0xff) << 16) + ((b[1] & 0xff) << 8) + (b[0] & 0xff)) & 0xffffffffL);
        }
    }

    // public for use by SocketChannelImpl
    public void close() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            // if this socket was created by a failed accept(), socket may be null
            if (socket != null)
            {
                socket.Close();
            }
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // public for use by SocketChannelImpl
    public void connect(InetAddress addr, int port) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.Connect(new IPEndPoint(getAddressFromInetAddress(addr), port));
            this.address = addr;
            this.port = port;
            this.localport = ((IPEndPoint)socket.get_LocalEndPoint()).get_Port();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new ConnectException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // for use by SocketChannelImpl
    public void beginConnect(InetSocketAddress address) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            asyncConnect = socket.BeginConnect(new IPEndPoint(getAddressFromInetAddress(address.getAddress()), address.getPort()), null, null);
            asyncAddress = address;
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new ConnectException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // for use by SocketChannelImpl
    public void endConnect() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            cli.System.IAsyncResult res = asyncConnect;
            asyncConnect = null;
            socket.EndConnect(res);
            this.address = asyncAddress.getAddress();
            this.port = asyncAddress.getPort();
            this.localport = ((IPEndPoint)socket.get_LocalEndPoint()).get_Port();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new ConnectException(x.getMessage());
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // for use by SocketChannelImpl
    public boolean isConnectFinished()
    {
        return asyncConnect.get_IsCompleted();
    }

    protected void connect(String hostname, int port) throws IOException
    {
        connect(InetAddress.getByName(hostname), port);
    }

    // public for use by SocketChannelImpl
    public void create(boolean stream) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            if(stream)
            {
                socket = new cli.System.Net.Sockets.Socket(AddressFamily.wrap(AddressFamily.InterNetwork), SocketType.wrap(SocketType.Stream), ProtocolType.wrap(ProtocolType.Tcp));
            }
            else
            {
                socket = new cli.System.Net.Sockets.Socket(AddressFamily.wrap(AddressFamily.InterNetwork), SocketType.wrap(SocketType.Dgram), ProtocolType.wrap(ProtocolType.Udp));
            }
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    protected void listen(int queuelen) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.Listen(queuelen);
            localport = ((IPEndPoint)socket.get_LocalEndPoint()).get_Port();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    // public for use by SocketChannelImpl
    public int read(byte[] buf, int offset, int len) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            if(timeout > 0 && !socket.Poll(Math.min(timeout, Integer.MAX_VALUE / 1000) * 1000,
                SelectMode.wrap(SelectMode.SelectRead)))
            {
                throw new SocketTimeoutException();
            }
            return socket.Receive(buf, offset, len, SocketFlags.wrap(SocketFlags.None));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            if(x.get_ErrorCode() == WSAESHUTDOWN)
            {
                // the socket was shutdown, so we have to return EOF
                return -1;
            }
            else if(x.get_ErrorCode() == WSAEWOULDBLOCK)
            {
                // nothing to read and would block
                return 0;
            }
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    protected void write(byte[] buf, int offset, int len) throws IOException
    {
        writeImpl(buf, offset, len);
    }

    // public for use by SocketChannelImpl
    public int writeImpl(byte[] buf, int offset, int len) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            return socket.Send(buf, offset, len, SocketFlags.wrap(SocketFlags.None));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
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
                case SocketOptions.TCP_NODELAY:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Tcp), SocketOptionName.wrap(SocketOptionName.NoDelay), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.SO_KEEPALIVE:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.KeepAlive), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.SO_LINGER:
                    {
                    cli.System.Net.Sockets.LingerOption linger;
                    if(val instanceof Boolean)
                    {
                        linger = new cli.System.Net.Sockets.LingerOption(false, 0);
                    }
                    else
                    {
                        linger = new cli.System.Net.Sockets.LingerOption(true, ((Integer)val).intValue());
                    }
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Linger), linger);
                    break;
                }
                case SocketOptions.SO_OOBINLINE:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.OutOfBandInline), ((Boolean)val).booleanValue() ? 1 : 0);
                    break;
                case SocketOptions.SO_TIMEOUT:
                    timeout = ((Integer)val).intValue();
                    break;
                default:
                    setCommonSocketOption(socket, option_id, val);
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

    static void setCommonSocketOption(cli.System.Net.Sockets.Socket socket, int option_id, Object val) throws SocketException
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
                case SocketOptions.SO_SNDBUF:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendBuffer), ((Integer)val).intValue());
                    break;
                case SocketOptions.SO_RCVBUF:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveBuffer), ((Integer)val).intValue());
                    break;
                case SocketOptions.IP_TOS:
                    socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.TypeOfService), ((Integer)val).intValue());
                    break;
                case SocketOptions.SO_BINDADDR:	// read-only
                default:
                    throw new SocketException("Invalid socket option: " + option_id);
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

    public Object getOption(int option_id) throws SocketException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            switch(option_id)
            {
                case SocketOptions.TCP_NODELAY:
                    return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Tcp), SocketOptionName.wrap(SocketOptionName.NoDelay))) != 0);
                case SocketOptions.SO_KEEPALIVE:
                    return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.KeepAlive))) != 0);
                case SocketOptions.SO_LINGER:
                    {
                    cli.System.Net.Sockets.LingerOption linger = (cli.System.Net.Sockets.LingerOption)socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Linger));
                    if(linger.get_Enabled())
                    {
                        return new Integer(linger.get_LingerTime());
                    }
                    return Boolean.FALSE;
                }
                case SocketOptions.SO_OOBINLINE:
                    return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.OutOfBandInline))));
                case SocketOptions.SO_TIMEOUT:
                    return new Integer(timeout);
                default:
                    return getCommonSocketOption(socket, option_id);
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

    static Object getCommonSocketOption(cli.System.Net.Sockets.Socket socket, int option_id) throws SocketException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            switch(option_id)
            {
                case SocketOptions.SO_REUSEADDR:
                    return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress))) != 0);
                case SocketOptions.SO_SNDBUF:
                    return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendBuffer))));
                case SocketOptions.SO_RCVBUF:
                    return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveBuffer))));
                case SocketOptions.IP_TOS:
                    return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.TypeOfService))));
                case SocketOptions.SO_BINDADDR:
                    return getInetAddressFromIPEndPoint((IPEndPoint)socket.get_LocalEndPoint());
                default:
                    throw new SocketException("Invalid socket option: " + option_id);
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

    private static InetAddress getInetAddressFromIPEndPoint(IPEndPoint endpoint)
    {
        try
        {
            return InetAddress.getByAddress(endpoint.get_Address().GetAddressBytes());
        }
        catch(UnknownHostException x)
        {
            // this exception only happens if the address byte array is of invalid length, which cannot happen unless
            // the .NET socket returns a bogus address
            throw (InternalError)new InternalError().initCause(x);
        }
    }

    protected InputStream getInputStream() throws IOException
    {
        return new InputStream() 
        {
            public int available() throws IOException 
            {
                return PlainSocketImpl.this.available();
            }
            public void close() throws IOException 
            {
                PlainSocketImpl.this.close();
            }
            public int read() throws IOException 
            {
                byte buf[] = new byte[1];
                int bytes_read = read(buf, 0, 1);
                if (bytes_read == 1)
                    return buf[0] & 0xFF;
                else
                    return -1;
            }
            public int read(byte[] buf) throws IOException 
            {
                return read(buf, 0, buf.length);
            }
            public int read(byte[] buf, int offset, int len) throws IOException 
            {
                int bytes_read = PlainSocketImpl.this.read(buf, offset, len);
                if (bytes_read == 0)
                    return -1;
                return bytes_read;
            }
        };
    }

    protected OutputStream getOutputStream() throws IOException
    {
        return new OutputStream() 
        {
            public void close() throws IOException 
            {
                PlainSocketImpl.this.close();
            }
            public void write(int b) throws IOException 
            {
                byte buf[] = { (byte)b };
                write(buf, 0, 1);
            }
            public void write(byte[] buf) throws IOException 
            {
                write(buf, 0, buf.length);
            }
            public void write(byte[] buf, int offset, int len) throws IOException 
            {
                PlainSocketImpl.this.write(buf, offset, len);
            }
        };
    }

    protected void connect(SocketAddress address, int timeout) throws IOException
    {
        // TODO support timeout
        InetSocketAddress inetAddress = (InetSocketAddress)address;
        if(inetAddress.isUnresolved())
        {
            throw new UnknownHostException(inetAddress.getHostName());
        }
        connect(inetAddress.getAddress(), inetAddress.getPort());
    }

    protected boolean supportsUrgentData()
    {
        return true;
    }

    public void sendUrgentData(int data) throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            byte[] oob = { (byte)data };
            socket.Send(oob, SocketFlags.wrap(SocketFlags.OutOfBand));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    public void shutdownInput() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.Shutdown(SocketShutdown.wrap(SocketShutdown.Receive));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    public void shutdownOutput() throws IOException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.Shutdown(SocketShutdown.wrap(SocketShutdown.Send));
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw convertSocketExceptionToIOException(x);
        }
        catch(cli.System.ObjectDisposedException x1)
        {
            throw new SocketException("Socket is closed");
        }
    }

    public void setBlocking(boolean blocking)
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            if(false) throw new cli.System.ObjectDisposedException("");
            socket.set_Blocking(blocking);
        }
        catch(cli.System.Net.Sockets.SocketException _)
        {
        }
        catch(cli.System.ObjectDisposedException _)
        {
        }
    }

    public boolean isInChannelOperation()
    {
	return false;
    }

    public cli.System.Net.Sockets.Socket getSocket()
    {
        return socket;
    }

    public InetSocketAddress getLocalAddress()
    {
        if(socket == null)
        {
            return null;
        }
        IPEndPoint endpoint = null;
        try
        {
            endpoint = (IPEndPoint)socket.get_LocalEndPoint();
        }
        catch(Throwable _)
        {
        }
        if(endpoint == null)
        {
            return null;
        }
        return new InetSocketAddress(getInetAddressFromIPEndPoint(endpoint), endpoint.get_Port());
    }
}
