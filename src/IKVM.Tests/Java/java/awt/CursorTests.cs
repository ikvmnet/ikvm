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
            Cursor.getSystemCustomCursor("CopyDrop.32x32").Should().NotBeNull();
        }

    }

}
