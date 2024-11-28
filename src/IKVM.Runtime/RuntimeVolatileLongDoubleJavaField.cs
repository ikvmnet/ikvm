﻿/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    /// <summary>
    /// Field wrapper for a field of type 'volatile long' or 'volatile double'.
    /// </summary>
    sealed class RuntimeVolatileLongDoubleJavaField : RuntimeJavaField
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="fieldType"></param>
        /// <param name="fi"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="modifiers"></param>
        internal RuntimeVolatileLongDoubleJavaField(RuntimeJavaType declaringType, RuntimeJavaType fieldType, FieldSymbol fi, string name, string sig, ExModifiers modifiers) :
            base(declaringType, fieldType, name, sig, modifiers, fi)
        {
            if (sig != "J" && sig != "D")
                throw new ArgumentException($"{nameof(RuntimeVolatileLongDoubleJavaField)} expects long or double signature.", nameof(sig));
            if (IsVolatile == false)
                throw new InternalException($"{nameof(RuntimeVolatileLongDoubleJavaField)} requires volatile type.");
        }

#if EMITTERS

        protected override void EmitGetImpl(CodeEmitter il)
        {
            var fi = GetField();

            if (fi.IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);

                il.Emit(OpCodes.Ldflda, fi);
            }

            if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadLong);
            }
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadDouble);
            }
            else
            {
                throw new InternalException($"{nameof(RuntimeVolatileLongDoubleJavaField)} expects long or double.");
            }
        }

        protected override void EmitSetImpl(CodeEmitter il)
        {
            var fi = GetField();

            var value = il.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
            il.Emit(OpCodes.Stloc, value);

            if (fi.IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);

                il.Emit(OpCodes.Ldflda, fi);
            }

            il.Emit(OpCodes.Ldloc, value);

            if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteLong);
            }
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteDouble);
            }
            else
            {
                throw new InternalException($"{nameof(RuntimeVolatileLongDoubleJavaField)} expects long or double.");
            }
        }

        protected override void EmitUnsafeVolatileGetImpl(CodeEmitter il)
        {
            var fi = GetField();

            if (IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);

                il.Emit(OpCodes.Ldflda, fi);
            }

            if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadLong);
            }
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadDouble);
            }
            else
            {
                throw new InternalException($"{nameof(RuntimeVolatileLongDoubleJavaField)} expects long or double.");
            }
        }

        protected override void EmitUnsafeVolatileSetImpl(CodeEmitter il)
        {
            var fi = GetField();

            var value = il.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
            il.Emit(OpCodes.Stloc, value);

            if (IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);

                il.Emit(OpCodes.Ldflda, fi);
            }

            il.Emit(OpCodes.Ldloc, value);

            if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteLong);
            }
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteDouble);
            }
            else
            {
                throw new InternalException($"{nameof(RuntimeVolatileLongDoubleJavaField)} expects long or double.");
            }
        }

        protected override void EmitUnsafeCompareAndSwapImpl(CodeEmitter il)
        {
            var fi = GetField();

            var value = il.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
            il.Emit(OpCodes.Stloc, value);
            var expect = il.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
            il.Emit(OpCodes.Stloc, expect);

            if (IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);

                il.Emit(OpCodes.Ldflda, fi);
            }

            il.Emit(OpCodes.Ldloc, expect);
            il.Emit(OpCodes.Ldloc, value);

            if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.CompareAndSwapLong);
            }
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.CompareAndSwapDouble);
            }
            else
            {
                throw new InternalException($"{nameof(RuntimeVolatileLongDoubleJavaField)} expects long or double.");
            }
        }

#endif

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        internal override object GetValue(object obj)
        {
            throw new InvalidOperationException();
        }

        internal override void SetValue(object obj, object value)
        {
            throw new InvalidOperationException();
        }

#endif

    }

}
