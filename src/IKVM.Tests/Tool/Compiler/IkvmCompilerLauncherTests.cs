using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tool;
using IKVM.Tool.Compiler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Tool.Compiler
{

    [TestClass]
    public class IkvmCompilerLauncherTests
    {

        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task Can_compile_netframework_jar()
        {
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "helloworld-2.0.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmCompilerLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmCompilerOptions()
            {
                TargetFramework = IkvmCompilerTargetFramework.NetFramework,
                ResponseFile = "ikvmc.rsp",
                Input = { "helloworld-2.0.jar" },
                Assembly = "helloworld-2.0",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            foreach (var f in Directory.GetFiles(@"C:\dev\ikvm\src\IKVM.Tests\bin\Debug\net461\ikvm-tools\netcoreapp3.1\win7-x64\refs"))
                o.References.Add(f);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);
        }

        [TestMethod]
        public async Task Can_compile_netcore_jar()
        {
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "helloworld-2.0.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmCompilerLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmCompilerOptions()
            {
                TargetFramework = IkvmCompilerTargetFramework.NetCore,
                ResponseFile = "ikvmc.rsp",
                Input = { "helloworld-2.0.jar" },
                Assembly = "helloworld-2.0",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            foreach (var f in Directory.GetFiles(@"C:\dev\ikvm\src\IKVM.Tests\bin\Debug\net461\ikvm-tools\netcoreapp3.1\win7-x64\refs"))
                o.References.Add(f);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);
        }

    }

}
