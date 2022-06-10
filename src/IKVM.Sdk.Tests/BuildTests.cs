using System;
using System.IO;

using Buildalyzer;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Sdk.Tests
{

    [TestClass]
    public class BuildTests
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
                eventSource.AnyEventRaised += (sender, evt) => context.WriteLine($"{evt.Message}");
            }

        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Can_build_test_project()
        {
            var nugetPackageRoot = Path.Combine(Path.GetTempPath(), "IKVM.Sdk.Tests", "nuget", "packages");
            if (Directory.Exists(nugetPackageRoot))
                Directory.Delete(nugetPackageRoot, true);
            Directory.CreateDirectory(nugetPackageRoot);

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(Path.Combine(@"Project", "Project.csproj"));
            analyzer.SetGlobalProperty("RestoreAdditionalProjectSources", Path.GetFullPath(@"nuget"));
            analyzer.SetGlobalProperty("RestorePackagesPath", nugetPackageRoot + Path.DirectorySeparatorChar);
            analyzer.AddBuildLogger(new TargetLogger(TestContext));
            var results = analyzer.Build();
            results.OverallSuccess.Should().Be(true);
        }

    }

}