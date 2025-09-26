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
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class MemberRefTable : Table<MemberRefTable.Record>
    {

        internal struct Record
        {

            internal int Class;
            internal StringHandle Name;
            internal BlobHandle Signature;

        }

        internal const int Index = 0x0A;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Class = mr.ReadMemberRefParent();
                records[i].Name = MetadataTokens.StringHandle(mr.ReadStringIndex());
                records[i].Signature = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddMemberReference(
                    MetadataTokens.EntityHandle(records[i].Class),
                    records[i].Name,
                    records[i].Signature);

                Debug.Assert(h == MetadataTokens.MemberReferenceHandle(i + 1));
            }
        }

        /// <summary>
        /// Finds the specified record in the table and returns the row number.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        internal int FindOrAddRecord(Record record)
        {
            for (int i = 0; i < rowCount; i++)
            {
                if (records[i].Class == record.Class &&
                    records[i].Name == record.Name &&
                    records[i].Signature == record.Signature)
                    return i + 1;
            }

            return AddRecord(record);
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
                moduleBuilder.FixupPseudoToken(ref records[i].Class);
        }

    }

}
