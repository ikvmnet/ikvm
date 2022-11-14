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

        [TestMethod]
        public void Can_compile_file()
        {
            var engine = new Mock<IBuildEngine3>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmJavaCompiler();
            t.BuildEngine = engine.Object;
            t.Sources = new[] { new TaskItem(Path.Combine(Path.GetDirectoryName(typeof(IkvmJavaCompilerTests).Assembly.Location), "IkvmJavaCompilerTests.java")) };
            t.Destination = Path.Combine(Path.GetDirectoryName(typeof(IkvmJavaCompilerTests).Assembly.Location), "out");
            t.Execute().Should().BeTrue();
            errors.Should().BeEmpty();
        }

    }

}
