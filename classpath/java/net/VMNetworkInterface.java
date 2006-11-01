/*
  Copyright (C) 2006 Jeroen Frijters

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

import java.util.ArrayList;
import java.util.HashSet;
import java.util.Set;

import cli.System.Activator;
import cli.System.Type;
import cli.System.Collections.IEnumerable;
import cli.System.Collections.IEnumerator;
import cli.System.Reflection.BindingFlags;

final class VMNetworkInterface
{
    final String name;
    final Set addresses;

    private VMNetworkInterface(String name, Set addresses)
    {
        this.name = name;
        this.addresses = addresses;
    }

    VMNetworkInterface()
    {
        name = null;
        addresses = new HashSet();
        addresses.add(InetAddress.ANY_IF);
    }

    static VMNetworkInterface[] getVMInterfaces() throws SocketException
    {
        // NOTE we use reflection to access the System.Management types, because I don't
        // want a static dependency on System.Management (for one, Mono currently doesn't implement it)
        // TODO once we move to .NET 2.0, we can use System.Net.NetworkInformation.NetworkInterface
        ArrayList netInterfaces = new ArrayList();
        Type type = Type.GetType("System.Management.ManagementClass, System.Management, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        if (type != null)
        {
            Object mgmt = Activator.CreateInstance(type, new Object[] { "Win32_NetworkAdapterConfiguration" });
            Object moc = invoke(mgmt, "GetInstances", new Object[0]);

            for (IEnumerator e = ((IEnumerable)moc).GetEnumerator(); e.MoveNext(); )
            {
                Object props = invoke(e.get_Current(), "get_Properties", new Object[0]);
                Object ipaddress = invoke(props, "get_Item", new Object[] { "IPAddress" });
                String[] addresses = (String[])invoke(ipaddress, "get_Value", new Object[0]);
                if (addresses != null)
                {
                    // TODO support IPv6
                    Object dnshostname = invoke(props, "get_Item", new Object[] { "DNSHostName" });
                    String hostName = (String)invoke(dnshostname, "get_Value", new Object[0]);
                    HashSet addrSet = new HashSet();
                    for (int i = 0; i < addresses.length; i++)
                    {
                        addrSet.add(new Inet4Address(getAddressBytes(addresses[i]), hostName));
                    }
                    Object description = invoke(props, "get_Item", new Object[] { "Description" });
                    netInterfaces.add(new VMNetworkInterface((String)invoke(description, "get_Value", new Object[0]), addrSet));
                }
            }
        }
        VMNetworkInterface[] arr = new VMNetworkInterface[netInterfaces.size()];
        netInterfaces.toArray(arr);
        return arr;
    }

    private static Object invoke(Object obj, String method, Object[] args)
    {
        return ((cli.System.Object)obj).GetType().InvokeMember(method, BindingFlags.wrap(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod), null, obj, args);
    }

    private static byte[] getAddressBytes(String aAddr)
    {
        String[] addrParts = aAddr.split("\\.");
        byte[] addrBytes = new byte[addrParts.length];
        for (int i = 0; i < addrParts.length; i++)
        {
            addrBytes[i] = (byte)Short.parseShort(addrParts[i]);
        }
        return addrBytes;
    }
}
