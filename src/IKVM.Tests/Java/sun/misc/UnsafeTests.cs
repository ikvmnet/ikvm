using System.Buffers.Binary;
using System.Runtime.InteropServices;

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
            public bool booleanField = false;
            public byte byteField = 0;
            public char charField = '\0';
            public short shortField = 0;
            public int intField = 0;
            public long longField = 0;
            public float floatField = 0;
            public double doubleField = 0;

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
        public void CanSetBooleanField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("booleanField"));
            u.putBoolean(o, f, true);
            o.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetByteField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("byteField"));
            u.putByte(o, f, 1);
            o.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetCharField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("charField"));
            u.putChar(o, f, 'A');
            o.charField.Should().Be('A');
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
        public void CanSetBooleanFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("booleanField"));
            u.putBooleanVolatile(o, f, true);
            o.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetByteFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("byteField"));
            u.putByteVolatile(o, f, 1);
            o.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetCharFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("charField"));
            u.putCharVolatile(o, f, 'A');
            o.charField.Should().Be('A');
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
        public void CanSetBoolInArray()
        {
            var a = new bool[16];
            u.putBoolean(a, sizeof(bool), true);
            a[0].Should().Be(false);
            a[1].Should().Be(true);
            a[2].Should().Be(false);
        }

        [TestMethod]
        public void CanSetByteInArray()
        {
            var a = new byte[16];
            u.putByte(a, sizeof(byte), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetCharInArray()
        {
            var a = new char[16];
            u.putChar(a, sizeof(char), 'A');
            a[0].Should().Be('\0');
            a[1].Should().Be('A');
            a[2].Should().Be('\0');
        }

        [TestMethod]
        public void CanSetShortInArray()
        {
            var a = new short[16];
            u.putShort(a, sizeof(short), 1);
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
            u.putLong(a, sizeof(long), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetFloatInArray()
        {
            var a = new float[16];
            u.putFloat(a, sizeof(float), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetDoubleInArray()
        {
            var a = new double[16];
            u.putFloat(a, sizeof(double), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanAllocateAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetBoolAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putBoolean(null, b, false);
            u.putBoolean(null, b + sizeof(bool), true);
            u.getBoolean(null, b + sizeof(bool)).Should().Be(true);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetByteAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putByte(null, b + sizeof(byte), 1);
            u.getByte(null, b).Should().Be(0);
            u.getByte(null, b + sizeof(byte)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetCharAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putChar(null, b + sizeof(char), 'A');
            u.getChar(null, b).Should().Be('\0');
            u.getChar(null, b + sizeof(char)).Should().Be('A');
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetShortAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putShort(null, b + sizeof(short), 1);
            u.getShort(null, b).Should().Be(0);
            u.getShort(null, b + sizeof(short)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetIntAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putInt(null, b + sizeof(int), 1);
            u.getInt(null, b).Should().Be(0);
            u.getInt(null, b + sizeof(int)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetLongAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putLong(null, b + sizeof(long), 1);
            u.getLong(null, b).Should().Be(0);
            u.getLong(null, b + sizeof(long)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetFloatAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putFloat(null, b + sizeof(float), 1);
            u.getFloat(null, b).Should().Be(0);
            u.getFloat(null, b + sizeof(float)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetDoubleAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.putDouble(null, b + sizeof(double), 1);
            u.getDouble(null, b).Should().Be(0);
            u.getDouble(null, b + sizeof(double)).Should().Be(1);
            u.freeMemory(b);
        }

    }

}
