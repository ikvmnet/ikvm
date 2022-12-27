/*
  Copyright (C) 2008-2015 Jeroen Frijters

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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using IKVM.Reflection.Emit;
using IKVM.Reflection.Impl;
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Writer
{
	static class ModuleWriter
	{
		internal static void WriteModule(StrongNameKeyPair keyPair, byte[] publicKey, ModuleBuilder moduleBuilder,
			PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine,
			ResourceSection resources, int entryPointToken)
		{
			WriteModule(keyPair, publicKey, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, resources, entryPointToken, null);
		}

		internal static void WriteModule(StrongNameKeyPair keyPair, byte[] publicKey, ModuleBuilder moduleBuilder,
			PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine,
			ResourceSection resources, int entryPointToken, Stream stream)
		{
			if (stream == null)
			{
				string fileName = moduleBuilder.FullyQualifiedName;
				bool mono = System.Type.GetType("Mono.Runtime") != null;
				if (mono)
				{
					try
					{
						// Mono mmaps the file, so unlink the previous version since it may be in use
						File.Delete(fileName);
					}
					catch { }
				}
				using (FileStream fs = new FileStream(fileName, FileMode.Create))
				{
					WriteModuleImpl(keyPair, publicKey, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, resources, entryPointToken, fs);
				}
				// if we're running on Mono, mark the module as executable by using a Mono private API extension
				if (mono)
				{
					File.SetAttributes(fileName, (FileAttributes)(unchecked((int)0x80000000)));
				}
			}
			else
			{
				WriteModuleImpl(keyPair, publicKey, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, resources, entryPointToken, stream);
			}
		}

		private static void WriteModuleImpl(StrongNameKeyPair keyPair, byte[] publicKey, ModuleBuilder moduleBuilder,
			PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine,
			ResourceSection resources, int entryPointToken, Stream stream)
		{
			moduleBuilder.ApplyUnmanagedExports(imageFileMachine);
			moduleBuilder.FixupMethodBodyTokens();

			int moduleVersionIdIndex = moduleBuilder.Guids.Add(moduleBuilder.GetModuleVersionIdOrEmpty());
			moduleBuilder.ModuleTable.Add(0, moduleBuilder.Strings.Add(moduleBuilder.moduleName), moduleVersionIdIndex, 0, 0);

			if (moduleBuilder.UserStrings.IsEmpty)
			{
				// for compat with Ref.Emit, if there aren't any user strings, we add one
				moduleBuilder.UserStrings.Add(" ");
			}

			if (resources != null)
			{
				resources.Finish();
			}

			PEWriter writer = new PEWriter(stream);
			writer.Headers.OptionalHeader.FileAlignment = (uint)moduleBuilder.__FileAlignment;
			switch (imageFileMachine)
			{
				case ImageFileMachine.I386:
					writer.Headers.FileHeader.Machine = IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386;
					writer.Headers.FileHeader.Characteristics |= IMAGE_FILE_HEADER.IMAGE_FILE_32BIT_MACHINE;
					writer.Headers.OptionalHeader.SizeOfStackReserve = moduleBuilder.GetStackReserve(0x100000);
					break;
				case ImageFileMachine.ARM:
					writer.Headers.FileHeader.Machine = IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM;
					writer.Headers.FileHeader.Characteristics |= IMAGE_FILE_HEADER.IMAGE_FILE_32BIT_MACHINE | IMAGE_FILE_HEADER.IMAGE_FILE_LARGE_ADDRESS_AWARE;
					writer.Headers.OptionalHeader.SizeOfStackReserve = moduleBuilder.GetStackReserve(0x100000);
					writer.Headers.OptionalHeader.SectionAlignment = 0x1000;
					break;
				case ImageFileMachine.AMD64:
					writer.Headers.FileHeader.Machine = IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_AMD64;
					writer.Headers.FileHeader.Characteristics |= IMAGE_FILE_HEADER.IMAGE_FILE_LARGE_ADDRESS_AWARE;
					writer.Headers.FileHeader.SizeOfOptionalHeader = 0xF0;
					writer.Headers.OptionalHeader.Magic = IMAGE_OPTIONAL_HEADER.IMAGE_NT_OPTIONAL_HDR64_MAGIC;
					writer.Headers.OptionalHeader.SizeOfStackReserve = moduleBuilder.GetStackReserve(0x400000);
					writer.Headers.OptionalHeader.SizeOfStackCommit = 0x4000;
					writer.Headers.OptionalHeader.SizeOfHeapCommit = 0x2000;
					break;
				case ImageFileMachine.IA64:
					writer.Headers.FileHeader.Machine = IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_IA64;
					writer.Headers.FileHeader.Characteristics |= IMAGE_FILE_HEADER.IMAGE_FILE_LARGE_ADDRESS_AWARE;
					writer.Headers.FileHeader.SizeOfOptionalHeader = 0xF0;
					writer.Headers.OptionalHeader.Magic = IMAGE_OPTIONAL_HEADER.IMAGE_NT_OPTIONAL_HDR64_MAGIC;
					writer.Headers.OptionalHeader.SizeOfStackReserve = moduleBuilder.GetStackReserve(0x400000);
					writer.Headers.OptionalHeader.SizeOfStackCommit = 0x4000;
					writer.Headers.OptionalHeader.SizeOfHeapCommit = 0x2000;
					break;
				case ImageFileMachine.ARM64:
					writer.Headers.FileHeader.Machine = IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_ARM64;
                    writer.Headers.FileHeader.Characteristics |= IMAGE_FILE_HEADER.IMAGE_FILE_LARGE_ADDRESS_AWARE;
                    writer.Headers.FileHeader.SizeOfOptionalHeader = 0xF0;
                    writer.Headers.OptionalHeader.Magic = IMAGE_OPTIONAL_HEADER.IMAGE_NT_OPTIONAL_HDR64_MAGIC;
                    writer.Headers.OptionalHeader.SizeOfStackReserve = moduleBuilder.GetStackReserve(0x400000);
                    writer.Headers.OptionalHeader.SizeOfStackCommit = 0x4000;
                    writer.Headers.OptionalHeader.SizeOfHeapCommit = 0x2000;
                    break;
                default:
					throw new ArgumentOutOfRangeException("imageFileMachine");
			}
			if (fileKind == PEFileKinds.Dll)
			{
				writer.Headers.FileHeader.Characteristics |= IMAGE_FILE_HEADER.IMAGE_FILE_DLL;
			}

			switch (fileKind)
			{
				case PEFileKinds.WindowApplication:
					writer.Headers.OptionalHeader.Subsystem = IMAGE_OPTIONAL_HEADER.IMAGE_SUBSYSTEM_WINDOWS_GUI;
					break;
				default:
					writer.Headers.OptionalHeader.Subsystem = IMAGE_OPTIONAL_HEADER.IMAGE_SUBSYSTEM_WINDOWS_CUI;
					break;
			}
			writer.Headers.OptionalHeader.DllCharacteristics = (ushort)moduleBuilder.__DllCharacteristics;

			CliHeader cliHeader = new CliHeader();
			cliHeader.Cb = 0x48;
			cliHeader.MajorRuntimeVersion = 2;
			cliHeader.MinorRuntimeVersion = moduleBuilder.MDStreamVersion < 0x20000 ? (ushort)0 : (ushort)5;
			if ((portableExecutableKind & PortableExecutableKinds.ILOnly) != 0)
			{
				cliHeader.Flags |= CliHeader.COMIMAGE_FLAGS_ILONLY;
			}
			if ((portableExecutableKind & PortableExecutableKinds.Required32Bit) != 0)
			{
				cliHeader.Flags |= CliHeader.COMIMAGE_FLAGS_32BITREQUIRED;
			}
			if ((portableExecutableKind & PortableExecutableKinds.Preferred32Bit) != 0)
			{
				cliHeader.Flags |= CliHeader.COMIMAGE_FLAGS_32BITREQUIRED | CliHeader.COMIMAGE_FLAGS_32BITPREFERRED;
			}
			if (keyPair != null)
			{
				cliHeader.Flags |= CliHeader.COMIMAGE_FLAGS_STRONGNAMESIGNED;
			}
			if (ModuleBuilder.IsPseudoToken(entryPointToken))
			{
				entryPointToken = moduleBuilder.ResolvePseudoToken(entryPointToken);
			}
			cliHeader.EntryPointToken = (uint)entryPointToken;

			moduleBuilder.Strings.Freeze();
			moduleBuilder.UserStrings.Freeze();
			moduleBuilder.Guids.Freeze();
			moduleBuilder.Blobs.Freeze();
			MetadataWriter mw = new MetadataWriter(moduleBuilder, stream);
			moduleBuilder.Tables.Freeze(mw);
			TextSection code = new TextSection(writer, cliHeader, moduleBuilder, ComputeStrongNameSignatureLength(publicKey));

			// Export Directory
			if (code.ExportDirectoryLength != 0)
			{
				writer.Headers.OptionalHeader.DataDirectory[0].VirtualAddress = code.ExportDirectoryRVA;
				writer.Headers.OptionalHeader.DataDirectory[0].Size = code.ExportDirectoryLength;
			}

			// Import Directory
			if (code.ImportDirectoryLength != 0)
			{
				writer.Headers.OptionalHeader.DataDirectory[1].VirtualAddress = code.ImportDirectoryRVA;
				writer.Headers.OptionalHeader.DataDirectory[1].Size = code.ImportDirectoryLength;
			}

			// Import Address Table Directory
			if (code.ImportAddressTableLength != 0)
			{
				writer.Headers.OptionalHeader.DataDirectory[12].VirtualAddress = code.ImportAddressTableRVA;
				writer.Headers.OptionalHeader.DataDirectory[12].Size = code.ImportAddressTableLength;
			}

			// COM Descriptor Directory
			writer.Headers.OptionalHeader.DataDirectory[14].VirtualAddress = code.ComDescriptorRVA;
			writer.Headers.OptionalHeader.DataDirectory[14].Size = code.ComDescriptorLength;

			// Debug Directory
			if (code.DebugDirectoryLength != 0)
			{
				writer.Headers.OptionalHeader.DataDirectory[6].VirtualAddress = code.DebugDirectoryRVA;
				writer.Headers.OptionalHeader.DataDirectory[6].Size = code.DebugDirectoryLength;
			}

			// Set the PE File timestamp
			writer.Headers.FileHeader.TimeDateStamp = moduleBuilder.GetTimeDateStamp();

			// we need to start by computing the number of sections, because code.PointerToRawData depends on that
			writer.Headers.FileHeader.NumberOfSections = 2;

			if (moduleBuilder.initializedData.Length != 0)
			{
				// .sdata
				writer.Headers.FileHeader.NumberOfSections++;
			}

			if (resources != null)
			{
				// .rsrc
				writer.Headers.FileHeader.NumberOfSections++;
			}

			SectionHeader text = new SectionHeader();
			text.Name = ".text";
			text.VirtualAddress = code.BaseRVA;
			text.VirtualSize = (uint)code.Length;
			text.PointerToRawData = code.PointerToRawData;
			text.SizeOfRawData = writer.ToFileAlignment((uint)code.Length);
			text.Characteristics = SectionHeader.IMAGE_SCN_CNT_CODE | SectionHeader.IMAGE_SCN_MEM_EXECUTE | SectionHeader.IMAGE_SCN_MEM_READ;

			SectionHeader sdata = new SectionHeader();
			sdata.Name = ".sdata";
			sdata.VirtualAddress = text.VirtualAddress + writer.ToSectionAlignment(text.VirtualSize);
			sdata.VirtualSize = (uint)moduleBuilder.initializedData.Length;
			sdata.PointerToRawData = text.PointerToRawData + text.SizeOfRawData;
			sdata.SizeOfRawData = writer.ToFileAlignment((uint)moduleBuilder.initializedData.Length);
			sdata.Characteristics = SectionHeader.IMAGE_SCN_CNT_INITIALIZED_DATA | SectionHeader.IMAGE_SCN_MEM_READ | SectionHeader.IMAGE_SCN_MEM_WRITE;

			SectionHeader rsrc = new SectionHeader();
			rsrc.Name = ".rsrc";
			rsrc.VirtualAddress = sdata.VirtualAddress + writer.ToSectionAlignment(sdata.VirtualSize);
			rsrc.PointerToRawData = sdata.PointerToRawData + sdata.SizeOfRawData;
			rsrc.VirtualSize = resources == null ? 0 : (uint)resources.Length;
			rsrc.SizeOfRawData = writer.ToFileAlignment(rsrc.VirtualSize);
			rsrc.Characteristics = SectionHeader.IMAGE_SCN_MEM_READ | SectionHeader.IMAGE_SCN_CNT_INITIALIZED_DATA;

			if (rsrc.SizeOfRawData != 0)
			{
				// Resource Directory
				writer.Headers.OptionalHeader.DataDirectory[2].VirtualAddress = rsrc.VirtualAddress;
				writer.Headers.OptionalHeader.DataDirectory[2].Size = rsrc.VirtualSize;
			}

			SectionHeader reloc = new SectionHeader();
			reloc.Name = ".reloc";
			reloc.VirtualAddress = rsrc.VirtualAddress + writer.ToSectionAlignment(rsrc.VirtualSize);
			reloc.VirtualSize = code.PackRelocations();
			reloc.PointerToRawData = rsrc.PointerToRawData + rsrc.SizeOfRawData;
			reloc.SizeOfRawData = writer.ToFileAlignment(reloc.VirtualSize);
			reloc.Characteristics = SectionHeader.IMAGE_SCN_MEM_READ | SectionHeader.IMAGE_SCN_CNT_INITIALIZED_DATA | SectionHeader.IMAGE_SCN_MEM_DISCARDABLE;

			if (reloc.SizeOfRawData != 0)
			{
				// Base Relocation Directory
				writer.Headers.OptionalHeader.DataDirectory[5].VirtualAddress = reloc.VirtualAddress;
				writer.Headers.OptionalHeader.DataDirectory[5].Size = reloc.VirtualSize;
			}

			writer.Headers.OptionalHeader.SizeOfCode = text.SizeOfRawData;
			writer.Headers.OptionalHeader.SizeOfInitializedData = sdata.SizeOfRawData + rsrc.SizeOfRawData + reloc.SizeOfRawData;
			writer.Headers.OptionalHeader.SizeOfUninitializedData = 0;
			writer.Headers.OptionalHeader.SizeOfImage = reloc.VirtualAddress + writer.ToSectionAlignment(reloc.VirtualSize);
			writer.Headers.OptionalHeader.SizeOfHeaders = text.PointerToRawData;
			writer.Headers.OptionalHeader.BaseOfCode = code.BaseRVA;
			writer.Headers.OptionalHeader.BaseOfData = sdata.VirtualAddress;
			writer.Headers.OptionalHeader.ImageBase = (ulong)moduleBuilder.__ImageBase;

			if (imageFileMachine == ImageFileMachine.IA64)
			{
				// apparently for IA64 AddressOfEntryPoint points to the address of the entry point
				// (i.e. there is an additional layer of indirection), so we add the offset to the pointer
				writer.Headers.OptionalHeader.AddressOfEntryPoint = code.StartupStubRVA + 0x20;
			}
			else
			{
				writer.Headers.OptionalHeader.AddressOfEntryPoint = code.StartupStubRVA + writer.Thumb;
			}

			writer.WritePEHeaders();
			writer.WriteSectionHeader(text);
			if (sdata.SizeOfRawData != 0)
			{
				writer.WriteSectionHeader(sdata);
			}
			if (rsrc.SizeOfRawData != 0)
			{
				writer.WriteSectionHeader(rsrc);
			}
			if (reloc.SizeOfRawData != 0)
			{
				writer.WriteSectionHeader(reloc);
			}

			stream.Seek(text.PointerToRawData, SeekOrigin.Begin);
			uint guidHeapOffset;
			code.Write(mw, sdata.VirtualAddress, out guidHeapOffset);

			if (sdata.SizeOfRawData != 0)
			{
				stream.Seek(sdata.PointerToRawData, SeekOrigin.Begin);
				mw.Write(moduleBuilder.initializedData);
			}

			if (rsrc.SizeOfRawData != 0)
			{
				stream.Seek(rsrc.PointerToRawData, SeekOrigin.Begin);
				resources.Write(mw, rsrc.VirtualAddress);
			}

			if (reloc.SizeOfRawData != 0)
			{
				stream.Seek(reloc.PointerToRawData, SeekOrigin.Begin);
				code.WriteRelocations(mw);
			}

			// file alignment
			stream.SetLength(reloc.PointerToRawData + reloc.SizeOfRawData);

			// if we don't have a guid, generate one based on the contents of the assembly
			if (moduleBuilder.universe.Deterministic && moduleBuilder.GetModuleVersionIdOrEmpty() == Guid.Empty)
			{
				Guid guid = GenerateModuleVersionId(stream);
				stream.Position = guidHeapOffset + (moduleVersionIdIndex - 1) * 16;
				stream.Write(guid.ToByteArray(), 0, 16);
				moduleBuilder.__SetModuleVersionId(guid);
			}

			// do the strong naming
			if (keyPair != null)
			{
				StrongName(stream, keyPair, writer.HeaderSize, text.PointerToRawData, code.StrongNameSignatureRVA - text.VirtualAddress + text.PointerToRawData, code.StrongNameSignatureLength);
			}

			if (moduleBuilder.symbolWriter != null)
			{
				moduleBuilder.WriteSymbolTokenMap();
				moduleBuilder.symbolWriter.Close();
			}
		}

		private static int ComputeStrongNameSignatureLength(byte[] publicKey)
		{
			if (publicKey == null)
			{
				return 0;
			}
			else if (publicKey.Length == 16)
			{
				// it must be the ECMA pseudo public key, we don't know the key size of the real key, but currently both Mono and Microsoft use a 1024 bit key size
				return 128;
			}
			else
			{
				// for the supported strong naming algorithms, the signature size is the same as the key size
				// (we have to subtract 32 for the header)
				return publicKey.Length - 32;
			}
		}

		private static void StrongName(Stream stream, StrongNameKeyPair keyPair, uint headerLength, uint textSectionFileOffset, uint strongNameSignatureFileOffset, uint strongNameSignatureLength)
		{
			byte[] hash;
			using (SHA1 sha1 = SHA1.Create())
			{
				stream.Seek(0, SeekOrigin.Begin);
				Stream skipStream = new SkipStream(stream, strongNameSignatureFileOffset, strongNameSignatureLength);
				skipStream = new SkipStream(skipStream, headerLength, textSectionFileOffset - headerLength);
				hash = sha1.ComputeHash(skipStream);
			}
			using (RSACryptoServiceProvider rsa = keyPair.CreateRSA())
			{
				byte[] signature = rsa.SignHash(hash, "1.3.14.3.2.26");
				Array.Reverse(signature);
				if (signature.Length != strongNameSignatureLength)
				{
					throw new InvalidOperationException("Signature length mismatch");
				}
				stream.Seek(strongNameSignatureFileOffset, SeekOrigin.Begin);
				stream.Write(signature, 0, signature.Length);
			}

			// compute the PE checksum
			stream.Seek(0, SeekOrigin.Begin);
			int count = (int)stream.Length / 4;
			BinaryReader br = new BinaryReader(stream);
			long sum = 0;
			for (int i = 0; i < count; i++)
			{
				sum += br.ReadUInt32();
				int carry = (int)(sum >> 32);
				sum &= 0xFFFFFFFFU;
				sum += carry;
			}
			while ((sum >> 16) != 0)
			{
				sum = (sum & 0xFFFF) + (sum >> 16);
			}
			sum += stream.Length;

			// write the PE checksum, note that it is always at offset 0xD8 in the file
			ByteBuffer bb = new ByteBuffer(4);
			bb.Write((int)sum);
			stream.Seek(0xD8, SeekOrigin.Begin);
			bb.WriteTo(stream);
		}

		private static Guid GenerateModuleVersionId(Stream stream)
		{
			byte[] hash;
			using (SHA1 sha1 = SHA1.Create())
			{
				stream.Seek(0, SeekOrigin.Begin);
				hash = sha1.ComputeHash(stream);
			}
			byte[] bytes = new byte[16];
			Buffer.BlockCopy(hash, 0, bytes, 0, bytes.Length);
			// set GUID type to "version 4" (random)
			bytes[7] &= 0x0F;
			bytes[7] |= 0x40;
			bytes[8] &= 0x3F;
			bytes[8] |= 0x80;
			return new Guid(bytes);
		}
	}

	sealed class SkipStream : Stream
	{
		private readonly Stream stream;
		private long skipOffset;
		private long skipLength;

		internal SkipStream(Stream stream, long skipOffset, long skipLength)
		{
			if (skipOffset < 0 || skipLength < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.stream = stream;
			this.skipOffset = skipOffset;
			this.skipLength = skipLength;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				stream.Dispose();
			}
		}

		public override bool CanRead
		{
			get { return stream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (skipLength != 0 && skipOffset < count)
			{
				if (skipOffset != 0)
				{
					count = (int)skipOffset;
				}
				else
				{
					// note that we loop forever if the skipped part lies beyond EOF
					while (skipLength != 0)
					{
						// use the output buffer as scratch space
						skipLength -= stream.Read(buffer, offset, (int)Math.Min(count, skipLength));
					}
				}
			}
			int totalBytesRead = stream.Read(buffer, offset, count);
			skipOffset -= totalBytesRead;
			return totalBytesRead;
		}

		public override long Length
		{
			get { throw new NotSupportedException(); }
		}

		public override long Position
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}
