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
using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Metadata
{

    sealed class ManifestResourceTable : Table<ManifestResourceTable.Record>
    {

        internal struct Record
        {

            internal int Offset;
            internal int Flags;
            internal StringHandle Name;
            internal int Implementation;

        }

        internal const int Index = 0x28;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Offset = mr.ReadInt32();
                records[i].Flags = mr.ReadInt32();
                records[i].Name = MetadataTokens.StringHandle(mr.ReadStringIndex());
                records[i].Implementation = mr.ReadImplementation();
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddManifestResource(
                    (System.Reflection.ManifestResourceAttributes)records[i].Flags,
                    records[i].Name,
                    MetadataTokens.EntityHandle(records[i].Implementation),
                    (uint)records[i].Offset);

                Debug.Assert(h == MetadataTokens.ManifestResourceHandle(i + 1));
            }
        }

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
                moduleBuilder.FixupPseudoToken(ref records[i].Implementation);
        }

    }

}
