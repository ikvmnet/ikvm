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

    readonly record struct OrdinalOrName(ushort Ordinal, string Name)
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ordinal"></param>
        internal OrdinalOrName(ushort ordinal) : 
            this(ordinal, null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        internal OrdinalOrName(string name) : 
            this(0xFFFF, name)
        {

        }

        /// <summary>
        /// Returns whether or not this ordinal is greater than the other ordinal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal readonly bool IsGreaterThan(OrdinalOrName other)
        {
            if (Name != null)
                return StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name) > 0;
            else
                return Ordinal > other.Ordinal;
        }

        public readonly bool Equals(OrdinalOrName other)
        {
            return Ordinal == other.Ordinal && StringComparer.OrdinalIgnoreCase.Equals(Name, other.Name);
        }

        /// <inheritdoc />
        public override readonly int GetHashCode()
        {
            return Ordinal.GetHashCode() ^ StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

    }

}
