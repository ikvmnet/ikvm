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
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace IKVM.Java.Externs.java.net
{

    static class NetworkInterface
    {

#if !FIRST_PASS

        private static NetworkInterfaceInfo cache;
        private static DateTime cachedSince;

#endif

        public static void init()
        {

        }

#if !FIRST_PASS

        private sealed class NetworkInterfaceInfo
        {

            internal System.Net.NetworkInformation.NetworkInterface[] dotnetInterfaces;
            internal global::java.net.NetworkInterface[] javaInterfaces;

        }

        private static int Compare(System.Net.NetworkInformation.NetworkInterface ni1, System.Net.NetworkInformation.NetworkInterface ni2)
        {
            int index1 = GetIndex(ni1);
            int index2 = GetIndex(ni2);
            return index1.CompareTo(index2);
        }

        private static IPv4InterfaceProperties GetIPv4Properties(IPInterfaceProperties props)
        {
            try
            {
                return props.GetIPv4Properties();
            }
            catch (NetworkInformationException)
            {
                return null;
            }
        }

        private static IPv6InterfaceProperties GetIPv6Properties(IPInterfaceProperties props)
        {
            try
            {
                return props.GetIPv6Properties();
            }
            catch (NetworkInformationException)
            {
                return null;
            }
        }

        private static int GetIndex(System.Net.NetworkInformation.NetworkInterface ni)
        {
            IPInterfaceProperties ipprops = ni.GetIPProperties();
            IPv4InterfaceProperties ipv4props = GetIPv4Properties(ipprops);
            if (ipv4props != null)
            {
                return ipv4props.Index;
            }
            else if (InetAddressImplFactory.isIPv6Supported())
            {
                IPv6InterfaceProperties ipv6props = GetIPv6Properties(ipprops);
                if (ipv6props != null)
                {
                    return ipv6props.Index;
                }
            }

            return -1;
        }

        private static bool IsValid(System.Net.NetworkInformation.NetworkInterface ni)
        {
            return GetIndex(ni) != -1;
        }

        private static NetworkInterfaceInfo GetInterfaces()
        {
            // Since many of the methods in java.net.NetworkInterface end up calling this method and the underlying stuff this is
            // based on isn't very quick either, we cache the array for a couple of seconds.
            if (cache != null && DateTime.UtcNow - cachedSince < new TimeSpan(0, 0, 5))
                return cache;

            System.Net.NetworkInformation.NetworkInterface[] ifaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            // on Mono (on Windows) we need to filter out the network interfaces that don't have any IP properties
            ifaces = Array.FindAll(ifaces, IsValid);
            Array.Sort(ifaces, Compare);
            global::java.net.NetworkInterface[] ret = new global::java.net.NetworkInterface[ifaces.Length];
            int eth = 0;
            int tr = 0;
            int fddi = 0;
            int lo = 0;
            int ppp = 0;
            int sl = 0;
            int wlan = 0;
            int net = 0;
            for (int i = 0; i < ifaces.Length; i++)
            {
                string name;
                switch (ifaces[i].NetworkInterfaceType)
                {
                    case NetworkInterfaceType.Ethernet:
                        name = "eth" + eth++;
                        break;
                    case NetworkInterfaceType.TokenRing:
                        name = "tr" + tr++;
                        break;
                    case NetworkInterfaceType.Fddi:
                        name = "fddi" + fddi++;
                        break;
                    case NetworkInterfaceType.Loopback:
                        if (lo > 0)
                        {
                            continue;
                        }
                        name = "lo";
                        lo++;
                        break;
                    case NetworkInterfaceType.Ppp:
                        name = "ppp" + ppp++;
                        break;
                    case NetworkInterfaceType.Slip:
                        name = "sl" + sl++;
                        break;
                    case NetworkInterfaceType.Wireless80211:
                        name = "wlan" + wlan++;
                        break;
                    default:
                        name = "net" + net++;
                        break;
                }
                global::java.net.NetworkInterface netif = new global::java.net.NetworkInterface();
                ret[i] = netif;
                netif._set1(name, ifaces[i].Description, GetIndex(ifaces[i]));
                UnicastIPAddressInformationCollection uipaic = ifaces[i].GetIPProperties().UnicastAddresses;
                List<global::java.net.InetAddress> addresses = new List<global::java.net.InetAddress>();
                List<global::java.net.InterfaceAddress> bindings = new List<global::java.net.InterfaceAddress>();
                for (int j = 0; j < uipaic.Count; j++)
                {
                    IPAddress addr = uipaic[j].Address;
                    if (addr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (ifaces[i].OperationalStatus != OperationalStatus.Up)
                        {
                            // HACK on Windows, OpenJDK seems to only return IPv4 addresses for interfaces that are up.
                            // This is possibly the result of their usage of the (legacy) Win32 API GetIpAddrTable.
                            // Not doing this filtering causes some OpenJDK tests to fail.
                            continue;
                        }
                        global::java.net.Inet4Address address = new global::java.net.Inet4Address(null, addr.GetAddressBytes());
                        global::java.net.InterfaceAddress binding = new global::java.net.InterfaceAddress();
                        short mask = 32;
                        global::java.net.Inet4Address broadcast = null;
                        IPAddress v4mask;
                        try
                        {
                            v4mask = uipaic[j].IPv4Mask;
                        }
                        catch (NotImplementedException)
                        {
                            // Mono (as of 2.6.7) doesn't implement the IPv4Mask property
                            v4mask = null;
                        }
                        if (v4mask != null && !v4mask.Equals(IPAddress.Any))
                        {
                            broadcast = new global::java.net.Inet4Address(null, -1);
                            mask = 0;
                            foreach (byte b in v4mask.GetAddressBytes())
                            {
                                mask += (short)global::java.lang.Integer.bitCount(b);
                            }
                        }
                        else if (address.isLoopbackAddress())
                        {
                            mask = 8;
                            broadcast = new global::java.net.Inet4Address(null, 0xffffff);
                        }
                        binding._set(address, broadcast, mask);
                        addresses.Add(address);
                        bindings.Add(binding);
                    }
                    else if (InetAddressImplFactory.isIPv6Supported())
                    {
                        int scope = 0;
                        if (addr.IsIPv6LinkLocal || addr.IsIPv6SiteLocal)
                        {
                            scope = (int)addr.ScopeId;
                        }
                        global::java.net.Inet6Address ia6 = new global::java.net.Inet6Address();
                        ia6._holder().ipaddress = addr.GetAddressBytes();
                        if (scope != 0)
                        {
                            ia6._holder().scope_id = scope;
                            ia6._holder().scope_id_set = true;
                            ia6._holder().scope_ifname = netif;
                            ia6._holder().scope_ifname_set = true;
                        }
                        global::java.net.InterfaceAddress binding = new global::java.net.InterfaceAddress();
                        // TODO where do we get the IPv6 subnet prefix length?
                        short mask = 128;
                        binding._set(ia6, null, mask);
                        addresses.Add(ia6);
                        bindings.Add(binding);
                    }
                }
                netif._set2(addresses.ToArray(), bindings.ToArray(), new global::java.net.NetworkInterface[0]);
            }
            NetworkInterfaceInfo nii = new NetworkInterfaceInfo();
            nii.dotnetInterfaces = ifaces;
            nii.javaInterfaces = ret;
            cache = nii;
            cachedSince = DateTime.UtcNow;
            return nii;
        }
#endif

        private static System.Net.NetworkInformation.NetworkInterface GetDotNetNetworkInterfaceByIndex(int index)
        {
#if FIRST_PASS
		    return null;
#else
            NetworkInterfaceInfo nii = GetInterfaces();
            for (int i = 0; i < nii.javaInterfaces.Length; i++)
            {
                if (nii.javaInterfaces[i].getIndex() == index)
                {
                    return nii.dotnetInterfaces[i];
                }
            }
            throw new global::java.net.SocketException("interface index not found");
#endif
        }

        public static object getAll()
        {
#if FIRST_PASS
		    return null;
#else
            return GetInterfaces().javaInterfaces;
#endif
        }

        public static object getByName0(string name)
        {
#if FIRST_PASS
		    return null;
#else
            foreach (global::java.net.NetworkInterface iface in GetInterfaces().javaInterfaces)
            {
                if (iface.getName() == name)
                {
                    return iface;
                }
            }
            return null;
#endif
        }

        public static object getByIndex0(int index)
        {
#if FIRST_PASS
		    return null;
#else
            foreach (global::java.net.NetworkInterface iface in GetInterfaces().javaInterfaces)
            {
                if (iface.getIndex() == index)
                {
                    return iface;
                }
            }
            return null;
#endif
        }

        public static object getByInetAddress0(object addr)
        {
#if FIRST_PASS
		    return null;
#else
            foreach (global::java.net.NetworkInterface iface in GetInterfaces().javaInterfaces)
            {
                global::java.util.Enumeration addresses = iface.getInetAddresses();
                while (addresses.hasMoreElements())
                {
                    if (addresses.nextElement().Equals(addr))
                    {
                        return iface;
                    }
                }
            }
            return null;
#endif
        }

        public static bool isUp0(string name, int ind)
        {
#if FIRST_PASS
		    return false;
#else
            return GetDotNetNetworkInterfaceByIndex(ind).OperationalStatus == OperationalStatus.Up;
#endif
        }

        public static bool isLoopback0(string name, int ind)
        {
#if FIRST_PASS
		    return false;
#else
            return GetDotNetNetworkInterfaceByIndex(ind).NetworkInterfaceType == NetworkInterfaceType.Loopback;
#endif
        }

        public static bool supportsMulticast0(string name, int ind)
        {
#if FIRST_PASS
		    return false;
#else
            return GetDotNetNetworkInterfaceByIndex(ind).SupportsMulticast;
#endif
        }

        public static bool isP2P0(string name, int ind)
        {
#if FIRST_PASS
		    return false;
#else
            switch (GetDotNetNetworkInterfaceByIndex(ind).NetworkInterfaceType)
            {
                case NetworkInterfaceType.Ppp:
                case NetworkInterfaceType.Slip:
                    return true;
                default:
                    return false;
            }
#endif
        }

        public static byte[] getMacAddr0(byte[] inAddr, string name, int ind)
        {
#if FIRST_PASS
		    return null;
#else
            return GetDotNetNetworkInterfaceByIndex(ind).GetPhysicalAddress().GetAddressBytes();
#endif
        }

        public static int getMTU0(string name, int ind)
        {
#if FIRST_PASS
		    return 0;
#else
            IPInterfaceProperties ipprops = GetDotNetNetworkInterfaceByIndex(ind).GetIPProperties();
            IPv4InterfaceProperties v4props = GetIPv4Properties(ipprops);
            if (v4props != null)
            {
                return v4props.Mtu;
            }
            if (InetAddressImplFactory.isIPv6Supported())
            {
                IPv6InterfaceProperties v6props = GetIPv6Properties(ipprops);
                if (v6props != null)
                {
                    return v6props.Mtu;
                }
            }
            return -1;
#endif
        }

    }

}
