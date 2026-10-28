using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Tests.Util;
using IKVM.Tools.Runner.Diagnostics;
using IKVM.Tools.Runner.Exporter;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Runner.Exporter
{

    [TestClass]
    public class IkvmExporterLauncherTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmExporterLauncherTests).Assembly.Location);

        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataRow("net472", "net472", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net472", "net472", "net481", ".NETFramework", "4.8.1")]
        [DataRow("net472", "net6.0", "net6.0", ".NET", "6.0")]
        [DataRow("net10.0", "net472", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net10.0", "net472", "net481", ".NETFramework", "4.8.1")]
        [DataRow("net10.0", "net6.0", "net6.0", ".NET", "6.0")]
        [DataRow("net10.0", "net6.0", "net7.0", ".NET", "7.0")]
        [DataRow("net10.0", "net6.0", "net8.0", ".NET", "8.0")]
        [DataRow("net10.0", "net8.0", "net8.0", ".NET", "8.0")]
        public async System.Threading.Tasks.Task CanExportDll(string toolFramework, string ikvmFramework, string targetFramework, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            var ikvmLibs = Path.Combine(TESTBASE, "lib", ikvmFramework);
            var refsPath = DotNetSdkUtil.GetPathToReferenceAssemblies(targetFramework, targetFrameworkIdentifier, targetFrameworkVersion);

            var a = Path.Combine(TESTBASE, "helloworld", ikvmFramework, "HelloWorldDotNet.dll");
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), Path.ChangeExtension(a, ".jar"));
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
            var l = new IkvmExporterLauncher(Path.Combine(Path.GetDirectoryName(typeof(IkvmExporterLauncherTests).Assembly.Location), "ikvmstub", toolFramework, toolRid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.Args); }));
            var o = new IkvmExporterOptions()
            {
                NoStdLib = true,
                Input = a,
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
