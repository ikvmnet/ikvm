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
using System.Collections.Generic;
using System.Diagnostics;

namespace IKVM.Reflection.Writer
{

    sealed class UserStringHeap : SimpleHeap
	{

		private List<string> list = new List<string>();
		private Dictionary<string, int> strings = new Dictionary<string, int>();
		private int nextOffset;

		internal UserStringHeap()
		{
			nextOffset = 1;
		}

		internal bool IsEmpty
		{
			get { return nextOffset == 1; }
		}

		internal int Add(string str)
		{
			Debug.Assert(!frozen);
			int offset;
			if (!strings.TryGetValue(str, out offset))
			{
				int length = str.Length * 2 + 1 + MetadataWriter.GetCompressedUIntLength(str.Length * 2 + 1);
				if (nextOffset + length > 0xFFFFFF)
				{
					throw new FileFormatLimitationExceededException("No logical space left to create more user strings.", FileFormatLimitationExceededException.META_E_STRINGSPACE_FULL);
				}
				offset = nextOffset;
				nextOffset += length;
				list.Add(str);
				strings.Add(str, offset);
			}
			return offset;
		}

		protected override int GetLength()
		{
			return nextOffset;
		}

		protected override void WriteImpl(MetadataWriter mw)
		{
			mw.Write((byte)0);
			foreach (string str in list)
			{
				mw.WriteCompressedUInt(str.Length * 2 + 1);
				byte hasSpecialChars = 0;
				foreach (char ch in str)
				{
					mw.Write((ushort)ch);
					if (hasSpecialChars == 0 && (ch < 0x20 || ch > 0x7E))
					{
						if (ch > 0x7E
							|| (ch >= 0x01 && ch <= 0x08)
							|| (ch >= 0x0E && ch <= 0x1F)
							|| ch == 0x27
							|| ch == 0x2D)
						{
							hasSpecialChars = 1;
						}
					}
				}
				mw.Write(hasSpecialChars);
			}
		}

	}

}
