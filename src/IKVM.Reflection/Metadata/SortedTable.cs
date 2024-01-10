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
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Metadata
{

    abstract class SortedTable<T> : Table<T>
        where T : SortedTable<T>.IRecord
    {

        internal interface IRecord
        {

            int SortKey { get; }

            int FilterKey { get; }

        }

        internal struct Enumerable
        {

            readonly SortedTable<T> table;
            readonly int token;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="table"></param>
            /// <param name="token"></param>
            internal Enumerable(SortedTable<T> table, int token)
            {
                this.table = table;
                this.token = token;
            }

            public Enumerator GetEnumerator()
            {
                var records = table.records;
                if (!table.Sorted)
                    return new Enumerator(records, table.RowCount - 1, -1, token);

                int index = BinarySearch(records, table.RowCount, token & 0xFFFFFF);
                if (index < 0)
                    return new Enumerator(null, 0, 1, -1);

                int start = index;
                while (start > 0 && (records[start - 1].FilterKey & 0xFFFFFF) == (token & 0xFFFFFF))
                    start--;

                int end = index;
                int max = table.RowCount - 1;
                while (end < max && (records[end + 1].FilterKey & 0xFFFFFF) == (token & 0xFFFFFF))
                    end++;

                return new Enumerator(records, end, start - 1, token);
            }

            static int BinarySearch(T[] records, int length, int maskedToken)
            {
                int min = 0;
                int max = length - 1;
                while (min <= max)
                {
                    int mid = min + ((max - min) / 2);
                    int maskedValue = records[mid].FilterKey & 0xFFFFFF;
                    if (maskedToken == maskedValue)
                        return mid;
                    else if (maskedToken < maskedValue)
                        max = mid - 1;
                    else
                        min = mid + 1;
                }

                return -1;
            }
        }

        internal struct Enumerator
        {

            readonly T[] records;
            readonly int token;
            readonly int max;
            int index;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="records"></param>
            /// <param name="max"></param>
            /// <param name="index"></param>
            /// <param name="token"></param>
            internal Enumerator(T[] records, int max, int index, int token)
            {
                this.records = records;
                this.token = token;
                this.max = max;
                this.index = index;
            }

            public readonly int Current => index;

            public bool MoveNext()
            {
                while (index < max)
                {
                    index++;
                    if (records[index].FilterKey == token)
                        return true;
                }

                return false;
            }
        }

        internal Enumerable Filter(int token)
        {
            return new Enumerable(this, token);
        }

        /// <summary>
        /// Sorts the records in the table.
        /// </summary>
        protected void Sort()
        {
#if NETFRAMEWORK
            HeapSort();
#else
            if (rowCount < 256)
                StackSort();
            else
                HeapSort();
#endif
        }

#if NETFRAMEWORK == false

        /// <summary>
        /// Sorts the rows without allocating a temporary array on the heap.
        /// </summary>
        unsafe void StackSort()
        {
            var map = (Span<ulong>)stackalloc ulong[rowCount];
            for (int i = 0; i < rowCount; i++)
                map[i] = ((ulong)records[i].SortKey << 32) | (uint)i;

            map.Sort(Comparer<ulong>.Default);
            var newRecords = new T[rowCount];
            for (int i = 0; i < rowCount; i++)
                newRecords[i] = records[(int)map[i]];

            records = newRecords;
            return;
        }

#endif

        /// <summary>
        /// Sorts the rows while allocating a temporary array on the heap.
        /// </summary>
        void HeapSort()
        {
            var map = new ulong[rowCount];
            for (uint i = 0; i < map.Length; i++)
                map[i] = ((ulong)records[i].SortKey << 32) | i;

            Array.Sort(map, Comparer<ulong>.Default);
            var newRecords = new T[rowCount];
            for (int i = 0; i < map.Length; i++)
                newRecords[i] = records[(int)map[i]];

            records = newRecords;
            return;
        }

    }

}
