using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.DependencyModel;
#endif

namespace IKVM.Tools.Exporter.Tests
{

    [TestClass]
    public class IkvmExporterTests
    {

        [TestMethod]
        public async Task Can_stub_library()
        {
            var options = new IkvmExporterOptions()
            {
                NoStdLib = true,
                References =
                {
                    Path.Combine(Path.GetDirectoryName(typeof(IkvmExporterTests).Assembly.Location), "IKVM.Runtime.dll"),
                    Path.Combine(Path.GetDirectoryName(typeof(IkvmExporterTests).Assembly.Location), "IKVM.Java.dll"),
                }
            };

#if NETFRAMEWORK
            options.Libraries.Add(RuntimeEnvironment.GetRuntimeDirectory());
            options.Assembly = typeof(System.Linq.Enumerable).Assembly.Location;
            options.Output = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(options.Assembly, ".jar")));
#else
            options.References.AddRange(DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()));
            options.Assembly = typeof(System.Linq.Enumerable).Assembly.Location;
            options.Output = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(options.Assembly, ".jar")));
#endif

            var ret = await IkvmExporterTool.ExecuteAsync(options, CancellationToken.None);
            ret.Should().Be(0);
            File.Exists(options.Output).Should().BeTrue();
        }

    }

}