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
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Metadata
{
    sealed class AssemblyTable : Table<AssemblyTable.Record>
    {

        internal struct Record
        {
            internal int HashAlgId;
            internal ushort MajorVersion;
            internal ushort MinorVersion;
            internal ushort BuildNumber;
            internal ushort RevisionNumber;
            internal int Flags;
            internal int PublicKey;
            internal int Name;
            internal int Culture;
        }

        internal const int Index = 0x20;

        internal override void Read(MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].HashAlgId = mr.ReadInt32();
                records[i].MajorVersion = mr.ReadUInt16();
                records[i].MinorVersion = mr.ReadUInt16();
                records[i].BuildNumber = mr.ReadUInt16();
                records[i].RevisionNumber = mr.ReadUInt16();
                records[i].Flags = mr.ReadInt32();
                records[i].PublicKey = mr.ReadBlobIndex();
                records[i].Name = mr.ReadStringIndex();
                records[i].Culture = mr.ReadStringIndex();
            }
        }

        internal override void Write(MetadataWriter mw)
        {
            for (int i = 0; i < rowCount; i++)
            {
                mw.Write(records[i].HashAlgId);
                mw.Write(records[i].MajorVersion);
                mw.Write(records[i].MinorVersion);
                mw.Write(records[i].BuildNumber);
                mw.Write(records[i].RevisionNumber);
                mw.Write(records[i].Flags);
                mw.WriteBlobIndex(records[i].PublicKey);
                mw.WriteStringIndex(records[i].Name);
                mw.WriteStringIndex(records[i].Culture);
            }
        }

        protected override int GetRowSize(RowSizeCalc rsc)
        {
            return rsc
                .AddFixed(16)
                .WriteBlobIndex()
                .WriteStringIndex()
                .WriteStringIndex()
                .Value;
        }

    }

}
