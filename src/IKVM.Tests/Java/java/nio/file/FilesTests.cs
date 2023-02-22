using FluentAssertions;

using IKVM.Java.Externs.java.io;
using IKVM.Runtime.Vfs;

using java.nio.file;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.file
{

    [TestClass]
    public class FilesTests
    {

        [TestMethod]
        public void CanReadAllBytes()
        {
            var f = global::System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllBytes(f, new byte[] { 1 });
            var b = Files.readAllBytes(Paths.get(f));
            b.Should().HaveCount(1);
            b[0].Should().Be(1);
        }

        [TestMethod]
        public void CanWriteAllBytes()
        {
            var f = global::System.IO.Path.GetTempFileName();
            Files.write(Paths.get(f), new byte[] { 1 }, StandardOpenOption.WRITE);
            var b = System.IO.File.ReadAllBytes(f);
            b.Should().HaveCount(1);
            b[0].Should().Be(1);
        }

        [TestMethod]
        public void CanGetSize()
        {
            var f = global::System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllBytes(f, new byte[] { 1 });
            var s = Files.size(Paths.get(f));
            s.Should().Be(1);
        }

        [TestMethod]
        public void CanGetIsDirectory()
        {
            var d = System.IO.Path.GetTempPath();
            Files.isDirectory(Paths.get(d)).Should().Be(true);
            var f = System.IO.Path.GetTempFileName();
            Files.isDirectory(Paths.get(f)).Should().Be(false);
        }

        [TestMethod]
        public void VfsAssemblyClassesDirectoryShouldBeDirectory()
        {
            var d = VfsTable.Default.GetAssemblyClassesPath(typeof(object).Assembly);
            Files.isDirectory(Paths.get(d)).Should().Be(true);
        }

        [TestMethod]
        public void VfsAssemblyClassesDirectoryCanBeListed()
        {
            var d = VfsTable.Default.GetAssemblyClassesPath(typeof(object).Assembly);
            var l = Files.list(Paths.get(d)).toArray();
            foreach (string s in l)
                System.Console.WriteLine(s);
        }

        [TestMethod]
        public void VfsAssemblyClassCanBeRead()
        {
            var f = System.IO.Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly), "java", "lang", "Object.class");
            var b = Files.readAllBytes(Paths.get(f));
            b.Length.Should().BeGreaterOrEqualTo(32);
        }

        [TestMethod]
        [ExpectedException(typeof(global::java.nio.file.AccessDeniedException))]
        public void VfsAssemblyClassCanNotBeWritten()
        {
            var f = System.IO.Path.Combine(VfsTable.Default.GetAssemblyClassesPath(typeof(global::java.lang.Object).Assembly), "java", "lang", "Object.class");
            Files.write(Paths.get(f), new byte[] { 1 });
        }

    }

}
