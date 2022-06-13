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
            t.ToolVersion = "0.0.0.0";
            t.ToolFramework = "netcoreapp3.1";
            t.RuntimeAssembly = typeof(IKVM.Runtime.InternalException).Assembly.Location;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            i1.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion, "0.0.0.0");
            i1.SetMetadata(IkvmJavaReferenceItemMetadata.Compile, @".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity).Should().Be(@"8328e23109508e20ded72593ce94a0e3");
        }

        [TestMethod]
        public void Should_assign_identity_to_jar_for_net461()
        {
            var t = new IkvmAssignJavaReferenceItemIdentity();
            t.ToolVersion = "0.0.0.0";
            t.ToolFramework = "net461";
            t.RuntimeAssembly = typeof(IKVM.Runtime.InternalException).Assembly.Location;
            var i1 = new TaskItem(@".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            i1.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion, "0.0.0.0");
            i1.SetMetadata(IkvmJavaReferenceItemMetadata.Compile, @".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity).Should().Be(@"dcd3bbb855492ae08c67c4e3c9254992");
        }

    }

}
