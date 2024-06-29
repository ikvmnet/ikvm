using java.awt;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.awt
{

    [TestClass]
    public class ToolkitTests
    {

        [TestMethod]
        public void CanGetDefaultToolkit()
        {
            Toolkit.getDefaultToolkit();
        }

    }

}
