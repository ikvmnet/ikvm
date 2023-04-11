using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ObjectTests
    {

        class ClassWithFinalize : global::java.lang.Object
        {

            Action<ClassWithFinalize> onFinalize;

            public ClassWithFinalize(Action<ClassWithFinalize> onFinalize)
            {
                this.onFinalize = onFinalize;
                throw new Exception(); // abort ctor, never calling base
            }

            protected override void finalize()
            {
                onFinalize(this);
            }

            ~ClassWithFinalize()
            {
                // this should be hit
            }

        }

        /// <summary>
        /// JLS tells us that objects that have not completed java.lang.Object:init should not invoke finalize().
        /// </summary>
        [TestMethod]
        public void AbortingConstructorShouldNotFinalize()
        {
            ClassWithFinalize o = null;

            void Run()
            {
                try
                {
                    new ClassWithFinalize(t => o = t);
                }
                catch (Exception e)
                {
                    // ignore
                }
            }

            Run();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            o.Should().BeNull();
        }

    }

}
