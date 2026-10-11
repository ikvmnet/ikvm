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

    /// <summary>
    /// Discovers the attributes of a property and provides access to property metadata.
    /// </summary>
    abstract class PropertyInfo : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal PropertyInfo()
        {
        }

        /// <summary>
        /// Gets a <see cref="MemberTypes"/> value indicating that this member is a property.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Property;

        /// <summary>
        /// Gets the attributes for this property.
        /// </summary>
        public abstract PropertyAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        public abstract bool CanRead { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be written to.
        /// </summary>
        public abstract bool CanWrite { get; }

        /// <summary>
        /// When overridden in a derived class, returns the public or non-public <c>get</c> accessor for this property.
        /// </summary>
        /// <param name="nonPublic">
        /// Indicates whether a non-public <c>get</c> accessor should be returned. <see langword="true"/> if a non-public
        /// accessor is to be returned; otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>
        /// A <see cref="MethodInfo"/> object representing the <c>get</c> accessor for this property, if
        /// <paramref name="nonPublic"/> is <see langword="true"/>. Returns <see langword="null"/> if
        /// <paramref name="nonPublic"/> is <see langword="false"/> and the <c>get</c> accessor is non-public, or if
        /// there is no <c>get</c> accessor.
        /// </returns>
        public abstract MethodInfo GetGetMethod(bool nonPublic);

        /// <summary>
        /// When overridden in a derived class, returns the <c>set</c> accessor for this property.
        /// </summary>
        /// <param name="nonPublic">
        /// Indicates whether the accessor should be returned if it is non-public. <see langword="true"/> if a
        /// non-public accessor is to be returned; otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>
        /// The <c>set</c> accessor for this property, or <see langword="null"/> if <paramref name="nonPublic"/> is
        /// <see langword="false"/> and the <c>set</c> accessor is non-public, or if there is no <c>set</c> accessor.
        /// </returns>
        public abstract MethodInfo GetSetMethod(bool nonPublic);

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public <c>get</c> and <c>set</c>
        /// accessors of the property.
        /// </summary>
        /// <param name="nonPublic">
        /// Indicates whether non-public methods should be returned in the returned array. <see langword="true"/> if
        /// non-public methods are to be included; otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>
        /// An array whose elements reflect the <c>get</c> and <c>set</c> accessors of the property, and any other
        /// accessors associated with it.
        /// </returns>
        public abstract MethodInfo[] GetAccessors(bool nonPublic);

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <returns>An <see cref="object"/> that contains the literal value associated with the property.</returns>
        public abstract object GetRawConstantValue();

        /// <summary>
        /// Gets whether any accessor of the property is public.
        /// </summary>
        internal abstract bool IsPublic { get; }

        /// <summary>
        /// Gets whether any accessor of the property is not private, which decides whether the property is visible
        /// from derived types.
        /// </summary>
        internal abstract bool IsNonPrivate { get; }

        /// <summary>
        /// Gets whether the property is static.
        /// </summary>
        internal abstract bool IsStatic { get; }

        /// <summary>
        /// Gets the signature of the property.
        /// </summary>
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
            /// <param name="property">The property the parameter belongs to.</param>
            /// <param name="parameter">The zero-based position of the index parameter.</param>
            /// <param name="accessor">The matching accessor parameter, or <see langword="null"/> if the property has no accessor.</param>
            internal ParameterInfoImpl(PropertyInfo property, int parameter, ParameterInfo accessor)
            {
                _property = property;
                _parameter = parameter;
                _accessor = accessor;
            }

            /// <summary>
            /// Gets the name of the matching accessor parameter, or <see langword="null"/> if there is none.
            /// </summary>
            public override string Name => _accessor?.Name;

            /// <summary>
            /// Gets the type of the parameter, as recorded in the property signature.
            /// </summary>
            public override Type ParameterType => _property.PropertySignature.GetParameter(_parameter);

            /// <summary>
            /// Gets the attributes of the matching accessor parameter, or <see cref="ParameterAttributes.None"/> if there
            /// is none.
            /// </summary>
            public override ParameterAttributes Attributes => _accessor?.Attributes ?? ParameterAttributes.None;

            /// <inheritdoc />
            public override int Position => _parameter;

            /// <summary>
            /// Gets the default value of the matching accessor parameter.
            /// </summary>
            /// <exception cref="InvalidOperationException">The property has no accessor.</exception>
            public override object RawDefaultValue => _accessor != null ? _accessor.RawDefaultValue : throw new InvalidOperationException();

            /// <summary>
            /// Gets the custom modifiers of the parameter, as recorded in the property signature.
            /// </summary>
            /// <returns>The custom modifiers.</returns>
            public override CustomModifiers __GetCustomModifiers() => _property.PropertySignature.GetParameterCustomModifiers(_parameter);

            /// <summary>
            /// Gets the marshaling information of the matching accessor parameter.
            /// </summary>
            /// <param name="fieldMarshal">When this method returns, the marshaling information, if any.</param>
            /// <returns>
            /// <see langword="true"/> if there is a matching accessor parameter with marshaling information; otherwise,
            /// <see langword="false"/>.
            /// </returns>
            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                if (_accessor != null)
                    return _accessor.__TryGetFieldMarshal(out fieldMarshal);

                fieldMarshal = new FieldMarshal();
                return false;
            }

            /// <summary>
            /// Gets the property the parameter belongs to.
            /// </summary>
            public override MemberInfo Member => _property;

            /// <summary>
            /// Gets the metadata token of the matching accessor parameter, or the nil Param token if there is none.
            /// </summary>
            public override int MetadataToken => _accessor?.MetadataToken ?? 0x08000000;

            /// <inheritdoc />
            public override Module Module => _property.Module;

        }

        /// <summary>
        /// When overridden in a derived class, returns an array of all the index parameters for the property.
        /// </summary>
        /// <returns>
        /// An array of type <see cref="ParameterInfo"/> containing the parameters for the indexes. If the property is
        /// not indexed, the array has 0 (zero) elements.
        /// </returns>
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

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        public Type PropertyType => PropertySignature.PropertyType;

        /// <summary>
        /// Gets the custom modifiers on the property type.
        /// </summary>
        /// <returns>The custom modifiers.</returns>
        public CustomModifiers __GetCustomModifiers() => PropertySignature.GetCustomModifiers();

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the property.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that identify the required custom modifiers of the current property.</returns>
        public Type[] GetRequiredCustomModifiers() => __GetCustomModifiers().GetRequired();

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the property.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that identify the optional custom modifiers of the current property.</returns>
        public Type[] GetOptionalCustomModifiers() => __GetCustomModifiers().GetOptional();

        /// <summary>
        /// Gets a value indicating whether the property is the special name.
        /// </summary>
        public bool IsSpecialName => (Attributes & PropertyAttributes.SpecialName) != 0;

        /// <summary>
        /// Gets the <c>get</c> accessor for this property.
        /// </summary>
        public MethodInfo GetMethod => GetGetMethod(true);

        /// <summary>
        /// Gets the <c>set</c> accessor for this property.
        /// </summary>
        public MethodInfo SetMethod => GetSetMethod(true);

        /// <summary>
        /// Returns the public <c>get</c> accessor for this property.
        /// </summary>
        /// <returns>
        /// A <see cref="MethodInfo"/> object representing the public <c>get</c> accessor for this property, or
        /// <see langword="null"/> if the <c>get</c> accessor is non-public or does not exist.
        /// </returns>
        public MethodInfo GetGetMethod() => GetGetMethod(false);

        /// <summary>
        /// Returns the public <c>set</c> accessor for this property.
        /// </summary>
        /// <returns>
        /// The <see cref="MethodInfo"/> object representing the <c>set</c> method for this property if the <c>set</c>
        /// accessor is public, or <see langword="null"/> if the <c>set</c> accessor is not public.
        /// </returns>
        public MethodInfo GetSetMethod() => GetSetMethod(false);

        /// <summary>
        /// Returns an array whose elements reflect the public <c>get</c> and <c>set</c> accessors of the property.
        /// </summary>
        /// <returns>
        /// An array of <see cref="MethodInfo"/> objects that reflect the public <c>get</c> and <c>set</c> accessors of
        /// the property, if found; otherwise, an empty array.
        /// </returns>
        public MethodInfo[] GetAccessors() => GetAccessors(false);

        /// <summary>
        /// Returns this property as a member of the given constructed generic type.
        /// </summary>
        /// <param name="type">The constructed type that supplies the type arguments.</param>
        /// <returns>The property, bound to the type arguments.</returns>
        internal virtual PropertyInfo BindTypeParameters(Type type) => new GenericPropertyInfo(DeclaringType.BindTypeParameters(type), this);

        /// <summary>
        /// Returns a string that represents the property, in the form <c>DeclaringType Name</c>.
        /// </summary>
        /// <returns>A string that represents the current property.</returns>
        public override string ToString() => DeclaringType.ToString() + " " + Name;

        /// <inheritdoc />
        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        /// <inheritdoc />
        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return IsNonPrivate
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

        /// <inheritdoc />
        internal sealed override MemberInfo SetReflectedType(Type type) => new PropertyInfoWithReflectedType(type, this);

        /// <summary>
        /// Returns <see langword="null"/>, because properties have no pseudo custom attributes.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter on, or <see langword="null"/> for all.</param>
        /// <returns><see langword="null"/>.</returns>
        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            // properties don't have pseudo custom attributes
            null;

    }

}
