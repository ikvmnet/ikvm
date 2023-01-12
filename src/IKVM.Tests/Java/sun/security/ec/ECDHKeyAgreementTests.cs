using FluentAssertions;

using java.security;

using javax.crypto;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.sun.security.ec
{

    [TestClass]
    public class ECDHKeyAgreementTests
    {

        [TestMethod]
        [Ignore]
        public void CanGenerateAgreementBetweenParties()
        {
            // party A
            var kpgA = KeyPairGenerator.getInstance("EC");
            kpgA.initialize(256);
            var keyA = kpgA.generateKeyPair();
            var prvA = keyA.getPrivate();
            var pubA = keyA.getPublic();

            // party B
            var kpgB = KeyPairGenerator.getInstance("EC");
            kpgB.initialize(256);
            var keyB = kpgB.generateKeyPair();
            var prvB = keyB.getPrivate();
            var pubB = keyB.getPublic();

            // agreement of party A with party B
            var kaA = KeyAgreement.getInstance("ECDH");
            kaA.init(prvA);
            kaA.doPhase(pubB, true);
            var secA = kaA.generateSecret();

            // agreement of party B with party A
            var kaB = KeyAgreement.getInstance("ECDH");
            kaB.init(prvB);
            kaB.doPhase(pubA, true);
            var secB = kaB.generateSecret();

            // check that the same agreement resulted
            secA.Should().HaveSameCount(secB);
            secA.Should().BeEquivalentTo(secB);
        }

    }

}
