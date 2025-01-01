using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Buildalyzer;
using Buildalyzer.Environment;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests.Util
{

    public static class ProjectTestUtil
    {

        public static void Init(
            TestContext context,
            string solutionName,
            out Dictionary<string, string> properties,
            out string testRoot,
            out string tempRoot,
            out string workRoot,
            out string nugetPackageRoot,
            out string ikvmCachePath,
            out string ikvmExportCachePath)
        {
            // properties to load into test build
            properties = File.ReadAllLines("IKVM.MSBuild.Tests.properties").Select(i => i.Split('=', 2)).ToDictionary(i => i[0], i => i[1]);

            // root of the project collection itself
            testRoot = Path.Combine(Path.GetDirectoryName(typeof(BasicProjectTests).Assembly.Location), "res", solutionName);

            // temporary directory
            tempRoot = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", Guid.NewGuid().ToString());
            if (Directory.Exists(tempRoot))
                Directory.Delete(tempRoot, true);
            Directory.CreateDirectory(tempRoot);

            // work directory
            workRoot = Path.Combine(context.TestRunResultsDirectory, "IKVM.MSBuild.Tests", solutionName);
            if (Directory.Exists(workRoot))
                Directory.Delete(workRoot, true);
            Directory.CreateDirectory(workRoot);

            // other required sub directories
            nugetPackageRoot = Path.Combine(tempRoot, "nuget", "packages");
            ikvmCachePath = Path.Combine(tempRoot, "ikvm", "cache");
            ikvmExportCachePath = Path.Combine(tempRoot, "ikvm", "expcache");

            // nuget.config file that defines package sources
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
                            new XAttribute("value", Path.Combine(Path.GetDirectoryName(typeof(ProjectTestUtil).Assembly.Location), @"nuget"))))))
                .Save(Path.Combine(testRoot, "nuget.config"));

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(testRoot, solutionName + ".sln"));
            analyzer.AddBuildLogger(new TargetLogger(context));
            analyzer.AddBinaryLogger(Path.Combine(workRoot, "msbuild.binlog"));
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
            options.Restore = true;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Restore");
            options.Arguments.Add("/v:d");

            var result = analyzer.Build(options);
            context.AddResultFile(Path.Combine(workRoot, "msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);
        }

    }

}
