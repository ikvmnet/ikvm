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

    sealed class IMAGE_NT_HEADERS
	{

		public const DWORD MAGIC_SIGNATURE = 0x00004550;	// "PE\0\0"

		public DWORD Signature;
		public IMAGE_FILE_HEADER FileHeader = new IMAGE_FILE_HEADER();
		public IMAGE_OPTIONAL_HEADER OptionalHeader = new IMAGE_OPTIONAL_HEADER();

		internal void Read(BinaryReader br)
		{
			Signature = br.ReadUInt32();
			if (Signature != IMAGE_NT_HEADERS.MAGIC_SIGNATURE)
			{
				throw new BadImageFormatException();
			}
			FileHeader.Read(br);
			long optionalHeaderPosition = br.BaseStream.Position;
			OptionalHeader.Read(br);
			if (br.BaseStream.Position > optionalHeaderPosition + FileHeader.SizeOfOptionalHeader)
			{
				throw new BadImageFormatException();
			}
			br.BaseStream.Seek(optionalHeaderPosition + FileHeader.SizeOfOptionalHeader, SeekOrigin.Begin);
		}

	}

}
