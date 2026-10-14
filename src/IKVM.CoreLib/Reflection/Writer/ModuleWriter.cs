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

    /// <summary>
    /// Writes a <see cref="IKVM.Reflection.Emit.ModuleBuilder"/> to a portable executable image, and optionally a
    /// portable PDB, using the System.Reflection.Metadata writers.
    /// </summary>
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
        // both 32 and 64 bit binaries used this value in the native stack
        const ushort DefaultFileAlignment64Bit = 0x200;

        const ushort DefaultSectionAlignment = 0x2000;

        /// <summary>
        /// Writes the module specified by <paramref name="module"/> to the file named by its fully qualified name, and
        /// its symbols, if the module emits them, to a PDB file next to it.
        /// </summary>
        /// <param name="keyPair">The key pair used to strong name sign the image, or <see langword="null"/> to leave it unsigned.</param>
        /// <param name="publicKey">The public key of the assembly, used to size the strong name signature, or <see langword="null"/> if the assembly has none.</param>
        /// <param name="module">The module to write.</param>
        /// <param name="fileKind">The kind of image to write.</param>
        /// <param name="portableExecutableKind">The nature of the code in the image.</param>
        /// <param name="imageFileMachine">The platform the image targets.</param>
        /// <param name="nativeResources">The unmanaged resources to place in the image.</param>
        /// <param name="entryPoint">The entry point of the image, or <see langword="null"/> if it has none.</param>
        internal static void WriteModule(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ModuleResourceSectionBuilder nativeResources, MethodInfo entryPoint) =>
            WriteModule(keyPair, publicKey, module, fileKind, portableExecutableKind, imageFileMachine, nativeResources, entryPoint, null, null, null, null);

        /// <summary>
        /// Writes the module specified by <paramref name="module"/> to the specified <see cref="Stream"/>. Streams that
        /// are not supplied are opened on files named after the module, and closed when writing completes.
        /// </summary>
        /// <param name="keyPair">The key pair used to strong name sign the image, or <see langword="null"/> to leave it unsigned.</param>
        /// <param name="publicKey">The public key of the assembly, used to size the strong name signature, or <see langword="null"/> if the assembly has none.</param>
        /// <param name="module">The module to write.</param>
        /// <param name="fileKind">The kind of image to write.</param>
        /// <param name="portableExecutableKind">The nature of the code in the image.</param>
        /// <param name="imageFileMachine">The platform the image targets.</param>
        /// <param name="nativeResources">The unmanaged resources to place in the image.</param>
        /// <param name="entryPoint">The entry point of the image, or <see langword="null"/> if it has none.</param>
        /// <param name="peFileName">The file name of the image, or <see langword="null"/> to use the module's fully qualified name.</param>
        /// <param name="peStream">The stream that receives the image, or <see langword="null"/> to create the file named by <paramref name="peFileName"/>.</param>
        /// <param name="pdbFileName">The file name of the PDB, or <see langword="null"/> to derive it from <paramref name="peFileName"/>.</param>
        /// <param name="pdbStream">The stream that receives the PDB, or <see langword="null"/> to create the file named by <paramref name="pdbFileName"/>. Ignored if the module does not emit symbols.</param>
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
        /// Writes the module metadata, then serializes the PDB, if the module emits symbols, and the PE image, fixes up
        /// the module version ID and strong name signs the image.
        /// </summary>
        /// <param name="keyPair">The key pair used to strong name sign the image, or <see langword="null"/> to leave it unsigned.</param>
        /// <param name="publicKey">The public key of the assembly, used to size the strong name signature, or <see langword="null"/> if the assembly has none.</param>
        /// <param name="module">The module to write.</param>
        /// <param name="fileKind">The kind of image to write.</param>
        /// <param name="portableExecutableKind">The nature of the code in the image.</param>
        /// <param name="imageFileMachine">The platform the image targets.</param>
        /// <param name="nativeResources">The unmanaged resources to place in the image.</param>
        /// <param name="entryPoint">The entry point of the image, or <see langword="null"/> if it has none.</param>
        /// <param name="peFileName">The file name of the image, used to derive the PDB file name.</param>
        /// <param name="peStream">The stream that receives the image.</param>
        /// <param name="pdbFileName">The file name of the PDB, recorded in the image's debug directory.</param>
        /// <param name="pdbStream">The stream that receives the PDB, or <see langword="null"/> to embed the PDB in the image.</param>
        static void WriteModuleImpl(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ModuleResourceSectionBuilder nativeResources, MethodInfo entryPoint, string peFileName, Stream peStream, string pdbFileName, Stream pdbStream)
        {
            // add some empty entries at the top just to ensure they are present
            module.Metadata.GetOrAddString("");
            module.Metadata.GetOrAddUserString("");
            module.Metadata.GetOrAddBlob(Array.Empty<byte>());
            module.Metadata.GetOrAddGuid(Guid.Empty);

            // write the module content
            WriteModuleImpl(module);

            // are we outputting for a 64 bit architecture?
            var is64BitArch = imageFileMachine is ImageFileMachine.AMD64 or ImageFileMachine.ARM64 or ImageFileMachine.IA64;

            // initialize PE header builder
            var peHeaderBuilder = new PEHeaderBuilder(
                machine: GetMachine(imageFileMachine),
                sectionAlignment: GetSectionAlignment(),
                fileAlignment: GetFileAlignment(module, is64BitArch),
                imageBase: GetImageBase(module, fileKind, is64BitArch),
                majorLinkerVersion: 0x30,
                minorLinkerVersion: 0,
                majorOperatingSystemVersion: 4,
                minorOperatingSystemVersion: 0,
                majorImageVersion: 0,
                minorImageVersion: 0,
                majorSubsystemVersion: GetMajorSubsystemVersion(imageFileMachine),
                minorSubsystemVersion: GetMinorSubsystemVersion(imageFileMachine),
                subsystem: GetSubsystem(fileKind),
                dllCharacteristics: (System.Reflection.PortableExecutable.DllCharacteristics)(int)module.__DllCharacteristics,
                imageCharacteristics: GetImageCharacteristics(imageFileMachine, fileKind),
                sizeOfStackReserve: GetSizeOfStackReserve(is64BitArch),
                sizeOfStackCommit: GetSizeOfStackCommit(is64BitArch),
                sizeOfHeapReserve: GetSizeOfHeapReserve(is64BitArch),
                sizeOfHeapCommit: GetSizeOfHeapCommit(is64BitArch));

            // the metadata root, whose row counts the PDB records and which the PE builder serializes
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

                // we are going to add a builder for a debug directory
                debugBuilder = new DebugDirectoryBuilder();

                // mark symbols as reproducible
                if (module.Universe.Deterministic)
                    debugBuilder.AddReproducibleEntry();

                // a checksum hash was produced, so we add it to the debug information
                if (pdbContentHash.IsDefaultOrEmpty == false)
                    debugBuilder.AddPdbChecksumEntry(module.Universe.PdbChecksumAlgorithm.Name, pdbContentHash);

                // a PDB stream means we are generating an external PDB
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
                rootBuilder,
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
            module.SetModuleVersionId(peContentId.Guid);

            // strong name specified, sign the blobs
            if (keyPair != null)
                peBuilder.Sign(pe, blobs => GetSignature(keyPair, blobs, strongNameSignatureSize));

            // write the final content to the output stream
            pe.WriteContentTo(peStream);
        }

        /// <summary>
        /// Pads the PDB path recorded in the CodeView debug entry to a minimal size, so that some tools can patch the
        /// path without the need to rewrite the entire image.
        /// </summary>
        /// <remarks>
        /// This is a workaround. It is copied from Roslyn.
        /// </remarks>
        /// <param name="path">The PDB path.</param>
        /// <returns>The path followed by enough null characters to make it at least 260 bytes long in UTF-8, including the terminator.</returns>
        static string PadPdbPath(string path)
        {
            const int MinLength = 260;
            return path + new string('\0', Math.Max(0, MinLength - Encoding.UTF8.GetByteCount(path) - 1));
        }

        /// <summary>
        /// Writes the managed metadata and resources of the module to its <see cref="MetadataBuilder"/>.
        /// </summary>
        /// <param name="module">The module to write.</param>
        static void WriteModuleImpl(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            // registers replacements for pseudo tokens
            module.FixupMethodBodyTokens();

            // close the symbol writer which may cause entries in the module table
            module.GetSymWriter()?.Close();

            // write to module metadata
            module.WriteResources();
            module.WriteMetadata();
        }

        /// <summary>
        /// Translates the <see cref="ImageFileMachine"/> parameter into a <see cref="Machine"/> value.
        /// </summary>
        /// <param name="imageFileMachine">The platform the image targets.</param>
        /// <returns>The machine type for the PE header.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="imageFileMachine"/> is not a supported platform.</exception>
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
        /// Gets the appropriate section alignment for the image. This is currently always the default alignment.
        /// </summary>
        /// <returns>The section alignment, in bytes.</returns>
        static int GetSectionAlignment() => DefaultSectionAlignment;

        /// <summary>
        /// Gets the appropriate file alignment for the image: the alignment set on the module, or the default if none
        /// was set.
        /// </summary>
        /// <param name="module">The module being written.</param>
        /// <param name="is64BitArch">Whether the image targets a 64 bit platform.</param>
        /// <returns>The file alignment, in bytes.</returns>
        static int GetFileAlignment(IKVM.Reflection.Emit.ModuleBuilder module, bool is64BitArch)
        {
            if (module.__FileAlignment == 0)
                return is64BitArch ? DefaultFileAlignment64Bit : DefaultFileAlignment32Bit;
            else
                return (int)module.__FileAlignment;
        }

        /// <summary>
        /// Gets the base address of the image: the image base set on the module, rounded to a multiple of 64 KB, or the
        /// default for the kind of image if that rounds to zero.
        /// </summary>
        /// <param name="module">The module being written.</param>
        /// <param name="fileKind">The kind of image being written.</param>
        /// <param name="is64BitArch">Whether the image targets a 64 bit platform.</param>
        /// <returns>The preferred base address of the image.</returns>
        static ulong GetImageBase(IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, bool is64BitArch)
        {
            var baseAddress = unchecked(module.__ImageBase + 0x8000) & (is64BitArch ? 0xffffffffffff0000 : 0x00000000ffff0000);

            // cover values smaller than 0x8000, overflow and the default value 0
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
        /// Gets the major subsystem version: 6 for ARM, which requires Windows 8, and 4 otherwise.
        /// </summary>
        /// <param name="imageFileMachine">The platform the image targets.</param>
        /// <returns>The major subsystem version.</returns>
        static ushort GetMajorSubsystemVersion(ImageFileMachine imageFileMachine) =>
            imageFileMachine == ImageFileMachine.ARM ? (ushort)6 : (ushort)4;

        /// <summary>
        /// Gets the minor subsystem version: 2 for ARM, which requires Windows 8, and 0 otherwise.
        /// </summary>
        /// <param name="imageFileMachine">The platform the image targets.</param>
        /// <returns>The minor subsystem version.</returns>
        static ushort GetMinorSubsystemVersion(ImageFileMachine imageFileMachine) =>
            imageFileMachine == ImageFileMachine.ARM ? (ushort)2 : (ushort)0;

        /// <summary>
        /// Obtains the appropriate <see cref="Subsystem"/> value for a given
        /// <see cref="IKVM.Reflection.Emit.PEFileKinds"/>.
        /// </summary>
        /// <param name="fileKind">The kind of image being written.</param>
        /// <returns><see cref="Subsystem.WindowsGui"/> for a Windows application, and <see cref="Subsystem.WindowsCui"/> otherwise.</returns>
        static Subsystem GetSubsystem(IKVM.Reflection.Emit.PEFileKinds fileKind) => fileKind switch
        {
            IKVM.Reflection.Emit.PEFileKinds.WindowApplication => Subsystem.WindowsGui,
            _ => Subsystem.WindowsCui,
        };

        /// <summary>
        /// Obtains the appropriate <see cref="Characteristics"/> value for the given <see cref="ImageFileMachine"/> and
        /// <see cref="IKVM.Reflection.Emit.PEFileKinds"/>.
        /// </summary>
        /// <param name="imageFileMachine">The platform the image targets.</param>
        /// <param name="fileKind">The kind of image being written.</param>
        /// <returns>The image characteristics for the PE header.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="imageFileMachine"/> is not a supported platform.</exception>
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
        /// Gets the size of the stack reserve. This is currently always the default for the platform.
        /// </summary>
        /// <param name="is64BitArch">Whether the image targets a 64 bit platform.</param>
        /// <returns>The size, in bytes.</returns>
        static ulong GetSizeOfStackReserve(bool is64BitArch) =>
            is64BitArch ? DefaultSizeOfStackReserve64Bit : DefaultSizeOfStackReserve32Bit;

        /// <summary>
        /// Gets the size of the stack commit. This is currently always the default for the platform.
        /// </summary>
        /// <param name="is64BitArch">Whether the image targets a 64 bit platform.</param>
        /// <returns>The size, in bytes.</returns>
        static ulong GetSizeOfStackCommit(bool is64BitArch) =>
            is64BitArch ? DefaultSizeOfStackCommit64Bit : DefaultSizeOfStackCommit32Bit;

        /// <summary>
        /// Gets the size of the heap reserve. This is currently always the default for the platform.
        /// </summary>
        /// <param name="is64BitArch">Whether the image targets a 64 bit platform.</param>
        /// <returns>The size, in bytes.</returns>
        static ulong GetSizeOfHeapReserve(bool is64BitArch) =>
            is64BitArch ? DefaultSizeOfHeapReserve64Bit : DefaultSizeOfHeapReserve32Bit;

        /// <summary>
        /// Gets the size of the heap commit. This is currently always the default for the platform.
        /// </summary>
        /// <param name="is64BitArch">Whether the image targets a 64 bit platform.</param>
        /// <returns>The size, in bytes.</returns>
        static ulong GetSizeOfHeapCommit(bool is64BitArch) =>
            is64BitArch ? DefaultSizeOfHeapCommit64Bit : DefaultSizeOfHeapCommit32Bit;

        /// <summary>
        /// Obtains the appropriate <see cref="CorFlags"/> value for the given <see cref="PortableExecutableKinds"/> and
        /// <see cref="StrongNameKeyPair"/>.
        /// </summary>
        /// <param name="portableExecutableKind">The nature of the code in the image.</param>
        /// <param name="keyPair">The key pair used to sign the image, or <see langword="null"/> if it is not signed.</param>
        /// <returns>The CLI header flags.</returns>
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
        /// <param name="module">The module being written.</param>
        /// <returns>A provider that derives the ID from a SHA-256 hash of the content if the universe is deterministic; otherwise, <see langword="null"/> to use a random ID.</returns>
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
        /// <param name="publicKey">The public key blob of the assembly, or <see langword="null"/> if the assembly has none.</param>
        /// <returns>The length of the signature in bytes, or 0 if there is no public key.</returns>
        static int ComputeStrongNameSignatureLength(byte[] publicKey)
        {
            if (publicKey == null)
            {
                return 0;
            }
            else if (publicKey.Length == 16)
            {
                // it must be the ECMA pseudo public key, we don't know the key size of the real key, but currently both
                // Mono and Microsoft use a 1024 bit key size
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
        /// <param name="keyPair">The key pair to sign with.</param>
        /// <param name="blobs">The blobs of the image that are covered by the signature.</param>
        /// <param name="strongNameSignatureSize">The size reserved for the signature in the image.</param>
        /// <returns>The signature, in the little-endian byte order the image expects.</returns>
        /// <exception cref="InvalidOperationException">The signature does not have the reserved size.</exception>
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
