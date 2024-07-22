using System.IO;
using System.Linq;

using java.lang;
using java.lang.reflect;
using java.net;

using Type = System.Type;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Provides information and accessors for JTReg types, which are dynamically loaded.
    /// </summary>
    internal static class JTRegTypes
    {

        static readonly string[] libs = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(typeof(JTRegTestManager).Assembly.Location), "jtreg"), "*.jar");
        public static readonly URLClassLoader ClassLoader = new URLClassLoader(libs.Select(i => new java.io.File(i).toURI().toURL()).ToArray());

        public static class TestEnvironment
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.TestEnvironment", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

            public static readonly Method AddDefaultPropTableMethod = Class.getMethod("addDefaultPropTable", typeof(string), typeof(java.util.Properties));
            public static dynamic AddDefaultPropTable(string name, java.util.Properties propTable) => AddDefaultPropTableMethod.invoke(null, name, propTable);


        }

        public static class TestDescription
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.TestDescription", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

        }

        public static class Agent
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.exec.Agent", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

            public static class Pool
            {

                public static readonly Class Class = Class.forName("com.sun.javatest.regtest.exec.Agent$Pool", true, ClassLoader);
                public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

                public static readonly Method InstanceMethod = Class.getMethod("instance", new Class[] { RegressionParameters.Class });
                public static dynamic Instance(object @params) => InstanceMethod.invoke(null, @params);

            }

        }

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

        public static class ElapsedTimeHandler
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.report.ElapsedTimeHandler", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor();

            public static dynamic New() => Constructor1.newInstance();

        }

        public static class TestStats
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.report.TestStats", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor();

            public static dynamic New() => Constructor1.newInstance();

        }

        public static class TestFilter
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.TestFilter", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

        }

        public static class CompositeFilter
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.CompositeFilter", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor(TestFilter.Type.MakeArrayType());

            public static dynamic New(System.Array filters) => Constructor1.newInstance(new[] { filters });

        }

        public static class CachingTestFilter
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.CachingTestFilter", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor(TestFilter.Type.MakeArrayType());

            public static dynamic New(System.Array filters) => Constructor1.newInstance(new[] { filters });

        }

        public static class ParameterFilter
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.ParameterFilter", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor();

            public static dynamic New() => Constructor1.newInstance();

        }

        public static class StatusFilter
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.StatusFilter", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor(typeof(bool[]), TestResultTable.Class);

            public static dynamic New(bool[] statusValues, object trt) => Constructor1.newInstance(statusValues, trt);

        }

        public static class RegressionReporter
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.report.RegressionReporter", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor(typeof(java.io.PrintWriter));

            public static dynamic New(java.io.PrintWriter out_) => Constructor1.newInstance(out_);

        }

        public static class TestManager
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.config.TestManager", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);
            public static readonly Constructor Constructor1 = Class.getConstructor(typeof(java.io.PrintWriter), typeof(java.io.File), TestFinder.ErrorHandler.Class);

            public static dynamic New(java.io.PrintWriter pw, java.io.File file, dynamic errorHandler) => Constructor1.newInstance(pw, file, errorHandler);

        }

        public static class TestResultTable
        {

            public static readonly Class Class = Class.forName("com.sun.javatest.regtest.TestResultTable", true, ClassLoader);
            public static readonly Type Type = ikvm.runtime.Util.getInstanceTypeFromClass(Class);

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
