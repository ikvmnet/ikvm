using System.IO;
using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.io
{

    [TestClass]
    public class FileTests
    {

        [TestMethod]
        public void CanCreateFile()
        {
            var f = new global::java.io.File(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
            f.createNewFile().Should().BeTrue();
        }

        [TestMethod]
        public void CanWriteFile()
        {
            var w = new global::java.io.FileWriter("test.txt");
            w.write("TEST");
            w.close();
        }

        [TestMethod]
        public void CanReadFile()
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
        public void ShouldRemoveDotFromCanonicalizedPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                new global::java.io.File(@"C:\Windows\.\System32").getCanonicalPath().Should().Be(@"C:\Windows\System32");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                new global::java.io.File(@"/usr/./lib").getCanonicalPath().Should().Be(@"/usr/lib");
        }

        [TestMethod]
        public void ShouldRemoveDotDotDotFromCanonicalizedPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                new global::java.io.File(@"C:\Windows\..\Windows\System32").getCanonicalPath().Should().Be(@"C:\Windows\System32");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                new global::java.io.File(@"/usr/../usr/lib").getCanonicalPath().Should().Be(@"/usr/lib");
        }

    }

}
