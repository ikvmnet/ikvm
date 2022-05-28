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
        public void Should_return_directory_for_home_path()
        {
            VfsTable.Default.GetPath(VfsTable.HomePath).Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void Should_return_directory_for_assembly_path()
        {
            VfsTable.Default.GetPath(Path.Combine(VfsTable.HomePath, "assembly")).Should().BeAssignableTo<VfsAssemblyDirectory>();
        }

        [TestMethod]
        public void Should_return_assembly_directories()
        {
            VfsTable.Default.GetPath(VfsTable.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly)).Should().BeAssignableTo<VfsDirectory>();
            VfsTable.Default.GetPath(VfsTable.GetAssemblyResourcesPath(typeof(global::java.lang.Object).Assembly)).Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void Should_list_assembly_classes_directory()
        {
            var dir = (VfsDirectory)VfsTable.Default.GetPath(VfsTable.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly));
            var lst = dir.List();
            lst.Should().NotContain("");
        }

        [TestMethod]
        public void Should_return_class_file_for_standard_classes()
        {
            var f1 = VfsTable.Default.GetPath(Path.Combine(VfsTable.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly), "java", "lang", "Object.class"));
            f1.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f1).Size.Should().BeGreaterThan(1);
            ((VfsFile)f1).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);

            var f2 = VfsTable.Default.GetPath(Path.Combine(VfsTable.GetAssemblyClassesPath(typeof(global::java.lang.String).Assembly), "java", "lang", "String.class"));
            f2.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f2).Size.Should().BeGreaterThan(1);
            ((VfsFile)f2).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);
        }

        [TestMethod]
        public void Should_return_resource_file_for_standard_resources()
        {
            var f1 = VfsTable.Default.GetPath(Path.Combine(VfsTable.GetAssemblyResourcesPath(typeof(global::java.lang.Object).Assembly), "resources.jar"));
            f1.Should().BeAssignableTo<VfsFile>();
            ((VfsFile)f1).Size.Should().BeGreaterThan(1);
            ((VfsFile)f1).Open(FileMode.Open, FileAccess.Read).Length.Should().BeGreaterThan(1);
        }

        [TestMethod]
        public void Should_return_resource_jar_file_for_standard_resources()
        {
            var fil = VfsTable.Default.GetPath(Path.Combine(VfsTable.GetAssemblyResourcesPath(typeof(global::java.lang.Object).Assembly), "resources.jar"));
            var stm = ((VfsFile)fil).Open(FileMode.Open, FileAccess.Read);
            var zip = new ZipArchive(stm);
            var man = zip.GetEntry("META-INF/MANIFEST.MF");
            man.Should().NotBeNull();
        }

        [TestMethod]
        public void Should_return_directory_for_home_path_lib()
        {
            VfsTable.Default.GetPath(Path.Combine(VfsTable.HomePath, "lib")).Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void Should_return_directory_for_home_path_bin()
        {
            VfsTable.Default.GetPath(Path.Combine(VfsTable.HomePath, "bin")).Should().BeAssignableTo<VfsDirectory>();
        }

        [TestMethod]
        public void Should_return_tzdb_dat()
        {
            VfsTable.Default.GetPath(Path.Combine(VfsTable.HomePath, "lib", "tzdb.dat")).Should().BeAssignableTo<VfsFile>();
        }

        [TestMethod]
        public void Should_return_tzmappings()
        {
            VfsTable.Default.GetPath(Path.Combine(VfsTable.HomePath, "lib", "tzmappings")).Should().BeAssignableTo<VfsFile>();
        }

    }

}
