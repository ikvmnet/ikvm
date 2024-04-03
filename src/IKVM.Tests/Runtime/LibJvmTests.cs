using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class LibJvmTests
    {

        [TestMethod]
        public void LoadLibraryShouldThrowLinkError()
        {
            var libpath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\badlib.dll" : "/libbadlib.so";
            LibJvm.Instance.Invoking(i => i.JVM_LoadLibrary(libpath)).Should().ThrowExactly<java.lang.UnsatisfiedLinkError>();
        }

    }

}
