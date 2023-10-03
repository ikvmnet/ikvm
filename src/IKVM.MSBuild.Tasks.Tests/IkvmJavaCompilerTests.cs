using System;
using System.Collections.Generic;
using System.IO;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.MSBuild.Tasks.Tests
{

    [TestClass]
    public class IkvmJavaCompilerTests
    {

        static string testDir;

        [ClassInitialize]

        public static void ClassInitialize()
        {
            testDir = Path.Combine(Path.GetTempPath(), "IKVM.MSBuild.Tasks.Tests", Guid.NewGuid().ToString(), "IkvmJavaCompilerTests");
        }

        [ClassCleanup]

        public static void ClassCleanup()
        {
            try
            {
                Directory.Delete(testDir, true);
            }
            catch
            {
                // ignore
            }
        }

        [TestMethod]
        public void CanExecuteCompiler()
        {
            var dir = Path.Combine(testDir, "CanExecuteCompiler");
            var classesDir = Path.Combine(dir, "classes");
            var headersDir = Path.Combine(dir, "headers");

            var engine = new Mock<IBuildEngine3>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmJavaCompiler();
            t.BuildEngine = engine.Object;
            t.Sources = new[] { new TaskItem(Path.Combine(Path.GetDirectoryName(typeof(IkvmJavaCompilerTests).Assembly.Location), "IkvmJavaCompilerTests.java")) };
            t.Destination = classesDir;
            t.HeaderDestination = headersDir;
            t.Execute().Should().BeTrue();
            errors.Should().BeEmpty();

            File.Exists(Path.Combine(classesDir, "ikvm", "msbuild", "tasks", "tests", "IkvmReferenceItemPrepareTests.class")).Should().BeTrue();
            File.Exists(Path.Combine(headersDir, "ikvm_msbuild_tasks_tests_IkvmReferenceItemPrepareTests.h")).Should().BeTrue();
        }

    }

}
