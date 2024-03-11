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
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;

using IKVM.Reflection.Diagnostics;
using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Writer
{

    static class ModuleWriter
    {

        const ulong DefaultExeBaseAddress32Bit = 0x00400000;
        const ulong DefaultExeBaseAddress64Bit = 0x0000000140000000;

        const ulong DefaultDllBaseAddress32Bit = 0x10000000;
        const ulong DefaultDllBaseAddress64Bit = 0x0000000180000000;

        const ulong DefaultSizeOfHeapReserve32Bit = 0x00100000;
        const ulong DefaultSizeOfHeapReserve64Bit = 0x00400000;

        const ulong DefaultSizeOfHeapCommit32Bit = 0x1000;
        const ulong DefaultSizeOfHeapCommit64Bit = 0x2000;

        const ulong DefaultSizeOfStackReserve32Bit = 0x00100000;
        const ulong DefaultSizeOfStackReserve64Bit = 0x00400000;

        const ulong DefaultSizeOfStackCommit32Bit = 0x1000;
        const ulong DefaultSizeOfStackCommit64Bit = 0x4000;

        const ushort DefaultFileAlignment32Bit = 0x200;
        const ushort DefaultFileAlignment64Bit = 0x200; //both 32 and 64 bit binaries used this value in the native stack.

        const ushort DefaultSectionAlignment = 0x2000;

        /// <summary>
        /// Writes the module specified by <paramref name="module"/>.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="publicKey"></param>
        /// <param name="module"></param>
        /// <param name="fileKind"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        /// <param name="nativeResources"></param>
        /// <param name="entryPoint"></param>
        internal static void WriteModule(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ModuleResourceSectionBuilder nativeResources, MethodInfo entryPoint)
        {
            WriteModule(keyPair, publicKey, module, fileKind, portableExecutableKind, imageFileMachine, nativeResources, entryPoint, null, null, null, null);
        }

        /// <summary>
        /// Writes the module specified by <paramref name="module"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="publicKey"></param>
        /// <param name="module"></param>
        /// <param name="fileKind"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        /// <param name="nativeResources"></param>
        /// <param name="entryPoint"></param>
        /// <param name="peStream"></param>
        /// <param name="pdbStream"></param>
        internal static void WriteModule(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ModuleResourceSectionBuilder nativeResources, MethodInfo entryPoint, string peFileName, Stream peStream, string pdbFileName, Stream pdbStream)
        {
            FileStream disposablePeStream = null;
            FileStream disposePdbStream = null;

            // default values for PE file output
            peFileName ??= module.FullyQualifiedName;
            peStream ??= disposablePeStream = new FileStream(peFileName, FileMode.Create);

            // default values for PDB file output, if emitting symbols
            if (module.GetSymWriter() is IMetadataSymbolWriter)
            {
                pdbFileName ??= Path.ChangeExtension(peFileName, ".pdb");
                pdbStream ??= disposePdbStream = new FileStream(pdbFileName, FileMode.Create);
            }

            try
            {
                WriteModuleImpl(keyPair, publicKey, module, fileKind, portableExecutableKind, imageFileMachine, nativeResources, entryPoint, peFileName, peStream, pdbFileName, pdbStream);
            }
            finally
            {
                disposablePeStream?.Dispose();
                disposePdbStream?.Dispose();
            }
        }

        /// <summary>
        /// Implementation of module writing.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="publicKey"></param>
        /// <param name="module"></param>
        /// <param name="fileKind"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        /// <param name="nativeResources"></param>
        /// <param name="entryPoint"></param>
        /// <param name="peFileName"></param>
        /// <param name="peStream"></param>
        /// <param name="pdbFileName"></param>
        /// <param name="pdbStream"></param>
        static void WriteModuleImpl(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ModuleResourceSectionBuilder nativeResources, MethodInfo entryPoint, string peFileName, Stream peStream, string pdbFileName, Stream pdbStream)
        {
            // add some empty entries at the top just to ensure they are present
            module.Metadata.GetOrAddString("");
            module.Metadata.GetOrAddUserString("");
            module.Metadata.GetOrAddBlob(Array.Empty<byte>());
            module.Metadata.GetOrAddGuid(Guid.Empty);

            // write the module content
            WriteModuleImpl(module);

            // are we outputing for a 64 bit architecture?
            var is64BitArch = imageFileMachine is ImageFileMachine.AMD64 or ImageFileMachine.ARM64 or ImageFileMachine.IA64;

            // initialize PE header builder
            var peHeaderBuilder = new PEHeaderBuilder(
                machine: GetMachine(imageFileMachine),
                sectionAlignment: GetSectionAlignment(module, is64BitArch),
                fileAlignment: GetFileAlignment(module, is64BitArch),
                imageBase: GetImageBase(module, fileKind, is64BitArch),
                majorLinkerVersion: 0x30,
                minorLinkerVersion: 0,
                majorOperatingSystemVersion: 4,
                minorOperatingSystemVersion: 0,
                majorImageVersion: 0,
                minorImageVersion: 0,
                majorSubsystemVersion: GetMajorSubsystemVersion(imageFileMachine, fileKind),
                minorSubsystemVersion: GetMinorSubsystemVersion(imageFileMachine, fileKind),
                subsystem: GetSubsystem(fileKind),
                dllCharacteristics: (System.Reflection.PortableExecutable.DllCharacteristics)(int)module.__DllCharacteristics,
                imageCharacteristics: GetImageCharacteristics(imageFileMachine, fileKind),
                sizeOfStackReserve: GetSizeOfStackReserve(module, is64BitArch),
                sizeOfStackCommit: GetSizeOfStackCommit(module, is64BitArch),
                sizeOfHeapReserve: GetSizeOfHeapReserve(module, is64BitArch),
                sizeOfHeapCommit: GetSizeOfHeapCommit(module, is64BitArch));

            // root builder provides access to information about the assembled type metadata
            var rootBuilder = new MetadataRootBuilder(module.Metadata);
            var debugBuilder = default(DebugDirectoryBuilder);

            // ask the symbol writer if it supplies a debug directory
            if (module.GetSymWriter() is IMetadataSymbolWriter msym)
            {
                // generate a new metadata image for the PDB
                var pdbMetadata = new MetadataBuilder();
                pdbMetadata.GetOrAddString("");
                pdbMetadata.GetOrAddUserString("");
                pdbMetadata.GetOrAddBlob(Array.Empty<byte>());
                pdbMetadata.GetOrAddGuid(Guid.Empty);
                msym.WriteTo(pdbMetadata, out var pdbUserEntryPoint);

                // create new PDB builder around produced metadata
                var pdbContentHash = ImmutableArray<byte>.Empty;
                var pdb = new PortablePdbBuilder(
                    pdbMetadata,
                    rootBuilder.Sizes.RowCounts,
                    pdbUserEntryPoint != 0 ? (MethodDefinitionHandle)MetadataTokens.EntityHandle(module.ResolvePseudoToken(pdbUserEntryPoint)) : default,
                    module.Universe.Deterministic ? new Func<IEnumerable<Blob>, BlobContentId>(content => BlobContentId.FromHash(pdbContentHash = CryptographicHashProvider.ComputeHash(module.Universe.PdbChecksumAlgorithm, content))) : null);

                // serialize the PDB
                var pdbContent = new BlobBuilder();
                var pdbContentId = pdb.Serialize(pdbContent);

                // we are going to add a builder for a debug diretory
                debugBuilder = new DebugDirectoryBuilder();

                // mark symbols as reproducible
                if (module.Universe.Deterministic)
                    debugBuilder.AddReproducibleEntry();

                // a checksum hash was produced, so we add it to the debug information
                if (pdbContentHash != null)
                    debugBuilder.AddPdbChecksumEntry(module.Universe.PdbChecksumAlgorithm.Name, pdbContentHash);

                // file name specified means we are generating an external PDB
                if (pdbStream != null)
                {
                    // default PDB file name
                    if (peFileName != null)
                        pdbFileName ??= Path.ChangeExtension(peFileName, ".pdb");

                    // we don't actually want a full path in the PDB
                    if (pdbFileName != null)
                        pdbFileName = Path.GetFileName(pdbFileName);

                    // add code view entry describing the external PDB
                    debugBuilder.AddCodeViewEntry(PadPdbPath(pdbFileName), pdbContentId, pdb.FormatVersion);

                    // output PDB to external stream
                    pdbContent.WriteContentTo(pdbStream);
                }
                else
                {
                    // embed PDB into PE
                    debugBuilder.AddEmbeddedPortablePdbEntry(pdbContent, pdb.FormatVersion);
                }
            }

            // initialize PE builder
            var strongNameSignatureSize = ComputeStrongNameSignatureLength(publicKey);
            var peBuilder = new ManagedPEBuilder(
                peHeaderBuilder,
                new MetadataRootBuilder(module.Metadata),
                module.ILStream,
                managedResources: module.ResourceStream,
                nativeResources: nativeResources.Count > 0 ? nativeResources : null,
                debugDirectoryBuilder: debugBuilder,
                strongNameSignatureSize: strongNameSignatureSize,
                entryPoint: entryPoint != null ? (MethodDefinitionHandle)MetadataTokens.EntityHandle(entryPoint.GetCurrentToken()) : default,
                flags: GetCorFlags(portableExecutableKind, keyPair),
                deterministicIdProvider: GetDeterministicIdProvider(module));

            // serialize the image
            var pe = new BlobBuilder();
            var peContentId = peBuilder.Serialize(pe);

            // fixup MVID after generation
            var mvidId = new BlobWriter(module.GetModuleVersionIdFixup().Content);
            mvidId.WriteGuid(peContentId.Guid);
            Debug.Assert(mvidId.RemainingBytes == 0);

            // strong name specified, sign the blobs
            if (keyPair != null)
                peBuilder.Sign(pe, blobs => GetSignature(keyPair, blobs, strongNameSignatureSize));

            // write the final content to the output stream
            pe.WriteContentTo(peStream);
        }

        /// <summary>
        /// We pad the path to this minimal size to allow some tools to patch the path without the need to rewrite the entire image.
        /// </summary>
        /// <remarks>
        /// This is a workaround. It is copied from Roslyn.
        /// </remarks>
        /// <param name="path"></param>
        /// <returns></returns>
        static string PadPdbPath(string path)
        {
            const int MinLength = 260;
            return path + new string('\0', Math.Max(0, MinLength - Encoding.UTF8.GetByteCount(path) - 1));
        }

        /// <summary>
        /// Writes the managed metadata to the <see cref="MetadataBuilder"/>.
        /// </summary>
        /// <param name="module"></param>
        static void WriteModuleImpl(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            // registers replacements for pseudo tokens
            module.FixupMethodBodyTokens();

            // fixup content in tables
            module.TypeRefTable.Fixup(module);
            module.MethodImplTable.Fixup(module);
            module.MethodSemanticsTable.Fixup(module);
            module.InterfaceImplTable.Fixup(module);
            module.ResolveInterfaceImplPseudoTokens();
            module.MemberRefTable.Fixup(module);
            module.ConstantTable.Fixup(module);
            module.FieldMarshalTable.Fixup(module);
            module.DeclSecurityTable.Fixup(module);
            module.GenericParamTable.Fixup(module);
            module.CustomAttributeTable.Fixup(module);
            module.FieldLayoutTable.Fixup(module);
            module.FieldRVATable.Fixup(module);
            module.ImplMapTable.Fixup(module);
            module.ExportedTypeTable.Fixup(module);
            module.ManifestResourceTable.Fixup(module);
            module.MethodSpecTable.Fixup(module);
            module.GenericParamConstraint.Fixup(module);

            // close the symbol writer which may cause entries in the module table
            module.GetSymWriter()?.Close();

            // write to module metadata
            module.WriteResources();
            module.WriteMetadata();
        }

        /// <summary>
        /// Translates the <see cref="ImageFileMachine"/> parameter into a <see cref="Machine"/> value.
        /// </summary>
        /// <param name="imageFileMachine"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        static Machine GetMachine(ImageFileMachine imageFileMachine)
        {
            return imageFileMachine switch
            {
                ImageFileMachine.UNKNOWN => Machine.Unknown,
                ImageFileMachine.I386 => Machine.I386,
                ImageFileMachine.ARM => Machine.Arm,
                ImageFileMachine.AMD64 => Machine.Amd64,
                ImageFileMachine.IA64 => Machine.IA64,
                ImageFileMachine.ARM64 => Machine.Arm64,
                _ => throw new ArgumentOutOfRangeException("imageFileMachine"),
            };
        }

        /// <summary>
        /// Gets the appropriate section alignment for the image.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="is64BitArch"></param>
        /// <returns></returns>
        static int GetSectionAlignment(IKVM.Reflection.Emit.ModuleBuilder module, bool is64BitArch)
        {
            return DefaultSectionAlignment;
        }

        /// <summary>
        /// Gets the appropriate file alignment for the image.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="is64BitArch"></param>
        /// <returns></returns>
        static int GetFileAlignment(IKVM.Reflection.Emit.ModuleBuilder module, bool is64BitArch)
        {
            if (module.__FileAlignment == 0)
                return is64BitArch ? DefaultFileAlignment64Bit : DefaultFileAlignment32Bit;
            else
                return (int)module.__FileAlignment;
        }

        /// <summary>
        /// Gets the base address of the image.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="fileKind"></param>
        /// <param name="is64BitArch"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static ulong GetImageBase(IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, bool is64BitArch)
        {
            var baseAddress = unchecked(module.__ImageBase + 0x8000) & (is64BitArch ? 0xffffffffffff0000 : 0x00000000ffff0000);

            // cover values smaller than 0x8000, overflow and default value 0):
            if (baseAddress == 0)
            {
                if (fileKind is IKVM.Reflection.Emit.PEFileKinds.ConsoleApplication or IKVM.Reflection.Emit.PEFileKinds.WindowApplication)
                    return is64BitArch ? DefaultExeBaseAddress64Bit : DefaultExeBaseAddress32Bit;
                else
                    return is64BitArch ? DefaultDllBaseAddress64Bit : DefaultDllBaseAddress32Bit;
            }

            return baseAddress;
        }

        /// <summary>
        /// Gets the major subsystem verison.
        /// </summary>
        /// <returns></returns>
        static ushort GetMajorSubsystemVersion(ImageFileMachine imageFileMachine, IKVM.Reflection.Emit.PEFileKinds fileKinds)
        {
            return imageFileMachine == ImageFileMachine.ARM ? (ushort)6 : (ushort)4;
        }

        /// <summary>
        /// Gets the minor subsystem version.
        /// </summary>
        /// <returns></returns>
        static ushort GetMinorSubsystemVersion(ImageFileMachine imageFileMachine, IKVM.Reflection.Emit.PEFileKinds fileKinds)
        {
            return imageFileMachine == ImageFileMachine.ARM ? (ushort)2 : (ushort)0;
        }

        /// <summary>
        /// Obtains the appropriate <see cref="Subsystem"/> value for a given <see cref="PEFileKinds"/>.
        /// </summary>
        /// <param name="fileKind"></param>
        /// <returns></returns>
        static Subsystem GetSubsystem(IKVM.Reflection.Emit.PEFileKinds fileKind) => fileKind switch
        {
            IKVM.Reflection.Emit.PEFileKinds.WindowApplication => Subsystem.WindowsGui,
            _ => Subsystem.WindowsCui,
        };

        /// <summary>
        /// Obtains the appropriate <see cref="Characteristics"/> value for the given <see cref="ImageFileMachine"/> and <see cref="PEFileKinds"/>.
        /// </summary>
        /// <param name="imageFileMachine"></param>
        /// <param name="fileKind"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        static Characteristics GetImageCharacteristics(ImageFileMachine imageFileMachine, IKVM.Reflection.Emit.PEFileKinds fileKind)
        {
            var characteristics = Characteristics.ExecutableImage;

            switch (imageFileMachine)
            {
                case ImageFileMachine.UNKNOWN:
                    break;
                case ImageFileMachine.I386:
                    characteristics |= Characteristics.Bit32Machine;
                    break;
                case ImageFileMachine.ARM:
                    characteristics |= Characteristics.Bit32Machine | Characteristics.LargeAddressAware;
                    break;
                case ImageFileMachine.AMD64:
                case ImageFileMachine.IA64:
                case ImageFileMachine.ARM64:
                    characteristics |= Characteristics.LargeAddressAware;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("imageFileMachine");
            }

            switch (fileKind)
            {
                case IKVM.Reflection.Emit.PEFileKinds.Dll:
                    characteristics |= Characteristics.Dll;
                    break;
            }

            return characteristics;
        }

        /// <summary>
        /// Gets the size of the stack reserve.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="is64BitArch"></param>
        /// <returns></returns>
        static ulong GetSizeOfStackReserve(IKVM.Reflection.Emit.ModuleBuilder module, bool is64BitArch)
        {
            return is64BitArch ? DefaultSizeOfStackReserve64Bit : DefaultSizeOfStackReserve32Bit;
        }

        /// <summary>
        /// Gets the size of the stack commit.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="is64BitArch"></param>
        /// <returns></returns>
        static ulong GetSizeOfStackCommit(IKVM.Reflection.Emit.ModuleBuilder module, bool is64BitArch)
        {
            return is64BitArch ? DefaultSizeOfStackCommit64Bit : DefaultSizeOfStackCommit32Bit;
        }

        /// <summary>
        /// Gets the size of the heap reserve.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="is64BitArch"></param>
        /// <returns></returns>
        static ulong GetSizeOfHeapReserve(IKVM.Reflection.Emit.ModuleBuilder module, bool is64BitArch)
        {
            return is64BitArch ? DefaultSizeOfHeapReserve64Bit : DefaultSizeOfHeapReserve32Bit;
        }

        /// <summary>
        /// Gets the size of the heap commit.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="is64BitArch"></param>
        /// <returns></returns>
        static ulong GetSizeOfHeapCommit(IKVM.Reflection.Emit.ModuleBuilder module, bool is64BitArch)
        {
            return is64BitArch ? DefaultSizeOfHeapCommit64Bit : DefaultSizeOfHeapCommit32Bit;
        }

        /// <summary>
        /// Obtains the appropriate <see cref="CorFlags"/> value for the given <see cref="PortableExecutableKinds"/> and <see cref="StrongNameKeyPair"/>.
        /// </summary>
        /// <param name="portableExecutableKind"></param>
        /// <param name="keyPair"></param>
        /// <returns></returns>
        static CorFlags GetCorFlags(PortableExecutableKinds portableExecutableKind, StrongNameKeyPair keyPair)
        {
            var flags = default(CorFlags);

            if ((portableExecutableKind & PortableExecutableKinds.ILOnly) != 0)
                flags |= CorFlags.ILOnly;
            if ((portableExecutableKind & PortableExecutableKinds.Required32Bit) != 0)
                flags |= CorFlags.Requires32Bit;
            if ((portableExecutableKind & PortableExecutableKinds.Preferred32Bit) != 0)
                flags |= CorFlags.Requires32Bit | CorFlags.Prefers32Bit;
            if (keyPair != null)
                flags |= CorFlags.StrongNameSigned;

            return flags;
        }

        /// <summary>
        /// Gets the appropriate ID provider for the generation of the module version ID.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        static Func<IEnumerable<Blob>, BlobContentId> GetDeterministicIdProvider(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            if (module.Universe.Deterministic)
                return e => BlobContentId.FromHash(CryptographicHashProvider.ComputeHash(HashAlgorithmName.SHA256, e));
            else
                return null;
        }

        /// <summary>
        /// Computes the length of the strong name signature.
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        static int ComputeStrongNameSignatureLength(byte[] publicKey)
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

        /// <summary>
        /// Calculates the strong name signature for the specified blobs.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="blobs"></param>
        /// <param name="strongNameSignatureSize"></param>
        /// <returns></returns>
        static byte[] GetSignature(StrongNameKeyPair keyPair, IEnumerable<Blob> blobs, int strongNameSignatureSize)
        {
            // sign the hash with the keypair
            using var rsa = keyPair.CreateRSA();
            var signature = rsa.SignHash(CryptographicHashProvider.ComputeHash(HashAlgorithmName.SHA1, blobs).ToArray(), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            Array.Reverse(signature);

            // check that our signature length matches
            if (signature.Length != strongNameSignatureSize)
                throw new InvalidOperationException("Signature length mismatch.");

            return signature;
        }

    }

}
