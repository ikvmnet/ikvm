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

    }

}
