using System.IO;
using System.Text;

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

        /// <summary>
        /// Builds 'public class p.Old { public static int f() { return 42; } }' by hand, because class files before 45.3
        /// use a narrower Code attribute header (u1 max_stack, u1 max_locals, u2 code_length).
        /// </summary>
        static byte[] BuildOldClass(ushort major, ushort minor, bool oldCodeLayout)
        {
            var s = new MemoryStream();
            void U1(int v) => s.WriteByte((byte)v);
            void U2(int v) { U1(v >> 8); U1(v); }
            void U4(int v) { U2(v >> 16); U2(v); }
            void Utf8(string v) { var b = Encoding.UTF8.GetBytes(v); U1(1); U2(b.Length); s.Write(b, 0, b.Length); }

            U4(unchecked((int)0xCAFEBABE));
            U2(minor);
            U2(major);
            U2(8);
            Utf8("p/Old");              // #1
            U1(7); U2(1);               // #2 class p/Old
            Utf8("java/lang/Object");   // #3
            U1(7); U2(3);               // #4 class java/lang/Object
            Utf8("f");                  // #5
            Utf8("()I");                // #6
            Utf8("Code");               // #7
            U2(0x0021);                 // public super
            U2(2);
            U2(4);
            U2(0);                      // interfaces
            U2(0);                      // fields
            U2(1);                      // methods
            U2(0x0009);                 // public static
            U2(5);
            U2(6);
            U2(1);
            U2(7);
            if (oldCodeLayout)
            {
                U4(11);
                U1(1);                  // max_stack
                U1(0);                  // max_locals
                U2(3);                  // code_length
            }
            else
            {
                U4(15);
                U2(1);
                U2(0);
                U4(3);
            }
            U1(0x10); U1(42);           // bipush 42
            U1(0xAC);                   // ireturn
            U2(0);                      // exception table
            U2(0);                      // code attributes
            U2(0);                      // class attributes
            return s.ToArray();
        }

        [DataTestMethod]
        [DataRow(45, 0, true)]
        [DataRow(45, 2, true)]
        [DataRow(45, 3, false)]
        public void DefineClassShouldAcceptVersion45(int major, int minor, bool oldCodeLayout)
        {
            var bytes = BuildOldClass((ushort)major, (ushort)minor, oldCodeLayout);
            var clazz = new ByteArrayClassLoader().Define("p.Old", bytes);
            clazz.getMethod("f").invoke(null).Should().Be(global::java.lang.Integer.valueOf(42));
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
