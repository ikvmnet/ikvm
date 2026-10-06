using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Compares text against a snapshot embedded in the test assembly from the 'CompilerSnapshots' directory. Set the
    /// environment variable IKVM_TESTS_UPDATE_SNAPSHOTS=true to write the actual text to the snapshot files in the
    /// source tree instead.
    /// </summary>
    /// <remarks>
    /// .NET Framework's ILGenerator encodes the same IL differently (for example, it does not use the short forms of
    /// ldc.i4, and it computes a different max stack), so tests running on .NET Framework use their own snapshots,
    /// named '{name}.netfx.il'.
    /// </remarks>
    static class SnapshotAssert
    {

        const string UpdateVariable = "IKVM_TESTS_UPDATE_SNAPSHOTS";

        static readonly string FileSuffix = RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.Ordinal) ? ".netfx.il" : ".il";

        public static void Match(string name, string actual, [CallerFilePath] string callerFilePath = "")
        {
            actual = Normalize(actual);
            var fileName = name + FileSuffix;

            if (string.Equals(Environment.GetEnvironmentVariable(UpdateVariable), "true", StringComparison.OrdinalIgnoreCase))
            {
                var path = Path.Combine(Path.GetDirectoryName(callerFilePath), "CompilerSnapshots", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, actual);
                return;
            }

            using var stream = typeof(SnapshotAssert).Assembly.GetManifestResourceStream("CompilerSnapshots/" + fileName);
            if (stream == null)
                Assert.Fail($"Snapshot '{fileName}' does not exist. Run the test with {UpdateVariable}=true to create it.");

            using var reader = new StreamReader(stream);
            var expected = Normalize(reader.ReadToEnd());
            if (expected != actual)
                Assert.Fail($"Snapshot '{fileName}' does not match. If the change is intended, run the test with {UpdateVariable}=true to update it.\n\nExpected:\n{expected}\nActual:\n{actual}");
        }

        static string Normalize(string text)
        {
            return text.Replace("\r\n", "\n");
        }

    }

}
