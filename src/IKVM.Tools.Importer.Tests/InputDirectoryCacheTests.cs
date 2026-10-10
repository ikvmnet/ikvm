using System;
using System.IO;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Importer.Tests
{

    [TestClass]
    public class InputDirectoryCacheTests
    {

        static readonly string[] Names =
        [
            "Foo.class",
            "foo.class",
            "FOO.CLASS",
            "Bar.java",
            "bar.JAVA",
            "Missing.class",
            "Sub.class",
            "Foo",
            "Foo.class.",
            "Foo.class ",
            "*.class",
            "Foo.cl?ss",
            "Foo.class*",
            ".",
            "..",
            "",
        ];

        string directory;

        [TestInitialize]
        public void Setup()
        {
            directory = Path.Combine(Path.GetTempPath(), "InputDirectoryCacheTests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "Foo.class"), "");
            File.WriteAllText(Path.Combine(directory, "Bar.java"), "");
            File.WriteAllText(Path.Combine(directory, "Other.class"), "");
            Directory.CreateDirectory(Path.Combine(directory, "Sub.class"));

            var hidden = Path.Combine(directory, "Hidden.class");
            File.WriteAllText(hidden, "");
            File.SetAttributes(hidden, FileAttributes.Hidden);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var hidden = Path.Combine(directory, "Hidden.class");
            if (File.Exists(hidden))
                File.SetAttributes(hidden, FileAttributes.Normal);

            Directory.Delete(directory, true);
        }

        static string[] GetFiles(string directory, string name)
        {
            try
            {
                return Directory.GetFiles(directory, name);
            }
            catch
            {
                return null;
            }
        }

        [TestMethod]
        public void TryGetFilesMatchesDirectoryGetFiles()
        {
            var cache = new InputDirectoryCache();
            foreach (var name in Names)
            {
                var expected = GetFiles(directory, name);
                var actual = cache.TryGetFiles(directory, name);

                // the cache either answers exactly like the file system, or declines and leaves it to the caller
                if (actual != null)
                    actual.Should().Equal(expected, "the cache answered for '{0}'", name);
            }
        }

        [TestMethod]
        public void TryGetFilesAnswersForPlainNamesOfFiles()
        {
            var cache = new InputDirectoryCache();
            cache.TryGetFiles(directory, "Foo.class").Should().Equal(Directory.GetFiles(directory, "Foo.class"));
            cache.TryGetFiles(directory, "Hidden.class").Should().Equal(Directory.GetFiles(directory, "Hidden.class"));
        }

        [TestMethod]
        public void TryGetFilesDeclinesForDirectoriesMissingFilesAndPatterns()
        {
            var cache = new InputDirectoryCache();
            cache.TryGetFiles(directory, "Sub.class").Should().BeNull();
            cache.TryGetFiles(directory, "Missing.class").Should().BeNull();
            cache.TryGetFiles(directory, "*.class").Should().BeNull();
            cache.TryGetFiles(directory, "Foo.cl?ss").Should().BeNull();
            cache.TryGetFiles(directory, "Foo.class.").Should().BeNull();
            cache.TryGetFiles(directory, "..").Should().BeNull();
            cache.TryGetFiles(Path.Combine(directory, "DoesNotExist"), "Foo.class").Should().BeNull();
        }

        [TestMethod]
        public void MayContainFileIsTrueWhenFileExists()
        {
            var cache = new InputDirectoryCache();
            foreach (var name in Names)
            {
                if (name.Length == 0)
                    continue;

                bool exists;
                try
                {
                    exists = File.Exists(Path.Combine(directory, name));
                }
                catch
                {
                    exists = false;
                }

                // it may only say no where the file system would also say no
                if (exists)
                    cache.MayContainFile(directory, name).Should().BeTrue("'{0}' exists", name);
            }
        }

        [TestMethod]
        public void MayContainFileRulesOutMissingFiles()
        {
            var cache = new InputDirectoryCache();
            cache.MayContainFile(directory, "Missing.class").Should().BeFalse();
            cache.MayContainFile(directory, "Sub.class").Should().BeFalse();
            cache.MayContainFile(directory, "Foo.class").Should().BeTrue();
            cache.MayContainFile(directory, "foo.CLASS").Should().BeTrue();
            cache.MayContainFile(directory, "Hidden.class").Should().BeTrue();
        }

        [TestMethod]
        public void MayContainFileDefersWhenItCannotTell()
        {
            var cache = new InputDirectoryCache();
            cache.MayContainFile(null, "Foo.class").Should().BeTrue();
            cache.MayContainFile(directory, "*.class").Should().BeTrue();
            cache.MayContainFile(directory, "..").Should().BeTrue();
            cache.MayContainFile(Path.Combine(directory, "DoesNotExist"), "Foo.class").Should().BeTrue();
        }

    }

}
