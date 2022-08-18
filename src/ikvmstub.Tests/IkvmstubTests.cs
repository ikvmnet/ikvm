using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using sun.tools.jar.resources;

#if NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.DependencyModel;
#endif

namespace ikvmstub.Tests
{

    [TestClass]
    public class IkvmstubTests
    {

        [TestMethod]
        public void Foo()
        {
            var ret = Program.Main(new[]
            {
                @"-out:ICSharpCode.SharpZipLib.jar",
                @"-reference:C:\Users\jhaltom\AppData\Local\Temp\IKVM.NET.Sdk.Tests\nuget\packages\ikvm\8.0.0-dev\lib\netcoreapp3.1\IKVM.Java.dll",
                @"-reference:C:\Users\jhaltom\AppData\Local\Temp\IKVM.NET.Sdk.Tests\nuget\packages\ikvm\8.0.0-dev\lib\netcoreapp3.1\IKVM.Reflection.dll",
                @"-reference:C:\Users\jhaltom\AppData\Local\Temp\IKVM.NET.Sdk.Tests\nuget\packages\ikvm\8.0.0-dev\lib\netcoreapp3.1\IKVM.Runtime.dll",
                @"-reference:C:\Users\jhaltom\AppData\Local\Temp\IKVM.NET.Sdk.Tests\nuget\packages\ikvm\8.0.0-dev\lib\netcoreapp3.1\IKVM.Runtime.JNI.dll",
                @"-reference:C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\mscorlib.dll",
                @"-reference:C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\netstandard.dll",
                @"-reference:C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Core.dll",
                @"-reference:C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.dll",
                @"-reference:C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.dll",
                @"-nostdlib",
                @"C:\Users\jhaltom\.nuget\packages\sharpziplib\1.3.3\lib\netstandard2.1\ICSharpCode.SharpZipLib.dll"
            });
        }

        [TestMethod]
        public void Can_stub_system_runtime()
        {
#if NET461
            var a = new[] { $"-lib:{RuntimeEnvironment.GetRuntimeDirectory()}" };
            var j = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "mscorlib.dll");
#else
            var a = DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()).Select(i => $"-r:{i}");
            var j = DependencyContext.Default.CompileLibraries.Where(i => i.Name == "netstandard").SelectMany(i => i.ResolveReferencePaths()).FirstOrDefault();
#endif

            var jar = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.ChangeExtension(j, ".jar")));
            var ret = Program.Main(a.Concat(new[] { "-nostdlib", $"-r:{Path.Combine(Path.GetDirectoryName(typeof(IkvmstubTests).Assembly.Location), "IKVM.Runtime.dll")}", $"-out:{jar}", j }).ToArray());
            ret.Should().Be(0);
            File.Exists(jar).Should().BeTrue();
        }

    }

}