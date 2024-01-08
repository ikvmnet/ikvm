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

    sealed class MultiArrayType : ElementHolderType
    {

        readonly int rank;
        readonly int[] sizes;
        readonly int[] lobounds;

        internal static Type Make(Type type, int rank, int[] sizes, int[] lobounds, CustomModifiers mods)
        {
            return type.Universe.CanonicalizeType(new MultiArrayType(type, rank, sizes, lobounds, mods));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lobounds"></param>
        /// <param name="mods"></param>
        MultiArrayType(Type type, int rank, int[] sizes, int[] lobounds, CustomModifiers mods) :
            base(type, mods, Signature.ELEMENT_TYPE_ARRAY)
        {
            this.rank = rank;
            this.sizes = sizes;
            this.lobounds = lobounds;
        }

        public override Type BaseType
        {
            get { return elementType.Module.universe.System_Array; }
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            var int32 = Module.universe.System_Int32;
            var setArgs = new Type[rank + 1];
            var getArgs = new Type[rank];
            var ctorArgs = new Type[rank * 2];
            for (int i = 0; i < rank; i++)
            {
                setArgs[i] = int32;
                getArgs[i] = int32;
                ctorArgs[i * 2 + 0] = int32;
                ctorArgs[i * 2 + 1] = int32;
            }

            setArgs[rank] = elementType;

            return new MethodBase[]
            {
                new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.universe.System_Void, getArgs)),
                new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.universe.System_Void, ctorArgs)),
                new BuiltinArrayMethod(Module, this, "Set", CallingConventions.Standard | CallingConventions.HasThis, Module.universe.System_Void, setArgs),
                new BuiltinArrayMethod(Module, this, "Address", CallingConventions.Standard | CallingConventions.HasThis, elementType.MakeByRefType(), getArgs),
                new BuiltinArrayMethod(Module, this, "Get", CallingConventions.Standard | CallingConventions.HasThis, elementType, getArgs),
            };
        }

        public override TypeAttributes Attributes
        {
            get { return TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable; }
        }

        public override int GetArrayRank()
        {
            return rank;
        }

        public override int[] __GetArraySizes()
        {
            return Util.Copy(sizes);
        }

        public override int[] __GetArrayLowerBounds()
        {
            return Util.Copy(lobounds);
        }

        public override bool Equals(object o)
        {
            var at = o as MultiArrayType;
            return EqualsHelper(at)
                && at.rank == rank
                && ArrayEquals(at.sizes, sizes)
                && ArrayEquals(at.lobounds, lobounds);
        }

        static bool ArrayEquals(int[] i1, int[] i2)
        {
            if (i1.Length == i2.Length)
            {
                for (int i = 0; i < i1.Length; i++)
                    if (i1[i] != i2[i])
                        return false;

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return elementType.GetHashCode() * 9 + rank;
        }

        internal override string GetSuffix()
        {
            if (rank == 1)
                return "[*]";
            else
                return "[" + new string(',', rank - 1) + "]";
        }

        protected override Type Wrap(Type type, CustomModifiers mods)
        {
            return Make(type, rank, sizes, lobounds, mods);
        }

    }

}
