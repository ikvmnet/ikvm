using FluentAssertions;

using global::java.lang;
using global::java.lang.invoke;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang.invoke
{

    [TestClass]
    public class MethodHandleTests
    {

        [TestMethod]
        [Ignore("#276")]
        public void CanInvokeVirtualMethodExact()
        {
            var lookup = MethodHandles.lookup();
            var mt = MethodType.methodType((Class)typeof(string), new[] { (Class)typeof(char), (Class)typeof(char) });
            var mh = lookup.findVirtual((Class)typeof(string), "replace", mt);
            var x = (string)mh.invokeExact(new object[] { "daddy", 'd', 'n' });
            x.Should().Be("nanny");
        }

        [TestMethod]
        [Ignore("#276")]
        public void CanInvokeVirtualMethodWithArguments()
        {
            var lookup = MethodHandles.lookup();
            var mt = MethodType.methodType((Class)typeof(string), new[] { (Class)typeof(char), (Class)typeof(char) });
            var mh = lookup.findVirtual((Class)typeof(string), "replace", mt);
            ((string)mh.invokeWithArguments("sappy", 'p', 'v')).Should().Be("savvy");
        }

        [TestMethod]
        public void CanInvokeStaticVarArgsMethod()
        {
            var lookup = MethodHandles.lookup();
            var mt = MethodType.methodType((Class)typeof(global::java.util.List), new[] { (Class)typeof(object[]) });
            var mh = lookup.findStatic(typeof(global::java.util.Arrays), "asList", mt);
            mh.isVarargsCollector().Should().BeTrue();
            var x = mh.invoke("one", "two");
            x.Should().Be(global::java.util.Arrays.asList("one", "two"));
        }

        [TestMethod]
        [Ignore("#276")]
        public void CanReverseArguments()
        {
            var lookup = MethodHandles.lookup();
            var mt = MethodType.methodType((Class)typeof(string), new[] { (Class)typeof(char), (Class)typeof(char) });
            var mh = lookup.findVirtual((Class)typeof(string), "replace", mt);
            MethodHandles.permuteArguments(mh, mt, 1, 0);
            mh.invoke("a", "b").Should().Be("ba");
        }

    }

}
