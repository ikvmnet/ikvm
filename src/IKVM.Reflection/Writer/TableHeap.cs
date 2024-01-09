/*
  Copyright (C) 2008 Jeroen Frijters

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

namespace IKVM.Reflection.Writer
{

    sealed class TableHeap : Heap
    {

        internal void Freeze(MetadataWriter mw)
        {
            if (frozen)
                throw new InvalidOperationException();

            frozen = true;
            unalignedlength = GetLength(mw);
        }

        protected override void WriteImpl(MetadataWriter mw)
        {
            var tables = mw.ModuleBuilder.GetTables();

            // Header
            mw.Write(0);                    // Reserved

            int ver = mw.ModuleBuilder.MDStreamVersion;
            mw.Write((byte)(ver >> 16));    // MajorVersion
            mw.Write((byte)ver);            // MinorVersion

            byte heapSizes = 0;
            if (mw.ModuleBuilder.Strings.IsBig)
                heapSizes |= 0x01;
            if (mw.ModuleBuilder.Guids.IsBig)
                heapSizes |= 0x02;
            if (mw.ModuleBuilder.Blobs.IsBig)
                heapSizes |= 0x04;
            mw.Write(heapSizes);            // HeapSizes

            // LAMESPEC spec says reserved, but .NET 2.0 Ref.Emit sets it to 0x10
            mw.Write((byte)0x10);           // Reserved

            long bit = 1;
            long valid = 0;
            foreach (var table in tables)
            {
                if (table != null && table.RowCount > 0)
                    valid |= bit;

                bit <<= 1;
            }

            mw.Write(valid);    // Valid
            mw.Write(0x0016003301FA00L);    // Sorted

            // Rows
            foreach (var table in tables)
                if (table != null && table.RowCount > 0)
                    mw.Write(table.RowCount);

            // Tables
            foreach (var table in tables)
            {
                if (table != null && table.RowCount > 0)
                {
                    var pos = mw.Position;
                    table.Write(mw);
                    Debug.Assert(mw.Position - pos == table.GetLength(mw));
                }
            }

            // unexplained extra padding
            mw.Write((byte)0);
        }

        static int GetLength(MetadataWriter mw)
        {
            int len = 4 + 4 + 8 + 8;

            foreach (var table in mw.ModuleBuilder.GetTables())
            {
                if (table != null && table.RowCount > 0)
                {
                    len += 4; // row count
                    len += table.GetLength(mw);
                }
            }

            // note that we pad one extra (unexplained) byte
            return len + 1;
        }

    }

}
