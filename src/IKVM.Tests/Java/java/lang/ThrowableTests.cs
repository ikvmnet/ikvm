using System;
using System.IO;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ThrowableTests
    {

        global::java.lang.Class testClass;

        [TestInitialize]
        public void Initialize()
        {
            var s = new StreamReader(typeof(ClassTests).Assembly.GetManifestResourceStream("IKVM.Tests.Java.java.lang.ThrowableTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.tests.java.lang.ThrowableTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
            testClass = c.GetClass("ikvm.tests.java.lang.ThrowableTests");
            if (testClass == null)
                throw new Exception();
        }

        [TestMethod]
        public void CanPrintStackTraceFromDynamic()
        {
            var m = testClass.getMethod("CanPrintStackTraceFromDynamic", new global::java.lang.Class[0]);
            m.invoke(null, new object[0]);
        }

    }

}
