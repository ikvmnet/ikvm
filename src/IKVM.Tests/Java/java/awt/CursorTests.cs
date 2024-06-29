using System.Runtime.InteropServices;

using FluentAssertions;

using java.awt;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.awt
{

    [TestClass]
    public class CursorTests
    {

        [TestMethod]
        public void CanGetSystemCopyDropCursor()
        {
            // skip due to hang on OS X
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            Cursor.getSystemCustomCursor("CopyDrop.32x32").Should().NotBeNull();
        }

    }

}
