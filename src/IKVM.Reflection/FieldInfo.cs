/*
  Copyright (C) 2009-2012 Jeroen Frijters

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

    public abstract class FieldInfo : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal FieldInfo()
        {

        }

        public sealed override MemberTypes MemberType
        {
            get { return MemberTypes.Field; }
        }

        public abstract FieldAttributes Attributes { get; }

        public abstract void __GetDataFromRVA(byte[] data, int offset, int length);

        public abstract int __FieldRVA { get; }

        public abstract object GetRawConstantValue();

        internal abstract FieldSignature FieldSignature { get; }

        public Type FieldType
        {
            get { return this.FieldSignature.FieldType; }
        }

        public CustomModifiers __GetCustomModifiers()
        {
            return this.FieldSignature.GetCustomModifiers();
        }

        public Type[] GetOptionalCustomModifiers()
        {
            return __GetCustomModifiers().GetOptional();
        }

        public Type[] GetRequiredCustomModifiers()
        {
            return __GetCustomModifiers().GetRequired();
        }

        public bool IsStatic
        {
            get { return (Attributes & FieldAttributes.Static) != 0; }
        }

        public bool IsLiteral
        {
            get { return (Attributes & FieldAttributes.Literal) != 0; }
        }

        public bool IsInitOnly
        {
            get { return (Attributes & FieldAttributes.InitOnly) != 0; }
        }

        public bool IsNotSerialized
        {
            get { return (Attributes & FieldAttributes.NotSerialized) != 0; }
        }

        public bool IsSpecialName
        {
            get { return (Attributes & FieldAttributes.SpecialName) != 0; }
        }

        public bool IsPublic
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public; }
        }

        public bool IsPrivate
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private; }
        }

        public bool IsFamily
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family; }
        }

        public bool IsFamilyOrAssembly
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem; }
        }

        public bool IsAssembly
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly; }
        }

        public bool IsFamilyAndAssembly
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem; }
        }

        public bool IsPinvokeImpl
        {
            get { return (Attributes & FieldAttributes.PinvokeImpl) != 0; }
        }

        public virtual FieldInfo __GetFieldOnTypeDefinition()
        {
            return this;
        }

        public abstract bool __TryGetFieldOffset(out int offset);

        public bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
        {
            return FieldMarshal.ReadFieldMarshal(this.Module, GetCurrentToken(), out fieldMarshal);
        }

        internal abstract int ImportTo(Emit.ModuleBuilder module);

        internal virtual FieldInfo BindTypeParameters(Type type)
        {
            return new GenericFieldInstance(this.DeclaringType.BindTypeParameters(type), this);
        }

        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return (Attributes & FieldAttributes.FieldAccessMask) > FieldAttributes.Private
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

        internal sealed override MemberInfo SetReflectedType(Type type)
        {
            return new FieldInfoWithReflectedType(type, this);
        }

        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType)
        {
            var module = Module;
            var list = new List<CustomAttributeData>();

            if (attributeType == null || attributeType.IsAssignableFrom(module.universe.System_Runtime_InteropServices_MarshalAsAttribute))
                if (__TryGetFieldMarshal(out var spec))
                    list.Add(CustomAttributeData.CreateMarshalAsPseudoCustomAttribute(module, spec));

            if (attributeType == null || attributeType.IsAssignableFrom(module.universe.System_Runtime_InteropServices_FieldOffsetAttribute))
                if (__TryGetFieldOffset(out var offset))
                    list.Add(CustomAttributeData.CreateFieldOffsetPseudoCustomAttribute(module, offset));

            return list;
        }

    }

}
