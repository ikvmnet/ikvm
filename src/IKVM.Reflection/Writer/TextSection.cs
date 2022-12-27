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
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using IKVM.Reflection.Emit;
using IKVM.Reflection.Impl;
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Writer
{
	sealed class TextSection
	{
		private readonly PEWriter peWriter;
		private readonly CliHeader cliHeader;
		private readonly ModuleBuilder moduleBuilder;
		private readonly uint strongNameSignatureLength;
		private readonly uint manifestResourcesLength;
		private readonly ExportTables exportTables;
		private readonly List<RelocationBlock> relocations = new List<RelocationBlock>();

		internal TextSection(PEWriter peWriter, CliHeader cliHeader, ModuleBuilder moduleBuilder, int strongNameSignatureLength)
		{
			this.peWriter = peWriter;
			this.cliHeader = cliHeader;
			this.moduleBuilder = moduleBuilder;
			this.strongNameSignatureLength = (uint)strongNameSignatureLength;
			this.manifestResourcesLength = (uint)moduleBuilder.GetManifestResourcesLength();
			if (moduleBuilder.unmanagedExports.Count != 0)
			{
				this.exportTables = new ExportTables(this);
			}
		}

		internal uint PointerToRawData
		{
			get { return peWriter.ToFileAlignment(peWriter.HeaderSize); }
		}

		internal uint BaseRVA
		{
			get { return peWriter.Headers.OptionalHeader.SectionAlignment; }
		}

		internal uint ImportAddressTableRVA
		{
			get { return BaseRVA; }
		}

		internal uint ImportAddressTableLength
		{
			get { return peWriter.Is32Bit ? 8u : 16u; }
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
				switch (peWriter.Headers.FileHeader.Machine)
				{
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386:
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM:
						return (MethodBodiesRVA + MethodBodiesLength + 3) & ~3U;
					default:
						return (MethodBodiesRVA + MethodBodiesLength + 15) & ~15U;
				}
			}
		}

		private uint ResourcesLength
		{
			get { return manifestResourcesLength; }
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
				return strongNameSignatureLength;
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

		private uint VTableFixupsRVA
		{
			get { return (MetadataRVA + MetadataLength + 7) & ~7U; }
		}

		private uint VTableFixupsLength
		{
			get { return (uint)moduleBuilder.vtablefixups.Count * 8; }
		}

		internal uint DebugDirectoryRVA
		{
			get { return VTableFixupsRVA + VTableFixupsLength; }
		}

		internal uint DebugDirectoryLength
		{
			get
			{
				if (DebugDirectoryContentsLength != 0)
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
					IMAGE_DEBUG_DIRECTORY idd = new IMAGE_DEBUG_DIRECTORY();
					return (uint)SymbolSupport.GetDebugInfo(moduleBuilder.symbolWriter, ref idd).Length;
				}
				return 0;
			}
		}

		internal uint ExportDirectoryRVA
		{
			get { return (DebugDirectoryRVA + DebugDirectoryLength + DebugDirectoryContentsLength + 15) & ~15U; }
		}

		internal uint ExportDirectoryLength
		{
			get { return moduleBuilder.unmanagedExports.Count == 0 ? 0U : 40U; }
		}

		private uint ExportTablesRVA
		{
			get { return ExportDirectoryRVA + ExportDirectoryLength; }
		}

		private uint ExportTablesLength
		{
			get { return exportTables == null ? 0U : exportTables.Length; }
		}

		internal uint ImportDirectoryRVA
		{
			// on AMD64 (and probably IA64) the import directory needs to be 16 byte aligned (on I386 4 byte alignment is sufficient)
			get { return (ExportTablesRVA + ExportTablesLength + 15) & ~15U; }
		}

		internal uint ImportDirectoryLength
		{
			get { return (ImportHintNameTableRVA - ImportDirectoryRVA) + 27; }
		}

		private uint ImportHintNameTableRVA
		{
			get
			{
				return peWriter.Is32Bit
					? (ImportDirectoryRVA + 48 + 15) & ~15U
					: (ImportDirectoryRVA + 52 + 15) & ~15U;
			}
		}

		internal uint StartupStubRVA
		{
			get
			{
				if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_IA64)
				{
					// note that the alignment is driven by the requirement that the two relocation fixups are in a single page
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
				switch (peWriter.Headers.FileHeader.Machine)
				{
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386:
						return 6;
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64:
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM:
						return 12;
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_IA64:
						return 48;
					default:
						throw new NotSupportedException();
				}
			}
		}

		private void WriteRVA(MetadataWriter mw, uint rva)
		{
			switch (peWriter.Headers.FileHeader.Machine)
			{
				case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386:
				case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM:
					mw.Write(rva);
					break;
				default:
					mw.Write((ulong)rva);
					break;
			}
		}

		internal void Write(MetadataWriter mw, uint sdataRVA, out uint guidHeapOffset)
		{
			// Now that we're ready to start writing, we need to do some fix ups
			moduleBuilder.TypeRef.Fixup(moduleBuilder);
			moduleBuilder.MethodDef.Fixup(this);
			moduleBuilder.MethodImpl.Fixup(moduleBuilder);
			moduleBuilder.MethodSemantics.Fixup(moduleBuilder);
			moduleBuilder.InterfaceImpl.Fixup();
			moduleBuilder.ResolveInterfaceImplPseudoTokens();
			moduleBuilder.MemberRef.Fixup(moduleBuilder);
			moduleBuilder.Constant.Fixup(moduleBuilder);
			moduleBuilder.FieldMarshal.Fixup(moduleBuilder);
			moduleBuilder.DeclSecurity.Fixup(moduleBuilder);
			moduleBuilder.GenericParam.Fixup(moduleBuilder);
			moduleBuilder.CustomAttribute.Fixup(moduleBuilder);
			moduleBuilder.FieldLayout.Fixup(moduleBuilder);
			moduleBuilder.FieldRVA.Fixup(moduleBuilder, (int)sdataRVA, (int)this.MethodBodiesRVA);
			moduleBuilder.ImplMap.Fixup(moduleBuilder);
			moduleBuilder.ExportedType.Fixup(moduleBuilder);
			moduleBuilder.ManifestResource.Fixup(moduleBuilder);
			moduleBuilder.MethodSpec.Fixup(moduleBuilder);
			moduleBuilder.GenericParamConstraint.Fixup(moduleBuilder);

			// Import Address Table
			AssertRVA(mw, ImportAddressTableRVA);
			if (ImportAddressTableLength != 0)
			{
				WriteRVA(mw, ImportHintNameTableRVA);
				WriteRVA(mw, 0);
			}

			// CLI Header
			AssertRVA(mw, ComDescriptorRVA);
			cliHeader.MetaData.VirtualAddress = MetadataRVA;
			cliHeader.MetaData.Size = MetadataLength;
			if (ResourcesLength != 0)
			{
				cliHeader.Resources.VirtualAddress = ResourcesRVA;
				cliHeader.Resources.Size = ResourcesLength;
			}
			if (StrongNameSignatureLength != 0)
			{
				cliHeader.StrongNameSignature.VirtualAddress = StrongNameSignatureRVA;
				cliHeader.StrongNameSignature.Size = StrongNameSignatureLength;
			}
			if (VTableFixupsLength != 0)
			{
				cliHeader.VTableFixups.VirtualAddress = VTableFixupsRVA;
				cliHeader.VTableFixups.Size = VTableFixupsLength;
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
			moduleBuilder.WriteResources(mw);

			// The strong name signature live here (if it exists), but it will written later
			// and the following alignment padding will take care of reserving the space.

			// alignment padding
			for (int i = (int)(MetadataRVA - (ResourcesRVA + ResourcesLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Metadata
			AssertRVA(mw, MetadataRVA);
			moduleBuilder.WriteMetadata(mw, out guidHeapOffset);

			// alignment padding
			for (int i = (int)(VTableFixupsRVA - (MetadataRVA + MetadataLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// VTableFixups
			AssertRVA(mw, VTableFixupsRVA);
			WriteVTableFixups(mw, sdataRVA);

			// Debug Directory
			AssertRVA(mw, DebugDirectoryRVA);
			WriteDebugDirectory(mw);

			// alignment padding
			for (int i = (int)(ExportDirectoryRVA - (DebugDirectoryRVA + DebugDirectoryLength + DebugDirectoryContentsLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Export Directory
			AssertRVA(mw, ExportDirectoryRVA);
			WriteExportDirectory(mw);

			// Export Tables
			AssertRVA(mw, ExportTablesRVA);
			WriteExportTables(mw, sdataRVA);
	
			// alignment padding
			for (int i = (int)(ImportDirectoryRVA - (ExportTablesRVA + ExportTablesLength)); i > 0; i--)
			{
				mw.Write((byte)0);
			}

			// Import Directory
			AssertRVA(mw, ImportDirectoryRVA);
			if (ImportDirectoryLength != 0)
			{
				WriteImportDirectory(mw);
			}

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
			else if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386)
			{
				mw.Write((ushort)0x25FF);
				mw.Write((uint)peWriter.Headers.OptionalHeader.ImageBase + ImportAddressTableRVA);
			}
			else if (peWriter.Headers.FileHeader.Machine == IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM)
			{
				uint rva = (uint)peWriter.Headers.OptionalHeader.ImageBase + ImportAddressTableRVA;
				ushort lo = (ushort)rva;
				ushort hi = (ushort)(rva >> 16);
				mw.Write((ushort)(0xF240 + (lo >> 12)));
				mw.Write((ushort)(0x0C00 + ((lo << 4) & 0xF000) + (lo & 0xFF)));
				mw.Write((ushort)(0xF2C0 + (hi >> 12)));
				mw.Write((ushort)(0x0C00 + ((hi << 4) & 0xF000) + (hi & 0xFF)));
				mw.Write((ushort)0xF8DC);
				mw.Write((ushort)0xF000);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		[Conditional("DEBUG")]
		private void AssertRVA(MetadataWriter mw, uint rva)
		{
			Debug.Assert(mw.Position - PointerToRawData + BaseRVA == rva);
		}

		private void WriteVTableFixups(MetadataWriter mw, uint sdataRVA)
		{
			foreach (ModuleBuilder.VTableFixups fixups in moduleBuilder.vtablefixups)
			{
				mw.Write(fixups.initializedDataOffset + sdataRVA);
				mw.Write(fixups.count);
				mw.Write(fixups.type);
			}
		}

		private void WriteDebugDirectory(MetadataWriter mw)
		{
			if (DebugDirectoryLength != 0)
			{
				IMAGE_DEBUG_DIRECTORY idd = new IMAGE_DEBUG_DIRECTORY();
				idd.Characteristics = 0;
				idd.TimeDateStamp = peWriter.Headers.FileHeader.TimeDateStamp;
				byte[] buf = SymbolSupport.GetDebugInfo(moduleBuilder.symbolWriter, ref idd);
				idd.PointerToRawData = (DebugDirectoryRVA - BaseRVA) + DebugDirectoryLength + PointerToRawData;
				idd.AddressOfRawData = DebugDirectoryRVA + DebugDirectoryLength;
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

		private sealed class ExportTables
		{
			private readonly TextSection text;
			internal readonly uint entries;
			internal readonly uint ordinalBase;
			internal readonly uint nameCount;
			internal readonly uint namesLength;
			internal readonly uint exportAddressTableRVA;
			internal readonly uint exportNamePointerTableRVA;
			internal readonly uint exportOrdinalTableRVA;
			internal readonly uint namesRVA;
			internal readonly uint stubsRVA;
			private readonly uint stubLength;

			internal ExportTables(TextSection text)
			{
				this.text = text;
				ordinalBase = GetOrdinalBase(out entries);
				namesLength = GetExportNamesLength(out nameCount);
				exportAddressTableRVA = text.ExportTablesRVA;
				exportNamePointerTableRVA = exportAddressTableRVA + 4 * entries;
				exportOrdinalTableRVA = exportNamePointerTableRVA + 4 * nameCount;
				namesRVA = exportOrdinalTableRVA + 2 * nameCount;
				stubsRVA = (namesRVA + namesLength + 15) & ~15U;
				// note that we align the stubs to avoid having to deal with the relocation crossing a page boundary
				switch (text.peWriter.Headers.FileHeader.Machine)
				{
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386:
						stubLength = 8;
						break;
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64:
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM:
						stubLength = 16;
						break;
					default:
						throw new NotSupportedException();
				}
			}

			internal uint Length
			{
				get { return (stubsRVA + stubLength * (uint)text.moduleBuilder.unmanagedExports.Count) - text.ExportTablesRVA; }
			}

			private uint GetOrdinalBase(out uint entries)
			{
				uint min = uint.MaxValue;
				uint max = uint.MinValue;
				foreach (UnmanagedExport exp in text.moduleBuilder.unmanagedExports)
				{
					uint ordinal = (uint)exp.ordinal;
					min = Math.Min(min, ordinal);
					max = Math.Max(max, ordinal);
				}
				entries = 1 + (max - min);
				return min;
			}

			private uint GetExportNamesLength(out uint nameCount)
			{
				nameCount = 0;
				// the first name in the names list is the module name (the Export Directory contains a name of the current module)
				uint length = (uint)text.moduleBuilder.fileName.Length + 1;
				foreach (UnmanagedExport exp in text.moduleBuilder.unmanagedExports)
				{
					if (exp.name != null)
					{
						nameCount++;
						length += (uint)exp.name.Length + 1;
					}
				}
				return length;
			}

			internal void Write(MetadataWriter mw, uint sdataRVA)
			{
				// sort the exports by ordinal
				text.moduleBuilder.unmanagedExports.Sort(CompareUnmanagedExportOrdinals);

				// Now write the Export Address Table
				text.AssertRVA(mw, exportAddressTableRVA);
				for (int i = 0, pos = 0; i < entries; i++)
				{
					if (text.moduleBuilder.unmanagedExports[pos].ordinal == i + ordinalBase)
					{
						mw.Write(text.peWriter.Thumb + stubsRVA + (uint)pos * stubLength);
						pos++;
					}
					else
					{
						mw.Write(0);
					}
				}

				// sort the exports by name
				text.moduleBuilder.unmanagedExports.Sort(CompareUnmanagedExportNames);

				// Now write the Export Name Pointer Table
				text.AssertRVA(mw, exportNamePointerTableRVA);
				uint nameOffset = (uint)text.moduleBuilder.fileName.Length + 1;
				foreach (UnmanagedExport exp in text.moduleBuilder.unmanagedExports)
				{
					if (exp.name != null)
					{
						mw.Write(namesRVA + nameOffset);
						nameOffset += (uint)exp.name.Length + 1;
					}
				}

				// Now write the Export Ordinal Table
				text.AssertRVA(mw, exportOrdinalTableRVA);
				foreach (UnmanagedExport exp in text.moduleBuilder.unmanagedExports)
				{
					if (exp.name != null)
					{
						mw.Write((ushort)(exp.ordinal - ordinalBase));
					}
				}

				// Now write the actual names
				text.AssertRVA(mw, namesRVA);
				mw.WriteAsciiz(text.moduleBuilder.fileName);
				foreach (UnmanagedExport exp in text.moduleBuilder.unmanagedExports)
				{
					if (exp.name != null)
					{
						mw.WriteAsciiz(exp.name);
					}
				}
				text.AssertRVA(mw, namesRVA + namesLength);

				// alignment padding
				for (int i = (int)(stubsRVA - (namesRVA + namesLength)); i > 0; i--)
				{
					mw.Write((byte)0);
				}

				// sort the exports by ordinal
				text.moduleBuilder.unmanagedExports.Sort(CompareUnmanagedExportOrdinals);

				// Now write the stubs
				text.AssertRVA(mw, stubsRVA);

				for (int i = 0, pos = 0; i < entries; i++)
				{
					if (text.moduleBuilder.unmanagedExports[pos].ordinal == i + ordinalBase)
					{
						switch (text.peWriter.Headers.FileHeader.Machine)
						{
							case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386:
								mw.Write((byte)0xFF);
								mw.Write((byte)0x25);
								mw.Write((uint)text.peWriter.Headers.OptionalHeader.ImageBase + text.moduleBuilder.unmanagedExports[pos].rva.initializedDataOffset + sdataRVA);
								mw.Write((short)0);	// alignment
								break;
							case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64:
								mw.Write((byte)0x48);
								mw.Write((byte)0xA1);
								mw.Write(text.peWriter.Headers.OptionalHeader.ImageBase + text.moduleBuilder.unmanagedExports[pos].rva.initializedDataOffset + sdataRVA);
								mw.Write((byte)0xFF);
								mw.Write((byte)0xE0);
								mw.Write(0); // alignment
								break;
							case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM:
								mw.Write((ushort)0xF8DF);
								mw.Write((ushort)0xC008);
								mw.Write((ushort)0xF8DC);
								mw.Write((ushort)0xC000);
								mw.Write((ushort)0x4760);
								mw.Write((ushort)0xDEFE);
								mw.Write((uint)text.peWriter.Headers.OptionalHeader.ImageBase + text.moduleBuilder.unmanagedExports[pos].rva.initializedDataOffset + sdataRVA);
								break;
							default:
								throw new NotSupportedException();
						}
						pos++;
					}
				}
			}

			private static int CompareUnmanagedExportNames(UnmanagedExport x, UnmanagedExport y)
			{
				if (x.name == null)
				{
					return y.name == null ? 0 : 1;
				}
				if (y.name == null)
				{
					return -1;
				}
				return String.CompareOrdinal(x.name, y.name);
			}

			private static int CompareUnmanagedExportOrdinals(UnmanagedExport x, UnmanagedExport y)
			{
				return x.ordinal.CompareTo(y.ordinal);
			}

			internal void GetRelocations(List<Relocation> list)
			{
				ushort type;
				uint rva;
				switch (text.peWriter.Headers.FileHeader.Machine)
				{
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386:
						type = 0x3000;
						rva = stubsRVA + 2;
						break;
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64:
						type = 0xA000;
						rva = stubsRVA + 2;
						break;
					case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM:
						type = 0x3000;
						rva = stubsRVA + 12;
						break;
					default:
						throw new NotSupportedException();
				}

				// we assume that unmanagedExports is still sorted by ordinal
				for (int i = 0, pos = 0; i < entries; i++)
				{
					if (text.moduleBuilder.unmanagedExports[pos].ordinal == i + ordinalBase)
					{
						list.Add(new Relocation(type, rva + (uint)pos * stubLength));
						pos++;
					}
				}
			}
		}

		private uint GetOrdinalBase(out uint entries)
		{
			uint min = uint.MaxValue;
			uint max = uint.MinValue;
			foreach (UnmanagedExport exp in moduleBuilder.unmanagedExports)
			{
				uint ordinal = (uint)exp.ordinal;
				min = Math.Min(min, ordinal);
				max = Math.Max(max, ordinal);
			}
			entries = 1 + (max - min);
			return min;
		}

		private uint GetExportNamesLength(out uint nameCount)
		{
			nameCount = 0;
			uint length = 0;
			foreach (UnmanagedExport exp in moduleBuilder.unmanagedExports)
			{
				if (exp.name != null)
				{
					nameCount++;
					length += (uint)exp.name.Length + 1;
				}
			}
			return length;
		}

		private void WriteExportDirectory(MetadataWriter mw)
		{
			if (ExportDirectoryLength != 0)
			{
				// Flags
				mw.Write(0);
				// Date/Time Stamp
				mw.Write(peWriter.Headers.FileHeader.TimeDateStamp);
				// Major Version
				mw.Write((short)0);
				// Minor Version
				mw.Write((short)0);
				// Name RVA
				mw.Write(exportTables.namesRVA);
				// Ordinal Base
				mw.Write(exportTables.ordinalBase);
				// Address Table Entries
				mw.Write(exportTables.entries);
				// Number of Name Pointers
				mw.Write(exportTables.nameCount);
				// Export Address Table RVA
				mw.Write(exportTables.exportAddressTableRVA);
				// Name Pointer RVA
				mw.Write(exportTables.exportNamePointerTableRVA);
				// Ordinal Table RVA
				mw.Write(exportTables.exportOrdinalTableRVA);
			}
		}

		private void WriteExportTables(MetadataWriter mw, uint sdataRVA)
		{
			if (exportTables != null)
			{
				exportTables.Write(mw, sdataRVA);
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
			if (!peWriter.Is32Bit)
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
				mw.WriteAsciiz("_CorDllMain");
			}
			else
			{
				mw.WriteAsciiz("_CorExeMain");
			}
			// Name
			mw.WriteAsciiz("mscoree.dll");
			mw.Write((byte)0);
		}

		internal int Length
		{
			get { return (int)(StartupStubRVA - BaseRVA + StartupStubLength); }
		}

		struct Relocation : IComparable<Relocation>
		{
			internal readonly uint rva;
			internal readonly ushort type;

			internal Relocation(ushort type, uint rva)
			{
				this.type = type;
				this.rva = rva;
			}

			int IComparable<Relocation>.CompareTo(Relocation other)
			{
				return rva.CompareTo(other.rva);
			}
		}

		struct RelocationBlock
		{
			internal readonly uint PageRVA;
			internal readonly ushort[] TypeOffset;

			internal RelocationBlock(uint pageRva, ushort[] typeOffset)
			{
				this.PageRVA = pageRva;
				this.TypeOffset = typeOffset;
			}
		}

		internal void WriteRelocations(MetadataWriter mw)
		{
			foreach (RelocationBlock block in relocations)
			{
				mw.Write(block.PageRVA);
				mw.Write(8 + block.TypeOffset.Length * 2);
				foreach (ushort typeOffset in block.TypeOffset)
				{
					mw.Write(typeOffset);
				}
			}
		}

		internal uint PackRelocations()
		{
			List<Relocation> list = new List<Relocation>();
			switch (peWriter.Headers.FileHeader.Machine)
			{
				case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386:
					list.Add(new Relocation(0x3000, this.StartupStubRVA + 2));
					break;
				case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64:
					list.Add(new Relocation(0xA000, this.StartupStubRVA + 2));
					break;
				case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_IA64:
					list.Add(new Relocation(0xA000, this.StartupStubRVA + 0x20));
					list.Add(new Relocation(0xA000, this.StartupStubRVA + 0x28));
					break;
				case IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM:
					list.Add(new Relocation(0x7000, this.StartupStubRVA));
					break;
				default:
					throw new NotSupportedException();
			}
			if (exportTables != null)
			{
				exportTables.GetRelocations(list);
			}
			list.Sort();
			uint size = 0;
			for (int i = 0; i < list.Count; )
			{
				uint pageRVA = list[i].rva & ~0xFFFU;
				int count = 1;
				while (i + count < list.Count && (list[i + count].rva & ~0xFFFU) == pageRVA)
				{
					count++;
				}
				ushort[] typeOffset = new ushort[(count + 1) & ~1];
				for (int j = 0; j < count; j++, i++)
				{
					typeOffset[j] = (ushort)(list[i].type + (list[i].rva - pageRVA));
				}
				relocations.Add(new RelocationBlock(pageRVA, typeOffset));
				size += (uint)(8 + typeOffset.Length * 2);
			}
			return size;
		}
	}
}
