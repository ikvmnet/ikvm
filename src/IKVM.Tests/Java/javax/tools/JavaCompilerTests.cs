using FluentAssertions;

using IKVM.Tests.Util;

using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.javax.tools
{

    [TestClass]
    public class JavaCompilerTests
    {

        /// <summary>
        /// We have encounted a few issues with loading resource bundles for javac. This ensures we can.
        /// </summary>
        [TestMethod]
        public void CanGetJavaCompilerResourceBundle()
        {
            var b = ResourceBundle.getBundle("com.sun.tools.javac.resources.javac");
            var s = b.getString("javac.msg.bug");
            s.Should().NotBeEmpty();
        }

        /// <summary>
        /// Checks that the compiler can handle nested lambda instances. These produce odd type names, and we have
        /// hit issues with various parts of the system not respecting them.
        /// </summary>
        [TestMethod]
        public void CanCompileLambda()
        {
            var s = """
public class L1 {
    public static class A {
        private Runnable r = () -> { };
    }
    public static class B {
        private Runnable r = () -> { };
    }
    private Runnable r = () -> { };
}
""";

            var f = new InMemoryCodeUnit("L1", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
        }

    }

}
