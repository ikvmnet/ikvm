using System;
using System.IO;

using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ikvmc.Tests
{

    [TestClass]
    public class IkvmcTests
    {

        [TestMethod]
        public void Should_convert_simple_jar()
        {
            var s = new StreamReader(typeof(IkvmcTests).Assembly.GetManifestResourceStream("ikvmc.Tests.IkvmcTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvmc.tests.IkvmcTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
            var j = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".jar");
            c.WriteJar(j);

            var asm = Path.ChangeExtension(j, ".dll");
            var ret = ikvmc.IkvmcCompiler.Main(new[] { "-assembly:ikvmc.Tests.Java", $"-out:{asm}", j });
            ret.Should().Be(0);
            File.Exists(asm).Should().BeTrue();
        }

    }
}
