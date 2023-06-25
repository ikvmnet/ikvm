using System;
using System.IO;

using java.io;
using java.util.jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.jar
{

    [TestClass]
    public class Pack200Tests
    {

        [TestMethod]
        public void CanPackJar()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            var packFilePath = new global::java.io.File(Path.Combine(dir, "helloworld.pack"));
            var testFilePath = new global::java.io.File(Path.Combine(dir, "helloworld.jar"));

            // pack JAR file into pack file
            var packer = Pack200.newPacker();
            var jarFile = new JarFile(Path.Combine("helloworld", "helloworld-2.0.jar"));
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
        }

    }

}
