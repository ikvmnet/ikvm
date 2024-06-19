using FluentAssertions;

using java.awt.dnd;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.awt.dnd
{

    [TestClass]
    public class DragSourceTests
    {

        [TestMethod]
        public void CanGetDragThreshold()
        {
            DragSource.getDragThreshold();
        }

        [TestMethod]
        public void CanGetDefaultCopyDropCursor()
        {
            DragSource.DefaultCopyDrop.Should().NotBeNull();
        }

        [TestMethod]
        public void CanGetDefaultCopyNoDropCursor()
        {
            DragSource.DefaultCopyNoDrop.Should().NotBeNull();
        }

    }

}
