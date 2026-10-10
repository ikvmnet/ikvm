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

    abstract class PropertyInfo : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal PropertyInfo()
        {
        }

        public sealed override MemberTypes MemberType => MemberTypes.Property;

        public abstract PropertyAttributes Attributes { get; }

        public abstract bool CanRead { get; }

        public abstract bool CanWrite { get; }

        public abstract MethodInfo GetGetMethod(bool nonPublic);

        public abstract MethodInfo GetSetMethod(bool nonPublic);

        public abstract MethodInfo[] GetAccessors(bool nonPublic);

        public abstract object GetRawConstantValue();

        internal abstract bool IsPublic { get; }

        internal abstract bool IsNonPrivate { get; }

        internal abstract bool IsStatic { get; }

        internal abstract PropertySignature PropertySignature { get; }

        /// <summary>
        /// An index parameter of a property. Like System.Reflection, name, attributes, default value, custom attributes
        /// and marshaling come from the matching parameter of an accessor when there is one.
        /// </summary>
        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly PropertyInfo _property;
            readonly int _parameter;
            readonly ParameterInfo _accessor;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="property"></param>
            /// <param name="parameter"></param>
            /// <param name="accessor">The matching accessor parameter, or <c>null</c> if the property has no accessor.</param>
            internal ParameterInfoImpl(PropertyInfo property, int parameter, ParameterInfo accessor)
            {
                _property = property;
                _parameter = parameter;
                _accessor = accessor;
            }

            public override string Name => _accessor?.Name;

            public override Type ParameterType => _property.PropertySignature.GetParameter(_parameter);

            public override ParameterAttributes Attributes => _accessor?.Attributes ?? ParameterAttributes.None;

            public override int Position => _parameter;

            public override object RawDefaultValue => _accessor != null ? _accessor.RawDefaultValue : throw new InvalidOperationException();

            public override CustomModifiers __GetCustomModifiers() => _property.PropertySignature.GetParameterCustomModifiers(_parameter);

            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                if (_accessor != null)
                    return _accessor.__TryGetFieldMarshal(out fieldMarshal);

                fieldMarshal = new FieldMarshal();
                return false;
            }

            public override MemberInfo Member => _property;

            public override int MetadataToken => _accessor?.MetadataToken ?? 0x08000000;

            public override Module Module => _property.Module;

        }

        public virtual ParameterInfo[] GetIndexParameters()
        {
            var count = PropertySignature.ParameterCount;
            if (count == 0)
                return [];

            // the getter has the index parameters; the setter has them followed by the value
            var accessors = GetGetMethod(true)?.GetParameters() ?? GetSetMethod(true)?.GetParameters();
            if (accessors != null && accessors.Length < count)
                accessors = null;

            var parameters = new ParameterInfo[count];
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoImpl(this, i, accessors?[i]);

            return parameters;
        }

        public Type PropertyType => PropertySignature.PropertyType;

        public CustomModifiers __GetCustomModifiers() => PropertySignature.GetCustomModifiers();

        public Type[] GetRequiredCustomModifiers() => __GetCustomModifiers().GetRequired();

        public Type[] GetOptionalCustomModifiers() => __GetCustomModifiers().GetOptional();

        public bool IsSpecialName => (Attributes & PropertyAttributes.SpecialName) != 0;

        public MethodInfo GetMethod => GetGetMethod(true);

        public MethodInfo SetMethod => GetSetMethod(true);

        public MethodInfo GetGetMethod() => GetGetMethod(false);

        public MethodInfo GetSetMethod() => GetSetMethod(false);

        public MethodInfo[] GetAccessors() => GetAccessors(false);

        internal virtual PropertyInfo BindTypeParameters(Type type) => new GenericPropertyInfo(DeclaringType.BindTypeParameters(type), this);

        public override string ToString() => DeclaringType.ToString() + " " + Name;

        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return IsNonPrivate
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

        internal sealed override MemberInfo SetReflectedType(Type type) => new PropertyInfoWithReflectedType(type, this);

        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            // properties don't have pseudo custom attributes
            null;

    }

}
