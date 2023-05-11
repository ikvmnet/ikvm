using System.Text;

using FluentAssertions;

using javax.net.ssl;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class URLTests
    {

        [TestMethod]
        public void CanCreateHttpURL()
        {
            var u = new global::java.net.URL("http://www.site.com/");
            u.getProtocol().Should().Be("http");
            u.getHost().Should().Be("www.site.com");
            u.getPath().Should().Be("/");
        }

        [TestMethod]
        public void CanCreateHttpsURL()
        {
            var u = new global::java.net.URL("https://www.site.com/");
            u.getProtocol().Should().Be("https");
            u.getHost().Should().Be("www.site.com");
            u.getPath().Should().Be("/");
        }

        [TestMethod]
        public void CanCreateWindowsFileURL()
        {
            var u = new global::java.net.URL("file:///c:/dir");
            u.getProtocol().Should().Be("file");
            u.getHost().Should().Be("");
            u.getPath().Should().Be("/c:/dir");
        }

        [TestMethod]
        public void CanCreateUnixFileURL()
        {
            var u = new global::java.net.URL("file:///tmp/foo");
            u.getProtocol().Should().Be("file");
            u.getHost().Should().Be("");
            u.getPath().Should().Be("/tmp/foo");
        }

        [DataTestMethod]
        [DataRow("http")]
        [DataRow("tls-v1-0")]
        [DataRow("tls-v1-1")]
        [DataRow("tls-v1-2")]
        [DataRow("sha256")]
        [DataRow("sha384")]
        [DataRow("sha512")]
        [DataRow("rsa2048")]
        [DataRow("rsa4096")]
        [DataRow("rsa8192")]
        [DataRow("ecc256")]
        [DataRow("ecc384")]
        [DataRow("dh480")]
        [DataRow("dh512")]
        [DataRow("dh1024")]
        public void CanGetFromURL(string prefix)
        {
            var stm = new global::java.net.URL($"https://{prefix}.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain($"{prefix}.badssl.com");
        }

    }

}
