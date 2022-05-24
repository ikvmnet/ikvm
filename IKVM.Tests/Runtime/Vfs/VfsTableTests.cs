
using System.IO;

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
