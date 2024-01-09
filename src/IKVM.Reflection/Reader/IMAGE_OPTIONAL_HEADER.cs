/*
  Copyright (C) 2009 Jeroen Frijters

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
using ULONGLONG = System.UInt64;
using WORD = System.UInt16;

namespace IKVM.Reflection.Reader
{

    sealed class IMAGE_OPTIONAL_HEADER
	{

		public const WORD IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b;
		public const WORD IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b;

		public const WORD IMAGE_SUBSYSTEM_WINDOWS_GUI = 2;
		public const WORD IMAGE_SUBSYSTEM_WINDOWS_CUI = 3;

		public WORD Magic;
		public BYTE MajorLinkerVersion;
		public BYTE MinorLinkerVersion;
		public DWORD SizeOfCode;
		public DWORD SizeOfInitializedData;
		public DWORD SizeOfUninitializedData;
		public DWORD AddressOfEntryPoint;
		public DWORD BaseOfCode;
		public DWORD BaseOfData;
		public ULONGLONG ImageBase;
		public DWORD SectionAlignment;
		public DWORD FileAlignment;
		public WORD MajorOperatingSystemVersion;
		public WORD MinorOperatingSystemVersion;
		public WORD MajorImageVersion;
		public WORD MinorImageVersion;
		public WORD MajorSubsystemVersion;
		public WORD MinorSubsystemVersion;
		public DWORD Win32VersionValue;
		public DWORD SizeOfImage;
		public DWORD SizeOfHeaders;
		public DWORD CheckSum;
		public WORD Subsystem;
		public WORD DllCharacteristics;
		public ULONGLONG SizeOfStackReserve;
		public ULONGLONG SizeOfStackCommit;
		public ULONGLONG SizeOfHeapReserve;
		public ULONGLONG SizeOfHeapCommit;
		public DWORD LoaderFlags;
		public DWORD NumberOfRvaAndSizes;
		public IMAGE_DATA_DIRECTORY[] DataDirectory;

		internal void Read(BinaryReader br)
		{
			Magic = br.ReadUInt16();
			if (Magic != IMAGE_NT_OPTIONAL_HDR32_MAGIC && Magic != IMAGE_NT_OPTIONAL_HDR64_MAGIC)
			{
				throw new BadImageFormatException();
			}
			MajorLinkerVersion = br.ReadByte();
			MinorLinkerVersion = br.ReadByte();
			SizeOfCode = br.ReadUInt32();
			SizeOfInitializedData = br.ReadUInt32();
			SizeOfUninitializedData = br.ReadUInt32();
			AddressOfEntryPoint = br.ReadUInt32();
			BaseOfCode = br.ReadUInt32();
			if (Magic == IMAGE_NT_OPTIONAL_HDR32_MAGIC)
			{
				BaseOfData = br.ReadUInt32();
				ImageBase = br.ReadUInt32();
			}
			else
			{
				ImageBase = br.ReadUInt64();
			}
			SectionAlignment = br.ReadUInt32();
			FileAlignment = br.ReadUInt32();
			MajorOperatingSystemVersion = br.ReadUInt16();
			MinorOperatingSystemVersion = br.ReadUInt16();
			MajorImageVersion = br.ReadUInt16();
			MinorImageVersion = br.ReadUInt16();
			MajorSubsystemVersion = br.ReadUInt16();
			MinorSubsystemVersion = br.ReadUInt16();
			Win32VersionValue = br.ReadUInt32();
			SizeOfImage = br.ReadUInt32();
			SizeOfHeaders = br.ReadUInt32();
			CheckSum = br.ReadUInt32();
			Subsystem = br.ReadUInt16();
			DllCharacteristics = br.ReadUInt16();
			if (Magic == IMAGE_NT_OPTIONAL_HDR32_MAGIC)
			{
				SizeOfStackReserve = br.ReadUInt32();
				SizeOfStackCommit = br.ReadUInt32();
				SizeOfHeapReserve = br.ReadUInt32();
				SizeOfHeapCommit = br.ReadUInt32();
			}
			else
			{
				SizeOfStackReserve = br.ReadUInt64();
				SizeOfStackCommit = br.ReadUInt64();
				SizeOfHeapReserve = br.ReadUInt64();
				SizeOfHeapCommit = br.ReadUInt64();
			}
			LoaderFlags = br.ReadUInt32();
			NumberOfRvaAndSizes = br.ReadUInt32();
			DataDirectory = new IMAGE_DATA_DIRECTORY[NumberOfRvaAndSizes];
			for (uint i = 0; i < NumberOfRvaAndSizes; i++)
			{
				DataDirectory[i] = new IMAGE_DATA_DIRECTORY();
				DataDirectory[i].Read(br);
			}
		}

	}

}
