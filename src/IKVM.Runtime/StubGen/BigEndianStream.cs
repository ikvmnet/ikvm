/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using System.IO;

namespace IKVM.StubGen
{

    sealed class BigEndianStream
	{

		private Stream stream;

		public BigEndianStream(Stream stream)
		{
			this.stream = stream;
		}

		public void WriteUInt16(ushort s)
		{
			stream.WriteByte((byte)(s >> 8));
			stream.WriteByte((byte)s);
		}

		public void WriteUInt32(uint u)
		{
			stream.WriteByte((byte)(u >> 24));
			stream.WriteByte((byte)(u >> 16));
			stream.WriteByte((byte)(u >> 8));
			stream.WriteByte((byte)u);
		}

		public void WriteInt64(long l)
		{
			WriteUInt32((uint)(l >> 32));
			WriteUInt32((uint)l);
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
			stream.WriteByte(b);
		}

		public void WriteBytes(byte[] data)
		{
			stream.Write(data, 0, data.Length);
		}

		public void WriteUtf8(string str)
		{
			byte[] buf = new byte[str.Length * 3 + 1];
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
					byte high_five = (byte)(ch >> 6);
					byte low_six = (byte)(ch & 0x3F);
					buf[j++] = (byte)(high_five | 0xC0); /* 110xxxxx */
					buf[j++] = (byte)(low_six | 0x80);   /* 10xxxxxx */
				}
				else
				{
					/* possibly full 16 bits. */
					byte high_four = (byte)(ch >> 12);
					byte mid_six = (byte)((ch >> 6) & 0x3F);
					byte low_six = (byte)(ch & 0x3f);
					buf[j++] = (byte)(high_four | 0xE0); /* 1110xxxx */
					buf[j++] = (byte)(mid_six | 0x80);   /* 10xxxxxx */
					buf[j++] = (byte)(low_six | 0x80);   /* 10xxxxxx*/
				}
			}
			WriteUInt16((ushort)j);
			stream.Write(buf, 0, j);
		}

	}

}
