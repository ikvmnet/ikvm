using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class SystemTests
    {

        [TestMethod]
        public void OsNameProperty()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                global::java.lang.System.getProperty("os.name").Should().StartWith("Windows");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                global::java.lang.System.getProperty("os.name").Should().StartWith("Linux");
        }

        [TestMethod]
        public void CanWriteToStdOut()
        {
            global::java.lang.System.@out.println("TEST");
        }

        [TestMethod]
        public void CanWriteToStdErr()
        {
            global::java.lang.System.err.println("TEST");
        }

    }

}
