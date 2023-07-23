using System.Linq;

using FluentAssertions;

using IKVM.Runtime;
using IKVM.Runtime.Vfs;

using java.nio.file;
using java.nio.file.attribute;

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
            var d = VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(object).Assembly, JVM.Properties.HomePath);
            Files.isDirectory(Paths.get(d)).Should().Be(true);
        }

        [TestMethod]
        public void VfsAssemblyClassesDirectoryCanBeListed()
        {
            var d = VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(object).Assembly, JVM.Properties.HomePath);
            var l = Files.list(Paths.get(d)).toArray();
            foreach (Path s in l)
                System.Console.WriteLine(s);
        }

        [TestMethod]
        public void VfsAssemblyClassCanBeRead()
        {
            var f = System.IO.Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "java", "lang", "Class.class");
            var b = Files.readAllBytes(Paths.get(f));
            b.Length.Should().BeGreaterOrEqualTo(32);
        }

        [TestMethod]
        [ExpectedException(typeof(global::java.nio.file.AccessDeniedException))]
        public void VfsAssemblyClassCanNotBeWritten()
        {
            var f = System.IO.Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "java", "lang", "Class.class");
            Files.write(Paths.get(f), new byte[] { 1 });
        }

        [TestMethod]
        public void VfsAssemblyClassesDirectoryShouldHaveBasicAttributes()
        {
            var f = VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath);
            var a = (BasicFileAttributes)Files.readAttributes(Paths.get(f), typeof(BasicFileAttributes));
            a.isDirectory().Should().BeTrue();
        }

        [TestMethod]
        public void VfsAssemblyClassesDirectoryShouldBeWalkable()
        {
            var f = VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath);
            var l = Files.walk(Paths.get(f)).toArray();
            l.Count().Should().BeGreaterOrEqualTo(16);
        }

    }

}
