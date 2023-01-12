using FluentAssertions;

using java.lang.invoke;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang.invoke
{

    [TestClass]
    public class MethodHandleTests
    {

        [TestMethod]
        public void CanInvokePublicVirtualMethodWithArgsAndReturn()
        {
            var publicLookup = MethodHandles.publicLookup();
            var mt = MethodType.methodType(typeof(string), typeof(string), typeof(string));
            var mh = publicLookup.findVirtual(typeof(string), "concat", mt);
            mh.invoke("a", "b").Should().Be("ab");
        }

        [TestMethod]
        public void CanReverseArguments()
        {
            var publicLookup = MethodHandles.publicLookup();
            var mt = MethodType.methodType(typeof(string), typeof(string), typeof(string));
            var mh = publicLookup.findVirtual(typeof(string), "concat", mt);
            MethodHandles.permuteArguments(mh, mt, 1, 0);
            mh.invoke("a", "b").Should().Be("ba");
        }

    }

}
