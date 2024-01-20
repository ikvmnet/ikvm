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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class LocalScopeTable : SortedTable<LocalScopeTable.Record>
    {

        internal struct Record : IRecord
        {

            internal MethodDefinitionHandle Method;
            internal ImportScopeHandle ImportScope;
            internal LocalVariableHandle VariableList;
            internal LocalConstantHandle ConstantList;
            internal int StartOffset;
            internal int Length;

            readonly int IRecord.FilterKey => MetadataTokens.GetToken(Method);

            readonly int IComparable<Record>.CompareTo(Record other)
            {
                if (Method == other.Method && StartOffset == other.StartOffset)
                    return -Comparer<int>.Default.Compare(Length, other.Length);
                else if (Method == other.Method)
                    return Comparer<int>.Default.Compare(StartOffset, other.StartOffset);
                else
                    return Comparer<int>.Default.Compare(MetadataTokens.GetRowNumber(Method), MetadataTokens.GetRowNumber(other.Method));
            }

        }

        internal const int Index = (int)TableIndex.LocalScope;

        internal override void Read(IKVM.Reflection.Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Method = (MethodDefinitionHandle)MetadataTokens.EntityHandle(mr.ReadInt32());
                records[i].ImportScope = (ImportScopeHandle)MetadataTokens.EntityHandle(mr.ReadInt32());
                records[i].VariableList = (LocalVariableHandle)MetadataTokens.EntityHandle(mr.ReadInt32());
                records[i].ConstantList = (LocalConstantHandle)MetadataTokens.EntityHandle(mr.ReadInt32());
                records[i].StartOffset = mr.ReadInt32();
                records[i].Length = mr.ReadInt32();
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            base.Write(module);

            for (int i = 0; i < rowCount; i++)
            {
                var h = module.Metadata.AddLocalScope(records[i].Method, records[i].ImportScope, records[i].VariableList, records[i].ConstantList, records[i].StartOffset, records[i].Length);
                Debug.Assert(h == MetadataTokens.LocalScopeHandle(i + 1));
            }
        }

    }

}
