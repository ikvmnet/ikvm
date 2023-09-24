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

            using (var raf = new RandomAccessFile(new File(p), "r"))
            {
                var buf = new byte[128];
                int n;

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

            System.IO.File.Delete(p);
        }

        [TestMethod]
        public void CanLock()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            using (var r = new RandomAccessFile(p, "rw"))
            using (var l = r.getChannel().@lock())
            {
                l.Should().NotBeNull();
                l.isValid().Should().BeTrue();
                l.isShared().Should().BeFalse();
                l.position().Should().Be(0);
                l.size().Should().Be(long.MaxValue);
            }

            System.IO.File.Delete(p);
        }

        [TestMethod]
        public void CanLockRangeExclusive()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            using (var r = new RandomAccessFile(p, "rw"))
            using (var l = r.getChannel().@lock(0, 512, false))
            {
                l.Should().NotBeNull();
                l.isValid().Should().BeTrue();
                l.isShared().Should().BeFalse();
                l.position().Should().Be(0);
                l.size().Should().Be(512);
            }

            System.IO.File.Delete(p);
        }

        [TestMethod]
        public void CanLockRangeShared()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            using (var r = new RandomAccessFile(p, "rw"))
            using (var l = r.getChannel().@lock(0, 512, true))
            {
                l.Should().NotBeNull();
                l.isValid().Should().BeTrue();
                l.isShared().Should().BeTrue();
                l.position().Should().Be(0);
                l.size().Should().Be(512);
            }

            System.IO.File.Delete(p);
        }

        [TestMethod]
        public void CanSetLength()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            using (var r = new RandomAccessFile(p, "rw"))
            {
                r.setLength(2048);
                r.length().Should().Be(2048, "length should have increased");
                r.getFilePointer().Should().Be(0, "getFilePointer should not have advanced to the end of the stream");
            }

            new System.IO.FileInfo(p).Length.Should().Be(2048, "length of the .NET file should correspond to Java");
            System.IO.File.Delete(p);

        }

        [TestMethod]
        public void SetLengthShouldThrowOnReadOnly()
        {
            // generate temporary file
            var p = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var c = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(c);
            System.IO.File.WriteAllBytes(p, c);

            using (var r = new RandomAccessFile(p, "r"))
            {
                r.Invoking(s => s.setLength(2048)).Should().Throw<IOException>();
                r.length().Should().Be(1024, "length should not have increased");
            }

            new System.IO.FileInfo(p).Length.Should().Be(1024, "length of the .NET file should not have changed after failure");
        }

    }

}
