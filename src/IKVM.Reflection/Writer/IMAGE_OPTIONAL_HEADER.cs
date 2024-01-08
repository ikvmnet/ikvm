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
using System.IO;

using BYTE = System.Byte;
using DWORD = System.UInt32;
using IMAGE_DATA_DIRECTORY = IKVM.Reflection.Reader.IMAGE_DATA_DIRECTORY;
using ULONGLONG = System.UInt64;
using WORD = System.UInt16;

namespace IKVM.Reflection.Writer
{

    sealed class IMAGE_OPTIONAL_HEADER
	{

		public const WORD IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b;
		public const WORD IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b;

		public const WORD IMAGE_SUBSYSTEM_WINDOWS_GUI = 2;
		public const WORD IMAGE_SUBSYSTEM_WINDOWS_CUI = 3;

		public WORD Magic = IMAGE_NT_OPTIONAL_HDR32_MAGIC;
		public BYTE MajorLinkerVersion = 8;
		public BYTE MinorLinkerVersion = 0;
		public DWORD SizeOfCode;
		public DWORD SizeOfInitializedData;
		public DWORD SizeOfUninitializedData;
		public DWORD AddressOfEntryPoint;
		public DWORD BaseOfCode;
		public DWORD BaseOfData;
		public ULONGLONG ImageBase;
		public DWORD SectionAlignment = 0x2000;
		public DWORD FileAlignment;
		public WORD MajorOperatingSystemVersion = 4;
		public WORD MinorOperatingSystemVersion = 0;
		public WORD MajorImageVersion = 0;
		public WORD MinorImageVersion = 0;
		public WORD MajorSubsystemVersion = 4;
		public WORD MinorSubsystemVersion = 0;
		public DWORD Win32VersionValue = 0;
		public DWORD SizeOfImage;
		public DWORD SizeOfHeaders;
		public DWORD CheckSum = 0;
		public WORD Subsystem;
		public WORD DllCharacteristics;
		public ULONGLONG SizeOfStackReserve;
		public ULONGLONG SizeOfStackCommit = 0x1000;
		public ULONGLONG SizeOfHeapReserve = 0x100000;
		public ULONGLONG SizeOfHeapCommit = 0x1000;
		public DWORD LoaderFlags = 0;
		public DWORD NumberOfRvaAndSizes = 16;
		public IMAGE_DATA_DIRECTORY[] DataDirectory = new IMAGE_DATA_DIRECTORY[16];

		internal void Write(BinaryWriter bw)
		{
			bw.Write(Magic);
			bw.Write(MajorLinkerVersion);
			bw.Write(MinorLinkerVersion);
			bw.Write(SizeOfCode);
			bw.Write(SizeOfInitializedData);
			bw.Write(SizeOfUninitializedData);
			bw.Write(AddressOfEntryPoint);
			bw.Write(BaseOfCode);
			if (Magic == IMAGE_NT_OPTIONAL_HDR32_MAGIC)
			{
				bw.Write(BaseOfData);
				bw.Write((DWORD)ImageBase);
			}
			else
			{
				bw.Write(ImageBase);
			}
			bw.Write(SectionAlignment);
			bw.Write(FileAlignment);
			bw.Write(MajorOperatingSystemVersion);
			bw.Write(MinorOperatingSystemVersion);
			bw.Write(MajorImageVersion);
			bw.Write(MinorImageVersion);
			bw.Write(MajorSubsystemVersion);
			bw.Write(MinorSubsystemVersion);
			bw.Write(Win32VersionValue);
			bw.Write(SizeOfImage);
			bw.Write(SizeOfHeaders);
			bw.Write(CheckSum);
			bw.Write(Subsystem);
			bw.Write(DllCharacteristics);
			if (Magic == IMAGE_NT_OPTIONAL_HDR32_MAGIC)
			{
				bw.Write((DWORD)SizeOfStackReserve);
				bw.Write((DWORD)SizeOfStackCommit);
				bw.Write((DWORD)SizeOfHeapReserve);
				bw.Write((DWORD)SizeOfHeapCommit);
			}
			else
			{
				bw.Write(SizeOfStackReserve);
				bw.Write(SizeOfStackCommit);
				bw.Write(SizeOfHeapReserve);
				bw.Write(SizeOfHeapCommit);
			}
			bw.Write(LoaderFlags);
			bw.Write(NumberOfRvaAndSizes);
			for (int i = 0; i < DataDirectory.Length; i++)
			{
				bw.Write(DataDirectory[i].VirtualAddress);
				bw.Write(DataDirectory[i].Size);
			}
		}
	}
}
