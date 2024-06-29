using System.Buffers.Binary;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class LibJvmTests
    {

        [TestMethod]
        public void LoadLibraryShouldThrowLinkError()
        {
            var libpath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\badlib.dll" : "/libbadlib.so";
            LibJvm.Instance.Invoking(i => i.JVM_LoadLibrary(libpath)).Should().ThrowExactly<java.lang.UnsatisfiedLinkError>();
        }

        [TestMethod]
        public void CanCopySwapInt16Array()
        {
            var a = new short[4] { 1, 2, 3, 4 };
            var b = new short[4];
            LibJvm.JVM_CopySwapMemory(a, 0, b, 0, sizeof(short) * a.Length, sizeof(short));
            b[0].Should().Be(BinaryPrimitives.ReverseEndianness((short)1));
            b[1].Should().Be(BinaryPrimitives.ReverseEndianness((short)2));
            b[2].Should().Be(BinaryPrimitives.ReverseEndianness((short)3));
            b[3].Should().Be(BinaryPrimitives.ReverseEndianness((short)4));
        }

        [TestMethod]
        public void CanCopySwapInt32Array()
        {
            var a = new int[4] { 1, 2, 3, 4 };
            var b = new int[4];
            LibJvm.JVM_CopySwapMemory(a, 0, b, 0, sizeof(int) * a.Length, sizeof(int));
            b[0].Should().Be(BinaryPrimitives.ReverseEndianness((int)1));
            b[1].Should().Be(BinaryPrimitives.ReverseEndianness((int)2));
            b[2].Should().Be(BinaryPrimitives.ReverseEndianness((int)3));
            b[3].Should().Be(BinaryPrimitives.ReverseEndianness((int)4));
        }

        [TestMethod]
        public void CanCopySwapInt64Array()
        {
            var a = new long[4] { 1, 2, 3, 4 };
            var b = new long[4];
            LibJvm.JVM_CopySwapMemory(a, 0, b, 0, sizeof(long) * a.Length, sizeof(long));
            b[0].Should().Be(BinaryPrimitives.ReverseEndianness((long)1));
            b[1].Should().Be(BinaryPrimitives.ReverseEndianness((long)2));
            b[2].Should().Be(BinaryPrimitives.ReverseEndianness((long)3));
            b[3].Should().Be(BinaryPrimitives.ReverseEndianness((long)4));
        }

    }

}
