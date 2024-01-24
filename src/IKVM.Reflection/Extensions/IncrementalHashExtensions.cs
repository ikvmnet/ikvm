using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace IKVM.Reflection.Extensions
{
    internal static class IncrementalHashExtensions
    {

        internal static void AppendData(this IncrementalHash hash, IEnumerable<Blob> blobs)
        {
            foreach (var blob in blobs)
                hash.AppendData(blob.GetBytes());
        }

        internal static void AppendData(this IncrementalHash hash, IEnumerable<ArraySegment<byte>> blobs)
        {
            foreach (var blob in blobs)
                hash.AppendData(blob);
        }

        internal static void AppendData(this IncrementalHash hash, ArraySegment<byte> segment)
        {
            hash.AppendData(segment.Array, segment.Offset, segment.Count);
        }

    }

}
