using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using java.io;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class RuntimeTests
    {

        /// <summary>
        /// Reads all the text from the input stream until ended.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Task<string> ReadAllText(InputStream s) => Task.Run(() =>
        {
            try
            {
                var m = new ByteArrayOutputStream();
                var i = 0;
                while ((i = s.read()) >= 0)
                    m.write(i);

                return m.toString();
            }
            catch
            {
                return null;
            }
        });

        /// <summary>
        /// Tests a variety of ways to execute the "CD" command using Runtime.exec. Each of these should print the current directory path.
        /// </summary>
        [TestMethod]
        public void ExecCanRunWindowsCommands()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            // generate some test scripts in the working directory
            System.IO.File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "cdcmd.cmd"), "@echo off\r\nCD\r\n");
            System.IO.File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "cdbat.bat"), "@echo off\r\nCD\r\n");

            var systemRoot = Environment.GetEnvironmentVariable("SystemRoot") ?? Environment.GetEnvironmentVariable("WINDIR");
            var systemDirW = Path.Combine(systemRoot, "System32");
            var systemDirM = systemDirW.Replace('\\', '/');
            var tests = new[]
            {
                "cmd",
                "cmd.exe",
                systemDirW + "\\cmd.exe",
                systemDirW + "\\cmd",
                systemDirM + "/cmd.exe",
                systemDirM + "/cmd",
                "/" + systemDirM + "/cmd",
                "cdcmd.cmd", "./cdcmd.cmd", ".\\cdcmd.cmd",
                "cdbat.bat", "./cdbat.bat", ".\\cdbat.bat",
            };

            foreach (var t in tests)
            {
                var p = global::java.lang.Runtime.getRuntime().exec(new[] { t, "/C", "CD" });

                // start two threads to read from both streams to prevent dead locks
                var ir = ReadAllText(p.getInputStream());
                var er = ReadAllText(p.getErrorStream());

                // obtain string results
                var it = ir.GetAwaiter().GetResult();
                var et = er.GetAwaiter().GetResult();

                it.Should().Be(Environment.CurrentDirectory + Environment.NewLine);
            }
        }

    }

}
