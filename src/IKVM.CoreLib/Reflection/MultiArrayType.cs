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
    /// Represents a general array type (ELEMENT_TYPE_ARRAY), which has an explicit rank and optional sizes and lower
    /// bounds, as opposed to a single-dimensional, zero-based array.
    /// </summary>
    sealed class MultiArrayType : ElementHolderType
    {

        readonly int _rank;
        readonly int[] _sizes;
        readonly int[] _lobounds;

        /// <summary>
        /// Gets the canonical general array type for the specified element type, shape and custom modifiers.
        /// </summary>
        /// <param name="type">The element type of the array.</param>
        /// <param name="rank">The number of dimensions of the array.</param>
        /// <param name="sizes">The sizes of the leading dimensions that have a specified size.</param>
        /// <param name="lobounds">The lower bounds of the leading dimensions that have a specified lower bound.</param>
        /// <param name="mods">The custom modifiers applied to the array type.</param>
        /// <returns>The canonical instance of the array type from the element type's universe.</returns>
        internal static Type Make(Type type, int rank, int[] sizes, int[] lobounds, CustomModifiers mods) =>
            type.Universe.CanonicalizeType(new MultiArrayType(type, rank, sizes, lobounds, mods));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type">The element type of the array.</param>
        /// <param name="rank">The number of dimensions of the array.</param>
        /// <param name="sizes">The sizes of the leading dimensions that have a specified size.</param>
        /// <param name="lobounds">The lower bounds of the leading dimensions that have a specified lower bound.</param>
        /// <param name="mods">The custom modifiers applied to the array type.</param>
        MultiArrayType(Type type, int rank, int[] sizes, int[] lobounds, CustomModifiers mods) :
            base(type, mods, Signature.ELEMENT_TYPE_ARRAY)
        {
            _rank = rank;
            _sizes = sizes;
            _lobounds = lobounds;
        }

        /// <summary>
        /// Gets the type from which the current type directly inherits, which for an array type is
        /// <see cref="System.Array"/>.
        /// </summary>
        public override Type BaseType => elementType.Module.Universe.System_Array;

        /// <summary>
        /// Returns the methods the runtime provides for the array type: the two constructors (taking the lengths, or
        /// the lower bounds and lengths, of every dimension) and the <c>Set</c>, <c>Address</c> and <c>Get</c>
        /// accessors.
        /// </summary>
        /// <returns>The built-in methods of the array type.</returns>
        public override MethodBase[] __GetDeclaredMethods()
        {
            var int32 = Module.Universe.System_Int32;
            var setArgs = new Type[_rank + 1];
            var getArgs = new Type[_rank];
            var ctorArgs = new Type[_rank * 2];
            for (var i = 0; i < _rank; i++)
            {
                setArgs[i] = int32;
                getArgs[i] = int32;
                ctorArgs[i * 2 + 0] = int32;
                ctorArgs[i * 2 + 1] = int32;
            }

            setArgs[_rank] = elementType;

            return new MethodBase[]
            {
                new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, getArgs)),
                new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, ctorArgs)),
                new BuiltinArrayMethod(Module, this, "Set", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, setArgs),
                new BuiltinArrayMethod(Module, this, "Address", CallingConventions.Standard | CallingConventions.HasThis, elementType.MakeByRefType(), getArgs),
                new BuiltinArrayMethod(Module, this, "Get", CallingConventions.Standard | CallingConventions.HasThis, elementType, getArgs),
            };
        }

        /// <summary>
        /// Gets the attributes associated with the type, which for an array type are always public, sealed and
        /// serializable.
        /// </summary>
        public override TypeAttributes Attributes => TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable;

        /// <inheritdoc />
        public override int GetArrayRank() => _rank;

        /// <inheritdoc />
        public override int[] __GetArraySizes() => Util.Copy(_sizes);

        /// <inheritdoc />
        public override int[] __GetArrayLowerBounds() => Util.Copy(_lobounds);

        /// <summary>
        /// Determines whether the specified object is a general array type with the same element type, custom
        /// modifiers, rank, sizes and lower bounds.
        /// </summary>
        /// <param name="o">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="o"/> represents the same array type; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object o)
        {
            var at = o as MultiArrayType;
            return EqualsHelper(at)
                && at._rank == _rank
                && ArrayEquals(at._sizes, _sizes)
                && ArrayEquals(at._lobounds, _lobounds);
        }

        /// <summary>
        /// Determines whether two arrays have the same length and the same elements.
        /// </summary>
        /// <param name="i1">The first array to compare.</param>
        /// <param name="i2">The second array to compare.</param>
        /// <returns><see langword="true"/> if the arrays are equal element by element; otherwise, <see langword="false"/>.</returns>
        static bool ArrayEquals(int[] i1, int[] i2)
        {
            if (i1.Length == i2.Length)
            {
                for (var i = 0; i < i1.Length; i++)
                    if (i1[i] != i2[i])
                        return false;

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode() => elementType.GetHashCode() * 9 + _rank;

        /// <summary>
        /// Gets the suffix appended to the element type's name: <c>[*]</c> for a rank 1 general array, to distinguish
        /// it from a single-dimensional zero-based array, and otherwise brackets containing one comma less than the
        /// rank.
        /// </summary>
        /// <returns>The array suffix.</returns>
        internal override string GetSuffix()
        {
            if (_rank == 1)
                return "[*]";
            else
                return "[" + new string(',', _rank - 1) + "]";
        }

        /// <inheritdoc />
        protected override Type Wrap(Type type, CustomModifiers mods) => Make(type, _rank, _sizes, _lobounds, mods);

    }

}
