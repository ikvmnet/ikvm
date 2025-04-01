using System.IO;

using FluentAssertions;

using IKVM.CoreLib.Jar;
using IKVM.Util.Jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Util.Jar
{

    [TestClass]
    public class JarFileExtensionsTests
    {

        [TestMethod]
        public void CanReadModuleName()
        {
            var z = new JarFile(Path.Combine("helloworld", "helloworld-mod.jar"));
            z.GetModuleInfo().Name.Should().Be("helloworld");
        }

    }

}
