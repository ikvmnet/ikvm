/*
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
using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    /// <summary>
    /// Field wrapper implementation for standard fields.
    /// </summary>
    sealed class RuntimeSimpleJavaField : RuntimeJavaField
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
        internal RuntimeSimpleJavaField(RuntimeJavaType declaringType, RuntimeJavaType fieldType, IFieldSymbol fi, string name, string sig, ExModifiers modifiers) :
            base(declaringType, fieldType, name, sig, modifiers, fi)
        {
            Debug.Assert(!(fieldType == declaringType.Context.PrimitiveJavaTypeFactory.DOUBLE || fieldType == declaringType.Context.PrimitiveJavaTypeFactory.LONG) || !IsVolatile);
        }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        internal override object GetValue(object obj)
        {
            return GetField().AsReflection().GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            GetField().AsReflection().SetValue(obj, value);
        }

#endif

#if EMITTERS

        /// <inheritdoc />
        protected override void EmitGetImpl(CodeEmitter il)
        {
            var fi = GetField();

            if (IsStatic == false && DeclaringType.IsNonPrimitiveValueType)
                il.Emit(OpCodes.Unbox, DeclaringType.TypeAsLocalOrStackType);

            // conduct load operation with volatile tag if field is volatile
            if (IsVolatile)
            {
                il.EmitMemoryBarrier();
                il.Emit(OpCodes.Volatile);
            }

            if (IsStatic)
                il.Emit(OpCodes.Ldsfld, fi);
            else
                il.Emit(OpCodes.Ldfld, fi);
        }

        /// <inheritdoc />
        protected override void EmitSetImpl(CodeEmitter il)
        {
            var fi = GetField();

            if (IsStatic == false && DeclaringType.IsNonPrimitiveValueType)
            {
                var value = il.DeclareLocal(FieldTypeWrapper.TypeAsLocalOrStackType);
                il.Emit(OpCodes.Stloc, value);
                il.Emit(OpCodes.Unbox, DeclaringType.TypeAsLocalOrStackType);
                il.Emit(OpCodes.Ldloc, value);
            }

            // conduct store operation with volatile tag if field is volatile
            if (IsVolatile)
                il.Emit(OpCodes.Volatile);

            if (IsStatic)
                il.Emit(OpCodes.Stsfld, fi);
            else
                il.Emit(OpCodes.Stfld, fi);

            if (IsVolatile)
                il.EmitMemoryBarrier();
        }

        /// <inheritdoc />
        protected override void EmitUnsafeGetImpl(CodeEmitter il)
        {
            var fi = GetField();

            if (IsStatic)
            {
                if (IsFinal)
                {
                    il.Emit(OpCodes.Ldsflda, fi);
                    il.Emit(OpCodes.Call, DeclaringType.Context.Resolver.ResolveRuntimeType(typeof(RuntimeSimpleJavaField).FullName).GetMethod(nameof(UnsafeGetImplByRefNoInline), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).MakeGenericMethod(fi.FieldType));
                }
                else
                {
                    il.Emit(OpCodes.Ldsfld, fi);
                }
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsLocalOrStackType);

                il.Emit(OpCodes.Ldfld, fi);
            }
        }

        /// <summary>
        /// Non-inlinable implementation to retrieve the value of the given reference. Prevents inlining of read-only fields by the JIT.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static T UnsafeGetImplByRefNoInline<T>(ref T r)
        {
            return r;
        }

        /// <inheritdoc />
        protected override void EmitUnsafeSetImpl(CodeEmitter il)
        {
            var fi = GetField();

            if (IsStatic == false && DeclaringType.IsNonPrimitiveValueType)
            {
                var value = il.DeclareLocal(FieldTypeWrapper.TypeAsLocalOrStackType);
                il.Emit(OpCodes.Stloc, value);
                il.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
                il.Emit(OpCodes.Ldloc, value);
            }

            if (IsStatic)
            {
                il.Emit(OpCodes.Stsfld, fi);
            }
            else
            {
                il.Emit(OpCodes.Stfld, fi);
            }
        }

        /// <inheritdoc />
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
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsLocalOrStackType);

                il.Emit(OpCodes.Ldflda, fi);
            }

            if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadBoolean);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.BYTE)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadByte);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.CHAR)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadChar);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.SHORT)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadShort);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.INT)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadInt);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadLong);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.FLOAT)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadFloat);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileReadDouble);
            else
            {
                il.EmitMemoryBarrier();
                il.Emit(OpCodes.Volatile);
                FieldTypeWrapper.EmitLdind(il);
            }
        }

        /// <inheritdoc />
        protected override void EmitUnsafeVolatileSetImpl(CodeEmitter il)
        {
            var fi = GetField();

            var value = il.DeclareLocal(FieldTypeWrapper.TypeAsLocalOrStackType);
            il.Emit(OpCodes.Stloc, value);

            if (IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsLocalOrStackType);

                il.Emit(OpCodes.Ldflda, fi);
            }

            il.Emit(OpCodes.Ldloc, value);

            if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteBoolean);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.BYTE)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteByte);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.CHAR)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteChar);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.SHORT)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteShort);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.INT)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteInt);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteLong);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.FLOAT)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteFloat);
            else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.VolatileWriteDouble);
            else
            {
                il.Emit(OpCodes.Volatile);
                FieldTypeWrapper.EmitStind(il);
                il.EmitMemoryBarrier();
            }
        }

        protected override void EmitUnsafeCompareAndSwapImpl(CodeEmitter il)
        {
            var fi = GetField();

            var update = il.AllocTempLocal(FieldTypeWrapper.TypeAsLocalOrStackType);
            var expect = il.AllocTempLocal(FieldTypeWrapper.TypeAsLocalOrStackType);

            il.Emit(OpCodes.Stloc, update);
            il.Emit(OpCodes.Stloc, expect);

            if (IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                    il.Emit(OpCodes.Unbox, DeclaringType.TypeAsLocalOrStackType);

                il.Emit(OpCodes.Ldflda, fi);
            }

            il.Emit(OpCodes.Ldloc, expect);
            il.Emit(OpCodes.Ldloc, update);

            if (FieldTypeWrapper.IsPrimitive)
            {
                if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.INT)
                {
                    il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.CompareAndSwapInt);
                }
                else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.LONG)
                {
                    il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.CompareAndSwapLong);
                }
                else if (FieldTypeWrapper == DeclaringType.Context.PrimitiveJavaTypeFactory.DOUBLE)
                {
                    il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.CompareAndSwapDouble);
                }
                else
                {
                    throw new InternalException();
                }
            }
            else
            {
                il.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.CompareAndSwapObject);
            }

            il.ReleaseTempLocal(expect);
            il.ReleaseTempLocal(update);
        }

#endif

    }

}
