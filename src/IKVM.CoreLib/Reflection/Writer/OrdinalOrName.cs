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

namespace IKVM.Reflection.Writer
{

    /// <summary>
    /// Identifies an entry in a Win32 resource directory, either by a numeric ordinal or by a name.
    /// </summary>
    /// <param name="Ordinal">The numeric identifier of the entry, or 0xFFFF when the entry is identified by name.</param>
    /// <param name="Name">The name of the entry, or <see langword="null"/> when the entry is identified by ordinal.</param>
    readonly record struct OrdinalOrName(ushort Ordinal, string Name)
    {

        /// <summary>
        /// Initializes a new instance that identifies an entry by ordinal.
        /// </summary>
        /// <param name="ordinal">The numeric identifier of the entry.</param>
        internal OrdinalOrName(ushort ordinal) :
            this(ordinal, null)
        {

        }

        /// <summary>
        /// Initializes a new instance that identifies an entry by name.
        /// </summary>
        /// <param name="name">The name of the entry.</param>
        internal OrdinalOrName(string name) :
            this(0xFFFF, name)
        {

        }

        /// <summary>
        /// Returns whether this identifier sorts after another one. Names are compared case-insensitively; ordinals
        /// are compared numerically.
        /// </summary>
        /// <param name="other">The identifier to compare with.</param>
        /// <returns><see langword="true"/> if this identifier sorts after <paramref name="other"/>; otherwise <see langword="false"/>.</returns>
        internal readonly bool IsGreaterThan(OrdinalOrName other)
        {
            if (Name != null)
                return StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name) > 0;
            else
                return Ordinal > other.Ordinal;
        }

        /// <summary>
        /// Returns whether this identifier is equal to another one, comparing the names case-insensitively.
        /// </summary>
        /// <param name="other">The identifier to compare with.</param>
        /// <returns><see langword="true"/> if both identifiers have the same ordinal and name; otherwise <see langword="false"/>.</returns>
        public readonly bool Equals(OrdinalOrName other) => Ordinal == other.Ordinal && StringComparer.OrdinalIgnoreCase.Equals(Name, other.Name);

        /// <inheritdoc />
        public override readonly int GetHashCode() => Ordinal.GetHashCode() ^ (Name is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Name));

    }

}
