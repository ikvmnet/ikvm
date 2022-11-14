using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tools.Runner;
using IKVM.Tools.Runner.Compiler;

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
#if NETCOREAPP3_1_OR_GREATER
            var tfm = IkvmToolFramework.NetCore;
            var dir = "netcoreapp3.1";
#else
            var tfm = IkvmToolFramework.NetFramework;
            var dir = "net461";
#endif

            var n = Guid.NewGuid().ToString("n");
            var p = Path.Combine(Path.GetTempPath(), n, $"helloworld_{n}.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmCompilerLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmCompilerOptions()
            {
                ToolFramework = tfm,
                Runtime = Path.Combine("lib", dir, "IKVM.Runtime.dll"),
                ResponseFile = $"{n}_ikvmc.rsp",
                Input = { "helloworld-2.0.jar" },
                Assembly = $"helloworld_{n}",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            o.References.Add(Path.Combine("lib", dir, "IKVM.Java.dll"));
            o.References.Add(Path.Combine("lib", dir, "IKVM.Runtime.dll"));
            o.References.Add(Path.Combine("lib", dir, "IKVM.Runtime.JNI.dll"));
            foreach (var f in Directory.GetFiles(l.GetReferenceAssemblyDirectory(o.ToolFramework)))
                o.References.Add(f);

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
