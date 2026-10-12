/*
  Copyright (C) 2008-2011 Jeroen Frijters

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
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

using IKVM.Reflection.Impl;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines and creates new instances of classes during run time.
    /// </summary>
    sealed class TypeBuilder : TypeInfo, ITypeOwner
    {

        /// <summary>
        /// The module builder that owns this type, or the enclosing <see cref="TypeBuilder"/> for a nested type.
        /// </summary>
        readonly ITypeOwner _owner;

        /// <summary>
        /// The TypeDef token of this type.
        /// </summary>
        readonly int _token;

        /// <summary>
        /// The token of the base type, computed when the type is created.
        /// </summary>
        int _extends;

        /// <summary>
        /// The base type set by <see cref="SetParent"/>. When it is <see langword="null"/> and this type is not an
        /// interface, <see cref="BaseType"/> fills it in with <see cref="object"/>.
        /// </summary>
        Type _lazyBaseType;

        /// <summary>
        /// The string heap handle of the name of this type.
        /// </summary>
        readonly StringHandle _typeName;

        /// <summary>
        /// The string heap handle of the namespace of this type, or a nil handle if it has none.
        /// </summary>
        readonly StringHandle _typeNameSpace;

        readonly string _ns;
        readonly string _name;
        readonly List<MethodBuilder> _methods = [];
        readonly List<FieldBuilder> _fields = [];
        List<PropertyBuilder> _properties;
        TypeAttributes _attribs;

        /// <summary>
        /// The generic type parameters defined by <see cref="DefineGenericParameters"/>, or <see langword="null"/> if
        /// the type is not generic.
        /// </summary>
        GenericTypeParameterBuilder[] _gtpb;

        List<Type> _interfaces;

        /// <summary>
        /// The tokens of the implemented interfaces, computed when the type is created.
        /// </summary>
        List<int> _interfaceTokens;

        List<TypeBuilder> _nestedTypes;

        /// <summary>
        /// The (possibly pseudo) tokens of the method bodies and declarations added by <see cref="DefineMethodOverride"/>.
        /// </summary>
        List<(int Body, int Declaration)> _methodImpls;

        /// <summary>
        /// The class size written to the ClassLayout row.
        /// </summary>
        int _size;

        /// <summary>
        /// The packing size written to the ClassLayout row.
        /// </summary>
        short _packingSize;

        /// <summary>
        /// Whether a ClassLayout row is written for this type.
        /// </summary>
        bool _hasLayout;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner">The module builder, or the enclosing type builder for a nested type.</param>
        /// <param name="ns">The namespace of the type, or <see langword="null"/>.</param>
        /// <param name="name">The name of the type.</param>
        internal TypeBuilder(ITypeOwner owner, string ns, string name)
        {
            _owner = owner;
            _token = ModuleBuilder.AllocTypeToken();
            _ns = ns;
            _name = name;
            _typeNameSpace = ns == null ? default : ModuleBuilder.GetOrAddString(ns);
            _typeName = ModuleBuilder.GetOrAddString(name);
            MarkKnownType(ns, name);
        }

        /// <summary>
        /// Defines the parameterless constructor. The constructor defined here will simply call the parameterless constructor of the parent.
        /// </summary>
        /// <param name="attributes">A <see cref="MethodAttributes"/> object representing the attributes to be applied to the constructor.</param>
        /// <returns>The constructor.</returns>
        public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
        {
            var cb = DefineConstructor(attributes, CallingConventions.Standard, Type.EmptyTypes);
            var ilgen = cb.GetILGenerator();
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, BaseType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
            ilgen.Emit(OpCodes.Ret);
            return cb;
        }

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes and signature.
        /// </summary>
        /// <param name="attribs">The attributes of the constructor.</param>
        /// <param name="callConv">The calling convention of the constructor.</param>
        /// <param name="parameterTypes">The parameter types of the constructor.</param>
        /// <returns>The defined constructor.</returns>
        public ConstructorBuilder DefineConstructor(MethodAttributes attribs, CallingConventions callConv, Type[] parameterTypes) =>
            DefineConstructor(attribs, callConv, parameterTypes, null, null);

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes, signature, and custom modifiers.
        /// </summary>
        /// <param name="attribs">The attributes of the constructor.</param>
        /// <param name="callingConvention">The calling convention of the constructor.</param>
        /// <param name="parameterTypes">The parameter types of the constructor.</param>
        /// <param name="requiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter. If a particular parameter has no required custom modifiers, specify <see langword="null"/> instead of an array of types. If none of the parameters have required custom modifiers, specify <see langword="null"/> instead of an array of arrays.</param>
        /// <param name="optionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter. If a particular parameter has no optional custom modifiers, specify <see langword="null"/> instead of an array of types. If none of the parameters have optional custom modifiers, specify <see langword="null"/> instead of an array of arrays.</param>
        /// <returns>The defined constructor.</returns>
        public ConstructorBuilder DefineConstructor(MethodAttributes attribs, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
        {
            attribs |= MethodAttributes.RTSpecialName | MethodAttributes.SpecialName;
            var name = (attribs & MethodAttributes.Static) == 0 ? ConstructorInfo.ConstructorName : ConstructorInfo.TypeConstructorName;
            var mb = DefineMethod(name, attribs, callingConvention, null, null, null, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
            return new ConstructorBuilder(mb);
        }

        /// <summary>
        /// Creates a method builder without a signature and adds it to the methods of this type.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="attributes">The attributes of the method.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <returns>The method builder.</returns>
        MethodBuilder CreateMethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            var mb = new MethodBuilder(this, name, attributes, callingConvention);
            _methods.Add(mb);
            return mb;
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and method signature.
        /// </summary>
        /// <param name="name">The name of the method. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attribs">The attributes of the method.</param>
        /// <param name="returnType">The return type of the method.</param>
        /// <param name="parameterTypes">The types of the parameters of the method.</param>
        /// <returns>The defined method.</returns>
        public MethodBuilder DefineMethod(string name, MethodAttributes attribs, Type returnType, Type[] parameterTypes) =>
            DefineMethod(name, attribs, CallingConventions.Standard, returnType, null, null, parameterTypes, null, null);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, and method signature.
        /// </summary>
        /// <param name="name">The name of the method. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attributes">The attributes of the method.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <param name="returnType">The return type of the method.</param>
        /// <param name="parameterTypes">The types of the parameters of the method.</param>
        /// <returns>A <see cref="MethodBuilder"/> representing the newly defined method.</returns>
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) =>
            DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, method signature, and custom modifiers.
        /// </summary>
        /// <param name="name">The name of the method. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attributes">The attributes of the method.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <param name="returnType">The return type of the method.</param>
        /// <param name="returnTypeRequiredCustomModifiers">An array of types representing the required custom modifiers for the return type of the method, or <see langword="null"/>.</param>
        /// <param name="returnTypeOptionalCustomModifiers">An array of types representing the optional custom modifiers for the return type of the method, or <see langword="null"/>.</param>
        /// <param name="parameterTypes">The types of the parameters of the method.</param>
        /// <param name="parameterTypeRequiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter, or <see langword="null"/>.</param>
        /// <param name="parameterTypeOptionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter, or <see langword="null"/>.</param>
        /// <returns>A <see cref="MethodBuilder"/> object representing the newly added method.</returns>
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            var mb = CreateMethodBuilder(name, attributes, callingConvention);
            mb.SetSignature(returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
            return mb;
        }

        /// <summary>
        /// Specifies a given method body that implements a given method declaration, potentially with a different name.
        /// </summary>
        /// <param name="methodInfoBody">The method body to be used. This should be a <see cref="MethodBuilder"/> object.</param>
        /// <param name="methodInfoDeclaration">The method whose declaration is to be used.</param>
        public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration) =>
            (_methodImpls ??= []).Add((ModuleBuilder.GetMethodToken(methodInfoBody).Token, ModuleBuilder.GetMethodToken(methodInfoDeclaration).Token));

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, and field type.
        /// </summary>
        /// <param name="name">The name of the field. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="fieldType">The type of the field.</param>
        /// <param name="attribs">The attributes of the field.</param>
        /// <returns>The defined field.</returns>
        public FieldBuilder DefineField(string name, Type fieldType, FieldAttributes attribs) => DefineField(name, fieldType, null, null, attribs);

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, field type, and custom modifiers.
        /// </summary>
        /// <param name="fieldName">The name of the field. <paramref name="fieldName"/> cannot contain embedded nulls.</param>
        /// <param name="type">The type of the field.</param>
        /// <param name="requiredCustomModifiers">An array of types representing the required custom modifiers for the field.</param>
        /// <param name="optionalCustomModifiers">An array of types representing the optional custom modifiers for the field.</param>
        /// <param name="attributes">The attributes of the field.</param>
        /// <returns>The defined field.</returns>
        public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes) =>
            __DefineField(fieldName, type, CustomModifiers.FromReqOpt(requiredCustomModifiers, optionalCustomModifiers), attributes);

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, field type, and custom modifiers in their original order.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="type">The type of the field.</param>
        /// <param name="customModifiers">The custom modifiers of the field.</param>
        /// <param name="attributes">The attributes of the field.</param>
        /// <returns>The defined field.</returns>
        public FieldBuilder __DefineField(string fieldName, Type type, CustomModifiers customModifiers, FieldAttributes attributes)
        {
            var fb = new FieldBuilder(this, fieldName, type, customModifiers, attributes);
            _fields.Add(fb);
            return fb;
        }

        /// <summary>
        /// Adds a new property to the type, with the given name and property signature.
        /// </summary>
        /// <param name="name">The name of the property. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attributes">The attributes of the property.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="parameterTypes">The types of the parameters of the property.</param>
        /// <returns>The defined property.</returns>
        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes) =>
            DefineProperty(name, attributes, returnType, null, null, parameterTypes, null, null);

        /// <summary>
        /// Adds a new property to the type, with the given name, property signature, and custom modifiers.
        /// </summary>
        /// <param name="name">The name of the property. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attributes">The attributes of the property.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="returnTypeRequiredCustomModifiers">An array of types representing the required custom modifiers for the return type of the property, or <see langword="null"/>.</param>
        /// <param name="returnTypeOptionalCustomModifiers">An array of types representing the optional custom modifiers for the return type of the property, or <see langword="null"/>.</param>
        /// <param name="parameterTypes">The types of the parameters of the property.</param>
        /// <param name="parameterTypeRequiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter, or <see langword="null"/>.</param>
        /// <param name="parameterTypeOptionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter, or <see langword="null"/>.</param>
        /// <returns>The defined property.</returns>
        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) =>
            DefinePropertyImpl(name, attributes, CallingConventions.Standard, true, returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, parameterTypeRequiredCustomModifiers, Util.NullSafeLength(parameterTypes)));

        /// <summary>
        /// Creates a property builder with the specified signature and adds it to the properties of this type.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="attributes">The attributes of the property.</param>
        /// <param name="callingConvention">The calling convention of the property signature.</param>
        /// <param name="patchCallingConvention"><see langword="true"/> to have the property builder set <see cref="CallingConventions.HasThis"/> from its accessors.</param>
        /// <param name="returnType">The type of the property.</param>
        /// <param name="parameterTypes">The types of the index parameters of the property.</param>
        /// <param name="customModifiers">The custom modifiers of the property type and parameters.</param>
        /// <returns>The defined property.</returns>
        PropertyBuilder DefinePropertyImpl(string name, PropertyAttributes attributes, CallingConventions callingConvention, bool patchCallingConvention, Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            _properties ??= [];
            var sig = PropertySignature.Create(callingConvention, returnType, parameterTypes, customModifiers);
            var pb = new PropertyBuilder(this, name, attributes, sig, patchCallingConvention);
            _properties.Add(pb);
            return pb;
        }

        /// <summary>
        /// Defines a nested type, given its name and attributes.
        /// </summary>
        /// <param name="name">The short name of the type. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attribs">The attributes of the type.</param>
        /// <returns>The defined nested type.</returns>
        public TypeBuilder DefineNestedType(string name, TypeAttributes attribs) => DefineNestedType(name, attribs, null);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.
        /// </summary>
        /// <param name="name">The short name of the type. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attr">The attributes of the type.</param>
        /// <param name="parent">The type that the nested type extends.</param>
        /// <param name="interfaces">The interfaces that the nested type implements.</param>
        /// <returns>The defined nested type.</returns>
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            var tb = DefineNestedType(name, attr, parent);
            if (interfaces != null)
                foreach (Type iface in interfaces)
                    tb.AddInterfaceImplementation(iface);

            return tb;
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, and the type that it extends.
        /// </summary>
        /// <param name="name">The short name of the type. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attr">The attributes of the type.</param>
        /// <param name="parent">The type that the nested type extends.</param>
        /// <returns>The defined nested type.</returns>
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent) => DefineNestedType(name, attr, parent, 0);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the total size of the type, and the type that it extends.
        /// </summary>
        /// <param name="name">The short name of the type. <paramref name="name"/> cannot contain embedded nulls.</param>
        /// <param name="attr">The attributes of the type.</param>
        /// <param name="parent">The type that the nested type extends.</param>
        /// <param name="typeSize">The total size of the type.</param>
        /// <returns>The defined nested type.</returns>
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize) =>
            DefineNestedType(name, attr, parent, PackingSize.Unspecified, typeSize);

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends. If <paramref name="name"/>
        /// contains a dot after its first character, the part before the last dot becomes the namespace of the nested type.
        /// </summary>
        /// <param name="name">The short name of the type, optionally prefixed with a namespace.</param>
        /// <param name="attr">The attributes of the type.</param>
        /// <param name="parent">The type that the nested type extends.</param>
        /// <param name="packSize">The packing size of the type.</param>
        /// <param name="typeSize">The total size of the type.</param>
        /// <returns>The defined nested type.</returns>
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize, int typeSize)
        {
            string ns = null;
            var lastdot = name.LastIndexOf('.');
            if (lastdot > 0)
            {
                ns = name[..lastdot];
                name = name[(lastdot + 1)..];
            }

            var typeBuilder = __DefineNestedType(ns, name);
            typeBuilder.__SetAttributes(attr);
            typeBuilder.SetParent(parent);
            if (packSize != PackingSize.Unspecified || typeSize != 0)
                typeBuilder.__SetLayout((int)packSize, typeSize);

            return typeBuilder;
        }

        /// <summary>
        /// Defines a nested type with the specified namespace and name, without setting its attributes or base type.
        /// </summary>
        /// <param name="ns">The namespace of the nested type, or <see langword="null"/>.</param>
        /// <param name="name">The name of the nested type.</param>
        /// <returns>The defined nested type.</returns>
        public TypeBuilder __DefineNestedType(string ns, string name)
        {
            typeFlags |= TypeFlags.HasNestedTypes;
            var typeBuilder = ModuleBuilder.DefineType(this, ns, name);
            (_nestedTypes ??= []).Add(typeBuilder);
            return typeBuilder;
        }

        /// <summary>
        /// Sets the base type of the type currently under construction.
        /// </summary>
        /// <param name="parent">The new base type, or <see langword="null"/> for <see cref="object"/> (or no base type for an interface).</param>
        public void SetParent(Type parent) => _lazyBaseType = parent;

        /// <summary>
        /// Adds an interface that this type implements.
        /// </summary>
        /// <param name="interfaceType">The interface that this type implements.</param>
        public void AddInterfaceImplementation(Type interfaceType)
        {
            _interfaces ??= [];
            _interfaces.Add(interfaceType);
        }

        /// <summary>
        /// Sets the packing size and class size of this type, so that a ClassLayout row is written for it.
        /// </summary>
        /// <param name="packingSize">The packing size of the type.</param>
        /// <param name="size">The total size of the type.</param>
        public void __SetLayout(int packingSize, int size)
        {
            _packingSize = (short)packingSize;
            _size = size;
            _hasLayout = true;
        }

        /// <summary>
        /// Applies a <see cref="StructLayoutAttribute"/> pseudo-custom attribute by setting the layout and string format
        /// attributes, the packing size and the class size of this type, instead of emitting the attribute.
        /// </summary>
        /// <param name="customBuilder">The decoded <see cref="StructLayoutAttribute"/>.</param>
        void SetStructLayoutPseudoCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            var val = customBuilder.GetConstructorArgument(0);
            var layout = val is short v ? (LayoutKind)v : (LayoutKind)val;
            _packingSize = (short)((int?)customBuilder.GetFieldValue("Pack") ?? 0);
            _size = (int?)customBuilder.GetFieldValue("Size") ?? 0;

            _attribs &= ~TypeAttributes.LayoutMask;
            switch (layout)
            {
                case LayoutKind.Auto:
                    _attribs |= TypeAttributes.AutoLayout;
                    break;
                case LayoutKind.Explicit:
                    _attribs |= TypeAttributes.ExplicitLayout;
                    break;
                case LayoutKind.Sequential:
                    _attribs |= TypeAttributes.SequentialLayout;
                    break;
            }

            _attribs &= ~TypeAttributes.StringFormatMask;

            switch (customBuilder.GetFieldValue<CharSet>("CharSet") ?? CharSet.None)
            {
                case CharSet.None:
                case CharSet.Ansi:
                    _attribs |= TypeAttributes.AnsiClass;
                    break;
                case CharSet.Auto:
                    _attribs |= TypeAttributes.AutoClass;
                    break;
                case CharSet.Unicode:
                    _attribs |= TypeAttributes.UnicodeClass;
                    break;
            }

            _hasLayout = _packingSize != 0 || _size != 0;
        }

        /// <summary>
        /// Sets a custom attribute using a custom attribute builder. Pseudo-custom attributes are applied to the type attributes
        /// or layout instead of being emitted.
        /// </summary>
        /// <param name="customBuilder">An instance of a helper class to define the custom attribute.</param>
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            switch (customBuilder.KnownCA)
            {
                case KnownCA.StructLayoutAttribute:
                    SetStructLayoutPseudoCustomAttribute(customBuilder.DecodeBlob(Assembly));
                    break;
                case KnownCA.SerializableAttribute:
                    _attribs |= TypeAttributes.Serializable;
                    break;
                case KnownCA.ComImportAttribute:
                    _attribs |= TypeAttributes.Import;
                    break;
                case KnownCA.SpecialNameAttribute:
                    _attribs |= TypeAttributes.SpecialName;
                    break;
                case KnownCA.SuppressUnmanagedCodeSecurityAttribute:
                    _attribs |= TypeAttributes.HasSecurity;
                    goto default;
                default:
                    ModuleBuilder.SetCustomAttribute(_token, customBuilder);
                    break;
            }
        }

        /// <summary>
        /// Defines the generic type parameters for the current type, specifying their number and their names, and returns an
        /// array of <see cref="GenericTypeParameterBuilder"/> objects that can be used to set their constraints.
        /// </summary>
        /// <param name="names">An array of names for the generic type parameters.</param>
        /// <returns>An array of <see cref="GenericTypeParameterBuilder"/> objects that can be used to define the constraints of the generic type parameters for the current type.</returns>
        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
        {
            typeFlags |= TypeFlags.IsGenericTypeDefinition;
            _gtpb = new GenericTypeParameterBuilder[names.Length];
            for (var i = 0; i < names.Length; i++)
                _gtpb[i] = new GenericTypeParameterBuilder(names[i], this, i);

            return (GenericTypeParameterBuilder[])_gtpb.Clone();
        }

        /// <inheritdoc />
        public override Type[] GetGenericArguments() => Util.Copy(_gtpb);

        /// <inheritdoc />
        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers() =>
            _gtpb == null ? Array.Empty<CustomModifiers>() : new CustomModifiers[_gtpb.Length];

        /// <inheritdoc />
        internal override Type GetGenericTypeArgument(int index) => _gtpb[index];

        /// <inheritdoc />
        public override bool ContainsGenericParameters => _gtpb != null;

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents a generic type definition from which the current type can be obtained.
        /// </summary>
        /// <returns>This instance, without checking whether the type is generic.</returns>
        public override Type GetGenericTypeDefinition() => this;

        /// <summary>
        /// Gets a <see cref="TypeInfo"/> object that represents this type. Defines a default constructor where one is required,
        /// bakes the methods and resolves the tokens of the base type and interfaces.
        /// </summary>
        /// <returns>An object that represents this type.</returns>
        /// <exception cref="NotImplementedException">The type has already been created.</exception>
        public TypeInfo CreateTypeInfo()
        {
            // .NET allows multiple invocations (subsequent invocations return the same baked type), but that is not implemented here
            if ((typeFlags & TypeFlags.Baked) != 0)
                throw new NotImplementedException();

            typeFlags |= TypeFlags.Baked;

            var hasConstructor = false;
            foreach (var mb in _methods)
            {
                hasConstructor |= mb.IsSpecialName && mb.Name == ConstructorInfo.ConstructorName;
                mb.Bake();
            }

            if (!hasConstructor && !IsModulePseudoType && !IsInterface && !IsValueType && !(IsAbstract && IsSealed))
                ((MethodBuilder)DefineDefaultConstructor(MethodAttributes.Public).GetMethodInfo()).Bake();

            if (!IsModulePseudoType)
            {
                var baseType = BaseType;
                if (baseType != null)
                    _extends = ModuleBuilder.GetTypeToken(baseType).Token;
            }

            if (_interfaces != null)
            {
                _interfaceTokens = new List<int>(_interfaces.Count);
                foreach (var interfaceType in _interfaces)
                    _interfaceTokens.Add(ModuleBuilder.GetTypeToken(interfaceType).Token);
            }

            return new BakedType(this);
        }

        /// <summary>
        /// Creates a <see cref="Type"/> object for the class. After defining fields and methods on the class, <see cref="CreateType"/> is called in order to load its <see cref="Type"/> object.
        /// </summary>
        /// <returns>The new <see cref="Type"/> object for this class.</returns>
        /// <exception cref="NotImplementedException">The type has already been created.</exception>
        public Type CreateType() => CreateTypeInfo();

        /// <summary>
        /// Writes the PropertyMap row of this type and the Property rows of its properties.
        /// </summary>
        internal void PopulatePropertyTable()
        {
            if (_properties != null)
            {
                ModuleBuilder.Metadata.AddPropertyMap(MetadataTokens.TypeDefinitionHandle(_token), MetadataTokens.PropertyDefinitionHandle(ModuleBuilder.PropertyCount + 1));
                foreach (var pb in _properties)
                    pb.Bake();
            }
        }

        /// <summary>
        /// Retrieves the base type of this type: the type set by <see cref="SetParent"/>, or <see cref="object"/> if none was set
        /// and this type is not an interface.
        /// </summary>
        public override Type BaseType
        {
            get
            {
                if (_lazyBaseType == null && !IsInterface)
                {
                    var obj = Module.Universe.System_Object;
                    if (this != obj)
                        _lazyBaseType = obj;
                }

                return _lazyBaseType;
            }
        }

        /// <inheritdoc />
        public override string FullName
        {
            get
            {
                if (IsNested)
                    return DeclaringType.FullName + "+" + TypeNameParser.Escape(_name);

                if (_ns == null)
                    return TypeNameParser.Escape(_name);
                else
                    return TypeNameParser.Escape(_ns) + "." + TypeNameParser.Escape(_name);
            }
        }

        /// <inheritdoc />
        internal override TypeName TypeName => new TypeName(_ns, _name);

        /// <summary>
        /// Retrieves the name of this type. Unlike <see cref="FullName"/>, the name is not escaped.
        /// </summary>
        public override string Name
            // FXBUG for a TypeBuilder the name is not escaped
            => _name;

        /// <summary>
        /// Retrieves the namespace where this type is defined, or an empty string if it has none.
        /// </summary>
        public override string Namespace =>
            // for some reason, TypeBuilder doesn't return null (and mcs depends on this)
            // note also that we don't return the declaring type namespace for nested types
            _ns ?? "";

        /// <inheritdoc />
        public override TypeAttributes Attributes => _attribs;

        /// <summary>
        /// Replaces the attributes of this type.
        /// </summary>
        /// <param name="attributes">The new attributes.</param>
        public void __SetAttributes(TypeAttributes attributes) => _attribs = attributes;

        /// <inheritdoc />
        public override Type[] __GetDeclaredInterfaces() => Util.ToArray(_interfaces, Type.EmptyTypes);

        /// <inheritdoc />
        public override MethodBase[] __GetDeclaredMethods()
        {
            var methods = new MethodBase[_methods.Count];
            for (var i = 0; i < methods.Length; i++)
            {
                var mb = _methods[i];
                methods[i] = mb.IsConstructor ? new ConstructorInfoImpl(mb) : mb;
            }

            return methods;
        }

        /// <inheritdoc />
        public override Type DeclaringType => _owner as TypeBuilder;

        /// <inheritdoc />
        public override bool IsGenericType => IsGenericTypeDefinition;

        /// <inheritdoc />
        public override bool IsGenericTypeDefinition => (typeFlags & TypeFlags.IsGenericTypeDefinition) != 0;

        /// <inheritdoc />
        public override int MetadataToken => _token;

        /// <summary>
        /// Returns the method of the specified constructed generic type that corresponds to the specified method of the generic type definition.
        /// </summary>
        /// <param name="type">The constructed generic type whose method is returned.</param>
        /// <param name="method">A method on the generic type definition of <paramref name="type"/>, which specifies which method of <paramref name="type"/> to return.</param>
        /// <returns>A <see cref="MethodInfo"/> object that represents the method of <paramref name="type"/> corresponding to <paramref name="method"/>.</returns>
        public static MethodInfo GetMethod(Type type, MethodInfo method) => new GenericMethodInstance(type, method, null);

        /// <summary>
        /// Returns the constructor of the specified constructed generic type that corresponds to the specified constructor of the generic type definition.
        /// </summary>
        /// <param name="type">The constructed generic type whose constructor is returned.</param>
        /// <param name="constructor">A constructor on the generic type definition of <paramref name="type"/>, which specifies which constructor of <paramref name="type"/> to return.</param>
        /// <returns>A <see cref="ConstructorInfo"/> object that represents the constructor of <paramref name="type"/> corresponding to <paramref name="constructor"/>.</returns>
        public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor) =>
            new ConstructorInfoImpl(GetMethod(type, constructor.GetMethodInfo()));

        /// <inheritdoc />
        public override Module Module => _owner.ModuleBuilder;

        /// <summary>
        /// Writes the TypeDef row of this type and advances the field and method list positions past its fields and methods.
        /// </summary>
        /// <param name="fieldList">The row number of the first field of this type; advanced by the number of fields.</param>
        /// <param name="methodList">The row number of the first method of this type; advanced by the number of methods.</param>
        internal void WriteTypeDefRecord(ref int fieldList, ref int methodList)
        {
            var h = ModuleBuilder.Metadata.AddTypeDefinition(
                (System.Reflection.TypeAttributes)_attribs,
                _typeNameSpace,
                _typeName,
                MetadataTokens.EntityHandle(_extends),
                MetadataTokens.FieldDefinitionHandle(fieldList),
                MetadataTokens.MethodDefinitionHandle(methodList));

            Debug.Assert(h == MetadataTokens.TypeDefinitionHandle(_token));

            // increment next expected method and field row numbers
            fieldList += _fields.Count;
            methodList += _methods.Count;
        }

        /// <summary>
        /// Writes the layout, interface implementations, method overrides and enclosing type of this type. Called for
        /// each type in TypeDef order, which keeps these tables sorted as the metadata requires.
        /// </summary>
        internal void WriteStructure()
        {
            var metadata = ModuleBuilder.Metadata;
            var handle = MetadataTokens.TypeDefinitionHandle(_token);

            if (_hasLayout)
                metadata.AddTypeLayout(handle, (ushort)_packingSize, (uint)_size);

            if (_interfaceTokens != null)
                foreach (var interfaceToken in _interfaceTokens)
                    metadata.AddInterfaceImplementation(handle, MetadataTokens.EntityHandle(interfaceToken));

            if (_methodImpls != null)
                foreach (var (body, declaration) in _methodImpls)
                    metadata.AddMethodImplementation(handle, MetadataTokens.EntityHandle(ModuleBuilder.ResolvePseudoToken(body)), MetadataTokens.EntityHandle(ModuleBuilder.ResolvePseudoToken(declaration)));

            if (_owner is TypeBuilder enclosing)
                metadata.AddNestedType(handle, MetadataTokens.TypeDefinitionHandle(enclosing._token));

            foreach (var field in _fields)
                if (field.__TryGetFieldOffset(out var offset))
                    metadata.AddFieldLayout((FieldDefinitionHandle)MetadataTokens.EntityHandle(ModuleBuilder.ResolvePseudoToken(field.GetCurrentToken())), offset);
        }

        /// <summary>
        /// Adds the generic parameters of this type and its methods.
        /// </summary>
        /// <param name="list">The list to add the generic parameters to.</param>
        internal void CollectGenericParameters(List<GenericTypeParameterBuilder> list)
        {
            if (_gtpb != null)
                list.AddRange(_gtpb);

            foreach (var method in _methods)
                if (method.GenericParameters != null)
                    list.AddRange(method.GenericParameters);
        }

        /// <summary>
        /// Writes the MethodDef rows of the methods of this type.
        /// </summary>
        /// <param name="paramList">The row number of the first parameter of the next method; advanced past the parameters written.</param>
        internal void WriteMethodDefRecords(ref int paramList)
        {
            foreach (var mb in _methods)
                mb.WriteMetadata(ref paramList);
        }

        /// <summary>
        /// Assigns the final MethodDef, Field and Param tokens to the methods, fields and parameters of this type.
        /// </summary>
        /// <param name="methodToken">The next MethodDef token; advanced for each method.</param>
        /// <param name="fieldToken">The next Field token; advanced for each field.</param>
        /// <param name="parameterToken">The next Param token; advanced for each parameter.</param>
        internal void ResolveMethodAndFieldTokens(ref int methodToken, ref int fieldToken, ref int parameterToken)
        {
            foreach (var method in _methods)
                method.FixupToken(methodToken++, ref parameterToken);
            foreach (var field in _fields)
                field.FixupToken(fieldToken++);
        }

        /// <summary>
        /// Writes the Param rows of the methods of this type.
        /// </summary>
        internal void WriteParamRecords()
        {
            foreach (var mb in _methods)
                mb.WriteParamRecords();
        }

        /// <summary>
        /// Writes the Field rows of the fields of this type.
        /// </summary>
        internal void WriteFieldRecords()
        {
            foreach (var fb in _fields)
                fb.WriteFieldRecords();
        }

        /// <summary>
        /// Gets the module builder in which this type is defined.
        /// </summary>
        internal ModuleBuilder ModuleBuilder => _owner.ModuleBuilder;

        /// <inheritdoc />
        ModuleBuilder ITypeOwner.ModuleBuilder => _owner.ModuleBuilder;

        /// <inheritdoc />
        internal override int GetModuleBuilderToken() => _token;

        /// <summary>
        /// Finds the method of this type with the specified metadata token. Helper for <see cref="ModuleBuilder.ResolveMethod(int, Type[], Type[])"/>.
        /// </summary>
        /// <param name="token">The metadata token of the method.</param>
        /// <returns>The method, or <see langword="null"/> if this type has no method with that token.</returns>
        internal MethodBase LookupMethod(int token)
        {
            foreach (var method in _methods)
                if (method.MetadataToken == token)
                    return method;

            return null;
        }

        /// <summary>
        /// Returns a value that indicates whether the current dynamic type has been created.
        /// </summary>
        /// <returns><see langword="true"/> if <see cref="CreateTypeInfo"/> or <see cref="CreateType"/> has been called; otherwise, <see langword="false"/>.</returns>
        public bool IsCreated() => (typeFlags & TypeFlags.Baked) != 0;

        /// <summary>
        /// Throws if this type has not been created yet.
        /// </summary>
        /// <exception cref="NotSupportedException">The type has not been created.</exception>
        internal override void CheckBaked()
        {
            if ((typeFlags & TypeFlags.Baked) == 0)
                throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override Type[] __GetDeclaredTypes() => _nestedTypes != null ? _nestedTypes.ToArray() : Type.EmptyTypes;

        /// <inheritdoc />
        public override FieldInfo[] __GetDeclaredFields() => Util.ToArray(_fields, Array.Empty<FieldInfo>());

        /// <summary>
        /// Returns the events declared by this type, which is always an empty array because <see cref="TypeBuilder"/> does not support defining events.
        /// </summary>
        /// <returns>An empty array.</returns>
        public override EventInfo[] __GetDeclaredEvents() => [];

        /// <inheritdoc />
        public override PropertyInfo[] __GetDeclaredProperties() => Util.ToArray(_properties, Array.Empty<PropertyInfo>());

        /// <inheritdoc />
        internal override bool IsModulePseudoType => _token == 0x02000001;

        /// <inheritdoc />
        internal override bool IsBaked => IsCreated();

        /// <inheritdoc />
        protected override bool IsValueTypeImpl
        {
            get
            {
                var baseType = BaseType;
                if (baseType != null && baseType.IsEnumOrValueType && !IsEnumOrValueType)
                {
                    if (IsCreated())
                        typeFlags |= TypeFlags.ValueType;

                    return true;
                }
                else
                {
                    if (IsCreated())
                        typeFlags |= TypeFlags.NotValueType;

                    return false;
                }
            }
        }

    }

}
