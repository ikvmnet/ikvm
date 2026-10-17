using System;
using System.Runtime.InteropServices;
using System.Threading;

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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                global::java.lang.System.getProperty("os.name").Should().StartWith("Mac OS X");
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

        [TestMethod]
        public void NanoTimeShouldReportAtLeastASecond()
        {
            var startTime = global::java.lang.System.nanoTime();
            Thread.Sleep(TimeSpan.FromSeconds(1.1));
            var totalTime = global::java.lang.System.nanoTime() - startTime;
            totalTime.Should().BeGreaterThanOrEqualTo(1000000000L);
        }

    }

}
