using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace IKVM.StubGen
{

    /// <summary>
    /// Provides methods to write basic building blocks to an incremental hash.
    /// </summary>
	/// <remarks>
	/// The Framework version of this code writes to a temporary heap array.
	/// </remarks>
    readonly struct SHA1HashWriter : IDisposable
    {

        readonly IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA1);
#if NETFRAMEWORK
        readonly byte[] temp = ArrayPool<byte>.Shared.Rent(1024);
#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SHA1HashWriter()
        {

        }

        public void WriteUInt16(ushort s)
        {
#if NETFRAMEWORK
            BinaryPrimitives.WriteUInt16BigEndian(temp, s);
            hash.AppendData(temp, 0, 2);
#else
			var buf = (Span<byte>)stackalloc byte[2];
            BinaryPrimitives.WriteUInt16BigEndian(buf, s);
            hash.AppendData(buf);
#endif
        }

        public void WriteUInt32(uint u)
        {
#if NETFRAMEWORK
            BinaryPrimitives.WriteUInt32BigEndian(temp, u);
            hash.AppendData(temp, 0, 4);
#else
			var span = (Span<byte>)stackalloc byte[4];
			BinaryPrimitives.WriteUInt32BigEndian(span, u);
			hash.AppendData(span);
#endif
        }

        public void WriteInt64(long l)
        {
#if NETFRAMEWORK
            BinaryPrimitives.WriteInt64BigEndian(temp, l);
            hash.AppendData(temp, 0, 8);
#else
			var span = (Span<byte>)stackalloc byte[8];
			BinaryPrimitives.WriteInt64BigEndian(span, l);
			hash.AppendData(span);
#endif
        }

        public void WriteFloat(float f)
        {
            WriteUInt32(BitConverter.ToUInt32(BitConverter.GetBytes(f), 0));
        }

        public void WriteDouble(double d)
        {
            WriteInt64(BitConverter.ToInt64(BitConverter.GetBytes(d), 0));
        }

        public void WriteByte(byte b)
        {
#if NETFRAMEWORK
            temp[0] = b;
            hash.AppendData(temp, 0, 1);
#else
			var span = (Span<byte>)stackalloc byte[1];
			span[0] = b;
			hash.AppendData(span);
#endif
        }

        public void WriteBytes(byte[] data)
        {
			hash.AppendData(data);
        }

        public void WriteUtf8(string str)
        {
            var buf = ArrayPool<byte>.Shared.Rent(str.Length * 3 + 1);

            try
            {
                int j = 0;
                for (int i = 0, e = str.Length; i < e; i++)
                {
                    char ch = str[i];
                    if ((ch != 0) && (ch <= 0x7f))
                    {
                        buf[j++] = (byte)ch;
                    }
                    else if (ch <= 0x7FF)
                    {
                        /* 11 bits or less. */
                        var high_five = (byte)(ch >> 6);
                        var low_six = (byte)(ch & 0x3F);
                        buf[j++] = (byte)(high_five | 0xC0); /* 110xxxxx */
                        buf[j++] = (byte)(low_six | 0x80);   /* 10xxxxxx */
                    }
                    else
                    {
                        /* possibly full 16 bits. */
                        var high_four = (byte)(ch >> 12);
                        var mid_six = (byte)((ch >> 6) & 0x3F);
                        var low_six = (byte)(ch & 0x3f);
                        buf[j++] = (byte)(high_four | 0xE0); /* 1110xxxx */
                        buf[j++] = (byte)(mid_six | 0x80);   /* 10xxxxxx */
                        buf[j++] = (byte)(low_six | 0x80);   /* 10xxxxxx*/
                    }
                }

                WriteUInt16((ushort)j);
                hash.AppendData(buf, 0, j);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buf);
            }
        }

        /// <summary>
        /// Finalizes the SHA1.
        /// </summary>
        /// <returns></returns>
        public byte[] Finalize()
        {
            return hash.GetHashAndReset();
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
#if NETFRAMEWORK
            ArrayPool<byte>.Shared.Return(temp);
#endif
        }

    }

}
