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

            var targets = new[]
            {
                ("net461",          "win7-x86"),
                ("net472",          "win7-x86"),
                ("net48",           "win7-x86"),
                ("netcoreapp3.1",   "win7-x86"),
                ("net6.0",          "win7-x86"),
                ("net461",          "win7-x64"),
                ("net472",          "win7-x64"),
                ("net48",           "win7-x64"),
                ("netcoreapp3.1",   "win7-x64"),
                ("net6.0",          "win7-x64"),
                ("net461",          "win81-arm"),
                ("net472",          "win81-arm"),
                ("net48",           "win81-arm"),
                ("netcoreapp3.1",   "win81-arm"),
                ("net6.0",          "win81-arm"),
                ("netcoreapp3.1",   "linux-x64"),
                ("net6.0",          "linux-x64"),
                ("netcoreapp3.1",   "linux-arm"),
                ("net6.0",          "linux-arm"),
                ("netcoreapp3.1",   "linux-arm64"),
                ("net6.0",          "linux-arm64"),
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                targets = new[]
                {
                    ("netcoreapp3.1",   "win7-x86"),
                    ("net6.0",          "win7-x86"),
                    ("netcoreapp3.1",   "win7-x64"),
                    ("net6.0",          "win7-x64"),
                    ("netcoreapp3.1",   "win81-arm"),
                    ("net6.0",          "win81-arm"),
                    ("netcoreapp3.1",   "linux-x64"),
                    ("net6.0",          "linux-x64"),
                    ("netcoreapp3.1",   "linux-arm"),
                    ("net6.0",          "linux-arm"),
                    ("netcoreapp3.1",   "linux-arm64"),
                    ("net6.0",          "linux-arm64"),
                };
            }

            // write out nuget.config with package sources
            // can't use property for sdk resolver
            new XDocument(
                new XElement("configuration",
                    new XElement("config",
                        new XElement("add",
                            new XAttribute("key", "globalPackagesFolder"),
                            new XAttribute("value", nugetPackageRoot))),
                    new XElement("packageSources",
                        new XElement("add",
                            new XAttribute("key", "dev"),
                            new XAttribute("value", Path.Combine(Path.GetDirectoryName(typeof(ProjectTests).Assembly.Location), @"nuget"))))))
                .Save(Path.Combine(@"Project", "nuget.config"));

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(@"Project", "Exe", "ProjectExe.csproj"));
            analyzer.SetGlobalProperty("IkvmCacheDir", ikvmCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("IkvmExportCacheDir", ikvmCachePath + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("PackageVersion", properties["PackageVersion"]);
            analyzer.SetGlobalProperty("RestorePackagesPath", nugetPackageRoot + Path.DirectorySeparatorChar);
            analyzer.SetGlobalProperty("CreateHardLinksForAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyAdditionalFilesIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyFilesToOutputDirectoryIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForCopyLocalIfPossible", "true");
            analyzer.SetGlobalProperty("CreateHardLinksForPublishFilesIfPossible", "true");

            // allow NuGet to locate packages in existing global packages folder if set
            // else fallback to standard location
            if (Environment.GetEnvironmentVariable("NUGET_PACKAGES") is string nugetPackagesDir && Directory.Exists(nugetPackagesDir))
                analyzer.SetGlobalProperty("RestoreAdditionalProjectFallbackFolders", nugetPackagesDir);
            else
            {
                var d = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
                Directory.CreateDirectory(d);
                analyzer.SetGlobalProperty("RestoreAdditionalProjectFallbackFolders", d);
            }

            analyzer.AddBuildLogger(new TargetLogger(TestContext) { Verbosity = LoggerVerbosity.Detailed });

            {
                var options = new EnvironmentOptions();
                options.DesignTime = false;
                options.TargetsToBuild.Clear();
                options.TargetsToBuild.Add("Restore");
                options.TargetsToBuild.Add("Clean");
                var results = analyzer.Build(options);
                results.OverallSuccess.Should().Be(true);
            }

            foreach (var (tfm, rid) in targets)
            {
                TestContext.WriteLine("Publishing with TargetFramework {0} and RuntimeIdentifier {1}.", tfm, rid);

                var options = new EnvironmentOptions();
                options.DesignTime = false;
                options.Restore = false;
                options.GlobalProperties.Add("TargetFramework", tfm);
                options.GlobalProperties.Add("RuntimeIdentifier", rid);
                options.TargetsToBuild.Clear();
                options.TargetsToBuild.Add("Build");
                options.TargetsToBuild.Add("Publish");
                var results = analyzer.Build(options);
                results.OverallSuccess.Should().Be(true);
            }
        }

    }

}
