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

    sealed class IMAGE_FILE_HEADER
	{

		public const WORD IMAGE_FILE_MACHINE_I386 = 0x014c;
		public const WORD IMAGE_FILE_MACHINE_ARM = 0x01c4;
		public const WORD IMAGE_FILE_MACHINE_IA64 = 0x0200;
		public const WORD IMAGE_FILE_MACHINE_AMD64 = 0x8664;
		public const WORD IMAGE_FILE_MACHINE_ARM64 = 0xAA64;

        public const WORD IMAGE_FILE_32BIT_MACHINE = 0x0100;
		public const WORD IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002;
		public const WORD IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020;
		public const WORD IMAGE_FILE_DLL = 0x2000;

		public WORD Machine;
		public WORD NumberOfSections;
		public DWORD TimeDateStamp;
		public DWORD PointerToSymbolTable = 0;
		public DWORD NumberOfSymbols = 0;
		public WORD SizeOfOptionalHeader = 0xE0;
		public WORD Characteristics = IMAGE_FILE_EXECUTABLE_IMAGE;

	}

}
