﻿/*
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
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class ExportedTypeTable : Table<ExportedTypeTable.Record>
    {

        internal struct Record
        {

            internal int Flags;
            internal int TypeDefId;
            internal StringHandle TypeName;
            internal StringHandle TypeNamespace;
            internal int Implementation;

        }

        internal const int Index = 0x27;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Flags = mr.ReadInt32();
                records[i].TypeDefId = mr.ReadInt32();
                records[i].TypeName = MetadataTokens.StringHandle(mr.ReadStringIndex());
                records[i].TypeNamespace = MetadataTokens.StringHandle(mr.ReadStringIndex());
                records[i].Implementation = mr.ReadImplementation();
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddExportedType(
                    (System.Reflection.TypeAttributes)records[i].Flags,
                    records[i].TypeNamespace,
                    records[i].TypeName,
                    MetadataTokens.EntityHandle(records[i].Implementation),
                    records[i].TypeDefId);

                Debug.Assert(h == MetadataTokens.ExportedTypeHandle(i + 1));
            }
        }

        internal int FindOrAddRecord(Record rec)
        {
            for (int i = 0; i < rowCount; i++)
            {
                if (records[i].Implementation == rec.Implementation &&
                    records[i].TypeName == rec.TypeName &&
                    records[i].TypeNamespace == rec.TypeNamespace)
                    return i + 1;
            }

            return AddRecord(rec);
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
                moduleBuilder.FixupPseudoToken(ref records[i].Implementation);
        }

    }

}
