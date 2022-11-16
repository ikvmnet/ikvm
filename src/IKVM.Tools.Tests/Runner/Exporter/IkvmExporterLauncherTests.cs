using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tools.Runner;
using IKVM.Tools.Runner.Exporter;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Runner.Exporter
{

    [TestClass]
    public class IkvmExporterLauncherTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmExporterLauncherTests).Assembly.Location);

        public TestContext TestContext { get; set; }

        async Task Can_export_dll(string tfm)
        {
            var libs = Path.Combine(TESTBASE, "lib", tfm);

            var a = Path.Combine(TESTBASE, "ext", tfm, "HelloWorldDotNet.dll");
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), Path.ChangeExtension(a, ".jar"));
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var rid = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                rid = "win7-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                rid = "linux-x64";

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmExporterLauncher(Path.Combine(Path.GetDirectoryName(typeof(IkvmExporterLauncherTests).Assembly.Location), "ikvmstub", tfm, rid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmExporterOptions()
            {
                NoStdLib = true,
                Input = a,
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
        public Task Can_export_netframework_dll()
        {
            // Framework not supported on ~Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return Task.CompletedTask;

            return Can_export_dll("net461");
        }

        [TestMethod]
        public Task Can_export_netcore_jar()
        {
            return Can_export_dll("netcoreapp3.1");
        }

    }

}
