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
using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Metadata
{
    sealed class GenericParamConstraintTable : SortedTable<GenericParamConstraintTable.Record>
    {

        internal struct Record : IRecord
        {

            internal int Owner;
            internal int Constraint;

            readonly int IRecord.SortKey => Owner;

            readonly int IRecord.FilterKey => Owner;

        }

        internal const int Index = 0x2C;

		internal override void Read(MetadataReader mr)
		{
			for (int i = 0; i < records.Length; i++)
			{
				records[i].Owner = mr.ReadGenericParam();
				records[i].Constraint = mr.ReadTypeDefOrRef();
			}
		}

		internal override void Write(MetadataWriter mw)
		{
			for (int i = 0; i < rowCount; i++)
			{
				mw.WriteGenericParam(records[i].Owner);
				mw.WriteTypeDefOrRef(records[i].Constraint);
			}
		}

		protected override int GetRowSize(RowSizeCalc rsc)
		{
			return rsc
				.WriteGenericParam()
				.WriteTypeDefOrRef()
				.Value;
		}

		internal void Fixup(ModuleBuilder moduleBuilder)
		{
			var fixups = moduleBuilder.GenericParam.GetIndexFixup();
			for (int i = 0; i < rowCount; i++)
				records[i].Owner = fixups[records[i].Owner - 1] + 1;

			Sort();
		}
	}
}
