using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ClassLoaderTests
    {

        class TestClassLoader : ClassLoader
        {

            Action<TestClassLoader> onFinalize;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="onFinalize"></param>
            public TestClassLoader(Action<TestClassLoader> onFinalize)
            {
                this.onFinalize = onFinalize ?? throw new ArgumentNullException(nameof(onFinalize));
            }

            ~TestClassLoader()
            {
                onFinalize(this);
            }

        }


        [TestMethod]
        public void CannotTrapClassLoaderWithFinalizer()
        {
            ClassLoader cl = null;
            new TestClassLoader(l => cl = l);

            global::java.lang.System.gc();
            global::java.lang.System.runFinalization();
            System.GC.Collect();

            cl.Should().BeNull();
        }

    }

}
