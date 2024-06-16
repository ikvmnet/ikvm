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
            l.Should().Contain(i => i.toString() == "SUN version 1.8");
            l.Should().Contain(i => i.toString() == "SunRsaSign version 1.8");
            l.Should().Contain(i => i.toString() == "SunEC version 1.8");
            l.Should().Contain(i => i.toString() == "SunJSSE version 1.8");
            l.Should().Contain(i => i.toString() == "SunJCE version 1.8");
            l.Should().Contain(i => i.toString() == "SunJGSS version 1.8");
            l.Should().Contain(i => i.toString() == "SunSASL version 1.8");
            l.Should().Contain(i => i.toString() == "XMLDSig version 1.8");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                l.Should().Contain(i => i.toString() == "SunMSCAPI version 1.8");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                l.Should().Contain(i => i.toString() == "Apple version 1.8");
        }

    }

}
