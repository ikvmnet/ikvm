using FluentAssertions;

using IKVM.Java.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ExceptionTests
    {

        class TestException : global::java.lang.Exception
        {



        }

        [TestMethod]
        public void CanThrowAndCatchCustomException()
        {
            TestException e;

            try
            {
                throw new TestException();
            }
            catch (TestException _)
            {
                e = _;
            }

            e.Should().NotBeNull();
        }

        [TestMethod]
        public void CanGetStackTraceOfThrownException()
        {
            TestException e;

            try
            {
                throw new TestException();
            }
            catch (TestException _)
            {
                e = _;
            }

            e.Should().NotBeNull();
            var t = e.getStackTrace();
        }

        [TestMethod]
        public void CanPrintStackTraceOfThrownExceptionFromDynamicCompiler()
        {
            // compile the java test code on the fly
            var source = """
package ikvm.tests.java.java.lang;

public final class CanGetStackTraceOfThrownExceptionFromDynamicCompilerTest {

    public void main() {
        try {
            throw new Exception();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
""";
            var unit = new InMemoryCodeUnit("ikvm.tests.java.java.lang.CanGetStackTraceOfThrownExceptionFromDynamicCompilerTest", source);
            var compiler = new InMemoryCompiler([unit]);
            compiler.Compile();

            // create an instance of the test class
            var clazz = compiler.GetClass("ikvm.tests.java.java.lang.CanGetStackTraceOfThrownExceptionFromDynamicCompilerTest");
            var ctor = clazz.getConstructor();
            var test = (dynamic)ctor.newInstance([]);
            test.main();
        }

    }

}
