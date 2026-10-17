using FluentAssertions;

using IKVM.CoreLib.Modules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Modules
{

    [TestClass]
    public class ReleaseVersionTests
    {

        [TestMethod]
        public void Foo()
        {
            var v = RuntimeVersion.Parse("1");
        }

        [TestMethod]
        public void CompareAlphanumericPreRelease()
        {
            RuntimeVersion.Parse("9-ea1").CompareTo(RuntimeVersion.Parse("9-ea2")).Should().BeNegative();
        }

        [TestMethod]
        public void CompareNumericPreRelease()
        {
            RuntimeVersion.Parse("9-10").CompareTo(RuntimeVersion.Parse("9-9")).Should().BePositive();
        }

        [TestMethod]
        public void ComparePreReleaseOrdinally()
        {
            // a culture-sensitive comparison puts "a" before "B", Java's String.compareTo puts "B" first
            RuntimeVersion.Parse("9-B").CompareTo(RuntimeVersion.Parse("9-a")).Should().BeNegative();
        }

        [TestMethod]
        public void CompareOptionalOrdinally()
        {
            RuntimeVersion.Parse("9+1-B").CompareTo(RuntimeVersion.Parse("9+1-a")).Should().BeNegative();
        }

    }

}
