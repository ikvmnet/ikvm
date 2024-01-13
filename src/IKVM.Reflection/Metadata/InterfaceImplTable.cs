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
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class InterfaceImplTable : SortedTable<InterfaceImplTable.Record>
    {

        internal struct Record : IRecord
        {

            internal int Class;
            internal int Interface;

            readonly int IRecord.SortKey => EncodeOwner(Class);

            readonly int IRecord.FilterKey => Class;
        }

        internal const int Index = (int)TableIndex.InterfaceImpl;

        internal override void Read(Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Class = mr.ReadTypeDef();
                records[i].Interface = mr.ReadTypeDefOrRef();
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddInterfaceImplementation(
                    MetadataTokens.TypeDefinitionHandle(records[i].Class),
                    MetadataTokens.EntityHandle(records[i].Interface));

                Debug.Assert(h == MetadataTokens.InterfaceImplementationHandle(i + 1));
            }
        }

        internal static int EncodeOwner(int token) => (token >> 24) switch
        {
            0 => 0,
            TypeDefTable.Index => (token & 0xFFFFFF) << 2 | 0,
            TypeRefTable.Index => (token & 0xFFFFFF) << 2 | 1,
            TypeSpecTable.Index => (token & 0xFFFFFF) << 2 | 2,
            _ => throw new InvalidOperationException(),
        };

        internal void Fixup()
        {
            // LAMESPEC the CLI spec says that InterfaceImpl should be sorted by { Class, Interface },
            // but it appears to only be necessary to sort by Class (and csc emits InterfaceImpl records in
            // source file order, so to be able to support round tripping, we need to retain ordering as well).
            Sort();
        }

    }

}
