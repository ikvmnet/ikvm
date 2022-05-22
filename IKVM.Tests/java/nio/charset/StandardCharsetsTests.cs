using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

namespace IKVM.Tests.java.nio.charset
{

    [TestClass]
    public class StandardCharsetsTests
    {

        [TestMethod]
        public void Can_init_StandardCharsets()
        {
            global::java.nio.charset.StandardCharsets.ISO_8859_1.Should().NotBeNull();
            global::java.nio.charset.StandardCharsets.US_ASCII.Should().NotBeNull();
            global::java.nio.charset.StandardCharsets.UTF_16.Should().NotBeNull();
            global::java.nio.charset.StandardCharsets.UTF_16BE.Should().NotBeNull();
            global::java.nio.charset.StandardCharsets.UTF_16LE.Should().NotBeNull();
            global::java.nio.charset.StandardCharsets.UTF_8.Should().NotBeNull();
        }

    }

}
