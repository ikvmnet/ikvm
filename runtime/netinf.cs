/*
  Copyright (C) 2007 Jeroen Frijters

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

#if !WHIDBEY
using System;
using System.Collections;
using System.Management;
using IPAddress = System.Net.IPAddress;

/*
 * This is a .NET 1.1 compatible partial implementation of the .NET 2.0 System.Net.NetworkInformation namespace.
 * It only provides the APIs needed by the NetworkInterface class in openjdk.cs.
 * It uses WMI (through the System.Management assembly) to query the relevant information.
 */

namespace System.Net.NetworkInformation
{
	enum NetworkInterfaceType
	{
		Unknown = -1,
		Ethernet = 0,
		TokenRing = 1,
		Fddi = 2,
		Loopback = -2,
		Ppp = 3,
		Slip = -3
	}

	enum OperationalStatus
	{
		Unknown,
		Up
	}

	class UnicastIPAddressInformation
	{
		private IPAddress address;

		internal UnicastIPAddressInformation(IPAddress address)
		{
			this.address = address;
		}

		internal IPAddress Address
		{
			get
			{
				return address;
			}
		}
	}

	class UnicastIPAddressInformationCollection
	{
		private IPAddress[] addresses;

		internal UnicastIPAddressInformationCollection(IPAddress[] addresses)
		{
			this.addresses = addresses;
		}

		internal UnicastIPAddressInformation this[int index]
		{
			get
			{
				return new UnicastIPAddressInformation(addresses[index]);
			}
		}
	
		internal int Count
		{
			get
			{
				return addresses.Length;
			}
		}
	}

	class IPInterfaceProperties
	{
		private UnicastIPAddressInformationCollection addresses;
		internal IPv4InterfaceProperties v4props;

		internal IPInterfaceProperties(IPAddress[] addresses, int mtu)
		{
			this.addresses = new UnicastIPAddressInformationCollection(addresses);
			this.v4props = new IPv4InterfaceProperties(mtu);
		}

		internal UnicastIPAddressInformationCollection UnicastAddresses
		{
			get
			{
				return addresses;
			}
		}

		internal IPv4InterfaceProperties GetIPv4Properties()
		{
			return v4props;
		}
	}

	class IPv4InterfaceProperties
	{
		private int mtu;

		internal IPv4InterfaceProperties(int mtu)
		{
			this.mtu = mtu;
		}

		internal int Mtu
		{
			get
			{
				return mtu;
			}
		}
	}

	class PhysicalAddress
	{
		private byte[] mac;

		internal PhysicalAddress(byte[] mac)
		{
			this.mac = mac;
		}

		internal byte[] GetAddressBytes()
		{
			return mac == null ? null : (byte[])mac.Clone();
		}
	}

	class NetworkInterface
	{
		private uint interfaceIndex;
		private string description;
		private IPInterfaceProperties props;
		private NetworkInterfaceType type = NetworkInterfaceType.Unknown;
		private OperationalStatus status = OperationalStatus.Unknown;
		private byte[] mac;

		private NetworkInterface(uint interfaceIndex, string description, IPAddress[] addresses, int mtu, byte[] mac)
		{
			this.interfaceIndex = interfaceIndex;
			this.description = description;
			this.props = new IPInterfaceProperties(addresses, mtu);
			this.mac = mac;
		}

		internal static NetworkInterface[] GetAllNetworkInterfaces()
		{
			NetworkInterface[] netif;
			using (ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration"))
			{
				ArrayList ifaces = new ArrayList();
				// loopback isn't reported, so we make it up
				NetworkInterface loopback = new NetworkInterface(0xFFFFFFFF, "Software Loopback Interface 1", new IPAddress[] { IPAddress.Loopback }, -1, new byte[0]);
				loopback.type = NetworkInterfaceType.Loopback;
				loopback.status = OperationalStatus.Up;
				ifaces.Add(loopback);
				using (ManagementObjectCollection moc = mgmt.GetInstances())
				{
					foreach (ManagementObject obj in moc)
					{
						PropertyDataCollection props = obj.Properties;
						PropertyData ipaddress = props["IPAddress"];
						string[] addresses = (string[])ipaddress.Value;
						IPAddress[] addr;
						if (addresses != null)
						{
							addr = new IPAddress[addresses.Length];
							for (int i = 0; i < addresses.Length; i++)
							{
								addr[i] = IPAddress.Parse(addresses[i]);
							}
						}
						else
						{
							addr = new IPAddress[0];
						}
						int mtu = -1;
						try
						{
							// TODO this doesn't work, the MTU value is always null
							mtu = (int)(uint)props["MTU"].Value;
						}
						catch
						{
						}
						byte[] mac = null;
						try
						{
							mac = ParseMacAddress((string)props["MACAddress"].Value);
						}
						catch
						{
						}
						ifaces.Add(new NetworkInterface((uint)props["InterfaceIndex"].Value, (string)props["Description"].Value, addr, mtu, mac));
					}
				}
				netif = (NetworkInterface[])ifaces.ToArray(typeof(NetworkInterface));
			}
			using (ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapter"))
			{
				using (ManagementObjectCollection moc = mgmt.GetInstances())
				{
					foreach (ManagementObject obj in moc)
					{
						PropertyDataCollection props = obj.Properties;
						uint interfaceIndex = (uint)props["InterfaceIndex"].Value;
						NetworkInterfaceType type = NetworkInterfaceType.Unknown;
						try
						{
							type = (NetworkInterfaceType)(ushort)props["AdapterTypeID"].Value;
						}
						catch
						{
						}
						OperationalStatus status = OperationalStatus.Unknown;
						try
						{
							if ((ushort)props["NetConnectionStatus"].Value == 2)
							{
								status = OperationalStatus.Up;
							}
						}
						catch
						{
						}
						for (int i = 0; i < netif.Length; i++)
						{
							if (netif[i].interfaceIndex == interfaceIndex)
							{
								netif[i].type = type;
								netif[i].status = status;
								break;
							}
						}
					}
				}
			}
			return netif;
		}

		private static byte[] ParseMacAddress(string mac)
		{
			string[] split = mac.Split(':');
			byte[] bytes = new byte[split.Length];
			for (int i = 0; i < split.Length; i++)
			{
				const string digits = "0123456789ABCDEF";
				if (split[i].Length != 2)
				{
					return null;
				}
				int d0 = digits.IndexOf(char.ToUpper(split[i][0]));
				int d1 = digits.IndexOf(char.ToUpper(split[i][1]));
				if (d0 == -1 || d1 == -1)
				{
					return null;
				}
				bytes[i] = (byte)(d0 * 16 + d1);
			}
			return bytes;
		}

		internal string Description
		{
			get
			{
				return description;
			}
		}

		internal NetworkInterfaceType NetworkInterfaceType
		{
			get
			{
				return type;
			}
		}

		internal IPInterfaceProperties GetIPProperties()
		{
			return props;
		}

		internal OperationalStatus OperationalStatus
		{
			get
			{
				return status;
			}
		}

		internal bool SupportsMulticast
		{
			get
			{
				// TODO I don't know how to query this
				return true;
			}
		}

		internal PhysicalAddress GetPhysicalAddress()
		{
			return new PhysicalAddress(mac);
		}
	}
}
#endif // !WHIDBEY
