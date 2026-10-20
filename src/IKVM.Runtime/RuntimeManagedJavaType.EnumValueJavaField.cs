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
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        internal sealed class EnumValueJavaField : RuntimeJavaField
        {

            readonly Type underlyingType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            /// <param name="fieldType"></param>
            internal EnumValueJavaField(RuntimeManagedJavaType tw, RuntimeJavaType fieldType) :
                base(tw, fieldType, "Value", fieldType.SignatureName, new ExModifiers(Modifiers.Public | Modifiers.Final, false), null)
            {
                underlyingType = EnumHelper.GetUnderlyingType(tw.type);
            }

#if EMITTERS

            protected override void EmitGetImpl(CodeEmitter ilgen)
            {
                // NOTE if the reference on the stack is null, we *want* the NullReferenceException, so we don't use TypeWrapper.EmitUnbox
                ilgen.Emit(OpCodes.Unbox, underlyingType);
                ilgen.Emit(OpCodes.Ldobj, underlyingType);
            }

            protected override void EmitSetImpl(CodeEmitter ilgen)
            {
                // NOTE even though the field is final, JNI reflection can still be used to set its value!
                CodeEmitterLocal temp = ilgen.AllocTempLocal(underlyingType);
                ilgen.Emit(OpCodes.Stloc, temp);
                ilgen.Emit(OpCodes.Unbox, underlyingType);
                ilgen.Emit(OpCodes.Ldloc, temp);
                ilgen.Emit(OpCodes.Stobj, underlyingType);
                ilgen.ReleaseTempLocal(temp);
            }

#endif

#if !EXPORTER && !IMPORTER && !FIRST_PASS

            internal override object GetValue(object obj)
            {
                return obj;
            }

            internal override void SetValue(object obj, object value)
            {
                obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)[0].SetValue(obj, value);
            }

#endif

        }

    }

}
