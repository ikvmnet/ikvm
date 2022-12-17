using FluentAssertions;

using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using sun.misc;

namespace IKVM.Tests.Java.sun.misc
{

    [TestClass]
    public class UnsafeTests
    {

        static readonly Unsafe u;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static UnsafeTests()
        {
            var f = ((Class)typeof(Unsafe)).getDeclaredField("theUnsafe");
            f.setAccessible(true);
            u = (Unsafe)f.get(null);
        }

        class TestObject
        {

            public object objectField = null;
            public short shortField = 0;
            public int intField = 0;
            public long longField = 0;
            public float floatField = 0;
            public double doubleField = 0;

        }

        [TestMethod]
        public void CanAllocateAndFreeMemory()
        {
            var b = u.allocateMemory(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanSetObjectField()
        {
            var o = new TestObject();
            var t = new object();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("objectField"));
            u.putObject(o, f, t);
            o.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetShortField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("shortField"));
            u.putShort(o, f, 1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetIntField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("intField"));
            u.putInt(o, f, 1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetLongField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("longField"));
            u.putLong(o, f, 1);
            o.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetFloatField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("floatField"));
            u.putFloat(o, f, 1);
            o.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetDoubleField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("doubleField"));
            u.putDouble(o, f, 1);
            o.doubleField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetObjectFieldVolatile()
        {
            var o = new TestObject();
            var t = new object();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("objectField"));
            u.putObjectVolatile(o, f, t);
            o.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetShortFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("shortField"));
            u.putShortVolatile(o, f, 1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetIntFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("intField"));
            u.putIntVolatile(o, f, 1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetLongFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("longField"));
            u.putLongVolatile(o, f, 1);
            o.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetFloatFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("floatField"));
            u.putFloatVolatile(o, f, 1);
            o.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetDoubleFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("doubleField"));
            u.putDoubleVolatile(o, f, 1);
            o.doubleField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetShortInArray()
        {
            var a = new short[16];
            u.putInt(a, sizeof(short), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetIntInArray()
        {
            var a = new int[16];
            u.putInt(a, sizeof(int), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetLongInArray()
        {
            var a = new long[16];
            u.putInt(a, sizeof(long), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

    }

}
