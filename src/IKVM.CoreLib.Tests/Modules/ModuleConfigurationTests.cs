using System.IO;

using FluentAssertions;

using IKVM.CoreLib.Modules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Modules
{

    [TestClass]
    public class ModuleConfigurationTests
    {

        static readonly string RESOURCE_PATH = Path.Combine(Path.GetDirectoryName(typeof(ModulePathTests).Assembly.Location!)!, "res", "Modules");

        [TestMethod]
        public void CanResolveBasicConfiguration()
        {
            var mockBaseFinder = ModuleTestUtil.FinderOf([
                ModuleDescriptor.CreateModule("java.base").Build(),
            ]);

            var cf = ModuleConfiguration.Resolve(mockBaseFinder, [], ModulePath.Create([
                Path.Combine(RESOURCE_PATH, "jars")
            ]), ["m1"]);

            cf.FindModule("m1").Should().NotBeNull();
            cf.FindModule("m2").Should().NotBeNull();
            cf.FindModule("m3").Should().NotBeNull();
            cf.FindModule("m4").Should().BeNull();
        }

    }

}
