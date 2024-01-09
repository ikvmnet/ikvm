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
using System.IO;

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Metadata
{

    struct RvaSize
	{

		internal uint VirtualAddress;
		internal uint Size;

		internal void Read(BinaryReader br)
		{
			VirtualAddress = br.ReadUInt32();
			Size = br.ReadUInt32();
		}

		internal void Write(MetadataWriter mw)
		{
			mw.Write(VirtualAddress);
			mw.Write(Size);
		}

	}

}
