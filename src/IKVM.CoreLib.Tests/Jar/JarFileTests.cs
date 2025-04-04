using System.IO;

using FluentAssertions;

using IKVM.CoreLib.Jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Jar
{

    [TestClass]
    public class JarFileTests
    {

        [TestMethod]
        public void CanReadManifestVersion()
        {
            var z = new JarFile(Path.Combine("res", "helloworld", "helloworld-2.0.jar"));
            z.Manifest.MainAttributes.Should().Contain("Manifest-Version", "1.0");
        }

    }

}
