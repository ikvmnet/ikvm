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

using DWORD = System.UInt32;

namespace IKVM.Reflection.Reader
{

    sealed class PEReader
	{

		MSDOS_HEADER msdos = new MSDOS_HEADER();
		IMAGE_NT_HEADERS headers = new IMAGE_NT_HEADERS();
		SectionHeader[] sections;
		bool mapped;

		internal void Read(BinaryReader br, bool mapped)
		{
			this.mapped = mapped;
			msdos.signature = br.ReadUInt16();
			br.BaseStream.Seek(58, SeekOrigin.Current);
			msdos.peSignatureOffset = br.ReadUInt32();

			if (msdos.signature != MSDOS_HEADER.MAGIC_MZ)
				throw new BadImageFormatException();

			br.BaseStream.Seek(msdos.peSignatureOffset, SeekOrigin.Begin);
			headers.Read(br);
			sections = new SectionHeader[headers.FileHeader.NumberOfSections];
			for (int i = 0; i < sections.Length; i++)
			{
				sections[i] = new SectionHeader();
				sections[i].Read(br);
			}
		}

		internal IMAGE_FILE_HEADER FileHeader
		{
			get { return headers.FileHeader; }
		}

		internal IMAGE_OPTIONAL_HEADER OptionalHeader
		{
			get { return headers.OptionalHeader; }
		}

		internal DWORD GetComDescriptorVirtualAddress()
		{
			return headers.OptionalHeader.DataDirectory[14].VirtualAddress;
		}

		internal long RvaToFileOffset(DWORD rva)
		{
			if (mapped)
				return rva;

			for (int i = 0; i < sections.Length; i++)
				if (rva >= sections[i].VirtualAddress && rva < sections[i].VirtualAddress + sections[i].VirtualSize)
					return sections[i].PointerToRawData + rva - sections[i].VirtualAddress;

			throw new BadImageFormatException();
		}

	}

}
