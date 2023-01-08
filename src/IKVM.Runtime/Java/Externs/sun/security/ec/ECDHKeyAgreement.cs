using System;
using System.Security.Cryptography;

#if FIRST_PASS == false

using java.lang;
using java.security;

#endif

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

            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            try
            {
                // array is the result of BigInteger.toByteArray, which may include a sign
                if (s[0] == 0)
                {
                    var t = new byte[s.Length - 1];
                    s.AsSpan().Slice(1).CopyTo(t);
                    s = t;
                }

                // read in keys
                using var prv = ECUtil.ImportECDiffieHellmanPrivateKey(curve, s);
                using var pub = ECUtil.ImportECDiffieHellmanPublicKey(curve, ECUtil.ImportECPoint(w));

                // derive material and return
                var mat = prv.DeriveKeyMaterial(pub);
                return mat;
            }
            catch (CryptographicException e)
            {
                throw new KeyException(e);
            }
            catch (PlatformNotSupportedException e)
            {
                throw new IllegalStateException(e);
            }
#endif
        }

    }

}
