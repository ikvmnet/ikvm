
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.javax.crypto
{

    [TestClass]
    public class CipherTests
    {

        [TestMethod]
        public void Can_get_standard_ciphers()
        {
            global::javax.crypto.Cipher.getInstance("RSA/ECB/PKCS1Padding");
        }

    }

}
