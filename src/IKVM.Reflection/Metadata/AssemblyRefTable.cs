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

    sealed class AssemblyRefTable : Table<AssemblyRefTable.Record>
    {

        internal struct Record
        {

            internal ushort MajorVersion;
            internal ushort MinorVersion;
            internal ushort BuildNumber;
            internal ushort RevisionNumber;
            internal int Flags;
            internal BlobHandle PublicKeyOrToken;
            internal StringHandle Name;
            internal StringHandle Culture;
            internal BlobHandle HashValue;

        }

        internal const int Index = 0x23;

        internal int FindOrAddRecord(Record rec)
        {
            for (int i = 0; i < rowCount; i++)
            {
                // note that we ignore HashValue here!
                if (records[i].Name == rec.Name &&
                    records[i].MajorVersion == rec.MajorVersion &&
                    records[i].MinorVersion == rec.MinorVersion &&
                    records[i].BuildNumber == rec.BuildNumber &&
                    records[i].RevisionNumber == rec.RevisionNumber &&
                    records[i].Flags == rec.Flags &&
                    records[i].PublicKeyOrToken == rec.PublicKeyOrToken &&
                    records[i].Culture == rec.Culture)
                    return i + 1;
            }

            return AddRecord(rec);
        }

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].MajorVersion = mr.ReadUInt16();
                records[i].MinorVersion = mr.ReadUInt16();
                records[i].BuildNumber = mr.ReadUInt16();
                records[i].RevisionNumber = mr.ReadUInt16();
                records[i].Flags = mr.ReadInt32();
                records[i].PublicKeyOrToken = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
                records[i].Name = MetadataTokens.StringHandle(mr.ReadStringIndex());
                records[i].Culture = MetadataTokens.StringHandle(mr.ReadStringIndex());
                records[i].HashValue = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddAssemblyReference(
                    records[i].Name,
                    new Version(records[i].MajorVersion, records[i].MinorVersion, records[i].BuildNumber, records[i].RevisionNumber),
                    records[i].Culture,
                    records[i].PublicKeyOrToken,
                    (System.Reflection.AssemblyFlags)records[i].Flags,
                    records[i].HashValue);

                Debug.Assert(h == MetadataTokens.AssemblyReferenceHandle(i + 1));
            }
        }

    }

}
