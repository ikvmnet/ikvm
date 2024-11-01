using FluentAssertions;

using IKVM.CoreLib.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Collections
{

    [TestClass]
    public class IndexRangeDictionaryTests
    {

        [TestMethod]
        public void AlignTowardsInfinity()
        {
            IndexRangeDictionary<string>.AlignTowardsInfinity(0).Should().Be(0);
            IndexRangeDictionary<string>.AlignTowardsInfinity(1).Should().Be(8);
            IndexRangeDictionary<string>.AlignTowardsInfinity(7).Should().Be(8);
            IndexRangeDictionary<string>.AlignTowardsInfinity(8).Should().Be(8);
            IndexRangeDictionary<string>.AlignTowardsInfinity(9).Should().Be(16);

            IndexRangeDictionary<string>.AlignTowardsInfinity(-0).Should().Be(-0);
            IndexRangeDictionary<string>.AlignTowardsInfinity(-1).Should().Be(-8);
            IndexRangeDictionary<string>.AlignTowardsInfinity(-7).Should().Be(-8);
            IndexRangeDictionary<string>.AlignTowardsInfinity(-8).Should().Be(-8);
            IndexRangeDictionary<string>.AlignTowardsInfinity(-9).Should().Be(-16);
        }

        [TestMethod]
        public void AlignTowardsZero()
        {
            IndexRangeDictionary<string>.AlignTowardsZero(0).Should().Be(0);
            IndexRangeDictionary<string>.AlignTowardsZero(1).Should().Be(0);
            IndexRangeDictionary<string>.AlignTowardsZero(7).Should().Be(0);
            IndexRangeDictionary<string>.AlignTowardsZero(8).Should().Be(8);
            IndexRangeDictionary<string>.AlignTowardsZero(9).Should().Be(8);

            IndexRangeDictionary<string>.AlignTowardsZero(-0).Should().Be(-0);
            IndexRangeDictionary<string>.AlignTowardsZero(-1).Should().Be(-0);
            IndexRangeDictionary<string>.AlignTowardsZero(-7).Should().Be(-0);
            IndexRangeDictionary<string>.AlignTowardsZero(-8).Should().Be(-8);
            IndexRangeDictionary<string>.AlignTowardsZero(-9).Should().Be(-8);
        }

        [TestMethod]
        public void CanAddBasicItem()
        {
            var d = new IndexRangeDictionary<string>();
            d[0] = "Item1";
            d[0].Should().Be("Item1");
            d._minKey.Should().Be(0);
            d._maxKey.Should().Be(0);
            d._items.Length.Should().Be(8);
        }

        [TestMethod]
        public void CanAddOffsetItem()
        {
            var d = new IndexRangeDictionary<string>();
            d[10] = "Item1";
            d[10].Should().Be("Item1");
            d._minKey.Should().Be(8);
            d._maxKey.Should().Be(16);
            d._items.Length.Should().Be(16);
        }

        [TestMethod]
        public void CanAddSparseRange()
        {
            var d = new IndexRangeDictionary<string>();
            d[10] = "Item1";
            d[10].Should().Be("Item1");
            d._minKey.Should().Be(8);
            d._maxKey.Should().Be(16);
            d._items.Length.Should().Be(16);
            d[20] = "Item2";
            d[10].Should().Be("Item1");
            d[20].Should().Be("Item2");
            d._minKey.Should().Be(8);
            d._maxKey.Should().Be(24);
            d._items.Length.Should().Be(32);
            d[5].Should().BeNull();
            d[10].Should().Be("Item1");
            d[15].Should().BeNull();
            d[19].Should().BeNull();
            d[20].Should().Be("Item2");
            d[21].Should().BeNull();
        }

        [TestMethod]
        public void CanAddMaxBeforeMin()
        {
            var d = new IndexRangeDictionary<string>();
            d[20] = "Item1";
            d[20].Should().Be("Item1");
            d[10] = "Item2";
            d[10].Should().Be("Item2");
            d[20] = "Item1";
            d[20].Should().Be("Item1");
        }

        [TestMethod]
        public void ShiftShouldBeEmpty()
        {
            var d = new IndexRangeDictionary<string>();
            d[2] = "Item2";
            d[2].Should().Be("Item2");
            d[0] = "Item0";
            d[0].Should().Be("Item0");
            d[1].Should().BeNull();
        }

    }

}
