using System.Configuration;
using System.IO;
using System.Linq;

using java.lang;
using java.lang.reflect;
using java.net;

using Type = System.Type;

namespace IKVM.JTReg.TestAdapter
{

    static class JTRegTypes
    {

        static readonly string[] libs = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "jtreg"), "*.jar");
        public static readonly URLClassLoader ClassLoader = new URLClassLoader(libs.Select(i => new java.io.File(i).toURI().toURL()).ToArray());

        public static class SearchPath
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.agent.SearchPath", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor(new Class[] { (Class)typeof(string[]) });

            public static dynamic New(params string[] paths) => Constructor1.newInstance(new[] { paths });

        }

        public static class Harness
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.Harness", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Method GetClassDirMethod = Class.getMethod("getClassDir");
            public static readonly Method SetClassDirMethod = Class.getMethod("setClassDir", typeof(java.io.File));

            public static dynamic New() => Class.newInstance();

            public static class Observer
            {

                public static readonly Class Class = Class.forName("com.sun.javatest.Harness$Observer", true, ClassLoader);
                public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

            }


        }

        public static class ProductInfo
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.ProductInfo", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Method GetJavaTestClassDirMethod = Class.getMethod("getJavaTestClassDir");

        }

        public static class TestManager
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.TestManager", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor(typeof(java.io.PrintWriter), typeof(java.io.File), TestFinder.ErrorHandler.Class);

            public static dynamic New(java.io.PrintWriter pw, java.io.File file, dynamic errorHandler) => Constructor1.newInstance(pw, file, errorHandler);

        }

        public static class TestFinder
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.TestFinder", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

            public static class ErrorHandler
            {

                public static readonly Class Class = Class.forName("com.sun.javatest.TestFinder$ErrorHandler", true, ClassLoader);
                public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

            }

        }

        public static class RegressionParameters
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.RegressionParameters", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static Constructor Constructor1 = Class.getConstructor(typeof(string), RegressionTestSuite.Class);

            public static dynamic New(string tag, dynamic testSuite) => Constructor1.newInstance(tag, testSuite);

        }

        public static class RegressionTestSuite
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.RegressionTestSuite", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

        }

        public static class JDK
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.JDK", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Method OfMethod = Class.getMethod("of", typeof(java.io.File));

            public static dynamic Of(java.io.File jdk) => OfMethod.invoke(null, jdk);

        }

        public static class ExecMode
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.ExecMode", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly dynamic OTHERVM = java.lang.Enum.valueOf(Class, "OTHERVM");
            public static readonly dynamic AGENTVM = java.lang.Enum.valueOf(Class, "AGENTVM");

        }

        public static class IgnoreKind
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.IgnoreKind", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly dynamic RUN = java.lang.Enum.valueOf(Class, "RUN");
            public static readonly dynamic QUIET = java.lang.Enum.valueOf(Class, "QUIET");

        }

        public static class OS
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.OS", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Method CurrentMethod = Class.getMethod("current");
            public static dynamic Current() => CurrentMethod.invoke(null);

        }

        public static class TestResult
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.TestResult", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

        }

    }

}
