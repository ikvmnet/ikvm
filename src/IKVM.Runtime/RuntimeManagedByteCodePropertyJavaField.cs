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
    /// Represents a .NET property defined in Java with the 'ikvm.lang.Property' annotation.
    /// </summary>
    sealed class RuntimeManagedByteCodePropertyJavaField : RuntimeJavaField
    {

        readonly PropertyInfo property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="property"></param>
        /// <param name="modifiers"></param>
        internal RuntimeManagedByteCodePropertyJavaField(RuntimeJavaType declaringType, PropertyInfo property, ExModifiers modifiers) :
            base(declaringType, declaringType.Context.ClassLoaderFactory.GetJavaTypeFromType(property.PropertyType), property.Name, declaringType.Context.ClassLoaderFactory.GetJavaTypeFromType(property.PropertyType).SignatureName, modifiers, null)
        {
            this.property = property;
        }

        internal PropertyInfo GetProperty()
        {
            return property;
        }

#if EMITTERS

        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            var getter = property.GetGetMethod(true);
            if (getter == null)
                RuntimeByteCodePropertyJavaField.EmitThrowNoSuchMethodErrorForGetter(ilgen, FieldTypeWrapper, this);
            else if (getter.IsStatic)
                ilgen.Emit(OpCodes.Call, getter);
            else
                ilgen.Emit(OpCodes.Callvirt, getter);
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            var setter = property.GetSetMethod(true);
            if (setter == null)
            {
                if (IsFinal)
                {
                    ilgen.Emit(OpCodes.Pop);
                    if (IsStatic == false)
                    {
                        ilgen.Emit(OpCodes.Pop);
                    }
                }
                else
                {
                    RuntimeByteCodePropertyJavaField.EmitThrowNoSuchMethodErrorForSetter(ilgen, this);
                }
            }
            else if (setter.IsStatic)
            {
                ilgen.Emit(OpCodes.Call, setter);
            }
            else
            {
                ilgen.Emit(OpCodes.Callvirt, setter);
            }
        }

#endif

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        internal override object GetValue(object obj)
        {
            var getter = property.GetGetMethod(true);
            if (getter == null)
                throw new java.lang.NoSuchMethodError();

            return getter.Invoke(obj, new object[0]);
        }

        internal override void SetValue(object obj, object value)
        {
            var setter = property.GetSetMethod(true);
            if (setter == null)
                throw new java.lang.NoSuchMethodError();

            setter.Invoke(obj, new object[] { value });
        }

#endif

    }

}
