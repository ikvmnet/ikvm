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
    public class Inet6AddressTests
    {

        [TestMethod]
        public void ScopeShouldAppearInLinkLocalAddress()
        {
            var dest = NetworkInterface.getNetworkInterfaces()
                .AsEnumerable<NetworkInterface>()
                .Where(i => i.isUp())
                .SelectMany(i => i.getInterfaceAddresses().AsEnumerable<InterfaceAddress>())
                .Select(i => i.getAddress())
                .OfType<Inet6Address>()
                .FirstOrDefault(i => i.isLinkLocalAddress());

            var sock = new ServerSocket(0);
            var port = sock.getLocalPort();

            var test = Task.Run(() =>
            {
                var s = new Socket(dest, port);
                var n = s.getInputStream();
                var i = n.read();
                n.close();
            });

            var s = sock.accept();
            var a = s.getInetAddress();
            var o = s.getOutputStream();
            o.write(1);
            o.close();
            a.Should().BeOfType<Inet6Address>();
            a.getHostAddress().Should().Contain("%");

            test.Wait();
        }

    }

}
