using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Buildalyzer;
using Buildalyzer.Environment;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests.Util
{

    public static class ProjectTestUtil
    {

        public struct ProjectState
        {

            public string ProjectName;
            public string ProjectRoot;
            public string ProjectFile;
            public Dictionary<string, string> Properties;
            public string TestRoot;
            public string TempRoot;
            public string WorkRoot;
            public string NuGetPackageRoot;
            public string IkvmCachePath;
            public string IkvmExportCachePath;

            /// <summary>
            /// Creates a new analyzer for the project specified relative to the solution root.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="id"></param>
            /// <param name="projectPath"></param>
            /// <returns></returns>
            public IProjectAnalyzer CreateAnalyzer(TestContext context, string id)
            {
                var manager = new AnalyzerManager();
                var analyzer = manager.GetProject(Path.Combine(TestRoot, ProjectFile));
                analyzer.AddBuildLogger(new TargetLogger(context));
                analyzer.AddBinaryLogger(Path.Combine(WorkRoot, id != null ? $"{id}-msbuild.binlog" : "msbuild.binlog"));
                analyzer.SetGlobalProperty("ImportDirectoryBuildProps", "false");
                analyzer.SetGlobalProperty("ImportDirectoryBuildTargets", "false");
                analyzer.SetGlobalProperty("IkvmCacheDir", IkvmCachePath + Path.DirectorySeparatorChar);
                analyzer.SetGlobalProperty("IkvmExportCacheDir", IkvmExportCachePath + Path.DirectorySeparatorChar);
                analyzer.SetGlobalProperty("RestorePackagesPath", NuGetPackageRoot + Path.DirectorySeparatorChar);
                analyzer.SetGlobalProperty("CreateHardLinksForAdditionalFilesIfPossible", "true");
                analyzer.SetGlobalProperty("CreateHardLinksForCopyAdditionalFilesIfPossible", "true");
                analyzer.SetGlobalProperty("CreateHardLinksForCopyFilesToOutputDirectoryIfPossible", "true");
                analyzer.SetGlobalProperty("CreateHardLinksForCopyLocalIfPossible", "true");
                analyzer.SetGlobalProperty("CreateHardLinksForPublishFilesIfPossible", "true");
                analyzer.SetGlobalProperty("Configuration", "Release");
                analyzer.SetGlobalProperties(Properties);
                return analyzer;
            }

        }

        /// <summary>
        /// 
        /// Initializes the given test solution. Solution files are expected to exist in the res/ directory of this assembly.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectName"></param>
        /// <param name="projectRoot"></param>
        /// <param name="projectFile"></param>
        /// <param name="state"></param>
        public static void Init(TestContext context, string projectName, string projectRoot, string projectFile, out ProjectState state)
        {
            // initialize state
            state = new ProjectState();
            state.ProjectName = projectName;
            state.ProjectRoot = projectRoot;
            state.ProjectFile = projectFile;

            // properties to load into test build
            state.Properties = File.ReadAllLines("IKVM.MSBuild.Tests.properties").Select(i => i.Split('=', 2)).ToDictionary(i => i[0], i => i[1]);

            // root of the project collection itself
            state.TestRoot = Path.Combine(Path.GetDirectoryName(typeof(ProjectTestUtil).Assembly.Location), "res", state.ProjectRoot);

            // temporary directory
            state.TempRoot = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tests", Guid.NewGuid().ToString());
            if (Directory.Exists(state.TempRoot))
                Directory.Delete(state.TempRoot, true);
            Directory.CreateDirectory(state.TempRoot);

            // work directory
            state.WorkRoot = Path.Combine(context.TestRunResultsDirectory, "IKVM.MSBuild.Tests", state.ProjectName);
            if (Directory.Exists(state.WorkRoot))
                Directory.Delete(state.WorkRoot, true);
            Directory.CreateDirectory(state.WorkRoot);

            // other required sub directories
            state.NuGetPackageRoot = Path.Combine(state.TempRoot, "nuget", "packages");
            state.IkvmCachePath = Path.Combine(state.TempRoot, "ikvm", "cache");
            state.IkvmExportCachePath = Path.Combine(state.TempRoot, "ikvm", "expcache");

            // nuget.config file that defines package sources
            new XDocument(
                new XElement("configuration",
                    new XElement("config",
                        new XElement("add",
                            new XAttribute("key", "globalPackagesFolder"),
                            new XAttribute("value", state.NuGetPackageRoot))),
                    new XElement("packageSources",
                        new XElement("clear"),
                        new XElement("add",
                            new XAttribute("key", "nuget.org"),
                            new XAttribute("value", "https://api.nuget.org/v3/index.json")),
                        new XElement("add",
                            new XAttribute("key", "dev"),
                            new XAttribute("value", Path.Combine(Path.GetDirectoryName(typeof(ProjectTestUtil).Assembly.Location), @"nuget"))))))
                .Save(Path.Combine(state.TestRoot, "nuget.config"));

            var analyzer = state.CreateAnalyzer(context, null);
            var options = new EnvironmentOptions();
            options.WorkingDirectory = state.TestRoot;
            options.DesignTime = false;
            options.Restore = true;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Restore");
            options.Arguments.Add("/v:d");

            var result = analyzer.Build(options);
            context.AddResultFile(Path.Combine(state.WorkRoot, "msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);
        }

        /// <summary>
        /// Invoke for test class cleanup.
        /// </summary>
        /// <param name="state"></param>
        public static void Clean(ProjectState state)
        {
            if (Directory.Exists(state.TempRoot))
                Directory.Delete(state.TempRoot, true);
        }

    }

}
