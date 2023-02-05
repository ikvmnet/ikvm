using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

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
        public async Task CanImportSampleJar()
        {

#if NETFRAMEWORK
            var a = new[] { $"-lib:{RuntimeEnvironment.GetRuntimeDirectory()}" };
#else
            var a = DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()).Select(i => $"-r:{i}");
#endif

            var o = Path.Combine(Path.GetTempPath(), "helloworld-2.0.dll");
            var j = Path.Combine(Path.GetDirectoryName(typeof(IkvmImporterTests).Assembly.Location), "helloworld-2.0.jar");
            var ret = await IkvmImporterTool.Main(a.Concat(new[] { @"-runtime:C:\dev\ikvm\src\IKVM.Runtime\bin\Debug\netcoreapp3.1\IKVM.Runtime.dll", "-nostdlib", "-assembly:IKVM.Tools.Importer.Tests.Java", $"-out:{o}", j }).ToArray(), CancellationToken.None);
            ret.Should().Be(0);
        }

        [TestMethod]
        public async Task Test()
        {
            await IkvmImporterTool.Main(new[] { @"@C:\Users\jhaltom\AppData\Local\Temp\ikvm\cache\1\54064d56c6a67d58f7bcade6ccdf39f4\com.google.common.dll.rsp" }, CancellationToken.None);
        }

    }

}