/*
  Copyright (C) 2012 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace IKVM.Reflection.Reader
{

    // This code is based on trial-and-error and some inspiration from the Mono.Security library.
    // It almost certainly has bugs and/or design flaws.
    static class Authenticode
    {

        const ushort IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b;
        const ushort IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b;
        const int WIN_CERT_REVISION_2_0 = 0x0200;
        const int WIN_CERT_TYPE_PKCS_SIGNED_DATA = 0x0002;

        internal static X509Certificate GetSignerCertificate(Stream stream)
        {
            stream.Seek(60, SeekOrigin.Begin);
            var br = new BinaryReader(stream);
            var peSignatureOffset = br.ReadInt32();
            var checksumOffset = peSignatureOffset + 24 + 64;

            // seek to the IMAGE_OPTIONAL_HEADER
            stream.Seek(peSignatureOffset + 24, SeekOrigin.Begin);
            var certificateTableDataDirectoryOffset = br.ReadUInt16() switch
            {
                IMAGE_NT_OPTIONAL_HDR32_MAGIC => peSignatureOffset + 24 + (64 + 4 * 8) + 8 * 4,
                IMAGE_NT_OPTIONAL_HDR64_MAGIC => peSignatureOffset + 24 + (64 + 4 * 8 + 16) + 8 * 4,
                _ => throw new BadImageFormatException(),
            };

            stream.Seek(certificateTableDataDirectoryOffset, SeekOrigin.Begin);
            var certificateTableOffset = br.ReadInt32();
            var certificateTableLength = br.ReadInt32();

            stream.Seek(certificateTableOffset, SeekOrigin.Begin);
            var dwLength = br.ReadInt32();
            var wRevision = br.ReadInt16();
            var wCertificateType = br.ReadInt16();
            if (wRevision != WIN_CERT_REVISION_2_0)
                return null;
            if (wCertificateType != WIN_CERT_TYPE_PKCS_SIGNED_DATA)
                return null;

            var buf = br.ReadBytes(certificateTableLength - 8);
            var cms = new SignedCms();
            try
            {
                cms.Decode(buf);
                cms.CheckSignature(false);
            }
            catch (CryptographicException)
            {
                return null;
            }

            var signerInfo = cms.SignerInfos[0];

            var offsets = new int[] { checksumOffset, certificateTableDataDirectoryOffset, certificateTableOffset };
            var lengths = new int[] { 4, 8, certificateTableLength };
            var actualHash = ComputeHashWithSkip(stream, signerInfo.DigestAlgorithm.FriendlyName, offsets, lengths);
            var requiredHash = DecodeASN1(cms.ContentInfo.Content, 0, 1, 1);

            if (requiredHash == null || actualHash.Length != requiredHash.Length)
                return null;

            for (int i = 0; i < actualHash.Length; i++)
                if (actualHash[i] != requiredHash[i])
                    return null;

            return signerInfo.Certificate;
        }

        static byte[] ComputeHashWithSkip(Stream stream, string hashAlgorithm, int[] skipOffsets, int[] skipLengths)
        {
            stream.Position = 0;
            for (int i = skipOffsets.Length - 1; i >= 0; i--)
            {
                stream = new IKVM.Reflection.Writer.SkipStream(stream, skipOffsets[i], skipLengths[i]);
            }
            using (HashAlgorithm hash = HashAlgorithm.Create(hashAlgorithm))
            {
                return hash.ComputeHash(stream);
            }
        }

        static byte[] DecodeASN1(byte[] buf, params int[] indexes)
        {
            return DecodeASN1(buf, 0, buf.Length, 0, indexes);
        }

        static byte[] DecodeASN1(byte[] buf, int pos, int end, int depth, int[] indexes)
        {
            for (var index = 0; pos < end; index++)
            {
                var tag = buf[pos++];
                var length = (int)buf[pos++];
                if (length > 128)
                {
                    var lenlen = length & 0x7F;
                    length = 0;
                    for (var i = 0; i < lenlen; i++)
                        length = length * 256 + buf[pos++];
                }
                if (indexes[depth] == index)
                {
                    if (depth == indexes.Length - 1)
                    {
                        var data = new byte[length];
                        Buffer.BlockCopy(buf, pos, data, 0, length);
                        return data;
                    }

                    if ((tag & 0x20) == 0)
                        return null;

                    return DecodeASN1(buf, pos, pos + length, depth + 1, indexes);
                }
                pos += length;
            }

            return null;
        }

    }

}
