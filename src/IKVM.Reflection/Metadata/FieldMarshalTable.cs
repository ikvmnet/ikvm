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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class FieldMarshalTable : SortedTable<FieldMarshalTable.Record>
    {

        internal struct Record : IRecord
        {

            internal int Parent;
            internal BlobHandle NativeType;

            readonly int IRecord.FilterKey => Parent;

            public readonly int CompareTo(Record other) => Comparer<int>.Default.Compare(EncodeHasFieldMarshal(Parent), EncodeHasFieldMarshal(other.Parent));

        }

        internal const int Index = 0x0D;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Parent = mr.ReadHasFieldMarshal();
                records[i].NativeType = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
                module.Metadata.AddMarshallingDescriptor(
                    MetadataTokens.EntityHandle(records[i].Parent),
                    records[i].NativeType);
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
                records[i].Parent = moduleBuilder.ResolvePseudoToken(records[i].Parent);

            Sort();
        }

        internal static int EncodeHasFieldMarshal(int token)
        {
            return (token >> 24) switch
            {
                FieldTable.Index => (token & 0xFFFFFF) << 1 | 0,
                ParamTable.Index => (token & 0xFFFFFF) << 1 | 1,
                _ => throw new InvalidOperationException(),
            };
        }

    }

}
