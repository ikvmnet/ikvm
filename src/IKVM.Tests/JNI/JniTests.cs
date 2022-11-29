using System.IO;

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
        public void NewObjectATest()
        {
            test.newObjectATest();
        }

    }

}
