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
        [DataRow("cp850")]
        [DataRow("cp852")]
        [DataRow("cp855")]
        [DataRow("cp857")]
        [DataRow("cp860")]
        [DataRow("cp861")]
        [DataRow("cp863")]
        [DataRow("cp865")]
        [DataRow("cp866")]
        [DataRow("cp869")]
        [DataRow("cp936")]
        public void CanEncodeAndDecode(string charset)
        {
            var o = Charset.forName(charset).encode("test");
            var i = Charset.forName(charset).decode(o);
            i.toString().Should().Be("test");
        }

    }

}
