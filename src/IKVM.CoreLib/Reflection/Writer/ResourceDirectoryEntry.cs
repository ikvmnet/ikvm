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

    /// <summary>
    /// A node in the tree of unmanaged (Win32) resources of an image: either a directory of child entries or, when
    /// <see cref="Data"/> is set, a leaf that holds the data of a resource.
    /// </summary>
    sealed class ResourceDirectoryEntry
    {

        readonly OrdinalOrName _ordinalOrName;

        /// <summary>
        /// The child entries, kept sorted with the named entries first, followed by the entries identified by ordinal.
        /// </summary>
        readonly List<ResourceDirectoryEntry> _entries = [];

        /// <summary>
        /// The number of entries at the start of <see cref="_entries"/> that are identified by name.
        /// </summary>
        int _namedEntries;

        /// <summary>
        /// The data of the resource, or <see langword="null"/> if this entry is a directory.
        /// </summary>
        internal ByteBuffer Data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ordinalOrName">The ordinal or name that identifies the entry within its parent directory.</param>
        internal ResourceDirectoryEntry(OrdinalOrName ordinalOrName)
        {
            _ordinalOrName = ordinalOrName;
        }

        /// <summary>
        /// Gets the number of entries added.
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        /// Gets the child entry specified by the given ID, adding it in sorted position if it does not exist yet.
        /// </summary>
        /// <param name="id">The ordinal or name of the child entry.</param>
        /// <returns>The existing or newly added child entry.</returns>
        internal ResourceDirectoryEntry this[OrdinalOrName id]
        {
            get
            {
                foreach (var entry in _entries)
                    if (entry._ordinalOrName.Equals(id))
                        return entry;

                // the entries must be sorted
                var newEntry = new ResourceDirectoryEntry(id);
                if (id.Name == null)
                {
                    for (var i = _namedEntries; i < _entries.Count; i++)
                    {
                        if (_entries[i]._ordinalOrName.IsGreaterThan(id))
                        {
                            _entries.Insert(i, newEntry);
                            return newEntry;
                        }
                    }

                    _entries.Add(newEntry);
                    return newEntry;
                }
                else
                {
                    for (var i = 0; i < _namedEntries; i++)
                    {
                        if (_entries[i]._ordinalOrName.IsGreaterThan(id))
                        {
                            _entries.Insert(i, newEntry);
                            _namedEntries++;
                            return newEntry;
                        }
                    }

                    _entries.Insert(_namedEntries++, newEntry);
                    return newEntry;
                }
            }
        }

        /// <summary>
        /// Gets the number of bytes this entry and its descendants occupy in the directory tables: 16 for a data entry,
        /// otherwise a 16 byte directory header and 8 bytes per child, plus the children's own lengths.
        /// </summary>
        int DirectoryLength
        {
            get
            {
                if (Data != null)
                {
                    return 16;
                }
                else
                {
                    var length = 16 + _entries.Count * 8;
                    foreach (var entry in _entries)
                        length += entry.DirectoryLength;

                    return length;
                }
            }
        }

        /// <summary>
        /// Writes the resource tree rooted at this entry: the directory tables of the three levels (type, name and
        /// language), the data entries, the string table and the resource data. Data offsets are written relative to
        /// the start of the section.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        /// <param name="linkOffsets">Receives the position of each data entry's offset field, which the caller must
        /// relocate by adding the section's relative virtual address.</param>
        internal void Write(ByteBuffer bb, List<int> linkOffsets)
        {
            if (_entries.Count != 0)
            {
                var stringTableOffset = DirectoryLength;
                var strings = new Dictionary<string, int>();
                var stringTable = new ByteBuffer(16);
                var offset = 16 + _entries.Count * 8;
                for (var pass = 0; pass < 3; pass++)
                    Write(bb, pass, 0, ref offset, strings, ref stringTableOffset, stringTable);

                // the pecoff spec says that the string table is between the directory entries and the data entries,
                // but the windows linker puts them after the data entries, so we do too.
                stringTable.Align(4);
                offset += stringTable.Length;
                WriteResourceDataEntries(bb, linkOffsets, ref offset);
                bb.WriteBuffer(stringTable);
                WriteData(bb);
            }
        }

        /// <summary>
        /// Writes a data entry for every leaf below this entry, in tree order.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        /// <param name="linkOffsets">Receives the position of each data entry's offset field.</param>
        /// <param name="offset">The offset at which the next resource's data will be written; advanced past each
        /// resource, rounded up to a multiple of four.</param>
        void WriteResourceDataEntries(ByteBuffer bb, List<int> linkOffsets, ref int offset)
        {
            foreach (var entry in _entries)
            {
                if (entry.Data != null)
                {
                    linkOffsets.Add(bb.Position);
                    bb.WriteInt32(offset);
                    bb.WriteInt32(entry.Data.Length);
                    bb.WriteInt32(0);    // code page
                    bb.WriteInt32(0);    // reserved
                    offset += (entry.Data.Length + 3) & ~3;
                }
                else
                {
                    entry.WriteResourceDataEntries(bb, linkOffsets, ref offset);
                }
            }
        }

        /// <summary>
        /// Writes the data of every leaf below this entry, in tree order, each aligned to four bytes.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        void WriteData(ByteBuffer bb)
        {
            foreach (var entry in _entries)
            {
                if (entry.Data != null)
                {
                    bb.WriteBuffer(entry.Data);
                    bb.Align(4);
                }
                else
                {
                    entry.WriteData(bb);
                }
            }
        }

        /// <summary>
        /// Writes the directory tables at one level of the tree: when <paramref name="currentDepth"/> equals
        /// <paramref name="writeDepth"/>, the directory header of this entry and the entries of its children, otherwise
        /// the same for each child one level further down.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        /// <param name="writeDepth">The depth of the level being written.</param>
        /// <param name="currentDepth">The depth of this entry.</param>
        /// <param name="offset">The offset at which the next directory or data entry will be placed; advanced past each
        /// entry written.</param>
        /// <param name="strings">The names already added to the string table, with their offsets.</param>
        /// <param name="stringTableOffset">The offset at which the next string will be placed in the string table.</param>
        /// <param name="stringTable">The buffer that collects the string table.</param>
        void Write(ByteBuffer bb, int writeDepth, int currentDepth, ref int offset, Dictionary<string, int> strings, ref int stringTableOffset, ByteBuffer stringTable)
        {
            if (currentDepth == writeDepth)
            {
                // directory header
                bb.WriteInt32(0);    // Characteristics
                bb.WriteInt32(0);    // Time/Date Stamp
                bb.WriteInt32(0);    // Version (Major / Minor)
                bb.WriteUInt16((ushort)_namedEntries);
                bb.WriteUInt16((ushort)(_entries.Count - _namedEntries));
            }

            foreach (var entry in _entries)
            {
                if (currentDepth == writeDepth)
                    entry.WriteEntry(bb, ref offset, strings, ref stringTableOffset, stringTable);
                else
                    entry.Write(bb, writeDepth, currentDepth + 1, ref offset, strings, ref stringTableOffset, stringTable);
            }
        }

        /// <summary>
        /// Writes the directory entry that points to this entry from its parent: its name or ordinal, followed by the
        /// offset of its subdirectory (with the high bit set) or of its data entry.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        /// <param name="offset">The offset of this entry's subdirectory or data entry; advanced past it.</param>
        /// <param name="strings">The names already added to the string table, with their offsets.</param>
        /// <param name="stringTableOffset">The offset at which the next string will be placed in the string table.</param>
        /// <param name="stringTable">The buffer that collects the string table.</param>
        void WriteEntry(ByteBuffer bb, ref int offset, Dictionary<string, int> strings, ref int stringTableOffset, ByteBuffer stringTable)
        {
            WriteNameOrOrdinal(bb, _ordinalOrName, strings, ref stringTableOffset, stringTable);
            if (Data == null)
                bb.WriteUInt32(0x80000000U | (uint)offset);
            else
                bb.WriteInt32(offset);

            offset += 16 + _entries.Count * 8;
        }

        /// <summary>
        /// Writes the name or ordinal field of a directory entry. A name is added to the string table once, as a length
        /// prefixed UTF-16 string, and referenced by its offset with the high bit set.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        /// <param name="id">The ordinal or name to write.</param>
        /// <param name="strings">The names already added to the string table, with their offsets.</param>
        /// <param name="stringTableOffset">The offset at which the next string will be placed in the string table.</param>
        /// <param name="stringTable">The buffer that collects the string table.</param>
        static void WriteNameOrOrdinal(ByteBuffer bb, OrdinalOrName id, Dictionary<string, int> strings, ref int stringTableOffset, ByteBuffer stringTable)
        {
            if (id.Name == null)
            {
                bb.WriteInt32((int)id.Ordinal);
            }
            else
            {
                if (strings.TryGetValue(id.Name, out var stringOffset) == false)
                {
                    stringOffset = stringTableOffset;
                    strings.Add(id.Name, stringOffset);
                    stringTableOffset += id.Name.Length * 2 + 2;
                    stringTable.WriteUInt16((ushort)id.Name.Length);
                    foreach (var c in id.Name)
                        stringTable.WriteInt16((short)c);
                }

                bb.WriteUInt32(0x80000000U | (uint)stringOffset);
            }
        }

    }

}
