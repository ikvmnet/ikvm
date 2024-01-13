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
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Metadata
{
    sealed class ClassLayoutTable : SortedTable<ClassLayoutTable.Record>
    {

        internal struct Record : IRecord
        {

            internal short PackingSize;
            internal int ClassSize;
            internal int Parent;

            int IRecord.SortKey => Parent;

            int IRecord.FilterKey => Parent;

        }

        internal const int Index = 0x0f;

        internal override void Read(Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].PackingSize = mr.ReadInt16();
                records[i].ClassSize = mr.ReadInt32();
                records[i].Parent = mr.ReadTypeDef();
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            Sort();

            for (int i = 0; i < rowCount; i++)
                module.Metadata.AddTypeLayout(
                    System.Reflection.Metadata.Ecma335.MetadataTokens.TypeDefinitionHandle(records[i].Parent),
                    (ushort)records[i].PackingSize,
                    (uint)records[i].ClassSize);
        }

    }

}
