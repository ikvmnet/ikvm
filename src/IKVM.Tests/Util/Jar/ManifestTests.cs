using FluentAssertions;

using IKVM.Util.Jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Util.Jar
{

    [TestClass]
    public class ManifestTests
    {

        [TestMethod]
        public void Should_parse_manifest_version()
        {
            var m = new Manifest(
@"Manifest-Version: 1.0
");
            m.MainAttributes.Should().Contain("Manifest-Version", "1.0");
        }

        [TestMethod]
        public void Should_parse_automatic_module_name()
        {
            var m = new Manifest(
@"Manifest-Version: 1.0
Automatic-Module-Name: test.module
");
            m.MainAttributes.Should().Contain("Automatic-Module-Name", "test.module");
        }

    }

}
