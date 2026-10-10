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
    /// Represents a type name as it appears in metadata: a namespace and a name, neither of them escaped. The namespace
    /// is <see langword="null"/> for the empty namespace, never the empty string.
    /// </summary>
    readonly struct TypeName : IEquatable<TypeName>
    {

        /// <summary>
        /// Determines whether two type names have the same namespace and name, compared ordinally.
        /// </summary>
        /// <param name="o1">The first type name to compare.</param>
        /// <param name="o2">The second type name to compare.</param>
        /// <returns><see langword="true"/> if the type names are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(TypeName o1, TypeName o2) => o1._ns == o2._ns && o1._name == o2._name;

        /// <summary>
        /// Determines whether two type names differ in namespace or name, compared ordinally.
        /// </summary>
        /// <param name="o1">The first type name to compare.</param>
        /// <param name="o2">The second type name to compare.</param>
        /// <returns><see langword="true"/> if the type names are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(TypeName o1, TypeName o2) => o1._ns != o2._ns || o1._name != o2._name;

        readonly string _ns;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ns">The unescaped namespace, or <see langword="null"/> for the empty namespace.</param>
        /// <param name="name">The unescaped name of the type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        internal TypeName(string ns, string name)
        {
            _ns = ns;
            _name = name ?? throw new ArgumentNullException("name");
        }

        /// <summary>
        /// Gets the unescaped name of the type.
        /// </summary>
        internal string Name => _name;

        /// <summary>
        /// Gets the unescaped namespace of the type, or <see langword="null"/> for the empty namespace.
        /// </summary>
        internal string Namespace => _ns;

        /// <inheritdoc />
        public override int GetHashCode() => _ns == null ? _name.GetHashCode() : _ns.GetHashCode() * 37 + _name.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var other = obj as TypeName?;
            return other != null && other.Value == this;
        }

        /// <summary>
        /// Returns the full name of the type: the namespace and name joined by a dot, or only the name when the type is
        /// in the empty namespace.
        /// </summary>
        /// <returns>The unescaped full name of the type.</returns>
        public override string ToString() => _ns == null ? _name : _ns + "." + _name;

        /// <inheritdoc />
        bool IEquatable<TypeName>.Equals(TypeName other) => this == other;

        /// <summary>
        /// Determines whether this type name matches the specified unescaped full name, compared ordinally.
        /// </summary>
        /// <param name="fullName">The full name, namespace and name joined by a dot, to compare with.</param>
        /// <returns><see langword="true"/> if <paramref name="fullName"/> names this type; otherwise, <see langword="false"/>.</returns>
        internal bool Matches(string fullName)
        {
            if (_ns == null)
                return _name == fullName;

            if (_ns.Length + 1 + _name.Length == fullName.Length)
                return fullName.StartsWith(_ns, StringComparison.Ordinal) && fullName[_ns.Length] == '.' && fullName.EndsWith(_name, StringComparison.Ordinal);

            return false;
        }

        /// <summary>
        /// Returns a copy of this type name with the namespace and name converted to lower case using the invariant
        /// culture, for case-insensitive lookups.
        /// </summary>
        /// <returns>The lower-cased type name.</returns>
        internal TypeName ToLowerInvariant() => new(_ns?.ToLowerInvariant(), _name.ToLowerInvariant());

        /// <summary>
        /// Splits an unescaped full type name into namespace and name at the last dot.
        /// </summary>
        /// <param name="name">The unescaped full name of the type.</param>
        /// <returns>The type name, with a <see langword="null"/> namespace if <paramref name="name"/> contains no dot.</returns>
        internal static TypeName Split(string name)
        {
            var dot = name.LastIndexOf('.');
            if (dot == -1)
                return new TypeName(null, name);
            else
                return new TypeName(name[..dot], name[(dot + 1)..]);
        }

    }

}
