using System.IO;

using FluentAssertions;

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
            var b = new ClassFileBuilder(new ByteCode.ClassFormatVersion(53, 0), ByteCode.AccessFlag.Module, "module-info", null);
            b.Attributes.Module(
                b.Constants.GetOrAddModule("org.test"),
                default,
                b.Constants.GetOrAddUtf8("1.2.3"),
                static a => { },
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

    }

}
