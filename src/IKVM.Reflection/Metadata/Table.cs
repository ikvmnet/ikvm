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

using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Metadata
{

    internal abstract class Table
    {

        protected sealed class RowSizeCalc
        {

            readonly MetadataWriter mw;
            int size;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="mw"></param>
            internal RowSizeCalc(MetadataWriter mw)
            {
                this.mw = mw;
            }

            internal RowSizeCalc AddFixed(int size)
            {
                this.size += size;
                return this;
            }

            internal RowSizeCalc WriteStringIndex()
            {
                size += mw.bigStrings ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteGuidIndex()
            {
                size += mw.bigGuids ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteBlobIndex()
            {
                size += mw.bigBlobs ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteTypeDefOrRef()
            {
                size += mw.bigTypeDefOrRef ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteField()
            {
                size += mw.bigField ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteMethodDef()
            {
                size += mw.bigMethodDef ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteParam()
            {
                size += mw.bigParam ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteResolutionScope()
            {
                size += mw.bigResolutionScope ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteMemberRefParent()
            {
                size += mw.bigMemberRefParent ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteHasCustomAttribute()
            {
                size += mw.bigHasCustomAttribute ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteCustomAttributeType()
            {
                size += mw.bigCustomAttributeType ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteHasConstant()
            {
                size += mw.bigHasConstant ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteTypeDef()
            {
                size += mw.bigTypeDef ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteMethodDefOrRef()
            {
                size += mw.bigMethodDefOrRef ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteEvent()
            {
                size += mw.bigEvent ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteProperty()
            {
                size += mw.bigProperty ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteHasSemantics()
            {
                size += mw.bigHasSemantics ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteImplementation()
            {
                size += mw.bigImplementation ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteTypeOrMethodDef()
            {
                size += mw.bigTypeOrMethodDef ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteGenericParam()
            {
                size += mw.bigGenericParam ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteHasDeclSecurity()
            {
                size += mw.bigHasDeclSecurity ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteMemberForwarded()
            {
                size += mw.bigMemberForwarded ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteModuleRef()
            {
                size += mw.bigModuleRef ? 4 : 2;
                return this;
            }

            internal RowSizeCalc WriteHasFieldMarshal()
            {
                size += mw.bigHasFieldMarshal ? 4 : 2;
                return this;
            }

            internal int Value
            {
                get { return size; }
            }

        }

        internal bool Sorted;

        internal bool IsBig => RowCount > 65535;

        internal abstract int RowCount { get; set; }

        internal abstract void Write(MetadataWriter mw);

        internal abstract void Read(MetadataReader mr);

        internal int GetLength(MetadataWriter md) => RowCount * GetRowSize(new RowSizeCalc(md));

        protected abstract int GetRowSize(RowSizeCalc rsc);

    }

    abstract class Table<T> : Table
    {

        internal T[] records = Array.Empty<T>();
        protected int rowCount;

        internal sealed override int RowCount
        {
            get => rowCount;
            set { rowCount = value; records = new T[value]; }
        }

        protected override int GetRowSize(RowSizeCalc rsc)
        {
            throw new InvalidOperationException();
        }

        internal int AddRecord(T newRecord)
        {
            if (rowCount == records.Length)
                Array.Resize(ref records, Math.Max(16, records.Length * 2));

            records[rowCount++] = newRecord;
            return rowCount;
        }

        internal int AddVirtualRecord()
        {
            return ++rowCount;
        }

        internal override void Write(MetadataWriter mw)
        {
            throw new InvalidOperationException();
        }

    }

}
