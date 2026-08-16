using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.ikvm.runtime
{

    [TestClass]
    public class UtilDelegateTests
    {

        static global::java.lang.Class Class(string name) => global::java.lang.Class.forName(name);

        static global::java.lang.reflect.Method Method(string className, string methodName, params string[] parameterTypeNames)
        {
            var parameterTypes = new global::java.lang.Class[parameterTypeNames.Length];
            for (int i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = Class(parameterTypeNames[i]);

            return Class(className).getMethod(methodName, parameterTypes);
        }

        [TestMethod]
        public void CanCreateDelegateForInstanceMethod()
        {
            var m = Method("java.util.ArrayList", "size");
            var d = (Func<object, int>)global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object, int>), m);

            var list = new global::java.util.ArrayList();
            list.add("a");
            list.add("b");

            d(list).Should().Be(2);
        }

        [TestMethod]
        public void CanCreateDelegateForInstanceMethodWithArgument()
        {
            var m = Method("java.lang.StringBuilder", "append", "java.lang.String");
            var d = (Func<object, object, object>)global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object, object, object>), m);

            var sb = new global::java.lang.StringBuilder("a");
            d(sb, "b").Should().BeSameAs(sb);
            sb.toString().Should().Be("ab");
        }

        [TestMethod]
        public void CanCreateDelegateForStaticMethod()
        {
            var m = Method("java.lang.Integer", "parseInt", "java.lang.String");
            var d = (Func<object, int>)global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object, int>), m);

            d("42").Should().Be(42);
        }

        [TestMethod]
        public void CanCreateDelegateForVoidMethod()
        {
            var m = Method("java.util.ArrayList", "clear");
            var d = (Action<object>)global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Action<object>), m);

            var list = new global::java.util.ArrayList();
            list.add("a");
            d(list);

            list.size().Should().Be(0);
        }

        /// <summary>
        /// java.lang.Object is a remapped type, so its methods have no single backing MethodBase; the handle has to
        /// route through the instance helper.
        /// </summary>
        [TestMethod]
        public void CanCreateDelegateForMethodOnRemappedType()
        {
            var m = Method("java.lang.Object", "hashCode");
            var d = (Func<object, int>)global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object, int>), m);

            var o = new object();
            d(o).Should().Be(global::java.lang.System.identityHashCode(o));
        }

        [TestMethod]
        public void CanCreateDelegateForConstructor()
        {
            var c = Class("java.util.ArrayList").getConstructor(new global::java.lang.Class[0]);
            var d = (Func<object>)global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object>), c);

            d().Should().BeOfType<global::java.util.ArrayList>();
        }

        /// <summary>
        /// Access is checked as for Lookup.unreflect, so an inaccessible member is rejected unless it has been marked
        /// accessible. java.lang.Runtime's constructor is private.
        /// </summary>
        [TestMethod]
        public void ThrowsForInaccessibleMemberUntilMarkedAccessible()
        {
            var c = Class("java.lang.Runtime").getDeclaredConstructor(new global::java.lang.Class[0]);

            var act = () => global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object>), c);
            act.Should().Throw<global::java.lang.IllegalAccessException>();

            c.setAccessible(true);
            var d = (Func<object>)global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object>), c);
            d().Should().BeOfType<global::java.lang.Runtime>();
        }

        [TestMethod]
        public void CanCreateDelegateForBoundMethodHandle()
        {
            var m = Method("java.util.ArrayList", "size");
            var list = new global::java.util.ArrayList();
            list.add("a");

            var mh = global::java.lang.invoke.MethodHandles.publicLookup().unreflect(m).bindTo(list);
            var d = (Func<int>)global::ikvm.runtime.Util.getDelegateFromMethodHandle(typeof(Func<int>), mh);

            d().Should().Be(1);
        }

        [TestMethod]
        public void ThrowsForTypeThatIsNotADelegate()
        {
            var m = Method("java.util.ArrayList", "size");

            var act = () => global::ikvm.runtime.Util.getDelegateFromMethod(typeof(string), m);
            act.Should().Throw<global::java.lang.IllegalArgumentException>();
        }

        [TestMethod]
        public void ThrowsForIncompatibleDelegateSignature()
        {
            var m = Method("java.lang.Integer", "parseInt", "java.lang.String");

            // parseInt takes one argument; this delegate supplies three
            var act = () => global::ikvm.runtime.Util.getDelegateFromMethod(typeof(Func<object, object, object, int>), m);
            act.Should().Throw<global::java.lang.invoke.WrongMethodTypeException>();
        }

        [TestMethod]
        public void ThrowsForNullArguments()
        {
            var m = Method("java.util.ArrayList", "size");

            var nullType = () => global::ikvm.runtime.Util.getDelegateFromMethodHandle(null, global::java.lang.invoke.MethodHandles.publicLookup().unreflect(m));
            nullType.Should().Throw<global::java.lang.NullPointerException>();

            var nullHandle = () => global::ikvm.runtime.Util.getDelegateFromMethodHandle(typeof(Func<object, int>), null);
            nullHandle.Should().Throw<global::java.lang.NullPointerException>();
        }

    }

}
