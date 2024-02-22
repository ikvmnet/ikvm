using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace IKVM.Reflection.Extensions
{

    internal static class IncrementalHashExtensions
    {

        /// <summary>
        /// Appends the blobs to the hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="blobs"></param>
        public static void AppendData(this IncrementalHash hash, IEnumerable<Blob> blobs)
        {
            foreach (var blob in blobs)
                AppendData(hash, blob.GetBytes());
        }

        /// <summary>
        /// Appens the array segments to the hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="blobs"></param>
        public static void AppendData(this IncrementalHash hash, IEnumerable<ArraySegment<byte>> blobs)
        {
            foreach (var blob in blobs)
                AppendData(hash, blob);
        }

        /// <summary>
        /// Appends the array segment to the hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="segment"></param>
        public static void AppendData(this IncrementalHash hash, ArraySegment<byte> segment)
        {
            hash.AppendData(segment.Array, segment.Offset, segment.Count);
        }

    }

}
