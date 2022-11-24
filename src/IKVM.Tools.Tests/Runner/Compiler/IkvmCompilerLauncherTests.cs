using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tools.Runner.Compiler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Runner.Test.Compiler
{

    [TestClass]
    public class IkvmCompilerLauncherTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmCompilerLauncherTests).Assembly.Location);

        public TestContext TestContext { get; set; }

        async Task CompileJar(string tfm)
        {
            var libs = Path.Combine(TESTBASE, "lib", tfm);

            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "ext", tfm, "helloworld-2.0.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var rid = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                rid = "win7-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                rid = "linux-x64";

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmCompilerLauncher(Path.Combine(Path.GetDirectoryName(typeof(IkvmCompilerLauncherTests).Assembly.Location), "ikvmc", tfm, rid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmCompilerOptions()
            {
                Runtime = Path.Combine(TESTBASE, "lib", tfm, "IKVM.Runtime.dll"),
                ResponseFile = $"CompileJar_{tfm}_ikvmc.rsp",
                Input = { Path.Combine(TESTBASE, "ext", "helloworld-2.0.jar") },
                Assembly = "helloworld-2.0",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            foreach (var dll in Directory.GetFiles(l.GetReferenceAssemblyDirectory()))
                o.References.Add(dll);
            foreach (var dll in Directory.GetFiles(libs, "*.dll"))
                o.References.Add(dll);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);
        }

        [TestMethod]
        public Task Can_compile_netframework_jar()
        {
            // Framework building not supported on ~Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return Task.CompletedTask;

            return CompileJar("net461");
        }

        [TestMethod]
        public Task Can_compile_netcore_jar()
        {
            return CompileJar("netcoreapp3.1");
        }

    }

}
