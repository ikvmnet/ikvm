using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Tests.Util;
using IKVM.Tools.Runner;
using IKVM.Tools.Runner.Exporter;

using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Runner.Exporter
{

    [TestClass]
    public class IkvmExporterLauncherTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmExporterLauncherTests).Assembly.Location);

        public TestContext TestContext { get; set; }

        [DataTestMethod]
        [DataRow("net472", "net461", ".NETFramework", "4.6.1")]
        [DataRow("net472", "netcoreapp3.1", ".NETCore", "3.1")]
        [DataRow("net472", "net6.0", ".NETCore", "6.0")]
        [DataRow("net6.0", "net461", ".NETFramework", "4.6.1")]
        [DataRow("net6.0", "netcoreapp3.1", ".NETCore", "3.1")]
        [DataRow("net6.0", "net6.0", ".NETCore", "6.0")]
        public async System.Threading.Tasks.Task CanExportDll(string toolFramework, string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            var ikvmLibs = Path.Combine(TESTBASE, "lib", tfm);
            var refsPath = DotNetSdkUtil.GetPathToReferenceAssemblies(tfm, targetFrameworkIdentifier, targetFrameworkVersion) ;

            var a = Path.Combine(TESTBASE, "helloworld", tfm, "HelloWorldDotNet.dll");
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), Path.ChangeExtension(a, ".jar"));
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
            var l = new IkvmExporterLauncher(Path.Combine(Path.GetDirectoryName(typeof(IkvmExporterLauncherTests).Assembly.Location), "ikvmstub", toolFramework, rid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
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

            var exitCode = await l.ExecuteAsync(o);
            File.Exists(p).Should().BeTrue();
            new FileInfo(p).Length.Should().BeGreaterThanOrEqualTo(64);
            exitCode.Should().Be(0);
        }

    }

}
