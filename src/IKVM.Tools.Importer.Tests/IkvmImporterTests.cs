using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

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
