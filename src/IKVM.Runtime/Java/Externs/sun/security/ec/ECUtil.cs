using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

#if FIRST_PASS == false

using java.lang;
using java.security;

#endif

namespace IKVM.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Utilities for working with eliptical curve cryptography.
    /// </summary>
    internal static class ECUtil
    {

        public const byte SEC_ASN1_OBJECT_ID = 0x06;
        public const int EC_POINT_FORM_UNCOMPRESSED = 0x04;
        public const int ANSI_X962_CURVE_OID_TOTAL_LEN = 10;
        public const int SECG_CURVE_OID_TOTAL_LEN = 7;

        public const uint BCRYPT_ECDSA_PUBLIC_P256_MAGIC = 0x31534345;
        public const uint BCRYPT_ECDSA_PRIVATE_P256_MAGIC = 0x32534345;
        public const uint BCRYPT_ECDSA_PUBLIC_P384_MAGIC = 0x33534345;
        public const uint BCRYPT_ECDSA_PRIVATE_P384_MAGIC = 0x34534345;
        public const uint BCRYPT_ECDSA_PUBLIC_P521_MAGIC = 0x35534345;
        public const uint BCRYPT_ECDSA_PRIVATE_P521_MAGIC = 0x36534345;

        public const uint BCRYPT_ECDH_PUBLIC_P256_MAGIC = 0x314B4345;
        public const uint BCRYPT_ECDH_PRIVATE_P256_MAGIC = 0x324B4345;
        public const uint BCRYPT_ECDH_PUBLIC_P384_MAGIC = 0x334B4345;
        public const uint BCRYPT_ECDH_PRIVATE_P384_MAGIC = 0x344B4345;
        public const uint BCRYPT_ECDH_PUBLIC_P521_MAGIC = 0x354B4345;
        public const uint BCRYPT_ECDH_PRIVATE_P521_MAGIC = 0x364B4345;

#if FIRST_PASS == false


        /// <summary>
        /// Creates a <see cref="ECDiffieHellman"/> instance for the given curve and private key material.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static ECDsa ImportECDsaPrivateKey(ECCurve curve, byte[] s)
        {
            // lookup key size as we may need to prefix s
            var keySize = GetCurveKeySize(curve);
            if (keySize == 0)
                throw new InvalidAlgorithmParameterException();

            // incoming key needs to be proper length
            var keyLength = (keySize + 7) >> 3;
            s = NormalizeLength(s, keyLength);

#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER

            // NET Core 3.1 does not support an empty Q
            // However, on Windows, a fake Q the length of D works
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var X = new byte[s.Length];
                var Y = X;
                return ECDsa.Create(new ECParameters() { Curve = curve, D = s, Q = new ECPoint() { X = X, Y = Y } });
            }
            else
            {
                // fail on linux and mac os for now
                throw new PlatformNotSupportedException();
            }
#else
            var algorithm = GetCurveCngAlgorithm(curve, ECMode.ECDsa);
            if (algorithm == null)
                throw new InvalidAlgorithmParameterException();

            var magic = GetCurveBCryptPrivateMagic(curve, ECMode.ECDsa);

            // CNG blob: magic + key size + X + Y + D
            var blob = new byte[sizeof(uint) + sizeof(uint) + keyLength + keyLength + keyLength];
            var span = blob.AsSpan();
            MemoryMarshal.Write(span.Slice(0, sizeof(uint)), ref magic);
            MemoryMarshal.Write(span.Slice(sizeof(uint), sizeof(uint)), ref keyLength);
            span.Slice(sizeof(uint) + sizeof(uint), keyLength).Fill(0);
            span.Slice(sizeof(uint) + sizeof(uint) + keyLength, keyLength).Fill(0);
            s.CopyTo(span.Slice(sizeof(uint) + sizeof(uint) + keyLength + keyLength, keyLength));

            // import blob and wrap with CNG
            var key = CngKey.Import(blob, CngKeyBlobFormat.EccPrivateBlob);
            var ech = new ECDsaCng(key);

            return ech;
#endif
        }

        /// <summary>
        /// Creates a <see cref="ECDiffieHellman"/> instance for the given curve and private key material.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static ECDsa ImportECDsaPublicKey(ECCurve curve, byte[] w)
        {
            // lookup key size
            var keySize = GetCurveKeySize(curve);
            if (keySize == 0)
                throw new InvalidAlgorithmParameterException();

#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            return ECDsa.Create(new ECParameters { Curve = curve, Q = ImportECPoint(w) });
#else
            // properties of the key
            var keyLength = (keySize + 7) >> 3;
            var magic = GetCurveBCryptPublicMagic(curve, ECMode.ECDsa);
            var q = ImportECPoint(w);

            // CNG blob: magic + key size + X + Y
            var blob = new byte[sizeof(uint) + sizeof(uint) + keyLength + keyLength];
            var span = blob.AsSpan();
            MemoryMarshal.Write(span.Slice(0, sizeof(uint)), ref magic);
            MemoryMarshal.Write(span.Slice(sizeof(uint), sizeof(uint)), ref keyLength);
            q.X.CopyTo(span.Slice(sizeof(uint) + sizeof(uint), keyLength));
            q.Y.CopyTo(span.Slice(sizeof(uint) + sizeof(uint) + keyLength, keyLength));

            // import blob and wrap with CNG
            using var key = CngKey.Import(blob, CngKeyBlobFormat.EccPublicBlob);
            var ech = new ECDsaCng(key);
            return ech;
#endif
        }

        /// <summary>
        /// Creates a <see cref="ECDiffieHellman"/> instance for the given curve and private key material.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static ECDiffieHellman ImportECDiffieHellmanPrivateKey(ECCurve curve, byte[] s)
        {
            // lookup key size as we may need to prefix s
            var keySize = GetCurveKeySize(curve);
            if (keySize == 0)
                throw new InvalidAlgorithmParameterException();

            // incoming key needs to be proper length
            var keyLength = (keySize + 7) >> 3;
            s = NormalizeLength(s, keyLength);

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
            var magic = GetCurveBCryptPrivateMagic(curve, ECMode.ECDH);

            // CNG blob: magic + key size + X + Y + D
            var blob = new byte[sizeof(uint) + sizeof(uint) + keyLength + keyLength + keyLength];
            var span = blob.AsSpan();
            MemoryMarshal.Write(span.Slice(0, sizeof(uint)), ref magic);
            MemoryMarshal.Write(span.Slice(sizeof(uint), sizeof(uint)), ref keyLength);
            span.Slice(sizeof(uint) + sizeof(uint), keyLength).Fill(0);
            span.Slice(sizeof(uint) + sizeof(uint) + keyLength, keyLength).Fill(0);
            s.CopyTo(span.Slice(sizeof(uint) + sizeof(uint) + keyLength + keyLength, keyLength));

            // import blob and wrap with CNG
            var key = CngKey.Import(blob, CngKeyBlobFormat.EccPrivateBlob);
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
        public static ECDiffieHellmanPublicKey ImportECDiffieHellmanPublicKey(ECCurve curve, ECPoint q)
        {
#if NET47_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            return ECDiffieHellman.Create(new ECParameters() { Curve = curve, Q = q }).PublicKey;
#else
            // properties of the key
            var keyLength = q.X.Length;
            var magic = GetCurveBCryptPublicMagic(curve, ECMode.ECDH);

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
        /// Exports an <see cref="ECPoint"/> in uncompressed form.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static byte[] ExportECPoint(ECPoint p)
        {
            // public key result needs to be prefixed with point form
            var b = new byte[1 + p.X.Length + p.Y.Length];
            b[0] = EC_POINT_FORM_UNCOMPRESSED;
            p.X.CopyTo(((Span<byte>)b).Slice(1, p.X.Length));
            p.Y.CopyTo(((Span<byte>)b).Slice(1 + p.X.Length, p.Y.Length));
            return b;
        }

        /// <summary>
        /// Imports an <see cref="ECPoint"/>.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAlgorithmParameterException"></exception>
        public static ECPoint ImportECPoint(ReadOnlySpan<byte> w)
        {
            // format of point
            var format = (int)w[0];
            if (format != EC_POINT_FORM_UNCOMPRESSED)
                throw new InvalidAlgorithmParameterException();

            // skip format, calculate x and y length    
            var W = w.Slice(1);
            var l = W.Length / 2;

            // read X and Y
            var x = W.Slice(0, l).ToArray();
            var y = W.Slice(l, l).ToArray();
            return new ECPoint() { X = x, Y = y };
        }

        /// <summary>
        /// Decodes the specified encoded parameters.
        /// </summary>
        /// <param name="encodedParams"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAlgorithmParameterException"></exception>
        public static ECCurve DecodeParameters(byte[] encodedParams)
        {
            if (encodedParams == null)
                throw new InvalidAlgorithmParameterException();
            if (encodedParams.Length < 2)
                throw new InvalidAlgorithmParameterException();

            if (encodedParams.Length != ANSI_X962_CURVE_OID_TOTAL_LEN &&
                encodedParams.Length != SECG_CURVE_OID_TOTAL_LEN)
                throw new InvalidAlgorithmParameterException();

            if (encodedParams[0] != SEC_ASN1_OBJECT_ID)
                throw new InvalidAlgorithmParameterException();

            var oid = ParseOid(((ReadOnlySpan<byte>)encodedParams).Slice(2));
            if (oid == null || oid.FriendlyName == null)
                throw new InvalidAlgorithmParameterException();

#if NET47_OR_GREATER
            return ECCurve.CreateFromValue(oid.Value);
#else
            return ECCurve.CreateFromOid(oid);
#endif
        }

        /// <summary>
        /// Parses the OID value in ASN1 format. This method can be replaced on later versions of .NET.
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAlgorithmParameterException"></exception>
        public static Oid ParseOid(ReadOnlySpan<byte> raw)
        {
            // parse the byte array into a set of individual items
            var u = new List<uint>(8) { (uint)(raw[0] / 40), (uint)(raw[0] % 40) };
            var b = (uint)0;
            for (var i = 1; i < raw.Length; i++)
            {
                if ((raw[i] & 0x80) == 0)
                {
                    u.Add(raw[i] + (b << 7));
                    b = 0;
                }
                else
                {
                    b <<= 7;
                    b += (uint)(raw[i] & 0x7F);
                }
            }

            // convert to string format
            var r = new System.Text.StringBuilder(u[0].ToString(CultureInfo.InvariantCulture));
            for (var k = 1; k < u.Count; k++)
                r.Append(".").Append(u[k].ToString(CultureInfo.InvariantCulture));

            // parse string format with built-in OID class, which has no raw constructor
            return new Oid(r.ToString());
        }

#endif

        /// <summary>
        /// CNG has different values for ECDSA and ECDH modes.
        /// </summary>
        public enum ECMode
        {

            ECDsa,
            ECDH,

        }

        /// <summary>
        /// Returns a <see cref="CngAlgorithm"/> value for the specified curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static CngAlgorithm GetCurveCngAlgorithm(ECCurve curve, ECMode mode) => curve.Oid.FriendlyName.ToLowerInvariant() switch
        {
            "nistp256" or "ecdsa_p256" => mode == ECMode.ECDH ? CngAlgorithm.ECDiffieHellmanP256 : CngAlgorithm.ECDsaP256,
            "nistp384" or "ecdsa_p384" => mode == ECMode.ECDH ? CngAlgorithm.ECDiffieHellmanP384 : CngAlgorithm.ECDsaP384,
            "nistp521" or "ecdsa_p521" => mode == ECMode.ECDH ? CngAlgorithm.ECDiffieHellmanP521 : CngAlgorithm.ECDsaP521,
            _ => null,
        };

        /// <summary>
        /// Returns the BCRYPT magic value for a private key of the specified curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static uint GetCurveBCryptPrivateMagic(ECCurve curve, ECMode mode) => curve.Oid.FriendlyName.ToLowerInvariant() switch
        {
            "nistp256" or "ecdsa_p256" => mode == ECMode.ECDH ? BCRYPT_ECDH_PRIVATE_P256_MAGIC : BCRYPT_ECDSA_PRIVATE_P256_MAGIC,
            "nistp384" or "ecdsa_p384" => mode == ECMode.ECDH ? BCRYPT_ECDH_PRIVATE_P384_MAGIC : BCRYPT_ECDSA_PRIVATE_P384_MAGIC,
            "nistp521" or "ecdsa_p521" => mode == ECMode.ECDH ? BCRYPT_ECDH_PRIVATE_P521_MAGIC : BCRYPT_ECDSA_PRIVATE_P521_MAGIC,
            _ => 0,
        };

        /// <summary>
        /// Returns the BCRYPT magic value for a public key of the specified curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static uint GetCurveBCryptPublicMagic(ECCurve curve, ECMode mode) => curve.Oid.FriendlyName.ToLowerInvariant() switch
        {
            "nistp256" or "ecdsa_p256" => mode == ECMode.ECDH ? BCRYPT_ECDH_PUBLIC_P256_MAGIC : BCRYPT_ECDSA_PUBLIC_P256_MAGIC,
            "nistp384" or "ecdsa_p384" => mode == ECMode.ECDH ? BCRYPT_ECDH_PUBLIC_P384_MAGIC : BCRYPT_ECDSA_PUBLIC_P384_MAGIC,
            "nistp521" or "ecdsa_p521" => mode == ECMode.ECDH ? BCRYPT_ECDH_PUBLIC_P521_MAGIC : BCRYPT_ECDSA_PUBLIC_P521_MAGIC,
            _ => 0,
        };

        /// <summary>
        /// Returns a <see cref="CngAlgorithm"/> value for the specified curve name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int GetCurveKeySize(ECCurve curve) => curve.Oid.FriendlyName.ToLowerInvariant() switch
        {
            "nistp256" or "ecdsa_p256" => 256,
            "nistp384" or "ecdsa_p384" => 384,
            "nistp521" or "ecdsa_p521" => 521,
            _ => 0,
        };

        /// <summary>
        /// Trims or expands an array to match the specified length.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static byte[] NormalizeLength(byte[] s, int length)
        {
            return NormalizeLength(s.AsSpan(), length).ToArray();
        }

        /// <summary>
        /// Trims or expands an array to match the specified length.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static ReadOnlySpan<byte> NormalizeLength(ReadOnlySpan<byte> s, int length)
        {
            if (s.Length > length)
            {
                // trim start
                s = s.Slice(s.Length - length);
            }
            else if (s.Length < length)
            {
                // rebuild longer
                var t = new byte[length];
                s.CopyTo(t.AsSpan().Slice(length - s.Length));
                s = t;
            }

            return s;
        }

    }

}
