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

    /// <summary>
    /// A growable little-endian byte buffer with a movable write position, used to build unmanaged (Win32) resources.
    /// </summary>
    sealed class ByteBuffer
    {

        /// <summary>
        /// Creates a buffer over an existing array, positioned at its end.
        /// </summary>
        /// <param name="buf">The array to use as the buffer's initial contents. It is used directly, not copied.</param>
        /// <returns>A buffer whose length is the length of <paramref name="buf"/>.</returns>
        internal static ByteBuffer Wrap(byte[] buf) => new ByteBuffer(buf, buf.Length);

        byte[] _buffer;
        int _pos;

        /// <summary>
        /// The furthest position written before <see cref="Position"/> was moved back. It is only valid if greater than
        /// <see cref="_pos"/>; otherwise <see cref="_pos"/> is the current length.
        /// </summary>
        int __length;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="initialCapacity">The initial size of the underlying array.</param>
        internal ByteBuffer(int initialCapacity)
        {
            _buffer = new byte[initialCapacity];
        }

        /// <summary>
        /// Initializes a new instance over an existing array.
        /// </summary>
        /// <param name="wrap">The array to use as the buffer.</param>
        /// <param name="length">The number of bytes of <paramref name="wrap"/> that are already in use; the position starts here.</param>
        ByteBuffer(byte[] wrap, int length)
        {
            _buffer = wrap;
            _pos = length;
        }

        /// <summary>
        /// Gets or sets the position at which the next write occurs. Moving the position back does not shorten the
        /// buffer.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is beyond the current length or capacity.</exception>
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

        /// <summary>
        /// Gets the number of bytes written to the buffer.
        /// </summary>
        internal int Length => Math.Max(_pos, __length);

        /// <summary>
        /// Enlarges the underlying array to at least double its size, or by <paramref name="minGrow"/> bytes if that is
        /// more.
        /// </summary>
        /// <param name="minGrow">The minimum number of bytes to add.</param>
        void Grow(int minGrow)
        {
            var newbuf = new byte[Math.Max(_buffer.Length + minGrow, _buffer.Length * 2)];
            Buffer.BlockCopy(_buffer, 0, newbuf, 0, _buffer.Length);
            _buffer = newbuf;
        }

        /// <summary>
        /// Reads the little-endian 32-bit integer at the current position.
        /// </summary>
        /// <remarks>
        /// This does not advance the position.
        /// </remarks>
        /// <returns>The integer at the current position.</returns>
        internal int GetInt32AtCurrentPosition()
        {
            return _buffer[_pos]
                + (_buffer[_pos + 1] << 8)
                + (_buffer[_pos + 2] << 16)
                + (_buffer[_pos + 3] << 24);
        }

        /// <summary>
        /// Writes an array of bytes at the current position and advances the position.
        /// </summary>
        /// <param name="value">The bytes to write.</param>
        internal void WriteBytes(byte[] value)
        {
            if (_pos + value.Length > _buffer.Length)
                Grow(value.Length);

            Buffer.BlockCopy(value, 0, _buffer, _pos, value.Length);
            _pos += value.Length;
        }

        /// <summary>
        /// Writes a byte at the current position and advances the position.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        internal void WriteByte(byte value)
        {
            if (_pos == _buffer.Length)
                Grow(1);
            _buffer[_pos++] = value;
        }

        /// <summary>
        /// Writes a little-endian unsigned 16-bit integer at the current position and advances the position.
        /// </summary>
        /// <param name="value">The value to write.</param>
        internal void WriteUInt16(ushort value) => WriteInt16((short)value);

        /// <summary>
        /// Writes a little-endian 16-bit integer at the current position and advances the position.
        /// </summary>
        /// <param name="value">The value to write.</param>
        internal void WriteInt16(short value)
        {
            if (_pos + 2 > _buffer.Length)
                Grow(2);

            _buffer[_pos++] = (byte)value;
            _buffer[_pos++] = (byte)(value >> 8);
        }

        /// <summary>
        /// Writes a little-endian unsigned 32-bit integer at the current position and advances the position.
        /// </summary>
        /// <param name="value">The value to write.</param>
        internal void WriteUInt32(uint value) => WriteInt32((int)value);

        /// <summary>
        /// Writes a little-endian 32-bit integer at the current position and advances the position.
        /// </summary>
        /// <param name="value">The value to write.</param>
        internal void WriteInt32(int value)
        {
            if (_pos + 4 > _buffer.Length)
                Grow(4);

            _buffer[_pos++] = (byte)value;
            _buffer[_pos++] = (byte)(value >> 8);
            _buffer[_pos++] = (byte)(value >> 16);
            _buffer[_pos++] = (byte)(value >> 24);
        }

        /// <summary>
        /// Writes the contents of another buffer at the current position and advances the position.
        /// </summary>
        /// <param name="bb">The buffer whose contents, up to its length, are written.</param>
        internal void WriteBuffer(ByteBuffer bb)
        {
            if (_pos + bb.Length > _buffer.Length)
                Grow(bb.Length);

            Buffer.BlockCopy(bb._buffer, 0, _buffer, _pos, bb.Length);
            _pos += bb.Length;
        }

        /// <summary>
        /// Writes zero bytes until the position is a multiple of <paramref name="alignment"/>.
        /// </summary>
        /// <param name="alignment">The alignment, which must be a power of two.</param>
        internal void Align(int alignment)
        {
            if (_pos + alignment > _buffer.Length)
                Grow(alignment);
            var newpos = (_pos + alignment - 1) & ~(alignment - 1);
            while (_pos < newpos)
                _buffer[_pos++] = 0;
        }

        /// <summary>
        /// Copies the bytes written to the buffer to a new array.
        /// </summary>
        /// <returns>A new array of <see cref="Length"/> bytes.</returns>
        internal byte[] ToArray()
        {
            var len = Length;
            var buf = new byte[len];
            Buffer.BlockCopy(_buffer, 0, buf, 0, len);
            return buf;
        }

    }

}
