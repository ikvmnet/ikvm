using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Attributes;

using java.util.concurrent.atomic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.concurrent.atomic
{

    [TestClass]
    public class AtomicTests
    {

        [TestMethod]
        public void CanLazySetBoolean()
        {
            var o = new AtomicBoolean(false);
            Task.Run(() => o.lazySet(true)).Wait();
            o.get().Should().Be(true);
        }

        [TestMethod]
        public void CanLazySetInteger()
        {
            var o = new AtomicInteger(0);
            Task.Run(() => o.lazySet(1)).Wait();
            o.get().Should().Be(1);
        }

        [TestMethod]
        public void CanLazySetLong()
        {
            var o = new AtomicLong(0);
            Task.Run(() => o.lazySet(1)).Wait();
            o.get().Should().Be(1);
        }

        class TestObject
        {

            public volatile object oo;
            public volatile string ss;
            public volatile int ii;

            [Modifiers(Modifiers.Volatile)]
            public long ll;

        }

        [TestMethod]
        public void CanLazySetObjectField()
        {
            var t = new object();
            var u = AtomicReferenceFieldUpdater.newUpdater(typeof(TestObject), typeof(object), "oo");
            var o = new TestObject();
            Task.Run(() => u.lazySet(o, t)).Wait();
            o.oo.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanLazySetStringField()
        {
            var t = "TEST";
            var u = AtomicReferenceFieldUpdater.newUpdater(typeof(TestObject), typeof(string), "ss");
            var o = new TestObject();
            Task.Run(() => u.lazySet(o, t)).Wait();
            o.ss.Should().BeSameAs(t);
        }

        [TestMethod]
        public void CanLazySetIntegerField()
        {
            var u = AtomicIntegerFieldUpdater.newUpdater(typeof(TestObject), "ii");
            var o = new TestObject();
            Task.Run(() => u.lazySet(o, 1)).Wait();
            o.ii.Should().Be(1);
        }

        [TestMethod]
        public void CanLazySetLongField()
        {
            var u = AtomicLongFieldUpdater.newUpdater(typeof(TestObject), "ll");
            var o = new TestObject();
            Task.Run(() => u.lazySet(o, 1)).Wait();
            o.ll.Should().Be(1);
        }

        [TestMethod]
        public void CanLazySetIntegerArray()
        {
            var a = new AtomicIntegerArray(1);
            Task.Run(() => a.lazySet(0, 1)).Wait();
            a.get(0).Should().Be(1);
        }

        [TestMethod]
        public void CanLazySetLongArray()
        {
            var a = new AtomicLongArray(1);
            Task.Run(() => a.lazySet(0, 1)).Wait();
            a.get(0).Should().Be(1);
        }

    }

}
