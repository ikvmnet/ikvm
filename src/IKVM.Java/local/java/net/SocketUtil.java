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

import java.io.IOException;

import sun.net.ConnectionResetException;

import ikvm.lang.CIL;

import cli.System.Net.IPAddress;
import cli.System.Net.IPEndPoint;
import cli.System.Net.Sockets.SocketOptionName;
import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.SocketError;

@ikvm.lang.Internal
public final class SocketUtil
{

    private SocketUtil()
    {

    }

    public static IOException convertSocketExceptionToIOException(cli.System.Net.Sockets.SocketException e) throws IOException
    {
        switch (e.get_SocketErrorCode().Value)
        {
            case SocketError.AddressAlreadyInUse:
                return new BindException(e.get_Message());
            case SocketError.NetworkUnreachable:
            case SocketError.HostUnreachable:
                return new NoRouteToHostException(e.get_Message());
            case SocketError.TimedOut:
                return new SocketTimeoutException(e.get_Message());
            case SocketError.ConnectionRefused:
                return new PortUnreachableException(e.get_Message());
            case SocketError.ConnectionReset:
                return new ConnectionResetException(e.get_Message());
            case SocketError.HostNotFound:
                return new UnknownHostException(e.get_Message());
            default:
                return new SocketException(e.get_Message() + "\nError Code: " + e.get_SocketErrorCode());
        }
    }

    public static IPAddress getAddressFromInetAddress(InetAddress addr)
    {
        return getAddressFromInetAddress(addr, false);
    }

    public static IPAddress getAddressFromInetAddress(InetAddress addr, boolean v4mapped)
    {
        byte[] b = addr.getAddress();
        if (v4mapped)
        {
            if (b[0] == 0 && b[1] == 0 && b[2] == 0 && b[3] == 0)
                return IPAddress.IPv6Any;

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

}
