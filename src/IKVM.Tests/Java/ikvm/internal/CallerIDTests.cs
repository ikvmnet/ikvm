using System;
using System.Reflection.Emit;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.ikvm.@internal
{

    [TestClass]
    public class CallerIDTests
    {

        public class Target
        {

        }

        /// <summary>
        /// Builds a <see cref="DynamicMethod"/> that calls <see cref="global::java.lang.Class.forName(string)"/>. The
        /// <see cref="DynamicMethod"/> frame has no declaring type, and so becomes the frame captured as the caller.
        /// </summary>
        /// <returns></returns>
        static Func<string, global::java.lang.Class> CreateDynamicForName()
        {
            var forName = typeof(global::java.lang.Class).GetMethod(nameof(global::java.lang.Class.forName), new[] { typeof(string) });
            forName.Should().NotBeNull();

            var dm = new DynamicMethod("ResolveService", typeof(global::java.lang.Class), new[] { typeof(string) }, typeof(CallerIDTests).Module, true);
            dm.DeclaringType.Should().BeNull();

            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, forName);
            il.Emit(OpCodes.Ret);

            return (Func<string, global::java.lang.Class>)dm.CreateDelegate(typeof(Func<string, global::java.lang.Class>));
        }

        [TestMethod]
        public void CallerSensitiveMethodCalledFromDynamicMethodShouldNotThrow()
        {
            var forName = CreateDynamicForName();
            forName.Invoking(f => f("java.lang.String")).Should().NotThrow();
            forName("java.lang.String").getName().Should().Be("java.lang.String");
        }

        [TestMethod]
        public void CallerSensitiveMethodCalledFromDynamicMethodShouldResolveNextRealCaller()
        {
            // Target is only visible through the class loader of this assembly, so it can only be found if the
            // DynamicMethod frame is skipped and this method is resolved as the caller
            var forName = CreateDynamicForName();
            var c = forName("cli.IKVM.Tests.Java.ikvm.internal.CallerIDTests$Target");
            c.Should().BeSameAs(global::ikvm.runtime.Util.getClassFromTypeHandle(typeof(Target).TypeHandle));
            c.getClassLoader().Should().BeSameAs(global::ikvm.runtime.Util.getClassFromTypeHandle(typeof(CallerIDTests).TypeHandle).getClassLoader());
        }

    }

}
