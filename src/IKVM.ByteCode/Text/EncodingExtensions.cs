using System;
using System.Text;

namespace IKVM.ByteCode.Text
{

    static class EncodingExtensions
    {

#if NETFRAMEWORK

        public static unsafe string GetString(this Encoding self, ReadOnlySpan<byte> bytes)
        {
            fixed (byte* bytesPtr = bytes)
                return self.GetString(bytesPtr, bytes.Length);
        }

#endif

    }

}
