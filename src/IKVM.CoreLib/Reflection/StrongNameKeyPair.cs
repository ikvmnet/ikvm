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

    /// <summary>
    /// Encapsulates access to a public or private key pair used to sign strong name assemblies.
    /// </summary>
    sealed class StrongNameKeyPair
    {

        readonly byte[] _keyPairArray;
        readonly string _keyPairContainer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="keyPairContainer">The name of the key container that holds the key pair.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyPairContainer"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Key containers are not supported when running on Mono on Windows.</exception>
        public StrongNameKeyPair(string keyPairContainer)
        {
            if (Universe.MonoRuntime && Environment.OSVersion.Platform == PlatformID.Win32NT)
                throw new NotSupportedException("IKVM.Reflection does not support key containers when running on Mono");

            _keyPairContainer = keyPairContainer ?? throw new ArgumentNullException("keyPairContainer");
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="keyPairArray">An array of bytes containing the key pair.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyPairArray"/> is <see langword="null"/>.</exception>
        public StrongNameKeyPair(byte[] keyPairArray)
        {
            if (keyPairArray == null)
                throw new ArgumentNullException("keyPairArray");

            _keyPairArray = (byte[])keyPairArray.Clone();
        }

        /// <summary>
        /// Gets the public part of the public key or public key token of the key pair.
        /// </summary>
        public byte[] PublicKey
        {
            get
            {
                // MONOBUG workaround for https://bugzilla.xamarin.com/show_bug.cgi?id=5299
                if (Universe.MonoRuntime)
                    return MonoGetPublicKey();

                using var rsa = CreateRSA();
                var rsaParameters = rsa.ExportParameters(false);
                var cspBlob = ExportPublicKey(rsaParameters);
                var publicKey = new byte[12 + cspBlob.Length];
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

        /// <summary>
        /// Creates an <see cref="RSA"/> instance holding the key pair, for signing the assembly.
        /// </summary>
        /// <returns>A new <see cref="RSA"/> instance that the caller must dispose.</returns>
        /// <exception cref="ArgumentException">The key pair cannot be loaded from the key blob or key container.</exception>
        internal RSA CreateRSA()
        {
            try
            {
                if (_keyPairArray != null)
                {
                    var rsa = RSA.Create();
                    // we import from parameters, as using ImportCspBlob
                    // causes the exception "KeySet not found" when signing a hash later.
                    rsa.ImportParameters(RSAParametersFromByteArray(_keyPairArray));
                    return rsa;
                }
                else
                {
#if NETFRAMEWORK
                    var parm = new CspParameters
                    {
                        KeyContainerName = _keyPairContainer
                    };

                    // MONOBUG Mono doesn't like it when Flags or KeyNumber are set
                    if (Universe.MonoRuntime == false)
                    {
                        parm.Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey;
                        // AT_SIGNATURE
                        parm.KeyNumber = 2;
                    }

                    return new RSACryptoServiceProvider(parm);
#else
                    throw new PlatformNotSupportedException("Strong name key container support is only available when targeting .NET Framework.");
#endif
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

        /// <summary>
        /// Encodes the public half of <paramref name="rsaParameters"/> as a CryptoAPI PUBLICKEYBLOB.
        /// </summary>
        /// <param name="rsaParameters">The RSA key parameters.</param>
        /// <returns>The PUBLICKEYBLOB bytes.</returns>
        /// <exception cref="ArgumentNullException">The modulus or the exponent of <paramref name="rsaParameters"/> is <see langword="null"/>.</exception>
        static byte[] ExportPublicKey(RSAParameters rsaParameters)
        {
            if (rsaParameters.Modulus == null || rsaParameters.Exponent == null)
                throw new ArgumentNullException(nameof(rsaParameters));

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            var keyBitLength = rsaParameters.Modulus.Length * 8;
            bw.Write((byte)0x06);
            bw.Write((byte)0x02);
            bw.Write((ushort)0x0000);
            bw.Write((uint)0x2400);
            bw.Write("RSA1".ToCharArray());
            bw.Write((uint)keyBitLength);
            bw.Write(rsaParameters.Exponent);
            bw.Write((byte)0x00);
            var modulus = (byte[])rsaParameters.Modulus.Clone();
            Array.Reverse(modulus);
            bw.Write(modulus);

            return ms.ToArray();
        }

        /// <summary>
        /// Reads RSA key parameters from a CryptoAPI key blob, as found in a .snk file.
        /// </summary>
        /// <param name="array">The key blob.</param>
        /// <returns>The RSA key parameters.</returns>
        static RSAParameters RSAParametersFromByteArray(byte[] array)
        {
            using var ms = new MemoryStream(array);
            return RSAParametersFromStream(ms);
        }

        /// <summary>
        /// Reads RSA key parameters from a stream containing a CryptoAPI PUBLICKEYBLOB or PRIVATEKEYBLOB.
        /// </summary>
        /// <param name="str">The stream to read the key blob from.</param>
        /// <returns>The RSA key parameters; the private key parameters are only set for a PRIVATEKEYBLOB.</returns>
        /// <exception cref="CryptographicException">The stream does not contain an RSA key blob.</exception>
        static RSAParameters RSAParametersFromStream(Stream str)
        {
            var rsaParameters = new RSAParameters();
            using var br = new BinaryReader(str);

            // read BLOBHEADER
            var keyType = br.ReadByte();
            if (keyType != 6 && keyType != 7)
                throw new CryptographicException("SNK file not in correct format");

            var blobVersion = br.ReadByte();
            var reserverd = br.ReadUInt16();
            var algorithmID = br.ReadUInt32();

            // read RSAPUBKEY
            var magic = new string(br.ReadChars(4));
            if (!magic.Equals("RSA1") && !magic.Equals("RSA2"))
                throw new CryptographicException("SNK file not in correct format");

            var keyBitLength = br.ReadUInt32();
            var publicExponent = br.ReadBytes(3);
            br.ReadByte();
            rsaParameters.Exponent = publicExponent;

            // read the modulus
            var modulus = br.ReadBytes((int)keyBitLength / 8);
            Array.Reverse(modulus);
            rsaParameters.Modulus = modulus;

            if (keyType == 7)
            {
                // read the private key parameters
                var prime1 = br.ReadBytes((int)keyBitLength / 16);
                var prime2 = br.ReadBytes((int)keyBitLength / 16);
                var exponent1 = br.ReadBytes((int)keyBitLength / 16);
                var exponent2 = br.ReadBytes((int)keyBitLength / 16);
                var coefficient = br.ReadBytes((int)keyBitLength / 16);
                var privateExponent = br.ReadBytes((int)keyBitLength / 8);

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

            return rsaParameters;
        }

        /// <summary>
        /// Gets the public key through <see cref="System.Reflection.StrongNameKeyPair"/>, as a workaround for a Mono bug.
        /// </summary>
        /// <returns>The public key.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        byte[] MonoGetPublicKey() =>
            _keyPairArray != null ? new System.Reflection.StrongNameKeyPair(_keyPairArray).PublicKey : new System.Reflection.StrongNameKeyPair(_keyPairContainer).PublicKey;

    }

}
