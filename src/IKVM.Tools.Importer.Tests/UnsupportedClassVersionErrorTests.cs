using FluentAssertions;

using IKVM.ByteCode;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Importer.Tests
{

    [TestClass]
    public class UnsupportedClassVersionErrorTests
    {

        [DataTestMethod]
        [DataRow(45, 3, true)]
        [DataRow(49, 0, true)]
        [DataRow(52, 0, true)]
        [DataRow(45, 2, false)]
        [DataRow(53, 0, false)]
        [DataRow(55, 0, false)]
        public void IsSupportedShouldMatchJava8Range(int major, int minor, bool expected)
        {
            UnsupportedClassVersionError.IsSupported(new ClassFormatVersion((ushort)major, (ushort)minor)).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(45, 0, "Java 1.0")]
        [DataRow(45, 3, "Java 1.1")]
        [DataRow(48, 0, "Java 1.4")]
        [DataRow(49, 0, "Java 5")]
        [DataRow(52, 0, "Java 8")]
        [DataRow(53, 0, "Java 9")]
        [DataRow(55, 0, "Java 11")]
        [DataRow(65, 0, "Java 21")]
        [DataRow(61, 65535, "Java 17 preview")]
        public void GetJavaVersionNameShouldMapMajorVersion(int major, int minor, string expected)
        {
            UnsupportedClassVersionError.GetJavaVersionName(new ClassFormatVersion((ushort)major, (ushort)minor)).Should().Be(expected);
        }

        [TestMethod]
        public void MessageShouldNameJavaVersionAndSupportedMaximum()
        {
            UnsupportedClassVersionError.FormatMessage(new ClassFormatVersion(55, 0)).Should().Be("Class file version 55.0 (Java 11) is not supported; IKVM supports class files up to version 52.0 (Java 8)");
        }

        [TestMethod]
        public void MessageShouldNameSupportedMinimumForOldVersions()
        {
            UnsupportedClassVersionError.FormatMessage(new ClassFormatVersion(45, 0)).Should().Be("Class file version 45.0 (Java 1.0) is not supported; IKVM supports class files from version 45.3 (Java 1.1) up to version 52.0 (Java 8)");
        }

        [TestMethod]
        public void ExceptionMessageShouldIncludeClassName()
        {
            var e = new UnsupportedClassVersionError(new ClassFormatVersion(55, 0), "foo.Bar");
            e.Message.Should().Be("foo.Bar: Class file version 55.0 (Java 11) is not supported; IKVM supports class files up to version 52.0 (Java 8)");
            e.Version.Should().Be(new ClassFormatVersion(55, 0));
        }

    }

}
