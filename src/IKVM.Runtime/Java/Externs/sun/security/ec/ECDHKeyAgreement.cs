using System;
using System.Buffers;
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

        readonly static RandomNumberGenerator rng = RandomNumberGenerator.Create();

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
                // read in keys
                using var prv = ECUtil.ImportECDiffieHellmanPrivateKey(curve, s);
                using var pub = ECUtil.ImportECDiffieHellmanPublicKey(curve, ECUtil.ImportECPoint(w));

                // generate new seed
                var seed = (byte[])null;
                try
                {
                    seed = ArrayPool<byte>.Shared.Rent(64);
                    rng.GetBytes(seed);

#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
                    var secret = prv.DeriveKeyTls(pub, Array.Empty<byte>(), seed);
#else
                    ((ECDiffieHellmanCng)prv).Label = Array.Empty<byte>();
                    ((ECDiffieHellmanCng)prv).Seed = seed;
                    ((ECDiffieHellmanCng)prv).HashAlgorithm = ECUtil.GetCurveCngAlgorithm(curve, ECUtil.ECMode.ECDH);
                    ((ECDiffieHellmanCng)prv).KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Tls;
                    var secret = prv.DeriveKeyMaterial(pub);
#endif
                    return secret;
                }
                finally
                {
                    if (seed != null)
                        ArrayPool<byte>.Shared.Return(seed);
                }
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
