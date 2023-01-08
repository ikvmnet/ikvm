using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using java.lang;
using java.security;

namespace IKVM.Java.Externs.Impl.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECDHKeyAgreement'.
    /// </summary>
    static class ECDHKeyAgreement
    {


        /// <summary>
        /// Creates a <see cref="ECDiffieHellman"/> instance for the given curve and private key material.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        static ECDiffieHellman ImportECDiffieHellmanPrivateKey(ECCurve curve, byte[] s)
        {
#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            // NET Core 3.1 does not support an empty Q
            // However, on Windows, a fake Q the length of D works
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var X = new byte[s.Length];
                var Y = X;
                return ECDiffieHellman.Create(new ECParameters() { Curve = curve, D = s, Q = new ECPoint() { X = X, Y = Y } });
            }
            else
            {
                // fail on linux and mac os for now
                throw new PlatformNotSupportedException();
            }
#else
            var algorithm = ECUtil.EcdsaCurveNameToAlgorithm(curve.FriendlyName, true);
            if (algorithm == null)
                throw new InvalidAlgorithmParameterException();

            // properties of the key
            var keyLength = s.Length;
            var magic = ECUtil.EcdsaCurveNameToPrivateMagic(curve.FriendlyName, true);

            // CNG blob: magic + key size + X + Y + D
            var blob = new byte[sizeof(uint) + sizeof(uint) + keyLength + keyLength + keyLength];
            var span = blob.AsSpan();
            MemoryMarshal.Write(span.Slice(0, sizeof(uint)), ref magic);
            MemoryMarshal.Write(span.Slice(sizeof(uint), sizeof(uint)), ref keyLength);
            span.Slice(sizeof(uint) + sizeof(uint), keyLength).Fill(0);
            span.Slice(sizeof(uint) + sizeof(uint) + keyLength, keyLength).Fill(0);
            s.CopyTo(span.Slice(sizeof(uint) + sizeof(uint) + keyLength + keyLength, keyLength));

            // import blob and wrap with CNG
            var key = CngKey.Create(algorithm, null, new CngKeyCreationParameters() { Parameters = { new CngProperty(CngKeyBlobFormat.EccPrivateBlob.Format, blob, CngPropertyOptions.None) } });
            var ech = new ECDiffieHellmanCng(key);

            return ech;
#endif
        }

        /// <summary>
        /// Creates a new <see cref="ECDiffieHellmanPublicKey"/> for the given curve and public key material.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        static ECDiffieHellmanPublicKey ImportECDiffieHellmanPublicKey(ECCurve curve, ECPoint q)
        {
#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            return ECDiffieHellman.Create(new ECParameters() { Curve = curve, Q = q }).PublicKey;
#else
            // properties of the key
            var keyLength = q.X.Length;
            var magic = ECUtil.EcdsaCurveNameToPublicMagic(curve.FriendlyName, true);

            // CNG blob: magic + key size + X + Y
            var blob = new byte[sizeof(uint) + sizeof(uint) + keyLength + keyLength];
            var span = blob.AsSpan();
            MemoryMarshal.Write(span.Slice(0, sizeof(uint)), ref magic);
            MemoryMarshal.Write(span.Slice(sizeof(uint), sizeof(uint)), ref keyLength);
            q.X.CopyTo(span.Slice(sizeof(uint) + sizeof(uint), keyLength));
            q.Y.CopyTo(span.Slice(sizeof(uint) + sizeof(uint) + keyLength, keyLength));

            return ECDiffieHellmanCngPublicKey.FromByteArray(blob, CngKeyBlobFormat.EccPublicBlob);
#endif
        }

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
                using var prv = ImportECDiffieHellmanPrivateKey(curve, s);
                using var pub = ImportECDiffieHellmanPublicKey(curve, ECUtil.ImportECPoint(w));

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
        }

    }

}
