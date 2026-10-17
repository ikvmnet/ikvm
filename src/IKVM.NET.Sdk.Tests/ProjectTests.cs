using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using Buildalyzer;
using Buildalyzer.Environment;

using CliWrap;
using CliWrap.Buffered;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.NET.Sdk.Tests
{

    [TestClass]
    public class ProjectTests
    {

        /// <summary>
        /// Forwards MSBuild events to the test context.
        /// </summary>
        class TargetLogger : Logger
        {

            readonly TestContext context;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public TargetLogger(TestContext context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public override void Initialize(IEventSource eventSource)
            {
                eventSource.AnyEventRaised += (sender, evt) => context.WriteLine(evt.Message);
            }

        }

        public static Dictionary<string, string> Properties { get; set; }

        public static string TestRoot { get; set; }

        public static string TempRoot { get; set; }

        public static string WorkRoot { get; set; }

        public static string NuGetPackageRoot { get; set; }

        public static string IkvmCachePath { get; set; }

        public static string IkvmExportCachePath { get; set; }

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            // properties to load into test build
            Properties = File.ReadAllLines("IKVM.NET.Sdk.Tests.properties").Select(i => i.Split('=', 2)).ToDictionary(i => i[0], i => i[1]);

            // root of the project collection itself
            TestRoot = Path.Combine(Path.GetDirectoryName(typeof(ProjectTests).Assembly.Location), "Project");

            // temporary directory
            TempRoot = Path.Combine(Path.GetTempPath(), "IKVM.NET.Sdk.Tests", Guid.NewGuid().ToString());
            if (Directory.Exists(TempRoot))
                Directory.Delete(TempRoot, true);
            Directory.CreateDirectory(TempRoot);

            // work directory
            WorkRoot = Path.Combine(context.TestRunResultsDirectory, "IKVM.NET.Sdk.Tests", "ProjectTests");
            if (Directory.Exists(WorkRoot))
                Directory.Delete(WorkRoot, true);
            Directory.CreateDirectory(WorkRoot);

            // other required sub directories
            NuGetPackageRoot = Path.Combine(TempRoot, "nuget", "packages");
            IkvmCachePath = Path.Combine(TempRoot, "ikvm", "cache");
            IkvmExportCachePath = Path.Combine(TempRoot, "ikvm", "expcache");

            // nuget.config file that defines package sources
            new XDocument(
                new XElement("configuration",
                    new XElement("config",
                        new XElement("add",
                            new XAttribute("key", "globalPackagesFolder"),
                            new XAttribute("value", NuGetPackageRoot))),
                    new XElement("packageSources",
                        new XElement("clear"),
                        new XElement("add",
                            new XAttribute("key", "nuget.org"),
                            new XAttribute("value", "https://api.nuget.org/v3/index.json")),
                        new XElement("add",
                            new XAttribute("key", "dev"),
                            new XAttribute("value", Path.Combine(Path.GetDirectoryName(typeof(ProjectTests).Assembly.Location), "nuget"))))))
                .Save(Path.Combine(TestRoot, "nuget.config"));

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(TestRoot, "Exe", "ProjectExe.msbuildproj"));
            analyzer.AddBuildLogger(new TargetLogger(context));
            analyzer.AddBinaryLogger(Path.Combine(WorkRoot, "msbuild.binlog"));
            analyzer.SetEnvironmentVariable("NUGET_PACKAGES", "");
            analyzer.SetGlobalProperty("ImportDirectoryBuildProps", "false");
            analyzer.SetGlobalProperty("ImportDirectoryBuildTargets", "false");
            analyzer.SetGlobalProperty("IkvmCacheDir", IkvmCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("IkvmExportCacheDir", IkvmExportCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("PackageVersion", Properties["PackageVersion"]);
            analyzer.SetGlobalProperty("RestorePackagesPath", NuGetPackageRoot + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("CreateHardLinksForAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyFilesToOutputDirectoryIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyLocalIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForPublishFilesIfPossible", "true");
            analyzer.SetGlobalProperty("Configuration", "Release");

            var options = new EnvironmentOptions();
            options.WorkingDirectory = TestRoot;
            options.DesignTime = false;
            options.Restore = true;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Restore");
            options.Arguments.Add("/v:diag");

            var result = analyzer.Build(options);
            context.WriteLine(Path.Combine(WorkRoot, "msbuild.binlog"));
            context.AddResultFile(Path.Combine(WorkRoot, "msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (Directory.Exists(TempRoot))
                Directory.Delete(TempRoot, true);
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
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
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net8.0-windows", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net10.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net10.0-windows", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-x64", "{0}.exe", "{0}.dll")]
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
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net8.0-windows", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net10.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net10.0-windows", "osx-arm64", "{0}", "lib{0}.dylib")]
        public async System.Threading.Tasks.Task CanBuildTestProject(EnvironmentPreference env, string tfm, string rid, string exe, string lib)
        {
            // skip framework tests for non-Windows platforms
            if (env == EnvironmentPreference.Framework || tfm == "net472" || tfm == "net48")
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                    return;

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(@"Project", "Exe", "ProjectExe.msbuildproj"));
            analyzer.AddBuildLogger(new TargetLogger(TestContext));
            analyzer.AddBinaryLogger(Path.Combine(WorkRoot, $"{env}-{tfm}-{rid}-msbuild.binlog"));
            analyzer.SetEnvironmentVariable("NUGET_PACKAGES", "");
            analyzer.SetGlobalProperty("ImportDirectoryBuildProps", "false");
            analyzer.SetGlobalProperty("ImportDirectoryBuildTargets", "false");
            analyzer.SetGlobalProperty("IkvmCacheDir", IkvmCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("IkvmExportCacheDir", IkvmExportCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("PackageVersion", Properties["PackageVersion"]);
            analyzer.SetGlobalProperty("RestorePackagesPath", NuGetPackageRoot + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("CreateHardLinksForAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyFilesToOutputDirectoryIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyLocalIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForPublishFilesIfPossible", "true");
            analyzer.SetGlobalProperty("Configuration", "Release");

            var options = new EnvironmentOptions();
            options.WorkingDirectory = TestRoot;
            options.Preference = env;
            options.DesignTime = false;
            options.Restore = false;
            options.GlobalProperties["TargetFramework"] = tfm;
            options.GlobalProperties["RuntimeIdentifier"] = rid;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Build");
            options.TargetsToBuild.Add("Publish");
            options.Arguments.Add("/v:diag");

            var result = analyzer.Build(options);
            TestContext.WriteLine(Path.Combine(WorkRoot, $"{env}-{tfm}-{rid}-msbuild.binlog"));
            TestContext.AddResultFile(Path.Combine(WorkRoot, $"{env}-{tfm}-{rid}-msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);

            var binDir = Path.Combine("Project", "Exe", "bin", "Release", tfm, rid);

            // check in build output and publish output
            foreach (var i in new[] { "", "publish" })
            {
                var outDir = Path.Combine(binDir, i);

                // main artifiacts generated by project
                File.Exists(Path.Combine(outDir, string.Format(exe, "ProjectExe"))).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ProjectLib.dll")).Should().BeTrue();

                // ikvm libraries
                File.Exists(Path.Combine(outDir, "IKVM.Runtime.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "IKVM.Java.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, string.Format(lib, "ikvm"))).Should().BeTrue();

                // ikvm image direcetories
                Directory.Exists(Path.Combine(outDir, "ikvm")).Should().BeTrue();
                Directory.Exists(Path.Combine(outDir, "ikvm", rid)).Should().BeTrue();
                Directory.Exists(Path.Combine(outDir, "ikvm", rid, "bin")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "TRADEMARK")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "tzdb.dat")).Should().BeTrue();

                if (rid.StartsWith("win-"))
                    File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "tzmappings")).Should().BeTrue();

                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "currency.data")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "security", "java.policy")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ikvm", rid, "lib", "security", "java.security")).Should().BeTrue();

                // ikvm image native libraries
                foreach (var libName in new[] { "awt", "iava", "jvm", "management", "net", "nio", "sunec", "unpack", "verify" })
                    File.Exists(Path.Combine(outDir, "ikvm", rid, "bin", string.Format(lib, libName))).Should().BeTrue();
            }

            // current rid matches target, execute and check output
            if (GetCurrentRid() == rid)
                (await Cli.Wrap(Path.Combine(binDir, "publish", string.Format(exe, "ProjectExe"))).WithArguments("Hello").ExecuteBufferedAsync()).StandardOutput.Trim().Should().Be("Hello");
        }

        /// <summary>
        /// Gets the current RID.
        /// </summary>
        /// <returns></returns>
        string GetCurrentRid()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return "win-x64";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return "linux-x64";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return "osx-x64";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                return "osx-arm64";
            else
                return null;
        }

    }

}
