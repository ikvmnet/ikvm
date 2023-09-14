using System.Linq;
using System.Runtime.InteropServices;
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

            using var sock = new ServerSocket(0);
            var port = sock.getLocalPort();

            var test = Task.Run(() =>
            {
                var s = new Socket(dest, port);
                var n = s.getInputStream();
                var i = n.read();
                n.close();
            });

            using var s = sock.accept();
            var a = s.getInetAddress();
            a.Should().BeOfType<Inet6Address>();
            a.getHostAddress().Should().Contain("%");

            using var o = s.getOutputStream();
            o.write(1);
            o.close();

            test.Wait();
        }

        [TestMethod]
        public void CanBindLocalWithoutScope()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            var addr = NetworkInterface.getNetworkInterfaces().AsEnumerable<NetworkInterface>()
                .SelectMany(i => i.getInetAddresses().AsEnumerable<InetAddress>())
                .OfType<Inet6Address>()
                .FirstOrDefault(i => i.isLinkLocalAddress());
            if (addr == null)
                throw new System.Exception("Could not find a link-local address");
            
            using var ss = new ServerSocket();
            ss.bind(new InetSocketAddress(addr, 0));

            // need to remove the %scope suffix
            var addr2 = (Inet6Address)InetAddress.getByAddress(addr.getAddress());
            using var ss2 = new ServerSocket();
            ss2.bind(new InetSocketAddress(addr2, 0));
        }

    }

}
