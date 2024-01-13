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
    sealed class DeclSecurityTable : SortedTable<DeclSecurityTable.Record>
    {

        internal struct Record : IRecord
        {

            internal short Action;
            internal int Parent;
            internal BlobHandle PermissionSet;

            readonly int IRecord.SortKey => Parent;

            readonly int IRecord.FilterKey => Parent;

        }

        internal const int Index = 0x0E;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Action = mr.ReadInt16();
                records[i].Parent = mr.ReadHasDeclSecurity();
                records[i].PermissionSet = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddDeclarativeSecurityAttribute(
                    MetadataTokens.EntityHandle(records[i].Parent),
                    (System.Reflection.DeclarativeSecurityAction)records[i].Action,
                    records[i].PermissionSet);

                Debug.Assert(h == MetadataTokens.DeclarativeSecurityAttributeHandle(i + 1));
            }
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var token = records[i].Parent;
                moduleBuilder.FixupPseudoToken(ref token);

                // do the HasDeclSecurity encoding, so that we can sort the table
                token = (token >> 24) switch
                {
                    TypeDefTable.Index => (token & 0xFFFFFF) << 2 | 0,
                    MethodDefTable.Index => (token & 0xFFFFFF) << 2 | 1,
                    AssemblyTable.Index => (token & 0xFFFFFF) << 2 | 2,
                    _ => throw new InvalidOperationException(),
                };

                records[i].Parent = token;
            }

            Sort();
        }

    }

}
