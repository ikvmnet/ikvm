using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Defining a class file with an unsupported version must throw exactly what HotSpot throws. The expected messages were
    /// captured from OpenJDK 8.
    /// </summary>
    [TestClass]
    public class UnsupportedClassVersionTests
    {

        static readonly global::sun.misc.Unsafe u;

        static UnsupportedClassVersionTests()
        {
            var f = ((global::java.lang.Class)typeof(global::sun.misc.Unsafe)).getDeclaredField("theUnsafe");
            f.setAccessible(true);
            u = (global::sun.misc.Unsafe)f.get(null);
        }

        static byte[] BuildClass(ushort major, ushort minor)
        {
            var builder = new ClassFileBuilder(new ClassFormatVersion(major, minor), AccessFlag.Public | AccessFlag.Super, "p/A", "java/lang/Object");
            var buffer = new BlobBuilder();
            builder.Serialize(buffer);
            return buffer.ToArray();
        }

        [DataTestMethod]
        [DataRow(55, 0, "p/A has been compiled by a more recent version of the Java Runtime (class file version 55.0), this version of the Java Runtime only recognizes class file versions up to 52.0")]
        [DataRow(61, 65535, "p/A has been compiled by a more recent version of the Java Runtime (class file version 61.65535), this version of the Java Runtime only recognizes class file versions up to 52.0")]
        [DataRow(44, 0, "p/A has been compiled by a more recent version of the Java Runtime (class file version 44.0), this version of the Java Runtime only recognizes class file versions up to 52.0")]
        [DataRow(255, 0, "p/A has been compiled by a more recent version of the Java Runtime (class file version 255.0), this version of the Java Runtime only recognizes class file versions up to 52.0")]
        public void DefineClassShouldThrowUnsupportedClassVersionError(int major, int minor, string message)
        {
            var bytes = BuildClass((ushort)major, (ushort)minor);
            FluentActions.Invoking(() => new ByteArrayClassLoader().Define("p.A", bytes))
                .Should().Throw<global::java.lang.UnsupportedClassVersionError>()
                .Which.getMessage().Should().Be(message);
        }

        [DataTestMethod]
        [DataRow(55)]
        [DataRow(255)]
        public void DefineClassWithoutNameShouldThrowUnsupportedClassVersionError(int major)
        {
            var bytes = BuildClass((ushort)major, 0);
            FluentActions.Invoking(() => new ByteArrayClassLoader().Define(null, bytes))
                .Should().Throw<global::java.lang.UnsupportedClassVersionError>()
                .Which.getMessage().Should().Be($"Unsupported class file version {major}.0, this version of the Java Runtime only recognizes class file versions up to 52.0");
        }

        [DataTestMethod]
        [DataRow(55)]
        [DataRow(255)]
        public void DefineAnonymousClassShouldThrowUnsupportedClassVersionError(int major)
        {
            var bytes = BuildClass((ushort)major, 0);
            FluentActions.Invoking(() => u.defineAnonymousClass((global::java.lang.Class)typeof(UnsupportedClassVersionTests), bytes, null))
                .Should().Throw<global::java.lang.UnsupportedClassVersionError>()
                .Which.getMessage().Should().Be($"Unsupported class file version {major}.0, this version of the Java Runtime only recognizes class file versions up to 52.0");
        }

    }

}
