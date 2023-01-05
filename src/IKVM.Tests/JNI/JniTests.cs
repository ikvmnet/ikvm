using System.IO;

using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.JNI
{

    [TestClass]
    public partial class JniTests
    {

        static dynamic test;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            // compile the java test code on the fly
            var source = new StreamReader(typeof(JniTests).Assembly.GetManifestResourceStream("IKVM.Tests.JNI.JniTests.java")).ReadToEnd();
            source = source.Replace("@@IKVM_TESTS_NATIVE@@", Native.GetLibraryPath().Replace(@"\", @"\\"));
            var unit = new InMemoryCodeUnit("ikvm.tests.jni.JniTests", source);
            var compiler = new InMemoryCompiler(new[] { unit });
            compiler.Compile();

            // create an isntance of the JniTests class
            var clazz = compiler.GetClass("ikvm.tests.jni.JniTests");
            var ctor = clazz.getConstructor();
            test = ctor.newInstance(System.Array.Empty<object>());
        }

        [TestMethod]
        public void GetVersionTest()
        {
            test.getVersionTest();
        }

        [TestMethod]
        public void DefineClassTest()
        {
            test.defineClassTest();
        }

        [TestMethod]
        public void FindClassTest()
        {
            test.findClassTest();
        }

        [TestMethod]
        public void GetSuperclassTest()
        {
            test.getSuperclassTest();
        }

        [TestMethod]
        [ExpectedException(typeof(global::java.lang.RuntimeException))]
        public void ThrowTest()
        {
            test.throwTest();
        }

        [TestMethod]
        [ExpectedException(typeof(global::java.lang.RuntimeException))]
        public void ThrowNewTest()
        {
            test.throwNewTest();
        }

        [TestMethod]
        public void NewObjectTest()
        {
            test.newObjectTest();
        }

        [TestMethod]
        public void NewObjectVTest()
        {
            test.newObjectVTest();
        }

        [TestMethod]
        public void NewObjectATest()
        {
            test.newObjectATest();
        }

        [TestMethod]
        public void NewObjectTestWithArg()
        {
            test.newObjectTestWithArg();
        }

        [TestMethod]
        public void NewObjectVTestWithArg()
        {
            test.newObjectVTestWithArg();
        }

        [TestMethod]
        public void NewObjectATestWithArg()
        {
            test.newObjectATestWithArg();
        }

        class TestObject
        {

            public object objectField;
            public object nullObjectField;
            public string stringField;
            public bool booleanField;
            public byte byteField;
            public char charField;
            public short shortField;
            public int intField;
            public long longField;
            public float floatField;
            public double doubleField;

        }

        [TestMethod]
        public void GetObjectField()
        {
            var t = new object();
            var o = new TestObject();
            o.objectField = t;
            var v = (object)test.getObjectField(typeof(TestObject), o);
            v.Should().BeSameAs(t);
        }

        [TestMethod]
        public void GetNullObjectField()
        {
            var o = new TestObject();
            o.nullObjectField = null;
            var v = (object)test.getObjectField(typeof(TestObject), o);
            v.Should().BeNull();
        }

        [TestMethod]
        public void GetStringField()
        {
            var t = "TEST";
            var o = new TestObject();
            o.stringField = t;
            var v = (string)test.getStringField(typeof(TestObject), o);
            v.Should().Be(t);
        }

        [TestMethod]
        public void GetBooleanField()
        {
            var o = new TestObject();
            o.booleanField = true;
            var v = (bool)test.getBooleanField(typeof(TestObject), o);
            v.Should().Be(true);
        }

        [TestMethod]
        public void GetByteField()
        {
            var o = new TestObject();
            o.byteField = 1;
            var v = (byte)test.getByteField(typeof(TestObject), o);
            v.Should().Be(1);
        }

        [TestMethod]
        public void GetCharField()
        {
            var o = new TestObject();
            o.charField = 'A';
            var v = (char)test.getCharField(typeof(TestObject), o);
            v.Should().Be('A');
        }

        [TestMethod]
        public void GetShortField()
        {
            var o = new TestObject();
            o.shortField = 1;
            var v = (short)test.getShortField(typeof(TestObject), o);
            v.Should().Be(1);
        }

        [TestMethod]
        public void GetIntField()
        {
            var o = new TestObject();
            o.intField = 1;
            var v = (int)test.getIntField(typeof(TestObject), o);
            v.Should().Be(1);
        }

        [TestMethod]
        public void GetLongField()
        {
            var o = new TestObject();
            o.longField = 1L;
            var v = (long)test.getLongField(typeof(TestObject), o);
            v.Should().Be(1L);
        }

        [TestMethod]
        public void GetFloatField()
        {
            var o = new TestObject();
            o.floatField = 1f;
            var v = (float)test.getFloatField(typeof(TestObject), o);
            v.Should().Be(1f);
        }

        [TestMethod]
        public void GetDoubleField()
        {
            var o = new TestObject();
            o.doubleField = 1d;
            var v = (double)test.getDoubleField(typeof(TestObject), o);
            v.Should().Be(1d);
        }

        class StaticTestObject
        {

            public static object nullObjectField = null;
            public static object objectField;
            public static string stringField;
            public static bool booleanField;
            public static byte byteField;
            public static char charField;
            public static short shortField;
            public static int intField;
            public static long longField;
            public static float floatField;
            public static double doubleField;

        }

        [TestMethod]
        public void GetStaticNullObjectField()
        {
            StaticTestObject.objectField = null;
            var v = (object)test.getStaticNullObjectField(typeof(StaticTestObject));
            v.Should().BeNull();
        }

        [TestMethod]
        public void GetStaticObjectField()
        {
            var t = new object();
            StaticTestObject.objectField = t;
            var v = (object)test.getStaticObjectField(typeof(StaticTestObject));
            v.Should().BeSameAs(t);
        }

        [TestMethod]
        public void GetStaticStringField()
        {
            var t = "TEST";
            StaticTestObject.stringField = t;
            var v = (string)test.getStaticStringField(typeof(StaticTestObject));
            v.Should().Be(t);
        }

        [TestMethod]
        public void GetStaticBooleanField()
        {
            StaticTestObject.booleanField = true;
            var v = (bool)test.getStaticBooleanField(typeof(StaticTestObject));
            v.Should().Be(true);
        }

        [TestMethod]
        public void GetStaticByteField()
        {
            StaticTestObject.byteField = 1;
            var v = (byte)test.getStaticByteField(typeof(StaticTestObject));
            v.Should().Be(1);
        }

        [TestMethod]
        public void GetStaticCharField()
        {
            StaticTestObject.charField = 'A';
            var v = (char)test.getStaticCharField(typeof(StaticTestObject));
            v.Should().Be('A');
        }

        [TestMethod]
        public void GetStaticShortField()
        {
            StaticTestObject.shortField = 1;
            var v = (short)test.getStaticShortField(typeof(StaticTestObject));
            v.Should().Be(1);
        }

        [TestMethod]
        public void GetStaticIntField()
        {
            StaticTestObject.intField = 1;
            var v = (int)test.getStaticIntField(typeof(StaticTestObject));
            v.Should().Be(1);
        }

        [TestMethod]
        public void GetStaticLongField()
        {
            StaticTestObject.longField = 1L;
            var v = (long)test.getStaticLongField(typeof(StaticTestObject));
            v.Should().Be(1L);
        }

        [TestMethod]
        public void GetStaticFloatField()
        {
            StaticTestObject.floatField = 1f;
            var v = (float)test.getStaticFloatField(typeof(StaticTestObject));
            v.Should().Be(1f);
        }

        [TestMethod]
        public void GetStaticDoubleField()
        {
            StaticTestObject.doubleField = 1d;
            var v = (double)test.getStaticDoubleField(typeof(StaticTestObject));
            v.Should().Be(1d);
        }

        [TestMethod]
        public void NewWeakGlobalRef()
        {
            var o = new object();
            ((object)test.newWeakGlobalRef(o)).Should().BeSameAs(o);
        }

    }

}
