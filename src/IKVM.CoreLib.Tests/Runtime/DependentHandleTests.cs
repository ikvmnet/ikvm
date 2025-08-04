using FluentAssertions;

using IKVM.CoreLib.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Runtime
{

    [TestClass]
    public class DependentHandleTests
    {

        [TestMethod]
        public void CanCreate()
        {
            var a = new object();
            var b = new object();
            var dh = new DependentHandle<object, object>(a, b);
            dh.IsAllocated.Should().BeTrue();
        }

        [TestMethod]
        public void CanGetTarget()
        {
            var a = new object();
            var b = new object();
            var dh = new DependentHandle<object, object>(a, b);
            dh.Target.Should().BeSameAs(a);
        }

        [TestMethod]
        public void CanGetDependent()
        {
            var a = new object();
            var b = new object();
            var dh = new DependentHandle<object, object>(a, b);
            dh.Dependent.Should().BeSameAs(b);
        }

        [TestMethod]
        public void CanGetTargetAndDependent()
        {
            var a = new object();
            var b = new object();
            var dh = new DependentHandle<object, object>(a, b);
            var td = dh.TargetAndDependent;
            td.Target.Should().BeSameAs(a);
            td.Dependent.Should().BeSameAs(b);
        }

        [TestMethod]
        public void CanDispose()
        {
            var a = new object();
            var b = new object();
            var dh = new DependentHandle<object, object>(a, b);
            dh.Dispose();
        }

    }

}
