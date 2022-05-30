using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ikvmc.Tests
{

    [TestClass]
    public class ExeTests
    {

        /// <summary>
        /// Can confirm we can simply execute the ikvmc executable and not have an error code.
        /// </summary>
        /// <param name="target"></param>
        [DataTestMethod]
        [DynamicData(nameof(GetVersionsToTest), DynamicDataSourceType.Method)]
        public void Should_execute(string target, bool useDotNet)
        {
            var dir = Path.Combine(Path.GetDirectoryName(typeof(ExeTests).Assembly.Location), $"ikvmc-{target}");
            var exe = useDotNet ? $"dotnet exec ${Path.Combine(dir, "ikvmc.dll")}" : Path.Combine(dir, "ikvmc.exe");
            var pid = Process.Start(exe);
            pid.WaitForExit();
            pid.ExitCode.Should().Be(0);
        }

        /// <summary>
        /// Gets the IKVMC builds to test based on the current test OS platform.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetVersionsToTest()
        {
            // we test windows executables on Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return new object[] { "net461-win7-any", false };
                yield return new object[] { "netcoreapp3.1-win7-x64", false };
                yield return new object[] { "netcoreapp3.1-win7-x86", false };
            }

            // we test Linux executables on Linux
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                yield return new object[] { "netcoreapp3.1-linux-x64", true };
            }
        }

    }

}
