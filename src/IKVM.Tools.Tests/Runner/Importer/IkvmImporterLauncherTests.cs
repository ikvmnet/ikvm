using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Tests.Util;
using IKVM.Tools.Runner;
using IKVM.Tools.Runner.Importer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Runner.Importer
{

    [TestClass]
    public class IkvmImporterLauncherTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmImporterLauncherTests).Assembly.Location);

        public TestContext TestContext { get; set; }

        [DataTestMethod]
        [DataRow("net472", "net461", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net472", "net461", "net481", ".NETFramework", "4.8.1")]
        [DataRow("net472", "netcoreapp3.1", "netcoreapp3.1", ".NETCore", "3.1")]
        [DataRow("net472", "net6.0", "net6.0", ".NETCore", "6.0")]
        [DataRow("net6.0", "net461", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net6.0", "net461", "net481", ".NETFramework", "4.8.1")]
        [DataRow("net6.0", "netcoreapp3.1", "netcoreapp3.1", ".NETCore", "3.1")]
        [DataRow("net6.0", "net6.0", "net6.0", ".NETCore", "6.0")]
        [DataRow("net6.0", "net6.0", "net7.0", ".NETCore", "7.0")]
        public async System.Threading.Tasks.Task CanImportJar(string toolFramework, string ikvmFramework, string targetFrameworkMoniker, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            if (toolFramework == "net472" && RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;
            if (targetFrameworkMoniker == "netcoreapp3.1" && RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;
            if (targetFrameworkIdentifier == ".NETFramework" && RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            var ikvmLibs = Path.Combine(TESTBASE, "lib", ikvmFramework);
            var refsPath = DotNetSdkUtil.GetPathToReferenceAssemblies(targetFrameworkMoniker, targetFrameworkIdentifier, targetFrameworkVersion);

            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "helloworld", "helloworld-2.0.dll");
            var d = Path.GetDirectoryName(p);
            Directory.CreateDirectory(d);

            var rid = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                rid = "win7-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                rid = "linux-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                rid = "osx-x64";

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmImporterLauncher(Path.Combine(Path.GetDirectoryName(typeof(IkvmImporterLauncherTests).Assembly.Location), "ikvmc", toolFramework, rid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmImporterOptions()
            {
                Runtime = Path.Combine(ikvmLibs, "IKVM.Runtime.dll"),
                ResponseFile = Path.Combine(d, "ikvmc.rsp"),
                Input = { Path.Combine(TESTBASE, "helloworld", "helloworld-2.0.jar") },
                Assembly = "helloworld-2.0",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            foreach (var dll in Directory.GetFiles(ikvmLibs, "*.dll"))
                o.References.Add(dll);
            foreach (var dir in refsPath)
                foreach (var dll in Directory.GetFiles(dir, "*.dll"))
                    if (DotNetSdkUtil.IsAssembly(dll))
                        o.References.Add(dll);

            var exitCode = await l.ExecuteAsync(o);
            File.Exists(p).Should().BeTrue();
            new FileInfo(p).Length.Should().BeGreaterThanOrEqualTo(64);
            exitCode.Should().Be(0);
        }

    }

}
