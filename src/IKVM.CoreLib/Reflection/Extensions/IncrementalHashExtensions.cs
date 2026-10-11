using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace IKVM.Reflection.Extensions
{

    /// <summary>
    /// Provides extension methods for appending metadata blobs and array segments to an <see cref="IncrementalHash"/>.
    /// </summary>
    static class IncrementalHashExtensions
    {

        /// <summary>
        /// Appends the blobs to the hash.
        /// </summary>
        /// <param name="hash">The hash to append to.</param>
        /// <param name="blobs">The blobs whose contents are appended, in order.</param>
        public static void AppendData(this IncrementalHash hash, IEnumerable<Blob> blobs)
        {
            foreach (var blob in blobs)
                AppendData(hash, blob.GetBytes());
        }

        /// <summary>
        /// Appends the array segment to the hash.
        /// </summary>
        /// <param name="hash">The hash to append to.</param>
        /// <param name="segment">The segment of bytes to append.</param>
        public static void AppendData(this IncrementalHash hash, ArraySegment<byte> segment) =>
            hash.AppendData(segment.Array, segment.Offset, segment.Count);

    }

}
