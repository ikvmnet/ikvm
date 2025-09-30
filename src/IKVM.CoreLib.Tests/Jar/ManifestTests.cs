using FluentAssertions;

using IKVM.CoreLib.Jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Jar
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

        [TestMethod]
        public void Should_ignore_name_in_main()
        {
            var m = new Manifest(
@"Manifest-Version: 1.0
Name: test
Automatic-Module-Name: test.module
");
            m.MainAttributes.Should().NotContainKey("Name");
        }

        [TestMethod]
        public void Should_parse_named_sections()
        {
            var m = new Manifest(
@"Manifest-Version: 1.0
Automatic-Module-Name: test.module

Name: foo
Value: value1

Name: bar
Value: value2
");
            m.MainAttributes.Should().Contain("Automatic-Module-Name", "test.module");
            m.Attributes.Should().ContainKey("foo");
            m.Attributes["foo"].Should().NotBeNull();
            m.Attributes["foo"].Should().Contain("Value", "value1");
            m.Attributes.Should().ContainKey("bar");
            m.Attributes["bar"].Should().NotBeNull();
            m.Attributes["bar"].Should().Contain("Value", "value2");
        }

    }

}
