using System.IO;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Runtime;
using IKVM.Runtime.Vfs;

using java.io;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.io
{

    [TestClass]
    public class FileTests
    {

        [TestMethod]
        public void CanConvertRelativePathToRealPath()
        {
            (new global::java.io.File(".")).toPath().toRealPath();
        }

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
        public void CanSetReadOnly()
        {
            var f = new global::java.io.File(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
            f.createNewFile().Should().BeTrue();
            f.canWrite().Should().BeTrue();
            f.setReadOnly().Should().BeTrue();
            f.canWrite().Should().BeFalse();
        }

        [TestMethod]
        public void ShouldRemoveDotFromCanonicalizedPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                new global::java.io.File(@"C:\Windows\.\System32").getCanonicalPath().Should().Be(@"C:\Windows\System32");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                new global::java.io.File(@"/usr/./lib").getCanonicalPath().Should().Be(@"/usr/lib");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                new global::java.io.File(@"/usr/./lib").getCanonicalPath().Should().Be(@"/usr/lib");
        }

        [TestMethod]
        public void ShouldRemoveDotDotDotFromCanonicalizedPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                new global::java.io.File(@"C:\Windows\..\Windows\System32").getCanonicalPath().Should().Be(@"C:\Windows\System32");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                new global::java.io.File(@"/usr/../usr/lib").getCanonicalPath().Should().Be(@"/usr/lib");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                new global::java.io.File(@"/usr/../usr/lib").getCanonicalPath().Should().Be(@"/usr/lib");
        }

        [TestMethod]
        public void CanGetPathSeparator()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                global::java.io.File.pathSeparator.Should().Be(";");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                global::java.io.File.pathSeparator.Should().Be(":");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                global::java.io.File.pathSeparator.Should().Be(":");
        }

        [TestMethod]
        public void VfsAssemblyClassesDirectoryShouldBeDirectory()
        {
            var d = VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(object).Assembly, JVM.Properties.HomePath);
            new global::java.io.File(d).isDirectory().Should().BeTrue();
        }

        [TestMethod]
        public void VfsAssemblyClassesDirectoryCanBeListed()
        {
            var d = VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(object).Assembly, JVM.Properties.HomePath);
            var l = new global::java.io.File(d).list();
            foreach (var s in l)
                System.Console.WriteLine(s);
        }

        [TestMethod]
        public void VfsAssemblyClassCanBeRead()
        {
            var f = System.IO.Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "java", "lang", "Class.class");
            var s = new global::java.io.FileInputStream(f);
            var b = new byte[new global::java.io.File(f).length()];
            s.read(b);
            b.Length.Should().BeGreaterThanOrEqualTo(32);
        }

        [TestMethod]
        [ExpectedException(typeof(global::java.io.FileNotFoundException))]
        public void VfsAssemblyClassCanNotBeWritten()
        {
            var f = System.IO.Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "java", "lang", "Class.class");
            new global::java.io.FileOutputStream(f).write(0);
        }

    }

}
