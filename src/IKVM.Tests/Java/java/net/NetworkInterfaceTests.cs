using System.Linq;

using FluentAssertions;

using java.net;
using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class NetworkInterfaceTests
    {

        [TestMethod]
        public void ShouldHaveAtLeast1NetworkInterface()
        {
            NetworkInterface.getNetworkInterfaces().AsEnumerable<NetworkInterface>().ToList().Should().HaveCountGreaterThan(0);
        }

    }

}
