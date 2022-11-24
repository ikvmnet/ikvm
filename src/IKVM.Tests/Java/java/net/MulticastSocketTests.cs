using System.Text;

using FluentAssertions;

using java.net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{


    [TestClass]
    public class MulticastSocketTests
    {

        [TestMethod]
        public void DefaultTimeToLive()
        {
            using var mc = new MulticastSocket();
            mc.getTimeToLive().Should().Be(1);
        }

        [TestMethod]
        public void GetLoopbackMode()
        {
            using var mc = new MulticastSocket();
            mc.getLoopbackMode().Should().Be(false);
            mc.setLoopbackMode(true);
            mc.getLoopbackMode().Should().Be(true);
            mc.setLoopbackMode(false);
            mc.getLoopbackMode().Should().Be(false);
        }

        [TestMethod]
        public void SetLoopbackMode()
        {
            using var mc = new MulticastSocket();
            var grp = InetAddress.getByName("ff01::1");

            // join group
            mc.joinGroup(grp);

            // sends a test packet to the group then returns whether it was received
            bool Test()
            {
                var b = Encoding.UTF8.GetBytes("hello");
                var p = new DatagramPacket(b, b.Length, grp, mc.getLocalPort());
                mc.send(p);
                mc.setSoTimeout(1000);

                var s = false;
                try
                {
                    // attempt to receive multicast packet
                    b = new byte[16];
                    p = new DatagramPacket(b, b.Length);
                    mc.receive(p);
                    Encoding.UTF8.GetString(b, 0, p.getLength()).Should().Be("hello");
                    s = true;

                    /* purge any additional copies of the packet */
                    for (; ; )
                        mc.receive(p);
                }
                catch (SocketTimeoutException)
                {
                    // ignore
                }

                return s;
            }

            mc.setLoopbackMode(true);
            mc.getLoopbackMode().Should().BeTrue();
            Test().Should().BeFalse();

            mc.setLoopbackMode(false);
            mc.getLoopbackMode().Should().BeFalse();
            Test().Should().BeTrue();
        }

    }

}
