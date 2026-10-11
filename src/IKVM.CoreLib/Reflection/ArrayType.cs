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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents a single-dimensional array type with a zero lower bound (an SZARRAY).
    /// </summary>
    sealed class ArrayType : ElementHolderType
    {

        /// <summary>
        /// Gets the canonical single-dimensional array type of the specified element type and custom modifiers.
        /// </summary>
        /// <param name="type">The element type of the array.</param>
        /// <param name="mods">The custom modifiers applied to the element type.</param>
        /// <returns>The array type, shared with any equal array type previously created in the same universe.</returns>
        internal static Type Make(Type type, CustomModifiers mods) => type.Universe.CanonicalizeType(new ArrayType(type, mods));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type">The element type of the array.</param>
        /// <param name="mods">The custom modifiers applied to the element type.</param>
        ArrayType(Type type, CustomModifiers mods) :
           base(type, mods, Signature.ELEMENT_TYPE_SZARRAY)
        {
        }

        /// <summary>
        /// Gets the type from which the current array type directly inherits, which is always <c>System.Array</c>.
        /// </summary>
        public override Type BaseType => elementType.Module.Universe.System_Array;

        /// <summary>
        /// Gets the interfaces the array type itself declares: <see cref="IList{T}"/>, <see cref="ICollection{T}"/>
        /// and <see cref="IEnumerable{T}"/> of the element type.
        /// </summary>
        /// <returns>The generic collection interfaces implemented by the array type.</returns>
        public override Type[] __GetDeclaredInterfaces()
        {
            return new Type[] {
                Module.Universe.Import(typeof(IList<>)).MakeGenericType(elementType),
                Module.Universe.Import(typeof(ICollection<>)).MakeGenericType(elementType),
                Module.Universe.Import(typeof(IEnumerable<>)).MakeGenericType(elementType)
            };
        }

        /// <summary>
        /// Gets the methods the runtime provides on the array type: <c>Set</c>, <c>Address</c>, <c>Get</c> and the
        /// constructors.
        /// </summary>
        /// <returns>
        /// The built-in array methods. Besides the constructor that takes a length, one additional constructor is
        /// included for each level of nested single-dimensional array in the element type, taking one more length per
        /// level.
        /// </returns>
        public override MethodBase[] __GetDeclaredMethods()
        {
            var int32 = new Type[] { Module.Universe.System_Int32 };
            var list = new List<MethodBase>
            {
                new BuiltinArrayMethod(Module, this, "Set", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, new Type[] { Module.Universe.System_Int32, elementType }),
                new BuiltinArrayMethod(Module, this, "Address", CallingConventions.Standard | CallingConventions.HasThis, elementType.MakeByRefType(), int32),
                new BuiltinArrayMethod(Module, this, "Get", CallingConventions.Standard | CallingConventions.HasThis, elementType, int32),
                new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, int32))
            };

            for (var type = elementType; type.IsSZArray; type = type.GetElementType())
            {
                Array.Resize(ref int32, int32.Length + 1);
                int32[^1] = int32[0];
                list.Add(new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, int32)));
            }

            return list.ToArray();
        }

        /// <inheritdoc />
        public override TypeAttributes Attributes => TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable;

        /// <inheritdoc />
        public override int GetArrayRank() => 1;

        /// <inheritdoc />
        public override bool Equals(object o) => EqualsHelper(o as ArrayType);

        /// <inheritdoc />
        public override int GetHashCode() => elementType.GetHashCode() * 5;

        /// <summary>
        /// Gets the suffix appended to the element type name to form the name of this type.
        /// </summary>
        /// <returns>The string <c>[]</c>.</returns>
        internal override string GetSuffix() => "[]";

        /// <summary>
        /// Creates a single-dimensional array type around a different element type.
        /// </summary>
        /// <param name="type">The new element type.</param>
        /// <param name="mods">The custom modifiers applied to the new element type.</param>
        /// <returns>The single-dimensional array type of <paramref name="type"/>.</returns>
        protected override Type Wrap(Type type, CustomModifiers mods) => Make(type, mods);

    }

}
