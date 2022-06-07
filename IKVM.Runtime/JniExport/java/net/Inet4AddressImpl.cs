/*
  Copyright (C) 2007-2015 Jeroen Frijters

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
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security;

namespace IKVM.Runtime.JniExport.java.net
{

    static class Inet4AddressImpl
    {

        public static string getLocalHostName(object thisInet4AddressImpl)
        {
#if FIRST_PASS
		    return null;
#else
            try
            {
                return Dns.GetHostName();
            }
            catch (SocketException)
            {

            }
            catch (SecurityException)
            {

            }

            return "localhost";
#endif
        }

        public static object lookupAllHostAddr(object thisInet4AddressImpl, string hostname)
        {
#if FIRST_PASS
		    return null;
#else
            try
            {
                IPAddress[] addr = Dns.GetHostAddresses(hostname);
                List<global::java.net.InetAddress> addresses = new List<global::java.net.InetAddress>();
                for (int i = 0; i < addr.Length; i++)
                {
                    byte[] b = addr[i].GetAddressBytes();
                    if (b.Length == 4)
                    {
                        addresses.Add(global::java.net.InetAddress.getByAddress(hostname, b));
                    }
                }
                if (addresses.Count == 0)
                {
                    throw new global::java.net.UnknownHostException(hostname);
                }
                return addresses.ToArray();
            }
            catch (ArgumentException x)
            {
                throw new global::java.net.UnknownHostException(x.Message);
            }
            catch (SocketException x)
            {
                throw new global::java.net.UnknownHostException(x.Message);
            }
#endif
        }

        public static string getHostByAddr(object thisInet4AddressImpl, byte[] addr)
        {
#if FIRST_PASS
		    return null;
#else
            try
            {
                return Dns.GetHostEntry(new IPAddress(addr)).HostName;
            }
            catch (ArgumentException x)
            {
                throw new global::java.net.UnknownHostException(x.Message);
            }
            catch (SocketException x)
            {
                throw new global::java.net.UnknownHostException(x.Message);
            }
#endif
        }

        public static bool isReachable0(object thisInet4AddressImpl, byte[] addr, int timeout, byte[] ifaddr, int ttl)
        {
            // like the JDK, we don't use Ping, but we try a TCP connection to the echo port
            // (.NET 2.0 has a System.Net.NetworkInformation.Ping class, but that doesn't provide the option of binding to a specific interface)
            try
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    if (ifaddr != null)
                    {
                        sock.Bind(new IPEndPoint(((ifaddr[3] << 24) + (ifaddr[2] << 16) + (ifaddr[1] << 8) + ifaddr[0]) & 0xFFFFFFFFL, 0));
                    }
                    if (ttl > 0)
                    {
                        sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, ttl);
                    }
                    IPEndPoint ep = new IPEndPoint(((addr[3] << 24) + (addr[2] << 16) + (addr[1] << 8) + addr[0]) & 0xFFFFFFFFL, 7);
                    IAsyncResult res = sock.BeginConnect(ep, null, null);
                    if (res.AsyncWaitHandle.WaitOne(timeout, false))
                    {
                        try
                        {
                            sock.EndConnect(res);
                            return true;
                        }
                        catch (SocketException x)
                        {
                            const int WSAECONNREFUSED = 10061;
                            if (x.ErrorCode == WSAECONNREFUSED)
                            {
                                // we got back an explicit "connection refused", that means the host was reachable.
                                return true;
                            }
                        }
                    }
                }
            }
            catch (SocketException)
            {

            }

            return false;
        }

    }

}