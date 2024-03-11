﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Tests.Util;
using IKVM.Tools.Runner;
using IKVM.Tools.Runner.Importer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.ikvm.runtime
{

    [TestClass]
    public class AssemblyClassLoaderTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(AssemblyClassLoaderTests).Assembly.Location);

        Assembly helloworldDll;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public async Task TestInitialize()
        {
#if NET7_0
            var ikvmTool = "net6.0";
            var ikvmLibs = Path.Combine(TESTBASE, "lib", "net6.0");
            var libPaths = DotNetSdkUtil.GetPathToReferenceAssemblies("net7.0", ".NETCore", "7.0");
#endif
#if NET6_0       
            var ikvmTool = "net6.0";
            var ikvmLibs = Path.Combine(TESTBASE, "lib", "net6.0");
            var libPaths = DotNetSdkUtil.GetPathToReferenceAssemblies("net6.0", ".NETCore", "6.0");
#endif
#if NET472
            var ikvmTool = "net472";
            var ikvmLibs = Path.Combine(TESTBASE, "lib", "net472");
            var libPaths = DotNetSdkUtil.GetPathToReferenceAssemblies("net472", ".NETFramework", "4.7.2");
#endif

            var n = Guid.NewGuid().ToString("n");
            var p = Path.Combine(Path.GetTempPath(), n, $"helloworld_{n}.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(p));

            var rid = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                rid = "win-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                rid = "linux-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                rid = "osx-x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                rid = "osx-arm64";

            var e = new List<IkvmToolDiagnosticEvent>();
            var l = new IkvmImporterLauncher(Path.Combine(Path.GetDirectoryName(typeof(AssemblyClassLoaderTests).Assembly.Location), "ikvmc", ikvmTool, rid), new IkvmToolDelegateDiagnosticListener(evt => { e.Add(evt); TestContext.WriteLine(evt.Message, evt.MessageArgs); }));
            var z = libPaths.Append(ikvmLibs).ToArray();
            var o = new IkvmImporterOptions()
            {
                Runtime = Path.Combine(ikvmLibs, "IKVM.Runtime.dll"),
                ResponseFile = $"{n}_ikvmc.rsp",
                Input = { Path.Combine("helloworld", "helloworld-2.0.jar") },
                Assembly = $"helloworld_{n}",
                Version = "1.0.0.0",
                NoStdLib = true,
                Output = p,
                Lib = z
            };

            var exitCode = await l.ExecuteAsync(o);
            exitCode.Should().Be(0);

            helloworldDll = Assembly.LoadFrom(p);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            helloworldDll = null;
        }

        [TestMethod]
        public void CanGetPackage()
        {
            var t = helloworldDll.GetType("sample.HelloworldImpl");
            var k = ((global::java.lang.Class)t).getPackage();
            k.Should().NotBeNull();
        }

        [TestMethod]
        public void CanGetResource()
        {
            var t = helloworldDll.GetType("sample.HelloworldImpl");
            var k = ((global::java.lang.Class)t).getResource("/helloworld.composite");
            var s = ((global::java.lang.Class)t).getResourceAsStream("/helloworld.composite");
            k.Should().NotBeNull();
            s.Should().NotBeNull();
        }

    }

}
