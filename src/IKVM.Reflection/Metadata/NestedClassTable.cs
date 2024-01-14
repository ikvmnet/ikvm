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
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class NestedClassTable : SortedTable<NestedClassTable.Record>
    {

        internal struct Record : IRecord
        {
            internal int NestedClass;
            internal int EnclosingClass;

            readonly int IRecord.FilterKey => NestedClass;

            public readonly int CompareTo(Record other) => Comparer<int>.Default.Compare(NestedClass, other.NestedClass);

        }

        internal const int Index = 0x29;

        internal override void Read(Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].NestedClass = mr.ReadTypeDef();
                records[i].EnclosingClass = mr.ReadTypeDef();
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
                module.Metadata.AddNestedType(
                    (TypeDefinitionHandle)MetadataTokens.EntityHandle(records[i].NestedClass),
                    (TypeDefinitionHandle)MetadataTokens.EntityHandle(records[i].EnclosingClass));
        }

        internal List<int> GetNestedClasses(int enclosingClass)
        {
            var nestedClasses = new List<int>();
            for (int i = 0; i < rowCount; i++)
                if (records[i].EnclosingClass == enclosingClass)
                    nestedClasses.Add(records[i].NestedClass);

            return nestedClasses;
        }

    }

}
