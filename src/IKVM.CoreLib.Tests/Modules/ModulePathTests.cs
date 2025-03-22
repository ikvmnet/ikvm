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
        public void CanLoadExplodedModules()
        {
            var path = ModulePath.Create(RESOURCE_PATH);
            path.Find("m1").Should().NotBeNull();
            path.Find("m2").Should().NotBeNull();
            path.Find("m3").Should().NotBeNull();
            path.Find("m4").Should().NotBeNull();
        }

        [TestMethod]
        public void CanLoadJarModules()
        {
            var path = ModulePath.Create(Path.Combine(RESOURCE_PATH, "jars"));
            path.Find("m1").Should().NotBeNull();
            path.Find("m2").Should().NotBeNull();
            path.Find("m3").Should().NotBeNull();
            path.Find("m4").Should().NotBeNull();
        }

        [TestMethod]
        public void CanFindAll()
        {
            var path = ModulePath.Create(Path.Combine(RESOURCE_PATH, "jars"));
            var h = path.FindAll();
        }

    }

}
