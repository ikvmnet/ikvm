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
