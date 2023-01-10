using System;
using System.IO;

using FluentAssertions;

using IKVM.Runtime.Vfs;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Vfs
{

    [TestClass]
    public class VfsAssemblyDirectoryTests
    {

        [TestMethod]
        public void CanGetAssemblyDirectory()
        {
            var ctx = VfsRuntimeContext.Instance;
            var dir = new VfsAssemblyDirectory(ctx);
            var asm = typeof(VfsAssemblyDirectoryTests).Assembly.GetName();
            var ent = dir.GetEntry($"{asm.Name}__{asm.Version}__{BitConverter.ToString(asm.GetPublicKeyToken()).ToLower().Replace("-", "")}") as VfsDirectory;
            ent.Should().NotBeNull();
            ent.List().Should().HaveCount(2);
            ent.GetEntry("classes").Should().BeAssignableTo<VfsDirectory>();
            ent.GetEntry("resources").Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void CanGetAssemblyResource()
        {
            var ctx = VfsRuntimeContext.Instance;
            var dir = new VfsAssemblyDirectory(ctx);
            var asm = typeof(VfsAssemblyDirectoryTests).Assembly.GetName();
            var ent = dir.GetEntry($"{asm.Name}__{asm.Version}__{BitConverter.ToString(asm.GetPublicKeyToken()).ToLower().Replace("-", "")}") as VfsDirectory;
            ent.Should().NotBeNull();
            ent.List().Should().HaveCount(2);
            var res = ent.GetEntry("resources") as VfsDirectory;
            res.Should().NotBeNull();
            var rsx = res.GetEntry("IKVM.Tests.Runtime.Vfs.VfsTestResource.txt") as VfsFile;
            rsx.Should().NotBeNull();
            new StreamReader(rsx.Open(FileMode.Open, FileAccess.Read)).ReadLine().Should().Be("TEST");
        }

        [TestMethod]
        public void CanFindClassFileInPackage()
        {
            var ctx = VfsRuntimeContext.Instance;
            var dir = new VfsAssemblyClassDirectory(ctx, typeof(global::java.lang.Object).Assembly, "java.lang");
            var ent = dir.GetEntry("Object.class");
            ent.Should().NotBeNull();
            ent.Should().BeOfType<VfsAssemblyClassFile>();
            new BinaryReader(((VfsFile)ent).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanFindClassFileForNestedClass()
        {
            var ctx = VfsRuntimeContext.Instance;
            var dir = new VfsAssemblyClassDirectory(ctx, typeof(global::java.lang.Character).Assembly, "java.lang");
            var ent = dir.GetEntry("Character$Subset.class");
            ent.Should().NotBeNull();
            ent.Should().BeOfType<VfsAssemblyClassFile>();
            new BinaryReader(((VfsFile)ent).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanFindClassFileForAnonymousClass()
        {
            var ctx = VfsRuntimeContext.Instance;
            var dir = new VfsAssemblyClassDirectory(ctx, typeof(global::java.lang.ClassLoader).Assembly, "java.lang");
            var ent = dir.GetEntry("ClassLoader$1.class");
            ent.Should().NotBeNull();
            ent.Should().BeOfType<VfsAssemblyClassFile>();
            new BinaryReader(((VfsFile)ent).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanFindClassFileForInnerClassOfInterface()
        {
            var ctx = VfsRuntimeContext.Instance;
            var dir = new VfsAssemblyClassDirectory(ctx, typeof(global::java.lang.CharSequence).Assembly, "java.lang");
            var ent = dir.GetEntry("CharSequence$1CharIterator.class");
            ent.Should().NotBeNull();
            ent.Should().BeOfType<VfsAssemblyClassFile>();
            new BinaryReader(((VfsFile)ent).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanFindClassFileForNestedClassOfInterface()
        {
            var ctx = VfsRuntimeContext.Instance;
            var dir = new VfsAssemblyClassDirectory(ctx, typeof(global::javax.tools.JavaFileObject).Assembly, "javax.tools");
            var ent = dir.GetEntry("JavaFileObject$Kind.class");
            ent.Should().NotBeNull();
            ent.Should().BeOfType<VfsAssemblyClassFile>();
            new BinaryReader(((VfsFile)ent).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

    }

}
