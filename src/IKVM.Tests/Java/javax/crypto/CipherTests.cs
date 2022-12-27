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

        [TestMethod]
        public void Should_return_high_max_key_size()
        {
            global::javax.crypto.Cipher.getMaxAllowedKeyLength("AES").Should().BeGreaterThan(128);
        }

    }

}
