using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

using java.security;

namespace IKVM.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Utilities for working with eliptical curve cryptography.
    /// </summary>
    static class ECUtil
    {

#if FIRST_PASS == false && NETCOREAPP

        const byte SEC_ASN1_OBJECT_ID = 0x06;
        const int EC_POINT_FORM_UNCOMPRESSED = 0x04;
        const int ANSI_X962_CURVE_OID_TOTAL_LEN = 10;
        const int SECG_CURVE_OID_TOTAL_LEN = 7;

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

            return ECCurve.CreateFromOid(oid);
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
            var r = new StringBuilder(u[0].ToString(CultureInfo.InvariantCulture));
            for (var k = 1; k < u.Count; k++)
                r.Append(".").Append(u[k].ToString(CultureInfo.InvariantCulture));

            // parse string format with built-in OID class, which has no raw constructor
            return new Oid(r.ToString());
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
            var y = W.Slice(l).ToArray();
            return new ECPoint() { X = x, Y = y };
        }

#endif

    }

}
