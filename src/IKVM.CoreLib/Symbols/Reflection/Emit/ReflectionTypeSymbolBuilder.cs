using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionTypeSymbolBuilder : ReflectionMemberSymbolBuilder, IReflectionTypeSymbolBuilder
    {

        TypeBuilder? _builder;
        Type _type;

        ReflectionMethodTable _methodTable;
        ReflectionFieldTable _fieldTable;
        ReflectionPropertyTable _propertyTable;
        ReflectionEventTable _eventTable;
        ReflectionGenericTypeParameterTable _genericTypeParameterTable;
        ReflectionTypeSpecTable _specTable;

        List<IReflectionMethodSymbolBuilder>? _incompleteMethods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="builder"></param>
        public ReflectionTypeSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder module, TypeBuilder builder) :
            base(context, module, null)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _type = _builder;
            _methodTable = new ReflectionMethodTable(context, module, this);
            _fieldTable = new ReflectionFieldTable(context, module, this);
            _propertyTable = new ReflectionPropertyTable(context, module, this);
            _eventTable = new ReflectionEventTable(context, module, this);
            _genericTypeParameterTable = new ReflectionGenericTypeParameterTable(context, module, this);
            _specTable = new ReflectionTypeSpecTable(context, module, this);
        }

        /// <inheritdoc />
        public Type UnderlyingType => _type;

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingType;

        /// <inheritdoc />
        public TypeBuilder UnderlyingTypeBuilder => _builder ?? throw new InvalidOperationException();

        #region ReflectionTypeSymbol

        /// <inheritdoc />
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return _methodTable.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            return _methodTable.GetOrCreateMethodBaseSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return _propertyTable.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return _eventTable.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        /// <inheritdoc />
        public IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateSZArrayTypeSymbol()
        {
            return _specTable.GetOrCreateSZArrayTypeSymbol();
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateArrayTypeSymbol(int rank)
        {
            return _specTable.GetOrCreateArrayTypeSymbol(rank);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreatePointerTypeSymbol()
        {
            return _specTable.GetOrCreatePointerTypeSymbol();
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateByRefTypeSymbol()
        {
            return _specTable.GetOrCreateByRefTypeSymbol();
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateGenericTypeSymbol(IReflectionTypeSymbol[] genericTypeDefinition)
        {
            return _specTable.GetOrCreateGenericTypeSymbol(genericTypeDefinition);
        }

        #endregion

        #region IReflectionTypeSymbolBuilder

        /// <inheritdoc />
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return _methodTable.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return _propertyTable.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return _eventTable.GetOrCreateEventSymbol(@event);
        }

        #endregion

        #region ITypeSymbolBuilder

        /// <inheritdoc />
        public void SetParent(ITypeSymbol? parent)
        {
            UnderlyingTypeBuilder.SetParent(parent?.Unpack());
        }

        /// <inheritdoc />
        public IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names)
        {
            var l = UnderlyingTypeBuilder.DefineGenericParameters(names);
            var a = new IGenericTypeParameterSymbolBuilder[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[i] = ResolveGenericTypeParameterSymbol(l[i]);

            return a;
        }

        /// <inheritdoc />
        public void AddInterfaceImplementation(ITypeSymbol interfaceType)
        {
            UnderlyingTypeBuilder.AddInterfaceImplementation(interfaceType.Unpack());
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, ITypeSymbol[]? parameterTypes)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineConstructor((MethodAttributes)attributes, CallingConventions.Standard, parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol[]? parameterTypes)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineConstructor((MethodAttributes)attributes, (CallingConventions)callingConvention, parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? requiredCustomModifiers, ITypeSymbol[][]? optionalCustomModifiers)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineConstructor((MethodAttributes)attributes, (CallingConventions)callingConvention, parameterTypes?.Unpack(), requiredCustomModifiers?.Unpack(), optionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineDefaultConstructor(System.Reflection.MethodAttributes attributes)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineDefaultConstructor((MethodAttributes)attributes));
        }

        /// <inheritdoc />
        public IEventSymbolBuilder DefineEvent(string name, System.Reflection.EventAttributes attributes, ITypeSymbol eventtype)
        {
            return ResolveEventSymbol(UnderlyingTypeBuilder.DefineEvent(name, (EventAttributes)attributes, eventtype.Unpack()));
        }

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, System.Reflection.FieldAttributes attributes)
        {
            return ResolveFieldSymbol(UnderlyingTypeBuilder.DefineField(fieldName, type.Unpack(), (FieldAttributes)attributes));
        }

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers, System.Reflection.FieldAttributes attributes)
        {
            return ResolveFieldSymbol(UnderlyingTypeBuilder.DefineField(fieldName, type.Unpack(), requiredCustomModifiers?.Unpack(), optionalCustomModifiers?.Unpack(), (FieldAttributes)attributes));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack()));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, returnType?.Unpack(), parameterTypes?.Unpack()));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public void DefineMethodOverride(IMethodSymbol methodInfoBody, IMethodSymbol methodInfoDeclaration)
        {
            UnderlyingTypeBuilder.DefineMethodOverride(methodInfoBody.Unpack(), methodInfoDeclaration.Unpack());
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), interfaces?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packSize, int typeSize)
        {
            return ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packSize, typeSize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packSize)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packSize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, int typeSize)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), typeSize));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return (IMethodSymbolBuilder)ResolveMethodSymbol(UnderlyingTypeBuilder.DefinePInvokeMethod(name, dllName, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return (IMethodSymbolBuilder)ResolveMethodSymbol(UnderlyingTypeBuilder.DefinePInvokeMethod(name, dllName, entryName, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return (IMethodSymbolBuilder)ResolveMethodSymbol(UnderlyingTypeBuilder.DefinePInvokeMethod(name, dllName, entryName, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, returnType.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, (CallingConventions)callingConvention, returnType.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, returnType.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, (CallingConventions)callingConvention, returnType.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineTypeInitializer()
        {
            return (IConstructorSymbolBuilder)ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineTypeInitializer());
        }
        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingTypeBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingTypeBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

        #region ITypeSymbol

        /// <inheritdoc />
        public IAssemblySymbol Assembly => ResolveAssemblySymbol(UnderlyingType.Assembly);

        /// <inheritdoc />
        public string? AssemblyQualifiedName => UnderlyingType.AssemblyQualifiedName;

        /// <inheritdoc />
        public System.Reflection.TypeAttributes Attributes => (System.Reflection.TypeAttributes)UnderlyingType.Attributes;

        /// <inheritdoc />
        public ITypeSymbol? BaseType => ResolveTypeSymbol(UnderlyingType.BaseType);

        /// <inheritdoc />
        public bool ContainsGenericParameters => UnderlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public IMethodBaseSymbol? DeclaringMethod => ResolveMethodBaseSymbol(UnderlyingType.DeclaringMethod);

        /// <inheritdoc />
        public string? FullName => UnderlyingType.FullName;

        /// <inheritdoc />
        public string? Namespace => UnderlyingType.Namespace;

        /// <inheritdoc />
        public System.Reflection.GenericParameterAttributes GenericParameterAttributes => (System.Reflection.GenericParameterAttributes)UnderlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public int GenericParameterPosition => UnderlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public ITypeSymbol[] GenericTypeArguments => ResolveTypeSymbols(UnderlyingType.GenericTypeArguments);

        /// <inheritdoc />
        public bool HasElementType => UnderlyingType.HasElementType;

        /// <inheritdoc />
        public TypeCode TypeCode => Type.GetTypeCode(UnderlyingType);

        /// <inheritdoc />
        public bool IsAbstract => UnderlyingType.IsAbstract;

        /// <inheritdoc />
        public bool IsSZArray => UnderlyingType.IsSZArray();

        /// <inheritdoc />
        public bool IsArray => UnderlyingType.IsArray;

        /// <inheritdoc />
        public bool IsAutoLayout => UnderlyingType.IsAutoLayout;

        /// <inheritdoc />
        public bool IsExplicitLayout => UnderlyingType.IsExplicitLayout;

        /// <inheritdoc />
        public bool IsByRef => UnderlyingType.IsByRef;

        /// <inheritdoc />
        public bool IsClass => UnderlyingType.IsClass;

        /// <inheritdoc />
        public bool IsEnum => UnderlyingType.IsEnum;

        /// <inheritdoc />
        public bool IsInterface => UnderlyingType.IsInterface;

        /// <inheritdoc />
        public bool IsConstructedGenericType => UnderlyingType.IsConstructedGenericType;

        /// <inheritdoc />
        public bool IsGenericParameter => UnderlyingType.IsGenericParameter;

        /// <inheritdoc />
        public bool IsGenericType => UnderlyingType.IsGenericType;

        /// <inheritdoc />
        public bool IsGenericTypeDefinition => UnderlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public bool IsLayoutSequential => UnderlyingType.IsLayoutSequential;

        /// <inheritdoc />
        public bool IsNested => UnderlyingType.IsNested;

        /// <inheritdoc />
        public bool IsNestedAssembly => UnderlyingType.IsNestedAssembly;

        /// <inheritdoc />
        public bool IsNestedFamANDAssem => UnderlyingType.IsNestedFamANDAssem;

        /// <inheritdoc />
        public bool IsNestedFamORAssem => UnderlyingType.IsNestedFamORAssem;

        /// <inheritdoc />
        public bool IsNestedFamily => UnderlyingType.IsNestedFamily;

        /// <inheritdoc />
        public bool IsNestedPrivate => UnderlyingType.IsNestedPrivate;

        /// <inheritdoc />
        public bool IsNestedPublic => UnderlyingType.IsNestedPublic;

        /// <inheritdoc />
        public bool IsNotPublic => UnderlyingType.IsNotPublic;

        /// <inheritdoc />
        public bool IsPointer => UnderlyingType.IsPointer;

#if NET8_0_OR_GREATER

        /// <inheritdoc />
        public bool IsFunctionPointer => UnderlyingType.IsFunctionPointer;

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => UnderlyingType.IsUnmanagedFunctionPointer;

#else

        /// <inheritdoc />
        public bool IsFunctionPointer => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => throw new NotImplementedException();

#endif

        /// <inheritdoc />
        public bool IsPrimitive => UnderlyingType.IsPrimitive;

        /// <inheritdoc />
        public bool IsPublic => UnderlyingType.IsPublic;

        /// <inheritdoc />
        public bool IsSealed => UnderlyingType.IsSealed;

        /// <inheritdoc />
        public bool IsSerializable => UnderlyingType.IsSerializable;

        /// <inheritdoc />
        public bool IsValueType => UnderlyingType.IsValueType;

        /// <inheritdoc />
        public bool IsVisible => UnderlyingType.IsVisible;

        /// <inheritdoc />
        public bool IsSignatureType => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsSpecialName => UnderlyingType.IsSpecialName;

        /// <inheritdoc />
        public IConstructorSymbol? TypeInitializer => ResolveConstructorSymbol(UnderlyingType.TypeInitializer);

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

        /// <inheritdoc />
        public int GetArrayRank()
        {
            return UnderlyingType.GetArrayRank();
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return ResolveConstructorSymbol(UnderlyingType.GetConstructor((BindingFlags)bindingAttr, binder: null, types.Unpack(), modifiers: null));
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return ResolveConstructorSymbol(UnderlyingType.GetConstructor(types.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors()
        {
            return ResolveConstructorSymbols(UnderlyingType.GetConstructors());
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveConstructorSymbols(UnderlyingType.GetConstructors((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetDefaultMembers()
        {
            return ResolveMemberSymbols(UnderlyingType.GetDefaultMembers());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetElementType()
        {
            return ResolveTypeSymbol(UnderlyingType.GetElementType());
        }

        /// <inheritdoc />
        public string? GetEnumName(object value)
        {
            return UnderlyingType.GetEnumName(value);
        }

        /// <inheritdoc />
        public string[] GetEnumNames()
        {
            return UnderlyingType.GetEnumNames();
        }

        /// <inheritdoc />
        public ITypeSymbol GetEnumUnderlyingType()
        {
            return ResolveTypeSymbol(UnderlyingType.GetEnumUnderlyingType());
        }

        /// <inheritdoc />
        public Array GetEnumValues()
        {
            return UnderlyingType.GetEnumValues();
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name)
        {
            return ResolveEventSymbol(UnderlyingType.GetEvent(name));
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveEventSymbol(UnderlyingType.GetEvent(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents()
        {
            return ResolveEventSymbols(UnderlyingType.GetEvents());
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveEventSymbols(UnderlyingType.GetEvents((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return ResolveFieldSymbol(UnderlyingType.GetField(name));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveFieldSymbol(UnderlyingType.GetField(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(UnderlyingType.GetFields());
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveFieldSymbols(UnderlyingType.GetFields((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(UnderlyingType.GetGenericArguments());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericParameterConstraints()
        {
            return ResolveTypeSymbols(UnderlyingType.GetGenericParameterConstraints());
        }

        /// <inheritdoc />
        public ITypeSymbol GetGenericTypeDefinition()
        {
            return ResolveTypeSymbol(UnderlyingType.GetGenericTypeDefinition());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name)
        {
            return ResolveTypeSymbol(UnderlyingType.GetInterface(name));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingType.GetInterface(name, ignoreCase));
        }

        /// <inheritdoc />
        public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetInterfaces(bool inherit = true)
        {
            if (inherit)
                return ResolveTypeSymbols(UnderlyingType.GetInterfaces());
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, System.Reflection.MemberTypes type, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name, (MemberTypes)type, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMembers((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers()
        {
            return ResolveMemberSymbols(UnderlyingType.GetMembers());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, types.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr, null, types.Unpack(), null));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            if (IsComplete)
                return ResolveMethodSymbol(UnderlyingType.GetMethod(name));
            else
                return GetIncompleteMethods().FirstOrDefault(i => i.Name == name);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            if (IsComplete)
                return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr, null, (CallingConventions)callConvention, types.Unpack(), modifiers?.Unpack()));
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            if (IsComplete)
                return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr, null, types.Unpack(), modifiers?.Unpack()));
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingAttr)
        {
            if (IsComplete)
                return ResolveMethodSymbols(UnderlyingType.GetMethods((BindingFlags)bindingAttr));
            else
                return GetIncompleteMethods(bindingAttr);
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            if (IsComplete)
                return ResolveMethodSymbols(UnderlyingType.GetMethods());
            else
                return GetIncompleteMethods();
        }

        /// <summary>
        /// Gets the set of incomplete methods.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol[] GetIncompleteMethods(System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance)
        {
            if (_incompleteMethods == null)
                return [];
            else
                return SymbolUtil.FilterMethods(this, _incompleteMethods, bindingAttr).Cast<IMethodSymbol>().ToArray();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name)
        {
            return ResolveTypeSymbol(UnderlyingType.GetNestedType(name));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveTypeSymbol(UnderlyingType.GetNestedType(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes()
        {
            return ResolveTypeSymbols(UnderlyingType.GetNestedTypes());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveTypeSymbols(UnderlyingType.GetNestedTypes((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties()
        {
            return ResolvePropertySymbols(UnderlyingType.GetProperties());
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolvePropertySymbols(UnderlyingType.GetProperties((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, types.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, returnType?.Unpack(), types.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, returnType?.Unpack()));
        }

        /// <inheritdoc />
        public bool IsAssignableFrom(ITypeSymbol? c)
        {
            return UnderlyingType.IsAssignableFrom(c?.Unpack());
        }

        /// <inheritdoc />
        public bool IsEnumDefined(object value)
        {
            return UnderlyingType.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public bool IsSubclassOf(ITypeSymbol c)
        {
            return UnderlyingType.IsSubclassOf(c.Unpack());
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType()
        {
            return ResolveTypeSymbol(_type.MakeArrayType());
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType(int rank)
        {
            return ResolveTypeSymbol(_type.MakeArrayType(rank));
        }

        /// <inheritdoc />
        public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            return ResolveTypeSymbol(_type.MakeGenericType(typeArguments.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbol MakePointerType()
        {
            return ResolveTypeSymbol(_type.MakePointerType());
        }

        /// <inheritdoc />
        public ITypeSymbol MakeByRefType()
        {
            return ResolveTypeSymbol(_type.MakeByRefType());
        }

        #endregion

        /// <inheritdoc />
        public void Complete()
        {
            if (_builder != null)
            {
                // complete type
                if (_builder.IsCreated() == false)
                {
                    _type = _builder.CreateType()!;
                    _builder = null;
                }

                // force module to reresolve
                Context.GetOrCreateModuleSymbol(ResolvingModule.UnderlyingModule);
                OnComplete();
            }
        }

        /// <inheritdoc />
        public override void OnComplete()
        {
            const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            foreach (var i in GetGenericArguments())
                if (i is IReflectionGenericTypeParameterSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetConstructors(DefaultBindingFlags))
                if (i is IReflectionConstructorSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetMethods(DefaultBindingFlags))
                if (i is IReflectionMethodSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetFields(DefaultBindingFlags))
                if (i is IReflectionFieldSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetProperties(DefaultBindingFlags))
                if (i is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();

            foreach (var m in GetEvents(DefaultBindingFlags))
                if (m is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();

            base.OnComplete();
        }

    }

}
