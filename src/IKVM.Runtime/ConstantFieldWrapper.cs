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
using System;
using System.Diagnostics;

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

    /// <summary>
    /// Wraps a const field.
    /// </summary>
    sealed class ConstantFieldWrapper : RuntimeJavaField
    {

        // NOTE this field wrapper can represent a .NET enum, but in that case "constant" contains the raw constant value (i.e. the boxed underlying primitive value, not a boxed enum)

        /// <summary>
        /// Reference to the constant. In the case of a primitive type or enum, this is the boxed underlying value.
        /// </summary>
        object constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="fieldType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="modifiers"></param>
        /// <param name="field"></param>
        /// <param name="constant"></param>
        /// <param name="flags"></param>
        /// <exception cref="InternalException"></exception>
        internal ConstantFieldWrapper(RuntimeJavaType declaringType, RuntimeJavaType fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, object constant, MemberFlags flags) :
            base(declaringType, fieldType, name, sig, modifiers, field, flags)
        {
            if (IsStatic == false)
                throw new InternalException();

            Debug.Assert(constant == null || constant.GetType().IsPrimitive || constant is string);
            this.constant = constant;
        }

        internal object GetConstantValue()
        {
            if (constant == null)
            {
                var field = GetField();
                constant = field.GetRawConstantValue();
            }

            return constant;
        }

#if EMITTERS

        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            // Reading a field should trigger the cctor, but since we're inlining the value
            // we have to trigger it explicitly
            DeclaringType.EmitRunClassConstructor(ilgen);

            // NOTE even though you're not supposed to access a constant static final (the compiler is supposed
            // to inline them), we have to support it (because it does happen, e.g. if the field becomes final
            // after the referencing class was compiled, or when we're accessing an unsigned primitive .NET field)
            var v = GetConstantValue();
            if (v == null)
            {
                ilgen.Emit(OpCodes.Ldnull);
            }
            else if (constant is int || constant is short || constant is ushort || constant is byte || constant is sbyte || constant is char || constant is bool)
            {
                ilgen.EmitLdc_I4(((IConvertible)constant).ToInt32(null));
            }
            else if (constant is string)
            {
                ilgen.Emit(OpCodes.Ldstr, (string)constant);
            }
            else if (constant is float)
            {
                ilgen.EmitLdc_R4((float)constant);
            }
            else if (constant is double)
            {
                ilgen.EmitLdc_R8((double)constant);
            }
            else if (constant is long)
            {
                ilgen.EmitLdc_I8((long)constant);
            }
            else if (constant is uint)
            {
                ilgen.EmitLdc_I4(unchecked((int)((IConvertible)constant).ToUInt32(null)));
            }
            else if (constant is ulong)
            {
                ilgen.EmitLdc_I8(unchecked((long)(ulong)constant));
            }
            else
            {
                throw new InvalidOperationException(constant.GetType().FullName);
            }
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            // when constant static final fields are updated, the JIT normally doesn't see that (because the
            // constant value is inlined), so we emulate that behavior by emitting a Pop
            ilgen.Emit(OpCodes.Pop);
        }

#endif

#if !EXPORTER && !IMPORTER && !FIRST_PASS

        internal override object GetValue(object obj)
        {
            var field = GetField();
            return FieldTypeWrapper.IsPrimitive || field == null ? GetConstantValue() : field.GetValue(null);
        }

        internal override void SetValue(object obj, object value)
        {

        }

#endif

    }

}
