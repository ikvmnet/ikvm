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

namespace IKVM.Reflection.Metadata
{
    sealed class CustomAttributeTable : SortedTable<CustomAttributeTable.Record>
    {

        internal struct Record : IRecord
        {

            internal int Parent;
            internal int Constructor;
            internal BlobHandle Value;

            readonly int IRecord.SortKey => EncodeHasCustomAttribute(Parent);

            readonly int IRecord.FilterKey => Parent;

        }

        internal const int Index = 0x0C;


        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Parent = mr.ReadHasCustomAttribute();
                records[i].Constructor = mr.ReadCustomAttributeType();
                records[i].Value = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddCustomAttribute(
                    MetadataTokens.EntityHandle(records[i].Parent),
                    MetadataTokens.EntityHandle(records[i].Constructor),
                    records[i].Value);

                Debug.Assert(h == MetadataTokens.CustomAttributeHandle(i));
            }
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            var genericParamFixup = moduleBuilder.GenericParam.GetIndexFixup();

            for (int i = 0; i < rowCount; i++)
            {
                moduleBuilder.FixupPseudoToken(ref records[i].Constructor);
                moduleBuilder.FixupPseudoToken(ref records[i].Parent);
                if (records[i].Parent >> 24 == GenericParamTable.Index)
                    records[i].Parent = (GenericParamTable.Index << 24) + genericParamFixup[(records[i].Parent & 0xFFFFFF) - 1] + 1;

                // TODO if we ever add support for custom attributes on DeclSecurity or GenericParamConstraint
                // we need to fix them up here (because they are sorted tables, like GenericParam)
            }

            Sort();
        }

        internal static int EncodeHasCustomAttribute(int token)
        {
            return (token >> 24) switch
            {
                MethodDefTable.Index => (token & 0xFFFFFF) << 5 | 0,
                FieldTable.Index => (token & 0xFFFFFF) << 5 | 1,
                TypeRefTable.Index => (token & 0xFFFFFF) << 5 | 2,
                TypeDefTable.Index => (token & 0xFFFFFF) << 5 | 3,
                ParamTable.Index => (token & 0xFFFFFF) << 5 | 4,
                InterfaceImplTable.Index => (token & 0xFFFFFF) << 5 | 5,
                MemberRefTable.Index => (token & 0xFFFFFF) << 5 | 6,
                ModuleTable.Index => (token & 0xFFFFFF) << 5 | 7,
                // LAMESPEC spec calls this Permission table
                DeclSecurityTable.Index => throw new NotImplementedException(), //return (token & 0xFFFFFF) << 5 | 8;
                PropertyTable.Index => (token & 0xFFFFFF) << 5 | 9,
                EventTable.Index => (token & 0xFFFFFF) << 5 | 10,
                StandAloneSigTable.Index => (token & 0xFFFFFF) << 5 | 11,
                ModuleRefTable.Index => (token & 0xFFFFFF) << 5 | 12,
                TypeSpecTable.Index => (token & 0xFFFFFF) << 5 | 13,
                AssemblyTable.Index => (token & 0xFFFFFF) << 5 | 14,
                AssemblyRefTable.Index => (token & 0xFFFFFF) << 5 | 15,
                FileTable.Index => (token & 0xFFFFFF) << 5 | 16,
                ExportedTypeTable.Index => (token & 0xFFFFFF) << 5 | 17,
                ManifestResourceTable.Index => (token & 0xFFFFFF) << 5 | 18,
                GenericParamTable.Index => (token & 0xFFFFFF) << 5 | 19,
                GenericParamConstraintTable.Index => throw new NotImplementedException(), //return (token & 0xFFFFFF) << 5 | 20;
                MethodSpecTable.Index => (token & 0xFFFFFF) << 5 | 21,
                _ => throw new InvalidOperationException(),
            };
        }

    }

}
