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

#if STATIC_COMPILER || STUB_GENERATOR
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
	public sealed class LineNumberTableAttribute : Attribute
	{
		private byte[] table;

		public LineNumberTableAttribute(ushort lineno)
		{
			LineNumberWriter w = new LineNumberWriter(1);
			w.AddMapping(0, lineno);
			table = w.ToArray();
		}

		public LineNumberTableAttribute(byte[] table)
		{
			this.table = table;
		}

		public sealed class LineNumberWriter
		{
			private System.IO.MemoryStream stream;
			private int prevILOffset;
			private int prevLineNum;
			private int count;

			public LineNumberWriter(int estimatedCount)
			{
				stream = new System.IO.MemoryStream(estimatedCount * 2);
			}

			public void AddMapping(int ilOffset, int linenumber)
			{
				if(count == 0)
				{
					if(ilOffset == 0 && linenumber != 0)
					{
						prevLineNum = linenumber;
						count++;
						WritePackedInteger(linenumber - (64 + 50));
						return;
					}
					else
					{
						prevLineNum = linenumber & ~3;
						WritePackedInteger(((-prevLineNum / 4) - (64 + 50)));
					}
				}
				bool pc_overflow;
				bool lineno_overflow;
				byte lead;
				int deltaPC = ilOffset - prevILOffset;
				if(deltaPC >= 0 && deltaPC < 31)
				{
					lead = (byte)deltaPC;
					pc_overflow = false;
				}
				else
				{
					lead = (byte)31;
					pc_overflow = true;
				}
				int deltaLineNo = linenumber - prevLineNum;
				const int bias = 2;
				if(deltaLineNo >= -bias && deltaLineNo < 7 - bias)
				{
					lead |= (byte)((deltaLineNo + bias) << 5);
					lineno_overflow = false;
				}
				else
				{
					lead |= (byte)(7 << 5);
					lineno_overflow = true;
				}
				stream.WriteByte(lead);
				if(pc_overflow)
				{
					WritePackedInteger(deltaPC - (64 + 31));
				}
				if(lineno_overflow)
				{
					WritePackedInteger(deltaLineNo);
				}
				prevILOffset = ilOffset;
				prevLineNum = linenumber;
				count++;
			}

			public int Count
			{
				get
				{
					return count;
				}
			}

			public int LineNo
			{
				get
				{
					return prevLineNum;
				}
			}

			public byte[] ToArray()
			{
				return stream.ToArray();
			}

			/*
			 * packed integer format:
			 * ----------------------
			 * 
			 * First byte:
			 * 00 - 7F      Single byte integer (-64 - 63)
			 * 80 - BF      Double byte integer (-8192 - 8191)
			 * C0 - DF      Triple byte integer (-1048576 - 1048576)
			 * E0 - FE      Reserved
			 * FF           Five byte integer
			 */
			private void WritePackedInteger(int val)
			{
				if(val >= -64 && val < 64)
				{
					val += 64;
					stream.WriteByte((byte)val);
				}
				else if(val >= -8192 && val < 8192)
				{
					val += 8192;
					stream.WriteByte((byte)(0x80 + (val >> 8)));
					stream.WriteByte((byte)val);
				}
				else if(val >= -1048576 && val < 1048576)
				{
					val += 1048576;
					stream.WriteByte((byte)(0xC0 + (val >> 16)));
					stream.WriteByte((byte)(val >> 8));
					stream.WriteByte((byte)val);
				}
				else
				{
					stream.WriteByte(0xFF);
					stream.WriteByte((byte)(val >> 24));
					stream.WriteByte((byte)(val >> 16));
					stream.WriteByte((byte)(val >>  8));
					stream.WriteByte((byte)(val >>  0));
				}
			}
		}

		private int ReadPackedInteger(ref int position)
		{
			byte b = table[position++];
			if(b < 128)
			{
				return b - 64;
			}
			else if((b & 0xC0) == 0x80)
			{
				return ((b & 0x7F) << 8) + table[position++] - 8192;
			}
			else if((b & 0xE0) == 0xC0)
			{
				int val = ((b & 0x3F) << 16);
				val += (table[position++] << 8);
				val += table[position++];
				return val - 1048576;
			}
			else if(b == 0xFF)
			{
				int val = table[position++] << 24;
				val += table[position++] << 16;
				val += table[position++] <<  8;
				val += table[position++] <<  0;
				return val;
			}
			else
			{
				throw new InvalidProgramException();
			}
		}

		public int GetLineNumber(int ilOffset)
		{
			int i = 0;
			int prevILOffset = 0;
			int prevLineNum = ReadPackedInteger(ref i) + (64 + 50);
			int line;
			if(prevLineNum > 0)
			{
				line = prevLineNum;
			}
			else
			{
				prevLineNum = 4 * -prevLineNum;
				line = -1;
			}
			while(i < table.Length)
			{
				byte lead = table[i++];
				int deltaPC = lead & 31;
				int deltaLineNo = (lead >> 5) - 2;
				if(deltaPC == 31)
				{
					deltaPC = ReadPackedInteger(ref i) + (64 + 31);
				}
				if(deltaLineNo == 5)
				{
					deltaLineNo = ReadPackedInteger(ref i);
				}
				int currILOffset = prevILOffset + deltaPC;
				if(currILOffset > ilOffset)
				{
					return line;
				}
				line = prevLineNum + deltaLineNo;
				prevILOffset = currILOffset;
				prevLineNum = line;
			}
			return line;
		}
	}
}
