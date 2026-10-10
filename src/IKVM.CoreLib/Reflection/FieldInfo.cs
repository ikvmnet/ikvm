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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    /// <summary>
    /// Discovers the attributes of a field and provides access to field metadata.
    /// </summary>
    abstract class FieldInfo : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal FieldInfo()
        {
        }

        /// <summary>
        /// Gets a <see cref="MemberTypes"/> value indicating that this member is a field.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Field;

        /// <summary>
        /// Gets the attributes associated with this field.
        /// </summary>
        public abstract FieldAttributes Attributes { get; }

        /// <summary>
        /// Returns a literal value associated with the field by a compiler.
        /// </summary>
        /// <returns>An <see cref="object"/> that contains the literal value associated with the field.</returns>
        public abstract object GetRawConstantValue();

        /// <summary>
        /// Gets the signature of the field, which carries its type and custom modifiers.
        /// </summary>
        internal abstract FieldSignature FieldSignature { get; }

        /// <summary>
        /// Gets the type of this field object.
        /// </summary>
        public Type FieldType => FieldSignature.FieldType;

        /// <summary>
        /// Gets the required and optional custom modifiers of the field type.
        /// </summary>
        /// <returns>The custom modifiers of the field type.</returns>
        public CustomModifiers __GetCustomModifiers() => FieldSignature.GetCustomModifiers();

        /// <summary>
        /// Gets an array of types that identify the optional custom modifiers of the field.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that identify the optional custom modifiers of the current field.</returns>
        public Type[] GetOptionalCustomModifiers() => __GetCustomModifiers().GetOptional();

        /// <summary>
        /// Gets an array of types that identify the required custom modifiers of the field.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that identify the required custom modifiers of the current field.</returns>
        public Type[] GetRequiredCustomModifiers() => __GetCustomModifiers().GetRequired();

        /// <summary>
        /// Gets a value indicating whether the field is static.
        /// </summary>
        public bool IsStatic => (Attributes & FieldAttributes.Static) != 0;

        /// <summary>
        /// Gets a value indicating whether the value is written at compile time and cannot be changed.
        /// </summary>
        public bool IsLiteral => (Attributes & FieldAttributes.Literal) != 0;

        /// <summary>
        /// Gets a value indicating whether the field can only be set in the body of the constructor.
        /// </summary>
        public bool IsInitOnly => (Attributes & FieldAttributes.InitOnly) != 0;

        /// <summary>
        /// Gets a value indicating whether this field has the <see cref="FieldAttributes.NotSerialized"/> attribute.
        /// </summary>
        public bool IsNotSerialized => (Attributes & FieldAttributes.NotSerialized) != 0;

        /// <summary>
        /// Gets a value indicating whether the corresponding <see cref="FieldAttributes.SpecialName"/> attribute is set.
        /// </summary>
        public bool IsSpecialName => (Attributes & FieldAttributes.SpecialName) != 0;

        /// <summary>
        /// Gets a value indicating whether the field is public.
        /// </summary>
        public bool IsPublic => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the field is private.
        /// </summary>
        public bool IsPrivate => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by
        /// <see cref="FieldAttributes.Family"/>; that is, the field is visible only within its class and derived classes.
        /// </summary>
        public bool IsFamily => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by
        /// <see cref="FieldAttributes.FamORAssem"/>; that is, the field can be accessed by derived classes wherever they
        /// are, and by classes in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by
        /// <see cref="FieldAttributes.Assembly"/>; that is, the field is visible at most to other types in the same
        /// assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        public bool IsAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by
        /// <see cref="FieldAttributes.FamANDAssem"/>; that is, the field can be accessed from derived classes, but only if
        /// they are in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;

        /// <summary>
        /// Gets a value indicating whether the corresponding <see cref="FieldAttributes.PinvokeImpl"/> attribute is set.
        /// </summary>
        public bool IsPinvokeImpl => (Attributes & FieldAttributes.PinvokeImpl) != 0;

        /// <summary>
        /// Gets the explicit offset of the field within its declaring type's layout, from the <c>FieldLayout</c> table.
        /// </summary>
        /// <param name="offset">When this method returns <see langword="true"/>, the offset of the field in bytes.</param>
        /// <returns><see langword="true"/> if the field has an explicit offset; otherwise, <see langword="false"/>.</returns>
        public abstract bool __TryGetFieldOffset(out int offset);

        /// <summary>
        /// Gets the marshaling information of the field, from the <c>FieldMarshal</c> table.
        /// </summary>
        /// <param name="fieldMarshal">When this method returns <see langword="true"/>, the marshaling information of the field.</param>
        /// <returns><see langword="true"/> if the field has marshaling information; otherwise, <see langword="false"/>.</returns>
        public bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) => FieldMarshal.ReadFieldMarshal(Module, GetCurrentToken(), out fieldMarshal);

        /// <summary>
        /// Gets a token that refers to this field from the specified module, adding a <c>MemberRef</c> to that module if
        /// necessary.
        /// </summary>
        /// <param name="module">The module that will reference the field.</param>
        /// <returns>The metadata token to use for this field in <paramref name="module"/>.</returns>
        internal abstract int ImportTo(Emit.ModuleBuilder module);

        /// <summary>
        /// Returns this field as a member of the constructed generic type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose generic arguments are substituted into the declaring type.</param>
        /// <returns>The field on the constructed declaring type.</returns>
        internal virtual FieldInfo BindTypeParameters(Type type) => new GenericFieldInstance(DeclaringType.BindTypeParameters(type), this);

        /// <inheritdoc />
        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        /// <inheritdoc />
        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return (Attributes & FieldAttributes.FieldAccessMask) > FieldAttributes.Private
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

        /// <inheritdoc />
        internal sealed override MemberInfo SetReflectedType(Type type) => new FieldInfoWithReflectedType(type, this);

        /// <summary>
        /// Gets the pseudo custom attributes of the field: <c>MarshalAsAttribute</c> from its marshaling information and
        /// <c>FieldOffsetAttribute</c> from its explicit offset.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter by, or <see langword="null"/> for all.</param>
        /// <returns>The pseudo custom attributes that match <paramref name="attributeType"/>.</returns>
        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType)
        {
            var module = Module;
            var list = new List<CustomAttributeData>();

            if (attributeType == null || attributeType.IsAssignableFrom(Module.Universe.System_Runtime_InteropServices_MarshalAsAttribute))
                if (__TryGetFieldMarshal(out var spec))
                    if (CustomAttributeData.CreateMarshalAsPseudoCustomAttribute(module, spec) is { } pseudo)
                        list.Add(pseudo);

            if (attributeType == null || attributeType.IsAssignableFrom(Module.Universe.System_Runtime_InteropServices_FieldOffsetAttribute))
                if (__TryGetFieldOffset(out var offset))
                    if (CustomAttributeData.CreateFieldOffsetPseudoCustomAttribute(module, offset) is { } pseudo)
                        list.Add(pseudo);

            return list;
        }

    }

}
