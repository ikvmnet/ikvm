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
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace IKVM.Reflection.Writer
{

    sealed class ByteBuffer
    {

        internal static ByteBuffer Wrap(byte[] buf) => new ByteBuffer(buf, buf.Length);

        byte[] _buffer;
        int _pos;
        int __length;   // __length is only valid if > pos, otherwise pos is the current length

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="initialCapacity"></param>
        internal ByteBuffer(int initialCapacity)
        {
            _buffer = new byte[initialCapacity];
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="wrap"></param>
        /// <param name="length"></param>
        ByteBuffer(byte[] wrap, int length)
        {
            _buffer = wrap;
            _pos = length;
        }

        internal int Position
        {
            get => _pos;
            set
            {
                if (value > Length || value > _buffer.Length)
                    throw new ArgumentOutOfRangeException();

                __length = Math.Max(__length, _pos);
                _pos = value;
            }
        }

        internal int Length => Math.Max(_pos, __length);

        void Grow(int minGrow)
        {
            var newbuf = new byte[Math.Max(_buffer.Length + minGrow, _buffer.Length * 2)];
            Buffer.BlockCopy(_buffer, 0, newbuf, 0, _buffer.Length);
            _buffer = newbuf;
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
            return _buffer[_pos]
                + (_buffer[_pos + 1] << 8)
                + (_buffer[_pos + 2] << 16)
                + (_buffer[_pos + 3] << 24);
        }

        internal void WriteBytes(byte[] value)
        {
            if (_pos + value.Length > _buffer.Length)
                Grow(value.Length);

            Buffer.BlockCopy(value, 0, _buffer, _pos, value.Length);
            _pos += value.Length;
        }

        internal void WriteByte(byte value)
        {
            if (_pos == _buffer.Length)
                Grow(1);
            _buffer[_pos++] = value;
        }

        internal void WriteUInt16(ushort value) => WriteInt16((short)value);

        internal void WriteInt16(short value)
        {
            if (_pos + 2 > _buffer.Length)
                Grow(2);

            _buffer[_pos++] = (byte)value;
            _buffer[_pos++] = (byte)(value >> 8);
        }

        internal void WriteUInt32(uint value) => WriteInt32((int)value);

        internal void WriteInt32(int value)
        {
            if (_pos + 4 > _buffer.Length)
                Grow(4);

            _buffer[_pos++] = (byte)value;
            _buffer[_pos++] = (byte)(value >> 8);
            _buffer[_pos++] = (byte)(value >> 16);
            _buffer[_pos++] = (byte)(value >> 24);
        }

        internal void WriteBuffer(ByteBuffer bb)
        {
            if (_pos + bb.Length > _buffer.Length)
                Grow(bb.Length);

            Buffer.BlockCopy(bb._buffer, 0, _buffer, _pos, bb.Length);
            _pos += bb.Length;
        }

        internal void Align(int alignment)
        {
            if (_pos + alignment > _buffer.Length)
                Grow(alignment);
            var newpos = (_pos + alignment - 1) & ~(alignment - 1);
            while (_pos < newpos)
                _buffer[_pos++] = 0;
        }

        internal byte[] ToArray()
        {
            var len = Length;
            var buf = new byte[len];
            Buffer.BlockCopy(_buffer, 0, buf, 0, len);
            return buf;
        }

    }

}
