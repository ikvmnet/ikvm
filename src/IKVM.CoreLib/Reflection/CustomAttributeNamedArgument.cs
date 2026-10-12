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
namespace IKVM.Reflection
{

    /// <summary>
    /// Represents a named argument of a custom attribute in the reflection-only context.
    /// </summary>
    readonly struct CustomAttributeNamedArgument
    {

        readonly MemberInfo _member;
        readonly CustomAttributeTypedArgument _value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="member">The field or property of the custom attribute that the argument sets.</param>
        /// <param name="value">The value of the argument.</param>
        internal CustomAttributeNamedArgument(MemberInfo member, CustomAttributeTypedArgument value)
        {
            _member = member;
            _value = value;
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> equals the type and value of this instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => this == obj as CustomAttributeNamedArgument?;

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => _member.GetHashCode() ^ 53 * _value.GetHashCode();

        /// <summary>
        /// Gets the attribute member that would be used to set the named argument.
        /// </summary>
        public MemberInfo MemberInfo => _member;

        /// <summary>
        /// Gets a <see cref="CustomAttributeTypedArgument"/> structure that can be used to obtain the type and value of
        /// the current named argument.
        /// </summary>
        public CustomAttributeTypedArgument TypedValue => _value;

        /// <summary>
        /// Gets a value that indicates whether the named argument is a field.
        /// </summary>
        public bool IsField => _member.MemberType == MemberTypes.Field;

        /// <summary>
        /// Gets the name of the attribute member that would be used to set the named argument.
        /// </summary>
        public string MemberName => _member.Name;

        /// <summary>
        /// Tests whether two <see cref="CustomAttributeNamedArgument"/> structures are equivalent.
        /// </summary>
        /// <param name="arg1">The structure to the left of the equality operator.</param>
        /// <param name="arg2">The structure to the right of the equality operator.</param>
        /// <returns><see langword="true"/> if the two structures are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(CustomAttributeNamedArgument arg1, CustomAttributeNamedArgument arg2) =>
            arg1._member.Equals(arg2._member) && arg1._value == arg2._value;

        /// <summary>
        /// Tests whether two <see cref="CustomAttributeNamedArgument"/> structures are different.
        /// </summary>
        /// <param name="arg1">The structure to the left of the inequality operator.</param>
        /// <param name="arg2">The structure to the right of the inequality operator.</param>
        /// <returns><see langword="true"/> if the two structures are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(CustomAttributeNamedArgument arg1, CustomAttributeNamedArgument arg2) => !(arg1 == arg2);

    }

}
