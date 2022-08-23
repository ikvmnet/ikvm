using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tool.Exporter;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tool.Tests.Exporter
{

    [TestClass]
    public class IkvmExporterLauncherTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmExporterLauncherTests).Assembly.Location);

        public TestContext TestContext { get; set; }

        public async Task Can_export_dll(IkvmToolFramework toolFramework, string tfm)
        {
            var libs = Path.Combine(TESTBASE, "lib", tfm);

            var a = Path.Combine(TESTBASE, "ext", tfm, "HelloWorldDotNet.dll");
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), Path.ChangeExtension(a, ".jar"));
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmExporterLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmExporterOptions()
            {
                ToolFramework = IkvmToolFramework.NetFramework,
                NoStdLib = true,
                Input = a,
                Output = p,
            };

            foreach (var dll in Directory.GetFiles(l.GetReferenceAssemblyDirectory(toolFramework)))
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

            return Can_export_dll(IkvmToolFramework.NetFramework, "net461");
        }

        [TestMethod]
        public Task Can_export_netcore_jar()
        {
            return Can_export_dll(IkvmToolFramework.NetCore, "netcoreapp3.1");
        }

    }

}
