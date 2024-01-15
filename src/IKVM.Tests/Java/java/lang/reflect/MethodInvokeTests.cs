using System.IO;

using FluentAssertions;

using IKVM.Java.Tests.Util;

using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang.reflect
{

    [TestClass]
    public class MethodInvokeTests
    {

        static class StaticTestObject
        {

            public static void TestVoidReturnType()
            {

            }

            public static void TestIntArgVoidReturnType(int i)
            {
                i.Should().Be(1);
            }

            public static void TestBoxedIntArgVoidReturnType(object i)
            {
                i.Should().BeOfType<int>();
                i.Should().Be(1);
            }

            public static void TestStringArgVoidReturnType(string i)
            {
                i.Should().Be("TEST");
            }

            public static bool TestPrimitiveReturnType()
            {
                return true;
            }

        }

        [TestMethod]
        public void CanInvokeStaticMethodWithVoidReturnType()
        {
            var m = ((Class)typeof(StaticTestObject)).getDeclaredMethod("TestVoidReturnType");
            var r = m.invoke(null);
            r.Should().BeNull();
        }

        [TestMethod]
        public void CanInvokeStaticMethodWithIntArgVoidReturnType()
        {
            var m = ((Class)typeof(StaticTestObject)).getDeclaredMethod("TestIntArgVoidReturnType", typeof(int));
            var r = m.invoke(null, new Integer(1));
            r.Should().BeNull();
        }

        [TestMethod]
        public void CanInvokeStaticMethodWithBoxedIntArgVoidReturnType()
        {
            var m = ((Class)typeof(StaticTestObject)).getDeclaredMethod("TestBoxedIntArgVoidReturnType", typeof(object));
            var r = m.invoke(null, 1);
            r.Should().BeNull();
        }

        [TestMethod]
        public void CanInvokeStaticMethodWithStringArgVoidReturnType()
        {
            var m = ((Class)typeof(StaticTestObject)).getDeclaredMethod("TestStringArgVoidReturnType", typeof(string));
            var r = m.invoke(null, "TEST");
            r.Should().BeNull();
        }

        [TestMethod]
        public void CanInvokeStaticMethodWithPrimitiveReturnType()
        {
            var m = ((Class)typeof(StaticTestObject)).getDeclaredMethod("TestPrimitiveReturnType");
            var r = (Boolean)m.invoke(null);
            r.booleanValue().Should().BeTrue();
        }

        class TestObject
        {

            public static void TestVoidReturnType()
            {

            }

            public bool TestPrimitiveReturnType()
            {
                return true;
            }

        }

        [TestMethod]
        public void CanInvokeMethodWithVoidReturnType()
        {
            var m = ((Class)typeof(TestObject)).getDeclaredMethod("TestVoidReturnType");
            var o = new TestObject();
            var r = m.invoke(o);
            r.Should().BeNull();
        }

        [TestMethod]
        public void CanInvokeMethodWithPrimitiveReturnType()
        {
            var m = ((Class)typeof(TestObject)).getDeclaredMethod("TestPrimitiveReturnType");
            var o = new TestObject();
            var r = (Boolean)m.invoke(o);
            r.booleanValue().Should().BeTrue();
        }

        struct TestValue
        {

            public static void TestVoidReturnType()
            {

            }

            public bool TestPrimitiveReturnType()
            {
                return true;
            }

        }

        [TestMethod]
        public void CanInvokeValueMethodWithVoidReturnType()
        {
            var m = ((Class)typeof(TestValue)).getDeclaredMethod("TestVoidReturnType");
            var o = new TestValue();
            var r = m.invoke(o);
            r.Should().BeNull();
        }

        [TestMethod]
        public void CanInvokeValueMethodWithPrimitiveReturnType()
        {
            var m = ((Class)typeof(TestValue)).getDeclaredMethod("TestPrimitiveReturnType");
            var o = new TestValue();
            var r = (Boolean)m.invoke(o);
            r.booleanValue().Should().BeTrue();
        }

        /// <summary>
        /// Failures have been indicated with invoking methods in dynamic code with covariant return types.
        /// </summary>
        /// <exception cref="Exception"></exception>
        [TestMethod]
        public void CanInvokenCovariantReturnTypeMethodDynamic()
        {
            var s = new StreamReader(typeof(ClassTests).Assembly.GetManifestResourceStream("IKVM.Tests.Java.java.lang.reflect.MethodInvokeTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.java.tests.java.lang.reflect.MethodInvokeTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();

            var z = c.GetClass("ikvm.java.tests.java.lang.reflect.MethodInvokeTests$CovariantReturn");
            if (z == null)
                throw new Exception();

            var m = z.getMethod("method", global::System.Array.Empty<global::java.lang.Class>());
            var o = z.newInstance();
            m.invoke(o, global::System.Array.Empty<object>());
        }

    }

}
