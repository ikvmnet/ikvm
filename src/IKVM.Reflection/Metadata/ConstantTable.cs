/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Metadata
{

    sealed class ConstantTable : SortedTable<ConstantTable.Record>
    {

        internal struct Record : IRecord
        {

            internal short Type;
            internal int Parent;
            internal BlobHandle Offset;
            internal object Value;

            readonly int IRecord.SortKey => EncodeHasConstant(Parent);

            readonly int IRecord.FilterKey => Parent;

        }

        internal const int Index = 0x0B;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Type = mr.ReadInt16();
                records[i].Parent = mr.ReadHasConstant();
                records[i].Offset = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                // check that blob handle ends up the same
                var b = module.Metadata.GetOrAddConstantBlob(records[i].Value);
                Debug.Assert(b == records[i].Offset);

                // insert constant, and allow reencoding; should use same blob
                var h = module.Metadata.AddConstant(MetadataTokens.EntityHandle(records[i].Parent), records[i].Value);
                Debug.Assert(h == MetadataTokens.ConstantHandle(i + 1));
            }
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
                moduleBuilder.FixupPseudoToken(ref records[i].Parent);

            Sort();
        }

        internal static int EncodeHasConstant(int token) => (token >> 24) switch
        {
            FieldTable.Index => (token & 0xFFFFFF) << 2 | 0,
            ParamTable.Index => (token & 0xFFFFFF) << 2 | 1,
            PropertyTable.Index => (token & 0xFFFFFF) << 2 | 2,
            _ => throw new InvalidOperationException(),
        };

        internal object GetRawConstantValue(Module module, int parent)
        {
            foreach (var i in Filter(parent))
            {
                var br = module.GetBlobReader(module.Constant.records[i].Offset);

                switch (module.Constant.records[i].Type)
                {
                    // see ModuleBuilder.AddConstant for the encodings
                    case Signature.ELEMENT_TYPE_BOOLEAN:
                        return br.ReadByte() != 0;
                    case Signature.ELEMENT_TYPE_I1:
                        return br.ReadSByte();
                    case Signature.ELEMENT_TYPE_I2:
                        return br.ReadInt16();
                    case Signature.ELEMENT_TYPE_I4:
                        return br.ReadInt32();
                    case Signature.ELEMENT_TYPE_I8:
                        return br.ReadInt64();
                    case Signature.ELEMENT_TYPE_U1:
                        return br.ReadByte();
                    case Signature.ELEMENT_TYPE_U2:
                        return br.ReadUInt16();
                    case Signature.ELEMENT_TYPE_U4:
                        return br.ReadUInt32();
                    case Signature.ELEMENT_TYPE_U8:
                        return br.ReadUInt64();
                    case Signature.ELEMENT_TYPE_R4:
                        return br.ReadSingle();
                    case Signature.ELEMENT_TYPE_R8:
                        return br.ReadDouble();
                    case Signature.ELEMENT_TYPE_CHAR:
                        return br.ReadChar();
                    case Signature.ELEMENT_TYPE_STRING:
                        {
                            var chars = new char[br.Length / 2];
                            for (int j = 0; j < chars.Length; j++)
                                chars[j] = br.ReadChar();

                            return new string(chars);
                        }
                    case Signature.ELEMENT_TYPE_CLASS:
                        if (br.ReadInt32() != 0)
                            throw new BadImageFormatException();

                        return null;
                    default:
                        throw new BadImageFormatException();
                }
            }

            throw new InvalidOperationException();
        }

    }

}
