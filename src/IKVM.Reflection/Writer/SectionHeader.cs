/*
  Copyright (C) 2008-2013 Jeroen Frijters

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
using DWORD = System.UInt32;
using WORD = System.UInt16;

namespace IKVM.Reflection.Writer
{

    class SectionHeader
	{

		public const DWORD IMAGE_SCN_CNT_CODE = 0x00000020;
		public const DWORD IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040;
		public const DWORD IMAGE_SCN_MEM_DISCARDABLE = 0x02000000;
		public const DWORD IMAGE_SCN_MEM_EXECUTE = 0x20000000;
		public const DWORD IMAGE_SCN_MEM_READ = 0x40000000;
		public const DWORD IMAGE_SCN_MEM_WRITE = 0x80000000;

		public string Name;		// 8 byte UTF8 encoded 0-padded
		public DWORD VirtualSize;
		public DWORD VirtualAddress;
		public DWORD SizeOfRawData;
		public DWORD PointerToRawData;
#pragma warning disable 649 // the follow fields are never assigned to
		public DWORD PointerToRelocations;
		public DWORD PointerToLinenumbers;
		public WORD NumberOfRelocations;
		public WORD NumberOfLinenumbers;
#pragma warning restore 649
		public DWORD Characteristics;

	}

}
