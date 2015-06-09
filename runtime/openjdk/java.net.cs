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
using System.Security;

static class Java_java_net_AbstractPlainDatagramSocketImpl
{
	public static void init()
	{
	}

	public static int dataAvailable(object _this)
	{
#if FIRST_PASS
		return 0;
#else
		try
		{
			java.net.AbstractPlainDatagramSocketImpl obj = (java.net.AbstractPlainDatagramSocketImpl)_this;
			if (obj.fd != null)
			{
				return obj.fd.getSocket().Available;
			}
		}
		catch (ObjectDisposedException)
		{
		}
		catch (SocketException)
		{
		}
		throw new java.net.SocketException("Socket closed");
#endif
	}
}

static class Java_java_net_DatagramPacket
{
	public static void init()
	{
	}
}

static class Java_java_net_InetAddress
{
	public static void init()
	{
	}

#if !FIRST_PASS
	internal static java.net.InetAddress ConvertIPAddress(IPAddress address, string hostname)
	{
		if (address.IsIPv6LinkLocal || address.IsIPv6SiteLocal)
		{
			return java.net.Inet6Address.getByAddress(hostname, address.GetAddressBytes(), (int)address.ScopeId);
		}
		else
		{
			return java.net.InetAddress.getByAddress(hostname, address.GetAddressBytes());
		}
	}
#endif
}

static class Java_java_net_InetAddressImplFactory
{
	private static readonly bool ipv6supported = Init();

	private static bool Init()
	{
		string env = IKVM.Internal.JVM.SafeGetEnvironmentVariable("IKVM_IPV6");
		int val;
		if (env != null && Int32.TryParse(env, out val))
		{
			return (val & 1) != 0;
		}
		// On Linux we can't bind both an IPv4 and IPv6 to the same port, so we have to disable IPv6 until we have a dual-stack implementation.
		// Mono on Windows doesn't appear to support IPv6 either (Mono on Linux does).
		return Type.GetType("Mono.Runtime") == null
			&& Environment.OSVersion.Platform == PlatformID.Win32NT
			&& Socket.OSSupportsIPv6;
	}

	public static bool isIPv6Supported()
	{
		return ipv6supported;
	}
}

static class Java_java_net_Inet4Address
{
	public static void init()
	{
	}
}

static class Java_java_net_Inet4AddressImpl
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
			List<java.net.InetAddress> addresses = new List<java.net.InetAddress>();
			for (int i = 0; i < addr.Length; i++)
			{
				byte[] b = addr[i].GetAddressBytes();
				if (b.Length == 4)
				{
					addresses.Add(java.net.InetAddress.getByAddress(hostname, b));
				}
			}
			if (addresses.Count == 0)
			{
				throw new java.net.UnknownHostException(hostname);
			}
			return addresses.ToArray();
		}
		catch (ArgumentException x)
		{
			throw new java.net.UnknownHostException(x.Message);
		}
		catch (SocketException x)
		{
			throw new java.net.UnknownHostException(x.Message);
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
			throw new java.net.UnknownHostException(x.Message);
		}
		catch (SocketException x)
		{
			throw new java.net.UnknownHostException(x.Message);
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

static class Java_java_net_Inet6Address
{
	public static void init()
	{
	}
}

static class Java_java_net_Inet6AddressImpl
{
	public static string getLocalHostName(object thisInet6AddressImpl)
	{
#if FIRST_PASS
		return null;
#else
		try
		{
			return Dns.GetHostName();
		}
		catch (SocketException x)
		{
			throw new java.net.UnknownHostException(x.Message);
		}
#endif
	}

	public static object lookupAllHostAddr(object thisInet6AddressImpl, string hostname)
	{
#if FIRST_PASS
		return null;
#else
		try
		{
			IPAddress[] addr = Dns.GetHostAddresses(hostname);
			java.net.InetAddress[] addresses = new java.net.InetAddress[addr.Length];
			int pos = 0;
			for (int i = 0; i < addr.Length; i++)
			{
				if (addr[i].AddressFamily == AddressFamily.InterNetworkV6 == java.net.InetAddress.preferIPv6Address)
				{
					addresses[pos++] = Java_java_net_InetAddress.ConvertIPAddress(addr[i], hostname);
				}
			}
			for (int i = 0; i < addr.Length; i++)
			{
				if (addr[i].AddressFamily == AddressFamily.InterNetworkV6 != java.net.InetAddress.preferIPv6Address)
				{
					addresses[pos++] = Java_java_net_InetAddress.ConvertIPAddress(addr[i], hostname);
				}
			}
			if (addresses.Length == 0)
			{
				throw new java.net.UnknownHostException(hostname);
			}
			return addresses;
		}
		catch (ArgumentException x)
		{
			throw new java.net.UnknownHostException(x.Message);
		}
		catch (SocketException x)
		{
			throw new java.net.UnknownHostException(x.Message);
		}
#endif
	}

	public static string getHostByAddr(object thisInet6AddressImpl, byte[] addr)
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
			throw new java.net.UnknownHostException(x.Message);
		}
		catch (SocketException x)
		{
			throw new java.net.UnknownHostException(x.Message);
		}
#endif
	}

	public static bool isReachable0(object thisInet6AddressImpl, byte[] addr, int scope, int timeout, byte[] inf, int ttl, int if_scope)
	{
		if (addr.Length == 4)
		{
			return Java_java_net_Inet4AddressImpl.isReachable0(null, addr, timeout, inf, ttl);
		}
		// like the JDK, we don't use Ping, but we try a TCP connection to the echo port
		// (.NET 2.0 has a System.Net.NetworkInformation.Ping class, but that doesn't provide the option of binding to a specific interface)
		try
		{
			using (Socket sock = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp))
			{
				if (inf != null)
				{
					sock.Bind(new IPEndPoint(new IPAddress(inf, (uint)if_scope), 0));
				}
				if (ttl > 0)
				{
					sock.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.HopLimit, ttl);
				}
				IPEndPoint ep = new IPEndPoint(new IPAddress(addr, (uint)scope), 7);
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
		catch (ArgumentException)
		{
		}
		catch (SocketException)
		{
		}
		return false;
	}
}

static class Java_java_net_NetworkInterface
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
		internal NetworkInterface[] dotnetInterfaces;
		internal java.net.NetworkInterface[] javaInterfaces;
	}

	private static int Compare(NetworkInterface ni1, NetworkInterface ni2)
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

	private static int GetIndex(NetworkInterface ni)
	{
		IPInterfaceProperties ipprops = ni.GetIPProperties();
		IPv4InterfaceProperties ipv4props = GetIPv4Properties(ipprops);
		if (ipv4props != null)
		{
			return ipv4props.Index;
		}
		else if (Java_java_net_InetAddressImplFactory.isIPv6Supported())
		{
			IPv6InterfaceProperties ipv6props = GetIPv6Properties(ipprops);
			if (ipv6props != null)
			{
				return ipv6props.Index;
			}
		}
		return -1;
	}

	private static bool IsValid(NetworkInterface ni)
	{
		return GetIndex(ni) != -1;
	}

	private static NetworkInterfaceInfo GetInterfaces()
	{
		// Since many of the methods in java.net.NetworkInterface end up calling this method and the underlying stuff this is
		// based on isn't very quick either, we cache the array for a couple of seconds.
		if (cache != null && DateTime.UtcNow - cachedSince < new TimeSpan(0, 0, 5))
		{
			return cache;
		}
		NetworkInterface[] ifaces = NetworkInterface.GetAllNetworkInterfaces();
		// on Mono (on Windows) we need to filter out the network interfaces that don't have any IP properties
		ifaces = Array.FindAll(ifaces, IsValid);
		Array.Sort(ifaces, Compare);
		java.net.NetworkInterface[] ret = new java.net.NetworkInterface[ifaces.Length];
		int eth = 0;
		int tr = 0;
		int fddi = 0;
		int lo = 0;
		int ppp = 0;
		int sl = 0;
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
				default:
					name = "net" + net++;
					break;
			}
			java.net.NetworkInterface netif = new java.net.NetworkInterface();
			ret[i] = netif;
			netif._set1(name, ifaces[i].Description, GetIndex(ifaces[i]));
			UnicastIPAddressInformationCollection uipaic = ifaces[i].GetIPProperties().UnicastAddresses;
			List<java.net.InetAddress> addresses = new List<java.net.InetAddress>();
			List<java.net.InterfaceAddress> bindings = new List<java.net.InterfaceAddress>();
			for (int j = 0; j < uipaic.Count; j++)
			{
				IPAddress addr = uipaic[j].Address;
				if (addr.AddressFamily == AddressFamily.InterNetwork)
				{
					java.net.Inet4Address address = new java.net.Inet4Address(null, addr.GetAddressBytes());
					java.net.InterfaceAddress binding = new java.net.InterfaceAddress();
					short mask = 32;
					java.net.Inet4Address broadcast = null;
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
						broadcast = new java.net.Inet4Address(null, -1);
						mask = 0;
						foreach (byte b in v4mask.GetAddressBytes())
						{
							mask += (short)java.lang.Integer.bitCount(b);
						}
					}
					else if (address.isLoopbackAddress())
					{
						mask = 8;
						broadcast = new java.net.Inet4Address(null, 0xffffff);
					}
					binding._set(address, broadcast, mask);
					addresses.Add(address);
					bindings.Add(binding);
				}
				else if (Java_java_net_InetAddressImplFactory.isIPv6Supported())
				{
					int scope = 0;
					if (addr.IsIPv6LinkLocal || addr.IsIPv6SiteLocal)
					{
						scope = (int)addr.ScopeId;
					}
					java.net.Inet6Address ia6 = new java.net.Inet6Address();
					ia6._holder().ipaddress = addr.GetAddressBytes();
					if (scope != 0)
					{
						ia6._holder().scope_id = scope;
						ia6._holder().scope_id_set = true;
						ia6._holder().scope_ifname = netif;
						ia6._holder().scope_ifname_set = true;
					}
					java.net.InterfaceAddress binding = new java.net.InterfaceAddress();
					// TODO where do we get the IPv6 subnet prefix length?
					short mask = 128;
					binding._set(ia6, null, mask);
					addresses.Add(ia6);
					bindings.Add(binding);
				}
			}
			netif._set2(addresses.ToArray(), bindings.ToArray(), new java.net.NetworkInterface[0]);
		}
		NetworkInterfaceInfo nii = new NetworkInterfaceInfo();
		nii.dotnetInterfaces = ifaces;
		nii.javaInterfaces = ret;
		cache = nii;
		cachedSince = DateTime.UtcNow;
		return nii;
	}
#endif

	private static NetworkInterface GetDotNetNetworkInterfaceByIndex(int index)
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
		throw new java.net.SocketException("interface index not found");
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
		foreach (java.net.NetworkInterface iface in GetInterfaces().javaInterfaces)
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
		foreach (java.net.NetworkInterface iface in GetInterfaces().javaInterfaces)
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
		foreach (java.net.NetworkInterface iface in GetInterfaces().javaInterfaces)
		{
			java.util.Enumeration addresses = iface.getInetAddresses();
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
		if (Java_java_net_InetAddressImplFactory.isIPv6Supported())
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
