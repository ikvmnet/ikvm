using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.javax.crypto
{

    [TestClass]
    public class CipherTests
    {

        [TestMethod]
        public void Can_get_standard_ciphers()
        {
            global::javax.crypto.Cipher.getInstance("RSA/ECB/PKCS1Padding").Should().NotBeNull();
            global::javax.crypto.Cipher.getInstance("AES/ECB/PKCS5Padding").Should().NotBeNull();
        }

    }

}
