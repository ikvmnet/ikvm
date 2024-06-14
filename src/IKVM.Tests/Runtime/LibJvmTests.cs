using System.Runtime.InteropServices;

using FluentAssertions;
using System.Buffers;

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
            LibJvm.JVM_CopySwapMemory(a, 0, b, 0, 4, 2);
            b.Should().Be(new short[] { BinaryPrimitives.ReverseEndianess(1), BinaryPrimitives.ReverseEndianess(2), BinaryPrimitives.ReverseEndianess(3), BinaryPrimitives.ReverseEndianess(4) });
        }

        [TestMethod]
        public void CanCopySwapInt32Array()
        {
            var a = new int[4] { 1, 2, 3, 4 };
            var b = new int[4];
            LibJvm.JVM_CopySwapMemory(a, 0, b, 0, 4, 4);
            b.Should().Be(new short[] { 256, 0, 0, 0 });
        }

        [TestMethod]
        public void CanCopySwapInt64Array()
        {
            var a = new long[4] { 1, 2, 3, 4 };
            var b = new long[4];
            LibJvm.JVM_CopySwapMemory(a, 0, b, 0, 4, 8);
            b.Should().Be(new short[] { 256, 0, 0, 0 });
        }

    }

}
