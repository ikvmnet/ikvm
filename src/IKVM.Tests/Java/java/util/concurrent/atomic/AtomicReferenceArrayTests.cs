using FluentAssertions;

using java.util.concurrent.atomic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.concurrent.atomic
{

    [TestClass]
    public class AtomicReferenceArrayTests
    {

        [TestMethod]
        public void CanLazySetArray()
        {
            var ao = new AtomicReferenceArray(32);
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.lazySet(idx, new object());
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.get(idx).Should().NotBeNull();
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.lazySet(idx, null);
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.get(idx).Should().BeNull();
        }

        [TestMethod]
        public void CanSetArray()
        {
            var ao = new AtomicReferenceArray(32);
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.set(idx, new object());
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.get(idx).Should().NotBeNull();
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.set(idx, null);
            for (int idx = 0; idx < ao.length(); ++idx)
                ao.get(idx).Should().BeNull();
        }

    }

}
