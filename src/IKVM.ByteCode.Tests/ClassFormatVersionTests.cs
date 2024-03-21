using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests
{

    [TestClass]
    public class ClassFormatVersionTests
    {

        [TestMethod]
        public void CompareToShouldBeZeroForEqual()
        {
            new ClassFormatVersion(1, 0).CompareTo(new ClassFormatVersion(1, 0)).Should().Be(0);
        }

        [TestMethod]
        public void CompareToShouldBeNegativeOneForGreaterMajor()
        {
            new ClassFormatVersion(1, 0).CompareTo(new ClassFormatVersion(2, 0)).Should().Be(-1);
        }

        [TestMethod]
        public void CompareToShouldBeNegativeOneForGreaterMinor()
        {
            new ClassFormatVersion(1, 0).CompareTo(new ClassFormatVersion(1, 1)).Should().Be(-1);
        }

        [TestMethod]
        public void CompareToShouldBeOneForGreaterMajor()
        {
            new ClassFormatVersion(2, 0).CompareTo(new ClassFormatVersion(1, 0)).Should().Be(1);
        }

        [TestMethod]
        public void CompareToShouldBeOneForGreaterMinor()
        {
            new ClassFormatVersion(1, 1).CompareTo(new ClassFormatVersion(1, 0)).Should().Be(1);
        }

        [TestMethod]
        public void ImplicitOperatorsShouldReturnCorrectValues()
        {
            (new ClassFormatVersion(1, 0) > new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(1, 0) < new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(1, 0) >= new ClassFormatVersion(1, 0)).Should().BeTrue();
            (new ClassFormatVersion(1, 0) <= new ClassFormatVersion(1, 0)).Should().BeTrue();
            (new ClassFormatVersion(1, 0) == new ClassFormatVersion(1, 0)).Should().BeTrue();
            (new ClassFormatVersion(1, 0) != new ClassFormatVersion(1, 0)).Should().BeFalse();

            (new ClassFormatVersion(1, 1) > new ClassFormatVersion(1, 0)).Should().BeTrue();
            (new ClassFormatVersion(1, 1) < new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(1, 1) >= new ClassFormatVersion(1, 0)).Should().BeTrue();
            (new ClassFormatVersion(1, 1) <= new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(1, 1) == new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(1, 1) != new ClassFormatVersion(1, 0)).Should().BeTrue();

            (new ClassFormatVersion(2, 0) > new ClassFormatVersion(1, 0)).Should().BeTrue();
            (new ClassFormatVersion(2, 0) < new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(2, 0) >= new ClassFormatVersion(1, 0)).Should().BeTrue();
            (new ClassFormatVersion(2, 0) <= new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(2, 0) == new ClassFormatVersion(1, 0)).Should().BeFalse();
            (new ClassFormatVersion(2, 0) != new ClassFormatVersion(1, 0)).Should().BeTrue();
        }

    }

}
