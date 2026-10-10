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

namespace IKVM.Reflection
{

    /// <summary>
    /// Describes the constraints on a generic type parameter of a generic type or method.
    /// </summary>
    [Flags]
    enum GenericParameterAttributes
    {

        /// <summary>
        /// There are no special flags.
        /// </summary>
        None = 0,

        /// <summary>
        /// The generic type parameter is covariant.
        /// </summary>
        Covariant = 1,

        /// <summary>
        /// The generic type parameter is contravariant.
        /// </summary>
        Contravariant = 2,

        /// <summary>
        /// Selects the combination of all variance flags.
        /// </summary>
        VarianceMask = 3,

        /// <summary>
        /// A type can be substituted for the generic type parameter only if it is a reference type.
        /// </summary>
        ReferenceTypeConstraint = 4,

        /// <summary>
        /// A type can be substituted for the generic type parameter only if it is a value type and is not nullable.
        /// </summary>
        NotNullableValueTypeConstraint = 8,

        /// <summary>
        /// A type can be substituted for the generic type parameter only if it has a parameterless constructor.
        /// </summary>
        DefaultConstructorConstraint = 16,

        /// <summary>
        /// Selects the combination of all special constraint flags.
        /// </summary>
        SpecialConstraintMask = 28,

    }

}
