/*
  Copyright (C) 2010-2012 Jeroen Frijters

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
using System.Linq;

namespace IKVM.Reflection.Writer
{

    sealed class ResourceDirectoryEntry
    {

        internal readonly OrdinalOrName ordinalOrName;
        internal readonly List<ResourceDirectoryEntry> entries = new();
        internal int namedEntries;

        internal ByteBuffer data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ordinalOrName"></param>
        internal ResourceDirectoryEntry(OrdinalOrName ordinalOrName)
        {
            this.ordinalOrName = ordinalOrName;
        }

        /// <summary>
        /// Initializes a new instance as a copy of the specified instance.
        /// </summary>
        /// <param name="id"></param>
        internal ResourceDirectoryEntry(ResourceDirectoryEntry other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            this.ordinalOrName = other.ordinalOrName;
            this.entries = other.entries.Select(i => new ResourceDirectoryEntry(i)).ToList();
            this.namedEntries = other.namedEntries;
        }

        /// <summary>
        /// Gets the directory entry specified by the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal ResourceDirectoryEntry this[OrdinalOrName id]
        {
            get
            {
                foreach (var entry in entries)
                    if (entry.ordinalOrName.Equals(id))
                        return entry;

                // the entries must be sorted
                var newEntry = new ResourceDirectoryEntry(id);
                if (id.Name == null)
                {
                    for (var i = namedEntries; i < entries.Count; i++)
                    {
                        if (entries[i].ordinalOrName.IsGreaterThan(id))
                        {
                            entries.Insert(i, newEntry);
                            return newEntry;
                        }
                    }

                    entries.Add(newEntry);
                    return newEntry;
                }
                else
                {
                    for (int i = 0; i < namedEntries; i++)
                    {
                        if (entries[i].ordinalOrName.IsGreaterThan(id))
                        {
                            entries.Insert(i, newEntry);
                            namedEntries++;
                            return newEntry;
                        }
                    }

                    entries.Insert(namedEntries++, newEntry);
                    return newEntry;
                }
            }
        }

        int DirectoryLength
        {
            get
            {
                if (data != null)
                {
                    return 16;
                }
                else
                {
                    var length = 16 + entries.Count * 8;
                    foreach (var entry in entries)
                        length += entry.DirectoryLength;

                    return length;
                }
            }
        }

        internal void Write(ByteBuffer bb, List<int> linkOffsets)
        {
            if (entries.Count != 0)
            {
                var stringTableOffset = DirectoryLength;
                var strings = new Dictionary<string, int>();
                var stringTable = new ByteBuffer(16);
                int offset = 16 + entries.Count * 8;
                for (int pass = 0; pass < 3; pass++)
                    Write(bb, pass, 0, ref offset, strings, ref stringTableOffset, stringTable);

                // the pecoff spec says that the string table is between the directory entries and the data entries,
                // but the windows linker puts them after the data entries, so we do too.
                stringTable.Align(4);
                offset += stringTable.Length;
                WriteResourceDataEntries(bb, linkOffsets, ref offset);
                bb.Write(stringTable);
                WriteData(bb);
            }
        }

        void WriteResourceDataEntries(ByteBuffer bb, List<int> linkOffsets, ref int offset)
        {
            foreach (var entry in entries)
            {
                if (entry.data != null)
                {
                    linkOffsets.Add(bb.Position);
                    bb.Write(offset);
                    bb.Write(entry.data.Length);
                    bb.Write(0);    // code page
                    bb.Write(0);    // reserved
                    offset += (entry.data.Length + 3) & ~3;
                }
                else
                {
                    entry.WriteResourceDataEntries(bb, linkOffsets, ref offset);
                }
            }
        }

        void WriteData(ByteBuffer bb)
        {
            foreach (var entry in entries)
            {
                if (entry.data != null)
                {
                    bb.Write(entry.data);
                    bb.Align(4);
                }
                else
                {
                    entry.WriteData(bb);
                }
            }
        }

        void Write(ByteBuffer bb, int writeDepth, int currentDepth, ref int offset, Dictionary<string, int> strings, ref int stringTableOffset, ByteBuffer stringTable)
        {
            if (currentDepth == writeDepth)
            {
                // directory header
                bb.Write(0);    // Characteristics
                bb.Write(0);    // Time/Date Stamp
                bb.Write(0);    // Version (Major / Minor)
                bb.Write((ushort)namedEntries);
                bb.Write((ushort)(entries.Count - namedEntries));
            }

            foreach (var entry in entries)
            {
                if (currentDepth == writeDepth)
                    entry.WriteEntry(bb, ref offset, strings, ref stringTableOffset, stringTable);
                else
                    entry.Write(bb, writeDepth, currentDepth + 1, ref offset, strings, ref stringTableOffset, stringTable);
            }
        }

        void WriteEntry(ByteBuffer bb, ref int offset, Dictionary<string, int> strings, ref int stringTableOffset, ByteBuffer stringTable)
        {
            WriteNameOrOrdinal(bb, ordinalOrName, strings, ref stringTableOffset, stringTable);
            if (data == null)
                bb.Write(0x80000000U | (uint)offset);
            else
                bb.Write(offset);

            offset += 16 + entries.Count * 8;
        }

        static void WriteNameOrOrdinal(ByteBuffer bb, OrdinalOrName id, Dictionary<string, int> strings, ref int stringTableOffset, ByteBuffer stringTable)
        {
            if (id.Name == null)
            {
                bb.Write((int)id.Ordinal);
            }
            else
            {
                if (strings.TryGetValue(id.Name, out var stringOffset) == false)
                {
                    stringOffset = stringTableOffset;
                    strings.Add(id.Name, stringOffset);
                    stringTableOffset += id.Name.Length * 2 + 2;
                    stringTable.Write((ushort)id.Name.Length);
                    foreach (var c in id.Name)
                        stringTable.Write((short)c);
                }

                bb.Write(0x80000000U | (uint)stringOffset);
            }
        }

    }

}
