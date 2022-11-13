using FluentAssertions;

using java.net;
using java.nio.channels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    }

}