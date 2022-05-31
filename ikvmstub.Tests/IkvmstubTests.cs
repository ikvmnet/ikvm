using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.DependencyModel;
#endif

namespace ikvmstub.Tests
{

    [TestClass]
    public class IkvmstubTests
    {

        [TestMethod]
        public void Can_stub_system_runtime()
        {
#if NET461
            var a = new[] { $"-lib:{RuntimeEnvironment.GetRuntimeDirectory()}" };
#else
            var a = DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()).Select(i => $"-r:{i}");
#endif

            var j = typeof(object).Assembly.Location;
            var jar = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(j, ".jar")));
            var ret = Program.Main(a.Concat(new[] { "-bootstrap", $"-r:{Path.Combine(Path.GetDirectoryName(typeof(IkvmstubTests).Assembly.Location), "IKVM.Runtime.dll")}", $"-out:{jar}", j }).ToArray());
            ret.Should().Be(0);
            File.Exists(jar).Should().BeTrue();
        }

    }

}
