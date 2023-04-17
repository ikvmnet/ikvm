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
        public void CanRedirectStandardOutput()
        {
            string c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = "cmd.exe /c echo hello";
            else
                c = "echo hello";

            var b = new ProcessBuilder(c);
            var p = b.start();

            var r = new BufferedReader(new InputStreamReader(p.getInputStream()));
            var l = r.readLine();
            p.waitFor();
            p.destroy();

            l.Should().Be("hello");
        }

    }

}
