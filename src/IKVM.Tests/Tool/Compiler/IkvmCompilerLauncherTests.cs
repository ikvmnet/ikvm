using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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
            // Framework building not supported on ~Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "helloworld-2.0.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmCompilerLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmCompilerOptions()
            {
                TargetFramework = IkvmCompilerTargetFramework.NetFramework,
                Runtime = Path.Combine("lib", "net461", "IKVM.Runtime.dll"),
                ResponseFile = "helloworld_netframework_ikvmc.rsp",
                Input = { "helloworld-2.0.jar" },
                Assembly = "helloworld-2.0",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            o.References.Add(Path.Combine("lib", "net461", "IKVM.Java.dll"));
            o.References.Add(Path.Combine("lib", "net461", "IKVM.Runtime.dll"));
            o.References.Add(Path.Combine("lib", "net461", "IKVM.Runtime.JNI.dll"));
            foreach (var f in Directory.GetFiles(l.GetReferenceAssemblyDirectory(o.TargetFramework)))
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
                Runtime = Path.Combine("lib", "netcoreapp3.1", "IKVM.Runtime.dll"),
                ResponseFile = "helloworld_netcore_ikvmc.rsp",
                Input = { "helloworld-2.0.jar" },
                Assembly = "helloworld-2.0",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            o.References.Add(Path.Combine("lib", "netcoreapp3.1", "IKVM.Java.dll"));
            o.References.Add(Path.Combine("lib", "netcoreapp3.1", "IKVM.Runtime.dll"));
            o.References.Add(Path.Combine("lib", "netcoreapp3.1", "IKVM.Runtime.JNI.dll"));
            foreach (var f in Directory.GetFiles(l.GetReferenceAssemblyDirectory(o.TargetFramework)))
                o.References.Add(f);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);
        }

    }

}
