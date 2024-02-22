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
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Emit
{

    public sealed class TypeBuilder : TypeInfo, ITypeOwner
    {

        public const int UnspecifiedTypeSize = 0;

        readonly ITypeOwner owner;
        readonly int token;
        int extends;
        Type lazyBaseType;      // (lazyBaseType == null && attribs & TypeAttributes.Interface) == 0) => BaseType == System.Object
        readonly StringHandle typeName;
        readonly StringHandle typeNameSpace;
        readonly string ns;
        readonly string name;
        readonly List<MethodBuilder> methods = new List<MethodBuilder>();
        readonly List<FieldBuilder> fields = new List<FieldBuilder>();
        List<PropertyBuilder> properties;
        List<EventBuilder> events;
        TypeAttributes attribs;
        GenericTypeParameterBuilder[] gtpb;
        List<CustomAttributeBuilder> declarativeSecurity;
        List<Type> interfaces;
        int size;
        short packingSize;
        bool hasLayout;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        internal TypeBuilder(ITypeOwner owner, string ns, string name)
        {
            this.owner = owner;
            this.token = ModuleBuilder.TypeDefTable.AllocToken();
            this.ns = ns;
            this.name = name;
            this.typeNameSpace = ns == null ? default : ModuleBuilder.GetOrAddString(ns);
            this.typeName = ModuleBuilder.GetOrAddString(name);
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

        public ConstructorBuilder DefineTypeInitializer()
        {
            var mb = DefineMethod(ConstructorInfo.TypeConstructorName, MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName, null, Type.EmptyTypes);
            return new ConstructorBuilder(mb);
        }

        private MethodBuilder CreateMethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            ModuleBuilder.MethodDefTable.AddVirtualRecord();
            var mb = new MethodBuilder(this, name, attributes, callingConvention);
            methods.Add(mb);
            return mb;
        }

        public MethodBuilder DefineMethod(string name, MethodAttributes attribs)
        {
            return DefineMethod(name, attribs, CallingConventions.Standard);
        }

        public MethodBuilder DefineMethod(string name, MethodAttributes attribs, CallingConventions callingConvention)
        {
            return CreateMethodBuilder(name, attribs, callingConvention);
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

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return DefinePInvokeMethod(name, dllName, null, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
        }

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
        }

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            var mb = DefineMethod(name, attributes | MethodAttributes.PinvokeImpl, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
            mb.SetDllImportPseudoCustomAttribute(dllName, entryName, nativeCallConv, nativeCharSet, null, null, null, null, null);
            return mb;
        }

        public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
        {
            var rec = new MethodImplTable.Record();
            rec.Class = token;
            rec.MethodBody = this.ModuleBuilder.GetMethodToken(methodInfoBody).Token;
            rec.MethodDeclaration = this.ModuleBuilder.GetMethodTokenWinRT(methodInfoDeclaration);
            ModuleBuilder.MethodImplTable.AddRecord(rec);
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
            fields.Add(fb);
            return fb;
        }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            return DefineProperty(name, attributes, returnType, null, null, parameterTypes, null, null);
        }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            return DefineProperty(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
        }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            return DefinePropertyImpl(name, attributes, CallingConventions.Standard, true, returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, parameterTypeRequiredCustomModifiers, Util.NullSafeLength(parameterTypes)));
        }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            return DefinePropertyImpl(name, attributes, callingConvention, false, returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, parameterTypeRequiredCustomModifiers, Util.NullSafeLength(parameterTypes)));
        }

        public PropertyBuilder __DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, CustomModifiers returnTypeCustomModifiers, Type[] parameterTypes, CustomModifiers[] parameterTypeCustomModifiers)
        {
            return DefinePropertyImpl(name, attributes, callingConvention, false, returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeCustomModifiers, parameterTypeCustomModifiers, Util.NullSafeLength(parameterTypes)));
        }

        private PropertyBuilder DefinePropertyImpl(string name, PropertyAttributes attributes, CallingConventions callingConvention, bool patchCallingConvention, Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            properties ??= new List<PropertyBuilder>();
            var sig = PropertySignature.Create(callingConvention, returnType, parameterTypes, customModifiers);
            var pb = new PropertyBuilder(this, name, attributes, sig, patchCallingConvention);
            properties.Add(pb);
            return pb;
        }

        public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
        {
            events ??= new List<EventBuilder>();
            var eb = new EventBuilder(this, name, attributes, eventtype);
            events.Add(eb);
            return eb;
        }

        public TypeBuilder DefineNestedType(string name)
        {
            return DefineNestedType(name, TypeAttributes.Class | TypeAttributes.NestedPrivate);
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

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize)
        {
            return DefineNestedType(name, attr, parent, packSize, 0);
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
            var rec = new NestedClassTable.Record();
            rec.NestedClass = typeBuilder.MetadataToken;
            rec.EnclosingClass = MetadataToken;
            ModuleBuilder.NestedClassTable.AddRecord(rec);
            return typeBuilder;
        }

        public void SetParent(Type parent)
        {
            lazyBaseType = parent;
        }

        public void AddInterfaceImplementation(Type interfaceType)
        {
            interfaces ??= new List<Type>();
            interfaces.Add(interfaceType);
        }

        public void __SetInterfaceImplementationCustomAttribute(Type interfaceType, CustomAttributeBuilder cab)
        {
            ModuleBuilder.SetInterfaceImplementationCustomAttribute(this, interfaceType, cab);
        }

        public int Size
        {
            get { return size; }
        }

        public PackingSize PackingSize
        {
            get { return (PackingSize)packingSize; }
        }

        public override bool __GetLayout(out int packingSize, out int size)
        {
            packingSize = this.packingSize;
            size = this.size;
            return hasLayout;
        }

        public void __SetLayout(int packingSize, int size)
        {
            this.packingSize = (short)packingSize;
            this.size = size;
            hasLayout = true;
        }

        void SetStructLayoutPseudoCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            var val = customBuilder.GetConstructorArgument(0);
            var layout = val is short v ? (LayoutKind)v : (LayoutKind)val;
            packingSize = (short)((int?)customBuilder.GetFieldValue("Pack") ?? 0);
            size = (int?)customBuilder.GetFieldValue("Size") ?? 0;

            attribs &= ~TypeAttributes.LayoutMask;
            switch (layout)
            {
                case LayoutKind.Auto:
                    attribs |= TypeAttributes.AutoLayout;
                    break;
                case LayoutKind.Explicit:
                    attribs |= TypeAttributes.ExplicitLayout;
                    break;
                case LayoutKind.Sequential:
                    attribs |= TypeAttributes.SequentialLayout;
                    break;
            }

            attribs &= ~TypeAttributes.StringFormatMask;

            switch (customBuilder.GetFieldValue<CharSet>("CharSet") ?? CharSet.None)
            {
                case CharSet.None:
                case CharSet.Ansi:
                    attribs |= TypeAttributes.AnsiClass;
                    break;
                case CharSet.Auto:
                    attribs |= TypeAttributes.AutoClass;
                    break;
                case CharSet.Unicode:
                    attribs |= TypeAttributes.UnicodeClass;
                    break;
            }

            hasLayout = packingSize != 0 || size != 0;
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            switch (customBuilder.KnownCA)
            {
                case KnownCA.StructLayoutAttribute:
                    SetStructLayoutPseudoCustomAttribute(customBuilder.DecodeBlob(this.Assembly));
                    break;
                case KnownCA.SerializableAttribute:
                    attribs |= TypeAttributes.Serializable;
                    break;
                case KnownCA.ComImportAttribute:
                    attribs |= TypeAttributes.Import;
                    break;
                case KnownCA.SpecialNameAttribute:
                    attribs |= TypeAttributes.SpecialName;
                    break;
                case KnownCA.SuppressUnmanagedCodeSecurityAttribute:
                    attribs |= TypeAttributes.HasSecurity;
                    goto default;
                default:
                    ModuleBuilder.SetCustomAttribute(token, customBuilder);
                    break;
            }
        }

        public void __AddDeclarativeSecurity(CustomAttributeBuilder customBuilder)
        {
            attribs |= TypeAttributes.HasSecurity;
            declarativeSecurity ??= new List<CustomAttributeBuilder>();
            declarativeSecurity.Add(customBuilder);
        }

        public void AddDeclarativeSecurity(System.Security.Permissions.SecurityAction securityAction, System.Security.PermissionSet permissionSet)
        {
            ModuleBuilder.AddDeclarativeSecurity(token, securityAction, permissionSet);
            attribs |= TypeAttributes.HasSecurity;
        }

        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
        {
            typeFlags |= TypeFlags.IsGenericTypeDefinition;
            gtpb = new GenericTypeParameterBuilder[names.Length];
            for (int i = 0; i < names.Length; i++)
                gtpb[i] = new GenericTypeParameterBuilder(names[i], this, i);

            return (GenericTypeParameterBuilder[])gtpb.Clone();
        }

        public override Type[] GetGenericArguments()
        {
            return Util.Copy(gtpb);
        }

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
        {
            return gtpb == null ? Array.Empty<CustomModifiers>() : new CustomModifiers[gtpb.Length];
        }

        internal override Type GetGenericTypeArgument(int index)
        {
            return gtpb[index];
        }

        public override bool ContainsGenericParameters
        {
            get { return gtpb != null; }
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
            if (hasLayout)
            {
                var rec = new ClassLayoutTable.Record();
                rec.PackingSize = packingSize;
                rec.ClassSize = size;
                rec.Parent = token;
                ModuleBuilder.ClassLayoutTable.AddRecord(rec);
            }

            var hasConstructor = false;
            foreach (var mb in methods)
            {
                hasConstructor |= mb.IsSpecialName && mb.Name == ConstructorInfo.ConstructorName;
                mb.Bake();
            }

            if (!hasConstructor && !IsModulePseudoType && !IsInterface && !IsValueType && !(IsAbstract && IsSealed) && Universe.AutomaticallyProvideDefaultConstructor)
                ((MethodBuilder)DefineDefaultConstructor(MethodAttributes.Public).GetMethodInfo()).Bake();

            if (declarativeSecurity != null)
                ModuleBuilder.AddDeclarativeSecurity(token, declarativeSecurity);

            if (!IsModulePseudoType)
            {
                var baseType = BaseType;
                if (baseType != null)
                    extends = ModuleBuilder.GetTypeToken(baseType).Token;
            }

            if (interfaces != null)
            {
                foreach (var interfaceType in interfaces)
                {
                    var rec = new InterfaceImplTable.Record();
                    rec.Class = token;
                    rec.Interface = ModuleBuilder.GetTypeToken(interfaceType).Token;
                    ModuleBuilder.InterfaceImplTable.AddRecord(rec);
                }
            }

            return new BakedType(this);
        }

        public Type CreateType()
        {
            return CreateTypeInfo();
        }

        internal void PopulatePropertyAndEventTables()
        {
            if (properties != null)
            {
                var rec = new PropertyMapTable.Record();
                rec.Parent = token;
                rec.PropertyList = MetadataTokens.GetToken(MetadataTokens.PropertyDefinitionHandle(ModuleBuilder.PropertyTable.RowCount + 1));
                ModuleBuilder.PropertyMapTable.AddRecord(rec);
                foreach (var pb in properties)
                    pb.Bake();
            }

            if (events != null)
            {
                var rec = new EventMapTable.Record();
                rec.Parent = token;
                rec.EventList = MetadataTokens.GetToken(MetadataTokens.EventDefinitionHandle(ModuleBuilder.EventTable.RowCount + 1));
                ModuleBuilder.EventMapTable.AddRecord(rec);
                foreach (var eb in events)
                    eb.Bake();
            }
        }

        public override Type BaseType
        {
            get
            {
                if (lazyBaseType == null && !IsInterface)
                {
                    var obj = Module.Universe.System_Object;
                    if (this != obj)
                        lazyBaseType = obj;
                }

                return lazyBaseType;
            }
        }

        public override string FullName
        {
            get
            {
                if (IsNested)
                    return DeclaringType.FullName + "+" + TypeNameParser.Escape(name);

                if (ns == null)
                    return TypeNameParser.Escape(name);
                else
                    return TypeNameParser.Escape(ns) + "." + TypeNameParser.Escape(name);
            }
        }

        internal override TypeName TypeName
        {
            get { return new TypeName(ns, name); }
        }

        public override string Name
        {
            // FXBUG for a TypeBuilder the name is not escaped
            get { return name; }
        }

        public override string Namespace
        {
            get
            {
                // for some reason, TypeBuilder doesn't return null (and mcs depends on this)
                // note also that we don't return the declaring type namespace for nested types
                return ns ?? "";
            }
        }

        public override TypeAttributes Attributes
        {
            get { return attribs; }
        }

        public void __SetAttributes(TypeAttributes attributes)
        {
            attribs = attributes;
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            return Util.ToArray(interfaces, Type.EmptyTypes);
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            var methods = new MethodBase[this.methods.Count];
            for (int i = 0; i < methods.Length; i++)
            {
                var mb = this.methods[i];
                methods[i] = mb.IsConstructor ? new ConstructorInfoImpl(mb) : mb;
            }

            return methods;
        }

        public override Type DeclaringType
        {
            get { return owner as TypeBuilder; }
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
            get { return token; }
        }

        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
        {
            return DefineInitializedData(name, new byte[size], attributes);
        }

        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
        {
            throw new NotImplementedException();

            //var fieldType = ModuleBuilder.GetType("$ArrayType$" + data.Length);
            //if (fieldType == null)
            //{
            //    var typeBuilder = ModuleBuilder.DefineType("$ArrayType$" + data.Length, TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.ExplicitLayout, Module.Universe.System_ValueType, PackingSize.Size1, data.Length);
            //    typeBuilder.CreateType();
            //    fieldType = typeBuilder;
            //}

            //var fieldBuilder = DefineField(name, fieldType, attributes | FieldAttributes.Static);
            //fieldBuilder.__SetDataAndRVA(data);
            //return fieldBuilder;
        }

        public static MethodInfo GetMethod(Type type, MethodInfo method)
        {
            return new GenericMethodInstance(type, method, null);
        }

        public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
        {
            return new ConstructorInfoImpl(GetMethod(type, constructor.GetMethodInfo()));
        }

        public static FieldInfo GetField(Type type, FieldInfo field)
        {
            return new GenericFieldInstance(type, field);
        }

        public override Module Module
        {
            get { return owner.ModuleBuilder; }
        }

        public TypeToken TypeToken
        {
            get { return new TypeToken(token); }
        }

        internal void WriteTypeDefRecord(ref int fieldList, ref int methodList)
        {
            var h = ModuleBuilder.Metadata.AddTypeDefinition(
                (System.Reflection.TypeAttributes)attribs,
                typeNameSpace,
                typeName,
                MetadataTokens.EntityHandle(extends),
                MetadataTokens.FieldDefinitionHandle(fieldList),
                MetadataTokens.MethodDefinitionHandle(methodList));

            Debug.Assert(h == MetadataTokens.TypeDefinitionHandle(token));

            // increment next expected method and field row numbers
            fieldList += fields.Count;
            methodList += methods.Count;
        }

        internal void WriteMethodDefRecords(ref int paramList)
        {
            foreach (var mb in methods)
                mb.WriteMetadata(ref paramList);
        }

        internal void ResolveMethodAndFieldTokens(ref int methodToken, ref int fieldToken, ref int parameterToken)
        {
            foreach (var method in methods)
                method.FixupToken(methodToken++, ref parameterToken);
            foreach (var field in fields)
                field.FixupToken(fieldToken++);
        }

        internal void WriteParamRecords()
        {
            foreach (var mb in methods)
                mb.WriteParamRecords();
        }

        internal void WriteFieldRecords()
        {
            foreach (var fb in fields)
                fb.WriteFieldRecords();
        }

        internal ModuleBuilder ModuleBuilder
        {
            get { return owner.ModuleBuilder; }
        }

        ModuleBuilder ITypeOwner.ModuleBuilder
        {
            get { return owner.ModuleBuilder; }
        }

        internal override int GetModuleBuilderToken()
        {
            return token;
        }

        internal bool HasNestedTypes
        {
            get { return (typeFlags & TypeFlags.HasNestedTypes) != 0; }
        }

        /// <summary>
        /// Helper for ModuleBuilder.ResolveMethod().
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal MethodBase LookupMethod(int token)
        {
            foreach (var method in methods)
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
            if (HasNestedTypes)
            {
                var types = new List<Type>();
                var classes = ModuleBuilder.NestedClassTable.GetNestedClasses(token);
                foreach (var nestedClass in classes)
                    types.Add(ModuleBuilder.ResolveType(nestedClass));

                return types.ToArray();
            }
            else
            {
                return Type.EmptyTypes;
            }
        }

        public override FieldInfo[] __GetDeclaredFields()
        {
            return Util.ToArray(fields, Array.Empty<FieldInfo>());
        }

        public override EventInfo[] __GetDeclaredEvents()
        {
            return Util.ToArray(events, Array.Empty<EventInfo>());
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            return Util.ToArray(properties, Array.Empty<PropertyInfo>());
        }

        internal override bool IsModulePseudoType
        {
            get { return token == 0x02000001; }
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
