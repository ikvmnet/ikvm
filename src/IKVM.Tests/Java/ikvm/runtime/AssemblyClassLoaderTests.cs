using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tools.Runner;
using IKVM.Tools.Runner.Importer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.ikvm.runtime
{

    [TestClass]
    public class AssemblyClassLoaderTests
    {

        Assembly helloworldDll;

        public TestContext TestContext { get; set; }

        /// <summary>
        /// Initializes the test DLL.
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public async Task Setup()
        {
#if NETCOREAPP
            var toolFramework = "net6.0";
#else
            var toolFramework = "net472";
#endif

            var refsDir = Path.Combine(Path.GetDirectoryName(typeof(AssemblyClassLoaderTests).Assembly.Location), "refs");
            var n = Guid.NewGuid().ToString("n");
            var p = Path.Combine(Path.GetTempPath(), n, $"helloworld_{n}.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var rid = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                rid = "win7-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                rid = "linux-x64";

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmImporterLauncher(Path.Combine(Path.GetDirectoryName(typeof(AssemblyClassLoaderTests).Assembly.Location), "ikvmc", toolFramework, rid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmImporterOptions()
            {
                Runtime = Path.Combine(refsDir, "IKVM.Runtime.dll"),
                ResponseFile = $"{n}_ikvmc.rsp",
                Input = { Path.Combine("helloworld", "helloworld-2.0.jar") },
                Assembly = $"helloworld_{n}",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            foreach (var dll in Directory.GetFiles(refsDir, "*.dll"))
                o.References.Add(dll);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);

            helloworldDll = Assembly.LoadFrom(p);
        }

        [TestMethod]
        public void Can_get_package()
        {
            var t = helloworldDll.GetType("sample.HelloworldImpl");
            var k = ((global::java.lang.Class)t).getPackage();
            k.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_get_resource()
        {
            var t = helloworldDll.GetType("sample.HelloworldImpl");
            var k = ((global::java.lang.Class)t).getResource("/helloworld.composite");
            var s = ((global::java.lang.Class)t).getResourceAsStream("/helloworld.composite");
            k.Should().NotBeNull();
            s.Should().NotBeNull();
        }

    }

}
