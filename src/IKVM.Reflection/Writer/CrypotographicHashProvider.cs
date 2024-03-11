using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;

using IKVM.Reflection.Extensions;

#nullable enable

namespace IKVM.Reflection.Writer
{

    /// <summary>
    /// Provides cryptographic operations.
    /// </summary>
    internal class CryptographicHashProvider
    {

        internal const int Sha1HashSize = 20;

        public static HashAlgorithm? TryGetAlgorithm(AssemblyHashAlgorithm algorithmId)
        {
            switch (algorithmId)
            {
                case AssemblyHashAlgorithm.None:
                case AssemblyHashAlgorithm.SHA1:
                    return SHA1.Create();
                case AssemblyHashAlgorithm.SHA256:
                    return SHA256.Create();
                case AssemblyHashAlgorithm.SHA384:
                    return SHA384.Create();
                case AssemblyHashAlgorithm.SHA512:
                    return SHA512.Create();
                case AssemblyHashAlgorithm.MD5:
                    return MD5.Create();
                default:
                    return null;
            }
        }

        public static bool IsSupportedAlgorithm(AssemblyHashAlgorithm algorithmId)
        {
            switch (algorithmId)
            {
                case AssemblyHashAlgorithm.None:
                case AssemblyHashAlgorithm.SHA1:
                case AssemblyHashAlgorithm.SHA256:
                case AssemblyHashAlgorithm.SHA384:
                case AssemblyHashAlgorithm.SHA512:
                case AssemblyHashAlgorithm.MD5:
                    return true;
                default:
                    return false;
            }
        }

        public static ImmutableArray<byte> ComputeSha1(Stream stream)
        {
            if (stream != null)
            {
                stream.Seek(0, SeekOrigin.Begin);
                using (var hashProvider = SHA1.Create())
                {
                    return ImmutableArray.Create(hashProvider.ComputeHash(stream));
                }
            }

            return ImmutableArray<byte>.Empty;
        }

        public static ImmutableArray<byte> ComputeSha1(ImmutableArray<byte> bytes)
        {
            return ComputeSha1(bytes.ToArray());
        }

        public static ImmutableArray<byte> ComputeSha1(byte[] bytes)
        {
            using (var hashProvider = SHA1.Create())
            {
                return ImmutableArray.Create(hashProvider.ComputeHash(bytes));
            }
        }

        public static ImmutableArray<byte> ComputeHash(HashAlgorithmName algorithmName, IEnumerable<Blob> bytes)
        {
            using (var incrementalHash = IncrementalHash.CreateHash(algorithmName))
            {
                incrementalHash.AppendData(bytes);
                return ImmutableArray.Create(incrementalHash.GetHashAndReset());
            }
        }

        public static ImmutableArray<byte> ComputeHash(HashAlgorithmName algorithmName, IEnumerable<ArraySegment<byte>> bytes)
        {
            using (var incrementalHash = IncrementalHash.CreateHash(algorithmName))
            {
                incrementalHash.AppendData(bytes);
                return ImmutableArray.Create(incrementalHash.GetHashAndReset());
            }
        }

    }

}

#nullable restore
