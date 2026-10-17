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
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    class RuntimePrimitiveJavaTypeFactory
    {

        readonly RuntimeContext context;

        internal readonly RuntimePrimitiveJavaType BYTE;
        internal readonly RuntimePrimitiveJavaType CHAR;
        internal readonly RuntimePrimitiveJavaType DOUBLE;
        internal readonly RuntimePrimitiveJavaType FLOAT;
        internal readonly RuntimePrimitiveJavaType INT;
        internal readonly RuntimePrimitiveJavaType LONG;
        internal readonly RuntimePrimitiveJavaType SHORT;
        internal readonly RuntimePrimitiveJavaType BOOLEAN;
        internal readonly RuntimePrimitiveJavaType VOID;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public RuntimePrimitiveJavaTypeFactory(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            BYTE = new RuntimePrimitiveJavaType(context, context.Types.Byte, "B");
            CHAR = new RuntimePrimitiveJavaType(context, context.Types.Char, "C");
            DOUBLE = new RuntimePrimitiveJavaType(context, context.Types.Double, "D");
            FLOAT = new RuntimePrimitiveJavaType(context, context.Types.Single, "F");
            INT = new RuntimePrimitiveJavaType(context, context.Types.Int32, "I");
            LONG = new RuntimePrimitiveJavaType(context, context.Types.Int64, "J");
            SHORT = new RuntimePrimitiveJavaType(context, context.Types.Int16, "S");
            BOOLEAN = new RuntimePrimitiveJavaType(context, context.Types.Boolean, "Z");
            VOID = new RuntimePrimitiveJavaType(context, context.Types.Void, "V");
        }

    }

    sealed class RuntimePrimitiveJavaType : RuntimeJavaType
    {

        readonly Type type;
        readonly string sigName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sigName"></param>
        public RuntimePrimitiveJavaType(RuntimeContext context, Type type, string sigName) :
            base(context, TypeFlags.None, Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null)
        {
            this.type = type;
            this.sigName = sigName;
        }

        internal override RuntimeJavaType BaseTypeWrapper => null;

        internal static bool IsPrimitiveType(RuntimeContext context, Type type)
        {
            return type == context.PrimitiveJavaTypeFactory.BYTE.type
                || type == context.PrimitiveJavaTypeFactory.CHAR.type
                || type == context.PrimitiveJavaTypeFactory.DOUBLE.type
                || type == context.PrimitiveJavaTypeFactory.FLOAT.type
                || type == context.PrimitiveJavaTypeFactory.INT.type
                || type == context.PrimitiveJavaTypeFactory.LONG.type
                || type == context.PrimitiveJavaTypeFactory.SHORT.type
                || type == context.PrimitiveJavaTypeFactory.BOOLEAN.type
                || type == context.PrimitiveJavaTypeFactory.VOID.type;
        }

        internal override string SignatureName => sigName;

        internal override RuntimeClassLoader ClassLoader => Context.ClassLoaderFactory.GetBootstrapClassLoader();

        internal override Type TypeAsTBD => type;

        public override string ToString() => "RuntimePrimitiveJavaType[" + sigName + "]";

#if EMITTERS

        internal override void EmitLdind(CodeEmitter il)
        {
            if (this == Context.PrimitiveJavaTypeFactory.BOOLEAN)
                il.Emit(OpCodes.Ldind_U1);
            else if (this == Context.PrimitiveJavaTypeFactory.BYTE)
                il.Emit(OpCodes.Ldind_U1);
            else if (this == Context.PrimitiveJavaTypeFactory.CHAR)
                il.Emit(OpCodes.Ldind_U2);
            else if (this == Context.PrimitiveJavaTypeFactory.SHORT)
                il.Emit(OpCodes.Ldind_I2);
            else if (this == Context.PrimitiveJavaTypeFactory.INT)
                il.Emit(OpCodes.Ldind_I4);
            else if (this == Context.PrimitiveJavaTypeFactory.LONG)
                il.Emit(OpCodes.Ldind_I8);
            else if (this == Context.PrimitiveJavaTypeFactory.FLOAT)
                il.Emit(OpCodes.Ldind_R4);
            else if (this == Context.PrimitiveJavaTypeFactory.DOUBLE)
                il.Emit(OpCodes.Ldind_R8);
            else
                throw new InternalException();
        }

        internal override void EmitStind(CodeEmitter il)
        {
            if (this == Context.PrimitiveJavaTypeFactory.BOOLEAN)
                il.Emit(OpCodes.Stind_I1);
            else if (this == Context.PrimitiveJavaTypeFactory.BYTE)
                il.Emit(OpCodes.Stind_I1);
            else if (this == Context.PrimitiveJavaTypeFactory.CHAR)
                il.Emit(OpCodes.Stind_I2);
            else if (this == Context.PrimitiveJavaTypeFactory.SHORT)
                il.Emit(OpCodes.Stind_I2);
            else if (this == Context.PrimitiveJavaTypeFactory.INT)
                il.Emit(OpCodes.Stind_I4);
            else if (this == Context.PrimitiveJavaTypeFactory.LONG)
                il.Emit(OpCodes.Stind_I8);
            else if (this == Context.PrimitiveJavaTypeFactory.FLOAT)
                il.Emit(OpCodes.Stind_R4);
            else if (this == Context.PrimitiveJavaTypeFactory.DOUBLE)
                il.Emit(OpCodes.Stind_R8);
            else
                throw new InternalException();
        }

#endif

    }

}
