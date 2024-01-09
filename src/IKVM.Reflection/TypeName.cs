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

    // this respresents a type name as in metadata:
    // - ns will be null for empty the namespace (never the empty string)
    // - the strings are not escaped
    readonly struct TypeName : IEquatable<TypeName>
    {

        public static bool operator ==(TypeName o1, TypeName o2)
        {
            return o1.ns == o2.ns && o1.name == o2.name;
        }

        public static bool operator !=(TypeName o1, TypeName o2)
        {
            return o1.ns != o2.ns || o1.name != o2.name;
        }

        readonly string ns;
        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal TypeName(string ns, string name)
        {
            this.ns = ns;
            this.name = name ?? throw new ArgumentNullException("name");
        }

        internal string Name => name;

        internal string Namespace => ns;

        public override int GetHashCode() => ns == null ? name.GetHashCode() : ns.GetHashCode() * 37 + name.GetHashCode();

        public override bool Equals(object obj)
        {
            var other = obj as TypeName?;
            return other != null && other.Value == this;
        }

        public override string ToString()
        {
            return ns == null ? name : ns + "." + name;
        }

        bool IEquatable<TypeName>.Equals(TypeName other)
        {
            return this == other;
        }

        internal bool Matches(string fullName)
        {
            if (ns == null)
                return name == fullName;

            if (ns.Length + 1 + name.Length == fullName.Length)
                return fullName.StartsWith(ns, StringComparison.Ordinal) && fullName[ns.Length] == '.' && fullName.EndsWith(name, StringComparison.Ordinal);

            return false;
        }

        internal TypeName ToLowerInvariant() => new TypeName(ns?.ToLowerInvariant(), name.ToLowerInvariant());

        internal static TypeName Split(string name)
        {
            int dot = name.LastIndexOf('.');
            if (dot == -1)
                return new TypeName(null, name);
            else
                return new TypeName(name.Substring(0, dot), name.Substring(dot + 1));
        }

    }

}
