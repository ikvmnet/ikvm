using System.Linq;

using FluentAssertions;

using java.lang;
using java.net;
using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class SocketTests
    {

        [TestMethod]
        public void ReuseAddressShouldWork()
        {
            var s1 = new Socket();
            s1.getReuseAddress().Should().BeFalse();
            s1.setReuseAddress(true);
            s1.getReuseAddress().Should().BeTrue();
            s1.setReuseAddress(false);
            s1.getReuseAddress().Should().BeFalse();
            s1.close();
        }

        [TestMethod]
        [ExpectedException(typeof(BindException))]
        public void BindingToSamePortShouldThrowBindException()
        {
            var s1 = new Socket();
            s1.bind(new InetSocketAddress(0));
            var s2 = new Socket();
            s2.bind(new InetSocketAddress(s1.getLocalPort()));
            s2.close();
            s1.close();
        }

        [TestMethod]
        [ExpectedException(typeof(SocketTimeoutException))]
        public void AcceptShouldTimeOut()
        {
            var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            server.accept();
            throw new System.Exception("accept should not have returned");
        }

        [TestMethod]
        public void ConnectShouldNotTimeOut()
        {
            var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            int port = server.getLocalPort();

            var dest = NetworkInterface.getNetworkInterfaces()
                .AsEnumerable<NetworkInterface>()
                .Where(i => i.isUp())
                .SelectMany(i => i.getInterfaceAddresses().AsEnumerable<InterfaceAddress>())
                .Select(i => i.getAddress())
                .OfType<Inet4Address>()
                .FirstOrDefault(i => i.isLoopbackAddress() == false && !i.isAnyLocalAddress());

            var c1 = new Socket();
            c1.connect(new InetSocketAddress(dest, port), 1000);
            var s1 = server.accept();
        }

        [TestMethod]
        [ExpectedException(typeof(ConnectException))]
        public void ConnectWithNoListeningServerShouldThrowConnectException()
        {
            var server = new ServerSocket(0);
            int port = server.getLocalPort();
            server.close();
            var client = new Socket();
            client.connect(new InetSocketAddress(InetAddress.getByName("127.0.0.1"), port), 2000);
        }

    }

}
