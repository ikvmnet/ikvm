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
using System.Diagnostics;

namespace IKVM.Reflection
{

    /// <summary>
    /// Wraps a constructor obtained through a type other than its declaring type, so that, as in System.Reflection,
    /// the same constructor reflected through different types does not compare equal.
    /// </summary>
    sealed class ConstructorInfoWithReflectedType : ConstructorInfo
    {

        readonly Type _reflectedType;
        readonly ConstructorInfo _ctor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType">The type through which the constructor was obtained. Must not be its declaring type.</param>
        /// <param name="ctor">The constructor to wrap.</param>
        internal ConstructorInfoWithReflectedType(Type reflectedType, ConstructorInfo ctor)
        {
            Debug.Assert(reflectedType != ctor.DeclaringType);
            _reflectedType = reflectedType;
            _ctor = ctor;
        }

        /// <summary>
        /// Returns whether the specified object wraps the same constructor obtained through the same type.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> wraps the same constructor and reflected type; otherwise <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as ConstructorInfoWithReflectedType;
            return other != null && other._reflectedType == _reflectedType && other._ctor == _ctor;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _reflectedType.GetHashCode() ^ _ctor.GetHashCode();

        /// <inheritdoc />
        internal override MethodInfo GetMethodInfo() => _ctor.GetMethodInfo();

    }

}
