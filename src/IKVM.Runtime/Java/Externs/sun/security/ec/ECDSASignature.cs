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
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            try
            {
                using var dsa = ECUtil.ImportECDsaPrivateKey(curve, s);
                var r = dsa.SignHash(digest);
                return r;
            }
            catch (CryptographicException e)
            {
                throw new IllegalStateException(e);
            }
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
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            // lookup key size as we may need to pad the signature
            var keySize = ECUtil.GetCurveKeySize(curve);
            if (keySize == 0)
                throw new InvalidAlgorithmParameterException();

            // signature is r and s concatinated
            var keyLength = (keySize + 7) >> 3;
            var sigLength = signature.Length / 2;
            if (sigLength != keyLength)
            {
                var r = ECUtil.NormalizeLength((ReadOnlySpan<byte>)signature.AsSpan().Slice(0, sigLength), keyLength);
                var s = ECUtil.NormalizeLength((ReadOnlySpan<byte>)signature.AsSpan().Slice(sigLength, sigLength), keyLength);

                // copy to appropriately sized new signature
                var tmp = new byte[r.Length + s.Length];
                r.CopyTo(tmp);
                s.CopyTo(tmp.AsSpan().Slice(keyLength));
                signature = tmp;
            }

            try
            {
                using var dsa = ECUtil.ImportECDsaPublicKey(curve, w);
                var result = dsa.VerifyHash(digest, signature);
                return result;
            }
            catch (CryptographicException e)
            {
                throw new IllegalStateException(e);
            }
#endif
        }

    }

}
