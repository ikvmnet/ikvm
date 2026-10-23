using System;

using FluentAssertions;

using java.io;
using java.nio.file;
using java.util.jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.jar
{

    [TestClass]
    public class Pack200Tests
    {

        [TestMethod]
        public void CanPackAndUnpack()
        {
            var dir = Paths.get(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            Files.createDirectory(dir);

            var packFilePath = dir.resolve("helloworld.pack").toFile();
            var testFilePath = dir.resolve("helloworld.jar").toFile();

            // pack JAR file into pack file
            var packer = Pack200.newPacker();
            var jarFile = new JarFile(Paths.get("helloworld", "helloworld-2.0.jar").toFile());
            using var fos = new FileOutputStream(packFilePath);
            packer.pack(jarFile, fos);
            jarFile.close();
            fos.close();

            // unpack pack file into JAR file
            using var fostream = new FileOutputStream(testFilePath);
            using var jostream = new JarOutputStream(fostream);
            var unpacker = Pack200.newUnpacker();
            unpacker.unpack(packFilePath, jostream);
            jostream.close();
            Files.size(testFilePath.toPath()).Should().BeGreaterThanOrEqualTo(100);
        }

    }

}
