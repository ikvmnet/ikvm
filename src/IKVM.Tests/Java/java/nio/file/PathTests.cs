﻿using System.Runtime.InteropServices;

using FluentAssertions;

using java.nio.file;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.file
{

    [TestClass]
    public class PathTests
    {

        [DataTestMethod]
        [DataRow("C:\\", "foo", "C:\\foo")]
        [DataRow("C:\\", "D:\\bar", "D:\\bar")]
        [DataRow("C:\\", "\\\\server\\share\\bar", "\\\\server\\share\\bar")]
        [DataRow("C:\\", "C:foo", "C:\\foo")]
        [DataRow("C:\\", "D:foo", "D:foo")]
        [DataRow("C:\\", "", "C:\\")]
        //[DataRow("C:", "foo", "C:foo")]
        [DataRow("C:", "", "C:")]
        public void CanResolveWindowsPaths(string path, string other, string expected)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            Paths.get(path).resolve(other).ToString().Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("/tmp", "foo", "/tmp/foo")]
        [DataRow("/tmp", "/foo", "/foo")]
        [DataRow("/tmp", "", "/tmp")]
        [DataRow("tmp", "foo", "tmp/foo")]
        [DataRow("tmp", "/foo", "/foo")]
        [DataRow("tmp", "", "tmp")]
        [DataRow("", "", "")]
        [DataRow("", "foo", "foo")]
        [DataRow("", "/foo", "/foo")]
        public void CanResolveUnixPaths(string path, string other, string expected)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            Paths.get(path).resolve(other).ToString().Should().Be(expected);
        }

        [TestMethod]
        public void ShouldAppendSlashToUriForExistingDirectory()
        {
            var t = System.IO.Path.GetTempPath().TrimEnd(System.IO.Path.DirectorySeparatorChar);
            var p = Paths.get(t);
            var u = p.toUri();
            u.ToString().Should().EndWith("/");
        }

        [TestMethod]
        public void ShouldNotAppendSlashToUriForMissingDirectory()
        {
            var r = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"C:\missing" : @"/missing";
            var p = Paths.get(r);
            var u = p.toUri();
            u.ToString().Should().NotEndWith("/");
        }

    }

}
