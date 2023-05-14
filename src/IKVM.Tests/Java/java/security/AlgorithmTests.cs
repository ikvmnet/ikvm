using System.Text;

using FluentAssertions;

using java.security;

using javax.crypto;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.security
{

    [TestClass]
    public class AlgorithmTests
    {

        [DataTestMethod]
        [DataRow("DSA", 2048, "SHA1withDSA")]
        [DataRow("DSA", 2048, "SHA256withDSA")]
        [DataRow("RSA", 2048, "SHA1withRSA")]
        [DataRow("RSA", 2048, "SHA256withRSA")]
        //[DataRow("EC", 256, "SHA1withECDSA")]
        //[DataRow("EC", 384, "SHA1withECDSA")]
        //[DataRow("EC", 521, "SHA1withECDSA")]
        //[DataRow("EC", 256, "SHA256withECDSA")]
        //[DataRow("EC", 384, "SHA256withECDSA")]
        //[DataRow("EC", 521, "SHA256withECDSA")]
        public void CanCreateKeysAndSignAndVerify(string keyAlgorithm, int keySize, string signatureAlgorithm)
        {
            // we do this many times because differences in the values can make padding required or not
            for (int i = 0; i < 16; i++)
            {
                var text = Encoding.UTF8.GetBytes("TEST");

                // create a new key pair
                var keyGenerator = KeyPairGenerator.getInstance(keyAlgorithm);
                keyGenerator.initialize(keySize);
                var keyPair = keyGenerator.generateKeyPair();
                var privateKey = keyPair.getPrivate();
                var publicKey = keyPair.getPublic();

                // sign some test data with the private key
                var signSignature = Signature.getInstance(signatureAlgorithm);
                signSignature.initSign(privateKey);
                signSignature.update(text);
                var signatureText = signSignature.sign();

                // verify the signature with the public key
                var verifySignature = Signature.getInstance(signatureAlgorithm);
                verifySignature.initVerify(publicKey);
                verifySignature.update(text);
                var verifyResult = verifySignature.verify(signatureText);
                verifyResult.Should().BeTrue();
            }
        }

        [DataTestMethod]
        [DataRow("RSA", 2048, "RSA")]
        public void CanEncryptAndDecrypt(string keyAlgorithm, int keySize, string cipherAlgorithm)
        {
            // we do this many times because differences in the values can make padding required or not
            for (int i = 0; i < 16; i++)
            {
                var text = Encoding.UTF8.GetBytes("TEST");

                // create a new key pair
                var keyGenerator = KeyPairGenerator.getInstance(keyAlgorithm);
                keyGenerator.initialize(keySize);
                var keyPair = keyGenerator.generateKeyPair();
                var privateKey = keyPair.getPrivate();
                var publicKey = keyPair.getPublic();

                // encrypt data with the given algorithm
                var encrypt = Cipher.getInstance(cipherAlgorithm);
                encrypt.init(Cipher.ENCRYPT_MODE, privateKey);
                var encryptData = encrypt.doFinal(text);

                // decrypt data with the given algorithm
                var decrypt = Cipher.getInstance(cipherAlgorithm);
                decrypt.init(Cipher.DECRYPT_MODE, publicKey);
                var decryptText = decrypt.doFinal(encryptData);
                decryptText.Should().BeEquivalentTo(text);
            }
        }

    }

}
