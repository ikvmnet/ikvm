using System.Security.Cryptography;

using FluentAssertions;

using java.io;
using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.io
{

    [TestClass]
    public class RandomAccessFileTests
    {

        [TestMethod]
        public void ShouldReturnNegativeOneAtEof()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            var buf = new byte[128];
            int n;
            using var raf = new RandomAccessFile(new File(p), "r");

            // read until end
            while (true)
            {
                n = raf.read(buf, 0, buf.Length);
                if (n <= 0)
                    break;
            }

            if (n != -1)
                throw new RuntimeException("Expected -1 for EOF, got " + n);
        }

        [TestMethod]
        public void CanLockRangeExclusive()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            var r = new RandomAccessFile(p, "rw");
            var l = r.getChannel().@lock(0, 512, false);
            l.Should().NotBeNull();
            l.isValid().Should().BeTrue();
            l.isShared().Should().BeFalse();
            l.position().Should().Be(0);
            l.size().Should().Be(512);
            l.close();
        }

        [TestMethod]
        public void CanLockRangeShared()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            var r = new RandomAccessFile(p, "rw");
            var l = r.getChannel().@lock(0, 512, true);
            l.Should().NotBeNull();
            l.isValid().Should().BeTrue();
            l.isShared().Should().BeTrue();
            l.position().Should().Be(0);
            l.size().Should().Be(512);
            l.close();
            r.close();
        }

    }

}
