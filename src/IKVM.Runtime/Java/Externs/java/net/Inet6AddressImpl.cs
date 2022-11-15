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
using System.Net;
using System.Net.Sockets;

using IKVM.Runtime.Java.Externs.java.net;

namespace IKVM.Java.Externs.java.net
{

    static class Inet6AddressImpl
    {

        public static string getLocalHostName(object self)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            try
            {
                return Dns.GetHostName();
            }
            catch (SocketException e)
            {
                throw new global::java.net.UnknownHostException(e.Message);
            }
#endif
        }

        public static object lookupAllHostAddr(object self, string hostname)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            try
            {
                var addr = Dns.GetHostAddresses(hostname);
                var addresses = new global::java.net.InetAddress[addr.Length];
                int pos = 0;
                for (int i = 0; i < addr.Length; i++)
                    if (addr[i].AddressFamily == AddressFamily.InterNetworkV6 == global::java.net.InetAddress.preferIPv6Address)
                        addresses[pos++] = addr[i].ToInetAddress(hostname);

                for (int i = 0; i < addr.Length; i++)
                    if (addr[i].AddressFamily == AddressFamily.InterNetworkV6 != global::java.net.InetAddress.preferIPv6Address)
                        addresses[pos++] = addr[i].ToInetAddress(hostname);

                if (addresses.Length == 0)
                    throw new global::java.net.UnknownHostException(hostname);

                return addresses;
            }
            catch (ArgumentException e)
            {
                throw new global::java.net.UnknownHostException(e.Message);
            }
            catch (SocketException e)
            {
                throw new global::java.net.UnknownHostException(e.Message);
            }
#endif
        }

        public static string getHostByAddr(object thisInet6AddressImpl, byte[] addr)
        {
#if FIRST_PASS
            throw new NotSupportedException();
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

        public static bool isReachable0(object self, byte[] addr, int scope, int timeout, byte[] inf, int ttl, int if_scope)
        {
            if (addr.Length == 4)
                return Inet4AddressImpl.isReachable0(null, addr, timeout, inf, ttl);

            try
            {
                using var socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                if (inf != null)
                    socket.Bind(new IPEndPoint(new IPAddress(inf, (uint)if_scope), 0));

                if (ttl > 0)
                    socket.Ttl = (short)ttl;

                var ep = new IPEndPoint(new IPAddress(addr, (uint)scope), 7);
                var res = socket.BeginConnect(ep, null, null);
                if (res.AsyncWaitHandle.WaitOne(timeout, false))
                {
                    try
                    {
                        socket.EndConnect(res);
                        return true;
                    }
                    catch (SocketException x)
                    {
                        if (x.SocketErrorCode == SocketError.ConnectionRefused)
                            return true;
                    }
                }
            }
            catch (ArgumentException)
            {

            }
            catch (SocketException)
            {

            }

            return false;
        }

    }

}
