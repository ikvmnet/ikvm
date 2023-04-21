using System.Runtime.InteropServices;

using FluentAssertions;

using java.io;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class RuntimeTests
    {

        [TestMethod]
        public void CanExecAndReadFromInputStream()
        {
            string c;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                c = "cmd.exe /c \"echo hello\"";
            else
                c = "/bin/sh -c 'echo hello'";

            var p = global::java.lang.Runtime.getRuntime().exec(c);
            
            var r = new BufferedReader(new InputStreamReader(p.getInputStream()));
            var l = r.readLine();
            p.waitFor();

            l.Should().Be("hello");
        }

    }

}
