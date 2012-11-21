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
#if !NO_AUTHENTICODE
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
		private const ushort IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b;
		private const ushort IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b;
		private const int WIN_CERT_REVISION_2_0 = 0x0200;
		private const int WIN_CERT_TYPE_PKCS_SIGNED_DATA = 0x0002;

		internal static X509Certificate GetSignerCertificate(Stream stream)
		{
			stream.Seek(60, SeekOrigin.Begin);
			BinaryReader br = new BinaryReader(stream);
			int peSignatureOffset = br.ReadInt32();
			int checksumOffset = peSignatureOffset + 24 + 64;
			// seek to the IMAGE_OPTIONAL_HEADER
			stream.Seek(peSignatureOffset + 24, SeekOrigin.Begin);
			int certificateTableDataDirectoryOffset;
			switch (br.ReadUInt16())
			{
				case IMAGE_NT_OPTIONAL_HDR32_MAGIC:
					certificateTableDataDirectoryOffset = peSignatureOffset + 24 + (64 + 4 * 8) + 8 * 4;
					break;
				case IMAGE_NT_OPTIONAL_HDR64_MAGIC:
					certificateTableDataDirectoryOffset = peSignatureOffset + 24 + (64 + 4 * 8 + 16) + 8 * 4;
					break;
				default:
					throw new BadImageFormatException();
			}
			stream.Seek(certificateTableDataDirectoryOffset, SeekOrigin.Begin);
			int certificateTableOffset = br.ReadInt32();
			int certificateTableLength = br.ReadInt32();

			stream.Seek(certificateTableOffset, SeekOrigin.Begin);
			int dwLength = br.ReadInt32();
			short wRevision = br.ReadInt16();
			short wCertificateType = br.ReadInt16();
			if (wRevision != WIN_CERT_REVISION_2_0)
			{
				return null;
			}
			if (wCertificateType != WIN_CERT_TYPE_PKCS_SIGNED_DATA)
			{
				return null;
			}
			byte[] buf = new byte[certificateTableLength - 8];
			stream.Read(buf, 0, buf.Length);

			SignedCms cms = new SignedCms();
			try
			{
				cms.Decode(buf);
				cms.CheckSignature(false);
			}
			catch (CryptographicException)
			{
				return null;
			}
			SignerInfo signerInfo = cms.SignerInfos[0];

			int[] offsets = new int[] { checksumOffset, certificateTableDataDirectoryOffset, certificateTableOffset };
			int[] lengths = new int[] { 4, 8, certificateTableLength };
			byte[] actualHash = ComputeHashWithSkip(stream, signerInfo.DigestAlgorithm.FriendlyName, offsets, lengths);
			byte[] requiredHash = DecodeASN1(cms.ContentInfo.Content, 0, 1, 1);

			if (requiredHash == null || actualHash.Length != requiredHash.Length)
			{
				return null;
			}

			for (int i = 0; i < actualHash.Length; i++)
			{
				if (actualHash[i] != requiredHash[i])
				{
					return null;
				}
			}

			return signerInfo.Certificate;
		}

		private static byte[] ComputeHashWithSkip(Stream stream, string hashAlgorithm, int[] skipOffsets, int[] skipLengths)
		{
			using (HashAlgorithm hash = HashAlgorithm.Create(hashAlgorithm))
			{
				using (CryptoStream cs = new CryptoStream(Stream.Null, hash, CryptoStreamMode.Write))
				{
					stream.Seek(0, SeekOrigin.Begin);
					byte[] buf = new byte[8192];
					HashChunk(stream, cs, buf, skipOffsets[0]);
					stream.Seek(skipLengths[0], SeekOrigin.Current);
					for (int i = 1; i < skipOffsets.Length; i++)
					{
						HashChunk(stream, cs, buf, skipOffsets[i] - (skipOffsets[i - 1] + skipLengths[i - 1]));
						stream.Seek(skipLengths[i], SeekOrigin.Current);
					}
					HashChunk(stream, cs, buf, (int)stream.Length - (skipOffsets[skipOffsets.Length - 1] + skipLengths[skipLengths.Length - 1]));
				}
				return hash.Hash;
			}
		}

		private static void HashChunk(Stream stream, CryptoStream cs, byte[] buf, int length)
		{
			while (length > 0)
			{
				int read = stream.Read(buf, 0, Math.Min(buf.Length, length));
				cs.Write(buf, 0, read);
				length -= read;
			}
		}

		private static byte[] DecodeASN1(byte[] buf, params int[] indexes)
		{
			return DecodeASN1(buf, 0, buf.Length, 0, indexes);
		}

		private static byte[] DecodeASN1(byte[] buf, int pos, int end, int depth, int[] indexes)
		{
			for (int index = 0; pos < end; index++)
			{
				int tag = buf[pos++];
				int length = buf[pos++];
				if (length > 128)
				{
					int lenlen = length & 0x7F;
					length = 0;
					for (int i = 0; i < lenlen; i++)
					{
						length = length * 256 + buf[pos++];
					}
				}
				if (indexes[depth] == index)
				{
					if (depth == indexes.Length - 1)
					{
						byte[] data = new byte[length];
						Buffer.BlockCopy(buf, pos, data, 0, length);
						return data;
					}
					if ((tag & 0x20) == 0)
					{
						return null;
					}
					return DecodeASN1(buf, pos, pos + length, depth + 1, indexes);
				}
				pos += length;
			}
			return null;
		}
	}
}
#endif // !NO_AUTHENTICODE
