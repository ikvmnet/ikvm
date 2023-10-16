using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class StringBufferTests
    {

        [TestMethod]
        public void Can_create_StringBuffer()
        {
            var b = new global::java.lang.StringBuffer("value");
            b.length().Should().Be(5);
        }

    }

}
