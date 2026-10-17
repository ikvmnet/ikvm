using System;
using System.IO;
using System.IO.Compression;
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

        [TestMethod]
        public async Task ShouldHideByRefLikeMembers()
        {
            var assemblyPath = typeof(ExportToolTests).Assembly.Location;
            var baseDir = Path.GetDirectoryName(assemblyPath);

            var options = new ExportOptions()
            {
                NoStdLib = true,
                Assembly = assemblyPath,
                Output = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".jar"),
                Namespaces = { typeof(Fixtures.ByRefLikeMembers).Namespace },
                References =
                {
                    Path.Combine(baseDir, "IKVM.Runtime.dll"),
                    Path.Combine(baseDir, "IKVM.Java.dll"),
                },
                Libraries =
                {
                    baseDir,
                }
            };

#if NETFRAMEWORK
            options.References.Add(typeof(object).Assembly.Location);
            options.Libraries.Add(RuntimeEnvironment.GetRuntimeDirectory());
#else
            options.References.AddRange(DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()).Where(i => Path.GetFileName(i) != Path.GetFileName(assemblyPath)));
#endif

            try
            {
                var ret = await ExportTool.ExecuteAsync(options, new NullDiagnosticHandler(), CancellationToken.None);
                ret.Should().Be(0);

                using var zip = ZipFile.OpenRead(options.Output);
                var entry = zip.GetEntry("cli/" + typeof(Fixtures.ByRefLikeMembers).FullName.Replace('.', '/') + ".class");
                entry.Should().NotBeNull();

                using var stream = entry.Open();
                using var buffer = new MemoryStream();
                stream.CopyTo(buffer);
                var text = System.Text.Encoding.ASCII.GetString(buffer.ToArray());

                // method names appear in the constant pool only if the method was exported
                text.Should().Contain(nameof(Fixtures.ByRefLikeMembers.Supported));
                text.Should().NotContain(nameof(Fixtures.ByRefLikeMembers.InReadOnlySpan));
                text.Should().NotContain(nameof(Fixtures.ByRefLikeMembers.ReadOnlySpanValue));
                text.Should().NotContain(nameof(Fixtures.ByRefLikeMembers.ReturnsReadOnlySpan));
            }
            finally
            {
                if (File.Exists(options.Output))
                    File.Delete(options.Output);
            }
        }

    }

}

namespace IKVM.Tools.Exporter.Tests.Fixtures
{

    public class ByRefLikeMembers
    {

        public void InReadOnlySpan(in ReadOnlySpan<char> value)
        {

        }

        public void ReadOnlySpanValue(ReadOnlySpan<char> value)
        {

        }

        public ReadOnlySpan<char> ReturnsReadOnlySpan()
        {
            return default;
        }

        public void Supported(string value)
        {

        }

    }

}
