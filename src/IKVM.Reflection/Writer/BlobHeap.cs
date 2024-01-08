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
using System.Diagnostics;

namespace IKVM.Reflection.Writer
{

    sealed class BlobHeap : SimpleHeap
	{

		private Key[] map = new Key[8179];
		private readonly ByteBuffer buf = new ByteBuffer(32);

		private struct Key
		{
			internal Key[] next;
			internal int len;
			internal int hash;
			internal int offset;
		}

		internal BlobHeap()
		{
			buf.Write((byte)0);
		}

		internal int Add(ByteBuffer bb)
		{
			Debug.Assert(!frozen);
			int bblen = bb.Length;
			if (bblen == 0)
			{
				return 0;
			}
			int lenlen = MetadataWriter.GetCompressedUIntLength(bblen);
			int hash = bb.Hash();
			int index = (hash & 0x7FFFFFFF) % map.Length;
			Key[] keys = map;
			int last = index;
			while (keys[index].offset != 0)
			{
				if (keys[index].hash == hash
					&& keys[index].len == bblen
					&& buf.Match(keys[index].offset + lenlen, bb, 0, bblen))
				{
					return keys[index].offset;
				}
				if (index == last)
				{
					if (keys[index].next == null)
					{
						keys[index].next = new Key[4];
						keys = keys[index].next;
						index = 0;
						break;
					}
					keys = keys[index].next;
					index = -1;
					last = keys.Length - 1;
				}
				index++;
			}
			int offset = buf.Position;
			buf.WriteCompressedUInt(bblen);
			buf.Write(bb);
			keys[index].len = bblen;
			keys[index].hash = hash;
			keys[index].offset = offset;
			return offset;
		}

		protected override int GetLength()
		{
			return buf.Position;
		}

		protected override void WriteImpl(MetadataWriter mw)
		{
			mw.Write(buf);
		}

		internal bool IsEmpty
		{
			get { return buf.Position == 1; }
		}

		internal IKVM.Reflection.Reader.ByteReader GetBlob(int blobIndex)
		{
			return buf.GetBlob(blobIndex);
		}

	}

}
