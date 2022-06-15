using FluentAssertions;

using IKVM.MSBuild.Tasks;

using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests.Tasks
{

    [TestClass]
    public class IkvmReferenceItemAssignIdentityTests
    {

        readonly static string HELLOWORLD_JAR = @".\Project\Lib\helloworld-2.0-1\helloworld-2.0.jar";

        /// <summary>
        /// Builds a basic task item.
        /// </summary>
        /// <param name="itemSpec"></param>
        /// <param name="assemblyName"></param>
        /// <param name="assemblyVersion"></param>
        /// <returns></returns>
        TaskItem BuildItem(string itemSpec, string assemblyName, string assemblyVersion)
        {
            var item = new TaskItem(itemSpec);
            item.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, assemblyName);
            item.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, assemblyVersion);
            item.SetMetadata(IkvmReferenceItemMetadata.Compile, itemSpec);
            return item;
        }

        /// <summary>
        /// Builds a new task instance with various information.
        /// </summary>
        /// <param name="toolFramework"></param>
        /// <returns></returns>
        IkvmReferenceItemAssignIdentity BuildTask(string toolFramework, string toolVersion)
        {
            var t = new IkvmReferenceItemAssignIdentity();
            t.ToolVersion = toolVersion;
            t.ToolFramework = toolFramework;
            t.RuntimeAssembly = typeof(IKVM.Runtime.InternalException).Assembly.Location;
            t.References = new[] { new TaskItem(typeof(object).Assembly.Location) };
            return t;
        }

        [TestMethod]
        public void Should_assign_identity_to_jar_for_netcoreapp3_1()
        {
            var t = BuildTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().NotBeUpperCased();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().HaveLength(32);
        }

        [TestMethod]
        public void Should_assign_identity_to_jar_for_net461()
        {
            var t = BuildTask("net461", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().NotBeUpperCased();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().HaveLength(32);
        }

        [TestMethod]
        public void Should_assign_consistent_identity_to_jar_for_netcoreapp3_1()
        {
            var t1 = BuildTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTask("netcoreapp3.1", "0.0.0");
            var i2 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().Be(identity2);
        }

        [TestMethod]
        public void Should_assign_consistent_identity_to_jar_for_net461()
        {
            var t1 = BuildTask("net461", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTask("net461", "0.0.0");
            var i2 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().Be(identity2);
        }

        [TestMethod]
        public void Should_vary_by_tool_framework()
        {
            var t1 = BuildTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTask("net461", "0.0.0");
            var i2 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

        [TestMethod]
        public void Should_vary_by_tool_version()
        {
            var t1 = BuildTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTask("netcoreapp3.1", "0.0.1");
            var i2 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

        [TestMethod]
        public void Should_vary_by_assembly_name()
        {
            var t1 = BuildTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld1", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTask("netcoreapp3.1", "0.0.0");
            var i2 = BuildItem(HELLOWORLD_JAR, "helloworld2", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

        [TestMethod]
        public void Should_vary_by_assembly_version()
        {
            var t1 = BuildTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTask("netcoreapp3.1", "0.0.1");
            var i2 = BuildItem(HELLOWORLD_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

    }

}
