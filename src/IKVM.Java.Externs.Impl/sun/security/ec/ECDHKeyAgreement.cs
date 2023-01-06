using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using java.security;

namespace IKVM.Java.Externs.Impl.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECDHKeyAgreement'.
    /// </summary>
    static class ECDHKeyAgreement
    {

#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER

        /// <summary>
        /// Creates a <see cref="ECDiffieHellman"/> instance for the given curve and public key material.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        static ECDiffieHellman CreateECDiffieHellmanForPublicKey(ECCurve curve, byte[] d)
        {
#if NETCOREAPP3_1
            // NET Core 3.1 does not support an empty Q
            // However, on Windows, a fake Q the length of D works
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var X = new byte[d.Length];
                var Y = X;
                return ECDiffieHellman.Create(new ECParameters() { Curve = curve, D = d, Q = new ECPoint() { X = X, Y = Y } });
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
#else
            return ECDiffieHellman.Create(new ECParameters() { Curve = curve, D = d });
#endif
        }

#endif

        /// <summary>
        /// Implements the native method for 'deriveKey'.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="w"></param>
        /// <param name="encodedParams"></param>
        /// <returns></returns>
        public static byte[] deriveKey(byte[] s, byte[] w, byte[] encodedParams)
        {
            if (encodedParams == null)
                throw new InvalidAlgorithmParameterException();

#if NET461
            throw new NotImplementedException();
#elif NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            // read in keys
            using var pub = ECDiffieHellman.Create(new ECParameters() { Curve = curve, Q = ECUtil.ImportECPoint(w) });
            using var prv = CreateECDiffieHellmanForPublicKey(curve, s.AsSpan().Slice(1).ToArray());

            // derive material and return
            return prv.DeriveKeyMaterial(pub.PublicKey);
#endif
        }

    }

}
