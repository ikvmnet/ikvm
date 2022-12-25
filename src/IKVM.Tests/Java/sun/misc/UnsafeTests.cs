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

        class TestObject
        {

            public object objectField = null;
            public string stringField = null;
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
            u.getObject(o, f).Should().BeSameAs(null);
            u.putObject(o, f, t);
            u.getObject(o, f).Should().BeSameAs(t);
            o.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStringField()
        {
            var o = new TestObject();
            var t = "TEST";
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("stringField"));
            u.getObject(o, f).Should().BeSameAs(null);
            u.putObject(o, f, t);
            u.getObject(o, f).Should().BeSameAs(t);
            o.stringField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetBooleanField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("booleanField"));
            u.getBoolean(o, f).Should().Be(false);
            u.putBoolean(o, f, true);
            u.getBoolean(o, f).Should().Be(true);
            o.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetByteField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("byteField"));
            u.getByte(o, f).Should().Be(0);
            u.putByte(o, f, 1);
            u.getByte(o, f).Should().Be(1);
            o.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetCharField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("charField"));
            u.getChar(o, f).Should().Be('\0');
            u.putChar(o, f, 'A');
            u.getChar(o, f).Should().Be('A');
            o.charField.Should().Be('A');
        }

        [TestMethod]
        public void CanSetShortField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("shortField"));
            u.getShort(o, f).Should().Be(0);
            u.putShort(o, f, 1);
            u.getShort(o, f).Should().Be(1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetIntField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("intField"));
            u.getInt(o, f).Should().Be(0);
            u.putInt(o, f, 1);
            u.getInt(o, f).Should().Be(1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetLongField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("longField"));
            u.getLong(o, f).Should().Be(0);
            u.putLong(o, f, 1);
            u.getLong(o, f).Should().Be(1);
            o.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetFloatField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("floatField"));
            u.getFloat(o, f).Should().Be(0);
            u.putFloat(o, f, 1);
            u.getFloat(o, f).Should().Be(1);
            o.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetDoubleField()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("doubleField"));
            u.getDouble(o, f).Should().Be(0);
            u.putDouble(o, f, 1);
            u.getDouble(o, f).Should().Be(1);
            o.doubleField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetObjectFieldVolatile()
        {
            var o = new TestObject();
            var t = new object();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("objectField"));
            u.getObjectVolatile(o, f).Should().BeSameAs(null);
            u.putObjectVolatile(o, f, t);
            u.getObjectVolatile(o, f).Should().BeSameAs(t);
            o.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStringFieldVolatile()
        {
            var o = new TestObject();
            var t = "TEST";
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("stringField"));
            u.getObjectVolatile(o, f).Should().BeSameAs(null);
            u.putObjectVolatile(o, f, t);
            u.getObjectVolatile(o, f).Should().BeSameAs(t);
            o.stringField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetBooleanFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("booleanField"));
            u.getBooleanVolatile(o, f).Should().Be(false);
            u.putBooleanVolatile(o, f, true);
            u.getBooleanVolatile(o, f).Should().Be(true);
            o.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetByteFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("byteField"));
            u.getByteVolatile(o, f).Should().Be(0);
            u.putByteVolatile(o, f, 1);
            u.getByteVolatile(o, f).Should().Be(1);
            o.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetCharFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("charField"));
            u.getCharVolatile(o, f).Should().Be('\0');
            u.putCharVolatile(o, f, 'A');
            u.getCharVolatile(o, f).Should().Be('A');
            o.charField.Should().Be('A');
        }

        [TestMethod]
        public void CanSetShortFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("shortField"));
            u.getShortVolatile(o, f).Should().Be(0);
            u.putShortVolatile(o, f, 1);
            u.getShortVolatile(o, f).Should().Be(1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetIntFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("intField"));
            u.getIntVolatile(o, f).Should().Be(0);
            u.putIntVolatile(o, f, 1);
            u.getIntVolatile(o, f).Should().Be(1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetLongFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("longField"));
            u.getLongVolatile(o, f).Should().Be(0);
            u.putLongVolatile(o, f, 1);
            u.getLongVolatile(o, f).Should().Be(1);
            o.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetFloatFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("floatField"));
            u.getFloatVolatile(o, f).Should().Be(0);
            u.putFloatVolatile(o, f, 1);
            u.getFloatVolatile(o, f).Should().Be(1);
            o.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetDoubleFieldVolatile()
        {
            var o = new TestObject();
            var f = u.objectFieldOffset(((Class)typeof(TestObject)).getField("doubleField"));
            u.getDoubleVolatile(o, f).Should().Be(0);
            u.putDoubleVolatile(o, f, 1);
            u.getDoubleVolatile(o, f).Should().Be(1);
            o.doubleField.Should().Be(1);
        }

        class ReadOnlyTestObject
        {

            public readonly object objectField = null;
            public readonly string stringField = null;
            public readonly bool booleanField = false;
            public readonly byte byteField = 0;
            public readonly char charField = '\0';
            public readonly short shortField = 0;
            public readonly int intField = 0;
            public readonly long longField = 0;
            public readonly float floatField = 0;
            public readonly double doubleField = 0;

        }

        [TestMethod]
        public void CanSetReadOnlyObjectField()
        {
            var o = new ReadOnlyTestObject();
            var t = new object();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("objectField"));
            u.getObject(o, f).Should().BeSameAs(null);
            u.putObject(o, f, t);
            u.getObject(o, f).Should().BeSameAs(t);
            o.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyStringField()
        {
            var o = new ReadOnlyTestObject();
            var t = "TEST";
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("stringField"));
            u.getObject(o, f).Should().BeSameAs(null);
            u.putObject(o, f, t);
            u.getObject(o, f).Should().BeSameAs(t);
            o.stringField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyBooleanField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("booleanField"));
            u.getBoolean(o, f).Should().Be(false);
            u.putBoolean(o, f, true);
            u.getBoolean(o, f).Should().Be(true);
            o.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetReadOnlyByteField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("byteField"));
            u.getByte(o, f).Should().Be(0);
            u.putByte(o, f, 1);
            u.getByte(o, f).Should().Be(1);
            o.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyCharField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("charField"));
            u.getChar(o, f).Should().Be('\0');
            u.putChar(o, f, 'A');
            u.getChar(o, f).Should().Be('A');
            o.charField.Should().Be('A');
        }

        [TestMethod]
        public void CanSetReadOnlyShortField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("shortField"));
            u.getShort(o, f).Should().Be(0);
            u.putShort(o, f, 1);
            u.getShort(o, f).Should().Be(1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyIntField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("intField"));
            u.getInt(o, f).Should().Be(0);
            u.putInt(o, f, 1);
            u.getInt(o, f).Should().Be(1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyLongField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("longField"));
            u.getLong(o, f).Should().Be(0);
            u.putLong(o, f, 1);
            u.getLong(o, f).Should().Be(1);
            o.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyFloatField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("floatField"));
            u.getFloat(o, f).Should().Be(0);
            u.putFloat(o, f, 1);
            u.getFloat(o, f).Should().Be(1);
            o.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyDoubleField()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("doubleField"));
            u.getDouble(o, f).Should().Be(0);
            u.putDouble(o, f, 1);
            u.getDouble(o, f).Should().Be(1);
            o.doubleField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyObjectFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var t = new object();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("objectField"));
            u.getObjectVolatile(o, f).Should().BeSameAs(null);
            u.putObjectVolatile(o, f, t);
            u.getObjectVolatile(o, f).Should().BeSameAs(t);
            o.objectField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyStringFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var t = "TEST";
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("stringField"));
            u.getObjectVolatile(o, f).Should().BeSameAs(null);
            u.putObjectVolatile(o, f, t);
            u.getObjectVolatile(o, f).Should().BeSameAs(t);
            o.stringField.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyBooleanFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("booleanField"));
            u.getBooleanVolatile(o, f).Should().Be(false);
            u.putBooleanVolatile(o, f, true);
            u.getBooleanVolatile(o, f).Should().Be(true);
            o.booleanField.Should().Be(true);
        }

        [TestMethod]
        public void CanSetReadOnlyByteFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("byteField"));
            u.getByteVolatile(o, f).Should().Be(0);
            u.putByteVolatile(o, f, 1);
            u.getByteVolatile(o, f).Should().Be(1);
            o.byteField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyCharFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("charField"));
            u.getCharVolatile(o, f).Should().Be('\0');
            u.putCharVolatile(o, f, 'A');
            u.getCharVolatile(o, f).Should().Be('A');
            o.charField.Should().Be('A');
        }

        [TestMethod]
        public void CanSetReadOnlyShortFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("shortField"));
            u.getShortVolatile(o, f).Should().Be(0);
            u.putShortVolatile(o, f, 1);
            u.getShortVolatile(o, f).Should().Be(1);
            o.shortField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyIntFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("intField"));
            u.getIntVolatile(o, f).Should().Be(0);
            u.putIntVolatile(o, f, 1);
            u.getIntVolatile(o, f).Should().Be(1);
            o.intField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyLongFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("longField"));
            u.getLongVolatile(o, f).Should().Be(0);
            u.putLongVolatile(o, f, 1);
            u.getLongVolatile(o, f).Should().Be(1);
            o.longField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyFloatFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("floatField"));
            u.getFloatVolatile(o, f).Should().Be(0);
            u.putFloatVolatile(o, f, 1);
            u.getFloatVolatile(o, f).Should().Be(1);
            o.floatField.Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyDoubleFieldVolatile()
        {
            var o = new ReadOnlyTestObject();
            var f = u.objectFieldOffset(((Class)typeof(ReadOnlyTestObject)).getField("doubleField"));
            u.getDoubleVolatile(o, f).Should().Be(0);
            u.putDoubleVolatile(o, f, 1);
            u.getDoubleVolatile(o, f).Should().Be(1);
            o.doubleField.Should().Be(1);
        }

        class StaticTestObject
        {

            public static object objectField = null;
            public static string stringField = null;
            public static bool booleanField = false;
            public static byte byteField = 0;
            public static char charField = '\0';
            public static short shortField = 0;
            public static int intField = 0;
            public static long longField = 0;
            public static float floatField = 0;
            public static double doubleField = 0;

        }

        [TestMethod]
        public void CanSetStaticObjectField()
        {
            var t = new object();
            var f = u.objectFieldOffset(((Class)typeof(StaticTestObject)).getField("objectField"));
            u.getObject(null, f).Should().BeSameAs(null);
            u.putObject(null, f, t);
            u.getObject(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStaticStringField()
        {
            var t = "TEST";
            var f = u.objectFieldOffset(((Class)typeof(StaticTestObject)).getField("stringField"));
            u.getObject(null, f).Should().BeSameAs(null);
            u.putObject(null, f, t);
            u.getObject(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStaticBooleanField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("booleanField"));
            u.getBoolean(null, f).Should().Be(false);
            u.putBoolean(null, f, true);
            u.getBoolean(null, f).Should().Be(true);
        }

        [TestMethod]
        public void CanSetStaticByteField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("byteField"));
            u.getByte(null, f).Should().Be(0);
            u.putByte(null, f, 1);
            u.getByte(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticCharField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("charField"));
            u.getChar(null, f).Should().Be('\0');
            u.putChar(null, f, 'A');
            u.getChar(null, f).Should().Be('A');
        }

        [TestMethod]
        public void CanSetStaticShortField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("shortField"));
            u.getShort(null, f).Should().Be(0);
            u.putShort(null, f, 1);
            u.getShort(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticIntField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("intField"));
            u.getInt(null, f).Should().Be(0);
            u.putInt(null, f, 1);
            u.getInt(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticLongField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("longField"));
            u.getLong(null, f).Should().Be(0);
            u.putLong(null, f, 1);
            u.getLong(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticFloatField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("floatField"));
            u.getFloat(null, f).Should().Be(0);
            u.putFloat(null, f, 1);
            u.getFloat(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticDoubleField()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticTestObject)).getField("doubleField"));
            u.getDouble(null, f).Should().Be(0);
            u.putDouble(null, f, 1);
            u.getDouble(null, f).Should().Be(1);
        }

        class StaticVolatileTestObject
        {

            public static object objectField = null;
            public static string stringField = null;
            public static bool booleanField = false;
            public static byte byteField = 0;
            public static char charField = '\0';
            public static short shortField = 0;
            public static int intField = 0;
            public static long longField = 0;
            public static float floatField = 0;
            public static double doubleField = 0;

        }

        [TestMethod]
        public void CanSetStaticObjectFieldVolatile()
        {
            var t = new object();
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("objectField"));
            u.getObjectVolatile(null, f).Should().BeSameAs(null);
            u.putObjectVolatile(null, f, t);
            u.getObjectVolatile(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStaticStringFieldVolatile()
        {
            var t = "TEST";
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("stringField"));
            u.getObjectVolatile(null, f).Should().BeSameAs(null);
            u.putObjectVolatile(null, f, t);
            u.getObjectVolatile(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetStaticBooleanFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("booleanField"));
            u.getBooleanVolatile(null, f).Should().Be(false);
            u.putBooleanVolatile(null, f, true);
            u.getBooleanVolatile(null, f).Should().Be(true);
        }

        [TestMethod]
        public void CanSetStaticByteFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("byteField"));
            u.getByteVolatile(null, f).Should().Be(0);
            u.putByteVolatile(null, f, 1);
            u.getByteVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticCharFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("charField"));
            u.getCharVolatile(null, f).Should().Be('\0');
            u.putCharVolatile(null, f, 'A');
            u.getCharVolatile(null, f).Should().Be('A');
        }

        [TestMethod]
        public void CanSetStaticShortFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("shortField"));
            u.getShortVolatile(null, f).Should().Be(0);
            u.putShortVolatile(null, f, 1);
            u.getShortVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticIntFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("intField"));
            u.getIntVolatile(null, f).Should().Be(0);
            u.putIntVolatile(null, f, 1);
            u.getIntVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticLongFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("longField"));
            u.getLongVolatile(null, f).Should().Be(0);
            u.putLongVolatile(null, f, 1);
            u.getLongVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticFloatFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("floatField"));
            u.getFloatVolatile(null, f).Should().Be(0);
            u.putFloatVolatile(null, f, 1);
            u.getFloatVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetStaticDoubleFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(StaticVolatileTestObject)).getField("doubleField"));
            u.getDoubleVolatile(null, f).Should().Be(0);
            u.putDoubleVolatile(null, f, 1);
            u.getDoubleVolatile(null, f).Should().Be(1);
        }

        class ReadOnlyStaticTestObject
        {

            public readonly static object objectField = null;
            public readonly static string stringField = null;
            public readonly static bool booleanField = false;
            public readonly static byte byteField = 0;
            public readonly static char charField = '\0';
            public readonly static short shortField = 0;
            public readonly static int intField = 0;
            public readonly static long longField = 0;
            public readonly static float floatField = 0;
            public readonly static double doubleField = 0;

        }

        [TestMethod]
        public void CanSetReadOnlyStaticObjectField()
        {
            var t = new object();
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("objectField"));
            u.getObject(null, f).Should().BeSameAs(null);
            u.putObject(null, f, t);
            u.getObject(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticStringField()
        {
            var t = "TEST";
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("stringField"));
            u.getObject(null, f).Should().BeSameAs(null);
            u.putObject(null, f, t);
            u.getObject(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticBooleanField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("booleanField"));
            u.getBoolean(null, f).Should().Be(false);
            u.putBoolean(null, f, true);
            u.getBoolean(null, f).Should().Be(true);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticByteField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("byteField"));
            u.getByte(null, f).Should().Be(0);
            u.putByte(null, f, 1);
            u.getByte(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticCharField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("charField"));
            u.getChar(null, f).Should().Be('\0');
            u.putChar(null, f, 'A');
            u.getChar(null, f).Should().Be('A');
        }

        [TestMethod]
        public void CanSetReadOnlyStaticShortField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("shortField"));
            u.getShort(null, f).Should().Be(0);
            u.putShort(null, f, 1);
            u.getShort(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticIntField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("intField"));
            u.getInt(null, f).Should().Be(0);
            u.putInt(null, f, 1);
            u.getInt(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticLongField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("longField"));
            u.getLong(null, f).Should().Be(0);
            u.putLong(null, f, 1);
            u.getLong(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticFloatField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("floatField"));
            u.getFloat(null, f).Should().Be(0);
            u.putFloat(null, f, 1);
            u.getFloat(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticDoubleField()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticTestObject)).getField("doubleField"));
            u.getDouble(null, f).Should().Be(0);
            u.putDouble(null, f, 1);
            u.getDouble(null, f).Should().Be(1);
        }

        class ReadOnlyStaticVolatileTestObject
        {

            public readonly static object objectField = null;
            public readonly static string stringField = null;
            public readonly static bool booleanField = false;
            public readonly static byte byteField = 0;
            public readonly static char charField = '\0';
            public readonly static short shortField = 0;
            public readonly static int intField = 0;
            public readonly static long longField = 0;
            public readonly static float floatField = 0;
            public readonly static double doubleField = 0;

        }

        [TestMethod]
        public void CanSetReadOnlyStaticObjectFieldVolatile()
        {
            var t = new object();
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("objectField"));
            u.getObjectVolatile(null, f).Should().BeSameAs(null);
            u.putObjectVolatile(null, f, t);
            u.getObjectVolatile(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticStringFieldVolatile()
        {
            var t = "TEST";
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("stringField"));
            u.getObjectVolatile(null, f).Should().BeSameAs(null);
            u.putObjectVolatile(null, f, t);
            u.getObjectVolatile(null, f).Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticBooleanFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("booleanField"));
            u.getBooleanVolatile(null, f).Should().Be(false);
            u.putBooleanVolatile(null, f, true);
            u.getBooleanVolatile(null, f).Should().Be(true);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticByteFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("byteField"));
            u.getByteVolatile(null, f).Should().Be(0);
            u.putByteVolatile(null, f, 1);
            u.getByteVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticCharFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("charField"));
            u.getCharVolatile(null, f).Should().Be('\0');
            u.putCharVolatile(null, f, 'A');
            u.getCharVolatile(null, f).Should().Be('A');
        }

        [TestMethod]
        public void CanSetReadOnlyStaticShortFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("shortField"));
            u.getShortVolatile(null, f).Should().Be(0);
            u.putShortVolatile(null, f, 1);
            u.getShortVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticIntFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("intField"));
            u.getIntVolatile(null, f).Should().Be(0);
            u.putIntVolatile(null, f, 1);
            u.getIntVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticLongFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("longField"));
            u.getLongVolatile(null, f).Should().Be(0);
            u.putLongVolatile(null, f, 1);
            u.getLongVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticFloatFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("floatField"));
            u.getFloatVolatile(null, f).Should().Be(0);
            u.putFloatVolatile(null, f, 1);
            u.getFloatVolatile(null, f).Should().Be(1);
        }

        [TestMethod]
        public void CanSetReadOnlyStaticDoubleFieldVolatile()
        {
            var f = u.staticFieldOffset(((Class)typeof(ReadOnlyStaticVolatileTestObject)).getField("doubleField"));
            u.getDoubleVolatile(null, f).Should().Be(0);
            u.putDoubleVolatile(null, f, 1);
            u.getDoubleVolatile(null, f).Should().Be(1);
        }


        [TestMethod]
        public void CanSetObjectInArray()
        {
            var o1 = new object();
            var o2 = new object();
            var a = new object[16];
            u.putObject(a, 0L, o1);
            u.getObject(a, 0L).Should().BeSameAs(o1);
            a[0].Should().BeSameAs(o1);
            u.putObject(a, 0L, null);
            u.getObject(a, 0L).Should().BeSameAs(null);
            a[0].Should().BeNull();
            u.putObject(a, 1L, o2);
            u.getObject(a, 1L).Should().BeSameAs(o2);
            a[1].Should().BeSameAs(o2);
            u.putObject(a, 1L, null);
            u.getObject(a, 1L).Should().BeSameAs(null);
            a[1].Should().BeNull();
        }

        [TestMethod]
        public void CanSetObjectInArrayVolatile()
        {
            var o1 = new object();
            var o2 = new object();
            var a = new object[16];
            u.putObjectVolatile(a, 0L, o1);
            u.getObjectVolatile(a, 0L).Should().BeSameAs(o1);
            a[0].Should().BeSameAs(o1);
            u.putObjectVolatile(a, 0L, null);
            u.getObjectVolatile(a, 0L).Should().BeSameAs(null);
            a[0].Should().BeNull();
            u.putObjectVolatile(a, 1L, o2);
            u.getObjectVolatile(a, 1L).Should().BeSameAs(o2);
            a[1].Should().BeSameAs(o2);
            u.putObjectVolatile(a, 1L, null);
            u.getObjectVolatile(a, 1L).Should().BeSameAs(null);
            a[1].Should().BeNull();
        }

        class AnonymousTestObject
        {



        }

        [TestMethod]
        public void CanSetObjectInTypedArray()
        {
            var o1 = new AnonymousTestObject();
            var o2 = new AnonymousTestObject();
            var a = new AnonymousTestObject[16];
            u.putObject(a, 0L, o1);
            u.getObject(a, 0L).Should().BeSameAs(o1);
            a[0].Should().BeSameAs(o1);
            u.putObject(a, 0L, null);
            u.getObject(a, 0L).Should().BeSameAs(null);
            a[0].Should().BeNull();
            u.putObject(a, 1L, o2);
            u.getObject(a, 1L).Should().BeSameAs(o2);
            a[1].Should().BeSameAs(o2);
            u.putObject(a, 1L, null);
            u.getObject(a, 1L).Should().BeSameAs(null);
            a[1].Should().BeNull();
        }

        [TestMethod]
        public void CanSetObjectInTypedArrayVolatile()
        {
            var o1 = new AnonymousTestObject();
            var o2 = new AnonymousTestObject();
            var a = new AnonymousTestObject[16];
            u.putObjectVolatile(a, 0L, o1);
            u.getObjectVolatile(a, 0L).Should().BeSameAs(o1);
            a[0].Should().BeSameAs(o1);
            u.putObjectVolatile(a, 0L, null);
            u.getObjectVolatile(a, 0L).Should().BeSameAs(null);
            a[0].Should().BeNull();
            u.putObjectVolatile(a, 1L, o2);
            u.getObjectVolatile(a, 1L).Should().BeSameAs(o2);
            a[1].Should().BeSameAs(o2);
            u.putObjectVolatile(a, 1L, null);
            u.getObjectVolatile(a, 1L).Should().BeSameAs(null);
            a[1].Should().BeNull();
        }

        [TestMethod]
        public void CanSetBoolInArray()
        {
            var a = new bool[16];
            u.putBoolean(a, (long)sizeof(bool), true);
            u.getBoolean(a, (long)sizeof(bool)).Should().Be(true);
            a[0].Should().Be(false);
            a[1].Should().Be(true);
            a[2].Should().Be(false);
        }

        [TestMethod]
        public void CanSetByteInArray()
        {
            var a = new byte[16];
            u.putByte(a, (long)sizeof(byte), 1);
            u.getByte(a, (long)sizeof(byte)).Should().Be(1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetCharInArray()
        {
            var a = new char[16];
            u.putChar(a, (long)sizeof(char), 'A');
            u.getChar(a, (long)sizeof(char)).Should().Be('A');
            a[0].Should().Be('\0');
            a[1].Should().Be('A');
            a[2].Should().Be('\0');
        }

        [TestMethod]
        public void CanSetShortInArray()
        {
            var a = new short[16];
            u.putShort(a, (long)sizeof(short), 1);
            u.getShort(a, (long)sizeof(short)).Should().Be(1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetShortInByteArray()
        {
            var a = new byte[16];
            u.putShort(a, (long)0, 1);
            u.getShort(a, (long)0).Should().Be(1);
            MemoryMarshal.Cast<byte, short>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetIntInArray()
        {
            var a = new int[16];
            u.putInt(a, (long)sizeof(int), 1);
            u.getInt(a, (long)sizeof(int)).Should().Be(1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetIntInByteArray()
        {
            var a = new byte[16];
            u.putInt(a, (long)0, 1);
            u.getInt(a, (long)0).Should().Be(1);
            MemoryMarshal.Cast<byte, int>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetLongInArray()
        {
            var a = new long[16];
            u.putLong(a, (long)sizeof(long), 1);
            u.getLong(a, (long)sizeof(long)).Should().Be(1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetLongInByteArray()
        {
            var a = new byte[16];
            u.putLong(a, (long)0, 1);
            u.getLong(a, (long)0).Should().Be(1);
            MemoryMarshal.Cast<byte, long>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetFloatInArray()
        {
            var a = new float[16];
            u.putFloat(a, (long)sizeof(float), 1);
            u.getFloat(a, (long)sizeof(float)).Should().Be(1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetFloatInByteArray()
        {
            var a = new byte[16];
            u.putFloat(a, (long)0, 1);
            u.getFloat(a, (long)0).Should().Be(1);
            MemoryMarshal.Cast<byte, float>(a)[0].Should().Be(1);
        }

        [TestMethod]
        public void CanSetDoubleInArray()
        {
            var a = new double[16];
            u.putDouble(a, (long)sizeof(double), 1);
            u.getDouble(a, (long)sizeof(double)).Should().Be(1);
            a[0].Should().Be(0);
            a[1].Should().Be(1);
            a[2].Should().Be(0);
        }

        [TestMethod]
        public void CanSetDoubleInByteArray()
        {
            var a = new byte[16];
            u.putDouble(a, (long)0, 1);
            u.getDouble(a, (long)0).Should().Be(1);
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

            public object objectField = null;
            public string stringField = null;
            public int intField = 0;
            public long longField = 0;

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
        public void CanCompareAndSwapStringField()
        {
            var o = new CompareAndSwapTestObject();
            var o1 = "TEST1";
            var o2 = "TEST2";
            var f = u.objectFieldOffset(((Class)typeof(CompareAndSwapTestObject)).getField("stringField"));
            u.compareAndSwapObject(o, f, null, o1).Should().BeTrue();
            o.stringField.Should().BeSameAs(o1);
            u.compareAndSwapObject(o, f, o1, o2).Should().BeTrue();
            o.stringField.Should().BeSameAs(o2);
            u.compareAndSwapObject(o, f, o1, o2).Should().BeFalse();
            o.stringField.Should().BeSameAs(o2);
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

            public static object objectField = null;
            public static string stringField = null;
            public static int intField = 0;
            public static long longField = 0;

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
        public void CanCompareAndSwapStaticStringField()
        {
            var o1 = "TEST1";
            var o2 = "TEST2";
            var f = u.staticFieldOffset(((Class)typeof(StaticCompareAndSwapTestObject)).getField("stringField"));
            u.compareAndSwapObject(null, f, null, o1).Should().BeTrue();
            StaticCompareAndSwapTestObject.stringField.Should().BeSameAs(o1);
            u.compareAndSwapObject(null, f, o1, o2).Should().BeTrue();
            StaticCompareAndSwapTestObject.stringField.Should().BeSameAs(o2);
            u.compareAndSwapObject(null, f, o1, o2).Should().BeFalse();
            StaticCompareAndSwapTestObject.stringField.Should().BeSameAs(o2);
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
        public void CanCompareAndSwapStringInArray()
        {
            var a = new string[4];
            for (int i = 0; i < 4; i++)
            {
                var o1 = "TEST1";
                var o2 = "TEST2";

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
        public void CanCompareAndSwapIntInLongArrayAligned()
        {
            var a = new long[4];
            for (int i = 0; i < 4; i++)
            {
                u.compareAndSwapInt(a, i * sizeof(long), 0, 1).Should().BeTrue();
                u.compareAndSwapInt(a, i * sizeof(long), 1, 2).Should().BeTrue();
                u.compareAndSwapInt(a, i * sizeof(long), 1, 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void CanCompareAndSwapIntInLongArrayUnaligned()
        {
            var a = new long[4];
            for (int i = 0; i < 4; i++)
            {
                u.compareAndSwapInt(a, i * sizeof(long) + 1, 0, 1).Should().BeTrue();
                u.compareAndSwapInt(a, i * sizeof(long) + 1, 1, 2).Should().BeTrue();
                u.compareAndSwapInt(a, i * sizeof(long) + 1, 1, 2).Should().BeFalse();
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

        [TestMethod]
        public void CanCompareAndSwapLongInIntArray()
        {
            var a = new int[8];
            for (int i = 0; i < 4; i++)
            {
                u.compareAndSwapLong(a, i * sizeof(long), 0, 1).Should().BeTrue();
                u.compareAndSwapLong(a, i * sizeof(long), 1, 2).Should().BeTrue();
                u.compareAndSwapLong(a, i * sizeof(long), 1, 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void CanCompareAndSwapLongInIntArrayUnaligned()
        {
            var a = new int[16];
            for (int i = 0; i < 4; i++)
            {
                u.compareAndSwapLong(a, i * sizeof(long) + 1, 0, 1).Should().BeTrue();
                u.compareAndSwapLong(a, i * sizeof(long) + 1, 1, 2).Should().BeTrue();
                u.compareAndSwapLong(a, i * sizeof(long) + 1, 1, 2).Should().BeFalse();
            }
        }

    }

}
