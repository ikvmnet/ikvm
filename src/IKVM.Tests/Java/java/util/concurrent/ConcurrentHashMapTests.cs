using FluentAssertions;

using java.util.concurrent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.concurrent
{

    [TestClass]
    public class ConcurrentHashMapTests
    {

        [TestMethod]
        public void CanPutIfAbsentAndGetOrDefault()
        {
            var h = new ConcurrentHashMap();
            var o = new object();
            h.clear();
            h.putIfAbsent(1, o);
            h.getOrDefault(1, null).Should().BeSameAs(o);
        }

    }

}
