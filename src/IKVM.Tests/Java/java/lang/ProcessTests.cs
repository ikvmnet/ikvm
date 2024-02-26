using System.IO;
using System.Runtime.InteropServices;

using FluentAssertions;

using java.io;
using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ProcessTests
    {

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanReadExitCode()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "exit 1" };
            else
                c = new[] { "/bin/sh", "-c", "exit 1" };

            var b = new ProcessBuilder(c);
            var p = b.start();

            p.waitFor();
            p.exitValue().Should().Be(1);
        }

        [TestMethod]
        public void CanReadFromInputStream()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello" };

            var b = new ProcessBuilder(c);
            b.redirectOutput(ProcessBuilder.Redirect.PIPE);
            var p = b.start();

            var r = new BufferedReader(new InputStreamReader(p.getInputStream()));
            var l = r.readLine();
            p.waitFor();

            l.Should().Be("hello");
        }

        [TestMethod]
        public void CanReadFromErrorStream()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello>&2" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello>&2" };

            var b = new ProcessBuilder(c);
            b.redirectError(ProcessBuilder.Redirect.PIPE);
            var p = b.start();

            var r = new BufferedReader(new InputStreamReader(p.getErrorStream()));
            var l = r.readLine();
            p.waitFor();

            l.Should().Be("hello");
        }

        [TestMethod]
        public void CanRedirectOutputToFile()
        {
            var f = Path.GetTempFileName();

            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello" };

            var b = new ProcessBuilder(c);
            b.redirectOutput(ProcessBuilder.Redirect.to(new global::java.io.File(f)));
            var p = b.start();
            p.waitFor();

            System.IO.File.ReadAllText(f).TrimEnd().Should().Be("hello");
        }

        [TestMethod]
        public void CanRedirectErrorToFile()
        {
            var f = Path.GetTempFileName();

            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello>&2" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello>&2" };

            var b = new ProcessBuilder(c);
            b.redirectError(ProcessBuilder.Redirect.to(new global::java.io.File(f)));
            var p = b.start();
            p.waitFor();

            System.IO.File.ReadAllText(f).TrimEnd().Should().Be("hello");
        }

        [TestMethod]
        public void CanRedirectOutputToPipe()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello" };

            var b = new ProcessBuilder(c);
            b.redirectOutput(ProcessBuilder.Redirect.PIPE);
            var p = b.start();
            p.waitFor();
        }

        [TestMethod]
        public void CanRedirectErrorToPipe()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello>&2" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello>&2" };

            var b = new ProcessBuilder(c);
            b.redirectError(ProcessBuilder.Redirect.PIPE);
            var p = b.start();
            p.waitFor();
        }

        [TestMethod]
        public void CanInheritInput()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello" };

            var b = new ProcessBuilder(c);
            b.redirectError(ProcessBuilder.Redirect.INHERIT);
            var p = b.start();
            p.waitFor();
            TestContext.WriteLine(p.ToString());
        }

        [TestMethod]
        public void CanInheritError()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello" };

            var b = new ProcessBuilder(c);
            b.redirectError(ProcessBuilder.Redirect.INHERIT);
            var p = b.start();
            TestContext.WriteLine(p.ToString());
        }

        [TestMethod]
        public void CanWaitForWithInheritIO()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello" };

            var b = new ProcessBuilder(c);
            b.inheritIO();
            var p = b.start();
            p.waitFor();
        }

    }

}
