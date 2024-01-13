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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class GenericParamTable : SortedTable<GenericParamTable.Record>, IComparer<GenericParamTable.Record>
    {

        internal struct Record : IRecord
        {

            internal short Number;
            internal short Flags;
            internal int Owner;
            internal StringHandle Name;

            // not part of the table, we use it to be able to fixup the GenericParamConstraint table
            internal int unsortedIndex;

            readonly int IRecord.SortKey => EncodeOwner(Owner);

            readonly int IRecord.FilterKey => Owner;

        }

        internal const int Index = 0x2A;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Number = mr.ReadInt16();
                records[i].Flags = mr.ReadInt16();
                records[i].Owner = mr.ReadTypeOrMethodDef();
                records[i].Name = MetadataTokens.StringHandle(mr.ReadStringIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddGenericParameter(
                    MetadataTokens.EntityHandle(records[i].Owner),
                    (System.Reflection.GenericParameterAttributes)records[i].Flags,
                    records[i].Name,
                    records[i].Number);

                Debug.Assert(h == MetadataTokens.GenericParameterHandle(i + 1));
            }
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
            {
                moduleBuilder.FixupPseudoToken(ref records[i].Owner);
                records[i].unsortedIndex = i;
            }

            Sort();
        }

        internal static int EncodeOwner(int token) => (token >> 24) switch
        {
            TypeDefTable.Index => (token & 0xFFFFFF) << 1 | 0,
            MethodDefTable.Index => (token & 0xFFFFFF) << 1 | 1,
            _ => throw new InvalidOperationException(),
        };

        int IComparer<Record>.Compare(Record x, Record y)
        {
            if (x.Owner == y.Owner)
                return x.Number == y.Number ? 0 : (x.Number > y.Number ? 1 : -1);

            return x.Owner > y.Owner ? 1 : -1;
        }

        internal void PatchAttribute(int token, GenericParameterAttributes genericParameterAttributes)
        {
            records[(token & 0xFFFFFF) - 1].Flags = (short)genericParameterAttributes;
        }

        internal int[] GetIndexFixup()
        {
            var array = new int[rowCount];
            for (int i = 0; i < rowCount; i++)
                array[records[i].unsortedIndex] = i;

            return array;
        }

        internal int FindFirstByOwner(int token)
        {
            foreach (int i in Filter(token))
                return i;

            return -1;
        }

    }

}
