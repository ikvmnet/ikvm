using System.Runtime.InteropServices;

using FluentAssertions;

using java.nio.file;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.file
{

    [TestClass]
    public class PathTests
    {

        [DataTestMethod]
        [DataRow("foo", "C:\\foo")]
        [DataRow("D:\\bar", "D:\\bar")]
        [DataRow("\\\\server\\share\\bar", "\\\\server\\share\\bar")]
        [DataRow("C:foo", "C:\\foo")]
        [DataRow("D:foo", "D:foo")]
        [DataRow("", "C:\\")]
        public void CanResolveFromWindowsDrive(string other, string expected)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            Paths.get("C:\\").resolve(other).ToString().Should().Be(expected);
        }

        [DataTestMethod]
        //[DataRow("foo", "C:foo")] currently broken
        [DataRow("", "C:")]
        public void CanResolveFromWindowsDriveName(string other, string expected)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            Paths.get("C:").resolve(other).ToString().Should().Be(expected);
        }

    }

}
