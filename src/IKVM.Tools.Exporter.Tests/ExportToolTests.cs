using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.CoreLib.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.DependencyModel;
#endif

namespace IKVM.Tools.Exporter.Tests
{

    [TestClass]
    public class ExportToolTests
    {

        [TestMethod]
        public async Task CanStubCoreLibrary()
        {
            var options = new ExportOptions()
            {
                NoStdLib = true,
                References =
                {
                    Path.Combine(Path.GetDirectoryName(typeof(ExportToolTests).Assembly.Location), "IKVM.Runtime.dll"),
                    Path.Combine(Path.GetDirectoryName(typeof(ExportToolTests).Assembly.Location), "IKVM.Java.dll"),
                }
            };

#if NETFRAMEWORK
            options.Libraries.Add(RuntimeEnvironment.GetRuntimeDirectory());
            options.Assembly = typeof(object).Assembly.Location;
            options.Output = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(options.Assembly, ".jar")));
#else
            options.References.AddRange(DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()));
            options.Assembly = options.References.First(i => Path.GetFileNameWithoutExtension(i) == "System.Runtime");
            options.Output = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(options.Assembly, ".jar")));
#endif

            var ret = await ExportTool.ExecuteAsync(options, new NullDiagnosticHandler(), CancellationToken.None);
            ret.Should().Be(0);
            File.Exists(options.Output).Should().BeTrue();
        }

    }

}
