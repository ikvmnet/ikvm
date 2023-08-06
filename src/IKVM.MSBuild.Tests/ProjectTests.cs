using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using Buildalyzer;
using Buildalyzer.Environment;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests
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

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Can_build_test_project()
        {
            var properties = File.ReadAllLines("IKVM.MSBuild.Tests.properties").Select(i => i.Split('=', 2)).ToDictionary(i => i[0], i => i[1]);

            var nugetPackageRoot = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", "nuget", "packages");
            if (Directory.Exists(nugetPackageRoot))
                Directory.Delete(nugetPackageRoot, true);
            Directory.CreateDirectory(nugetPackageRoot);

            var ikvmCachePath = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", "ikvm", "cache");
            if (Directory.Exists(ikvmCachePath))
                Directory.Delete(ikvmCachePath, true);

            var ikvmExportCachePath = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", "ikvm", "expcache");
            if (Directory.Exists(ikvmExportCachePath))
                Directory.Delete(ikvmExportCachePath, true);

            new XDocument(
                new XElement("configuration",
                    new XElement("config",
                        new XElement("add",
                            new XAttribute("key", "globalPackagesFolder"),
                            new XAttribute("value", nugetPackageRoot))),
                    new XElement("packageSources",
                        new XElement("clear"),
                        new XElement("add",
                            new XAttribute("key", "nuget.org"),
                            new XAttribute("value", "https://api.nuget.org/v3/index.json")),
                        new XElement("add",
                            new XAttribute("key", "dev"),
                            new XAttribute("value", Path.Combine(Path.GetDirectoryName(typeof(ProjectTests).Assembly.Location), @"nuget"))),
                        new XElement("add",
                            new XAttribute("key", "nuget.org"),
                            new XAttribute("value", "https://api.nuget.org/v3/index.json")))))
                .Save(Path.Combine(@"Project", "nuget.config"));

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(@"Project", "Exe", "ProjectExe.csproj"));
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

            analyzer.AddBuildLogger(new TargetLogger(TestContext) { Verbosity = LoggerVerbosity.Detailed });
            var o = new EnvironmentOptions();
            o.TargetsToBuild.Clear();
            o.TargetsToBuild.Add("Restore");
            o.TargetsToBuild.Add("Clean");
            analyzer.Build(o).OverallSuccess.Should().BeTrue();

            var targets = new[]
            {
                ("net472",          "win7-x86"),
                ("net472",          "win7-x64"),
                ("net472",          "win81-arm"),
                ("net48",           "win7-x86"),
                ("net48",           "win7-x64"),
                ("net48",           "win81-arm"),
                ("net6.0",          "win7-x86"),
                ("net6.0",          "win7-x64"),
                ("net6.0",          "win81-arm"),
                ("net6.0",          "linux-x64"),
                ("net6.0",          "linux-arm"),
                ("net6.0",          "linux-arm64"),
                ("net6.0",          "linux-musl-x64"),
                ("net6.0",          "linux-musl-arm"),
                ("net6.0",          "linux-musl-arm64"),
                ("net6.0",          "osx-x64"),
                ("net6.0",          "osx-arm64"),
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                targets = new[]
                {
                    ("net6.0",          "win7-x86"),
                    ("net6.0",          "win7-x64"),
                    ("net6.0",          "win81-arm"),
                    ("net6.0",          "linux-x64"),
                    ("net6.0",          "linux-arm"),
                    ("net6.0",          "linux-arm64"),
                    ("net6.0",          "linux-musl-x64"),
                    ("net6.0",          "linux-musl-arm"),
                    ("net6.0",          "linux-musl-arm64"),
                    ("net6.0",          "osx-x64"),
                    ("net6.0",          "osx-arm64"),
                };
            }

            foreach (var (tfm, rid) in targets)
            {
                TestContext.WriteLine("Publishing with TargetFramework {0} and RuntimeIdentifier {1}.", tfm, rid);
                var options = new EnvironmentOptions();
                options.DesignTime = false;
                options.Restore = false;
                options.GlobalProperties["TargetFramework"] = tfm;
                options.GlobalProperties["RuntimeIdentifier"] = rid;
                options.TargetsToBuild.Clear();
                options.TargetsToBuild.Add("Build");
                options.TargetsToBuild.Add("Publish");
                analyzer.Build(o).OverallSuccess.Should().Be(true);
            }
        }

    }

}
