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
    public class IkvmReferenceItemPrepareTests
    {

        readonly static string HELLOWORLD1_JAR = @".\ext\helloworld-2.0-1\helloworld-2.0.jar";

        /// <summary>
        /// Builds a new task instance with various information.
        /// </summary>
        /// <param name="toolFramework"></param>
        /// <returns></returns>
        IkvmReferenceItemPrepare BuildTestTask(string toolFramework, string toolVersion)
        {
            var t = new IkvmReferenceItemPrepare();
            t.ToolVersion = toolVersion;
            t.TargetFramework = toolFramework;
            t.RuntimeAssembly = typeof(IKVM.Runtime.InternalException).Assembly.Location;
            t.References = new[] { new TaskItem(typeof(object).Assembly.Location) };
            t.CacheDir = Path.Combine(Path.GetTempPath(), "ikvm", "cache", "1");
            t.StageDir = Path.Combine(Path.GetTempPath(), "ikvm", "stage", "1");
            return t;
        }

        [TestMethod]
        public void Should_normalize_jar_itemspec()
        {
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1 }));
            i1.ItemSpec.Should().Be(Path.Combine("ext", "helloworld-2.0-1", "helloworld-2.0.jar"));
        }

        [TestMethod]
        public void Should_normalize_dir_itemspec()
        {
            var i1 = new TaskItem(@".\ext");
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1 }));
            i1.ItemSpec.Should().Be(@"ext" + Path.DirectorySeparatorChar);
        }

        [TestMethod]
        public void Should_not_normalize_unknown_itemspec()
        {
            var i1 = new TaskItem("itemspecvalue");
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1 }));
            i1.ItemSpec.Should().Be("itemspecvalue");
        }

        [TestMethod]
        public void Should_add_jar_identity_to_compile()
        {
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", "helloworld-2.0.jar"));
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1 }));
            var c = i1.GetMetadata(IkvmReferenceItemMetadata.Compile);
            c.Split(IkvmReferenceItemMetadata.PropertySeperatorChar).Should().Contain(Path.Combine("ext", "helloworld-2.0-1", "helloworld-2.0.jar"));
        }

        [TestMethod]
        public void Should_not_add_dir_identity_to_compile()
        {
            var i1 = new TaskItem(Path.Combine(".", "ext"));
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1 }));
            var c = i1.GetMetadata(IkvmReferenceItemMetadata.Compile);
            c.Split(IkvmReferenceItemMetadata.PropertySeperatorChar).Should().NotContain(@"ext");
        }

        [TestMethod]
        public void Should_resolve_reference()
        {
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", "helloworld-2.0.jar"));
            var i2 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-2", "helloworld-2.0.jar"));
            i2.SetMetadata(IkvmReferenceItemMetadata.References, Path.Combine(".", "ext", "helloworld-2.0-1", "helloworld-2.0.jar"));
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1, i2 }));
            var c = i2.GetMetadata(IkvmReferenceItemMetadata.References);
            c.Split(IkvmReferenceItemMetadata.PropertySeperatorChar).Should().Contain(Path.Combine("ext", "helloworld-2.0-1", "helloworld-2.0.jar"));
        }

        [TestMethod]
        public void Should_get_default_assembly_name_from_jar()
        {
            var i1 = new TaskItem(HELLOWORLD1_JAR);
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1 }));
            var c = i1.GetMetadata(IkvmReferenceItemMetadata.AssemblyName);
            c.Should().Be("helloworld");
        }

        [TestMethod]
        public void Should_get_default_assembly_version_from_jar()
        {
            var i1 = new TaskItem(HELLOWORLD1_JAR);
            IkvmReferenceItemPrepare.AssignMetadata(IkvmReferenceItemUtil.Import(new[] { i1 }));
            var c = i1.GetMetadata(IkvmReferenceItemMetadata.AssemblyVersion);
            c.Should().Be("2.0.0.0");
        }

        [TestMethod]
        public void Should_fail_when_missing_name()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemPrepare();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "2.0");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            t.Items = new[] { i1 };
            t.Validate(IkvmReferenceItemUtil.Import(t.Items)).Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0011");
        }

        [TestMethod]
        public void Should_fail_when_missing_version()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemPrepare();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            t.Items = new[] { i1 };
            t.Validate(IkvmReferenceItemUtil.Import(t.Items)).Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0012");
        }

        [TestMethod]
        public void Should_fail_when_missing_file_version()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemPrepare();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            t.Items = new[] { i1 };
            t.Validate(IkvmReferenceItemUtil.Import(t.Items)).Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0013");
        }

        [TestMethod]
        public void Should_fail_when_invalid_version()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemPrepare();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "invalid");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            t.Items = new[] { i1 };
            t.Validate(IkvmReferenceItemUtil.Import(t.Items)).Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0003");
        }

        [TestMethod]
        public void Should_fail_when_missing_compile()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemPrepare();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(@"missing");
            t.Items = new[] { i1 };
            t.Validate(IkvmReferenceItemUtil.Import(t.Items)).Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0011");
            errors.Should().Contain(x => x.Code == "IKVMSDK0012");
            errors.Should().Contain(x => x.Code == "IKVMSDK0013");
            errors.Should().Contain(x => x.Code == "IKVMSDK0010");
        }

        [TestMethod]
        public void Should_fail_when_invalid_sources()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemPrepare();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "2.0");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.Sources, "invalid");
            t.Items = new[] { i1 };
            t.Validate(IkvmReferenceItemUtil.Import(t.Items)).Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0007");
        }

        [TestMethod]
        public void Should_fail_when_missing_sources()
        {
            var engine = new Mock<IBuildEngine>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var t = new IkvmReferenceItemPrepare();
            t.BuildEngine = engine.Object;
            var i1 = new TaskItem(Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "2.0");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, Path.Combine(".", "ext", "helloworld-2.0-1", ".", "helloworld-2.0.jar"));
            i1.SetMetadata(IkvmReferenceItemMetadata.Sources, "missing.java");
            t.Items = new[] { i1 };
            t.Validate(IkvmReferenceItemUtil.Import(t.Items)).Should().BeFalse();
            errors.Should().Contain(x => x.Code == "IKVMSDK0009");
        }

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

        [TestMethod]
        public void Should_assign_identity_to_jar_for_netcoreapp3_1()
        {
            var t = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().NotBeUpperCased();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().HaveLength(32);
        }

        [TestMethod]
        public void Should_assign_identity_to_jar_for_net461()
        {
            var t = BuildTestTask("net461", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t.Items = new[] { i1 };
            t.Execute().Should().BeTrue();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().NotBeUpperCased();
            i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity).Should().HaveLength(32);
        }

        [TestMethod]
        public void Should_assign_consistent_identity_to_jar_for_netcoreapp3_1()
        {
            var t1 = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i2 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().Be(identity2);
        }

        [TestMethod]
        public void Should_assign_consistent_identity_to_jar_for_net461()
        {
            var t1 = BuildTestTask("net461", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTestTask("net461", "0.0.0");
            var i2 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().Be(identity2);
        }

        [TestMethod]
        public void Should_vary_by_tool_framework()
        {
            var t1 = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTestTask("net461", "0.0.0");
            var i2 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

        [TestMethod]
        public void Should_vary_by_tool_version()
        {
            var t1 = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTestTask("netcoreapp3.1", "0.0.1");
            var i2 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

        [TestMethod]
        public void Should_vary_by_assembly_name()
        {
            var t1 = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld1", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i2 = BuildItem(HELLOWORLD1_JAR, "helloworld2", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

        [TestMethod]
        public void Should_vary_by_assembly_version()
        {
            var t1 = BuildTestTask("netcoreapp3.1", "0.0.0");
            var i1 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t1.Items = new[] { i1 };
            t1.Execute().Should().BeTrue();

            var t2 = BuildTestTask("netcoreapp3.1", "0.0.1");
            var i2 = BuildItem(HELLOWORLD1_JAR, "helloworld", "0.0.0.0");
            t2.Items = new[] { i2 };
            t2.Execute().Should().BeTrue();

            var identity1 = i1.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            var identity2 = i2.GetMetadata(IkvmReferenceItemMetadata.IkvmIdentity);
            identity1.Should().NotBe(identity2);
        }

        [TestMethod]
        public void Should_sort_by_reference_order()
        {
            var t = BuildTestTask("netcoreapp3.1", "0.0.0");

            var i1 = new TaskItem("helloworld1");
            i1.SetMetadata(IkvmReferenceItemMetadata.Compile, HELLOWORLD1_JAR);
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld1");
            i1.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "0.0.0.0");
            var i2 = new TaskItem("helloworld2");
            i2.SetMetadata(IkvmReferenceItemMetadata.Compile, HELLOWORLD1_JAR);
            i2.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld2");
            i2.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "0.0.0.0");
            i2.SetMetadata(IkvmReferenceItemMetadata.References, "helloworld1");
            var i3 = new TaskItem("helloworld3");
            i3.SetMetadata(IkvmReferenceItemMetadata.Compile, HELLOWORLD1_JAR);
            i3.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, "helloworld3");
            i3.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, "0.0.0.0");
            i3.SetMetadata(IkvmReferenceItemMetadata.References, "helloworld2");
            t.Items = new[] { i2, i3, i1 };
            t.Execute().Should().BeTrue();
            t.Items[0].Should().Be(i1);
            t.Items[1].Should().Be(i2);
            t.Items[2].Should().Be(i3);
        }

    }

}
