using FluentAssertions;

using java.nio.charset;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.charset
{

    [TestClass]
    public class CharsetTests
    {

        [TestMethod]
        public void CanGetDefaultCharsets()
        {
            Charset.defaultCharset();
        }

        [TestMethod]
        public void CanGetAvailableCharsets()
        {
            Charset.availableCharsets();
        }

        [DataTestMethod]
        [DataRow("cp437")]
        [DataRow("ms850")]
        [DataRow("ms852")]
        [DataRow("ms855")]
        [DataRow("ms857")]
        [DataRow("ms860")]
        [DataRow("ms861")]
        [DataRow("ms863")]
        [DataRow("ms865")]
        [DataRow("ms866")]
        [DataRow("ms869")]
        [DataRow("ms936")]
        [DataRow("cp65001")]
        public void CanEncodeAndDecode(string charset)
        {
            var o = Charset.forName(charset).encode("test");
            var i = Charset.forName(charset).decode(o);
            i.Should().Be("test");
        }

    }

}
