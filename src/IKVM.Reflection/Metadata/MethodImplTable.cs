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

    sealed class MethodImplTable : SortedTable<MethodImplTable.Record>
    {

        internal struct Record : IRecord
        {

            internal int Class;
            internal int MethodBody;
            internal int MethodDeclaration;

            readonly int IRecord.SortKey => Class;

            readonly int IRecord.FilterKey => Class;

        }

        internal const int Index = 0x19;

		internal override void Read(MetadataReader mr)
		{
			for (int i = 0; i < records.Length; i++)
			{
				records[i].Class = mr.ReadTypeDef();
				records[i].MethodBody = mr.ReadMethodDefOrRef();
				records[i].MethodDeclaration = mr.ReadMethodDefOrRef();
			}
		}

		internal override void Write(MetadataWriter mw)
		{
			for (int i = 0; i < rowCount; i++)
			{
				mw.WriteTypeDef(records[i].Class);
				mw.WriteMethodDefOrRef(records[i].MethodBody);
				mw.WriteMethodDefOrRef(records[i].MethodDeclaration);
			}
		}

		protected override int GetRowSize(RowSizeCalc rsc)
		{
			return rsc
				.WriteTypeDef()
				.WriteMethodDefOrRef()
				.WriteMethodDefOrRef()
				.Value;
		}

		internal void Fixup(ModuleBuilder moduleBuilder)
		{
			for (int i = 0; i < rowCount; i++)
			{
				moduleBuilder.FixupPseudoToken(ref records[i].MethodBody);
				moduleBuilder.FixupPseudoToken(ref records[i].MethodDeclaration);
			}

			Sort();
		}
	}

}
