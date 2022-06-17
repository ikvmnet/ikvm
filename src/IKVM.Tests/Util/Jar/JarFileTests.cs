using FluentAssertions;

using IKVM.Util.Jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Util.Jar
{

    [TestClass]
    public class JarFileTests
    {

        [TestMethod]
        public void Can_read_manifest_version()
        {
            var z = new JarFile(@"helloworld-2.0.jar");
            z.Manifest.MainAttributes.Should().Contain("Manifest-Version", "1.0");
        }

    }

}
