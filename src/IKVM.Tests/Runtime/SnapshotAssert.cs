using System;
using System.IO;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Compares text against a snapshot embedded in the test assembly from the 'CompilerSnapshots' directory. Set the
    /// environment variable IKVM_TESTS_UPDATE_SNAPSHOTS=true to write the actual text to the snapshot files in the
    /// source tree instead.
    /// </summary>
    static class SnapshotAssert
    {

        const string UpdateVariable = "IKVM_TESTS_UPDATE_SNAPSHOTS";

        public static void Match(string name, string actual, [CallerFilePath] string callerFilePath = "")
        {
            actual = Normalize(actual);

            if (string.Equals(Environment.GetEnvironmentVariable(UpdateVariable), "true", StringComparison.OrdinalIgnoreCase))
            {
                var path = Path.Combine(Path.GetDirectoryName(callerFilePath), "CompilerSnapshots", name + ".il");
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, actual);
                return;
            }

            using var stream = typeof(SnapshotAssert).Assembly.GetManifestResourceStream("CompilerSnapshots/" + name + ".il");
            if (stream == null)
                Assert.Fail($"Snapshot '{name}' does not exist. Run the test with {UpdateVariable}=true to create it.");

            using var reader = new StreamReader(stream);
            var expected = Normalize(reader.ReadToEnd());
            if (expected != actual)
                Assert.Fail($"Snapshot '{name}' does not match. If the change is intended, run the test with {UpdateVariable}=true to update it.\n\nExpected:\n{expected}\nActual:\n{actual}");
        }

        static string Normalize(string text)
        {
            return text.Replace("\r\n", "\n");
        }

    }

}
