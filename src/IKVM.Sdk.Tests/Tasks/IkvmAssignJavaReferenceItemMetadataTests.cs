using FluentAssertions;

using IKVM.Sdk.Tasks;

using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Sdk.Tests.Tasks
{

    [TestClass]
    public class IkvmAssignJavaReferenceItemMetadataTests
    {

        [TestMethod]
        public void Should_normalize_jar_itemspec()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\.\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.ItemSpec.Should().Be(@"Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
        }

        [TestMethod]
        public void Should_normalize_dir_itemspec()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem(@".\Project");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.ItemSpec.Should().Be(@"Project\");
        }

        [TestMethod]
        public void Should_not_normalize_unknown_itemspec()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem("itemspecvalue");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.ItemSpec.Should().Be("itemspecvalue");
        }

        [TestMethod]
        public void Should_add_jar_identity_to_compile()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            var c = i1.GetMetadata(IkvmJavaReferenceItemMetadata.Compile);
            c.Split(IkvmJavaReferenceItemMetadata.PropertySeperatorChar).Should().Contain(@"Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
        }

        [TestMethod]
        public void Should_not_add_dir_identity_to_compile()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem(@".\Project");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            var c = i1.GetMetadata(IkvmJavaReferenceItemMetadata.Compile);
            c.Split(IkvmJavaReferenceItemMetadata.PropertySeperatorChar).Should().NotContain(@"Project\");
        }

        [TestMethod]
        public void Should_resolve_reference()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            var i2 = new TaskItem(@".\Project\Lib\helloworld-2.0-2\helloworld-2.0.jar");
            i2.SetMetadata(IkvmJavaReferenceItemMetadata.References, @".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            t.Items = new[] { i1, i2 };
            t.Execute().Should().BeTrue();
            var c = i2.GetMetadata(IkvmJavaReferenceItemMetadata.References);
            c.Split(IkvmJavaReferenceItemMetadata.PropertySeperatorChar).Should().Contain(@"Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
        }

        [TestMethod]
        public void Should_get_default_assembly_name_from_jar()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            var c = i1.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName);
            c.Should().Be("helloworld");
        }

        [TestMethod]
        public void Should_get_default_assembly_version_from_jar()
        {
            var t = new IkvmAssignJavaReferenceItemMetadata();
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            var c = i1.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion);
            c.Should().Be("2.0");
        }

    }

}
