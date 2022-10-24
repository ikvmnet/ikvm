using System;
using System.Buffers.Binary;

namespace IKVM.ByteCode.Parser
{

    static class ClassFilePrimitives
    {

        /// <summary>
        /// Reads a 'u4', which is an unsigned integer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static uint ReadU4(ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var v = BinaryPrimitives.ReadUInt32BigEndian(buffer);
            buffer = buffer.Slice(sizeof(uint));
            length += sizeof(uint);
            return v;
        }

        /// <summary>
        /// Reads a 'u2', which is an unsigned short.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ushort ReadU2(ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var v = BinaryPrimitives.ReadUInt16BigEndian(buffer);
            buffer = buffer.Slice(sizeof(ushort));
            length += sizeof(ushort);
            return v;
        }

    }

}
