using System;
using System.IO;

using FluentAssertions;

using IKVM.CoreLib.Jar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Jar
{

    [TestClass]
    public class JarFileTests
    {

        static string HelloWorldJarPath => Path.Combine("res", "helloworld", "helloworld-2.0.jar");

        [TestMethod]
        public void CanReadManifestVersion()
        {
            var z = new JarFile(HelloWorldJarPath);
            z.Manifest.MainAttributes.Should().Contain("Manifest-Version", "1.0");
        }

        [TestMethod]
        public void DisposeShouldCloseOwnedStream()
        {
            var stream = File.OpenRead(HelloWorldJarPath);

            using (var jar = new JarFile(stream))
                jar.Manifest.Should().NotBeNull();

            stream.CanRead.Should().BeFalse();
        }

        [TestMethod]
        public void DisposeShouldNotCloseUnownedStream()
        {
            using var stream = File.OpenRead(HelloWorldJarPath);

            using (var jar = new JarFile(stream, false))
                jar.Manifest.Should().NotBeNull();

            stream.CanRead.Should().BeTrue();
        }

        [TestMethod]
        public void DisposeShouldReleaseFileOpenedByPath()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".jar");
            File.Copy(HelloWorldJarPath, path);

            try
            {
                using (var jar = new JarFile(path))
                    jar.Manifest.Should().NotBeNull();

                // nothing may still hold the file: an outstanding FileShare.Read handle prevents
                // another process from replacing or deleting the JAR, which on Windows survives
                // the build itself when the MSBuild node is reused
                Action open = () => File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None).Dispose();
                open.Should().NotThrow();
            }
            finally
            {
                // best effort: a failing assertion above means the file is still held
                try { File.Delete(path); } catch (IOException) { }
            }
        }

    }

}
