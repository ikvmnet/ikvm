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

    sealed class RuntimeManagedByteCodeAccessStubJavaField : RuntimeJavaField
    {

        readonly MethodInfo getter;
        readonly MethodInfo setter;

        static Modifiers GetModifiers(PropertyInfo property)
        {
            // NOTE we only support the subset of modifiers that is expected for "access stub" properties
            var getter = property.GetGetMethod(true);
            var modifiers = getter.IsPublic ? Modifiers.Public : Modifiers.Protected;
            if (!property.CanWrite)
                modifiers |= Modifiers.Final;
            if (getter.IsStatic)
                modifiers |= Modifiers.Static;

            return modifiers;
        }

        /// <summary>
        /// Initializes a new instance. This constructor is used for type 1 access stubs.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="property"></param>
        /// <param name="propertyType"></param>
        internal RuntimeManagedByteCodeAccessStubJavaField(RuntimeJavaType wrapper, PropertyInfo property, RuntimeJavaType propertyType) :
            this(wrapper, property, null, propertyType, GetModifiers(property), MemberFlags.HideFromReflection | MemberFlags.AccessStub)
        {

        }

        /// <summary>
        /// Initializes a new instance. This constructor is used for type 2 access stubs.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="property"></param>
        /// <param name="field"></param>
        /// <param name="propertyType"></param>
        internal RuntimeManagedByteCodeAccessStubJavaField(RuntimeJavaType wrapper, PropertyInfo property, FieldInfo field, RuntimeJavaType propertyType) :
            this(wrapper, property, field, propertyType, wrapper.Context.AttributeHelper.GetModifiersAttribute(property).Modifiers, MemberFlags.AccessStub)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="property"></param>
        /// <param name="field"></param>
        /// <param name="propertyType"></param>
        /// <param name="modifiers"></param>
        /// <param name="flags"></param>
        private RuntimeManagedByteCodeAccessStubJavaField(RuntimeJavaType wrapper, PropertyInfo property, FieldInfo field, RuntimeJavaType propertyType, Modifiers modifiers, MemberFlags flags) :
            base(wrapper, propertyType, property.Name, propertyType.SignatureName, modifiers, field, flags)
        {
            this.getter = property.GetGetMethod(true);
            this.setter = property.GetSetMethod(true);
        }

#if EMITTERS

        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, getter);
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, setter);
        }

#endif

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        internal override object GetValue(object obj)
        {
            // we can only be invoked on type 2 access stubs (because type 1 access stubs are HideFromReflection), so we know we have a field
            return GetField().GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            // we can only be invoked on type 2 access stubs (because type 1 access stubs are HideFromReflection), so we know we have a field
            GetField().SetValue(obj, value);
        }

#endif 

    }

}
