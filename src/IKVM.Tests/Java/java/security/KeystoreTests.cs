using java.io;
using java.security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.security
{

    [TestClass]
    public class KeystoreTests
    {

        [DataTestMethod]
        [DataRow("JKS", "jks")]
        [DataRow("JCEKS", "jceks")]
        [DataRow("PKCS12", "p12")]
        public void CanInitKeyStore(string type, string ext)
        {
            var ks = KeyStore.getInstance(type);
            ks.load(null, null);

            var dir = new File(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "CanInitKeyStore", type));
            System.IO.Directory.Delete(dir.getPath());
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
            var dir = new File(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "CanLoadKeyStore", type));
            System.IO.Directory.Delete(dir.getPath());
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
            var dir = new File(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "CanLoadP12KeyStoreWithJKS"));
            System.IO.Directory.Delete(dir.getPath());
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