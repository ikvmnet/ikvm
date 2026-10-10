/*
  Copyright (C) 2008-2011 Jeroen Frijters

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

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines and represents a constructor of a dynamic class. The constructor is backed by a
    /// <see cref="MethodBuilder"/> that holds its definition.
    /// </summary>
    sealed class ConstructorBuilder : ConstructorInfo
    {

        readonly MethodBuilder _methodBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="mb">The method builder that defines the constructor.</param>
        internal ConstructorBuilder(MethodBuilder mb)
        {
            _methodBuilder = mb;
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object. Two instances are equal
        /// when they wrap equal method builders.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> represents the same constructor; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            ConstructorBuilder other = obj as ConstructorBuilder;
            return other != null && other._methodBuilder.Equals(_methodBuilder);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _methodBuilder.GetHashCode();

        /// <summary>
        /// Gets an <see cref="ILGenerator"/> for this constructor.
        /// </summary>
        /// <returns>An <see cref="ILGenerator"/> object for this constructor.</returns>
        public ILGenerator GetILGenerator() => _methodBuilder.GetILGenerator();

        /// <summary>
        /// Gets the method builder that defines the constructor.
        /// </summary>
        /// <returns>The underlying <see cref="MethodBuilder"/>.</returns>
        internal override MethodInfo GetMethodInfo() => _methodBuilder;

    }

}
