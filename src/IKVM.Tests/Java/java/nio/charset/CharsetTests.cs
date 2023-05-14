using java.nio.charset;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.charset
{

    [TestClass]
    public class CharsetTests
    {

        [TestMethod]
        public void CanGetAvailableCharsets()
        {
            Charset.availableCharsets();
        }

    }

}
