/*
  Copyright (C) 2008 Jeroen Frijters

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
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Writer
{

    sealed class ByteBuffer
    {

        internal static ByteBuffer Wrap(byte[] buf)
        {
            return new ByteBuffer(buf, buf.Length);
        }

        byte[] buffer;
        int pos;
        int __length;   // __length is only valid if > pos, otherwise pos is the current length

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="initialCapacity"></param>
        internal ByteBuffer(int initialCapacity)
        {
            buffer = new byte[initialCapacity];
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="wrap"></param>
        /// <param name="length"></param>
        private ByteBuffer(byte[] wrap, int length)
        {
            buffer = wrap;
            pos = length;
        }

        internal int Position
        {
            get { return pos; }
            set
            {
                if (value > Length || value > buffer.Length)
                    throw new ArgumentOutOfRangeException();

                __length = Math.Max(__length, pos);
                pos = value;
            }
        }

        internal int Length
        {
            get { return Math.Max(pos, __length); }
        }

        void Grow(int minGrow)
        {
            var newbuf = new byte[Math.Max(buffer.Length + minGrow, buffer.Length * 2)];
            Buffer.BlockCopy(buffer, 0, newbuf, 0, buffer.Length);
            buffer = newbuf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This does not advance the position.
        /// </remarks>
        /// <returns></returns>
        internal int GetInt32AtCurrentPosition()
        {
            return buffer[pos]
                + (buffer[pos + 1] << 8)
                + (buffer[pos + 2] << 16)
                + (buffer[pos + 3] << 24);
        }

        internal void WriteBytes(byte[] value)
        {
            if (pos + value.Length > buffer.Length)
                Grow(value.Length);

            Buffer.BlockCopy(value, 0, buffer, pos, value.Length);
            pos += value.Length;
        }

        internal void WriteByte(byte value)
        {
            if (pos == buffer.Length)
                Grow(1);
            buffer[pos++] = value;
        }

        internal void WriteUInt16(ushort value)
        {
            WriteInt16((short)value);
        }

        internal void WriteInt16(short value)
        {
            if (pos + 2 > buffer.Length)
                Grow(2);

            buffer[pos++] = (byte)value;
            buffer[pos++] = (byte)(value >> 8);
        }

        internal void WriteUInt32(uint value)
        {
            WriteInt32((int)value);
        }

        internal void WriteInt32(int value)
        {
            if (pos + 4 > buffer.Length)
                Grow(4);

            buffer[pos++] = (byte)value;
            buffer[pos++] = (byte)(value >> 8);
            buffer[pos++] = (byte)(value >> 16);
            buffer[pos++] = (byte)(value >> 24);
        }

        internal void WriteBuffer(ByteBuffer bb)
        {
            if (pos + bb.Length > buffer.Length)
                Grow(bb.Length);

            Buffer.BlockCopy(bb.buffer, 0, buffer, pos, bb.Length);
            pos += bb.Length;
        }

        internal void Align(int alignment)
        {
            if (pos + alignment > buffer.Length)
                Grow(alignment);
            int newpos = (pos + alignment - 1) & ~(alignment - 1);
            while (pos < newpos)
                buffer[pos++] = 0;
        }

        internal byte[] ToArray()
        {
            var len = Length;
            var buf = new byte[len];
            Buffer.BlockCopy(buffer, 0, buf, 0, len);
            return buf;
        }

    }

}
