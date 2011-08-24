/*
  Copyright (C) 2002-2010 Jeroen Frijters

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

package java.net;

import cli.System.Net.IPAddress;
import cli.System.Net.IPEndPoint;
import cli.System.Net.Sockets.SocketOptionName;
import cli.System.Net.Sockets.SocketOptionLevel;
import ikvm.lang.CIL;
import java.io.IOException;

@ikvm.lang.Internal
public final class SocketUtil
{
    private SocketUtil() { }

    // Winsock Error Codes
    public static final int WSAEINVAL = 10022;
    public static final int WSAEWOULDBLOCK = 10035;
    public static final int WSAEMSGSIZE = 10040;
    public static final int WSAENOPROTOOPT = 10042;
    public static final int WSAEADDRINUSE = 10048;
    public static final int WSAENETUNREACH = 10051;
    public static final int WSAECONNRESET = 10054;
    public static final int WSAESHUTDOWN = 10058;
    public static final int WSAETIMEDOUT = 10060;
    public static final int WSAECONNREFUSED = 10061;
    public static final int WSAEHOSTUNREACH = 10065;
    public static final int WSAHOST_NOT_FOUND = 11001;

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
        return getAddressFromInetAddress(addr, false);
    }

    public static IPAddress getAddressFromInetAddress(InetAddress addr, boolean v4mapped)
    {
        byte[] b = addr.getAddress();
        if (b.length == 16)
        {
            // FXBUG in .NET 1.1 you can only construct IPv6 addresses (not IPv4) with this constructor
            // (according to the documentation this was fixed in .NET 2.0)
            return new IPAddress(b);
        }
        else if (v4mapped)
        {
            if (b[0] == 0 && b[1] == 0 && b[2] == 0 && b[3] == 0)
            {
                return IPAddress.IPv6Any;
            }
            byte[] b16 = new byte[16];
            b16[10] = -1;
            b16[11] = -1;
            b16[12] = b[0];
            b16[13] = b[1];
            b16[14] = b[2];
            b16[15] = b[3];
            return new IPAddress(b16);
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
}
