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
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using IKVM.Reflection.Emit.Impl;

namespace IKVM.Reflection.Emit.Writer
{
	sealed class TextSection
	{
		private readonly PEWriter peWriter;
		private readonly CliHeader cliHeader;
		private readonly ModuleBuilder moduleBuilder;

		internal TextSection(PEWriter peWriter, CliHeader cliHeader, ModuleBuilder moduleBuilder)
		{
			this.peWriter = peWriter;
			this.cliHeader = cliHeader;
			this.moduleBuilder = moduleBuilder;
			moduleBuilder.Freeze();
		}

		internal uint PointerToRawData
		{
			get { return peWriter.ToFileAlignment(peWriter.HeaderSize); }
		}

		internal uint BaseRVA
		{
			get { return 0x2000; }
		}

		internal uint ImportAddressTableRVA
		{
			get { return BaseRVA; }
		}

		internal uint ImportAddressTableLength
		{
			get
			{
				if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386)
				{
					return 8;
				}
				else
				{
					return 16;
				}
			}
		}

		internal uint ComDescriptorRVA
		{
			get { return ImportAddressTableRVA + ImportAddressTableLength; }
		}

		internal uint ComDescriptorLength
		{
			get { return cliHeader.Cb; }
		}

		internal uint MethodBodiesRVA
		{
			get { return (ComDescriptorRVA + ComDescriptorLength + 7) & ~7U; }
		}

		private uint MethodBodiesLength
		{
			get { return (uint)moduleBuilder.methodBodies.Length; }
		}

		private uint ResourcesRVA
		{
			get
			{
				if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386)
				{
					return (MethodBodiesRVA + MethodBodiesLength + 3) & ~3U;
				}
				else
				{
					return (MethodBodiesRVA + MethodBodiesLength + 15) & ~15U;
				}
			}
		}

		private uint ResourcesLength
		{
			get { return (uint)moduleBuilder.manifestResources.Length; }
		}

		internal uint StrongNameSignatureRVA
		{
			get
			{
				return (ResourcesRVA + ResourcesLength + 3) & ~3U;
			}
		}

		internal uint StrongNameSignatureLength
		{
			get
			{
				if ((cliHeader.Flags & CliHeader.COMIMAGE_FLAGS_STRONGNAMESIGNED) != 0)
				{
					return 128;
				}
				else
				{
					return 0;
				}
			}
		}

		private uint MetadataRVA
		{
			get
			{
				return (StrongNameSignatureRVA + StrongNameSignatureLength + 3) & ~3U;
			}
		}

		private uint MetadataLength
		{
			get { return (uint)moduleBuilder.MetadataLength; }
		}

		internal uint DebugDirectoryRVA
		{
			get { return MetadataRVA + MetadataLength; }
		}

		internal uint DebugDirectoryLength
		{
			get
			{
				if (moduleBuilder.symbolWriter != null)
				{
					return 28;
				}
				return 0;
			}
		}

		private uint DebugDirectoryContentsLength
		{
			get
			{
				if (moduleBuilder.symbolWriter != null)
				{
					PdbSupport.IMAGE_DEBUG_DIRECTORY idd = new PdbSupport.IMAGE_DEBUG_DIRECTORY();
					return (uint)PdbSupport.GetDebugInfo(moduleBuilder.symbolWriter, ref idd).Length;
				}
				return 0;
			}
		}

		internal uint ImportDirectoryRVA
		{
			get { return (DebugDirectoryRVA + DebugDirectoryLength + DebugDirectoryContentsLength + 3) & ~3U; }
		}

		internal uint ImportDirectoryLength
		{
			get { return (ImportHintNameTableRVA - ImportDirectoryRVA) + 27; }
		}

		private uint ImportHintNameTableRVA
		{
			get
			{
				if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386)
				{
					return (ImportDirectoryRVA + 48 + 15) & ~15U;
				}
				else
				{
					return (ImportDirectoryRVA + 48 + 4 + 15) & ~15U;
				}
			}
		}

		internal uint StartupStubRVA
		{
			get
			{
				if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_IA64)
				{
					return (ImportDirectoryRVA + ImportDirectoryLength + 15U) & ~15U;
				}
				else
				{
					// the additional 2 bytes padding are to align the address in the jump (which is a relocation fixup)
					return 2 + ((ImportDirectoryRVA + ImportDirectoryLength + 3U) & ~3U);
				}
			}
		}

		internal uint StartupStubLength
		{
			get
			{
				if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64)
				{
					return 12;
				}
				else if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_IA64)
				{
					return 48;
				}
				else
				{
					return 6;
				}
			}
		}

		private void WriteRVA(MetadataWriter mw, uint rva)
		{
			if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386)
			{
				mw.Write(rva);
			}
			else
			{
				mw.Write((ulong)rva);
			}
		}

		internal void Write(MetadataWriter mw, int sdataRVA)
		{
			// Now that we're ready to start writing, we need to do some fix ups
			moduleBuilder.Tables.MethodDef.Fixup(this);
			moduleBuilder.Tables.MethodImpl.Fixup(moduleBuilder);
			moduleBuilder.Tables.MethodSemantics.Fixup(moduleBuilder);
			moduleBuilder.Tables.InterfaceImpl.Fixup(moduleBuilder);
			moduleBuilder.Tables.Constant.Fixup(moduleBuilder);
			moduleBuilder.Tables.FieldMarshal.Fixup(moduleBuilder);
			moduleBuilder.Tables.DeclSecurity.Fixup(moduleBuilder);
			moduleBuilder.Tables.CustomAttribute.Fixup(moduleBuilder);
			moduleBuilder.Tables.FieldLayout.Fixup(moduleBuilder);
			moduleBuilder.Tables.FieldRVA.Fixup(moduleBuilder, sdataRVA);
			moduleBuilder.Tables.ImplMap.Fixup(moduleBuilder);
			moduleBuilder.Tables.GenericParam.Fixup(moduleBuilder);
			moduleBuilder.Tables.MethodSpec.Fixup(moduleBuilder);
			moduleBuilder.Tables.GenericParamConstraint.Fixup(moduleBuilder);

			// Import Address Table
			AssertRVA(mw, ImportAddressTableRVA);
			WriteRVA(mw, ImportHintNameTableRVA);
			WriteRVA(mw, 0);

			// CLI Header
			AssertRVA(mw, ComDescriptorRVA);
			cliHeader.MetaDataRVA = MetadataRVA;
			cliHeader.MetaDataSize = MetadataLength;
			if (ResourcesLength != 0)
			{
				cliHeader.ResourcesRVA = ResourcesRVA;
				cliHeader.ResourcesSize = ResourcesLength;
			}
			if (StrongNameSignatureLength != 0)
			{
				cliHeader.StrongNameSignatureRVA = StrongNameSignatureRVA;
				cliHeader.StrongNameSignatureSize = StrongNameSignatureLength;
			}
			cliHeader.Write(mw);

			// alignment padding
			for (int i = (int)(MethodBodiesRVA - (ComDescriptorRVA + ComDescriptorLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Method Bodies
			mw.Write(moduleBuilder.methodBodies);

			// alignment padding
			for (int i = (int)(ResourcesRVA - (MethodBodiesRVA + MethodBodiesLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Resources
			mw.Write(moduleBuilder.manifestResources);

			// The strong name signature live here (if it exists), but it will written later
			// and the following alignment padding will take care of reserving the space.

			// alignment padding
			for (int i = (int)(MetadataRVA - (ResourcesRVA + ResourcesLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Metadata
			AssertRVA(mw, MetadataRVA);
			moduleBuilder.WriteMetadata(mw);

			// Debug Directory
			AssertRVA(mw, DebugDirectoryRVA);
			WriteDebugDirectory(mw);

			// alignment padding
			for (int i = (int)(ImportDirectoryRVA - (DebugDirectoryRVA + DebugDirectoryLength + DebugDirectoryContentsLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Import Directory
			AssertRVA(mw, ImportDirectoryRVA);
			WriteImportDirectory(mw);

			// alignment padding
			for (int i = (int)(StartupStubRVA - (ImportDirectoryRVA + ImportDirectoryLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Startup Stub
			AssertRVA(mw, StartupStubRVA);
			if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64)
			{
				/*
				 *   48 A1 00 20 40 00 00 00 00 00        mov         rax,qword ptr [0000000000402000h]
				 *   FF E0                                jmp         rax
				 */
				mw.Write((ushort)0xA148);
				mw.Write(peWriter.Headers.OptionalHeader.ImageBase + ImportAddressTableRVA);
				mw.Write((ushort)0xE0FF);
			}
			else if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_IA64)
			{
				mw.Write(new byte[] {
						0x0B, 0x48, 0x00, 0x02, 0x18, 0x10, 0xA0, 0x40, 0x24, 0x30, 0x28, 0x00, 0x00, 0x00, 0x04, 0x00,
						0x10, 0x08, 0x00, 0x12, 0x18, 0x10, 0x60, 0x50, 0x04, 0x80, 0x03, 0x00, 0x60, 0x00, 0x80, 0x00
					});
				mw.Write(peWriter.Headers.OptionalHeader.ImageBase + StartupStubRVA);
				mw.Write(peWriter.Headers.OptionalHeader.ImageBase + BaseRVA);
			}
			else
			{
				mw.Write((ushort)0x25FF);
				mw.Write((uint)peWriter.Headers.OptionalHeader.ImageBase + ImportAddressTableRVA);
			}
		}

		[Conditional("DEBUG")]
		private void AssertRVA(MetadataWriter mw, uint rva)
		{
			Debug.Assert(mw.Position - PointerToRawData + BaseRVA == rva);
		}

		private void WriteDebugDirectory(MetadataWriter mw)
		{
			if (moduleBuilder.symbolWriter != null)
			{
				PdbSupport.IMAGE_DEBUG_DIRECTORY idd = new PdbSupport.IMAGE_DEBUG_DIRECTORY();
				idd.Characteristics = 0;
				idd.TimeDateStamp = peWriter.Headers.FileHeader.TimeDateStamp;
				byte[] buf = PdbSupport.GetDebugInfo(moduleBuilder.symbolWriter, ref idd);
				idd.PointerToRawData = (DebugDirectoryRVA - BaseRVA) + DebugDirectoryLength + PointerToRawData;
				mw.Write(idd.Characteristics);
				mw.Write(idd.TimeDateStamp);
				mw.Write(idd.MajorVersion);
				mw.Write(idd.MinorVersion);
				mw.Write(idd.Type);
				mw.Write(idd.SizeOfData);
				mw.Write(idd.AddressOfRawData);
				mw.Write(idd.PointerToRawData);
				mw.Write(buf);
			}
		}

		private void WriteImportDirectory(MetadataWriter mw)
		{
			mw.Write(ImportDirectoryRVA + 40);		// ImportLookupTable
			mw.Write(0);							// DateTimeStamp
			mw.Write(0);							// ForwarderChain
			mw.Write(ImportHintNameTableRVA + 14);	// Name
			mw.Write(ImportAddressTableRVA);
			mw.Write(new byte[20]);
			// Import Lookup Table
			mw.Write(ImportHintNameTableRVA);		// Hint/Name Table RVA
			int size = 48;
			if (peWriter.Headers.FileHeader.Machine != IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386)
			{
				size += 4;
				mw.Write(0);
			}
			mw.Write(0);

			// alignment padding
			for (int i = (int)(ImportHintNameTableRVA - (ImportDirectoryRVA + size)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Hint/Name Table
			AssertRVA(mw, ImportHintNameTableRVA);
			mw.Write((ushort)0);		// Hint
			if ((peWriter.Headers.FileHeader.Characteristics & IMAGE_FILE_HEADER.IMAGE_FILE_DLL) != 0)
			{
				mw.Write(System.Text.Encoding.ASCII.GetBytes("_CorDllMain"));
			}
			else
			{
				mw.Write(System.Text.Encoding.ASCII.GetBytes("_CorExeMain"));
			}
			mw.Write((byte)0);
			// Name
			mw.Write(System.Text.Encoding.ASCII.GetBytes("mscoree.dll"));
			mw.Write((ushort)0);
		}

		internal int Length
		{
			get { return (int)(StartupStubRVA - BaseRVA + StartupStubLength); }
		}
	}

	class CliHeader
	{
		internal const uint COMIMAGE_FLAGS_ILONLY = 0x00000001;
		internal const uint COMIMAGE_FLAGS_32BITREQUIRED = 0x00000002;
		internal const uint COMIMAGE_FLAGS_STRONGNAMESIGNED = 0x00000008;

		internal uint Cb = 0x48;
		internal ushort MajorRuntimeVersion;
		internal ushort MinorRuntimeVersion;
		internal uint MetaDataRVA;
		internal uint MetaDataSize;
		internal uint Flags;
		internal uint EntryPointToken;
		internal uint ResourcesRVA;
		internal uint ResourcesSize;
		internal uint StrongNameSignatureRVA;
		internal uint StrongNameSignatureSize;
		internal ulong CodeManagerTable = 0;
		internal uint VTableFixupsRVA = 0;
		internal uint VTableFixupsSize = 0;
		internal ulong ExportAddressTableJumps = 0;
		internal ulong ManagedNativeHeader = 0;

		internal void Write(MetadataWriter mw)
		{
			mw.Write(Cb);
			mw.Write(MajorRuntimeVersion);
			mw.Write(MinorRuntimeVersion);
			mw.Write(MetaDataRVA);
			mw.Write(MetaDataSize);
			mw.Write(Flags);
			mw.Write(EntryPointToken);
			mw.Write(ResourcesRVA);
			mw.Write(ResourcesSize);
			mw.Write(StrongNameSignatureRVA);
			mw.Write(StrongNameSignatureSize);
			mw.Write(CodeManagerTable);
			mw.Write(VTableFixupsRVA);
			mw.Write(VTableFixupsSize);
			mw.Write(ExportAddressTableJumps);
			mw.Write(ManagedNativeHeader);
		}
	}
}
