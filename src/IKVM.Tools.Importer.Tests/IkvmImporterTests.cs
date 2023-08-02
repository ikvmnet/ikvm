using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETCOREAPP
using Microsoft.Extensions.DependencyModel;
#endif

namespace IKVM.Tools.Importer.Tests
{

    [TestClass]
    public class IkvmImporterTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmImporterTests).Assembly.Location);

        [DataTestMethod]
        [DataRow("net461", "net461", ".NETFramework", "4.6.1")]
        [DataRow("net461", "net462", ".NETFramework", "4.6.2")]
        [DataRow("net461", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net461", "net481", ".NETFramework", "4.8.1")]
        [DataRow("netcoreapp3.1", "netcoreapp3.1", ".NETCore", "3.1")]
        [DataRow("net6.0", "net6.0", ".NETCore", "6.0")]
        [DataRow("net6.0", "net7.0", ".NETCore", "7.0")]
        public async Task CanImportSimpleTest(string ikvmFrameworkMoniker, string targetFrameworkMoniker, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            var s = new StreamReader(typeof(IkvmImporterTests).Assembly.GetManifestResourceStream("IKVM.Tools.Importer.Tests.IkvmImporterTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.tools.importer.tests.IkvmImporterTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
            var j = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".jar");
            c.WriteJar(j);

            var ikvmLibs = Path.Combine(TESTBASE, "lib", ikvmFrameworkMoniker);
            var libPaths = DotNetSdkUtil.GetPathToReferenceAssemblies(targetFrameworkMoniker, targetFrameworkIdentifier, targetFrameworkVersion);

            // add references to libraries
            var asm = Path.ChangeExtension(j, ".dll");
            var args = new List<string>();
            foreach (var i in libPaths)
                args.Add($"-lib:{i}");
            foreach (var dll in Directory.GetFiles(ikvmLibs, "*.dll"))
                args.Add($"-reference:{dll}");

            // add additional command options
            args.Add($"-runtime:{Path.Combine(ikvmLibs, "IKVM.Runtime.dll")}");
            args.Add("-nostdlib");
            args.Add("-assembly:IKVM.Tools.Importer.Tests.Java");
            args.Add($"-out:{asm}");
            args.Add(j);

            // initiate the import
            var ret = await IkvmImporterTool.Main(args.ToArray(), CancellationToken.None);
            ret.Should().Be(0);
            File.Exists(asm).Should().BeTrue();
            new FileInfo(asm).Length.Should().BeGreaterThanOrEqualTo(128);
        }
    }

}