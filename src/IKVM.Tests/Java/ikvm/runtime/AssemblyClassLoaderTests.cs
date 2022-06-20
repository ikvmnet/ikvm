using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tool;
using IKVM.Tool.Compiler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.ikvm.runtime
{

    [TestClass]
    public class AssemblyClassLoaderTests
    {

        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task Can_get_package()
        {
#if NETCOREAPP3_1_OR_GREATER
            var tfm = IkvmCompilerTargetFramework.NetCore;
            var dir = "netcoreapp3.1";
#else
            var tfm = IkvmCompilerTargetFramework.NetFramework;
            var dir = "net461";
#endif

            var p = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "helloworld-2.0.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmCompilerLauncher(new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var o = new IkvmCompilerOptions()
            {
                TargetFramework = tfm,
                Runtime = Path.Combine("lib", dir, "IKVM.Runtime.dll"),
                ResponseFile = "ikvmc.rsp",
                Input = { "helloworld-2.0.jar" },
                Assembly = "helloworld-2.0",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
            };

            o.References.Add(Path.Combine("lib", dir, "IKVM.Java.dll"));
            foreach (var f in Directory.GetFiles(l.GetReferenceAssemblyDirectory(o.TargetFramework)))
                o.References.Add(f);

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);

            var asm = Assembly.LoadFile(p);
            var t = asm.GetType("sample.HelloworldImpl");
            var k = ((global::java.lang.Class)t).getPackage();
        }

    }

}
