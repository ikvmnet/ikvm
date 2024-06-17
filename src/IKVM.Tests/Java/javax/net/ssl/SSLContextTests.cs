using FluentAssertions;

using javax.net.ssl;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.javax.net.ssl
{

    [TestClass]
    public class SSLContextTests
    {

        [TestMethod]
        public void CanGetDefaultCipherSuites()
        {
            var ctx = SSLContext.getDefault();
            var ptm = ctx.getDefaultSSLParameters();
            var lst = ptm.getCipherSuites();
            lst.Should().NotBeNullOrEmpty();
        }

    }

}
