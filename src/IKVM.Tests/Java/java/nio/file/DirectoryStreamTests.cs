using System;

using FluentAssertions;

using java.io;
using java.nio.file;
using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.file
{

    [TestClass]
    public class DirectoryStreamTests
    {

        [TestMethod]
        public void ShouldListFilesAtAbsolutePath()
        {
            var d = Files.createTempDirectory("test");
            d.toFile().deleteOnExit();

            var f1 = new File(d.toFile(), "test1.txt");
            f1.createNewFile();
            var f2 = new File(d.toFile(), "test2.txt");
            f2.createNewFile();
            var f3 = new File(d.toFile(), "test3.txt");
            f3.createNewFile();

            using var stream = Files.newDirectoryStream(d);
            var l = stream.iterator().RemainingToList<Path>();
            l.Should().HaveCount(3);
            l.Should().Contain(i => i.ToString() == f1.ToString());
            l.Should().Contain(i => i.ToString() == f2.ToString());
            l.Should().Contain(i => i.ToString() == f3.ToString());
        }

        [TestMethod]
        public void ShouldListFilesAtRelativePath()
        {
            var n = Guid.NewGuid().ToString("n");
            var d = Files.createDirectory(Paths.get(n));
            d.toFile().deleteOnExit();

            var f1 = new File(d.toFile(), "test1.txt");
            f1.createNewFile();
            var f2 = new File(d.toFile(), "test2.txt");
            f2.createNewFile();
            var f3 = new File(d.toFile(), "test3.txt");
            f3.createNewFile();

            using var stream = Files.newDirectoryStream(d);
            var l = stream.iterator().RemainingToList<Path>();
            l.Should().HaveCount(3);
            l.Should().Contain(i => i.ToString() == f1.ToString());
            l.Should().Contain(i => i.ToString() == f2.ToString());
            l.Should().Contain(i => i.ToString() == f3.ToString());
        }

    }

}
