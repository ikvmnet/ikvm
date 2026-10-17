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
    /// Computes cryptographic hashes over metadata and image content, for deterministic content IDs and strong name
    /// signatures.
    /// </summary>
    class CryptographicHashProvider
    {

        /// <summary>
        /// Computes the hash of a sequence of blobs, as if they were one contiguous block of data.
        /// </summary>
        /// <param name="algorithmName">The hash algorithm to use.</param>
        /// <param name="bytes">The blobs to hash, in order.</param>
        /// <returns>The computed hash.</returns>
        public static ImmutableArray<byte> ComputeHash(HashAlgorithmName algorithmName, IEnumerable<Blob> bytes)
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
