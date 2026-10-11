using System.IO;

using Xunit;

namespace IKVM.Reflection.Tests.Reader
{

    public class ModuleReaderTests
    {

        /// <summary>
        /// Loads the fixture built for <paramref name="tfm"/> and checks IKVM.Reflection reports what the runtime of that
        /// target framework reports, whatever the host runtime is.
        /// </summary>
        /// <param name="tfm"></param>
        [Theory]
        [MemberData(nameof(Fixture.GetTargetFrameworkTestData), MemberType = typeof(Fixture))]
        public void ReadsFixtureLikeTargetRuntime(string tfm)
        {
            using var u = TestUniverse.CreateLikeImporter(tfm);
            var assembly = u.Universe.LoadFile(Fixture.GetAssemblyPath(tfm));
            var actual = Dump.Ikvm.ReflectionDump.Dump(assembly);

            TextDiff.ShouldMatch(actual, Fixture.ReadSnapshot(tfm), $"between IKVM.Reflection on {Fixture.HostTargetFramework} and the {tfm} runtime");
        }

        /// <summary>
        /// The tables stream version is read from the metadata root rather than from System.Reflection.Metadata, which
        /// does not expose it. Roslyn writes version 2.0, which is what the runtime reports for its own modules.
        /// </summary>
        /// <param name="tfm"></param>
        [Theory]
        [MemberData(nameof(Fixture.GetTargetFrameworkTestData), MemberType = typeof(Fixture))]
        public void ReportsTablesStreamVersion(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var assembly = u.Universe.LoadFile(Fixture.GetAssemblyPath(tfm));

            Assert.Equal(typeof(object).Module.MDStreamVersion, assembly.ManifestModule.MDStreamVersion);
            Assert.Equal(0x20000, assembly.ManifestModule.MDStreamVersion);
        }

    }

}
