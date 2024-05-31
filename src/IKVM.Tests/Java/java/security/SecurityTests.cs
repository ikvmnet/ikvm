using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.security
{

    [TestClass]
    public class SecurityTests
    {

        [TestMethod]
        public void CanGetProviders()
        {
            var l = global::java.security.Security.getProviders();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                l.Should().HaveCount(9);
            else
                l.Should().HaveCount(8);
        }

    }

}
