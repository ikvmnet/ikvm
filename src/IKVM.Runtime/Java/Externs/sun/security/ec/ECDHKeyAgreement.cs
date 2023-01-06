using System;
using System.Security.Cryptography;

using java.security;

namespace IKVM.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECDHKeyAgreement'.
    /// </summary>
    static class ECDHKeyAgreement
    {

        /// <summary>
        /// Implements the native method for 'deriveKey'.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="w"></param>
        /// <param name="encodedParams"></param>
        /// <returns></returns>
        public static byte[] deriveKey(byte[] s, byte[] w, byte[] encodedParams)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (encodedParams == null)
                throw new InvalidAlgorithmParameterException();

#if NETFRAMEWORK
            throw new NotImplementedException();
#else
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            // read in key and derive material
            using var pub = ECDiffieHellman.Create(new ECParameters() { Curve = curve, Q = ECUtil.ImportECPoint(w) });
            ECParameters
            // to create the private key, we abuse a ctor that accepts parameters to initialize the instance, but replace the private key
            var prm = pub.ExportParameters(false);
            prm.D = s.AsSpan().Slice(1).ToArray();
            using var prv = ECDiffieHellman.Create(prm);
            var mat = prv.DeriveKeyMaterial(pub.PublicKey);

            return mat;
#endif
#endif
        }

    }

}
