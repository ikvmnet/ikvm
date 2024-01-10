/*
  Copyright (C) 2009-2011 Jeroen Frijters

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
using System.IO;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class MetadataReader : MetadataRW
    {

        const int bufferLength = 2048;

        readonly Stream stream;
        readonly byte[] buffer = new byte[bufferLength];

        int pos = bufferLength;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="stream"></param>
        /// <param name="heapSizes"></param>
        internal MetadataReader(ModuleReader module, Stream stream, byte heapSizes) :
            base(module, (heapSizes & 0x01) != 0, (heapSizes & 0x02) != 0, (heapSizes & 0x04) != 0)
        {
            this.stream = stream;
        }

        void FillBuffer(int needed)
        {
            int count = bufferLength - pos;
            if (count != 0)
            {
                // move remaining bytes to the front of the buffer
                Buffer.BlockCopy(buffer, pos, buffer, 0, count);
            }

            pos = 0;

            while (count < needed)
            {
                var len = stream.Read(buffer, count, bufferLength - count);
                if (len == 0)
                    throw new BadImageFormatException();

                count += len;
            }

            if (count != bufferLength)
            {
                // we didn't fill the buffer completely, so have to restore the invariant
                // that all data from pos up until the end of the buffer is valid
                Buffer.BlockCopy(buffer, 0, buffer, bufferLength - count, count);
                pos = bufferLength - count;
            }
        }

        internal ushort ReadUInt16()
        {
            return (ushort)ReadInt16();
        }

        internal short ReadInt16()
        {
            if (pos > bufferLength - 2)
                FillBuffer(2);

            var b1 = buffer[pos++];
            var b2 = buffer[pos++];
            return (short)(b1 | (b2 << 8));
        }

        internal int ReadInt32()
        {
            if (pos > bufferLength - 4)
                FillBuffer(4);

            var b1 = buffer[pos++];
            var b2 = buffer[pos++];
            var b3 = buffer[pos++];
            var b4 = buffer[pos++];
            return b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
        }

        int ReadIndex(bool big)
        {
            return big ? ReadInt32() : ReadUInt16();
        }

        internal int ReadStringIndex()
        {
            return ReadIndex(bigStrings);
        }

        internal int ReadGuidIndex()
        {
            return ReadIndex(bigGuids);
        }

        internal int ReadBlobIndex()
        {
            return ReadIndex(bigBlobs);
        }

        internal int ReadResolutionScope()
        {
            var codedIndex = ReadIndex(bigResolutionScope);
            return (codedIndex & 3) switch
            {
                0 => (ModuleTable.Index << 24) + (codedIndex >> 2),
                1 => (ModuleRefTable.Index << 24) + (codedIndex >> 2),
                2 => (AssemblyRefTable.Index << 24) + (codedIndex >> 2),
                3 => (TypeRefTable.Index << 24) + (codedIndex >> 2),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadTypeDefOrRef()
        {
            var codedIndex = ReadIndex(bigTypeDefOrRef);
            return (codedIndex & 3) switch
            {
                0 => (TypeDefTable.Index << 24) + (codedIndex >> 2),
                1 => (TypeRefTable.Index << 24) + (codedIndex >> 2),
                2 => (TypeSpecTable.Index << 24) + (codedIndex >> 2),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadMemberRefParent()
        {
            var codedIndex = ReadIndex(bigMemberRefParent);
            return (codedIndex & 7) switch
            {
                0 => (TypeDefTable.Index << 24) + (codedIndex >> 3),
                1 => (TypeRefTable.Index << 24) + (codedIndex >> 3),
                2 => (ModuleRefTable.Index << 24) + (codedIndex >> 3),
                3 => (MethodDefTable.Index << 24) + (codedIndex >> 3),
                4 => (TypeSpecTable.Index << 24) + (codedIndex >> 3),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadHasCustomAttribute()
        {
            var codedIndex = ReadIndex(bigHasCustomAttribute);
            return (codedIndex & 31) switch
            {
                0 => (MethodDefTable.Index << 24) + (codedIndex >> 5),
                1 => (FieldTable.Index << 24) + (codedIndex >> 5),
                2 => (TypeRefTable.Index << 24) + (codedIndex >> 5),
                3 => (TypeDefTable.Index << 24) + (codedIndex >> 5),
                4 => (ParamTable.Index << 24) + (codedIndex >> 5),
                5 => (InterfaceImplTable.Index << 24) + (codedIndex >> 5),
                6 => (MemberRefTable.Index << 24) + (codedIndex >> 5),
                7 => (ModuleTable.Index << 24) + (codedIndex >> 5),
                8 => (DeclSecurityTable.Index << 24) + (codedIndex >> 5),
                9 => (PropertyTable.Index << 24) + (codedIndex >> 5),
                10 => (EventTable.Index << 24) + (codedIndex >> 5),
                11 => (StandAloneSigTable.Index << 24) + (codedIndex >> 5),
                12 => (ModuleRefTable.Index << 24) + (codedIndex >> 5),
                13 => (TypeSpecTable.Index << 24) + (codedIndex >> 5),
                14 => (AssemblyTable.Index << 24) + (codedIndex >> 5),
                15 => (AssemblyRefTable.Index << 24) + (codedIndex >> 5),
                16 => (FileTable.Index << 24) + (codedIndex >> 5),
                17 => (ExportedTypeTable.Index << 24) + (codedIndex >> 5),
                18 => (ManifestResourceTable.Index << 24) + (codedIndex >> 5),
                19 => (GenericParamTable.Index << 24) + (codedIndex >> 5),
                20 => (GenericParamConstraintTable.Index << 24) + (codedIndex >> 5),
                21 => (MethodSpecTable.Index << 24) + (codedIndex >> 5),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadCustomAttributeType()
        {
            var codedIndex = ReadIndex(bigCustomAttributeType);
            return (codedIndex & 7) switch
            {
                2 => (MethodDefTable.Index << 24) + (codedIndex >> 3),
                3 => (MemberRefTable.Index << 24) + (codedIndex >> 3),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadMethodDefOrRef()
        {
            var codedIndex = ReadIndex(bigMethodDefOrRef);
            return (codedIndex & 1) switch
            {
                0 => (MethodDefTable.Index << 24) + (codedIndex >> 1),
                1 => (MemberRefTable.Index << 24) + (codedIndex >> 1),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadHasConstant()
        {
            var codedIndex = ReadIndex(bigHasConstant);
            return (codedIndex & 3) switch
            {
                0 => (FieldTable.Index << 24) + (codedIndex >> 2),
                1 => (ParamTable.Index << 24) + (codedIndex >> 2),
                2 => (PropertyTable.Index << 24) + (codedIndex >> 2),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadHasSemantics()
        {
            var codedIndex = ReadIndex(bigHasSemantics);
            return (codedIndex & 1) switch
            {
                0 => (EventTable.Index << 24) + (codedIndex >> 1),
                1 => (PropertyTable.Index << 24) + (codedIndex >> 1),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadHasFieldMarshal()
        {
            var codedIndex = ReadIndex(bigHasFieldMarshal);
            return (codedIndex & 1) switch
            {
                0 => (FieldTable.Index << 24) + (codedIndex >> 1),
                1 => (ParamTable.Index << 24) + (codedIndex >> 1),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadHasDeclSecurity()
        {
            var codedIndex = ReadIndex(bigHasDeclSecurity);
            return (codedIndex & 3) switch
            {
                0 => (TypeDefTable.Index << 24) + (codedIndex >> 2),
                1 => (MethodDefTable.Index << 24) + (codedIndex >> 2),
                2 => (AssemblyTable.Index << 24) + (codedIndex >> 2),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadTypeOrMethodDef()
        {
            var codedIndex = ReadIndex(bigTypeOrMethodDef);
            return (codedIndex & 1) switch
            {
                0 => (TypeDefTable.Index << 24) + (codedIndex >> 1),
                1 => (MethodDefTable.Index << 24) + (codedIndex >> 1),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadMemberForwarded()
        {
            var codedIndex = ReadIndex(bigMemberForwarded);
            return (codedIndex & 1) switch
            {
                0 => (FieldTable.Index << 24) + (codedIndex >> 1),
                1 => (MethodDefTable.Index << 24) + (codedIndex >> 1),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadImplementation()
        {
            var codedIndex = ReadIndex(bigImplementation);
            return (codedIndex & 3) switch
            {
                0 => (FileTable.Index << 24) + (codedIndex >> 2),
                1 => (AssemblyRefTable.Index << 24) + (codedIndex >> 2),
                2 => (ExportedTypeTable.Index << 24) + (codedIndex >> 2),
                _ => throw new BadImageFormatException(),
            };
        }

        internal int ReadField()
        {
            return ReadIndex(bigField);
        }

        internal int ReadMethodDef()
        {
            return ReadIndex(bigMethodDef);
        }

        internal int ReadParam()
        {
            return ReadIndex(bigParam);
        }

        internal int ReadProperty()
        {
            return ReadIndex(bigProperty);
        }

        internal int ReadEvent()
        {
            return ReadIndex(bigEvent);
        }

        internal int ReadTypeDef()
        {
            return ReadIndex(bigTypeDef) | (TypeDefTable.Index << 24);
        }

        internal int ReadGenericParam()
        {
            return ReadIndex(bigGenericParam) | (GenericParamTable.Index << 24);
        }

        internal int ReadModuleRef()
        {
            return ReadIndex(bigModuleRef) | (ModuleRefTable.Index << 24);
        }

    }

}
