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

    internal sealed class TypeBuilder : TypeInfo, ITypeOwner
    {

        readonly ITypeOwner _owner;
        readonly int _token;
        int _extends;
        Type _lazyBaseType;      // (lazyBaseType == null && attribs & TypeAttributes.Interface) == 0) => BaseType == System.Object
        readonly StringHandle _typeName;
        readonly StringHandle _typeNameSpace;
        readonly string _ns;
        readonly string _name;
        readonly List<MethodBuilder> _methods = new List<MethodBuilder>();
        readonly List<FieldBuilder> _fields = new List<FieldBuilder>();
        List<PropertyBuilder> _properties;
        TypeAttributes _attribs;
        GenericTypeParameterBuilder[] _gtpb;
        List<Type> _interfaces;
        List<int> _interfaceTokens;
        List<TypeBuilder> _nestedTypes;
        List<(int Body, int Declaration)> _methodImpls;
        int _size;
        short _packingSize;
        bool _hasLayout;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        internal TypeBuilder(ITypeOwner owner, string ns, string name)
        {
            this._owner = owner;
            this._token = ModuleBuilder.AllocTypeToken();
            this._ns = ns;
            this._name = name;
            this._typeNameSpace = ns == null ? default : ModuleBuilder.GetOrAddString(ns);
            this._typeName = ModuleBuilder.GetOrAddString(name);
            MarkKnownType(ns, name);
        }

        public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
        {
            var cb = DefineConstructor(attributes, CallingConventions.Standard, Type.EmptyTypes);
            var ilgen = cb.GetILGenerator();
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, BaseType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
            ilgen.Emit(OpCodes.Ret);
            return cb;
        }

        public ConstructorBuilder DefineConstructor(MethodAttributes attribs, CallingConventions callConv, Type[] parameterTypes)
        {
            return DefineConstructor(attribs, callConv, parameterTypes, null, null);
        }

        public ConstructorBuilder DefineConstructor(MethodAttributes attribs, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
        {
            attribs |= MethodAttributes.RTSpecialName | MethodAttributes.SpecialName;
            var name = (attribs & MethodAttributes.Static) == 0 ? ConstructorInfo.ConstructorName : ConstructorInfo.TypeConstructorName;
            var mb = DefineMethod(name, attribs, callingConvention, null, null, null, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
            return new ConstructorBuilder(mb);
        }

        private MethodBuilder CreateMethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            var mb = new MethodBuilder(this, name, attributes, callingConvention);
            _methods.Add(mb);
            return mb;
        }

        public MethodBuilder DefineMethod(string name, MethodAttributes attribs, Type returnType, Type[] parameterTypes)
        {
            return DefineMethod(name, attribs, CallingConventions.Standard, returnType, null, null, parameterTypes, null, null);
        }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            return DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
        }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            var mb = CreateMethodBuilder(name, attributes, callingConvention);
            mb.SetSignature(returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
            return mb;
        }

        public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
        {
            (_methodImpls ??= new List<(int, int)>()).Add((ModuleBuilder.GetMethodToken(methodInfoBody).Token, ModuleBuilder.GetMethodToken(methodInfoDeclaration).Token));
        }

        public FieldBuilder DefineField(string name, Type fieldType, FieldAttributes attribs)
        {
            return DefineField(name, fieldType, null, null, attribs);
        }

        public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
        {
            return __DefineField(fieldName, type, CustomModifiers.FromReqOpt(requiredCustomModifiers, optionalCustomModifiers), attributes);
        }

        public FieldBuilder __DefineField(string fieldName, Type type, CustomModifiers customModifiers, FieldAttributes attributes)
        {
            var fb = new FieldBuilder(this, fieldName, type, customModifiers, attributes);
            _fields.Add(fb);
            return fb;
        }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            return DefineProperty(name, attributes, returnType, null, null, parameterTypes, null, null);
        }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            return DefinePropertyImpl(name, attributes, CallingConventions.Standard, true, returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, parameterTypeRequiredCustomModifiers, Util.NullSafeLength(parameterTypes)));
        }

        private PropertyBuilder DefinePropertyImpl(string name, PropertyAttributes attributes, CallingConventions callingConvention, bool patchCallingConvention, Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            _properties ??= new List<PropertyBuilder>();
            var sig = PropertySignature.Create(callingConvention, returnType, parameterTypes, customModifiers);
            var pb = new PropertyBuilder(this, name, attributes, sig, patchCallingConvention);
            _properties.Add(pb);
            return pb;
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attribs)
        {
            return DefineNestedType(name, attribs, null);
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            var tb = DefineNestedType(name, attr, parent);
            if (interfaces != null)
                foreach (Type iface in interfaces)
                    tb.AddInterfaceImplementation(iface);

            return tb;
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
        {
            return DefineNestedType(name, attr, parent, 0);
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize)
        {
            return DefineNestedType(name, attr, parent, PackingSize.Unspecified, typeSize);
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize, int typeSize)
        {
            string ns = null;
            var lastdot = name.LastIndexOf('.');
            if (lastdot > 0)
            {
                ns = name.Substring(0, lastdot);
                name = name.Substring(lastdot + 1);
            }

            var typeBuilder = __DefineNestedType(ns, name);
            typeBuilder.__SetAttributes(attr);
            typeBuilder.SetParent(parent);
            if (packSize != PackingSize.Unspecified || typeSize != 0)
                typeBuilder.__SetLayout((int)packSize, typeSize);

            return typeBuilder;
        }

        public TypeBuilder __DefineNestedType(string ns, string name)
        {
            typeFlags |= TypeFlags.HasNestedTypes;
            var typeBuilder = ModuleBuilder.DefineType(this, ns, name);
            (_nestedTypes ??= new List<TypeBuilder>()).Add(typeBuilder);
            return typeBuilder;
        }

        public void SetParent(Type parent)
        {
            _lazyBaseType = parent;
        }

        public void AddInterfaceImplementation(Type interfaceType)
        {
            _interfaces ??= new List<Type>();
            _interfaces.Add(interfaceType);
        }

        public void __SetLayout(int packingSize, int size)
        {
            this._packingSize = (short)packingSize;
            this._size = size;
            _hasLayout = true;
        }

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

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            switch (customBuilder.KnownCA)
            {
                case KnownCA.StructLayoutAttribute:
                    SetStructLayoutPseudoCustomAttribute(customBuilder.DecodeBlob(this.Assembly));
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

        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
        {
            typeFlags |= TypeFlags.IsGenericTypeDefinition;
            _gtpb = new GenericTypeParameterBuilder[names.Length];
            for (int i = 0; i < names.Length; i++)
                _gtpb[i] = new GenericTypeParameterBuilder(names[i], this, i);

            return (GenericTypeParameterBuilder[])_gtpb.Clone();
        }

        public override Type[] GetGenericArguments()
        {
            return Util.Copy(_gtpb);
        }

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
        {
            return _gtpb == null ? Array.Empty<CustomModifiers>() : new CustomModifiers[_gtpb.Length];
        }

        internal override Type GetGenericTypeArgument(int index)
        {
            return _gtpb[index];
        }

        public override bool ContainsGenericParameters
        {
            get { return _gtpb != null; }
        }

        public override Type GetGenericTypeDefinition()
        {
            return this;
        }

        public TypeInfo CreateTypeInfo()
        {
            // .NET allows multiple invocations (subsequent invocations return the same baked type)
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

        public Type CreateType()
        {
            return CreateTypeInfo();
        }

        internal void PopulatePropertyTable()
        {
            if (_properties != null)
            {
                ModuleBuilder.Metadata.AddPropertyMap(MetadataTokens.TypeDefinitionHandle(_token), MetadataTokens.PropertyDefinitionHandle(ModuleBuilder.PropertyCount + 1));
                foreach (var pb in _properties)
                    pb.Bake();
            }
        }

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

        internal override TypeName TypeName
        {
            get { return new TypeName(_ns, _name); }
        }

        public override string Name
        {
            // FXBUG for a TypeBuilder the name is not escaped
            get { return _name; }
        }

        public override string Namespace
        {
            get
            {
                // for some reason, TypeBuilder doesn't return null (and mcs depends on this)
                // note also that we don't return the declaring type namespace for nested types
                return _ns ?? "";
            }
        }

        public override TypeAttributes Attributes
        {
            get { return _attribs; }
        }

        public void __SetAttributes(TypeAttributes attributes)
        {
            _attribs = attributes;
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            return Util.ToArray(_interfaces, Type.EmptyTypes);
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            var methods = new MethodBase[this._methods.Count];
            for (int i = 0; i < methods.Length; i++)
            {
                var mb = this._methods[i];
                methods[i] = mb.IsConstructor ? new ConstructorInfoImpl(mb) : mb;
            }

            return methods;
        }

        public override Type DeclaringType
        {
            get { return _owner as TypeBuilder; }
        }

        public override bool IsGenericType
        {
            get { return IsGenericTypeDefinition; }
        }

        public override bool IsGenericTypeDefinition
        {
            get { return (typeFlags & TypeFlags.IsGenericTypeDefinition) != 0; }
        }

        public override int MetadataToken
        {
            get { return _token; }
        }

        public static MethodInfo GetMethod(Type type, MethodInfo method)
        {
            return new GenericMethodInstance(type, method, null);
        }

        public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
        {
            return new ConstructorInfoImpl(GetMethod(type, constructor.GetMethodInfo()));
        }

        public override Module Module
        {
            get { return _owner.ModuleBuilder; }
        }

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
        /// <param name="list"></param>
        internal void CollectGenericParameters(List<GenericTypeParameterBuilder> list)
        {
            if (_gtpb != null)
                list.AddRange(_gtpb);

            foreach (var method in _methods)
                if (method.GenericParameters != null)
                    list.AddRange(method.GenericParameters);
        }

        internal void WriteMethodDefRecords(ref int paramList)
        {
            foreach (var mb in _methods)
                mb.WriteMetadata(ref paramList);
        }

        internal void ResolveMethodAndFieldTokens(ref int methodToken, ref int fieldToken, ref int parameterToken)
        {
            foreach (var method in _methods)
                method.FixupToken(methodToken++, ref parameterToken);
            foreach (var field in _fields)
                field.FixupToken(fieldToken++);
        }

        internal void WriteParamRecords()
        {
            foreach (var mb in _methods)
                mb.WriteParamRecords();
        }

        internal void WriteFieldRecords()
        {
            foreach (var fb in _fields)
                fb.WriteFieldRecords();
        }

        internal ModuleBuilder ModuleBuilder
        {
            get { return _owner.ModuleBuilder; }
        }

        ModuleBuilder ITypeOwner.ModuleBuilder
        {
            get { return _owner.ModuleBuilder; }
        }

        internal override int GetModuleBuilderToken()
        {
            return _token;
        }

        /// <summary>
        /// Helper for ModuleBuilder.ResolveMethod().
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal MethodBase LookupMethod(int token)
        {
            foreach (var method in _methods)
                if (method.MetadataToken == token)
                    return method;

            return null;
        }

        public bool IsCreated()
        {
            return (typeFlags & TypeFlags.Baked) != 0;
        }

        internal override void CheckBaked()
        {
            if ((typeFlags & TypeFlags.Baked) == 0)
                throw new NotSupportedException();
        }

        public override Type[] __GetDeclaredTypes()
        {
            return _nestedTypes != null ? _nestedTypes.ToArray() : Type.EmptyTypes;
        }

        public override FieldInfo[] __GetDeclaredFields()
        {
            return Util.ToArray(_fields, Array.Empty<FieldInfo>());
        }

        public override EventInfo[] __GetDeclaredEvents()
        {
            return [];
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            return Util.ToArray(_properties, Array.Empty<PropertyInfo>());
        }

        internal override bool IsModulePseudoType
        {
            get { return _token == 0x02000001; }
        }

        internal override bool IsBaked
        {
            get { return IsCreated(); }
        }

        protected override bool IsValueTypeImpl
        {
            get
            {
                var baseType = this.BaseType;
                if (baseType != null && baseType.IsEnumOrValueType && !this.IsEnumOrValueType)
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
