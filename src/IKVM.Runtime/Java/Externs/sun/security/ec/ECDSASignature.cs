using System;
using System.Security.Cryptography;

#if FIRST_PASS == false

using java.lang;
using java.security;

#endif

namespace IKVM.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECDSASignature'.
    /// </summary>
    static class ECDSASignature
    {

        /// <summary>
        /// Implements the native method for 'signDigest'.
        /// </summary>
        /// <param name="digest"></param>
        /// <param name="s"></param>
        /// <param name="encodedParams"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static byte[] signDigest(byte[] digest, byte[] s, byte[] encodedParams, byte[] seed)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            try
            {
                // import key as private key
                var p = new ECParameters();
                p.Curve = curve;
                p.D = s;
                using var dsa = ECDsa.Create(p);

                // sign the digest
                var r = dsa.SignHash(digest);
                return r;
            }
            catch (CryptographicException e)
            {
                throw new IllegalStateException(e);
            }
#else
            throw new NotImplementedException();
#endif
#endif
        }

        /// <summary>
        /// Implements the native method for 'verifySignedDigest'.
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="digest"></param>
        /// <param name="w"></param>
        /// <param name="encodedParams"></param>
        /// <returns></returns>
        public static bool verifySignedDigest(byte[] signature, byte[] digest, byte[] w, byte[] encodedParams)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            try
            {
                // import key as private key
                var p = new ECParameters();
                p.Curve = curve;
                p.Q = ECUtil.ImportECPoint(w);
                using var dsa = ECDsa.Create(p);

                // verify the signature
                var r = dsa.VerifyHash(digest, signature);
                return r;
            }
            catch (CryptographicException e)
            {
                throw new IllegalStateException(e);
            }
#else
            throw new NotImplementedException();
#endif
#endif
        }

    }

}
