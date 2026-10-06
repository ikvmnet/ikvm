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

    }

}
