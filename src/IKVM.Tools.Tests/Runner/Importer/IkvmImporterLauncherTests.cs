using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Tests.Util;
using IKVM.Tools.Runner.Diagnostics;
using IKVM.Tools.Runner.Importer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Runner.Importer
{

    [TestClass]
    public class IkvmImporterLauncherTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmImporterLauncherTests).Assembly.Location);

        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataRow("net472", "net472", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net472", "net472", "net481", ".NETFramework", "4.8.1")]
        [DataRow("net472", "net6.0", "net6.0", ".NET", "6.0")]
        [DataRow("net472", "net8.0", "net8.0", ".NET", "8.0")]
        [DataRow("net472", "net8.0", "net10.0", ".NET", "10.0")]
        [DataRow("net10.0", "net472", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net10.0", "net472", "net481", ".NETFramework", "4.8.1")]
        [DataRow("net10.0", "net6.0", "net6.0", ".NET", "6.0")]
        [DataRow("net10.0", "net6.0", "net7.0", ".NET", "7.0")]
        [DataRow("net10.0", "net6.0", "net8.0", ".NET", "8.0")]
        [DataRow("net10.0", "net6.0", "net10.0", ".NET", "10.0")]
        [DataRow("net10.0", "net8.0", "net8.0", ".NET", "8.0")]
        [DataRow("net10.0", "net8.0", "net10.0", ".NET", "10.0")]
        public async System.Threading.Tasks.Task CanImportJar(string toolFramework, string ikvmFramework, string targetFrameworkMoniker, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            var ikvmLibs = Path.Combine(TESTBASE, "lib", ikvmFramework);
            var refsPath = DotNetSdkUtil.GetPathToReferenceAssemblies(targetFrameworkMoniker, targetFrameworkIdentifier, targetFrameworkVersion);

            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "helloworld", "helloworld-2.0.dll");
            var d = Path.GetDirectoryName(p);
            Directory.CreateDirectory(d);

            var toolRid = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                toolRid = "win-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                toolRid = "win-arm64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                toolRid = "linux-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                toolRid = "linux-arm";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                toolRid = "linux-arm64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                toolRid = "osx-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                toolRid = "osx-arm64";
            if (string.IsNullOrEmpty(toolRid))
                return;

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmImporterLauncher(Path.Combine(Path.GetDirectoryName(typeof(IkvmImporterLauncherTests).Assembly.Location), "ikvmc", toolFramework, toolRid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.Args); }));
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

            if (File.Exists(p))
                File.Delete(p);

            var exitCode = await l.ExecuteAsync(o);
            e.Count(i => i.Level >= IkvmToolDiagnosticEventLevel.Error).Should().Be(0);
            File.Exists(p).Should().BeTrue();
            new FileInfo(p).Length.Should().BeGreaterThanOrEqualTo(64);
            exitCode.Should().Be(0);
        }

    }

}
