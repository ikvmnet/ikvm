using FluentAssertions;

using java.security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.security
{

    [TestClass]
    public class SecureRandomTests
    {

        [TestMethod]
        public void CanGenerateBytes()
        {
            var random = new SecureRandom();
            var bytes = new byte[32];
            random.nextBytes(bytes);
        }

        [TestMethod]
        public void CanGenerateSeedBytes()
        {
            var random = new SecureRandom();
            var bytes = random.generateSeed(32);
            bytes.Should().HaveCount(32);
        }

        [TestMethod]
        public void CanGetAndUseInstance()
        {
            var rnd = new SecureRandom();
            rnd.Should().NotBeNull();
            rnd.generateSeed(32).Should().HaveCount(32);

            var b = new byte[32];
            rnd.nextBytes(b);

            rnd.nextBoolean();
            rnd.nextInt();
            rnd.nextFloat();
            rnd.nextDouble();
            rnd.nextLong();
        }

        [TestMethod]
        public void CanGetAndUseStrongInstance()
        {
            var rnd = SecureRandom.getInstanceStrong();
            rnd.Should().NotBeNull();
            rnd.generateSeed(32).Should().HaveCount(32);

            var b = new byte[32];
            rnd.nextBytes(b);

            rnd.nextBoolean();
            rnd.nextInt();
            rnd.nextLong();
            rnd.nextFloat();
            rnd.nextDouble();
        }

    }

}
