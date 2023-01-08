using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using java.lang;
using java.security;

namespace IKVM.Java.Externs.Impl.sun.security.ec
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
#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

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
            var curve = ECUtil.DecodeParameters(encodedParams);
            if (curve.IsNamed == false)
                throw new InvalidAlgorithmParameterException();

            try
            {
                var algorithm = ECUtil.EcdsaCurveNameToAlgorithm(curve.FriendlyName);
                if (algorithm == null)
                    throw new InvalidAlgorithmParameterException();

                // byte size of keys
                var keyLength = keySize / 8;

                // generate a new CNG key
                var key = CngKey.Create(algorithm, null, new CngKeyCreationParameters() { ExportPolicy = CngExportPolicies.AllowPlaintextExport, Parameters = { new CngProperty("Length", BitConverter.GetBytes(keySize), CngPropertyOptions.None) } });
                var src = key.Export(CngKeyBlobFormat.EccPrivateBlob).AsSpan();

                // read the key size and validate
                
                var magic = MemoryMarshal.Read<uint>(src.Slice(0, sizeof(uint)));
                if (magic != ECUtil.EcdsaCurveNameToPrivateMagic(curve.FriendlyName))
                    throw new KeyException();

                // read the key size and validate
                var kzz = MemoryMarshal.Read<uint>(src.Slice(sizeof(uint), sizeof(uint)));
                if (kzz != keyLength)
                    throw new KeyException();

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
