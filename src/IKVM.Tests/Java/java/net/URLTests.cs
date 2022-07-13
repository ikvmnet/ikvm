using System.Text;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class URLTests
    {

        [TestMethod]
        public void Can_fetch_http_url()
        {
            var stm = new global::java.net.URL("http://ptsv2.com/t/utqb9-1657681652/post").openStream();
            var buf = new byte[1024];
            var bytesRead = stm.read(buf, 0, 1024);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Be("Thank you for this dump. I hope you have a lovely day!");
        }

    }

}
