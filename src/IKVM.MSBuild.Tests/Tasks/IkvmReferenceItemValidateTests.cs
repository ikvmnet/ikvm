using System.Collections.Generic;

using FluentAssertions;

using IKVM.MSBuild.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.MSBuild.Tests.Tasks
{

    [TestClass]
    public class IkvmReferenceItemValidateTests
    {

        [TestMethod]
        public void Should_fail_when_missing_name()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemValidate();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "2.0");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, @".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0002");
        }

        [TestMethod]
        public void Should_fail_when_missing_version()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemValidate();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, @".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0003");
        }

        [TestMethod]
        public void Should_fail_when_invalid_version()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemValidate();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "invalid");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, @".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0003");
        }

        [TestMethod]
        public void Should_fail_when_missing_compile()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemValidate();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(@"missing");
            t.Items = new[] { i1 };
            t.Execute().Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0002");
            errors.Should().Contain(x => x.Code == "IKVMSDK0003");
            errors.Should().Contain(x => x.Code == "IKVMSDK0010");
        }

        [TestMethod]
        public void Should_fail_when_invalid_sources()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemValidate();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "2.0");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, @".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.Sources, "invalid");
            t.Items = new[] { i1 };
            t.Execute().Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0007");
        }

        [TestMethod]
        public void Should_fail_when_missing_sources()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemValidate();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "2.0");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, @".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.Sources, "missing.java");
            t.Items = new[] { i1 };
            t.Execute().Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0009");
        }

    }

}
