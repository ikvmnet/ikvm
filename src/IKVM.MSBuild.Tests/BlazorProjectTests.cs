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

        static ProjectTestUtil.ProjectState state;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ProjectTestUtil.Init(context, "BlazorProject", "BlazorProject", Path.Combine("BlazorApp", "BlazorApp.csproj"), out state);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ProjectTestUtil.Clean(state);
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanBuildBlazorApp()
        {
            var analyzer = state.CreateAnalyzer(TestContext, "BlazorApp");
            var options = new EnvironmentOptions();
            options.WorkingDirectory = state.TestRoot;
            options.DesignTime = false;
            options.Restore = false;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Build");
            options.TargetsToBuild.Add("Publish");
            options.Arguments.Add("/v:d");

            var result = analyzer.Build(options);
            TestContext.AddResultFile(Path.Combine(state.WorkRoot, $"BlazorApp-msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);
        }

    }

}
