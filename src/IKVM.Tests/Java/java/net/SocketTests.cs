using FluentAssertions;

using java.net;

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

    }

}
