using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CliWrap;

using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ikvmc.Exe.Tests
{

    [TestClass]
    public class ExeTests
    {

        /// <summary>
        /// Describes the result of executing an exe.
        /// </summary>
        class ExeResult
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="exitCode"></param>
            /// <param name="standardOutput"></param>
            /// <param name="standardError"></param>
            public ExeResult(int exitCode, string standardOutput, string standardError)
            {
                ExitCode = exitCode;
                StandardOutput = standardOutput;
                StandardError = standardError;
            }

            public int ExitCode { get; set; }

            public string StandardOutput { get; set; }

            public string StandardError { get; set; }

        }

        /// <summary>
        /// Executes the IKVMC command line.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="useNetCore"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        async Task<ExeResult> ExecuteIkvmc(string target, bool useNetCore, params string[] arguments)
        {
            var dir = Path.Combine(Path.GetDirectoryName(typeof(ExeTests).Assembly.Location), $"ikvmc-{target}");
            var cmd = useNetCore ? Cli.Wrap("dotnet").WithArguments(o => o.Add("exec").Add(Path.Combine(dir, "ikvmc.dll"))) : Cli.Wrap(Path.Combine(dir, "ikvmc.exe"));
            cmd = cmd.WithArguments(o => o.Add(arguments));
            cmd = cmd.WithValidation(CommandResultValidation.None);
            var stdout = new StringBuilder();
            cmd = cmd.WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdout));
            var stderr = new StringBuilder();
            cmd = cmd.WithStandardErrorPipe(PipeTarget.ToStringBuilder(stderr));
            var tsk = await cmd.ExecuteAsync();
            return new ExeResult(tsk.ExitCode, stdout.ToString(), stderr.ToString());
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
            var rsl = await ExecuteIkvmc(target, useNetCore);
            rsl.ExitCode.Should().Be(0);
            rsl.StandardOutput.Should().StartWith("IKVM.NET");
            rsl.StandardOutput.Should().Contain("Compiler Options:");
        }

        /// <summary>
        /// Can confirm we can simply execute the ikvmc executable and not have an error code.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="useNetCore"></param>
        [DataTestMethod]
        [DynamicData(nameof(GetVersionsToTest), DynamicDataSourceType.Method)]
        public async Task Should_convert_simple_jar(string target, bool useNetCore)
        {
            var s = new StreamReader(typeof(ExeTests).Assembly.GetManifestResourceStream("ikvmc.Exe.Tests.ExeTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvmc.exe.tests.ExeTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
            var j = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".jar");
            c.WriteJar(j);

            var asm = Path.ChangeExtension(j, ".dll");
            var rsl = await ExecuteIkvmc(target, useNetCore, "-assembly:ikvmc.Tests.Java", $"-out:{asm}", j);
            rsl.ExitCode.Should().Be(0);
            rsl.StandardOutput.Should().StartWith("IKVM.NET");
            File.Exists(asm).Should().BeTrue();
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
