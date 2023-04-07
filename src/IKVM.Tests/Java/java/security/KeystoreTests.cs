using System;

using java.io;
using java.security;
using java.security.cert;

using javax.crypto;
using javax.security.auth.callback;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static java.security.KeyStore;

namespace IKVM.Tests.Java.java.security
{

    [TestClass]
    public class KeystoreTests
    {

        private static readonly char[] PASSWORD = "changeit".ToCharArray();
        private static readonly String DIR = global::java.lang.System.getProperty("test.src", ".");
        // This is an arbitrary X.509 certificate
        private static readonly String CERT_FILE = "trusted.pem";

        [TestMethod]
        public void TestKeystoreCompat()
        {
            init("empty.jks", "JKS");
            init("empty.jceks", "JCEKS");
            init("empty.p12", "PKCS12");

            load("empty.jks", "JKS");
            load("empty.jceks", "JCEKS");
            load("empty.p12", "PKCS12");
            load("empty.p12", "JKS"); // test compatibility mode
            load("empty.jks", "PKCS12", true); // test without compatibility mode
            load("empty.jks", "JKS", false); // test without compatibility mode
            load("empty.p12", "JKS", true); // test without compatibility mode
            load("empty.p12", "PKCS12", false); // test without compatibility mode

            build("empty.jks", "JKS", true);
            build("empty.jks", "JKS", false);
            build("empty.jceks", "JCEKS", true);
            build("empty.jceks", "JCEKS", false);
            build("empty.p12", "PKCS12", true);
            build("empty.p12", "PKCS12", false);

            // Testing keystores containing an X.509 certificate

            X509Certificate cert = loadCertificate(CERT_FILE);
            init("onecert.jks", "JKS", cert);
            init("onecert.jceks", "JCEKS", cert);
            init("onecert.p12", "PKCS12", cert);

            load("onecert.jks", "JKS");
            load("onecert.jceks", "JCEKS");
            load("onecert.p12", "PKCS12");
            load("onecert.p12", "JKS"); // test compatibility mode
            load("onecert.jks", "PKCS12", true); // test without compatibility mode
            load("onecert.jks", "JKS", false); // test without compatibility mode
            load("onecert.p12", "JKS", true); // test without compatibility mode
            load("onecert.p12", "PKCS12", false); // test without compatibility mode

            build("onecert.jks", "JKS", true);
            build("onecert.jks", "JKS", false);
            build("onecert.jceks", "JCEKS", true);
            build("onecert.jceks", "JCEKS", false);
            build("onecert.p12", "PKCS12", true);
            build("onecert.p12", "PKCS12", false);

            // Testing keystores containing a secret key

            SecretKey key = generateSecretKey("AES", 128);
            init("onekey.jceks", "JCEKS", key);
            init("onekey.p12", "PKCS12", key);

            load("onekey.jceks", "JCEKS");
            load("onekey.p12", "PKCS12");
            load("onekey.p12", "JKS"); // test compatibility mode
            load("onekey.p12", "JKS", true); // test without compatibility mode
            load("onekey.p12", "PKCS12", false); // test without compatibility mode

            build("onekey.jceks", "JCEKS", true);
            build("onekey.jceks", "JCEKS", false);
            build("onekey.p12", "PKCS12", true);
            build("onekey.p12", "PKCS12", false);
        }

        // Instantiate an empty keystore using the supplied keystore type
        private static void init(String file, String type)
        {
            KeyStore ks = KeyStore.getInstance(type);
            ks.load(null, null);
            using (OutputStream stream = new FileOutputStream(file))
            {
                ks.store(stream, PASSWORD);
            }
        }

        // Instantiate a keystore using the supplied keystore type & create an entry
        private static void init(String file, String type, X509Certificate cert)
        {
            KeyStore ks = KeyStore.getInstance(type);
            ks.load(null, null);
            ks.setEntry("mycert", new KeyStore.TrustedCertificateEntry(cert), null);
            using (OutputStream stream = new FileOutputStream(file))
            {
                ks.store(stream, PASSWORD);
            }
        }

        // Instantiate a keystore using the supplied keystore type & create an entry
        private static void init(String file, String type, SecretKey key)
        {
            KeyStore ks = KeyStore.getInstance(type);
            ks.load(null, null);
            ks.setEntry("mykey", new KeyStore.SecretKeyEntry(key),
                    new PasswordProtection(PASSWORD));
            using (OutputStream stream = new FileOutputStream(file))
            {
                ks.store(stream, PASSWORD);
            }
        }

        // Instantiate a keystore by probing the supplied file for the keystore type
        private static void build(String file, String type, bool usePassword)
        {

            Builder builder;
            if (usePassword)
            {
                builder = Builder.newInstance(type, null, new File(file),
                    new PasswordProtection(PASSWORD));
            }
            else
            {
                builder = Builder.newInstance(type, null, new File(file),
                    new CallbackHandlerProtection(new DummyHandler()));
            }
            KeyStore ks = builder.getKeyStore();
            if (!type.Equals(ks.getType(), StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("ERROR: expected a " + type + " keystore, " +
                    "got a " + ks.getType() + " keystore instead");
            }
        }

        // Load the keystore entries
        private static void load(String file, String type)
        {
            KeyStore ks = KeyStore.getInstance(type);
            using (InputStream stream = new FileInputStream(file))
            {
                ks.load(stream, PASSWORD);
            }
            if (!type.Equals(ks.getType(), StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("ERROR: expected a " + type + " keystore, " +
                    "got a " + ks.getType() + " keystore instead");
            }
        }

        // Load the keystore entries (with compatibility mode disabled)
        private static void load(String file, String type, bool expectFailure)
        {
            Security.setProperty("keystore.type.compat", "false");
            try
            {
                load(file, type);
                if (expectFailure)
                {
                    throw new Exception("ERROR: expected load to fail but it didn't");
                }
            }
            catch (IOException e)
            {
                if (!expectFailure)
                {
                    throw e;
                }
            }
            finally
            {
                Security.setProperty("keystore.type.compat", "true");
            }
        }

        // Read an X.509 certificate from the supplied file
        private static X509Certificate loadCertificate(String certFile)
        {
            X509Certificate cert = null;
            using (FileInputStream certStream =
                new FileInputStream(DIR + "/" + certFile))
            {
                CertificateFactory factory =
                    CertificateFactory.getInstance("X.509");
                return (X509Certificate)factory.generateCertificate(certStream);
            }
        }

        // Generate a secret key using the supplied algorithm name and key size
        private static SecretKey generateSecretKey(String algorithm, int size)
        {
            KeyGenerator generator = KeyGenerator.getInstance(algorithm);
            generator.init(size);
            return generator.generateKey();
        }

        private sealed class DummyHandler : CallbackHandler
        {
            public void handle(Callback[] callbacks)
            {
                for (int i = 0; i < callbacks.Length; i++)
                {
                    Callback cb = callbacks[i];
                    if (cb is PasswordCallback pcb)
                    {
                        pcb.setPassword(PASSWORD);
                        break;
                    }
                }
            }
        }
    }
}