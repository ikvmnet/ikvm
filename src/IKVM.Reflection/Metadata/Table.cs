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

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Metadata
{

    internal abstract class Table
    {

        internal bool Sorted;

        internal bool IsBig => RowCount > 65535;

        internal abstract int RowCount { get; set; }

        internal abstract void Read(MetadataReader mr);

        internal abstract void Write(ModuleBuilder module);

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

        internal override void Write(ModuleBuilder module)
        {

        }

    }

}
