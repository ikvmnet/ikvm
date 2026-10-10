using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Locates the builds of IKVM.Reflection.Tests.Fixture and the runtime snapshots recorded for each of them.
    /// </summary>
    static class Fixture
    {

        /// <summary>
        /// Set to regenerate the snapshot of the host target framework instead of comparing against it.
        /// </summary>
        public const string UpdateSnapshotsVariable = "IKVM_REFLECTION_TESTS_UPDATE_SNAPSHOTS";

        /// <summary>
        /// Gets the target frameworks the fixture is built for.
        /// </summary>
        public static readonly string[] TargetFrameworks = ["net472", "net6.0", "net8.0"];

        /// <summary>
        /// Gets the fixture target frameworks, as theory data.
        /// </summary>
        public static IEnumerable<object[]> GetTargetFrameworkTestData() => TargetFrameworks.Select(i => new object[] { i });

        /// <summary>
        /// Gets the target framework of the running host.
        /// </summary>
        public static string HostTargetFramework =>
#if NET8_0
            "net8.0";
#elif NET6_0
            "net6.0";
#elif NET472
            "net472";
#else
#error Unknown host target framework.
#endif

        /// <summary>
        /// Gets the path of the fixture assembly built for the given target framework.
        /// </summary>
        /// <param name="tfm"></param>
        /// <returns></returns>
        public static string GetAssemblyPath(string tfm) => Path.Combine(AppContext.BaseDirectory, "fixture", tfm, "IKVM.Reflection.Tests.Fixture.dll");

        /// <summary>
        /// Gets the path of the checked-in snapshot for the given target framework, in the output directory.
        /// </summary>
        /// <param name="tfm"></param>
        /// <returns></returns>
        public static string GetSnapshotPath(string tfm) => Path.Combine(AppContext.BaseDirectory, "Snapshots", tfm + ".txt");

        /// <summary>
        /// Gets the path of the snapshot for the given target framework in the source tree, for regeneration.
        /// </summary>
        /// <param name="tfm"></param>
        /// <returns></returns>
        public static string GetSourceSnapshotPath(string tfm)
        {
            var dir = typeof(Fixture).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().First(i => i.Key == "ProjectDirectory").Value!;
            return Path.Combine(dir, "Snapshots", tfm + ".txt");
        }

        /// <summary>
        /// Reads the checked-in snapshot for the given target framework, normalizing line endings.
        /// </summary>
        /// <param name="tfm"></param>
        /// <returns></returns>
        public static string ReadSnapshot(string tfm) => File.ReadAllText(GetSnapshotPath(tfm)).Replace("\r\n", "\n");

        /// <summary>
        /// Gets whether snapshots should be regenerated.
        /// </summary>
        public static bool UpdateSnapshots => Environment.GetEnvironmentVariable(UpdateSnapshotsVariable) is "1" or "true";

    }

}
