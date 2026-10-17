using System.Collections.Immutable;
using System.IO;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Decoding;
using IKVM.ByteCode.Encoding;
using IKVM.CoreLib.Modules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Modules
{

    [TestClass]
    public class ModuleDescriptorTests
    {

        [TestMethod]
        public void CanCreateModule()
        {
            var b = ModuleDescriptor.CreateModule("test");
            var m = b.Build();
            m.Name.Should().Be("test");
            m.IsAutomatic.Should().BeFalse();
            m.IsOpen.Should().BeFalse();
            m.Requires.Should().BeEmpty();
            m.Exports.Should().BeEmpty();
            m.Opens.Should().BeEmpty();
            m.Uses.Should().BeEmpty();
            m.Provides.Should().BeEmpty();
        }

        [TestMethod]
        public void CanReadModuleDescriptor()
        {
            var b = new ClassFileBuilder(new ClassFormatVersion(53, 0), AccessFlag.Module, "module-info", null);
            b.Attributes.Module(
                b.Constants.GetOrAddModule("org.test"),
                default,
                b.Constants.GetOrAddUtf8("1.2.3"),
                a => a.Requires(b.Constants.GetOrAddModule("java.base"), ModuleRequiresFlag.Mandated, Utf8ConstantHandle.Nil),
                static a => { },
                static a => { },
                static a => { },
                static a => { });

            var l = new BlobBuilder();
            b.Serialize(l);
            var m = new MemoryStream();
            l.WriteContentTo(m);
            m.Position = 0;

            using var cf = ClassFile.Read(m);
            var md = ModuleDescriptor.Read(cf);
            md.IsOpen.Should().BeFalse();
            md.IsAutomatic.Should().BeFalse();
            md.Name.Should().Be("org.test");
            md.Version.Should().Be(ModuleVersion.Parse("1.2.3"));
        }

        [TestMethod]
        public void CompareNamesOrdinally()
        {
            // a culture-sensitive comparison puts "a" before "B", Java's String.compareTo puts "B" first
            var b = ModuleDescriptor.CreateModule("B").Build();
            var a = ModuleDescriptor.CreateModule("a").Build();
            b.CompareTo(a).Should().BeNegative();
            a.CompareTo(b).Should().BePositive();
        }

        [TestMethod]
        public void CompareDescriptorsWithTheSameName()
        {
            var b = ModuleDescriptor.CreateModule("m").Exports("p", ImmutableHashSet.Create("B")).Build();
            var a = ModuleDescriptor.CreateModule("m").Exports("p", ImmutableHashSet.Create("a")).Build();
            b.CompareTo(a).Should().BeNegative();
            a.CompareTo(b).Should().BePositive();
            b.CompareTo(ModuleDescriptor.CreateModule("m").Exports("p", ImmutableHashSet.Create("B")).Build()).Should().Be(0);
        }

        [TestMethod]
        public void CompareExportsAndOpensTargetsOrdinally()
        {
            new ModuleExports(default, "p", ImmutableHashSet.Create("B")).CompareTo(new ModuleExports(default, "p", ImmutableHashSet.Create("a"))).Should().BeNegative();
            new ModuleOpens(default, "p", ImmutableHashSet.Create("B")).CompareTo(new ModuleOpens(default, "p", ImmutableHashSet.Create("a"))).Should().BeNegative();
        }

        [TestMethod]
        public void CompareProvidersLexicographically()
        {
            new ModuleProvides("s", ImmutableArray.Create("b")).CompareTo(new ModuleProvides("s", ImmutableArray.Create("a", "c"))).Should().BePositive();
            new ModuleProvides("s", ImmutableArray.Create("a")).CompareTo(new ModuleProvides("s", ImmutableArray.Create("a", "c"))).Should().BeNegative();
            new ModuleProvides("s", ImmutableArray.Create("B")).CompareTo(new ModuleProvides("s", ImmutableArray.Create("a"))).Should().BeNegative();
        }

    }

}
