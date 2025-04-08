using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.CoreLib.Jar;
using IKVM.CoreLib.Modules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Modules
{

    [TestClass]
    public class JarModuleReaderTests
    {

        static readonly string RESOURCE_PATH = Path.Combine(Path.GetDirectoryName(typeof(ModulePathTests).Assembly.Location!)!, "res", "Modules");

        [TestMethod]
        public void CanListItems()
        {
            using var m1 = new JarModuleReader(Path.Combine(RESOURCE_PATH, "jars", "m1.jar"), JarFile.RUNTIME_VERSION);
            var m1l = m1.List().ToList();
            m1l.Should().Contain([
                "META-INF/MANIFEST.MF",
                "module-info.class",
                "p/Main.class",
                "p/Service.class"
            ]);

            using var m2 = new JarModuleReader(Path.Combine(RESOURCE_PATH, "jars", "m2.jar"), JarFile.RUNTIME_VERSION);
            var m2l = m2.List().ToList();
            m2l.Should().Contain([
                "META-INF/MANIFEST.MF",
                "module-info.class",
                "q/Hello.class"
            ]);

            using var m3 = new JarModuleReader(Path.Combine(RESOURCE_PATH, "jars", "m3.jar"), JarFile.RUNTIME_VERSION);
            var m3l = m3.List().ToList();
            m3l.Should().Contain([
                "META-INF/MANIFEST.MF",
                "module-info.class",
                "w/Hello.class"
            ]);

            using var m4 = new JarModuleReader(Path.Combine(RESOURCE_PATH, "jars", "m4.jar"), JarFile.RUNTIME_VERSION);
            var m4l = m4.List().ToList();
            m4l.Should().Contain([
                "META-INF/MANIFEST.MF",
                "module-info.class",
                "impl/ServiceImpl.class"
            ]);
        }

        [TestMethod]
        public void CanOpenItem()
        {
            using var reader = new JarModuleReader(Path.Combine(RESOURCE_PATH, "jars", "m1.jar"), JarFile.RUNTIME_VERSION);
            using var stream = reader.Open("p/Main.class");
            stream.Should().NotBeNull();
        }

    }

}
