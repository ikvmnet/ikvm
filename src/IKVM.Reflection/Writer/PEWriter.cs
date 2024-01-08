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

namespace IKVM.Reflection.Writer
{

    sealed class PEWriter
	{

		private readonly BinaryWriter bw;
		private readonly IMAGE_NT_HEADERS hdr = new IMAGE_NT_HEADERS();

		internal PEWriter(Stream stream)
		{
			bw = new BinaryWriter(stream);
			WriteMSDOSHeader();
		}

		public IMAGE_NT_HEADERS Headers
		{
			get { return hdr; }
		}

		public uint HeaderSize
		{
			get
			{
				return (uint)
					((8 * 16) +	// MSDOS header
					4 +				// signature
					20 +			// IMAGE_FILE_HEADER
					hdr.FileHeader.SizeOfOptionalHeader +
					hdr.FileHeader.NumberOfSections * 40);
			}
		}

		private void WriteMSDOSHeader()
		{
			bw.Write(new byte[] {
				0x4D, 0x5A, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00,
				0x04, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00,
				0xB8, 0x00, 0x00, 0x00, 0x00, 0x00,	0x00, 0x00,
				0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00,
				0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD,
				0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21, 0x54, 0x68,
				0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72,
				0x61, 0x6D, 0x20, 0x63, 0x61, 0x6E, 0x6E, 0x6F,
				0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x75, 0x6E,
				0x20, 0x69, 0x6E, 0x20, 0x44, 0x4F, 0x53, 0x20,
				0x6D, 0x6F, 0x64, 0x65, 0x2E, 0x0D, 0x0D, 0x0A,
				0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
			});
		}

		internal void WritePEHeaders()
		{
			bw.Write(hdr.Signature);

			// IMAGE_FILE_HEADER
			bw.Write(hdr.FileHeader.Machine);
			bw.Write(hdr.FileHeader.NumberOfSections);
			bw.Write(hdr.FileHeader.TimeDateStamp);
			bw.Write(hdr.FileHeader.PointerToSymbolTable);
			bw.Write(hdr.FileHeader.NumberOfSymbols);
			bw.Write(hdr.FileHeader.SizeOfOptionalHeader);
			bw.Write(hdr.FileHeader.Characteristics);

			// IMAGE_OPTIONAL_HEADER
			hdr.OptionalHeader.Write(bw);
		}

		internal void WriteSectionHeader(SectionHeader sectionHeader)
		{
			byte[] name = new byte[8];
			System.Text.Encoding.UTF8.GetBytes(sectionHeader.Name, 0, sectionHeader.Name.Length, name, 0);
			bw.Write(name);
			bw.Write(sectionHeader.VirtualSize);
			bw.Write(sectionHeader.VirtualAddress);
			bw.Write(sectionHeader.SizeOfRawData);
			bw.Write(sectionHeader.PointerToRawData);
			bw.Write(sectionHeader.PointerToRelocations);
			bw.Write(sectionHeader.PointerToLinenumbers);
			bw.Write(sectionHeader.NumberOfRelocations);
			bw.Write(sectionHeader.NumberOfLinenumbers);
			bw.Write(sectionHeader.Characteristics);
		}

		internal uint ToFileAlignment(uint p)
		{
			return (p + (Headers.OptionalHeader.FileAlignment - 1)) & ~(Headers.OptionalHeader.FileAlignment - 1);
		}

		internal uint ToSectionAlignment(uint p)
		{
			return (p + (Headers.OptionalHeader.SectionAlignment - 1)) & ~(Headers.OptionalHeader.SectionAlignment - 1);
		}

		internal bool Is32Bit
		{
			get { return (Headers.FileHeader.Characteristics & IMAGE_FILE_HEADER.IMAGE_FILE_32BIT_MACHINE) != 0; }
		}

		internal uint Thumb
		{
			// On ARM we need to set the least significant bit of the program counter to select the Thumb instruction set
			get { return Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM ? 1u : 0u; }
		}

	}

}
