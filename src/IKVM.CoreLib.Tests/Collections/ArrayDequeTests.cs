using System.Collections.Generic;

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

        [TestMethod]
        public void CanEnumerateAfterRemoveFirst()
        {
            var d = new ArrayDeque<int>();
            d.InsertLast(1);
            d.InsertLast(2);
            d.InsertLast(3);
            d.RemoveFirst();
            d.Should().Equal(2, 3);
            var l = new List<int>();
            d.CopyTo(l);
            l.Should().Equal(2, 3);
            d.ToImmutableArray().Should().Equal(2, 3);
        }

        [TestMethod]
        public void CanEnumerateWhenWrapped()
        {
            var d = new ArrayDeque<int>();
            d.InsertFirst(1);
            d.InsertLast(2);
            d.Should().Equal(1, 2);
            var l = new List<int>();
            d.CopyTo(l);
            l.Should().Equal(1, 2);
            d.ToImmutableArray().Should().Equal(1, 2);
            d.ToArray().Should().Equal(1, 2);
        }

    }

}
