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
using System.IO;

sealed class BigEndianBinaryReader
{
	private byte[] buf;
	private int pos;
	private int end;

	internal BigEndianBinaryReader(byte[] buf, int offset, int length)
	{
		this.buf = buf;
		this.pos = offset;
		this.end = checked(offset + length);
		if(offset < 0 || length < 0 || buf.Length - offset < length)
		{
			throw new ClassFormatError("Truncated class file");
		}
	}

	internal BigEndianBinaryReader Section(uint length)
	{
		BigEndianBinaryReader br = new BigEndianBinaryReader(buf, pos, checked((int)length));
		Skip(length);
		return br;
	}

	internal bool IsAtEnd
	{
		get
		{
			return pos == end;
		}
	}

	internal int Position
	{
		get
		{
			return pos;
		}
	}

	internal void Skip(uint count)
	{
		if(end - pos < count)
		{
			throw new ClassFormatError("Truncated class file");
		}
		checked
		{
			pos += (int)count;
		}
	}

	internal byte ReadByte()
	{
		if(pos == end)
		{
			throw new ClassFormatError("Truncated class file");
		}
		return buf[pos++];
	}

	internal sbyte ReadSByte()
	{
		if(pos == end)
		{
			throw new ClassFormatError("Truncated class file");
		}
		return (sbyte)buf[pos++];
	}

	internal double ReadDouble()
	{
		return BitConverter.Int64BitsToDouble(ReadInt64());
	}

	internal short ReadInt16()
	{
		if(end - pos < 2)
		{
			throw new ClassFormatError("Truncated class file");
		}
		short s = (short)((buf[pos] << 8) + buf[pos + 1]);
		pos += 2;
		return s;
	}

	internal int ReadInt32()
	{
		if(end - pos < 4)
		{
			throw new ClassFormatError("Truncated class file");
		}
		int i = (int)((buf[pos] << 24) + (buf[pos + 1] << 16) + (buf[pos + 2] << 8) + buf[pos + 3]);
		pos += 4;
		return i;
	}

	internal long ReadInt64()
	{
		if(end - pos < 8)
		{
			throw new ClassFormatError("Truncated class file");
		}
		uint i1 = (uint)((buf[pos] << 24) + (buf[pos + 1] << 16) + (buf[pos + 2] << 8) + buf[pos + 3]);
		uint i2 = (uint)((buf[pos + 4] << 24) + (buf[pos + 5] << 16) + (buf[pos + 6] << 8) + buf[pos + 7]);
		long l = (((long)i1) << 32) + i2;
		pos += 8;
		return l;
	}

	internal float ReadSingle()
	{
		return BitConverter.ToSingle(BitConverter.GetBytes(ReadInt32()), 0);
	}

	internal string ReadString(string classFile, int majorVersion)
	{
		int len = ReadUInt16();
		if(end - pos < len)
		{
			throw new ClassFormatError("{0} (Truncated class file)", classFile);
		}
		// special code path for ASCII strings (which occur *very* frequently)
		for(int j = 0; j < len; j++)
		{
			if(buf[pos + j] == 0 || buf[pos + j] >= 128)
			{
				// NOTE we *cannot* use System.Text.UTF8Encoding, because this is *not* compatible
				// (esp. for embedded nulls)
				char[] ch = new char[len];
				int l = 0;
				for(int i = 0; i < len; i++)
				{
					int c = buf[pos + i];
					int char2, char3;
					switch (c >> 4)
					{
						case 0:
							if(c == 0)
							{
								goto default;
							}
							break;
						case 1: case 2: case 3: case 4: case 5: case 6: case 7:
							// 0xxxxxxx
							break;
						case 12: case 13:
							// 110x xxxx   10xx xxxx
							char2 = buf[pos + ++i];
							if((char2 & 0xc0) != 0x80 || i >= len)
							{
								goto default;
							}
							c = (((c & 0x1F) << 6) | (char2 & 0x3F));
							if(c < 0x80 && c != 0 && majorVersion >= 48)
							{
								goto default;
							}
							break;
						case 14:
							// 1110 xxxx  10xx xxxx  10xx xxxx
							char2 = buf[pos + ++i];
							char3 = buf[pos + ++i];
							if((char2 & 0xc0) != 0x80 || (char3 & 0xc0) != 0x80 || i >= len)
							{
								goto default;
							}
							c = (((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0));
							if(c < 0x800 && majorVersion >= 48)
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
				return new String(ch, 0, l);
			}
		}
		string s = System.Text.ASCIIEncoding.ASCII.GetString(buf, pos, len);
		pos += len;
		return s;
	}

	internal ushort ReadUInt16()
	{
		if(end - pos < 2)
		{
			throw new ClassFormatError("Truncated class file");
		}
		ushort s = (ushort)((buf[pos] << 8) + buf[pos + 1]);
		pos += 2;
		return s;
	}

	internal uint ReadUInt32()
	{
		if(end - pos < 4)
		{
			throw new ClassFormatError("Truncated class file");
		}
		uint i = (uint)((buf[pos] << 24) + (buf[pos + 1] << 16) + (buf[pos + 2] << 8) + buf[pos + 3]);
		pos += 4;
		return i;
	}

	internal byte[] ToArray()
	{
		byte[] res = new byte[end - pos];
		Buffer.BlockCopy(buf, pos, res, 0, res.Length);
		return res;
	}
}
