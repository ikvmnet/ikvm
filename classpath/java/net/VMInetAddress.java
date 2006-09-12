/*
  Copyright (C) 2005 Jeroen Frijters

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

final class VMInetAddress
{
    static String getLocalHostname() throws UnknownHostException
    {
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            return cli.System.Net.Dns.GetHostName();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new UnknownHostException(x.getMessage());
        }
    }

    static byte[] lookupInaddrAny() throws UnknownHostException
    {
        return new byte[] { 0, 0, 0, 0 };        
    }

    static String getHostByAddr(byte[] ip) throws UnknownHostException
    {
        String s;
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            String address = (ip[0] & 0xFF) + "." + (ip[1] & 0xFF) + "." + (ip[2] & 0xFF) + "." + (ip[3] & 0xFF);
            s = cli.System.Net.Dns.GetHostByAddress(address).get_HostName();
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            throw new UnknownHostException(x.getMessage());
        }
        try
        {
            if(false) throw new cli.System.Net.Sockets.SocketException();
            cli.System.Net.Dns.GetHostByName(s);
        }
        catch(cli.System.Net.Sockets.SocketException x)
        {
            // FXBUG .NET framework bug
            // HACK if GetHostByAddress returns a netbios name, it appends the default DNS suffix, but if the
            // machine's netbios name isn't the same as the DNS hostname, this might result in an unresolvable
            // name, if that happens we chop of the DNS suffix.
            int idx = s.indexOf('.');
            if(idx > 0)
            {
                return s.substring(0, idx);
            }
        }
        return s;
    }

    static byte[] aton(String address)
    {
        try
        {
            return cli.System.Net.IPAddress.Parse(address).GetAddressBytes();
        }
        catch(Throwable _)
        {
            return null;
        }
    }

    static byte[][] getHostByName(String name) throws UnknownHostException
    {
        // TODO error handling
        try
        {
            cli.System.Net.IPHostEntry he = cli.System.Net.Dns.GetHostByName(name);
            cli.System.Net.IPAddress[] addresses = he.get_AddressList();
            byte[][] list = new byte[addresses.length][];
            for(int i = 0; i < addresses.length; i++)
            {
                list[i] = addresses[i].GetAddressBytes();
            }
            return list;
        }
        catch(Throwable x)
        {
            throw new UnknownHostException(x.getMessage());
        }
    }
}
