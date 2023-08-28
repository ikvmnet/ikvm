using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using java.lang;
using java.net;
using java.nio;
using java.nio.channels;
using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class DatagramChannelTests
    {

        [TestMethod]
        public void CanBindNull()
        {
            using var dc = DatagramChannel.open();
            dc.bind(null);
        }

        [TestMethod]
        public void CanBindNullIPv4()
        {
            using var dc = DatagramChannel.open(StandardProtocolFamily.INET);
            dc.bind(null);
        }

        [TestMethod]
        public void CanBindNullIPv6()
        {
            using var dc = DatagramChannel.open(StandardProtocolFamily.INET6);
            dc.bind(null);
        }

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

        [TestMethod]
        public void CanSendWhileConnected()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            using var sndChannel = DatagramChannel.open();
            sndChannel.socket().bind(null);

            var address = InetAddress.getLocalHost();
            if (address.isLoopbackAddress())
                address = InetAddress.getLoopbackAddress();

            var sender = new InetSocketAddress(address, sndChannel.socket().getLocalPort());

            using var rcvChannel = DatagramChannel.open();
            rcvChannel.socket().bind(null);
            var receiver = new InetSocketAddress(address, rcvChannel.socket().getLocalPort());

            rcvChannel.connect(sender);
            sndChannel.connect(receiver);

            var bb = ByteBuffer.allocate(256);
            bb.put(System.Text.Encoding.ASCII.GetBytes("hello"));
            bb.flip();

            var sent = sndChannel.send(bb, receiver);
            bb.clear();
            rcvChannel.receive(bb);
            bb.flip();

            var cb = System.Text.Encoding.ASCII.GetString(bb.array(), bb.arrayOffset() + bb.position(), bb.remaining());
            cb.Should().Be("hello");

            rcvChannel.close();
            sndChannel.close();
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void ShouldFailAdhocSendWhenConnected()
        {
            var dc = DatagramChannel.open().bind(new InetSocketAddress(0));
            var sa = new InetSocketAddress("127.0.0.1", 14121);
            dc.connect(sa);

            var bd = new InetSocketAddress("127.0.0.1", 14122);
            var bb = ByteBuffer.allocateDirect(256);
            dc.send(bb, bd);
        }

        /// <summary>
        /// Tests that interfaces can join an IPv6 multicast group. Sourced from jdk/java/nio/channels/DatagramChannel/BasicMulticastTests.
        /// </summary>
        /// <exception cref="RuntimeException"></exception>
        [TestMethod]
        public void CanJoinIPv6MulticastGroupOnInterfaces()
        {
            var group = InetAddress.getByName("ff02::a");

            foreach (var nif in NetworkInterface.getNetworkInterfaces().AsEnumerable<NetworkInterface>())
            {
                var source = nif.getInetAddresses().AsEnumerable<InetAddress>().OfType<Inet6Address>().Where(i => !i.isAnyLocalAddress()).FirstOrDefault();
                if (source == null)
                    continue;

                var dc = DatagramChannel.open(StandardProtocolFamily.INET6)
                    .setOption(StandardSocketOptions.SO_REUSEADDR, new Boolean(true))
                    .bind(new InetSocketAddress(source, 0));

                var thisKey = dc.join(group, nif);
                var sameKey = dc.join(group, nif);
                sameKey.Should().BeSameAs(thisKey);

                // check key
                if (!thisKey.isValid())
                    throw new RuntimeException("key is not valid");
                if (!thisKey.group().equals(group))
                    throw new RuntimeException("group is incorrect");
                if (!thisKey.networkInterface().equals(nif))
                    throw new RuntimeException("network interface is incorrect");
                if (thisKey.sourceAddress() != null)
                    throw new RuntimeException("key is source specific");

                // drop membership
                thisKey.drop();
                if (thisKey.isValid())
                    throw new RuntimeException("key is still valid");

                // source-specific
                try
                {
                    thisKey = dc.join(group, nif, source);
                    sameKey = dc.join(group, nif, source);
                    sameKey.Should().BeSameAs(thisKey);

                    if (!thisKey.isValid())
                        throw new RuntimeException("key is not valid");
                    if (!thisKey.group().equals(group))
                        throw new RuntimeException("group is incorrect");
                    if (!thisKey.networkInterface().equals(nif))
                        throw new RuntimeException("network interface is incorrect");
                    if (!thisKey.sourceAddress().equals(source))
                        throw new RuntimeException("key's source address incorrect");

                    // drop membership
                    thisKey.drop();
                    if (thisKey.isValid())
                        throw new RuntimeException("key is still valid");
                }
                catch (UnsupportedOperationException e)
                {

                }

                dc.close();
            }
        }

    }

}
