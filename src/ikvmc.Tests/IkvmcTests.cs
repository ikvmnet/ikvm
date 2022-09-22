using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.DependencyModel;
#endif

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

#if NETFRAMEWORK
            var a = new[] { $"-lib:{RuntimeEnvironment.GetRuntimeDirectory()}" };
#else
            var a = DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()).Select(i => $"-r:{i}");
#endif

            var asm = Path.ChangeExtension(j, ".dll");
            var ret = ikvmc.IkvmcCompiler.Main(a.Concat(new[] { "-nostdlib", "-assembly:ikvmc.Tests.Java", $"-out:{asm}", j }).ToArray());
            ret.Should().Be(0);
            File.Exists(asm).Should().BeTrue();
        }

    }

}