using com.sun.org.apache.bcel.@internal.generic;

using FluentAssertions;

using java.lang;
using java.net;
using java.nio.channels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static com.sun.tools.javac.tree.DCTree;
using sun.net.www.content.image;
using java.util;
using System.Linq;
using System.Collections.Generic;
using System;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class DatagramChannelTests
    {

        [TestMethod]
        public void ReuseAddressShouldSet()
        {
            var ch = DatagramChannel.open();
            ch.setOption(StandardSocketOptions.SO_REUSEADDR, global::java.lang.Boolean.TRUE);
            ch.getOption(StandardSocketOptions.SO_REUSEADDR).Should().Be(global::java.lang.Boolean.TRUE);
            ch.setOption(StandardSocketOptions.SO_REUSEADDR, global::java.lang.Boolean.FALSE);
            ch.getOption(StandardSocketOptions.SO_REUSEADDR).Should().Be(global::java.lang.Boolean.FALSE);
        }

        [TestMethod]
        public void CanBindToRandomPort()
        {
            var ch = DatagramChannel.open();
            ch.setOption(StandardSocketOptions.SO_REUSEADDR, global::java.lang.Boolean.FALSE);
            ch.bind(new InetSocketAddress(0));
        }

        [TestMethod]
        public void CanBindToSamePortWithReuse()
        {
            var ch = DatagramChannel.open();
            ch.setOption(StandardSocketOptions.SO_REUSEADDR, global::java.lang.Boolean.TRUE);
            ch.bind(new InetSocketAddress(0));

            var ch2 = DatagramChannel.open();
            ch2.setOption(StandardSocketOptions.SO_REUSEADDR, global::java.lang.Boolean.TRUE);
            ch2.bind(ch.getLocalAddress());
        }

        /**
         * Tests that existing membership key is returned by join methods and that
         * membership key methods return the expected results
         */

        void MembershipKeyTests(NetworkInterface nif, InetAddress group, InetAddress source)
        {
            ProtocolFamily family = (group is Inet4Address) ? StandardProtocolFamily.INET : StandardProtocolFamily.INET6;

            DatagramChannel dc = DatagramChannel.open(family)
                .setOption(StandardSocketOptions.SO_REUSEADDR, global::java.lang.Boolean.TRUE)
                .bind(new InetSocketAddress(source, 0));

            // check existing key is returned
            MembershipKey key = dc.join(group, nif);
            MembershipKey other = dc.join(group, nif);
            if (other != key)
            {
                throw new RuntimeException("existing key not returned");
            }

            // check key
            if (!key.isValid())
                throw new RuntimeException("key is not valid");
            if (!key.group().equals(group))
                throw new RuntimeException("group is incorrect");
            if (!key.networkInterface().equals(nif))
                throw new RuntimeException("network interface is incorrect");
            if (key.sourceAddress() != null)
                throw new RuntimeException("key is source specific");

            // drop membership
            key.drop();
            if (key.isValid())
            {
                throw new RuntimeException("key is still valid");
            }

            // source-specific
            try
            {
                key = dc.join(group, nif, source);
                other = dc.join(group, nif, source);
                if (other != key)
                {
                    throw new RuntimeException("existing key not returned");
                }
                if (!key.isValid())
                    throw new RuntimeException("key is not valid");
                if (!key.group().equals(group))
                    throw new RuntimeException("group is incorrect");
                if (!key.networkInterface().equals(nif))
                    throw new RuntimeException("network interface is incorrect");
                if (!key.sourceAddress().equals(source))
                    throw new RuntimeException("key's source address incorrect");

                // drop membership
                key.drop();
                if (key.isValid())
                {
                    throw new RuntimeException("key is still valid");
                }
            }
            catch (UnsupportedOperationException x)
            {
            }

            // done
            dc.close();
        }

        /**
         * Tests exceptions for invalid arguments or scenarios
         */
        void ExceptionTests(NetworkInterface nif)
        {
            var dc = DatagramChannel.open(StandardProtocolFamily.INET).setOption(StandardSocketOptions.SO_REUSEADDR, global::java.lang.Boolean.TRUE).bind(new InetSocketAddress(0));
            var group = InetAddress.getByName("225.4.5.6");
            var notGroup = InetAddress.getByName("1.2.3.4");
            var thisHost = InetAddress.getLocalHost();

            // IllegalStateException
            MembershipKey key;
            key = dc.join(group, nif);
            try
            {
                dc.join(group, nif, thisHost);
                throw new RuntimeException("IllegalStateException not thrown");
            }
            catch (IllegalStateException x)
            {
            }
            catch (UnsupportedOperationException x)
            {
            }
            key.drop();
            try
            {
                key = dc.join(group, nif, thisHost);
                try
                {
                    dc.join(group, nif);
                    throw new RuntimeException("IllegalStateException not thrown");
                }
                catch (IllegalStateException x)
                {
                }
                key.drop();
            }
            catch (UnsupportedOperationException x)
            {
            }

            // IllegalArgumentException
            try
            {
                dc.join(notGroup, nif);
                throw new RuntimeException("IllegalArgumentException not thrown");
            }
            catch (IllegalArgumentException x)
            {

            }

            try
            {
                dc.join(notGroup, nif, thisHost);
                throw new RuntimeException("IllegalArgumentException not thrown");
            }
            catch (IllegalArgumentException x)
            {

            }
            catch (UnsupportedOperationException x)
            {

            }

            // NullPointerException
            try
            {
                dc.join(null, nif);
                throw new RuntimeException("NullPointerException not thrown");
            }
            catch (NullReferenceException x)
            {

            }

            try
            {
                dc.join(group, null);
                throw new RuntimeException("NullPointerException not thrown");
            }
            catch (NullReferenceException x)
            {

            }

            try
            {
                dc.join(group, nif, null);
                throw new RuntimeException("NullPointerException not thrown");
            }
            catch (NullPointerException x)
            {

            }
            catch (UnsupportedOperationException x)
            {

            }

            dc.close();

            // ClosedChannelException
            try
            {
                dc.join(group, nif);
                throw new RuntimeException("ClosedChannelException not thrown");
            }
            catch (ClosedChannelException x)
            {

            }

            try
            {
                dc.join(group, nif, thisHost);
                throw new RuntimeException("ClosedChannelException not thrown");
            }
            catch (ClosedChannelException x)
            {

            }
            catch (UnsupportedOperationException x)
            {

            }
        }

        [TestMethod]
        public void MulticastTests()
        {
            // multicast groups used for the test
            InetAddress ip4Group = InetAddress.getByName("225.4.5.6");
            InetAddress ip6Group = InetAddress.getByName("ff02::a");


            NetworkConfiguration config = NetworkConfiguration.probe();

            NetworkInterface nif = config.Ip4Interfaces().FirstOrDefault();
            InetAddress anySource = config.Ip4Addresses(nif).FirstOrDefault();
            MembershipKeyTests(nif, ip4Group, anySource);
            ExceptionTests(nif);

            // re-run the membership key tests with IPv6 if available

            nif = config.Ip6Interfaces().FirstOrDefault();
            if (nif != null)
            {
                anySource = config.Ip6Addresses(nif).FirstOrDefault();
                MembershipKeyTests(nif, ip6Group, anySource);
            }
        }

        class NetworkConfiguration
        {

            private Dictionary<NetworkInterface, List<InetAddress>> ip4Interfaces;
            private Dictionary<NetworkInterface, List<InetAddress>> ip6Interfaces;

            private NetworkConfiguration(Dictionary<NetworkInterface, List<InetAddress>> ip4Interfaces, Dictionary<NetworkInterface, List<InetAddress>> ip6Interfaces)
            {
                this.ip4Interfaces = ip4Interfaces;
                this.ip6Interfaces = ip6Interfaces;
            }

            public IEnumerable<NetworkInterface> Ip4Interfaces()
            {
                return ip4Interfaces.Keys;
            }

            public IEnumerable<NetworkInterface> Ip6Interfaces()
            {
                return ip6Interfaces.Keys;
            }

            public IEnumerable<InetAddress> Ip4Addresses(NetworkInterface nif)
            {
                return ip4Interfaces.TryGetValue(nif, out var aa) ? aa : null;
            }

            public IEnumerable<InetAddress> Ip6Addresses(NetworkInterface nif)
            {
                return ip6Interfaces.TryGetValue(nif, out var aa) ? aa : null;
            }

            // IPv6 not supported for Windows XP/Server 2003
            static bool isIPv6Supported()
            {
                if (global::java.lang.System.getProperty("os.name").StartsWith("Windows"))
                {
                    string ver = global::java.lang.System.getProperty("os.version");
                    int major = Integer.parseInt(ver.Split('.')[0]);
                    return (major >= 6);
                }
                return true;
            }

            public static NetworkConfiguration probe()
            {
                Dictionary<NetworkInterface, List<InetAddress>> ip4Interfaces = new Dictionary<NetworkInterface, List<InetAddress>>();
                Dictionary<NetworkInterface, List<InetAddress>> ip6Interfaces = new Dictionary<NetworkInterface, List<InetAddress>>();

                // find the interfaces that support IPv4 and IPv6
                var nifs = NetworkInterface.getNetworkInterfaces().AsEnumerable<NetworkInterface>();
                foreach (NetworkInterface nif in nifs)
                {
                    // ignore intertaces that are down or don't support multicast
                    if (!nif.isUp() || !nif.supportsMulticast() || nif.isLoopback())
                        continue;

                    var addrs = nif.getInetAddresses().AsEnumerable<InetAddress>();
                    foreach (InetAddress addr in addrs)
                    {
                        if (!addr.isAnyLocalAddress())
                        {
                            if (addr is Inet4Address)
                            {
                                List<InetAddress> list = ip4Interfaces.TryGetValue(nif, out var ip4) ? ip4 : null;
                                if (list == null)
                                {
                                    list = new List<InetAddress>();
                                }
                                list.Add(addr);
                                ip4Interfaces[nif] = list;
                            }
                            else if (addr is Inet6Address)
                            {
                                List<InetAddress> list = ip6Interfaces.TryGetValue(nif, out var ip6) ? ip6 : null;
                                if (list == null)
                                {
                                    list = new List<InetAddress>();
                                }
                                list.Add(addr);
                                ip6Interfaces[nif] = list;
                            }
                        }
                    }
                }

                return new NetworkConfiguration(ip4Interfaces, ip6Interfaces);
            }

        }

    }

}