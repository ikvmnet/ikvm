using System.IO;

using FluentAssertions;

using IKVM.CoreLib.Modules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Modules
{

    [TestClass]
    public class ModulePathTests
    {

        static readonly string RESOURCE_PATH = Path.Combine(Path.GetDirectoryName(typeof(ModulePathTests).Assembly.Location!)!, "res", "Modules");

        [TestMethod]
        public void CanFindExplodedModules()
        {
            var path = ModulePath.Create([RESOURCE_PATH]);
            path.Find("m1").Should().NotBeNull();
            path.Find("m2").Should().NotBeNull();
            path.Find("m3").Should().NotBeNull();
            path.Find("m4").Should().NotBeNull();
        }

        [TestMethod]
        public void CanFindJarModules()
        {
            var path = ModulePath.Create([Path.Combine(RESOURCE_PATH, "jars")]);
            path.Find("m1").Should().NotBeNull();
            path.Find("m2").Should().NotBeNull();
            path.Find("m3").Should().NotBeNull();
            path.Find("m4").Should().NotBeNull();
        }

        [TestMethod]
        public void CanFindAll()
        {
            var path = ModulePath.Create([Path.Combine(RESOURCE_PATH, "jars")]);
            var h = path.FindAll();
            h.Should().ContainSingle(m => m.Descriptor.Name == "m1");
            h.Should().ContainSingle(m => m.Descriptor.Name == "m2");
            h.Should().ContainSingle(m => m.Descriptor.Name == "m3");
            h.Should().ContainSingle(m => m.Descriptor.Name == "m4");
        }

        [TestMethod]
        public void CanFindAutomaticModule()
        {
            var path = ModulePath.Create([Path.Combine(RESOURCE_PATH, "automatic", "jars")]);
            var alib = path.Find("alib");
            alib.Should().NotBeNull();
            alib!.Descriptor.IsAutomatic.Should().BeTrue();
            alib!.Descriptor.Name.Should().Be("alib");
            alib!.Descriptor.Version.IsValid.Should().BeFalse();
            alib!.Descriptor.Packages.Should().Contain("q");
            var m = path.Find("m");
            m.Should().NotBeNull();
            m!.Descriptor.IsAutomatic.Should().BeFalse();
            m!.Descriptor.Name.Should().Be("m");
            m!.Descriptor.Version.IsValid.Should().BeFalse();
            m!.Descriptor.Requires.Should().HaveCountGreaterThanOrEqualTo(1);
            m!.Descriptor.Requires.Should().Contain(r => r.Name == "alib");
        }

    }

}
