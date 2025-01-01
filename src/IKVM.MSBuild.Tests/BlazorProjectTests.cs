using System.Collections.Generic;
using System.IO;

using Buildalyzer;
using Buildalyzer.Environment;

using FluentAssertions;

using IKVM.MSBuild.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests
{

    [TestClass]
    public class BlazorProjectTests
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
            ProjectTestUtil.Init(context, "BlazorProject", out properties, out testRoot, out tempRoot, out workRoot, out nugetPackageRoot, out ikvmCachePath, out ikvmExportCachePath);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (Directory.Exists(tempRoot))
                Directory.Delete(tempRoot, true);
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanBuildBlazorApp()
        {
            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(testRoot, "BlazorApp", "BlazorApp.csproj"));
            analyzer.AddBuildLogger(new TargetLogger(TestContext));
            analyzer.AddBinaryLogger(Path.Combine(workRoot, $"BlazorApp-msbuild.binlog"));
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
            options.DesignTime = false;
            options.Restore = false;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Build");
            options.Arguments.Add("/v:d");

            var result = analyzer.Build(options);
            TestContext.AddResultFile(Path.Combine(workRoot, $"BlazorApp-msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);
        }

    }

}
