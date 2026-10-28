using System;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// A native Java searches PATH for the dependencies of a JNI library on Windows. Loads a library from one
        /// directory whose dependency is only available from another directory on PATH.
        /// </summary>
        [TestMethod]
        public void LoadShouldFindDependencyOnPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            var rid = "win-" + RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant();
            string Find(string name) => Directory.GetFiles(AppContext.BaseDirectory, name, SearchOption.AllDirectories).First(i => i.Contains(Path.DirectorySeparatorChar + rid + Path.DirectorySeparatorChar));

            // place the library and its dependency in separate directories
            var root = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var libDir = Directory.CreateDirectory(Path.Combine(root, "lib")).FullName;
            var depDir = Directory.CreateDirectory(Path.Combine(root, "dep")).FullName;
            File.Copy(Find("ikvm-tests-dependent.dll"), Path.Combine(libDir, "ikvm-tests-dependent.dll"));
            File.Copy(Find("ikvm-tests-dep.dll"), Path.Combine(depDir, "ikvm-tests-dep.dll"));

            var path = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", path + Path.PathSeparator + depDir);

            try
            {
                global::java.lang.System.load(Path.Combine(libDir, "ikvm-tests-dependent.dll"));
            }
            finally
            {
                Environment.SetEnvironmentVariable("PATH", path);
            }
        }

    }

}
