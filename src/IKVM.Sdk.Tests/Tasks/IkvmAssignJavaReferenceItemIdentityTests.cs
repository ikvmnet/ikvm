using FluentAssertions;

using IKVM.Sdk.Tasks;

using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Sdk.Tests.Tasks
{

    [TestClass]
    public class IkvmAssignJavaReferenceItemIdentityTests
    {

        [TestMethod]
        public void Should_assign_identity_to_jar()
        {
            var t = new IkvmAssignJavaReferenceItemIdentity();
            t.RuntimeAssembly = typeof(IKVM.Runtime.InternalException).Assembly.Location;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity).Should().Be(@"3736b42933bd384f048812670eac32c3");
        }

    }

}
