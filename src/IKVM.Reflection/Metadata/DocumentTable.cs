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

    sealed class DocumentTable : Table<DocumentTable.Record>
    {

        internal struct Record
        {

            internal BlobHandle Name;
            internal GuidHandle HashAlgorithm;
            internal BlobHandle Hash;
            internal GuidHandle Language;

        }

        internal const int Index = (int)TableIndex.MethodDebugInformation;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Name = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
                records[i].HashAlgorithm = MetadataTokens.GuidHandle(mr.ReadGuidIndex());
                records[i].Hash = MetadataTokens.BlobHandle(mr.ReadBlobIndex());
                records[i].Language = MetadataTokens.GuidHandle(mr.ReadGuidIndex());
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            base.Write(module);

            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddDocument(records[i].Name, records[i].HashAlgorithm, records[i].Hash, records[i].Language);
                Debug.Assert(h == MetadataTokens.DocumentHandle(i + 1));
            }
        }

    }

}
