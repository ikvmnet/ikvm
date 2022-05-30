using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CliWrap;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ikvmc.Tests
{

    [TestClass]
    public class ExeTests
    {

        /// <summary>
        /// Executes the IKVMC command line.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="useNetCore"></param>
        /// <param name="exitCode"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        async Task<string> ExecuteIkvmc(string target, bool useNetCore, int exitCode, params string[] arguments)
        {
            var dir = Path.Combine(Path.GetDirectoryName(typeof(ExeTests).Assembly.Location), $"ikvmc-{target}");
            var cmd = useNetCore ? Cli.Wrap("dotnet").WithArguments(o => o.Add("exec").Add(Path.Combine(dir, "ikvmc.dll"))) : Cli.Wrap(Path.Combine(dir, "ikvmc.exe"));
            var txt = new StringBuilder();
            cmd = cmd.WithStandardOutputPipe(PipeTarget.ToStringBuilder(txt));
            cmd = cmd.WithStandardErrorPipe(PipeTarget.ToStringBuilder(txt));
            var tsk = await cmd.ExecuteAsync();
            tsk.ExitCode.Should().Be(exitCode);
            var str = txt.ToString();
            return str;
        }

        /// <summary>
        /// Can confirm we can simply execute the ikvmc executable and not have an error code.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="useNetCore"></param>
        [DataTestMethod]
        [DynamicData(nameof(GetVersionsToTest), DynamicDataSourceType.Method)]
        public async Task Should_display_options_and_exit_successfully_with_no_arguments(string target, bool useNetCore)
        {
            var str = await ExecuteIkvmc(target, useNetCore, 0);
            str.Should().StartWith("IKVM.NET");
            str.Should().Contain("Compiler Options:");
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
