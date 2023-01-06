using System;
using System.Security.Cryptography;

using java.security;

namespace IKVM.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECKeyPairGenerator'.
    /// </summary>
    static class ECKeyPairGenerator
    {

        /// <summary>
        /// Implements the native method for 'generateECKeyPair'.
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="encodedParams"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static object[] generateECKeyPair(int keySize, byte[] encodedParams, byte[] seed)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETFRAMEWORK
            throw new NotImplementedException();
#else
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            // create a new ECDsa instance, set parameters, and generate key; then export results
            using var dsa = ECDsa.Create();
            dsa.KeySize = keySize;
            dsa.GenerateKey(curve);
            var p = dsa.ExportParameters(true);

            // result is a two item array, with the private key followed by exported ECPoint
            return new object[] { p.D, ECUtil.ExportECPoint(p.Q) };
#endif
#endif
        }

    }

}
