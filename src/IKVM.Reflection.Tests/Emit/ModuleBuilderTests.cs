using System.IO;

using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    public class ModuleBuilderTests
    {

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineManifestResource(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test");
            module.DefineManifestResource("Resource1", new MemoryStream([0x01, 0x02, 0x03, 0x04, 0x05]), ResourceAttributes.Public);
            module.DefineManifestResource("Resource2", new MemoryStream([0x06, 0x07, 0x08, 0x09, 0x0a]), ResourceAttributes.Public);
            module.DefineManifestResource("Resource3", new MemoryStream([0x0b, 0x0c, 0x0d, 0x0e, 0x0f]), ResourceAttributes.Public);
            type.CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            a.GetManifestResourceNames().Should().BeEquivalentTo("Resource1", "Resource2", "Resource3");
            ReadAll(a.GetManifestResourceStream("Resource1")!).Should().Equal(0x01, 0x02, 0x03, 0x04, 0x05);
            ReadAll(a.GetManifestResourceStream("Resource2")!).Should().Equal(0x06, 0x07, 0x08, 0x09, 0x0a);
            ReadAll(a.GetManifestResourceStream("Resource3")!).Should().Equal(0x0b, 0x0c, 0x0d, 0x0e, 0x0f);
        }

        /// <summary>
        /// Re-emits the fixture built for <paramref name="tfm"/> through the builders and checks that the copy, read back,
        /// renders like the original within what the builders can emit. The reader tests tie the rendering of the original
        /// to what the runtime of that target framework reports.
        /// </summary>
        /// <param name="tfm"></param>
        [Theory]
        [MemberData(nameof(Fixture.GetTargetFrameworkTestData), MemberType = typeof(Fixture))]
        public void CanRewriteFixtureLikeTargetRuntime(string tfm)
        {
            using var source = TestUniverse.CreateLikeImporter(tfm);
            using var target = TestUniverse.CreateLikeImporter(tfm);
            var fileName = new FixtureCopier(source.Universe.LoadFile(Fixture.GetAssemblyPath(tfm)), target.Universe, target.TempPath).Copy();
            target.Verify(fileName);

            using var reader = TestUniverse.CreateLikeImporter(tfm);
            var expected = Dump.Ikvm.ReflectionDump.Dump(reader.Universe.LoadFile(Fixture.GetAssemblyPath(tfm)), Dump.Ikvm.DumpScope.Emittable);
            using var copyReader = TestUniverse.CreateLikeImporter(tfm);
            var actual = Dump.Ikvm.ReflectionDump.Dump(copyReader.Universe.LoadFile(Path.Combine(target.TempPath, fileName)), Dump.Ikvm.DumpScope.Emittable);
            TextDiff.ShouldMatch(actual, expected, $"between the {tfm} fixture and its copy emitted on {Fixture.HostTargetFramework}");
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void ModuleVersionIdMatchesWrittenImage(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            module.DefineType("Test").CreateType();
            assembly.Save("Test.dll");

            module.ModuleVersionId.Should().Be(u.VerifyAndLoad("Test.dll").ManifestModule.ModuleVersionId);
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void DeterministicOutputIsRepeatable(string tfm)
        {
            byte[] Save()
            {
                using var u = TestUniverse.Create(tfm, UniverseOptions.DeterministicOutput);
                var assembly = u.DefineAssembly();
                var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
                var type = module.DefineType("Test", TypeAttributes.Public);
                type.DefineField("value", u.Import(typeof(int)), FieldAttributes.Public);
                type.CreateType();
                assembly.Save("Test.dll");
                return File.ReadAllBytes(Path.Combine(u.TempPath, "Test.dll"));
            }

            Save().Should().Equal(Save());
        }

        static byte[] ReadAll(Stream stream)
        {
            using var m = new MemoryStream();
            stream.CopyTo(m);
            return m.ToArray();
        }

    }

}
