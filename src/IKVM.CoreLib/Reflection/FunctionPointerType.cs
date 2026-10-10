/*
  Copyright (C) 2009-2015 Jeroen Frijters

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
    /// Represents a function pointer type (ELEMENT_TYPE_FNPTR) described by a stand-alone method signature.
    /// </summary>
    sealed class FunctionPointerType : TypeInfo
    {

        readonly Universe _universe;
        readonly __StandAloneMethodSig _sig;

        /// <summary>
        /// Gets the canonical function pointer type for the specified signature.
        /// </summary>
        /// <param name="universe">The universe the type belongs to.</param>
        /// <param name="sig">The signature of the function the pointer points to.</param>
        /// <returns>The canonical instance of the function pointer type.</returns>
        internal static Type Make(Universe universe, __StandAloneMethodSig sig) => universe.CanonicalizeType(new FunctionPointerType(universe, sig));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe">The universe the type belongs to.</param>
        /// <param name="sig">The signature of the function the pointer points to.</param>
        FunctionPointerType(Universe universe, __StandAloneMethodSig sig) :
            base(Signature.ELEMENT_TYPE_FNPTR)
        {
            _universe = universe;
            _sig = sig;
        }

        /// <summary>
        /// Returns whether the specified object is a function pointer type in the same universe with an equal signature.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an equal function pointer type; otherwise <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as FunctionPointerType;
            return other != null
                && other._universe == _universe
                && other._sig.Equals(_sig);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _sig.GetHashCode();

        /// <summary>
        /// Gets the signature of the function the pointer points to.
        /// </summary>
        public override __StandAloneMethodSig __MethodSignature => _sig;

        /// <summary>
        /// Gets the base type, which is always <see langword="null"/> for a function pointer.
        /// </summary>
        public override Type BaseType => null;

        /// <summary>
        /// Gets the attributes of the type, which are always empty for a function pointer.
        /// </summary>
        public override TypeAttributes Attributes => 0;

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a function pointer has no name.</exception>
        public override string Name => throw new InvalidOperationException();

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a function pointer has no name.</exception>
        public override string FullName => throw new InvalidOperationException();

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a function pointer belongs to no module.</exception>
        public override Module Module => throw new InvalidOperationException();

        /// <inheritdoc />
        internal override Universe Universe => _universe;

        /// <summary>
        /// Returns a placeholder string for the function pointer type.
        /// </summary>
        /// <returns>The string <c>&lt;FunctionPtr&gt;</c>.</returns>
        public override string ToString() => "<FunctionPtr>";

        /// <summary>
        /// Gets whether the function pointer's signature refers to a missing type.
        /// </summary>
        protected override bool ContainsMissingTypeImpl => _sig.ContainsMissingType;

        /// <summary>
        /// Gets whether the type is complete, which is always the case for a function pointer.
        /// </summary>
        internal override bool IsBaked => true;

        /// <summary>
        /// Gets whether the type is a value type, which is never the case for a function pointer.
        /// </summary>
        protected override bool IsValueTypeImpl => false;

    }

}
