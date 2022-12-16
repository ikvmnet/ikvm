using FluentAssertions;

using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using sun.misc;

namespace IKVM.Tests.Java.sun.misc
{

    [TestClass]
    public class UnsafeTests
    {

        static readonly Unsafe u = Unsafe.getUnsafe();

        class TestObject
        {

            public short shortField = 0;
            public int intField = 0;
            public long longField = 0;

        }

        [TestMethod]
        public static void CanAllocateAndFreeMemory()
        {
            var b = u.allocateMemory(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public static void CanSetShortField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("shortField"));
            u.putShort(o, f, 1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public static void CanSetIntField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("intField"));
            u.putInt(o, f, 1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public static void CanSetLongField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("longField"));
            u.putLong(o, f, 1);
            o.longField.Should().Be(1);
        }

        [TestMethod]
        public static void CanSetShortFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("shortField"));
            u.putShortVolatile(o, f, 1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public static void CanSetIntFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("intField"));
            u.putIntVolatile(o, f, 1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public static void CanSetLongFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("longField"));
            u.putLongVolatile(o, f, 1);
            o.longField.Should().Be(1);
        }

    }

}
