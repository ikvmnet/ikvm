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

    sealed class ArrayType : ElementHolderType
    {

        internal static Type Make(Type type, CustomModifiers mods)
        {
            return type.Universe.CanonicalizeType(new ArrayType(type, mods));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mods"></param>
        ArrayType(Type type, CustomModifiers mods) :
           base(type, mods, Signature.ELEMENT_TYPE_SZARRAY)
        {

        }

        public override Type BaseType
        {
            get { return elementType.Module.Universe.System_Array; }
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            return new Type[] {
                Module.Universe.Import(typeof(IList<>)).MakeGenericType(elementType),
                Module.Universe.Import(typeof(ICollection<>)).MakeGenericType(elementType),
                Module.Universe.Import(typeof(IEnumerable<>)).MakeGenericType(elementType)
            };
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            var int32 = new Type[] { Module.Universe.System_Int32 };
            var list = new List<MethodBase>();
            list.Add(new BuiltinArrayMethod(Module, this, "Set", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, new Type[] { Module.Universe.System_Int32, elementType }));
            list.Add(new BuiltinArrayMethod(Module, this, "Address", CallingConventions.Standard | CallingConventions.HasThis, elementType.MakeByRefType(), int32));
            list.Add(new BuiltinArrayMethod(Module, this, "Get", CallingConventions.Standard | CallingConventions.HasThis, elementType, int32));
            list.Add(new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, int32)));

            for (var type = elementType; type.__IsVector; type = type.GetElementType())
            {
                Array.Resize(ref int32, int32.Length + 1);
                int32[int32.Length - 1] = int32[0];
                list.Add(new ConstructorInfoImpl(new BuiltinArrayMethod(Module, this, ".ctor", CallingConventions.Standard | CallingConventions.HasThis, Module.Universe.System_Void, int32)));
            }

            return list.ToArray();
        }

        public override TypeAttributes Attributes
        {
            get { return TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable; }
        }

        public override int GetArrayRank()
        {
            return 1;
        }

        public override bool Equals(object o)
        {
            return EqualsHelper(o as ArrayType);
        }

        public override int GetHashCode()
        {
            return elementType.GetHashCode() * 5;
        }

        internal override string GetSuffix()
        {
            return "[]";
        }

        protected override Type Wrap(Type type, CustomModifiers mods)
        {
            return Make(type, mods);
        }

    }

}
