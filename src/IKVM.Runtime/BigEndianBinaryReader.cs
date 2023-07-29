/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Text;

namespace IKVM.Runtime
{

    sealed class BigEndianBinaryReader
    {

        readonly ReadOnlyMemory<byte> buf;

        int pos;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="buf"></param>
        /// <exception cref="ClassFormatError"></exception>
        internal BigEndianBinaryReader(ReadOnlyMemory<byte> buf)
        {
            this.buf = buf;
            this.pos = 0;
        }

        internal BigEndianBinaryReader Section(uint length)
        {
            var br = new BigEndianBinaryReader(buf.Slice(pos, checked((int)length)));
            Skip(length);
            return br;
        }

        internal bool IsAtEnd => pos == buf.Length;

        internal int Position => pos;

        internal void Skip(uint count)
        {
            if (buf.Length - pos < count)
                throw new ClassFormatError("Truncated class file");

            checked
            {
                pos += (int)count;
            }
        }

        internal byte ReadByte()
        {
            if (pos == buf.Length)
                throw new ClassFormatError("Truncated class file");

            return buf.Span[pos++];
        }

        internal sbyte ReadSByte()
        {
            if (pos == buf.Length)
                throw new ClassFormatError("Truncated class file");

            return (sbyte)buf.Span[pos++];
        }

        internal double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadInt64());
        }

        internal short ReadInt16()
        {
            if (buf.Length - pos < 2)
                throw new ClassFormatError("Truncated class file");

            var s = (short)((buf.Span[pos] << 8) + buf.Span[pos + 1]);
            pos += 2;

            return s;
        }

        internal int ReadInt32()
        {
            if (buf.Length - pos < 4)
                throw new ClassFormatError("Truncated class file");

            var i = (int)((buf.Span[pos] << 24) + (buf.Span[pos + 1] << 16) + (buf.Span[pos + 2] << 8) + buf.Span[pos + 3]);
            pos += 4;
            return i;
        }

        internal long ReadInt64()
        {
            if (buf.Length - pos < 8)
                throw new ClassFormatError("Truncated class file");

            var i1 = (uint)((buf.Span[pos] << 24) + (buf.Span[pos + 1] << 16) + (buf.Span[pos + 2] << 8) + buf.Span[pos + 3]);
            var i2 = (uint)((buf.Span[pos + 4] << 24) + (buf.Span[pos + 5] << 16) + (buf.Span[pos + 6] << 8) + buf.Span[pos + 7]);
            var l = (((long)i1) << 32) + i2;
            pos += 8;
            return l;
        }

        internal float ReadSingle()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(ReadInt32()), 0);
        }

        internal unsafe string ReadString(string classFile, int majorVersion)
        {
            int len = ReadUInt16();
            if (buf.Length - pos < len)
                throw new ClassFormatError("{0} (Truncated class file)", classFile);

            // special code path for ASCII strings (which occur *very* frequently)
            for (int j = 0; j < len; j++)
            {
                if (buf.Span[pos + j] == 0 || buf.Span[pos + j] >= 128)
                {
                    // NOTE we *cannot* use System.Text.UTF8Encoding, because this is *not* compatible
                    // (esp. for embedded nulls)
                    char[] ch = new char[len];
                    int l = 0;
                    for (int i = 0; i < len; i++)
                    {
                        int c = buf.Span[pos + i];
                        int char2, char3;
                        switch (c >> 4)
                        {
                            case 0:
                                if (c == 0)
                                {
                                    goto default;
                                }
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                // 0xxxxxxx
                                break;
                            case 12:
                            case 13:
                                // 110x xxxx   10xx xxxx
                                char2 = buf.Span[pos + ++i];
                                if ((char2 & 0xc0) != 0x80 || i >= len)
                                {
                                    goto default;
                                }
                                c = (((c & 0x1F) << 6) | (char2 & 0x3F));
                                if (c < 0x80 && c != 0 && majorVersion >= 48)
                                {
                                    goto default;
                                }
                                break;
                            case 14:
                                // 1110 xxxx  10xx xxxx  10xx xxxx
                                char2 = buf.Span[pos + ++i];
                                char3 = buf.Span[pos + ++i];
                                if ((char2 & 0xc0) != 0x80 || (char3 & 0xc0) != 0x80 || i >= len)
                                {
                                    goto default;
                                }
                                c = (((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0));
                                if (c < 0x800 && majorVersion >= 48)
                                {
                                    goto default;
                                }
                                break;
                            default:
                                throw new ClassFormatError("Illegal UTF8 string in constant pool in class file {0}", classFile ?? "<Unknown>");
                        }
                        ch[l++] = (char)c;
                    }
                    pos += len;
                    return new string(ch, 0, l);
                }
            }

#if NETFRAMEWORK
            string s;
            fixed (byte* p = buf.Slice(pos, len).Span)
                s = Encoding.ASCII.GetString(p, buf.Length);
#else
        var s = Encoding.ASCII.GetString(buf.Slice(pos, len).Span);
#endif
            pos += len;
            return s;
        }

        internal ushort ReadUInt16()
        {
            if (buf.Length - pos < 2)
                throw new ClassFormatError("Truncated class file");

            var s = (ushort)((buf.Span[pos] << 8) + buf.Span[pos + 1]);
            pos += 2;
            return s;
        }

        internal uint ReadUInt32()
        {
            if (buf.Length - pos < 4)
                throw new ClassFormatError("Truncated class file");

            uint i = (uint)((buf.Span[pos] << 24) + (buf.Span[pos + 1] << 16) + (buf.Span[pos + 2] << 8) + buf.Span[pos + 3]);
            pos += 4;
            return i;
        }

        internal byte[] ToArray()
        {
            var res = new byte[buf.Length - pos];
            buf.Slice(pos, res.Length).CopyTo(res);
            return res;
        }

    }

}
