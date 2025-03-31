﻿using System.IO;

using FluentAssertions;

using IKVM.CoreLib.Jar;
using IKVM.Util.Jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Util.Jar
{

    [TestClass]
    public class JarFileTests
    {

        [TestMethod]
        public void CanReadManifestVersion()
        {
            var z = new JarFile(Path.Combine("helloworld", "helloworld-2.0.jar"));
            z.Manifest.MainAttributes.Should().Contain("Manifest-Version", "1.0");
        }

        [TestMethod]
        public void CanReadModuleName()
        {
            var z = new JarFile(Path.Combine("helloworld", "helloworld-mod.jar"));
            z.GetModuleInfo().Name.Should().Be("helloworld");
        }

    }

}
