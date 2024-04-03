using FluentAssertions;

using IKVM.Runtime.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Extensions
{

    [TestClass]
    public class MemoryMarshalExtensionsTests
    {

        [TestMethod]
        public unsafe void CanFindNullByte()
        {
            fixed (byte* ptr = new byte[] { 0x01, 0x00 })
                MemoryMarshalExtensions.GetIndexOfNull(ptr).Should().Be(1);
            fixed (byte* ptr = new byte[] { 0x00, 0x00 })
                MemoryMarshalExtensions.GetIndexOfNull(ptr).Should().Be(0);
            fixed (byte* ptr = new byte[] { 0x01, 0x01, 0x00 })
                MemoryMarshalExtensions.GetIndexOfNull(ptr).Should().Be(2);
        }

    }

}
