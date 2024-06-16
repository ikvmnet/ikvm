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
        public void CanPerformMoveReplace()
        {
            var source = global::System.IO.Path.GetTempFileName();
            var target = global::System.IO.Path.GetTempFileName();
            Files.write(Paths.get(source), new byte[] { 1, 2, 3, 4 }, StandardOpenOption.WRITE);
            Files.move(Paths.get(source), Paths.get(target), new[] { StandardCopyOption.REPLACE_EXISTING });
            global::System.IO.File.Exists(source).Should().BeFalse();
            global::System.IO.File.Exists(target).Should().BeTrue();
            var b = System.IO.File.ReadAllBytes(target);
            b.Should().HaveCount(4);
            b[0].Should().Be(1);
            b[1].Should().Be(2);
            b[2].Should().Be(3);
            b[3].Should().Be(4);
        }

        [TestMethod]
        public void CanPerformAtomicMoveReplace()
        {
            var source = global::System.IO.Path.GetTempFileName();
            var target = global::System.IO.Path.GetTempFileName();
            Files.write(Paths.get(source), new byte[] { 1, 2, 3, 4 }, StandardOpenOption.WRITE);
            Files.move(Paths.get(source), Paths.get(target), new[] { StandardCopyOption.ATOMIC_MOVE, StandardCopyOption.REPLACE_EXISTING });
            global::System.IO.File.Exists(source).Should().BeFalse();
            global::System.IO.File.Exists(target).Should().BeTrue();
            var b = System.IO.File.ReadAllBytes(target);
            b.Should().HaveCount(4);
            b[0].Should().Be(1);
            b[1].Should().Be(2);
            b[2].Should().Be(3);
            b[3].Should().Be(4);
        }

    }

}
