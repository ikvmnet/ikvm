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

        public IProjectAnalyzer Analyzer { get; set; }

        /// <summary>
        /// Initializes Buildalyzer for the tests.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            var properties = File.ReadAllLines("IKVM.MSBuild.Tests.properties").Select(i => i.Split('=', 2)).ToDictionary(i => i[0], i => i[1]);

            var nugetPackageRoot = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", Guid.NewGuid().ToString(), "nuget", "packages");
            if (Directory.Exists(nugetPackageRoot))
                Directory.Delete(nugetPackageRoot, true);
            Directory.CreateDirectory(nugetPackageRoot);

            var ikvmCachePath = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", Guid.NewGuid().ToString(), "ikvm", "cache");
            if (Directory.Exists(ikvmCachePath))
                Directory.Delete(ikvmCachePath, true);

            var ikvmExportCachePath = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", Guid.NewGuid().ToString(), "ikvm", "expcache");
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
            Analyzer = manager.GetProject(Path.Combine(@"Project", "Exe", "ProjectExe.csproj"));
            Analyzer.SetGlobalProperty("ImportDirectoryBuildProps", "false");
            Analyzer.SetGlobalProperty("ImportDirectoryBuildTargets", "false");
            Analyzer.SetGlobalProperty("IkvmCacheDir", ikvmCachePath + Path.DirectorySeparatorChar);
            Analyzer.SetGlobalProperty("IkvmExportCacheDir", ikvmExportCachePath + Path.DirectorySeparatorChar);
            Analyzer.SetGlobalProperty("PackageVersion", properties["PackageVersion"]);
            Analyzer.SetGlobalProperty("RestorePackagesPath", nugetPackageRoot + Path.DirectorySeparatorChar);
            Analyzer.SetGlobalProperty("CreateHardLinksForAdditionalFilesIfPossible", "true");
            Analyzer.SetGlobalProperty("CreateHardLinksForCopyAdditionalFilesIfPossible", "true");
            Analyzer.SetGlobalProperty("CreateHardLinksForCopyFilesToOutputDirectoryIfPossible", "true");
            Analyzer.SetGlobalProperty("CreateHardLinksForCopyLocalIfPossible", "true");
            Analyzer.SetGlobalProperty("CreateHardLinksForPublishFilesIfPossible", "true");
            Analyzer.SetGlobalProperty("Configuration", "Release");

            Analyzer.AddBuildLogger(new TargetLogger(TestContext) { Verbosity = LoggerVerbosity.Detailed });
            var options = new EnvironmentOptions();
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Restore");
            options.TargetsToBuild.Add("Clean");
            Analyzer.Build(options).OverallSuccess.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("net472", "win-x86")]
        [DataRow("net472", "win-x64")]
        [DataRow("net48", "win-x86")]
        [DataRow("net48", "win-x64")]
        [DataRow("net6.0", "win-x86")]
        [DataRow("net6.0", "win-x64")]
        [DataRow("net6.0", "linux-x64")]
        [DataRow("net6.0", "linux-arm")]
        [DataRow("net6.0", "linux-arm64")]
        [DataRow("net6.0", "linux-musl-x64")]
        [DataRow("net6.0", "linux-musl-arm")]
        [DataRow("net6.0", "linux-musl-arm64")]
        [DataRow("net6.0", "osx-x64")]
        [DataRow("net6.0", "osx-arm64")]
        [DataRow("net7.0", "win-x86")]
        [DataRow("net7.0", "win-x64")]
        [DataRow("net7.0", "linux-x64")]
        [DataRow("net7.0", "linux-arm")]
        [DataRow("net7.0", "linux-arm64")]
        [DataRow("net7.0", "linux-musl-x64")]
        [DataRow("net7.0", "linux-musl-arm")]
        [DataRow("net7.0", "linux-musl-arm64")]
        [DataRow("net7.0", "osx-x64")]
        [DataRow("net7.0", "osx-arm64")]
        public void CanBuildTestProject(string tfm, string rid)
        {
            // skip framework tests for non-Windows platforms
            if (tfm == "net472" || tfm == "net48")
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                    return;

            var options = new EnvironmentOptions();
            options.DesignTime = false;
            options.Restore = false;
            options.GlobalProperties["TargetFramework"] = tfm;
            options.GlobalProperties["RuntimeIdentifier"] = rid;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Build");
            options.TargetsToBuild.Add("Publish");
            Analyzer.Build(options).OverallSuccess.Should().Be(true);
        }

    }

}
