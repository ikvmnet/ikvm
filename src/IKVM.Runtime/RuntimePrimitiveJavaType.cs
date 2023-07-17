/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed class RuntimePrimitiveJavaType : RuntimeJavaType
    {

        internal static readonly RuntimePrimitiveJavaType BYTE = new RuntimePrimitiveJavaType(Types.Byte, "B");
        internal static readonly RuntimePrimitiveJavaType CHAR = new RuntimePrimitiveJavaType(Types.Char, "C");
        internal static readonly RuntimePrimitiveJavaType DOUBLE = new RuntimePrimitiveJavaType(Types.Double, "D");
        internal static readonly RuntimePrimitiveJavaType FLOAT = new RuntimePrimitiveJavaType(Types.Single, "F");
        internal static readonly RuntimePrimitiveJavaType INT = new RuntimePrimitiveJavaType(Types.Int32, "I");
        internal static readonly RuntimePrimitiveJavaType LONG = new RuntimePrimitiveJavaType(Types.Int64, "J");
        internal static readonly RuntimePrimitiveJavaType SHORT = new RuntimePrimitiveJavaType(Types.Int16, "S");
        internal static readonly RuntimePrimitiveJavaType BOOLEAN = new RuntimePrimitiveJavaType(Types.Boolean, "Z");
        internal static readonly RuntimePrimitiveJavaType VOID = new RuntimePrimitiveJavaType(Types.Void, "V");

        readonly Type type;
        readonly string sigName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sigName"></param>
        private RuntimePrimitiveJavaType(Type type, string sigName) :
            base(TypeFlags.None, Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null)
        {
            this.type = type;
            this.sigName = sigName;
        }

        internal override RuntimeJavaType BaseTypeWrapper => null;

        internal static bool IsPrimitiveType(Type type)
        {
            return type == BYTE.type
                || type == CHAR.type
                || type == DOUBLE.type
                || type == FLOAT.type
                || type == INT.type
                || type == LONG.type
                || type == SHORT.type
                || type == BOOLEAN.type
                || type == VOID.type;
        }

        internal override string SigName => sigName;

        internal override RuntimeClassLoader GetClassLoader() => RuntimeClassLoader.GetBootstrapClassLoader();

        internal override Type TypeAsTBD => type;

        public override string ToString() => "RuntimePrimitiveJavaType[" + sigName + "]";

#if EMITTERS

        internal override void EmitLdind(CodeEmitter il)
        {
            if (this == BOOLEAN)
                il.Emit(OpCodes.Ldind_U1);
            else if (this == BYTE)
                il.Emit(OpCodes.Ldind_U1);
            else if (this == CHAR)
                il.Emit(OpCodes.Ldind_U2);
            else if (this == SHORT)
                il.Emit(OpCodes.Ldind_I2);
            else if (this == INT)
                il.Emit(OpCodes.Ldind_I4);
            else if (this == LONG)
                il.Emit(OpCodes.Ldind_I8);
            else if (this == FLOAT)
                il.Emit(OpCodes.Ldind_R4);
            else if (this == DOUBLE)
                il.Emit(OpCodes.Ldind_R8);
            else
                throw new InternalException();
        }

        internal override void EmitStind(CodeEmitter il)
        {
            if (this == BOOLEAN)
                il.Emit(OpCodes.Stind_I1);
            else if (this == BYTE)
                il.Emit(OpCodes.Stind_I1);
            else if (this == CHAR)
                il.Emit(OpCodes.Stind_I2);
            else if (this == SHORT)
                il.Emit(OpCodes.Stind_I2);
            else if (this == INT)
                il.Emit(OpCodes.Stind_I4);
            else if (this == LONG)
                il.Emit(OpCodes.Stind_I8);
            else if (this == FLOAT)
                il.Emit(OpCodes.Stind_R4);
            else if (this == DOUBLE)
                il.Emit(OpCodes.Stind_R8);
            else
                throw new InternalException();
        }

#endif

    }

}
