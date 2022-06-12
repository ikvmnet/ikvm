using System.Text;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using java.nio;
using java.nio.charset;

namespace IKVM.Tests.Java.java.nio.charset
{

    [TestClass]
    public class StandardCharsetsTests
    {

        /// <summary>
        /// Ensures we can initialize the standard character sets.
        /// </summary>
        [TestMethod]
        public void Can_init_StandardCharsets()
        {
            StandardCharsets.ISO_8859_1.Should().NotBeNull();
            StandardCharsets.US_ASCII.Should().NotBeNull();
            StandardCharsets.UTF_16.Should().NotBeNull();
            StandardCharsets.UTF_16BE.Should().NotBeNull();
            StandardCharsets.UTF_16LE.Should().NotBeNull();
            StandardCharsets.UTF_8.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_encode_ISO_8859_1()
        {
            StandardCharsets.ISO_8859_1.encode("test").array().Should().BeEquivalentTo(Encoding.GetEncoding("ISO-8859-1").GetBytes("test"));
        }

        [TestMethod]
        public void Can_decode_ISO_8859_1()
        {
            StandardCharsets.ISO_8859_1.decode(ByteBuffer.wrap(Encoding.GetEncoding("ISO-8859-1").GetBytes("test"))).ToString().Should().Be("test");
        }

        [TestMethod]
        public void Can_encode_US_ASCII()
        {
            StandardCharsets.US_ASCII.encode("test").array().Should().BeEquivalentTo(Encoding.ASCII.GetBytes("test"));
        }

        [TestMethod]
        public void Can_decode_US_ASCII()
        {
            StandardCharsets.US_ASCII.decode(ByteBuffer.wrap(Encoding.ASCII.GetBytes("test"))).ToString().Should().Be("test");
        }

        [TestMethod]
        public void Can_encode_UTF_16BE()
        {
            StandardCharsets.UTF_16BE.encode("test").array().Should().BeEquivalentTo(Encoding.GetEncoding("UTF-16BE").GetBytes("test"));
        }

        [TestMethod]
        public void Can_decode_UTF_16BE()
        {
            StandardCharsets.UTF_16BE.decode(ByteBuffer.wrap(Encoding.GetEncoding("UTF-16BE").GetBytes("test"))).ToString().Should().Be("test");
        }

        [TestMethod]
        public void Can_encode_UTF_16LE()
        {
            StandardCharsets.UTF_16LE.encode("test").array().Should().BeEquivalentTo(Encoding.GetEncoding("UTF-16LE").GetBytes("test"));
        }

        [TestMethod]
        public void Can_decode_UTF_16LE()
        {
            StandardCharsets.UTF_16LE.decode(ByteBuffer.wrap(Encoding.GetEncoding("UTF-16LE").GetBytes("test"))).ToString().Should().Be("test");
        }

        [TestMethod]
        public void Can_encode_UTF_8()
        {
            StandardCharsets.UTF_8.encode("test").array().Should().BeEquivalentTo(Encoding.UTF8.GetBytes("test"));
        }

        [TestMethod]
        public void Can_decode_UTF_8()
        {
            StandardCharsets.UTF_8.decode(ByteBuffer.wrap(Encoding.UTF8.GetBytes("test"))).ToString().Should().Be("test");
        }

    }

}
