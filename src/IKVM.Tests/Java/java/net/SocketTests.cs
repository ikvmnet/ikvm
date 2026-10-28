using System.Linq;
using System.Threading.Tasks;

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
            using var s1 = new Socket();
            s1.getReuseAddress().Should().BeFalse();
            s1.setReuseAddress(true);
            s1.getReuseAddress().Should().BeTrue();
            s1.setReuseAddress(false);
            s1.getReuseAddress().Should().BeFalse();
            s1.close();
        }

        [TestMethod]
        public void BindingToSamePortShouldThrowBindException()
        {
            using var s1 = new ServerSocket();
            s1.bind(new InetSocketAddress(0));
            using var s2 = new Socket();
            s2.setReuseAddress(false);
            Assert.ThrowsExactly<BindException>(() => s2.bind(new InetSocketAddress(s1.getLocalPort())));
        }

        [TestMethod]
        public void AcceptShouldTimeOut()
        {
            using var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            Assert.ThrowsExactly<SocketTimeoutException>(server.accept);
        }

        [TestMethod]
        public void ConnectShouldNotTimeOut()
        {
            using var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            int port = server.getLocalPort();

            var dest = NetworkInterface.getNetworkInterfaces()
                .AsEnumerable<NetworkInterface>()
                .Where(i => i.isUp())
                .SelectMany(i => i.getInterfaceAddresses().AsEnumerable<InterfaceAddress>())
                .Select(i => i.getAddress())
                .OfType<Inet4Address>()
                .FirstOrDefault(i => i.isLoopbackAddress() == false && !i.isAnyLocalAddress());

            using var c1 = new Socket();
            c1.connect(new InetSocketAddress(dest, port), 1000);
            using var s1 = server.accept();
        }

        [TestMethod]
        public void ConnectWithNoListeningServerShouldThrowConnectException()
        {
            using var server = new ServerSocket(0);
            int port = server.getLocalPort();
            server.close();
            using var client = new Socket();
            Assert.ThrowsExactly<ConnectException>(() => client.connect(new InetSocketAddress(InetAddress.getByName("127.0.0.1"), port), 8888));
        }

        [TestMethod]
        public void ShouldThrowSocketExceptionWhenClosedDuringRead()
        {
            var b = false;

            var ss = new ServerSocket(0);
            var lh = InetAddress.getLocalHost();
            var s = new Socket();
            s.connect(new InetSocketAddress(lh, ss.getLocalPort()));

            var s2 = ss.accept();
            var t = Task.Run(() =>
            {
                try
                {
                    var i = s.getInputStream();
                    var n = i.read();
                    throw new System.Exception();
                }
                catch (SocketException)
                {
                    b = true;
                }
                catch (Exception)
                {
                    throw;
                }
            });

            Thread.sleep(1000);
            s.close();
            Thread.sleep(15000);
            b.Should().BeTrue();
            t.Wait();
        }

        [TestMethod]
        public void ShouldThrowSocketExceptionWhenClosedDuringReadWithTimeout()
        {
            var b = false;

            var ss = new ServerSocket(0);
            var lh = InetAddress.getLocalHost();
            var s = new Socket();
            s.connect(new InetSocketAddress(lh, ss.getLocalPort()));

            var s2 = ss.accept();
            var t = Task.Run(() =>
            {
                try
                {
                    s.setSoTimeout(15000);
                    var i = s.getInputStream();
                    var n = i.read();
                    throw new System.Exception();
                }
                catch (SocketException)
                {
                    b = true;
                }
                catch (Exception)
                {
                    throw;
                }
            });

            Thread.sleep(1000);
            s.close();
            Thread.sleep(5000);
            b.Should().BeTrue();
            t.Wait();
        }

    }

}
