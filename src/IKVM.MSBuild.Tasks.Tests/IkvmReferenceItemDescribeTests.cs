using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.MSBuild.Tasks.Tests
{

    [TestClass]
    public class IkvmReferenceItemDescribeTests
    {

        readonly static string HELLOWORLD1_JAR = @".\helloworld\helloworld-2.0-1\helloworld-2.0.jar";

        static IkvmReferenceItemDescribe BuildTestTask(params ITaskItem[] items)
        {
            var engine = new Mock<IBuildEngine>();
            return new IkvmReferenceItemDescribe() { BuildEngine = engine.Object, Items = items };
        }

        [TestMethod]
        public void Should_describe_jar()
        {
            var t = BuildTestTask(new TaskItem(HELLOWORLD1_JAR));
            t.Execute().Should().BeTrue();
            t.DescribedItems.Should().HaveCount(1);

            var i = t.DescribedItems[0];
            i.ItemSpec.Should().Be(HELLOWORLD1_JAR);
            i.GetMetadata(IkvmReferenceItemDescribe.OriginalItemSpecMetadataName).Should().Be(HELLOWORLD1_JAR);
            i.GetMetadata(IkvmReferenceItemMetadata.AssemblyName).Should().Be("helloworld");
            i.GetMetadata(IkvmReferenceItemMetadata.AssemblyVersion).Should().Be("2.0.0.0");
            i.GetMetadata(IkvmReferenceItemMetadata.Compile).Should().Contain("helloworld-2.0.jar");
            i.GetMetadata(IkvmReferenceItemDescribe.IsResolvedMetadataName).Should().Be("true");
            i.GetMetadata(IkvmReferenceItemDescribe.DiagnosticMetadataName).Should().BeEmpty();
        }

        [TestMethod]
        public void Should_keep_specified_metadata()
        {
            var item = new TaskItem(HELLOWORLD1_JAR);
            item.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "custom");
            item.SetMetadata(IkvmReferenceItemMetadata.Aliases, "hw");

            var t = BuildTestTask(item);
            t.Execute().Should().BeTrue();

            var i = t.DescribedItems[0];
            i.GetMetadata(IkvmReferenceItemMetadata.AssemblyName).Should().Be("custom");
            i.GetMetadata(IkvmReferenceItemMetadata.Aliases).Should().Be("hw");
        }

        [TestMethod]
        public void Should_report_missing_jar_without_failing()
        {
            var t = BuildTestTask(new TaskItem(@".\missing\missing.jar"), new TaskItem(HELLOWORLD1_JAR));
            t.Execute().Should().BeTrue();
            t.DescribedItems.Should().HaveCount(2);

            var missing = t.DescribedItems[0];
            missing.ItemSpec.Should().Be(@".\missing\missing.jar");
            missing.GetMetadata(IkvmReferenceItemDescribe.IsResolvedMetadataName).Should().Be("false");
            missing.GetMetadata(IkvmReferenceItemDescribe.DiagnosticMetadataName).Should().NotBeEmpty();

            t.DescribedItems[1].GetMetadata(IkvmReferenceItemDescribe.IsResolvedMetadataName).Should().Be("true");
        }

        [TestMethod]
        public void Should_report_missing_compile_path()
        {
            var item = new TaskItem("named");
            item.SetMetadata(IkvmReferenceItemMetadata.Compile, @".\missing\classes.jar");
            item.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "named");
            item.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "1.0");

            var t = BuildTestTask(item);
            t.Execute().Should().BeTrue();

            var i = t.DescribedItems[0];
            i.GetMetadata(IkvmReferenceItemDescribe.IsResolvedMetadataName).Should().Be("false");
            // as a build reports it: missing paths are dropped while expanding Compile, which leaves it empty
            i.GetMetadata(IkvmReferenceItemDescribe.DiagnosticMetadataName).Should().Contain("IKVMSDK0010");
        }

        [TestMethod]
        public void Should_ignore_unresolvable_references()
        {
            var item = new TaskItem(HELLOWORLD1_JAR);
            item.SetMetadata(IkvmReferenceItemMetadata.References, "other.jar");

            var t = BuildTestTask(item);
            t.Execute().Should().BeTrue();

            var i = t.DescribedItems[0];
            i.GetMetadata(IkvmReferenceItemDescribe.IsResolvedMetadataName).Should().Be("true");
            i.GetMetadata(IkvmReferenceItemMetadata.References).Should().Be("other.jar");
        }

    }

}
