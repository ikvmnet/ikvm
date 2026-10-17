using FluentAssertions;

using IKVM.CoreLib.Modules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Modules
{

    [TestClass]
    public class ModuleVersionTests
    {

        [TestMethod]
        public void CanParseOnePartVersion()
        {
            var v = ModuleVersion.Parse("1");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(1);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Pre.Should().BeEmpty();
            v.Build.Should().BeEmpty();
        }

        [TestMethod]
        public void CanParseTwoPartVersion()
        {
            var v = ModuleVersion.Parse("1.0");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(2);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Sequence[1].AsInteger().Should().Be(0);
            v.Pre.Should().BeEmpty();
            v.Build.Should().BeEmpty();
        }

        [TestMethod]
        public void CanParseThreePartVersion()
        {
            var v = ModuleVersion.Parse("1.0.1");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(3);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Sequence[1].AsInteger().Should().Be(0);
            v.Sequence[2].AsInteger().Should().Be(1);
            v.Pre.Should().BeEmpty();
            v.Build.Should().BeEmpty();
        }

        [TestMethod]
        public void CanParseOnePartPreVersion()
        {
            var v = ModuleVersion.Parse("1.0.1-pre");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(3);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Sequence[1].AsInteger().Should().Be(0);
            v.Sequence[2].AsInteger().Should().Be(1);
            v.Pre.Should().HaveCount(1);
            v.Pre[0].AsString().Should().Be("pre");
            v.Build.Should().BeEmpty();
        }

        [TestMethod]
        public void CanParseTwoPartPreVersion()
        {
            var v = ModuleVersion.Parse("1.0.1-pre.1");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(3);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Sequence[1].AsInteger().Should().Be(0);
            v.Sequence[2].AsInteger().Should().Be(1);
            v.Pre.Should().HaveCount(2);
            v.Pre[0].AsString().Should().Be("pre");
            v.Pre[1].AsInteger().Should().Be(1);
            v.Build.Should().BeEmpty();
        }

        [TestMethod]
        public void CanParseThreePartPreVersion()
        {
            var v = ModuleVersion.Parse("1.0.1-pre.1.0");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(3);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Sequence[1].AsInteger().Should().Be(0);
            v.Sequence[2].AsInteger().Should().Be(1);
            v.Pre.Should().HaveCount(3);
            v.Pre[0].AsString().Should().Be("pre");
            v.Pre[1].AsInteger().Should().Be(1);
            v.Pre[2].AsInteger().Should().Be(0);
            v.Build.Should().BeEmpty();
        }

        [TestMethod]
        public void CanParseOnePartBuildVersion()
        {
            var v = ModuleVersion.Parse("1.0.1-pre.1.0+1");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(3);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Sequence[1].AsInteger().Should().Be(0);
            v.Sequence[2].AsInteger().Should().Be(1);
            v.Pre.Should().HaveCount(3);
            v.Pre[0].AsString().Should().Be("pre");
            v.Pre[1].AsInteger().Should().Be(1);
            v.Pre[2].AsInteger().Should().Be(0);
            v.Build.Should().HaveCount(1);
            v.Build[0].AsInteger().Should().Be(1);
        }

        [TestMethod]
        public void CanParseTwoPartBuildVersion()
        {
            var v = ModuleVersion.Parse("1.0.1-pre.1.0+1.0");
            v.IsValid.Should().BeTrue();
            v.Sequence.Should().HaveCount(3);
            v.Sequence[0].AsInteger().Should().Be(1);
            v.Sequence[1].AsInteger().Should().Be(0);
            v.Sequence[2].AsInteger().Should().Be(1);
            v.Pre.Should().HaveCount(3);
            v.Pre[0].AsString().Should().Be("pre");
            v.Pre[1].AsInteger().Should().Be(1);
            v.Pre[2].AsInteger().Should().Be(0);
            v.Build.Should().HaveCount(2);
            v.Build[0].AsInteger().Should().Be(1);
            v.Build[1].AsInteger().Should().Be(0);
        }

        [TestMethod]
        public void CompareIsLexicographic()
        {
            ModuleVersion.Parse("1.10").CompareTo(ModuleVersion.Parse("1.2.3")).Should().BePositive();
            ModuleVersion.Parse("1.2").CompareTo(ModuleVersion.Parse("1.2.1")).Should().BeNegative();
            ModuleVersion.Parse("1.2.3").CompareTo(ModuleVersion.Parse("1.2.3")).Should().Be(0);
        }

        [TestMethod]
        public void CompareStringComponentsOrdinally()
        {
            // a culture-sensitive comparison puts "a" before "B", Java's String.compareTo puts "B" first
            ModuleVersion.Parse("1.0-B").CompareTo(ModuleVersion.Parse("1.0-a")).Should().BeNegative();
        }

        [TestMethod]
        public void CompareInvalidVersions()
        {
            default(ModuleVersion).CompareTo(default).Should().Be(0);
            ModuleVersion.Parse("1").CompareTo(default).Should().BePositive();
            default(ModuleVersion).CompareTo(ModuleVersion.Parse("1")).Should().BeNegative();
        }

    }

}
