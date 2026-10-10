using System.IO;

using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Checks the recorded runtime snapshot of the fixture against what the running runtime reports. Each host can only
    /// check the snapshot of its own target framework; set <see cref="Fixture.UpdateSnapshotsVariable"/> to regenerate it.
    /// </summary>
    public class FixtureSnapshotTests
    {

        [Fact]
        public void HostSnapshotIsCurrent()
        {
            var tfm = Fixture.HostTargetFramework;
            var assembly = System.Reflection.Assembly.LoadFrom(Fixture.GetAssemblyPath(tfm));
            var actual = Dump.Runtime.ReflectionDump.Dump(assembly);

            if (Fixture.UpdateSnapshots)
            {
                File.WriteAllText(Fixture.GetSourceSnapshotPath(tfm), actual);
                return;
            }

            File.Exists(Fixture.GetSnapshotPath(tfm)).Should().BeTrue($"a snapshot for {tfm} should be recorded; run on {tfm} with {Fixture.UpdateSnapshotsVariable}=1 to create it");
            TextDiff.ShouldMatch(actual, Fixture.ReadSnapshot(tfm), $"between the {tfm} runtime and its recorded snapshot");
        }

    }

}
