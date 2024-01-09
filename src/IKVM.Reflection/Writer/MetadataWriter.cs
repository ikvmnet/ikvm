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
using System.IO;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Writer
{

    sealed class MetadataWriter : MetadataRW
    {

        readonly ModuleBuilder moduleBuilder;
        readonly Stream stream;
        readonly byte[] buffer = new byte[8];

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="stream"></param>
        internal MetadataWriter(ModuleBuilder module, Stream stream) : base(module, module.Strings.IsBig, module.Guids.IsBig, module.Blobs.IsBig)
        {
            this.moduleBuilder = module;
            this.stream = stream;
        }

        internal ModuleBuilder ModuleBuilder => moduleBuilder;

        internal uint Position => (uint)stream.Position;

        internal void Write(ByteBuffer bb) => bb.WriteTo(stream);

        internal void WriteAsciiz(string value)
        {
            foreach (var c in value)
                stream.WriteByte((byte)c);

            stream.WriteByte(0);
        }

        internal void Write(byte[] value)
        {
            stream.Write(value, 0, value.Length);
        }

        internal void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);
        }

        internal void Write(byte value)
        {
            stream.WriteByte(value);
        }

        internal void Write(ushort value)
        {
            Write((short)value);
        }

        internal void Write(short value)
        {
            stream.WriteByte((byte)value);
            stream.WriteByte((byte)(value >> 8));
        }

        internal void Write(uint value)
        {
            Write((int)value);
        }

        internal void Write(int value)
        {
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            stream.Write(buffer, 0, 4);
        }

        internal void Write(ulong value)
        {
            Write((long)value);
        }

        internal void Write(long value)
        {
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            buffer[4] = (byte)(value >> 32);
            buffer[5] = (byte)(value >> 40);
            buffer[6] = (byte)(value >> 48);
            buffer[7] = (byte)(value >> 56);
            stream.Write(buffer, 0, 8);
        }

        internal void WriteCompressedUInt(int value)
        {
            if (value <= 0x7F)
            {
                Write((byte)value);
            }
            else if (value <= 0x3FFF)
            {
                Write((byte)(0x80 | (value >> 8)));
                Write((byte)value);
            }
            else
            {
                Write((byte)(0xC0 | (value >> 24)));
                Write((byte)(value >> 16));
                Write((byte)(value >> 8));
                Write((byte)value);
            }
        }

        internal static int GetCompressedUIntLength(int value)
        {
            if (value <= 0x7F)
            {
                return 1;
            }
            else if (value <= 0x3FFF)
            {
                return 2;
            }
            else
            {
                return 4;
            }
        }

        internal void WriteStringIndex(int index)
        {
            if (bigStrings)
            {
                Write(index);
            }
            else
            {
                Write((short)index);
            }
        }

        internal void WriteGuidIndex(int index)
        {
            if (bigGuids)
            {
                Write(index);
            }
            else
            {
                Write((short)index);
            }
        }

        internal void WriteBlobIndex(int index)
        {
            if (bigBlobs)
            {
                Write(index);
            }
            else
            {
                Write((short)index);
            }
        }

        internal void WriteTypeDefOrRef(int token)
        {
            switch (token >> 24)
            {
                case 0:
                    break;
                case TypeDefTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 0;
                    break;
                case TypeRefTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 1;
                    break;
                case TypeSpecTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 2;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            Write(bigTypeDefOrRef ? token : (short)token);
        }

        internal void WriteEncodedTypeDefOrRef(int encodedToken)
        {
            Write(bigTypeDefOrRef ? encodedToken : (short)encodedToken);
        }

        internal void WriteHasCustomAttribute(int token)
        {
            var encodedToken = CustomAttributeTable.EncodeHasCustomAttribute(token);
            Write(bigHasCustomAttribute ? encodedToken : (short)encodedToken);
        }

        internal void WriteCustomAttributeType(int token)
        {
            token = (token >> 24) switch
            {
                MethodDefTable.Index => (token & 0xFFFFFF) << 3 | 2,
                MemberRefTable.Index => (token & 0xFFFFFF) << 3 | 3,
                _ => throw new InvalidOperationException(),
            };

            Write(bigCustomAttributeType ? token : (short)token);
        }

        internal void WriteField(int index)
        {
            Write(bigField ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteMethodDef(int index)
        {
            Write(bigMethodDef ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteParam(int index)
        {
            Write(bigParam ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteTypeDef(int index)
        {
            Write(bigTypeDef ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteEvent(int index)
        {
            Write(bigEvent ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteProperty(int index)
        {
            Write(bigProperty ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteGenericParam(int index)
        {
            Write(bigGenericParam ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteModuleRef(int index)
        {
            Write(bigModuleRef ? (index & 0xFFFFFF) : (short)index);
        }

        internal void WriteResolutionScope(int token)
        {
            token = (token >> 24) switch
            {
                ModuleTable.Index => (token & 0xFFFFFF) << 2 | 0,
                ModuleRefTable.Index => (token & 0xFFFFFF) << 2 | 1,
                AssemblyRefTable.Index => (token & 0xFFFFFF) << 2 | 2,
                TypeRefTable.Index => (token & 0xFFFFFF) << 2 | 3,
                _ => throw new InvalidOperationException(),
            };

            Write(bigResolutionScope ? token : (short)token);
        }

        internal void WriteMemberRefParent(int token)
        {
            token = (token >> 24) switch
            {
                TypeDefTable.Index => (token & 0xFFFFFF) << 3 | 0,
                TypeRefTable.Index => (token & 0xFFFFFF) << 3 | 1,
                ModuleRefTable.Index => (token & 0xFFFFFF) << 3 | 2,
                MethodDefTable.Index => (token & 0xFFFFFF) << 3 | 3,
                TypeSpecTable.Index => (token & 0xFFFFFF) << 3 | 4,
                _ => throw new InvalidOperationException(),
            };

            Write(bigMemberRefParent ? token : (short)token);
        }

        internal void WriteMethodDefOrRef(int token)
        {
            token = (token >> 24) switch
            {
                MethodDefTable.Index => (token & 0xFFFFFF) << 1 | 0,
                MemberRefTable.Index => (token & 0xFFFFFF) << 1 | 1,
                _ => throw new InvalidOperationException(),
            };

            Write(bigMethodDefOrRef ? token : (short)token);
        }

        internal void WriteHasConstant(int token)
        {
            var encodedToken = ConstantTable.EncodeHasConstant(token);
            Write(bigHasConstant ? encodedToken : (short)encodedToken);
        }

        internal void WriteHasSemantics(int encodedToken)
        {
            // because we've already had to do the encoding (to be able to sort the table) here we simple write the value
            Write(bigHasSemantics ? encodedToken : (short)encodedToken);
        }

        internal void WriteImplementation(int token)
        {
            switch (token >> 24)
            {
                case 0:
                    break;
                case FileTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 0;
                    break;
                case AssemblyRefTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 1;
                    break;
                case ExportedTypeTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 2;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            Write(bigImplementation ? token : (short)token);
        }

        internal void WriteTypeOrMethodDef(int encodedToken)
        {
            // because we've already had to do the encoding (to be able to sort the table) here we simple write the value
            Write(bigTypeOrMethodDef ? encodedToken : (short)encodedToken);
        }

        internal void WriteHasDeclSecurity(int encodedToken)
        {
            // because we've already had to do the encoding (to be able to sort the table) here we simple write the value
            Write(bigHasDeclSecurity ? encodedToken : (short)encodedToken);
        }

        internal void WriteMemberForwarded(int token)
        {
            token = (token >> 24) switch
            {
                FieldTable.Index => (token & 0xFFFFFF) << 1 | 0,
                MethodDefTable.Index => (token & 0xFFFFFF) << 1 | 1,
                _ => throw new InvalidOperationException(),
            };
            Write(bigMemberForwarded ? token : (short)token);
        }

        internal void WriteHasFieldMarshal(int token)
        {
            int encodedToken = FieldMarshalTable.EncodeHasFieldMarshal(token);
            Write(bigHasFieldMarshal ? encodedToken : (short)encodedToken);
        }

    }

}
