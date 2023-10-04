using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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

        public static Dictionary<string, string> Properties { get; set; }

        public static string TempRoot { get; set; }

        public static string WorkRoot { get; set; }

        public static string NuGetPackageRoot { get; set; }

        public static string IkvmCachePath { get; set; }

        public static string IkvmExportCachePath { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // properties to load into test build
            Properties = File.ReadAllLines("IKVM.MSBuild.Tests.properties").Select(i => i.Split('=', 2)).ToDictionary(i => i[0], i => i[1]);

            // temporary directory
            TempRoot = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", Guid.NewGuid().ToString());
            if (Directory.Exists(TempRoot))
                Directory.Delete(TempRoot, true);
            Directory.CreateDirectory(TempRoot);

            // work directory
            WorkRoot = Path.Combine(context.TestRunResultsDirectory, "IKVM.MSBuild.Tests", Guid.NewGuid().ToString());
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
                            new XAttribute("value", Path.Combine(Path.GetDirectoryName(typeof(ProjectTests).Assembly.Location), @"nuget"))))))
                .Save(Path.Combine(@"Project", "nuget.config"));

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(@"Project", "Exe", "ProjectExe.csproj"));
            analyzer.AddBuildLogger(new TargetLogger(context));
            analyzer.AddBinaryLogger(Path.Combine(TempRoot, "msbuild.binlog"));
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
            options.DesignTime = false;
            options.Restore = true;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Restore");
            analyzer.Build(options).OverallSuccess.Should().Be(true);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (Directory.Exists(TempRoot))
                Directory.Delete(TempRoot, true);
        }

        public TestContext TestContext { get; set; }

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

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(@"Project", "Exe", "ProjectExe.csproj"));
            analyzer.AddBuildLogger(new TargetLogger(TestContext));
            analyzer.AddBinaryLogger(Path.Combine(WorkRoot, $"{tfm}-{rid}-msbuild.binlog"));
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
            options.DesignTime = false;
            options.Restore = true;
            options.GlobalProperties["TargetFramework"] = tfm;
            options.GlobalProperties["RuntimeIdentifier"] = rid;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Build");
            options.TargetsToBuild.Add("Publish");
            analyzer.Build(options).OverallSuccess.Should().Be(true);
        }

    }

}
