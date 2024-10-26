using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    abstract class TypeSymbolBuilderBase : ITypeSymbolBuilder
    {

        const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        readonly ISymbolContext _context;
        readonly IModuleSymbolBuilder _module;

        string _name;
        TypeAttributes _attributes;
        ITypeSymbol? _parentType;
        ImmutableList<IGenericTypeParameterSymbolBuilder> _genericTypeParameters = ImmutableList<IGenericTypeParameterSymbolBuilder>.Empty;
        ImmutableList<ITypeSymbol> _interfaces = ImmutableList<ITypeSymbol>.Empty;
        ImmutableList<IFieldSymbolBuilder> _fields = ImmutableList<IFieldSymbolBuilder>.Empty;
        ImmutableList<IConstructorSymbolBuilder> _constructors = ImmutableList<IConstructorSymbolBuilder>.Empty;
        ImmutableList<IMethodSymbolBuilder> _methods = ImmutableList<IMethodSymbolBuilder>.Empty;
        ImmutableList<(IMethodSymbol, IMethodSymbol)> _methodOverrides = ImmutableList<(IMethodSymbol, IMethodSymbol)>.Empty;
        ImmutableList<IPropertySymbolBuilder> _properties = ImmutableList<IPropertySymbolBuilder>.Empty;
        ImmutableList<IEventSymbolBuilder> _events = ImmutableList<IEventSymbolBuilder>.Empty;
        ImmutableList<CustomAttribute> _customAttributes = ImmutableList<CustomAttribute>.Empty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        public TypeSymbolBuilderBase(ISymbolContext context, IModuleSymbolBuilder module, TypeAttributes attributes)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _attributes = attributes;
        }

        /// <inheritdoc />
        public ISymbolContext Context => _context;

        /// <inheritdoc />
        public IModuleSymbolBuilder ResolvingModuleBuilder => _module;

        #region ITypeSymbolBuilder

        /// <inheritdoc />
        public void SetParent(ITypeSymbol? parent)
        {
            _parentType = parent;
        }

        /// <inheritdoc />
        public IImmutableList<IGenericTypeParameterSymbolBuilder> DefineGenericParameters(IImmutableList<string> names)
        {
            if (names.Count >= 1 && _genericTypeParameters.IsEmpty == false)
                throw new InvalidOperationException("Generic parameters already defined.");

            var a = ImmutableList.Create<IGenericTypeParameterSymbolBuilder>();
            for (int i = 0; i < names.Count; i++)
                a = a.Add(CreateGenericParameter(names[i]));

            return _genericTypeParameters = a;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IGenericTypeParameterSymbolBuilder"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected abstract IGenericTypeParameterSymbolBuilder CreateGenericParameter(string name);

        /// <inheritdoc />
        public void AddInterfaceImplementation(ITypeSymbol interfaceType)
        {
            _interfaces = _interfaces.Add(interfaceType);
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IConstructorSymbolBuilder"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> requiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> optionalCustomModifiers)
        {
            var constructor = CreateConstructorBuilder(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
            _constructors = _constructors.Add(constructor);
            return constructor;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IConstructorSymbolBuilder"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        protected abstract IConstructorSymbolBuilder CreateConstructorBuilder(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> requiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> optionalCustomModifiers);

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, IImmutableList<ITypeSymbol> parameterTypes)
        {
            var constructor = CreateConstructorBuilder(attributes, parameterTypes);
            _constructors = _constructors.Add(constructor);
            return constructor;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IConstructorSymbolBuilder"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected abstract IConstructorSymbolBuilder CreateConstructorBuilder(MethodAttributes attributes, IImmutableList<ITypeSymbol> parameterTypes);

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes)
        {
            var constructor = CreateConstructorBuilder(attributes, callingConvention, parameterTypes);
            _constructors = _constructors.Add(constructor);
            return constructor;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IConstructorSymbolBuilder"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected abstract IConstructorSymbolBuilder CreateConstructorBuilder(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes);

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> requiredCustomModifiers, ImmutableArray<IImmutableList<ITypeSymbol>> optionalCustomModifiers)
        {
            var constructor = CreateConstructorBuilder(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
            _constructors = _constructors.Add(constructor);
            return constructor;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IConstructorSymbolBuilder"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        protected abstract IConstructorSymbolBuilder CreateConstructorBuilder(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> requiredCustomModifiers, ImmutableArray<IImmutableList<ITypeSymbol>> optionalCustomModifiers);

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineDefaultConstructor(MethodAttributes attributes)
        {
            var constructor = CreateConstructorBuilder(attributes);
            _constructors = _constructors.Add(constructor);
            return constructor;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IConstructorSymbolBuilder"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        protected abstract IConstructorSymbolBuilder CreateConstructorBuilder(MethodAttributes attributes);

        /// <inheritdoc />
        public IEventSymbolBuilder DefineEvent(string name, EventAttributes attributes, ITypeSymbol eventtype)
        {
            var @event = CreateEventBuilder(name, attributes, eventtype);
            _events = _events.Add(@event);
            return @event;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IEventSymbolBuilder"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="eventtype"></param>
        /// <returns></returns>
        protected abstract IEventSymbolBuilder CreateEventBuilder(string name, EventAttributes attributes, ITypeSymbol eventtype);

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, FieldAttributes attributes)
        {
            var field = CreateFieldBuilder(fieldName, type, attributes);
            _fields = _fields.Add(field);
            return field;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IFieldSymbolBuilder"/>.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        protected abstract IFieldSymbolBuilder CreateFieldBuilder(string fieldName, ITypeSymbol type, FieldAttributes attributes);

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, IImmutableList<ITypeSymbol> requiredCustomModifiers, IImmutableList<ITypeSymbol> optionalCustomModifiers, FieldAttributes attributes)
        {
            var field = CreateFieldBuilder(fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
            _fields = _fields.Add(field);
            return field;
        }

        /// <summary>
        /// 
        /// Override this method to implement the creation of a <see cref="IFieldSymbolBuilder"/>.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        protected abstract IFieldSymbolBuilder CreateFieldBuilder(string fieldName, ITypeSymbol type, IImmutableList<ITypeSymbol> requiredCustomModifiers, IImmutableList<ITypeSymbol> optionalCustomModifiers, FieldAttributes attributes);

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers)
        {
            var method = CreateMethodBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
            _methods = _methods.Add(method);
            return method;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IMethodSymbolBuilder"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnTypeRequiredCustomModifiers"></param>
        /// <param name="returnTypeOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterTypeRequiredCustomModifiers"></param>
        /// <param name="parameterTypeOptionalCustomModifiers"></param>
        /// <returns></returns>
        protected abstract IMethodSymbolBuilder CreateMethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers);

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes)
        {
            var method = CreateMethodBuilder(name, attributes, callingConvention, returnType, parameterTypes);
            _methods = _methods.Add(method);
            return method;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IMethodSymbolBuilder"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected abstract IMethodSymbolBuilder CreateMethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            var method = CreateMethodBuilder(name, attributes, callingConvention);
            _methods = _methods.Add(method);
            return method;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IMethodSymbolBuilder"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <returns></returns>
        protected abstract IMethodSymbolBuilder CreateMethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention);

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes)
        {
            var method = CreateMethodBuilder(name, attributes);
            _methods = _methods.Add(method);
            return method;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IMethodSymbolBuilder"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        protected abstract IMethodSymbolBuilder CreateMethodBuilder(string name, MethodAttributes attributes);

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes)
        {
            var method = CreateMethodBuilder(name, attributes, returnType, parameterTypes);
            _methods = _methods.Add(method);
            return method;
        }

        /// <summary>
        /// Override this method to implement the creation of a <see cref="IMethodSymbolBuilder"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected abstract IMethodSymbolBuilder CreateMethodBuilder(string name, MethodAttributes attributes, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <inheritdoc />
        public void DefineMethodOverride(IMethodSymbol methodInfoBody, IMethodSymbol methodInfoDeclaration)
        {
            _methodOverrides = _methodOverrides.Add((methodInfoBody, methodInfoDeclaration));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name)
        {
            return _module.DefineNestedType(this, name);
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr)
        {
            return _module.DefineNestedType(this, name, attr);
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent)
        {
            return _module.DefineNestedType(this, name, attr, parent);
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, IImmutableList<ITypeSymbol> interfaces)
        {
            return _module.DefineNestedType(this, name, attr, parent, interfaces);
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, int typeSize)
        {
            return _module.DefineNestedType(this, name, attr, parent, typeSize);
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize)
        {
            return _module.DefineNestedType(this, name, attr, parent, packSize);
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize, int typeSize)
        {
            return _module.DefineNestedType(this, name, attr, parent, packSize, typeSize);
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            var method = CreatePInvokeMethodBuilder(name, dllName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
            _methods = _methods.Add(method);
            return method;
        }

        protected abstract IMethodSymbolBuilder CreatePInvokeMethodBuilder(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet);

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            var method = CreatePInvokeMethodBuilder(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
            _methods = _methods.Add(method);
            return method;
        }

        protected abstract IMethodSymbolBuilder CreatePInvokeMethodBuilder(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet);

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            var method = CreatePInvokeMethodBuilder(name, dllName, entryName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
            _methods = _methods.Add(method);
            return method;
        }

        protected abstract IMethodSymbolBuilder CreatePInvokeMethodBuilder(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet);

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, IImmutableList<ITypeSymbol> parameterTypes)
        {
            var property = CreatePropertyBuilder(name, attributes, returnType, parameterTypes);
            _properties = _properties.Add(property);
            return property;
        }

        protected abstract IPropertySymbolBuilder CreatePropertyBuilder(string name, PropertyAttributes attributes, ITypeSymbol returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, IImmutableList<ITypeSymbol> parameterTypes)
        {
            var property = CreatePropertyBuilder(name, attributes, callingConvention, returnType, parameterTypes);
            _properties = _properties.Add(property);
            return property;
        }

        protected abstract IPropertySymbolBuilder CreatePropertyBuilder(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers)
        {
            var property = CreatePropertyBuilder(name, attributes, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
            _properties = _properties.Add(property);
            return property;
        }

        protected abstract IPropertySymbolBuilder CreatePropertyBuilder(string name, PropertyAttributes attributes, ITypeSymbol returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers);

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers)
        {
            var property = CreatePropertyBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
            _properties = _properties.Add(property);
            return property;
        }

        protected abstract IPropertySymbolBuilder CreatePropertyBuilder(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers);

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineTypeInitializer()
        {
            var constructor = CreateTypeInitializerBuilder();
            _constructors = _constructors.Add(constructor);
            return constructor;
        }

        protected abstract IConstructorSymbolBuilder CreateTypeInitializerBuilder();

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes = _customAttributes.Add(attribute);
        }

        #endregion

        #region ITypeSymbol

        public void Foo()
        {
            System.Reflection.Metadata.TypeName t;
        }

        public TypeAttributes Attributes => _attributes;

        public IAssemblySymbol Assembly => _module.Assembly;

        public IMethodBaseSymbol? DeclaringMethod => null;

        public string? AssemblyQualifiedName => System.Reflection.Assembly.CreateQualifiedName(Assembly.FullName, FullName);

        public string? FullName => _name;

        public string? Namespace => throw new NotImplementedException();

        public TypeCode TypeCode => throw new NotImplementedException();

        public ITypeSymbol? BaseType => throw new NotImplementedException();

        public bool ContainsGenericParameters => throw new NotImplementedException();

        public GenericParameterAttributes GenericParameterAttributes => throw new NotImplementedException();

        public int GenericParameterPosition => throw new NotImplementedException();

        public IImmutableList<ITypeSymbol> GenericTypeArguments => throw new NotImplementedException();

        public bool IsConstructedGenericType => throw new NotImplementedException();

        public bool IsGenericType => throw new NotImplementedException();

        public bool IsGenericTypeDefinition => throw new NotImplementedException();

        public bool IsGenericParameter => throw new NotImplementedException();

        public bool IsAutoLayout => throw new NotImplementedException();

        public bool IsExplicitLayout => throw new NotImplementedException();

        public bool IsLayoutSequential => throw new NotImplementedException();

        public bool HasElementType => throw new NotImplementedException();

        public bool IsClass => throw new NotImplementedException();

        public bool IsValueType => throw new NotImplementedException();

        public bool IsInterface => throw new NotImplementedException();

        public bool IsPrimitive => throw new NotImplementedException();

        public bool IsSZArray => throw new NotImplementedException();

        public bool IsArray => throw new NotImplementedException();

        public bool IsEnum => throw new NotImplementedException();

        public bool IsPointer => throw new NotImplementedException();

        public bool IsFunctionPointer => throw new NotImplementedException();

        public bool IsUnmanagedFunctionPointer => throw new NotImplementedException();

        public bool IsByRef => throw new NotImplementedException();

        public bool IsAbstract => throw new NotImplementedException();

        public bool IsSealed => throw new NotImplementedException();

        public bool IsVisible => throw new NotImplementedException();

        public bool IsPublic => throw new NotImplementedException();

        public bool IsNotPublic => throw new NotImplementedException();

        public bool IsNested => throw new NotImplementedException();

        public bool IsNestedAssembly => throw new NotImplementedException();

        public bool IsNestedFamANDAssem => throw new NotImplementedException();

        public bool IsNestedFamily => throw new NotImplementedException();

        public bool IsNestedFamORAssem => throw new NotImplementedException();

        public bool IsNestedPrivate => throw new NotImplementedException();

        public bool IsNestedPublic => throw new NotImplementedException();

        public bool IsSerializable => throw new NotImplementedException();

        public bool IsSignatureType => throw new NotImplementedException();

        public bool IsSpecialName => throw new NotImplementedException();

        public IConstructorSymbol? TypeInitializer => throw new NotImplementedException();

        public IModuleSymbol Module => throw new NotImplementedException();

        public ITypeSymbol? DeclaringType => throw new NotImplementedException();

        public MemberTypes MemberType => throw new NotImplementedException();

        public int MetadataToken => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool IsMissing => throw new NotImplementedException();

        public bool ContainsMissing => throw new NotImplementedException();

        public bool IsComplete => throw new NotImplementedException();

        public int GetArrayRank()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IMemberSymbol> GetDefaultMembers()
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol? GetElementType()
        {
            throw new NotImplementedException();
        }

        public string? GetEnumName(object value)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<string> GetEnumNames()
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol GetEnumUnderlyingType()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<ITypeSymbol> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<ITypeSymbol> GetGenericParameterConstraints()
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol GetGenericTypeDefinition()
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol? GetInterface(string name)
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<ITypeSymbol> GetInterfaces(bool inherit = true)
        {
            throw new NotImplementedException();
        }

        public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IConstructorSymbol> GetDeclaredConstructors()
        {
            throw new NotImplementedException();
        }

        public IConstructorSymbol? GetConstructor(IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        public IConstructorSymbol? GetConstructor(BindingFlags bindingFlags, IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IConstructorSymbol> GetConstructors()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IConstructorSymbol> GetConstructors(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IFieldSymbol? GetField(string name)
        {
            throw new NotImplementedException();
        }

        public IFieldSymbol? GetField(string name, BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IFieldSymbol> GetFields()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IFieldSymbol> GetFields(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IMethodSymbol> GetDeclaredMethods()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return SymbolUtil.SelectMethod(this, DefaultBindingFlags, name, null, null);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags, IImmutableList<ITypeSymbol>? types, IImmutableList<ParameterModifier>? modifiers)
        {
            return SymbolUtil.SelectMethod(this, DefaultBindingFlags, name, types, modifiers);
        }

        public IMethodSymbol? GetMethod(string name, IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags, IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags, CallingConventions callConvention, IImmutableList<ITypeSymbol> types, IImmutableList<ParameterModifier> modifiers)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingFlags, CallingConventions callConvention, IImmutableList<ITypeSymbol> types, IImmutableList<ParameterModifier> modifiers)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingFlags, IImmutableList<ITypeSymbol> types, IImmutableList<ParameterModifier> modifiers)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol? GetMethod(string name, int genericParameterCount, IImmutableList<ITypeSymbol> types, IImmutableList<ParameterModifier> modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<IMethodSymbol> GetMethods()
        {
            return SymbolUtil.SelectMethods(this, DefaultBindingFlags, null, null, null);
        }

        /// <inheritdoc />
        public IImmutableList<IMethodSymbol> GetMethods(BindingFlags bindingFlags)
        {
            return SymbolUtil.SelectMethods(this, bindingFlags, null, null, null);
        }

        public IImmutableList<IPropertySymbol> GetDeclaredProperties()
        {
            throw new NotImplementedException();
        }

        public IPropertySymbol? GetProperty(string name)
        {
            throw new NotImplementedException();
        }

        public IPropertySymbol? GetProperty(string name, BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IPropertySymbol? GetProperty(string name, IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IPropertySymbol> GetProperties()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IPropertySymbol> GetProperties(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IEventSymbol> GetDeclaredEvents()
        {
            throw new NotImplementedException();
        }

        public IEventSymbol? GetEvent(string name)
        {
            throw new NotImplementedException();
        }

        public IEventSymbol? GetEvent(string name, BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IEventSymbol> GetEvents()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<IEventSymbol> GetEvents(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol? GetNestedType(string name)
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol? GetNestedType(string name, BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<ITypeSymbol> GetNestedTypes()
        {
            throw new NotImplementedException();
        }

        public IImmutableList<ITypeSymbol> GetNestedTypes(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public bool IsAssignableFrom(ITypeSymbol? c)
        {
            throw new NotImplementedException();
        }

        public bool IsSubclassOf(ITypeSymbol c)
        {
            throw new NotImplementedException();
        }

        public bool IsEnumDefined(object value)
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol MakeArrayType()
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol MakeArrayType(int rank)
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol MakeGenericType(IImmutableList<ITypeSymbol> typeArguments)
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol MakePointerType()
        {
            throw new NotImplementedException();
        }

        public ITypeSymbol MakeByRefType()
        {
            throw new NotImplementedException();
        }

        CustomAttribute[] ICustomAttributeProvider.GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICustomAttributeProvider

        /// <inheritdoc />
        public override IImmutableList<CustomAttribute> GetCustomAttributes(bool inherit = false)
        {
            if (inherit == false || BaseType == null)
                return _customAttributes?.ToArray() ?? [];
            else
                return Enumerable.Concat(_customAttributes?.ToArray() ?? [], ResolveCustomAttributes(BaseType.Unpack().GetInheritedCustomAttributesData())).ToArray();
        }

        /// <inheritdoc />
        public override CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).FirstOrDefault(i => i.AttributeType == attributeType);
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToArray();
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
                    _type = _builder.CreateType() ?? throw new InvalidOperationException();
                    _methods = null;
                    _customAttributes = null;
                }

                // force module to reresolve
                Context.GetOrCreateModuleSymbol(ResolvingModule.UnderlyingModule);
                OnComplete();
            }
        }

        /// <inheritdoc />
        public void OnComplete()
        {
            const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // apply the runtime generic type parameters and pass them to the symbols for completion
            var srcGenericArgs = UnderlyingRuntimeType.GetGenericArguments() ?? [];
            var dstGenericArgs = GetGenericArguments() ?? [];
            Debug.Assert(srcGenericArgs.Length == dstGenericArgs.Length);
            for (int i = 0; i < srcGenericArgs.Length; i++)
                if (dstGenericArgs[i] is IReflectionGenericTypeParameterSymbolBuilder b)
                    b.OnComplete(srcGenericArgs[i]);

            foreach (var i in GetConstructors(DefaultBindingFlags) ?? [])
                if (i is IReflectionConstructorSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetMethods(DefaultBindingFlags) ?? [])
                if (i is IReflectionMethodSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetFields(DefaultBindingFlags) ?? [])
                if (i is IReflectionFieldSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetProperties(DefaultBindingFlags) ?? [])
                if (i is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();

            foreach (var m in GetEvents(DefaultBindingFlags) ?? [])
                if (m is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();
        }

    }

}
