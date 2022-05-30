using System;
using System.IO;

using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.JNI
{

    [TestClass]
    public partial class JniTests
    {

        [TestMethod]
        public void Can_invoke_native_method()
        {
            var s = new StreamReader(typeof(JniTests).Assembly.GetManifestResourceStream("IKVM.Tests.JNI.JniTests.java")).ReadToEnd();
            s = s.Replace("@@IKVM_TESTS_NATIVE@@", Native.GetLibraryPath().Replace(@"\", @"\\"));
            var f = new InMemoryCodeUnit("ikvm.tests.jni.JniTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
            var z = c.GetClass("ikvm.tests.jni.JniTests");
            if (z == null)
                throw new Exception();

            var m = z.getDeclaredMethod("echo", typeof(string));
            var r = (string)m.invoke(null, "TEST");
            r.Should().Be("TEST");
        }

    }

}
