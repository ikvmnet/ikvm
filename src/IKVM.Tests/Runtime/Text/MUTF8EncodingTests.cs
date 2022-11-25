using System;

using FluentAssertions;

using IKVM.Runtime.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Text
{

    [TestClass]
    public class MUTF8EncodingTests
    {

        [TestMethod]
        public unsafe void CanFindNullByte()
        {
            fixed (byte* ptr = new byte[] { 0x01, 0x00 })
                MUTF8Encoding.IndexOfNull(ptr).Should().Be(1);
            fixed (byte* ptr = new byte[] { 0x00, 0x00 })
                MUTF8Encoding.IndexOfNull(ptr).Should().Be(0);
            fixed (byte* ptr = new byte[] { 0x01, 0x01, 0x00 })
                MUTF8Encoding.IndexOfNull(ptr).Should().Be(2);
        }

        [TestMethod]
        public void CanEncodeNull()
        {
            MUTF8Encoding.MUTF8.GetBytes("\0").Should().HaveCount(2);
            MUTF8Encoding.MUTF8.GetBytes("a\0").Should().HaveCount(3);
            MUTF8Encoding.MUTF8.GetBytes("a\0a").Should().HaveCount(4);
            MUTF8Encoding.MUTF8.GetBytes("\0\0").Should().HaveCount(4);
            MUTF8Encoding.MUTF8.GetBytes("a\0\0").Should().HaveCount(5);
            MUTF8Encoding.MUTF8.GetBytes("a\0\0a").Should().HaveCount(6);
        }

        [TestMethod]
        public void CanHandleEmptyString()
        {
            MUTF8Encoding.MUTF8.GetBytes("").Should().BeEmpty();
            MUTF8Encoding.MUTF8.GetString(Array.Empty<byte>()).Should().Be("");
        }

    }

}
