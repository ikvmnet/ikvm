using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.MSBuild.Tasks.Tests
{

    [TestClass]
    public class IkvmWriteExportsFileTests
    {

        /// <summary>
        /// Builds a new task instance writing to the given path.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        static IkvmWriteExportsFile BuildTestTask(string output, params string[] references)
        {
            var engine = new Mock<IBuildEngine9>();

            var t = new IkvmWriteExportsFile();
            t.BuildEngine = engine.Object;
            t.Output = output;
            t.References = Array.ConvertAll(references, i => (ITaskItem)new TaskItem(i));
            return t;
        }

        /// <summary>
        /// Gets a unique path to write an exports file to.
        /// </summary>
        /// <returns></returns>
        static string GetTempPath()
        {
            var dir = Path.Combine(Path.GetTempPath(), "ikvm", "tests", Guid.NewGuid().ToString("n"));
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "ikvm.exports");
        }

        /// <summary>
        /// Reads back an exports file as a list of assembly name and type count pairs.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static IList<KeyValuePair<string, int>> ReadExports(string path)
        {
            using var stm = File.OpenRead(path);
            var rdr = new BinaryReader(stm, Encoding.UTF8);
            var l = new List<KeyValuePair<string, int>>();

            var count = rdr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var name = rdr.ReadString();
                var typeCount = rdr.ReadInt32();
                for (int j = 0; j < typeCount; j++)
                    rdr.ReadInt32();

                l.Add(new KeyValuePair<string, int>(name, typeCount));
            }

            stm.Position.Should().Be(stm.Length);
            return l;
        }

        [TestMethod]
        public void Should_write_wildcard_export_per_reference()
        {
            var output = GetTempPath();
            var a = typeof(IkvmWriteExportsFileTests).Assembly;
            var b = typeof(IkvmWriteExportsFile).Assembly;

            var t = BuildTestTask(output, a.Location, b.Location);
            t.Execute().Should().BeTrue();

            var exports = ReadExports(output);
            exports.Should().HaveCount(2);
            exports[0].Key.Should().Be(a.GetName().FullName);
            exports[0].Value.Should().Be(0);
            exports[1].Key.Should().Be(b.GetName().FullName);
            exports[1].Value.Should().Be(0);
        }

        [TestMethod]
        public void Should_preserve_reference_order()
        {
            var output = GetTempPath();
            var a = typeof(IkvmWriteExportsFileTests).Assembly;
            var b = typeof(IkvmWriteExportsFile).Assembly;

            var t = BuildTestTask(output, b.Location, a.Location);
            t.Execute().Should().BeTrue();

            var exports = ReadExports(output);
            exports[0].Key.Should().Be(b.GetName().FullName);
            exports[1].Key.Should().Be(a.GetName().FullName);
        }

        [TestMethod]
        public void Should_reverse_reference_order_when_asked()
        {
            var output = GetTempPath();
            var a = typeof(IkvmWriteExportsFileTests).Assembly;
            var b = typeof(IkvmWriteExportsFile).Assembly;

            var t = BuildTestTask(output, a.Location, b.Location);
            t.Reverse = true;
            t.Execute().Should().BeTrue();

            var exports = ReadExports(output);
            exports[0].Key.Should().Be(b.GetName().FullName);
            exports[1].Key.Should().Be(a.GetName().FullName);
        }

        [TestMethod]
        public void Should_skip_duplicate_reference()
        {
            var output = GetTempPath();
            var a = typeof(IkvmWriteExportsFileTests).Assembly;

            var t = BuildTestTask(output, a.Location, a.Location);
            t.Execute().Should().BeTrue();

            ReadExports(output).Should().HaveCount(1);
        }

        [TestMethod]
        public void Should_skip_missing_reference()
        {
            var output = GetTempPath();
            var a = typeof(IkvmWriteExportsFileTests).Assembly;

            var t = BuildTestTask(output, Path.Combine(Path.GetTempPath(), "ikvm", "tests", Guid.NewGuid().ToString("n") + ".dll"), a.Location);
            t.Execute().Should().BeTrue();

            var exports = ReadExports(output);
            exports.Should().HaveCount(1);
            exports[0].Key.Should().Be(a.GetName().FullName);
        }

        [TestMethod]
        public void Should_write_empty_file_for_no_references()
        {
            var output = GetTempPath();

            var t = BuildTestTask(output);
            t.Execute().Should().BeTrue();

            ReadExports(output).Should().BeEmpty();
        }

        [TestMethod]
        public void Should_leave_file_untouched_when_unchanged()
        {
            var output = GetTempPath();
            var a = typeof(IkvmWriteExportsFileTests).Assembly;

            BuildTestTask(output, a.Location).Execute().Should().BeTrue();
            var lastWriteTimeUtc = File.GetLastWriteTimeUtc(output);
            File.SetLastWriteTimeUtc(output, lastWriteTimeUtc - TimeSpan.FromDays(1));
            lastWriteTimeUtc = File.GetLastWriteTimeUtc(output);

            BuildTestTask(output, a.Location).Execute().Should().BeTrue();
            File.GetLastWriteTimeUtc(output).Should().Be(lastWriteTimeUtc);
        }

        [TestMethod]
        public void Should_rewrite_file_when_changed()
        {
            var output = GetTempPath();
            var a = typeof(IkvmWriteExportsFileTests).Assembly;
            var b = typeof(IkvmWriteExportsFile).Assembly;

            BuildTestTask(output, a.Location).Execute().Should().BeTrue();
            ReadExports(output).Should().HaveCount(1);

            BuildTestTask(output, a.Location, b.Location).Execute().Should().BeTrue();
            ReadExports(output).Should().HaveCount(2);
        }

        [TestMethod]
        public void Should_skip_reference_that_is_not_an_assembly()
        {
            var output = GetTempPath();
            var notAnAssembly = Path.Combine(Path.GetDirectoryName(output), "notanassembly.dll");
            File.WriteAllText(notAnAssembly, "this is not an assembly");

            var t = BuildTestTask(output, notAnAssembly, typeof(IkvmWriteExportsFileTests).Assembly.Location);
            t.Execute().Should().BeTrue();

            ReadExports(output).Should().HaveCount(1);
        }

    }

}
