using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

#if FIRST_PASS == false

using java.lang;
using java.security;

#endif

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
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            try
            {
                // create a new ECDsa instance, set parameters, and generate key; then export results
                using var dsa = ECDsa.Create();
                dsa.KeySize = keySize;
                dsa.GenerateKey(curve);
                var p = dsa.ExportParameters(true);

                // result is a two item array, with the private key followed by exported ECPoint
                return new object[] { p.D, ECUtil.ExportECPoint(p.Q) };
            }
            catch (CryptographicException e)
            {
                throw new KeyException(e);
            }
            catch (PlatformNotSupportedException e)
            {
                throw new IllegalStateException(e);
            }
#else

            try
            {
                var algorithm = ECUtil.GetCurveCngAlgorithm(curve, ECUtil.ECMode.ECDsa);
                if (algorithm == null)
                    throw new InvalidAlgorithmParameterException();

                // generate a new CNG key
                var key = CngKey.Create(algorithm, null, new CngKeyCreationParameters() { ExportPolicy = CngExportPolicies.AllowPlaintextExport });
                var src = key.Export(CngKeyBlobFormat.EccPrivateBlob).AsSpan();

                // read the key magic and validate
                if (MemoryMarshal.Read<uint>(src.Slice(0, sizeof(uint))) != ECUtil.GetCurveBCryptPrivateMagic(curve, ECUtil.ECMode.ECDsa))
                    throw new KeyException();

                // read the key length, which is in bytes
                var keyLength = (int)MemoryMarshal.Read<uint>(src.Slice(sizeof(uint), sizeof(uint)));

                // ECPoint structure is uncompressed, followed by X and Y, which start at the beginning of the BLOB
                var q = new byte[1 + keyLength + keyLength];
                var w = q.AsSpan();
                w[0] = ECUtil.EC_POINT_FORM_UNCOMPRESSED;
                src.Slice(sizeof(uint) + sizeof(uint), keyLength).CopyTo(w.Slice(1, keyLength));
                src.Slice(sizeof(uint) + sizeof(uint) + keyLength, keyLength).CopyTo(w.Slice(1 + keyLength, keyLength));

                // private key starts after that
                var d = new byte[keyLength];
                src.Slice(sizeof(uint) + sizeof(uint) + keyLength + keyLength, keyLength).CopyTo(d);

                // result is a two item array
                return new object[] { d, q };
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
#endif
        }

    }

}
