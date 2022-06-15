using FluentAssertions;

using IKVM.MSBuild.Tasks;

using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests.Tasks
{

    [TestClass]
    public class IkvmReferenceItemSortTests
    {

        [TestMethod]
        public void Should_sort_by_reference_order()
        {
            var t = new IkvmReferenceItemSort();
            var i2 = new TaskItem(@".\Project\Lib\helloworld-2.0-2\helloworld-2.0.jar");
            i2.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i2.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "0.0.0.0");
            i2.SetMetadata(IkvmReferenceItemMetadata.References, @".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "0.0.0.0");
            t.Items = new[] { i1, i2 };
            t.Execute().Should().BeTrue();
            t.Items[0].Should().Be(i2);
            t.Items[1].Should().Be(i1);
        }

    }

}
