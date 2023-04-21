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

        [TestMethod]
        public void CanReadFromInputStream()
        {
            string[] c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = new[] { "cmd.exe", "/c", "echo hello" };
            else
                c = new[] { "/bin/sh", "-c", "echo hello" };

            var b = new ProcessBuilder(c);
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
            var p = b.start();

            var r = new BufferedReader(new InputStreamReader(p.getErrorStream()));
            var l = r.readLine();
            p.waitFor();

            l.Should().Be("hello");
        }

    }

}
