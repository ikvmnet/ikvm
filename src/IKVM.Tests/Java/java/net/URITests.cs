using FluentAssertions;

using java.nio.file;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class URITests
    {

        [TestMethod]
        public void CanCreateHttpURI()
        {
            var u = new global::java.net.URI("http://www.site.com/");
            u.getScheme().Should().Be("http");
            u.getHost().Should().Be("www.site.com");
            u.getPath().Should().Be("/");
        }

        [TestMethod]
        public void CanCreateHttpsURI()
        {
            var u = new global::java.net.URI("https://www.site.com/");
            u.getScheme().Should().Be("https");
            u.getHost().Should().Be("www.site.com");
            u.getPath().Should().Be("/");
        }

        [TestMethod]
        public void CanCreateWindowsFileURI()
        {
            var u = new global::java.net.URI("file:///c:/dir");
            u.getScheme().Should().Be("file");
            u.getHost().Should().Be(null);
            u.getPath().Should().Be("/c:/dir");
        }

        [TestMethod]
        public void CanCreateUnixFileURI()
        {
            var u = new global::java.net.URI("file:///tmp/foo");
            u.getScheme().Should().Be("file");
            u.getHost().Should().Be(null);
            u.getPath().Should().Be("/tmp/foo");
        }

    }

}
