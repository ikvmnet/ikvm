using FluentAssertions;

using IKVM.CoreLib.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Collections
{

    [TestClass]
    public class ArrayDequeTests
    {

        [TestMethod]
        public void CanInsertFirstAndRemoveFirst()
        {
            var o = new object();
            var d = new ArrayDeque<object>();
            d.InsertFirst(o);
            d.RemoveFirst().Should().BeSameAs(o);
        }

    }

}
