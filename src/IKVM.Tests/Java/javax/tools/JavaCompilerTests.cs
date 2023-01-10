using FluentAssertions;

using IKVM.Tests.Util;

using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.javax.tools
{

    [TestClass]
    public class JavaCompilerTests
    {

        [TestMethod]
        public void CanGetJavaCompilerResourceBundle()
        {
            var b = ResourceBundle.getBundle("com.sun.tools.javac.resources.javac");
            var s = b.getString("javac.msg.bug");
            s.Should().NotBeEmpty();
        }

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
    }sdk
    private Runnable r = () -> { };
}
""";

            var f = new InMemoryCodeUnit("L1", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
        }

    }

}
