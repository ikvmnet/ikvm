using System.Runtime.InteropServices;

using FluentAssertions;

using java.lang;
using java.net;
using java.nio;
using java.nio.channels;

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

    }

}
