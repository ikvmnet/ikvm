using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;
using IKVM.Java.Tests.Util;
using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Importer.Tests
{

    [TestClass]
    public class IkvmImporterTests
    {

        static readonly string TESTBASE = Path.GetDirectoryName(typeof(IkvmImporterTests).Assembly.Location);

        [DataTestMethod]
        [DataRow("net472", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net472", "net481", ".NETFramework", "4.8.1")]
        [DataRow("net6.0", "net6.0", ".NET", "6.0")]
        [DataRow("net6.0", "net7.0", ".NET", "7.0")]
        [DataRow("net6.0", "net8.0", ".NET", "8.0")]
        [DataRow("net6.0", "net10.0", ".NET", "10.0")]
        [DataRow("net8.0", "net8.0", ".NET", "8.0")]
        [DataRow("net8.0", "net10.0", ".NET", "10.0")]
        public async Task CanImportSimpleTest(string ikvmFramework, string targetFramework, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            var s = new StreamReader(typeof(IkvmImporterTests).Assembly.GetManifestResourceStream("IKVM.Tools.Importer.Tests.IkvmImporterTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.tools.importer.tests.IkvmImporterTests", s);
            var c = new InMemoryCompiler([f]);
            c.Compile();
            var j = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".jar");
            c.WriteJar(j);

            var ikvmLibs = Path.Combine(TESTBASE, "lib", ikvmFramework);
            var libPaths = DotNetSdkUtil.GetPathToReferenceAssemblies(targetFramework, targetFrameworkIdentifier, targetFrameworkVersion);

            // add references to libraries
            var asm = Path.ChangeExtension(j, ".dll");
            var args = new List<string>();
            foreach (var i in libPaths)
                args.Add($"-lib:{i}");
            foreach (var dll in Directory.GetFiles(ikvmLibs, "*.dll"))
                args.Add($"-reference:{dll}");

            // add additional command options
            args.Add($"-runtime:{Path.Combine(ikvmLibs, "IKVM.Runtime.dll")}");
            args.Add("-nostdlib");
            args.Add("-assembly:IKVM.Tools.Importer.Tests.Java");
            args.Add($"-out:{asm}");
            args.Add(j);

            // initiate the import
            var ret = await ImportTool.InvokeAsync(args.ToArray(), CancellationToken.None);
            ret.Should().Be(0);
            File.Exists(asm).Should().BeTrue();
            new FileInfo(asm).Length.Should().BeGreaterThanOrEqualTo(128);
        }

        [DataTestMethod]
        [DataRow("net472", "net472", ".NETFramework", "4.7.2")]
        [DataRow("net8.0", "net8.0", ".NET", "8.0")]
        public async Task DeterministicImportIsReproducible(string ikvmFramework, string targetFramework, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            var s = new StreamReader(typeof(IkvmImporterTests).Assembly.GetManifestResourceStream("IKVM.Tools.Importer.Tests.IkvmImporterTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.tools.importer.tests.IkvmImporterTests", s);
            var c = new InMemoryCompiler([f]);
            c.Compile();
            var d = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n"));
            Directory.CreateDirectory(d);
            var j = Path.Combine(d, "IKVM.Tools.Importer.Tests.Java.jar");
            c.WriteJar(j);

            // add a plain resource so the embedded jar carries more than the class list
            using (var zip = new ZipArchive(File.Open(j, FileMode.Open), ZipArchiveMode.Update))
            using (var w = new StreamWriter(zip.CreateEntry("ikvm/tools/importer/tests/resource.txt").Open()))
                w.Write("resource");

            var ikvmLibs = Path.Combine(TESTBASE, "lib", ikvmFramework);
            var libPaths = DotNetSdkUtil.GetPathToReferenceAssemblies(targetFramework, targetFrameworkIdentifier, targetFrameworkVersion);

            async Task<string> ImportAsync(string name)
            {
                var asm = Path.Combine(d, name, "IKVM.Tools.Importer.Tests.Java.dll");
                Directory.CreateDirectory(Path.GetDirectoryName(asm));

                var args = new List<string>();
                foreach (var i in libPaths)
                    args.Add($"-lib:{i}");
                foreach (var dll in Directory.GetFiles(ikvmLibs, "*.dll"))
                    args.Add($"-reference:{dll}");

                args.Add($"-runtime:{Path.Combine(ikvmLibs, "IKVM.Runtime.dll")}");
                args.Add("-nostdlib");
                args.Add("-deterministic");
                args.Add("-assembly:IKVM.Tools.Importer.Tests.Java");
                args.Add($"-out:{asm}");
                args.Add(j);

                var ret = await ImportTool.InvokeAsync(args.ToArray(), CancellationToken.None);
                ret.Should().Be(0);
                File.Exists(asm).Should().BeTrue();
                return asm;
            }

            var a = await ImportAsync("a");
            var b = await ImportAsync("b");
            File.ReadAllBytes(b).Should().Equal(File.ReadAllBytes(a));

            // entry timestamps must not depend on the time of the build, even when both imports land in the same second
            var entries = ReadEmbeddedJarEntries(a);
            entries.Should().Contain(i => i.FullName == "ikvm/tools/importer/tests/resource.txt");
            entries.Should().OnlyContain(i => i.LastWriteTime.DateTime == new DateTime(1980, 1, 1));
        }

        [TestMethod]
        public async Task UnsupportedClassVersionIsSummarizedPerJar()
        {
            var j = WriteJar(new()
            {
                ["test/A.class"] = BuildClass(55, "test/A"),
                ["test/B.class"] = BuildClass(55, "test/B"),
                ["test/C.class"] = BuildClass(61, "test/C"),
            });

            var (ret, diagnostics) = await ImportWithDiagnosticsAsync(j);
            ret.Should().Be(0);

            // a single summary instead of a warning per class
            diagnostics.Should().NotContain(i => i.Id == 101);
            var summary = diagnostics.Should().ContainSingle(i => i.Id == 146).Subject;
            summary.Args[0].Should().Be("3");
            summary.Args[1].Should().Be(Path.GetFileName(j));
            summary.Args[2].Should().Be("Java 11 (class file version 55.0), Java 17 (class file version 61.0)");
            summary.Args[3].Should().BeOneOf("test.A", "test.B", "test.C");

            // nothing at all could be compiled from the jar
            var empty = diagnostics.Should().ContainSingle(i => i.Id == 147).Subject;
            empty.Args[0].Should().Be(Path.GetFileName(j));
            empty.Args[1].Should().Be("3");
        }

        [TestMethod]
        public async Task UnsupportedClassVersionDoesNotReportEmptyOutputWhenSomeClassesCompile()
        {
            var j = WriteJar(new()
            {
                ["test/A.class"] = BuildClass(52, "test/A"),
                ["test/B.class"] = BuildClass(55, "test/B"),
            });

            var (ret, diagnostics) = await ImportWithDiagnosticsAsync(j);
            ret.Should().Be(0);

            diagnostics.Should().NotContain(i => i.Id == 101);
            diagnostics.Should().NotContain(i => i.Id == 147);
            var summary = diagnostics.Should().ContainSingle(i => i.Id == 146).Subject;
            summary.Args[0].Should().Be("1");
            summary.Args[2].Should().Be("Java 11 (class file version 55.0)");
            summary.Args[3].Should().Be("test.B");
        }

        [TestMethod]
        public async Task ModuleInfoAndMultiReleaseEntriesAreNotCompiled()
        {
            var j = WriteJar(new()
            {
                ["test/A.class"] = BuildClass(52, "test/A"),
                ["module-info.class"] = BuildClass(53, "module-info"),
                ["META-INF/versions/11/test/A.class"] = BuildClass(55, "test/A"),
            });

            var (ret, diagnostics) = await ImportWithDiagnosticsAsync(j);
            ret.Should().Be(0);

            // a Java 8 runtime never loads these as classes, so they are not reported as failures
            diagnostics.Should().NotContain(i => i.Id == 101 || i.Id == 135 || i.Id == 146 || i.Id == 147);
        }

        /// <summary>
        /// Builds an empty public class with the given class file major version.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static byte[] BuildClass(ushort major, string name)
        {
            var builder = new ClassFileBuilder(new ClassFormatVersion(major, 0), AccessFlag.Public | AccessFlag.Super, name, "java/lang/Object");
            var buffer = new BlobBuilder();
            builder.Serialize(buffer);
            return buffer.ToArray();
        }

        /// <summary>
        /// Writes a jar with the given entries to a temporary location.
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        static string WriteJar(Dictionary<string, byte[]> entries)
        {
            var j = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".jar");

            using (var zip = new ZipArchive(File.Create(j), ZipArchiveMode.Create))
                foreach (var kvp in entries)
                    using (var s = zip.CreateEntry(kvp.Key).Open())
                        s.Write(kvp.Value, 0, kvp.Value.Length);

            return j;
        }

        /// <summary>
        /// Imports the given jar and returns the exit code and the diagnostics that were reported.
        /// </summary>
        /// <param name="jar"></param>
        /// <returns></returns>
        static async Task<(int, List<(int Id, string[] Args)>)> ImportWithDiagnosticsAsync(string jar)
        {
            var ikvmLibs = Path.Combine(TESTBASE, "lib", "net8.0");
            var libPaths = DotNetSdkUtil.GetPathToReferenceAssemblies("net8.0", ".NET", "8.0");

            var asm = Path.ChangeExtension(jar, ".dll");
            var log = Path.ChangeExtension(jar, ".log");
            var args = new List<string>();
            foreach (var i in libPaths)
                args.Add($"-lib:{i}");
            foreach (var dll in Directory.GetFiles(ikvmLibs, "*.dll"))
                args.Add($"-reference:{dll}");

            args.Add($"-runtime:{Path.Combine(ikvmLibs, "IKVM.Runtime.dll")}");
            args.Add("-nostdlib");
            args.Add($"-log:json,file={log}");
            args.Add($"-out:{asm}");
            args.Add(jar);

            var ret = await ImportTool.InvokeAsync(args.ToArray(), CancellationToken.None);

            var diagnostics = new List<(int Id, string[] Args)>();
            foreach (var line in File.ReadAllLines(log))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                using var json = JsonDocument.Parse(line);
                var id = json.RootElement.GetProperty("id").GetInt32();
                var a = json.RootElement.GetProperty("args").EnumerateArray().Select(i => i.ToString()).ToArray();
                diagnostics.Add((id, a));
            }

            return (ret, diagnostics);
        }

        /// <summary>
        /// Reads the entries of each jar embedded as a manifest resource in the given assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static List<ZipArchiveEntry> ReadEmbeddedJarEntries(string path)
        {
            using var pe = new PEReader(File.OpenRead(path));
            var md = pe.GetMetadataReader();
            var resources = pe.GetSectionData(pe.PEHeaders.CorHeader.ResourcesDirectory.RelativeVirtualAddress);

            var entries = new List<ZipArchiveEntry>();
            foreach (var handle in md.ManifestResources)
            {
                var resource = md.GetManifestResource(handle);
                if (md.GetString(resource.Name).EndsWith(".jar") == false)
                    continue;

                var reader = resources.GetReader((int)resource.Offset, resources.Length - (int)resource.Offset);
                var data = reader.ReadBytes(reader.ReadInt32());
                entries.AddRange(new ZipArchive(new MemoryStream(data), ZipArchiveMode.Read).Entries);
            }

            entries.Should().NotBeEmpty();
            return entries;
        }

    }

}
