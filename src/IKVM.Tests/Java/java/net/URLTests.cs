using System.Text;
using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.Json.Nodes;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class URLTests
    {

        [TestMethod]
        public void CanGetFromPlainURL()
        {
            var stm = new global::java.net.URL("http://http.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("http.badssl.com");
        }

        [TestMethod]
        public void CanGetFromTLS10Url()
        {
            var stm = new global::java.net.URL("https://tls-v1-0.badssl.com:1010/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("tls-v1-0.badssl.com");
        }

        [TestMethod]
        public void CanGetFromTLS11Url()
        {
            var stm = new global::java.net.URL("https://tls-v1-1.badssl.com:1011/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("tls-v1-1.badssl.com");
        }

        [TestMethod]
        public void CanGetFromTLS12Url()
        {
            var stm = new global::java.net.URL("https://tls-v1-2.badssl.com:1012/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("tls-v1-2.badssl.com");
        }

        [TestMethod]
        public void CanGetFromSha256Url()
        {
            var stm = new global::java.net.URL("https://sha256.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("sha256.badssl.com");
        }

        [TestMethod]
        public void CanGetFromRsa2048Url()
        {
            var stm = new global::java.net.URL("https://rsa2048.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("rsa2048.badssl.com");
        }

        [TestMethod]
        public void CanGetFromRsa8192Url()
        {
            var stm = new global::java.net.URL("https://rsa8192.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("rsa8192.badssl.com");
        }

        [TestMethod]
        [Ignore]
        public void CanGetFromEcc256Url()
        {
            var stm = new global::java.net.URL("https://ecc256.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("ecc256.badssl.com");
        }

        [TestMethod]
        [Ignore]
        public void CanGetFromEcc384Url()
        {
            var stm = new global::java.net.URL("https://ecc384.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("ecc384.badssl.com");
        }

        [TestMethod]
        [Ignore]
        public void CanGetFromDh480Url()
        {
            var stm = new global::java.net.URL("https://dh480.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("dh480.badssl.com");
        }

        [TestMethod]
        public void CanGetFromDh512Url()
        {
            var stm = new global::java.net.URL("https://dh512.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("dh512.badssl.com");
        }

        [TestMethod]
        public void CanGetFromDh1024Url()
        {
            var stm = new global::java.net.URL("https://dh1024.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain("dh1024.badssl.com");
        }

    }

}
