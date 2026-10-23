using FluentAssertions;

using IKVM.ByteCode;
using UnsupportedClassVersionException = IKVM.CoreLib.Linking.UnsupportedClassVersionException;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Linking
{

    [TestClass]
    public class UnsupportedClassVersionExceptionTests
    {

        [DataTestMethod]
        [DataRow(45, 0, true)]
        [DataRow(45, 2, true)]
        [DataRow(45, 3, true)]
        [DataRow(49, 0, true)]
        [DataRow(52, 0, true)]
        [DataRow(44, 0, false)]
        [DataRow(53, 0, false)]
        [DataRow(55, 0, false)]
        public void IsSupportedShouldMatchHotSpotRange(int major, int minor, bool expected)
        {
            UnsupportedClassVersionException.IsSupported(new ClassFormatVersion((ushort)major, (ushort)minor)).Should().Be(expected);
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
            UnsupportedClassVersionException.GetJavaVersionName(new ClassFormatVersion((ushort)major, (ushort)minor)).Should().Be(expected);
        }

        [TestMethod]
        public void MessageShouldNameJavaVersionAndSupportedMaximum()
        {
            UnsupportedClassVersionException.FormatMessage(new ClassFormatVersion(55, 0)).Should().Be("Class file version 55.0 (Java 11) is not supported; IKVM supports class files up to version 52.0 (Java 8)");
        }

        [TestMethod]
        public void MessageShouldNameSupportedMinimumForOldVersions()
        {
            UnsupportedClassVersionException.FormatMessage(new ClassFormatVersion(44, 0)).Should().Be("Class file version 44.0 (unknown Java version) is not supported; IKVM supports class files from version 45.0 (Java 1.0) up to version 52.0 (Java 8)");
        }

        [TestMethod]
        public void ExceptionMessageShouldMatchHotSpot()
        {
            var e = new UnsupportedClassVersionException(new ClassFormatVersion(55, 0), "foo.Bar");
            e.Message.Should().Be("foo/Bar has been compiled by a more recent version of the Java Runtime (class file version 55.0), this version of the Java Runtime only recognizes class file versions up to 52.0");
            e.Version.Should().Be(new ClassFormatVersion(55, 0));
        }

        [TestMethod]
        public void ExceptionMessageWithoutClassNameShouldMatchHotSpot()
        {
            var e = new UnsupportedClassVersionException(new ClassFormatVersion(55, 0));
            e.Message.Should().Be("Unsupported class file version 55.0, this version of the Java Runtime only recognizes class file versions up to 52.0");
        }

    }

}
