using java.security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.sun.security.ec
{

    [TestClass]
    public class ECKeyPairGeneratorTests
    {

        [TestMethod]
        [Ignore]
        public void CanGenerateKeyPair()
        {
            var kpg = KeyPairGenerator.getInstance("EC");
            kpg.initialize(256);
            var key = kpg.generateKeyPair();
            var prv = key.getPrivate();
            var pub = key.getPublic();
        }

    }

}
