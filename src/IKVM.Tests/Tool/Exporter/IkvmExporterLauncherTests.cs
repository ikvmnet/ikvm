using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tool;
using IKVM.Tool.Compiler;
using IKVM.Tool.Exporter;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Tool.Exporter
{

    [TestClass]
    public class IkvmExporterLauncherTests
    {

        public TestContext TestContext { get; set; }

#if NETFRAMEWORK

        [TestMethod]
        public async Task Can_export_netframework_dll()
        {
            // Framework building not supported on ~Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            var a = typeof(List<>).Assembly;
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), a.GetName().Name + ".jar");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmExporterLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmExporterOptions()
            {
                ToolFramework = IkvmToolFramework.NetFramework,
                NoStdLib = true,
                Input = a.Location,
                Output = p,
            };

            o.References.Add(Assembly.Load("mscorlib").Location);
            o.References.Add(Assembly.Load("IKVM.Runtime").Location);
            o.References.Add(Assembly.Load("IKVM.Java").Location);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);
        }

#endif

#if NETCOREAPP

        [TestMethod]
        public async Task Can_export_netcore_jar()
        {
            var a = typeof(List<>).Assembly;
            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), a.GetName().Name + ".jar");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmExporterLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmExporterOptions()
            {
                ToolFramework = IkvmToolFramework.NetCore,
                NoStdLib = true,
                Input = a.Location,
                Output = p,
            };

            o.References.Add(Assembly.Load("netstandard").Location);
            o.References.Add(Assembly.Load("System.Runtime").Location);
            o.References.Add(Assembly.Load("IKVM.Runtime").Location);
            o.References.Add(Assembly.Load("IKVM.Java").Location);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);
        }

#endif

    }

}
