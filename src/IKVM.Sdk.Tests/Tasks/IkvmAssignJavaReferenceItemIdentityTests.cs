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
        public void Should_assign_identity_to_jar_for_netcoreapp3_1()
        {
            var t = new IkvmAssignJavaReferenceItemIdentity();
            t.ToolFramework = "netcoreapp3.1";
            t.RuntimeAssembly = typeof(IKVM.Runtime.InternalException).Assembly.Location;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity).Should().Be(@"a68c90ce67d62b3f06cb22b1d2150050");
        }

        [TestMethod]
        public void Should_assign_identity_to_jar_for_net461()
        {
            var t = new IkvmAssignJavaReferenceItemIdentity();
            t.ToolFramework = "net461";
            t.RuntimeAssembly = typeof(IKVM.Runtime.InternalException).Assembly.Location;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity).Should().Be(@"858d32c71a02d24422e139b9320ac519");
        }

    }

}
