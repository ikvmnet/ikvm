/*
  Copyright (C) 2009-2012 Jeroen Frijters

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

namespace IKVM.Reflection
{
	public sealed class StrongNameKeyPair
	{
		private readonly byte[] keyPairArray;
		private readonly string keyPairContainer;

		public StrongNameKeyPair(string keyPairContainer)
		{
			if (keyPairContainer == null)
			{
				throw new ArgumentNullException("keyPairContainer");
			}
#if !NETSTANDARD
			if (Universe.MonoRuntime && Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				throw new NotSupportedException("IKVM.Reflection does not support key containers when running on Mono");
			}
#endif
			this.keyPairContainer = keyPairContainer;
		}

		public StrongNameKeyPair(byte[] keyPairArray)
		{
			if (keyPairArray == null)
			{
				throw new ArgumentNullException("keyPairArray");
			}
			this.keyPairArray = (byte[])keyPairArray.Clone();
		}

		public StrongNameKeyPair(FileStream keyPairFile)
			: this(ReadAllBytes(keyPairFile))
		{
		}

		private static byte[] ReadAllBytes(FileStream keyPairFile)
		{
			if (keyPairFile == null)
			{
				throw new ArgumentNullException("keyPairFile");
			}
			byte[] buf = new byte[keyPairFile.Length - keyPairFile.Position];
			keyPairFile.Read(buf, 0, buf.Length);
			return buf;
		}

		public byte[] PublicKey
		{
			get
			{
#if !NETSTANDARD
				if (Universe.MonoRuntime)
				{
					// MONOBUG workaround for https://bugzilla.xamarin.com/show_bug.cgi?id=5299
					return MonoGetPublicKey();
				}
#endif
				using (RSACryptoServiceProvider rsa = CreateRSA())
				{
					var rsaParameters = rsa.ExportParameters(false);
					byte[] cspBlob = ExportPublicKey(rsaParameters);
					byte[] publicKey = new byte[12 + cspBlob.Length];
					Buffer.BlockCopy(cspBlob, 0, publicKey, 12, cspBlob.Length);
					publicKey[1] = 36;
					publicKey[4] = 4;
					publicKey[5] = 128;
					publicKey[8] = (byte)(cspBlob.Length >> 0);
					publicKey[9] = (byte)(cspBlob.Length >> 8);
					publicKey[10] = (byte)(cspBlob.Length >> 16);
					publicKey[11] = (byte)(cspBlob.Length >> 24);
					return publicKey;
				}
			}
		}

		internal RSACryptoServiceProvider CreateRSA()
		{
			try
			{
				if (keyPairArray != null)
				{
					RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
					// we import from parameters, as using ImportCspBlob
					// causes the exception "KeySet not found" when signing a hash later.
					rsa.ImportParameters(RSAParametersFromByteArray(keyPairArray));
					return rsa;
				}
				else
				{
					CspParameters parm = new CspParameters();
					parm.KeyContainerName = keyPairContainer;
					// MONOBUG Mono doesn't like it when Flags or KeyNumber are set
					if (!Universe.MonoRuntime)
					{
						parm.Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey;
						parm.KeyNumber = 2;	// Signature
					}
					return new RSACryptoServiceProvider(parm);
				}
			}
			catch
			{
				throw new ArgumentException("Unable to obtain public key for StrongNameKeyPair.");
			}
		}

		// helper functions ExportPublicKey, RSAParametersFromStream and RSAParametersFromByteArray
		// based on code in the following article:-
		// https://www.developerfusion.com/article/84422/the-key-to-strong-names/
		static byte[] ExportPublicKey(RSAParameters rsaParameters)
		{
			if (rsaParameters.Modulus == null || rsaParameters.Exponent == null)
			{
				throw new ArgumentNullException(nameof(rsaParameters));
			}

			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					var keyBitLength = rsaParameters.Modulus.Length * 8;
					bw.Write((byte)0x06);
					bw.Write((byte)0x02);
					bw.Write((UInt16)0x0000);
					bw.Write((UInt32)0x2400);
					bw.Write("RSA1".ToCharArray());
					bw.Write((UInt32)keyBitLength);
					bw.Write(rsaParameters.Exponent);
					bw.Write((byte)0x00);
					byte[] modulus = (byte[])rsaParameters.Modulus.Clone();
					Array.Reverse(modulus);
					bw.Write(modulus);

					return ms.ToArray();
				}
			}
		}

		static RSAParameters RSAParametersFromByteArray(byte[] array)
		{
			using (MemoryStream ms = new MemoryStream(array))
			{
				return RSAParametersFromStream(ms);
			}
		}

		static RSAParameters RSAParametersFromStream(Stream str)
		{
			RSAParameters rsaParameters = new RSAParameters();

			using (var br = new BinaryReader(str))
			{
				// Read BLOBHEADER
				byte keyType = br.ReadByte();
				if (keyType != 6 && keyType != 7)
				{
					throw new CryptographicException("SNK file not in correct format");
				}
				byte blobVersion = br.ReadByte();
				UInt16 reserverd = br.ReadUInt16();
				UInt32 algorithmID = br.ReadUInt32();
				// Read RSAPUBKEY
				string magic = new string(br.ReadChars(4));
				if (!magic.Equals("RSA1") && !magic.Equals("RSA2"))
				{
					throw new CryptographicException("SNK file not in correct format");
				}
				UInt32 keyBitLength = br.ReadUInt32();
				byte[] publicExponent = br.ReadBytes(3);
				br.ReadByte();
				rsaParameters.Exponent = publicExponent;

				// Read Modulus
				byte[] modulus = br.ReadBytes(
					(int)keyBitLength / 8);
				// Read Private Key Paremeters
				Array.Reverse(modulus);
				rsaParameters.Modulus = modulus;

				if (keyType == 7)
				{
					byte[] prime1 = br.ReadBytes(
						(int)keyBitLength / 16);
					byte[] prime2 = br.ReadBytes(
						(int)keyBitLength / 16);
					byte[] exponent1 = br.ReadBytes(
						(int)keyBitLength / 16);
					byte[] exponent2 = br.ReadBytes(
						(int)keyBitLength / 16);
					byte[] coefficient = br.ReadBytes(
						(int)keyBitLength / 16);
					byte[] privateExponent = br.ReadBytes(
						(int)keyBitLength / 8);

					Array.Reverse(prime1);
					Array.Reverse(prime2);
					Array.Reverse(exponent1);
					Array.Reverse(exponent2);
					Array.Reverse(coefficient);
					Array.Reverse(privateExponent);
					rsaParameters.P = prime1;
					rsaParameters.Q = prime2;
					rsaParameters.DP = exponent1;
					rsaParameters.DQ = exponent2;
					rsaParameters.InverseQ = coefficient;
					rsaParameters.D = privateExponent;
				}
			}

			return rsaParameters;
		}

#if !NETSTANDARD
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		private byte[] MonoGetPublicKey()
		{
			return keyPairArray != null
				? new System.Reflection.StrongNameKeyPair(keyPairArray).PublicKey
				: new System.Reflection.StrongNameKeyPair(keyPairContainer).PublicKey;
		}
#endif
	}
}
