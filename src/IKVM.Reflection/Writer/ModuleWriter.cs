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
using System.IO;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Writer
{

    static class ModuleWriter
    {

        /// <summary>
        /// Writes the module specified by <paramref name="moduleBuilder"/>.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="publicKey"></param>
        /// <param name="moduleBuilder"></param>
        /// <param name="fileKind"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        /// <param name="nativeResources"></param>
        /// <param name="entryPointToken"></param>
        internal static void WriteModule(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder moduleBuilder, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ResourceSectionBuilder nativeResources, MethodDefinitionHandle entryPointToken)
        {
            WriteModule(keyPair, publicKey, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, nativeResources, entryPointToken, null);
        }

        /// <summary>
        /// Writes the module specified by <paramref name="moduleBuilder"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="publicKey"></param>
        /// <param name="moduleBuilder"></param>
        /// <param name="fileKind"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        /// <param name="nativeResources"></param>
        /// <param name="entryPointToken"></param>
        /// <param name="stream"></param>
        internal static void WriteModule(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder moduleBuilder, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ResourceSectionBuilder nativeResources, MethodDefinitionHandle entryPointToken, Stream stream)
        {
            if (stream == null)
            {
                var fileName = moduleBuilder.FullyQualifiedName;
                var mono = System.Type.GetType("Mono.Runtime") != null;
                if (mono)
                {
                    try
                    {
                        // Mono mmaps the file, so unlink the previous version since it may be in use
                        File.Delete(fileName);
                    }
                    catch
                    {

                    }
                }

                using (var fs = new FileStream(fileName, FileMode.Create))
                {
                    var entryPoint = entryPointToken.IsNil == false ? entryPointToken : default;
                    WriteModuleImpl(keyPair, publicKey, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, nativeResources, entryPoint, fs);
                }

                // if we're running on Mono, mark the module as executable by using a Mono private API extension
                if (mono)
                    File.SetAttributes(fileName, (FileAttributes)(unchecked((int)0x80000000)));
            }
            else
            {
                var entryPoint = entryPointToken.IsNil == false ? entryPointToken : default;
                WriteModuleImpl(keyPair, publicKey, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, nativeResources, entryPoint, stream);
            }
        }

        /// <summary>
        /// Implementation of module writing to output stream.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="publicKey"></param>
        /// <param name="module"></param>
        /// <param name="fileKind"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        /// <param name="nativeResources"></param>
        /// <param name="entryPoint"></param>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        static void WriteModuleImpl(StrongNameKeyPair keyPair, byte[] publicKey, IKVM.Reflection.Emit.ModuleBuilder module, IKVM.Reflection.Emit.PEFileKinds fileKind, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine, ResourceSectionBuilder nativeResources, MethodDefinitionHandle entryPoint, Stream stream)
        {
            module.ApplyUnmanagedExports(imageFileMachine);
            module.FixupMethodBodyTokens();

            // for compatibility with Reflection.Emit, if there aren't any user strings, we add one
            module.Metadata.GetOrAddUserString("");

#if NETFRAMEWORK
            if (module.symbolWriter != null)
            {
                module.WriteSymbolTokenMap();
                module.symbolWriter.Close();
            }
#endif

            // write the module content
            WriteModuleImpl(module);

            // initialize PE header builder
            var peHeaderBuilder = new PEHeaderBuilder(
                machine: GetMachine(imageFileMachine),
                fileAlignment: module.__FileAlignment,
                imageBase: (ulong)module.__ImageBase,
                subsystem: GetSubsystem(fileKind),
                dllCharacteristics: (System.Reflection.PortableExecutable.DllCharacteristics)(int)module.__DllCharacteristics,
                imageCharacteristics: GetImageCharacteristics(imageFileMachine, fileKind),
                sizeOfStackReserve: module.GetStackReserve(1048576));

            // initialize PE builder
            var strongNameSignatureSize = ComputeStrongNameSignatureLength(publicKey);
            var peBuilder = new ManagedPEBuilder(
                peHeaderBuilder,
                new MetadataRootBuilder(module.Metadata),
                module.ILStream,
                managedResources: module.ResourceStream,
                nativeResources: nativeResources,
                strongNameSignatureSize: strongNameSignatureSize,
                entryPoint: entryPoint,
                flags: GetCorFlags(portableExecutableKind, keyPair),
                deterministicIdProvider: GetDeterministicIdProvider(module));

            // serialize the image
            var pe = new BlobBuilder();
            peBuilder.Serialize(pe);

            // strong name specified, sign the blobs
            if (keyPair != null)
                peBuilder.Sign(pe, blobs => GetSignature(keyPair, blobs, strongNameSignatureSize));

            // write the final content to the output stream
            pe.WriteContentTo(stream);
        }

        /// <summary>
        /// Writes the managed metadata to the <see cref="MetadataBuilder"/>.
        /// </summary>
        /// <param name="moduleBuilder"></param>
        static void WriteModuleImpl(IKVM.Reflection.Emit.ModuleBuilder moduleBuilder)
        {
            // now that we're ready to start writing, we need to do some fix ups
            moduleBuilder.TypeRef.Fixup(moduleBuilder);
            moduleBuilder.MethodImpl.Fixup(moduleBuilder);
            moduleBuilder.MethodSemantics.Fixup(moduleBuilder);
            moduleBuilder.InterfaceImpl.Fixup(moduleBuilder);
            moduleBuilder.ResolveInterfaceImplPseudoTokens();
            moduleBuilder.MemberRef.Fixup(moduleBuilder);
            moduleBuilder.Constant.Fixup(moduleBuilder);
            moduleBuilder.FieldMarshal.Fixup(moduleBuilder);
            moduleBuilder.DeclSecurity.Fixup(moduleBuilder);
            moduleBuilder.GenericParam.Fixup(moduleBuilder);
            moduleBuilder.CustomAttribute.Fixup(moduleBuilder);
            moduleBuilder.FieldLayout.Fixup(moduleBuilder);
            moduleBuilder.FieldRVA.Fixup(moduleBuilder);
            moduleBuilder.ImplMap.Fixup(moduleBuilder);
            moduleBuilder.ExportedType.Fixup(moduleBuilder);
            moduleBuilder.ManifestResource.Fixup(moduleBuilder);
            moduleBuilder.MethodSpec.Fixup(moduleBuilder);
            moduleBuilder.GenericParamConstraint.Fixup(moduleBuilder);

            moduleBuilder.ILStream.WriteBytes(moduleBuilder.methodBodies.ToArray());
            moduleBuilder.WriteResources();
            moduleBuilder.WriteMetadata();
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
                ImageFileMachine.I386 => Machine.I386,
                ImageFileMachine.ARM => Machine.Arm,
                ImageFileMachine.AMD64 => Machine.Amd64,
                ImageFileMachine.IA64 => Machine.IA64,
                ImageFileMachine.ARM64 => Machine.Arm64,
                _ => throw new ArgumentOutOfRangeException("imageFileMachine"),
            };
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
            var characteristics = default(Characteristics);

            switch (imageFileMachine)
            {
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

            if (fileKind == IKVM.Reflection.Emit.PEFileKinds.Dll)
                characteristics |= Characteristics.Dll;

            return characteristics;
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
        /// <param name="moduleBuilder"></param>
        /// <returns></returns>
        static Func<IEnumerable<Blob>, BlobContentId> GetDeterministicIdProvider(Emit.ModuleBuilder moduleBuilder)
        {
            if (moduleBuilder.GetModuleVersionIdOrEmpty() is Guid mvid && mvid != Guid.Empty)
                return e => new BlobContentId(mvid, 1);
            else if (moduleBuilder.universe.Deterministic)
                return e => BlobContentId.FromHash(GetSHA1Hash(e));
            else
                return BlobContentId.GetTimeBasedProvider();

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
        /// Gets a hash of the specified blobs.
        /// </summary>
        /// <param name="blobs"></param>
        /// <returns></returns>
        static byte[] GetSHA1Hash(IEnumerable<Blob> blobs)
        {
            using var sha1 = SHA1.Create();

            foreach (var blob in blobs)
            {
                var data = blob.GetBytes();
                sha1.TransformBlock(data.Array, data.Offset, data.Count, null, 0);
            }

            sha1.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

            return sha1.Hash;
        }

        /// <summary>
        /// Calculates the strong name signature for the specified blobs.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="blobs"></param>
        /// <param name="strongNameSignatureSize"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static byte[] GetSignature(StrongNameKeyPair keyPair, IEnumerable<Blob> blobs, int strongNameSignatureSize)
        {
            // sign the hash with the keypair
            using var rsa = keyPair.CreateRSA();
            var signature = rsa.SignHash(GetSHA1Hash(blobs), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            Array.Reverse(signature);

            // check that our signature length matches
            if (signature.Length != strongNameSignatureSize)
                throw new InvalidOperationException("Signature length mismatch.");

            return signature;
        }

    }

}
