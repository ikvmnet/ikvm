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
namespace IKVM.Reflection
{

    /// <summary>
    /// Represents an unmanaged pointer type.
    /// </summary>
    sealed class PointerType : ElementHolderType
    {

        /// <summary>
        /// Gets the canonical pointer type to <paramref name="type"/> with the specified custom modifiers.
        /// </summary>
        /// <param name="type">The type pointed to.</param>
        /// <param name="mods">The custom modifiers applied to the pointer type.</param>
        /// <returns>The pointer type.</returns>
        internal static Type Make(Type type, CustomModifiers mods) => type.Universe.CanonicalizeType(new PointerType(type, mods));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type">The type pointed to.</param>
        /// <param name="mods">The custom modifiers applied to the pointer type.</param>
        PointerType(Type type, CustomModifiers mods) :
            base(type, mods, Signature.ELEMENT_TYPE_PTR)
        {
        }

        /// <inheritdoc />
        public override bool Equals(object o) => EqualsHelper(o as PointerType);

        /// <inheritdoc />
        public override int GetHashCode() => elementType.GetHashCode() * 7;

        /// <inheritdoc />
        public override Type BaseType => null;

        /// <inheritdoc />
        public override TypeAttributes Attributes => 0;

        /// <inheritdoc />
        internal override string GetSuffix() => "*";

        /// <inheritdoc />
        protected override Type Wrap(Type type, CustomModifiers mods) => Make(type, mods);

    }

}
