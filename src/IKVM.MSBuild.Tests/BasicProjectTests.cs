using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Buildalyzer;
using Buildalyzer.Environment;

using FluentAssertions;

using IKVM.MSBuild.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests
{

    [TestClass]
    public class BasicProjectTests
    {

        static Dictionary<string, string> properties;
        static string testRoot;
        static string tempRoot;
        static string workRoot;
        static string nugetPackageRoot;
        static string ikvmCachePath;
        static string ikvmExportCachePath;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ProjectTestUtil.Init(context, "BasicProject", out properties, out testRoot, out tempRoot, out workRoot, out nugetPackageRoot, out ikvmCachePath, out ikvmExportCachePath);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (Directory.Exists(tempRoot))
                Directory.Delete(tempRoot, true);
        }

        public TestContext TestContext { get; set; }

        [DataTestMethod]
        [DataRow(EnvironmentPreference.Core, "net472", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net472", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net472", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net48", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net48", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net48", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        public void CanBuildTestProject(EnvironmentPreference env, string tfm, string rid, string exe, string lib)
        {
            // skip framework tests for non-Windows platforms
            if (env == EnvironmentPreference.Framework || tfm == "net472" || tfm == "net48")
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                    return;

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(testRoot, "Exe", "ProjectExe.csproj"));
            analyzer.AddBuildLogger(new TargetLogger(TestContext));
            analyzer.AddBinaryLogger(Path.Combine(workRoot, $"{env}-{tfm}-{rid}-msbuild.binlog"));
            analyzer.SetGlobalProperty("ImportDirectoryBuildProps", "false");
            analyzer.SetGlobalProperty("ImportDirectoryBuildTargets", "false");
            analyzer.SetGlobalProperty("IkvmCacheDir", ikvmCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("IkvmExportCacheDir", ikvmExportCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("PackageVersion", properties["PackageVersion"]);
            analyzer.SetGlobalProperty("RestorePackagesPath", nugetPackageRoot + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("CreateHardLinksForAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyFilesToOutputDirectoryIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyLocalIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForPublishFilesIfPossible", "true");
            analyzer.SetGlobalProperty("Configuration", "Release");

            var options = new EnvironmentOptions();
            options.WorkingDirectory = testRoot;
            options.Preference = env;
            options.DesignTime = false;
            options.Restore = false;
            options.GlobalProperties["TargetFramework"] = tfm;
            options.GlobalProperties["RuntimeIdentifier"] = rid;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Build");
            options.TargetsToBuild.Add("Publish");
            options.Arguments.Add("/v:d");

            var result = analyzer.Build(options);
            TestContext.AddResultFile(Path.Combine(workRoot, $"{env}-{tfm}-{rid}-msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);

            var binDir = Path.Combine("Project", "Exe", "bin", "Release", tfm, rid);

            // check in build output and publish output
            foreach (var i in new[] { "", "publish" })
            {
                var outDir = Path.Combine(binDir, i);

                // main artifiacts generated by project
                File.Exists(Path.Combine(outDir, string.Format(exe, "ProjectExe"))).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ProjectLib.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "helloworld.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "helloworld-2.dll")).Should().BeTrue();

                // ikvm libraries
                File.Exists(Path.Combine(outDir, "IKVM.Runtime.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "IKVM.Java.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, string.Format(lib, "ikvm"))).Should().BeTrue();

                // ikvm image direcetories
                Directory.Exists(Path.Combine(outDir, "ikvm")).Should().BeTrue();
                Directory.Exists(Path.Combine(outDir, "ikvm", rid)).Should().BeTrue();
                Directory.Exists(Path.Combine(outDir, "ikvm", rid, "bin")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "TRADEMARK")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "bin", "IKVM.Runtime.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "bin", "IKVM.Java.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "tzdb.dat")).Should().BeTrue();

                if (rid.StartsWith("win-"))
                    File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "tzmappings")).Should().BeTrue();

                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "currency.data")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "security", "java.policy")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "security", "java.security")).Should().BeTrue();

                // ikvm image bin exeecutables
                foreach (var exeName in new[] { "jar", "jarsigner", "java", "javac", "javah", "javap", "jdeps", "keytool", "native2ascii", "orbd", "policytool", "rmic", "schemagen", "wsgen", "wsimport", "xjc" })
                    File.Exists(Path.Combine(outDir, "ikvm", rid, "bin", string.Format(exe, exeName))).Should().BeTrue();

                // ikvm image native libraries
                foreach (var libName in new[] { "awt", "iava", "jvm", "management", "net", "nio", "sunec", "unpack", "verify" })
                    File.Exists(Path.Combine(outDir, "ikvm", rid, "bin", string.Format(lib, libName))).Should().BeTrue();
            }

            if (rid == "win-x86")
            {
                var outDir = Path.Combine(binDir, "publish");
                Directory.GetDirectories(Path.Combine(outDir, "ikvm")).Should().HaveCount(2);
                Directory.Exists(Path.Combine(outDir, "ikvm")).Should().BeTrue();
                Directory.Exists(Path.Combine(outDir, "ikvm", "win-x64")).Should().BeTrue();
                Directory.Exists(Path.Combine(outDir, "ikvm", "win-x64", "bin")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", "win-x64", "bin", "IKVM.Runtime.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", "win-x64", "bin", "IKVM.Java.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", "win-x64", "bin", string.Format(lib, "iava"))).Should().BeTrue();

                // ikvm image native libraries
                foreach (var libName in new[] { "awt", "iava", "jvm", "management", "net", "nio", "sunec", "unpack", "verify" })
                    File.Exists(Path.Combine(outDir, "ikvm", "win-x64", "bin", string.Format(lib, libName))).Should().BeTrue();
            }
            else
            {
                var outDir = Path.Combine(binDir, "publish");
                Directory.GetDirectories(Path.Combine(outDir, "ikvm")).Should().HaveCount(1);
            }
        }

    }

}
