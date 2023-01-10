using System.IO;
using System.IO.Compression;

using FluentAssertions;

using IKVM.Runtime.Vfs;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Vfs
{

    [TestClass]
    public class VfsTableTests
    {

        [TestMethod]
        public void CanGetDirectoryForVfsRoot()
        {
            VfsTable.Default.GetPath(VfsTable.RootPath).Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void CanGetAssemblyDirectoryByPath()
        {
            VfsTable.Default.GetPath(Path.Combine(VfsTable.RootPath, "assembly")).Should().BeAssignableTo<VfsAssemblyDirectory>();
        }

        [TestMethod]
        public void CanGetAssemblyDirectory()
        {
            VfsTable.Default.GetPath(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly)).Should().BeAssignableTo<VfsDirectory>();
            VfsTable.Default.GetPath(VfsTable.Default.GetAssemblyResourcesPath(typeof(global::java.lang.Object).Assembly)).Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void CanListAssemblyClassesDirectory()
        {
            var dir = (VfsDirectory)VfsTable.Default.GetPath(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly));
            var lst = dir.List();
            lst.Should().NotContain("");
        }

        [TestMethod]
        public void CanGetClassFileForStandardClass()
        {
            var f1 = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly), "java", "lang", "Object.class"));
            f1.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f1).Size.Should().BeGreaterThan(1);
            ((VfsFile)f1).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);

            var f2 = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.String).Assembly), "java", "lang", "String.class"));
            f2.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f2).Size.Should().BeGreaterThan(1);
            ((VfsFile)f2).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);
        }

        [TestMethod]
        public void CanGetResourcesJar()
        {
            var f1 = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyResourcesPath(typeof(global::java.lang.Object).Assembly), "resources.jar"));
            f1.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f1).Size.Should().BeGreaterThan(1);
            ((VfsFile)f1).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);
        }

        [TestMethod]
        public void CanReadManifestFromResourcesJar()
        {
            var fil = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyResourcesPath(typeof(global::java.lang.Object).Assembly), "resources.jar"));
            var stm = ((VfsFile)fil).Open(FileMode.Open, FileAccess.Read);
            var zip = new ZipArchive(stm);
            var man = zip.GetEntry("META-INF/MANIFEST.MF");
            man.Should().NotBeNull();
            new StreamReader(man.Open()).ReadToEnd();
        }

        [TestMethod]
        public void CanGetClassFileForNestedClass()
        {
            var file = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.Character).Assembly), "java", "lang", "Character$Subset.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanGetClassFileForAnonymousClass()
        {
            var file = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.ClassLoader).Assembly), "java", "lang", "ClassLoader$1.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanGetClassFileForInnerClassOfInterface()
        {
            var file = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.CharSequence).Assembly), "java", "lang", "CharSequence$1CharIterator.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanGetClassFileForNestedClassOfInterface()
        {
            var file = VfsTable.Default.GetPath(Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::javax.tools.JavaFileObject).Assembly), "javax", "tools", "JavaFileObject$Kind.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

    }

}
