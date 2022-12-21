using System;
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

        class StaticTestObject
        {

            public static object objectField = null;
            public static bool booleanField = false;
            public static byte byteField = 0;
            public static char charField = '\0';
            public static short shortField = 0;
            public static int intField = 0;
            public static long longField = 0;
            public static float floatField = 0;
            public static double doubleField = 0;

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
        public void CanSetStaticObjectField()
        {
            var t = new object();
            var f = u.objectFieldOffset(((Class)typeof(StaticTestObject)).getField("objectField"));
            u.putObject(null, f, t);
            StaticTestObject.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStaticBooleanField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("booleanField"));
            u.putBoolean(null, f, true);
            StaticTestObject.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetStaticByteField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("byteField"));
            u.putByte(null, f, 1);
            StaticTestObject.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticCharField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("charField"));
            u.putChar(null, f, 'A');
            StaticTestObject.charField.Should().Be('A');
        }

        [TestMethod]
        public void CanSetStaticShortField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("shortField"));
            u.putShort(null, f, 1);
            StaticTestObject.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticIntField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("intField"));
            u.putInt(null, f, 1);
            StaticTestObject.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticLongField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("longField"));
            u.putLong(null, f, 1);
            StaticTestObject.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticFloatField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("floatField"));
            u.putFloat(null, f, 1);
            StaticTestObject.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticDoubleField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("doubleField"));
            u.putDouble(null, f, 1);
            StaticTestObject.doubleField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticObjectFieldVolatile()
        {
            var t = new object();
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("objectField"));
            u.putObjectVolatile(null, f, t);
            StaticTestObject.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStaticBooleanFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("booleanField"));
            u.putBooleanVolatile(null, f, true);
            StaticTestObject.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetStaticByteFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("byteField"));
            u.putByteVolatile(null, f, 1);
            StaticTestObject.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticCharFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("charField"));
            u.putCharVolatile(null, f, 'A');
            StaticTestObject.charField.Should().Be('A');
        }

        [TestMethod]
        public void CanSetStaticShortFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("shortField"));
            u.putShortVolatile(null, f, 1);
            StaticTestObject.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticIntFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("intField"));
            u.putIntVolatile(null, f, 1);
            StaticTestObject.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticLongFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("longField"));
            u.putLongVolatile(null, f, 1);
            StaticTestObject.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticFloatFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("floatField"));
            u.putFloatVolatile(null, f, 1);
            StaticTestObject.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticDoubleFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("doubleField"));
            u.putDoubleVolatile(null, f, 1);
            StaticTestObject.doubleField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetBoolInArray()
        {
            var a = new bool[16];
            u.putBoolean(a, (long)sizeof(bool), true);
            a[0].Should().Be(false);
            a[1].Should().Be(true);
            a[2].Should().Be(false);
        }

        [TestMethod]
        public void CanSetByteInArray()
        {
            var a = new byte[16];
            u.putByte(a, (long)sizeof(byte), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetCharInArray()
        {
            var a = new char[16];
            u.putChar(a, (long)sizeof(char), 'A');
            a[0].Should().Be('\0');
            a[1].Should().Be('A');
            a[2].Should().Be('\0');
        }

        [TestMethod]
        public void CanSetShortInArray()
        {
            var a = new short[16];
            u.putShort(a, (long)sizeof(short), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetShortInByteArray()
        {
            var a = new byte[16];
            u.putShort(a, (long)0, 1);
            MemoryMarshal.Cast<byte, short>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetIntInArray()
        {
            var a = new int[16];
            u.putInt(a, (long)sizeof(int), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetIntInByteArray()
        {
            var a = new byte[16];
            u.putInt(a, (long)0, 1);
            MemoryMarshal.Cast<byte, int>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetLongInArray()
        {
            var a = new long[16];
            u.putLong(a, (long)sizeof(long), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetLongInByteArray()
        {
            var a = new byte[16];
            u.putLong(a, (long)0, 1);
            MemoryMarshal.Cast<byte, long>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetFloatInArray()
        {
            var a = new float[16];
            u.putFloat(a, (long)sizeof(float), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetFloatInByteArray()
        {
            var a = new byte[16];
            u.putFloat(a, (long)0, 1);
            MemoryMarshal.Cast<byte, float>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetDoubleInArray()
        {
            var a = new double[16];
            u.putDouble(a, (long)sizeof(double), 1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetDoubleInByteArray()
        {
            var a = new byte[16];
            u.putDouble(a, (long)0, 1);
            MemoryMarshal.Cast<byte, double>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanAllocateAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateAndSetAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);

            for (int i = 0; i < 32; i++)
                Marshal.ReadByte((IntPtr)(b + i)).Should().Be(0);

            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetByteAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);
            u.putByte(b + sizeof(byte), 1);
            u.getByte(b).Should().Be(0);
            u.getByte(b + sizeof(byte)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetCharAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);
            u.putChar(b + sizeof(char), 'A');
            u.getChar(b).Should().Be('\0');
            u.getChar(b + sizeof(char)).Should().Be('A');
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetShortAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);
            u.putShort(b + sizeof(short), 1);
            u.getShort(b).Should().Be(0);
            u.getShort(b + sizeof(short)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetIntAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);
            u.putInt(b + sizeof(int), 1);
            u.getInt(b).Should().Be(0);
            u.getInt(b + sizeof(int)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetLongAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);
            u.putLong(b + sizeof(long), 1);
            u.getLong(b).Should().Be(0);
            u.getLong(b + sizeof(long)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetFloatAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);
            u.putFloat(b + sizeof(float), 1);
            u.getFloat(b).Should().Be(0);
            u.getFloat(b + sizeof(float)).Should().Be(1);
            u.freeMemory(b);
        }

        [TestMethod]
        public void CanAllocateSetDoubleAndFreeMemory()
        {
            var b = u.allocateMemory(32);
            u.setMemory(b, 32, 0);
            u.putDouble(b + sizeof(double), 1);
            u.getDouble(b).Should().Be(0);
            u.getDouble(b + sizeof(double)).Should().Be(1);
            u.freeMemory(b);
        }

        class CompareAndSwapTestObject
        {

            public object objectField;
            public int intField;
            public long longField;

        }

        [TestMethod]
        public void CanCompareAndSwapObjectField()
        {
            var o = new CompareAndSwapTestObject();
            var o1 = new object();
            var o2 = new object();
            var f = u.objectFieldOffset(((Class)typeof(CompareAndSwapTestObject)).getField("objectField"));
            u.compareAndSwapObject(o, f, null, o1).Should().BeTrue();
            o.objectField.Should().BeSameAs(o1);
            u.compareAndSwapObject(o, f, o1, o2).Should().BeTrue();
            o.objectField.Should().BeSameAs(o2);
            u.compareAndSwapObject(o, f, o1, o2).Should().BeFalse();
            o.objectField.Should().BeSameAs(o2);
        }

        [TestMethod]
        public void CanCompareAndSwapIntField()
        {
            var o = new CompareAndSwapTestObject();
            var f = u.objectFieldOffset(((Class)typeof(CompareAndSwapTestObject)).getField("intField"));
            u.compareAndSwapInt(o, f, 0, 1).Should().BeTrue();
            o.intField.Should().Be(1);
            u.compareAndSwapInt(o, f, 1, 2).Should().BeTrue();
            o.intField.Should().Be(2);
            u.compareAndSwapInt(o, f, 1, 2).Should().BeFalse();
            o.intField.Should().Be(2);
        }

        [TestMethod]
        public void CanCompareAndSwapLongField()
        {
            var o = new CompareAndSwapTestObject();
            var f = u.objectFieldOffset(((Class)typeof(CompareAndSwapTestObject)).getField("longField"));
            u.compareAndSwapLong(o, f, 0, 1).Should().BeTrue();
            o.longField.Should().Be(1);
            u.compareAndSwapLong(o, f, 1, 2).Should().BeTrue();
            o.longField.Should().Be(2);
            u.compareAndSwapLong(o, f, 1, 2).Should().BeFalse();
            o.longField.Should().Be(2);
        }

        class StaticCompareAndSwapTestObject
        {

            public static object objectField;
            public static int intField;
            public static long longField;

        }

        [TestMethod]
        public void CanCompareAndSwapStaticObjectField()
        {
            var o1 = new object();
            var o2 = new object();
            var f = u.staticFieldOffset(((Class)typeof(StaticCompareAndSwapTestObject)).getField("objectField"));
            u.compareAndSwapObject(null, f, null, o1).Should().BeTrue();
            StaticCompareAndSwapTestObject.objectField.Should().BeSameAs(o1);
            u.compareAndSwapObject(null, f, o1, o2).Should().BeTrue();
            StaticCompareAndSwapTestObject.objectField.Should().BeSameAs(o2);
            u.compareAndSwapObject(null, f, o1, o2).Should().BeFalse();
            StaticCompareAndSwapTestObject.objectField.Should().BeSameAs(o2);
        }

        [TestMethod]
        public void CanCompareAndSwapStaticIntField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticCompareAndSwapTestObject)).getField("intField"));
            u.compareAndSwapInt(null, f, 0, 1).Should().BeTrue();
            StaticCompareAndSwapTestObject.intField.Should().Be(1);
            u.compareAndSwapInt(null, f, 1, 2).Should().BeTrue();
            StaticCompareAndSwapTestObject.intField.Should().Be(2);
            u.compareAndSwapInt(null, f, 1, 2).Should().BeFalse();
            StaticCompareAndSwapTestObject.intField.Should().Be(2);
        }

        [TestMethod]
        public void CanCompareAndSwapStaticLongField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticCompareAndSwapTestObject)).getField("longField"));
            u.compareAndSwapLong(null, f, 0, 1).Should().BeTrue();
            StaticCompareAndSwapTestObject.longField.Should().Be(1);
            u.compareAndSwapLong(null, f, 1, 2).Should().BeTrue();
            StaticCompareAndSwapTestObject.longField.Should().Be(2);
            u.compareAndSwapLong(null, f, 1, 2).Should().BeFalse();
            StaticCompareAndSwapTestObject.longField.Should().Be(2);
        }

        [TestMethod]
        public void CanCompareAndSwapObjectInArray()
        {
            var a = new object[4];
            for (int i = 0; i < 4; i++)
            {
                var o1 = new object();
                var o2 = new object();

                u.compareAndSwapObject(a, i, null, o1).Should().BeTrue();
                a[i].Should().BeSameAs(o1);
                u.compareAndSwapObject(a, i, o1, o2).Should().BeTrue();
                a[i].Should().BeSameAs(o2);
                u.compareAndSwapObject(a, i, o1, o2).Should().BeFalse();
                a[i].Should().BeSameAs(o2);
            }
        }

        [TestMethod]
        public void CanCompareAndSwapIntInArray()
        {
            var a = new int[4];
            for (int i = 0; i < 4; i++)
            {
                u.compareAndSwapInt(a, i * sizeof(int), 0, 1).Should().BeTrue();
                a[i].Should().Be(1);
                u.compareAndSwapInt(a, i * sizeof(int), 1, 2).Should().BeTrue();
                a[i].Should().Be(2);
                u.compareAndSwapInt(a, i * sizeof(int), 1, 2).Should().BeFalse();
                a[i].Should().Be(2);
            }
        }

        [TestMethod]
        public void CanCompareAndSwapLongInArray()
        {
            var a = new long[4];
            for (int i = 0; i < 4; i++)
            {
                u.compareAndSwapLong(a, i * sizeof(long), 0, 1).Should().BeTrue();
                a[i].Should().Be(1);
                u.compareAndSwapLong(a, i * sizeof(long), 1, 2).Should().BeTrue();
                a[i].Should().Be(2);
                u.compareAndSwapLong(a, i * sizeof(long), 1, 2).Should().BeFalse();
                a[i].Should().Be(2);
            }
        }

    }

}
