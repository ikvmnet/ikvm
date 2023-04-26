using java.io;
using java.nio.file;
using java.security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.security
{

    [TestClass]
    public class KeystoreTests
    {

        public TestContext TestContext { get; set; }

        [DataTestMethod]
        [DataRow("JKS", "jks")]
        [DataRow("JCEKS", "jceks")]
        [DataRow("PKCS12", "p12")]
        public void CanInitKeyStore(string type, string ext)
        {
            var ks = KeyStore.getInstance(type);
            ks.load(null, null);

            var dir = Paths.get(TestContext.TestRunDirectory, "CanInitKeyStore", type).toFile();
            dir.mkdirs();

            using (var stream = new FileOutputStream(new File(dir, $"empty.{ext}")))
                ks.store(stream, "changeit".ToCharArray());
        }

        [DataTestMethod]
        [DataRow("JKS", "jks")]
        [DataRow("JCEKS", "jceks")]
        [DataRow("PKCS12", "p12")]
        public void CanLoadKeyStore(string type, string ext)
        {
            var dir = Paths.get(TestContext.TestRunDirectory, "CanLoadKeyStore", type).toFile();
            dir.mkdirs();

            using (var stream = new FileOutputStream(new File(dir, $"empty.{ext}")))
            {
                var ks = KeyStore.getInstance(type);
                ks.load(null, null);
                ks.store(stream, "changeit".ToCharArray());
            }

            using (var stream = new FileInputStream(new File(dir, $"empty.{ext}")))
            {
                var ks = KeyStore.getInstance(type);
                ks.load(stream, "changeit".ToCharArray());
            }
        }

        [TestMethod]
        public void CanLoadP12KeyStoreWithJKS()
        {
            var dir = Paths.get(TestContext.TestRunDirectory, "CanLoadP12KeyStoreWithJKS").toFile();
            dir.mkdirs();

            using (var stream = new FileOutputStream(new File(dir, "empty.p12")))
            {
                var ks = KeyStore.getInstance("PKCS12");
                ks.load(null, null);
                ks.store(stream, "changeit".ToCharArray());
            }

            using (var stream = new FileInputStream(new File(dir, "empty.p12")))
            {
                var ks = KeyStore.getInstance("JKS");
                ks.load(stream, "changeit".ToCharArray());
            }
        }

    }

}