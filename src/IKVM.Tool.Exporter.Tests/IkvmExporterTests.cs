using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

#if NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.DependencyModel;
#endif

namespace IKVM.Tool.Exporter.Tests.Tests
{

    [TestClass]
    public class IkvmExporterTests
    {

        [TestMethod]
        public async Task Can_stub_corelib()
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

#if NET461
            options.Libraries.Add(RuntimeEnvironment.GetRuntimeDirectory());
            options.Assembly = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "mscorlib.dll");
            options.Output = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(options.Assembly, ".jar")));
#else
            options.References.AddRange(DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()));
            options.Assembly = DependencyContext.Default.CompileLibraries.Where(i => i.Name == "netstandard").SelectMany(i => i.ResolveReferencePaths()).FirstOrDefault();
            options.Output = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(options.Assembly, ".jar")));
#endif

            var ret = await new IkvmExporter(options).ExecuteAsync(CancellationToken.None);
            ret.Should().Be(0);
            File.Exists(options.Output).Should().BeTrue();
        }

    }

}