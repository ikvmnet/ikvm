using System.IO;

using FluentAssertions;

using IKVM.Runtime.Vfs;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class FileTests
    {

        [TestMethod]
        public void Can_create_file()
        {
            var f = new global::java.io.File(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
            f.createNewFile().Should().BeTrue();
        }

        [TestMethod]
        public void Can_write_file()
        {
            var w = new global::java.io.FileWriter("test.txt");
            w.write("TEST");
            w.close();
        }

        [TestMethod]
        public void Can_read_file()
        {
            var w = new global::java.io.FileWriter("test.txt");
            w.write("TEST");
            w.close();

            var f = new global::java.io.File("test.txt");
            var r = new global::java.util.Scanner(f);
            r.hasNextLine().Should().BeTrue();
            r.nextLine().Should().Be("TEST");
            r.hasNextLine().Should().BeFalse();
            r.close();
        }

        [TestMethod]
        public void Can_check_if_vfs_file_exists()
        {
            new global::java.io.File(Path.Combine(VfsTable.HomePath, "lib", "tzdb.dat")).exists().Should().BeTrue();
        }

        [TestMethod]
        public void Can_check_vfs_file_length()
        {
            new global::java.io.File(Path.Combine(VfsTable.HomePath, "lib", "tzdb.dat")).length().Should().BeGreaterThan(1);
        }

    }

}
