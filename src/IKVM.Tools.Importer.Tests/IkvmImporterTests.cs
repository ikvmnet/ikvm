using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETCOREAPP
using Microsoft.Extensions.DependencyModel;
#endif

namespace IKVM.Tools.Importer.Tests
{

    [TestClass]
    public class IkvmImporterTests
    {

        [TestMethod]
        public async Task CanImportSimpleTest()
        {
            var s = new StreamReader(typeof(IkvmImporterTests).Assembly.GetManifestResourceStream("IKVM.Tools.Importer.Tests.IkvmImporterTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.tools.importer.tests.IkvmImporterTests", s);
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
            var ret = await IkvmImporterTool.Main(a.Concat(new[] { "-nostdlib", "-assembly:IKVM.Tools.Importer.Tests.Java", $"-out:{asm}", j }).ToArray(), CancellationToken.None);
            ret.Should().Be(0);
            File.Exists(asm).Should().BeTrue();
        }
    }

}