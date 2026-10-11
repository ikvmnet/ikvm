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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    // disable warnings that complain about us having == and != operators without also overriding Equals/GetHashCode,
    // this is intentional because most subtypes use reference equality
#pragma warning disable 660, 661
    /// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    abstract class MemberInfo : ICustomAttributeProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal MemberInfo()
        {
        }

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        public abstract Type DeclaringType { get; }

        /// <summary>
        /// When overridden in a derived class, gets a <see cref="MemberTypes"/> value indicating the type of the member,
        /// for example method, constructor or event.
        /// </summary>
        public abstract MemberTypes MemberType { get; }

        /// <summary>
        /// Returns this member as seen through the specified type. When <paramref name="type"/> is not the declaring
        /// type, the result is a wrapper that compares unequal to the same member obtained through another type.
        /// </summary>
        /// <param name="type">The type through which the member was obtained.</param>
        /// <returns>The member, or a wrapper that records <paramref name="type"/> as its reflected type.</returns>
        internal abstract MemberInfo SetReflectedType(Type type);

        /// <summary>
        /// Gets a value that identifies a metadata element.
        /// </summary>
        /// <exception cref="NotSupportedException">The member does not have a metadata token.</exception>
        public virtual int MetadataToken => throw new NotSupportedException();

        /// <summary>
        /// Gets the module in which the type that declares the member represented by the current
        /// <see cref="MemberInfo"/> is defined.
        /// </summary>
        public abstract Module Module
        {
            get;
        }

        /// <summary>
        /// Gets whether this member is a placeholder for a member that could not be resolved.
        /// </summary>
        public virtual bool __IsMissing => false;

        /// <summary>
        /// Indicates whether one or more attributes of the specified type or of its derived types is applied to this
        /// member.
        /// </summary>
        /// <param name="attributeType">The type of custom attribute to search for.</param>
        /// <param name="inherit"><see langword="true"/> to search this member's inheritance chain to find the attributes; otherwise <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if one or more instances of <paramref name="attributeType"/> or any of its derived types is applied to this member; otherwise <see langword="false"/>.</returns>
        public bool IsDefined(Type attributeType, bool inherit) => CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;

        /// <summary>
        /// Returns the custom attributes of the specified type, or of its derived types, applied to this member.
        /// </summary>
        /// <param name="attributeType">The type of custom attribute to search for, or <see langword="null"/> for all attributes.</param>
        /// <param name="inherit"><see langword="true"/> to search this member's inheritance chain to find the attributes; otherwise <see langword="false"/>.</param>
        /// <returns>The matching custom attribute data.</returns>
        public IList<CustomAttributeData> __GetCustomAttributes(Type attributeType, bool inherit) =>
            CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit);

        /// <summary>
        /// Returns a list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have
        /// been applied to the target member.
        /// </summary>
        /// <returns>A generic list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have been applied to the target member.</returns>
        public IList<CustomAttributeData> GetCustomAttributesData() => CustomAttributeData.GetCustomAttributes(this);

        /// <summary>
        /// Indicates whether two <see cref="MemberInfo"/> objects are equal.
        /// </summary>
        /// <param name="m1">The <see cref="MemberInfo"/> to compare to <paramref name="m2"/>.</param>
        /// <param name="m2">The <see cref="MemberInfo"/> to compare to <paramref name="m1"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="m1"/> is equal to <paramref name="m2"/>; otherwise <see langword="false"/>.</returns>
        public static bool operator ==(MemberInfo m1, MemberInfo m2) => ReferenceEquals(m1, m2) || (!ReferenceEquals(m1, null) && m1.Equals(m2));

        /// <summary>
        /// Indicates whether two <see cref="MemberInfo"/> objects are not equal.
        /// </summary>
        /// <param name="m1">The <see cref="MemberInfo"/> to compare to <paramref name="m2"/>.</param>
        /// <param name="m2">The <see cref="MemberInfo"/> to compare to <paramref name="m1"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="m1"/> is not equal to <paramref name="m2"/>; otherwise <see langword="false"/>.</returns>
        public static bool operator !=(MemberInfo m1, MemberInfo m2) => !(m1 == m2);

        /// <summary>
        /// Gets the metadata token currently assigned to the member, which for a member being built may still be a
        /// pseudo token.
        /// </summary>
        /// <returns>The current metadata token.</returns>
        internal abstract int GetCurrentToken();

        /// <summary>
        /// Gets the pseudo custom attributes of the member: attributes such as DllImport or MarshalAs that are stored
        /// as metadata flags and tables rather than as custom attributes, but are reported as if they were.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter on, or <see langword="null"/> for all.</param>
        /// <returns>The pseudo custom attributes.</returns>
        internal abstract List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType);

        /// <summary>
        /// Gets whether the member is complete, meaning it was read from a module or its type has been created.
        /// </summary>
        internal abstract bool IsBaked { get; }

        /// <summary>
        /// Returns whether the member matches the visibility and static/instance criteria in the specified flags.
        /// </summary>
        /// <param name="flags">The binding flags to match.</param>
        /// <returns><see langword="true"/> if the member matches; otherwise <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">The member kind does not support binding flag matching.</exception>
        internal virtual bool BindingFlagsMatch(BindingFlags flags) => throw new InvalidOperationException();

        /// <summary>
        /// Returns whether the member, seen as inherited from a base type, matches the specified flags. Private
        /// members never match.
        /// </summary>
        /// <param name="flags">The binding flags to match.</param>
        /// <returns><see langword="true"/> if the inherited member matches; otherwise <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">The member kind does not support binding flag matching.</exception>
        internal virtual bool BindingFlagsMatchInherited(BindingFlags flags) => throw new InvalidOperationException();

        /// <summary>
        /// Returns whether a boolean member property matches a pair of binding flags.
        /// </summary>
        /// <param name="state">The value of the member property, for example whether it is public.</param>
        /// <param name="flags">The binding flags to match.</param>
        /// <param name="trueFlag">The flag that must be set when <paramref name="state"/> is <see langword="true"/>.</param>
        /// <param name="falseFlag">The flag that must be set when <paramref name="state"/> is <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if the flag matching <paramref name="state"/> is set; otherwise <see langword="false"/>.</returns>
        protected static bool BindingFlagsMatch(bool state, BindingFlags flags, BindingFlags trueFlag, BindingFlags falseFlag) =>
            (state && (flags & trueFlag) == trueFlag) || (!state && (flags & falseFlag) == falseFlag);

        /// <summary>
        /// Returns the member as seen through the specified type.
        /// </summary>
        /// <typeparam name="T">The kind of member.</typeparam>
        /// <param name="member">The member, or <see langword="null"/>.</param>
        /// <param name="type">The type through which the member was obtained.</param>
        /// <returns>The member with its reflected type set, or <see langword="null"/> if <paramref name="member"/> is <see langword="null"/>.</returns>
        protected static T SetReflectedType<T>(T member, Type type)
            where T : MemberInfo => member == null ? null : (T)member.SetReflectedType(type);

        /// <summary>
        /// Replaces each member in the array with the member as seen through the specified type.
        /// </summary>
        /// <typeparam name="T">The kind of member.</typeparam>
        /// <param name="members">The members, which are replaced in place.</param>
        /// <param name="type">The type through which the members were obtained.</param>
        /// <returns>The same array, <paramref name="members"/>.</returns>
        protected static T[] SetReflectedType<T>(T[] members, Type type)
            where T : MemberInfo
        {
            for (var i = 0; i < members.Length; i++)
                members[i] = SetReflectedType(members[i], type);

            return members;
        }

    }

}
