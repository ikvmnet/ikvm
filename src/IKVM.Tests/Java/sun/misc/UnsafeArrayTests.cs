using System;
using System.IO;

using IKVM.Java.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.sun.misc
{

    /// <summary>
    /// Runs the Unsafe object array operations from compiled Java code, where they are subject to the intrinsics.
    /// </summary>
    [TestClass]
    public class UnsafeArrayTests
    {

        static global::java.lang.Class testClass;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var s = new StreamReader(typeof(UnsafeArrayTests).Assembly.GetManifestResourceStream("IKVM.Tests.Java.sun.misc.UnsafeArrayTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.tests.java.sun.misc.UnsafeArrayTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
            testClass = c.GetClass("ikvm.tests.java.sun.misc.UnsafeArrayTests") ?? throw new Exception();
        }

        [DataTestMethod]
        [DataRow("testObjectArray")]
        [DataRow("testStringArray")]
        [DataRow("testIntegerArray")]
        [DataRow("testClassArray")]
        [DataRow("testInterfaceArray")]
        [DataRow("testSubClassArrayAsClassArray")]
        [DataRow("testExactStringArray")]
        [DataRow("testObjectArrayOfArrays")]
        public void CanOperateOnArray(string name)
        {
            try
            {
                testClass.getMethod(name, Array.Empty<global::java.lang.Class>()).invoke(null, Array.Empty<object>());
            }
            catch (global::java.lang.reflect.InvocationTargetException e)
            {
                throw new Exception(e.getCause().ToString(), e.getCause());
            }
        }

    }

}
