using System.IO;
using System.IO.Compression;

using FluentAssertions;

using IKVM.Runtime;
using IKVM.Runtime.Vfs;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Vfs
{

    [TestClass]
    public class VfsTableTests
    {

        [TestMethod]
        public void CanGetAssemblyDirectory()
        {
            JVM.Vfs.GetEntry(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath)).Should().BeAssignableTo<VfsDirectory>();
            JVM.Vfs.GetEntry(VfsTable.GetAssemblyResourcesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath)).Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void CanListAssemblyClassesDirectory()
        {
            var dir = (VfsDirectory)JVM.Vfs.GetEntry(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath));
            var lst = dir.List();
            lst.Should().NotContain("");
        }

        [TestMethod]
        public void CanGetClassFileForStandardClass()
        {
            var f1 = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "java", "lang", "Object.class"));
            f1.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f1).Size.Should().BeGreaterThan(1);
            ((VfsFile)f1).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);

            var f2 = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.String).Assembly, JVM.Properties.HomePath), "java", "lang", "String.class"));
            f2.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f2).Size.Should().BeGreaterThan(1);
            ((VfsFile)f2).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);
        }

        [TestMethod]
        public void CanGetResourcesJar()
        {
            var f1 = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyResourcesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "resources.jar"));
            f1.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f1).Size.Should().BeGreaterThan(1);
            ((VfsFile)f1).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);
        }

        [TestMethod]
        public void CanReadManifestFromResourcesJar()
        {
            var fil = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyResourcesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "resources.jar"));
            var stm = ((VfsFile)fil).Open(FileMode.Open, FileAccess.Read);
            var zip = new ZipArchive(stm);
            var man = zip.GetEntry("META-INF/MANIFEST.MF");
            man.Should().NotBeNull();
            new StreamReader(man.Open()).ReadToEnd();
        }

        [TestMethod]
        public void CanGetClassFileForNestedClass()
        {
            var file = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Character).Assembly, JVM.Properties.HomePath), "java", "lang", "Character$Subset.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanGetClassFileForAnonymousClass()
        {
            var file = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.ClassLoader).Assembly, JVM.Properties.HomePath), "java", "lang", "ClassLoader$1.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanGetClassFileForInnerClassOfInterface()
        {
            var file = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.CharSequence).Assembly, JVM.Properties.HomePath), "java", "lang", "CharSequence$1CharIterator.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

        [TestMethod]
        public void CanGetClassFileForNestedClassOfInterface()
        {
            var file = JVM.Vfs.GetEntry(Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::javax.tools.JavaFileObject).Assembly, JVM.Properties.HomePath), "javax", "tools", "JavaFileObject$Kind.class"));
            file.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)file).Size.Should().BeGreaterThan(1);
            new BinaryReader(((VfsFile)file).Open(FileMode.Open, FileAccess.Read)).ReadUInt32().Should().Be(0xBEBAFECA);
        }

    }

}
