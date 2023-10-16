using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class RuntimeUtilTests
    {

        [TestMethod]
        public void SupportedRuntimeIdentifiersShouldHaveAny()
        {
            RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("any");
        }

        [TestMethod]
        public void SupportedRuntimeIdentifiersShouldContainLocalGenericPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("win-x86");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("win-x64");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("win-arm");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("win-arm64");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("linux-x64");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("linux-arm");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("linux-arm64");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("osx-x64");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                RuntimeUtil.SupportedRuntimeIdentifiers.Should().Contain("osx-arm64");
        }

    }

}
