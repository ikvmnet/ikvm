using FluentAssertions;

using IKVM.CoreLib.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Collections
{

    [TestClass]
    public class IndexRangeDictionaryTests
    {

        [TestMethod]
        public void CanAddBasicItem()
        {
            var d = new IndexRangeDictionary<string>();
            d[0] = "Item1";
            d[0].Should().Be("Item1");
        }

        [TestMethod]
        public void CanAddOffsetItem()
        {
            var d = new IndexRangeDictionary<string>();
            d[10] = "Item1";
            d[10].Should().Be("Item1");
        }

        [TestMethod]
        public void CanAddSparseRange()
        {
            var d = new IndexRangeDictionary<string>();
            d[10] = "Item1";
            d[20] = "Item2";
            d[9].Should().BeNull();
            d[10].Should().Be("Item1");
            d[11].Should().BeNull();
            d[19].Should().BeNull();
            d[20].Should().Be("Item2");
            d[21].Should().BeNull();
        }

        [TestMethod]
        public void CanAddMaxBeforeMin()
        {
            var d = new IndexRangeDictionary<string>();
            d[20] = "Item1";
            d[10] = "Item2";
            d[20].Should().Be("Item1");
            d[10].Should().Be("Item2");
        }

    }

}
