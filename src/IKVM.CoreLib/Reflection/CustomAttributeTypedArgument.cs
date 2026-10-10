/*
  Copyright (C) 2009 Jeroen Frijters

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
    /// Represents an argument of a custom attribute, or an element of an array argument.
    /// </summary>
    readonly struct CustomAttributeTypedArgument
    {

        readonly Type _type;
        readonly object _value;

        /// <summary>
        /// Initializes a new instance with the specified type and value.
        /// </summary>
        /// <param name="type">The type of the custom attribute argument.</param>
        /// <param name="value">The value of the custom attribute argument.</param>
        internal CustomAttributeTypedArgument(Type type, object value)
        {
            _type = type;
            _value = value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="CustomAttributeTypedArgument"/> with an equal type and value; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => this == obj as CustomAttributeTypedArgument?;

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode() => _type.GetHashCode() ^ 77 * (_value == null ? 0 : _value.GetHashCode());

        /// <summary>
        /// Gets the type of the argument or of the array argument element.
        /// </summary>
        public Type ArgumentType => _type;

        /// <summary>
        /// Gets the value of the argument for a simple argument or for an element of an array argument; gets a
        /// collection of values for an array argument.
        /// </summary>
        public object Value => _value;

        /// <summary>
        /// Tests whether two <see cref="CustomAttributeTypedArgument"/> structures are equivalent.
        /// </summary>
        /// <param name="arg1">The structure on the left side of the equality operator.</param>
        /// <param name="arg2">The structure on the right side of the equality operator.</param>
        /// <returns><see langword="true"/> if the two structures are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(CustomAttributeTypedArgument arg1, CustomAttributeTypedArgument arg2) =>
            arg1._type.Equals(arg2._type) && (arg1._value == arg2._value || (arg1._value != null && arg1._value.Equals(arg2._value)));

        /// <summary>
        /// Tests whether two <see cref="CustomAttributeTypedArgument"/> structures are different.
        /// </summary>
        /// <param name="arg1">The structure on the left side of the inequality operator.</param>
        /// <param name="arg2">The structure on the right side of the inequality operator.</param>
        /// <returns><see langword="true"/> if the two structures are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(CustomAttributeTypedArgument arg1, CustomAttributeTypedArgument arg2) => !(arg1 == arg2);

    }

}
