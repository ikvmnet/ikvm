using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class TypeSymbolBuilder : TypeSymbol, ICustomAttributeBuilder
    {

        readonly string _name;
        readonly string _namespace;
        readonly TypeSymbolBuilder? _declaringType;
        readonly TypeAttributes _attributes;
        readonly PackingSize _packingSize;
        readonly int _typeSize;
        TypeSymbol? _parent;

        readonly ImmutableArray<GenericTypeParameterTypeSymbolBuilder>.Builder _typeParameters = ImmutableArray.CreateBuilder<GenericTypeParameterTypeSymbolBuilder>();
        ImmutableArray<TypeSymbol> _typeParametersCache;
        readonly ImmutableArray<TypeSymbol>.Builder _interfaces = ImmutableArray.CreateBuilder<TypeSymbol>();
        ImmutableArray<TypeSymbol> _interfacesCache;
        readonly ImmutableArray<FieldSymbolBuilder>.Builder _fields = ImmutableArray.CreateBuilder<FieldSymbolBuilder>();
        ImmutableArray<FieldSymbol> _fieldsCache;
        readonly ImmutableArray<MethodSymbolBuilder>.Builder _methods = ImmutableArray.CreateBuilder<MethodSymbolBuilder>();
        ImmutableArray<MethodSymbol> _methodsCache;
        readonly ImmutableArray<PropertySymbolBuilder>.Builder _properties = ImmutableArray.CreateBuilder<PropertySymbolBuilder>();
        ImmutableArray<PropertySymbol> _propertiesCache;
        readonly ImmutableArray<EventSymbolBuilder>.Builder _events = ImmutableArray.CreateBuilder<EventSymbolBuilder>();
        ImmutableArray<EventSymbol> _eventsCache;
        readonly ImmutableArray<TypeSymbolBuilder>.Builder _nestedTypes = ImmutableArray.CreateBuilder<TypeSymbolBuilder>();
        ImmutableArray<TypeSymbol> _nestedTypesCache;
        readonly ConcurrentDictionary<MethodSymbol, ImmutableHashSet<MethodSymbol>.Builder> _methodImpl = new ConcurrentDictionary<MethodSymbol, ImmutableHashSet<MethodSymbol>.Builder>();
        MethodImplementationMapping _methodImplCache;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();
        ImmutableArray<CustomAttribute> _customAttributesCache;

        bool _frozen;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringModule"></param>
        /// <param name="fullName"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <param name="packingSize"></param>
        /// <param name="typeSize"></param>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal TypeSymbolBuilder(SymbolContext context, ModuleSymbolBuilder declaringModule, string fullName, TypeAttributes attributes, TypeSymbol? parent, ImmutableArray<TypeSymbol> interfaces, PackingSize packingSize, int typeSize, TypeSymbolBuilder? declaringType) :
            base(context, declaringModule)
        {
            if (fullName == null)
                throw new ArgumentNullException(nameof(fullName));

            int iLast = fullName.LastIndexOf('.');
            if (iLast <= 0)
            {
                // no name space
                _namespace = "";
                _name = fullName;
            }
            else
            {
                // split the name space
                _namespace = fullName[..iLast];
                _name = fullName[(iLast + 1)..];
            }

            _attributes = attributes;
            _parent = parent;
            _interfaces = interfaces.ToBuilder();
            _packingSize = packingSize;
            _typeSize = typeSize;
            _declaringType = declaringType;
        }

        /// <summary>
        /// Gets the module builder of the type.
        /// </summary>
        new ModuleSymbolBuilder Module => (ModuleSymbolBuilder)base.Module;

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override MethodSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override string? Namespace => _namespace;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => TypeCode.Object;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => _parent;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => _typeParameters.Count > 0;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => true;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => ContainsGenericParameters;

        /// <inheritdoc />
        public sealed override TypeSymbol GenericTypeDefinition => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        public sealed override bool HasElementType => false;

        /// <inheritdoc />
        public sealed override bool IsPrimitive => false;

        /// <inheritdoc />
        public sealed override bool IsSZArray => false;

        /// <inheritdoc />
        public override bool IsArray => false;

        /// <inheritdoc />
        public sealed override bool IsEnum => false;

        /// <inheritdoc />
        public sealed override bool IsPointer => false;

        /// <inheritdoc />
        public sealed override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsByRef => false;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => false;

        /// <summary>
        /// Gets the packing size.
        /// </summary>
        public PackingSize PackingSize => _packingSize;

        /// <summary>
        /// Gets the type size.
        /// </summary>
        public int TypeSize => _typeSize;

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override string? GetEnumName(object value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericArguments => ComputeGenericArguments();

        /// <summary>
        /// Computes the value for <see cref="GenericArguments"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> ComputeGenericArguments()
        {
            if (_typeParametersCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _typeParametersCache, _typeParameters.ToImmutable().CastArray<TypeSymbol>());

            return _typeParametersCache;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericParameterConstraints => throw new InvalidOperationException();

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfacesCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfacesCache, _interfaces.ToImmutable());

            return _interfacesCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_eventsCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _eventsCache, _events.ToImmutable().CastArray<EventSymbol>());

            return _eventsCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fieldsCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _fieldsCache, _fields.ToImmutable().CastArray<FieldSymbol>());

            return _fieldsCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methodsCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _methodsCache, _methods.ToImmutable().CastArray<MethodSymbol>());

            return _methodsCache;
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            if (_methodImplCache.Type == null)
            {
                var k = _methodImpl.Keys;
                var v = _methodImpl.Values;

                var impl = ImmutableArray.CreateBuilder<MethodSymbol>(k.Count);
                foreach (var i in k)
                    impl.Add(i);

                var decl = ImmutableArray.CreateBuilder<ImmutableArray<MethodSymbol>>(v.Count);
                foreach (var i in v)
                    decl.Add(i.ToImmutableArray());

                lock (this)
                    if (_methodImplCache.Type == null)
                        _methodImplCache = new MethodImplementationMapping(this, impl.DrainToImmutable(), decl.DrainToImmutable());
            }

            return _methodImplCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            if (_nestedTypesCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypesCache, _nestedTypes.ToImmutable().CastArray<TypeSymbol>());

            return _nestedTypesCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_propertiesCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _propertiesCache, _properties.ToImmutable().CastArray<PropertySymbol>());

            return _propertiesCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributesCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributesCache, _customAttributes.ToImmutable());

            return _customAttributesCache;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <summary>
        /// Freezes the type builder.
        /// </summary>
        internal void SetFrozen()
        {
            _frozen = true;
        }

        /// <summary>
        /// Throws an exception if the builder is frozen.
        /// </summary>
        void ThrowIfFrozen()
        {
            if (_frozen)
                throw new InvalidOperationException("TypeSymbolBuilder is frozen.");
        }

        /// <summary>
        /// Sets the base type of the type currently under construction.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(TypeSymbol? parent)
        {
            ThrowIfFrozen();
            _parent = parent;
        }

        /// <summary>
        /// Defines the generic type parameters for the current type, specifying their number and their names, and returns an array of <see cref="GenericTypeParameterTypeSymbolBuilder"/> objects that can be used to set their constraints.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public ImmutableArray<GenericTypeParameterTypeSymbolBuilder> DefineGenericParameters(ImmutableArray<string> names)
        {
            if (_typeParameters.Count > 0)
                throw new InvalidOperationException();

            ThrowIfFrozen();

            for (int i = 0; i < names.Length; i++)
                _typeParameters.Add(new GenericTypeParameterTypeSymbolBuilder(Context, this, names[i], GenericParameterAttributes.None, i));

            return _typeParameters.ToImmutable();
        }

        /// <summary>
        /// Adds an interface that this type implements.
        /// </summary>
        /// <param name="interfaceType"></param>
        public void AddInterfaceImplementation(TypeSymbol interfaceType)
        {
            ThrowIfFrozen();
            _interfaces.Add(interfaceType);
        }

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, and field type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public FieldSymbolBuilder DefineField(string name, TypeSymbol type, FieldAttributes attributes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            return DefineField(name, type, [], [], FieldAttributes.Public);
        }

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, field type, and custom modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public FieldSymbolBuilder DefineField(string name, TypeSymbol type, ImmutableArray<TypeSymbol> requiredCustomModifiers, ImmutableArray<TypeSymbol> optionalCustomModifiers, FieldAttributes attributes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (requiredCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(requiredCustomModifiers));
            if (optionalCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(optionalCustomModifiers));

            ThrowIfFrozen();
            var b = new FieldSymbolBuilder(Context, Module, this, name, attributes, type, requiredCustomModifiers, optionalCustomModifiers);
            _fields.Add(b);
            _fieldsCache = default;
            return b;
        }

        /// <summary>
        /// Defines the initializer for this type.
        /// </summary>
        /// <returns></returns>
        public MethodSymbolBuilder DefineTypeInitializer()
        {
            return DefineMethod(ConstructorInfo.TypeConstructorName, MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, null, default, default, [], [], []);
        }

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes and signature and the standard calling convention.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineConstructor(MethodAttributes attributes, ImmutableArray<TypeSymbol> parameterTypes)
        {
            return DefineConstructor(attributes, CallingConventions.HasThis, parameterTypes);
        }

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes and signature.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, ImmutableArray<TypeSymbol> parameterTypes)
        {
            return DefineConstructor(attributes, CallingConventions.HasThis, parameterTypes, [], []);
        }

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes, signature, and custom modifiers.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers)
        {
            return DefineMethod(ConstructorInfo.ConstructorName, attributes | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, callingConvention, null, [], [], parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
        }

        /// <summary>
        /// Defines the parameterless constructor. The constructor defined here will simply call the parameterless constructor of the parent.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineDefaultConstructor(MethodAttributes attributes)
        {
            return DefineConstructor(attributes, CallingConventions.HasThis, [], [], []);
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, method signature, and custom modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnRequiredCustomModifiers"></param>
        /// <param name="returnOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> returnRequiredCustomModifiers, ImmutableArray<TypeSymbol> returnOptionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (parameterTypes.IsDefault)
                throw new ArgumentNullException(nameof(parameterTypes));
            if (returnRequiredCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(returnRequiredCustomModifiers));
            if (returnOptionalCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(returnOptionalCustomModifiers));
            if (parameterRequiredCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(parameterRequiredCustomModifiers));
            if (parameterOptionalCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(parameterOptionalCustomModifiers));

            ThrowIfFrozen();
            var b = new MethodSymbolBuilder(Context, Module, this, name, attributes, callingConvention, returnType ?? Context.ResolveCoreType("System.Void"), returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
            _methods.Add(b);
            _methodsCache = default;
            return b;
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (parameterTypes.IsDefault)
                throw new ArgumentNullException(nameof(parameterTypes));

            return DefineMethod(name, attributes, callingConvention, returnType, [], [], parameterTypes, [], []);
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and calling convention.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return DefineMethod(name, attributes, callingConvention, null, []);
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name and method attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return DefineMethod(name, attributes, CallingConventions.HasThis);
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (parameterTypes.IsDefault)
                throw new ArgumentNullException(nameof(parameterTypes));

            return DefineMethod(name, attributes, CallingConventions.HasThis, returnType, parameterTypes);
        }

        /// <summary>
        /// Specifies a given method body that implements a given method declaration, potentially with a different name.
        /// </summary>
        /// <param name="methodInfoBody"></param>
        /// <param name="methodInfoDeclaration"></param>
        public void DefineMethodOverride(MethodSymbol methodInfoBody, MethodSymbol methodInfoDeclaration)
        {
            if (methodInfoBody is null)
                throw new ArgumentNullException(nameof(methodInfoBody));
            if (methodInfoDeclaration is null)
                throw new ArgumentNullException(nameof(methodInfoDeclaration));

            ThrowIfFrozen();
            var decl = _methodImpl.GetOrAdd(methodInfoBody, _ => ImmutableHashSet.CreateBuilder<MethodSymbol>());
            decl.Add(methodInfoDeclaration);
            _methodImplCache = default;
        }

        /// <summary>
        /// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dllName"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="nativeCallConv"></param>
        /// <param name="nativeCharSet"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, [], [], parameterTypes, [], [], nativeCallConv, nativeCharSet);
        }

        /// <summary>
        /// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the name of the entry point, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dllName"></param>
        /// <param name="entryName"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="nativeCallConv"></param>
        /// <param name="nativeCharSet"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, [], [], parameterTypes, [], [], nativeCallConv, nativeCharSet);
        }

        /// <summary>
        /// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the name of the entry point, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, the PInvoke flags, and custom modifiers for the parameters and return type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dllName"></param>
        /// <param name="entryName"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnRequiredCustomModifiers"></param>
        /// <param name="returnOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        /// <param name="nativeCallConv"></param>
        /// <param name="nativeCharSet"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> returnRequiredCustomModifiers, ImmutableArray<TypeSymbol> returnOptionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            if ((attributes & MethodAttributes.Abstract) != 0)
                throw new ArgumentException(nameof(attributes));
            if ((_attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Interface)
                throw new ArgumentException(nameof(attributes));
            if (returnRequiredCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(returnRequiredCustomModifiers));
            if (returnOptionalCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(returnOptionalCustomModifiers));
            if (parameterTypes.IsDefault)
                throw new ArgumentNullException(nameof(parameterTypes));
            if (parameterRequiredCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(parameterRequiredCustomModifiers));
            if (parameterOptionalCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(parameterOptionalCustomModifiers));

            ThrowIfFrozen();
            var b = new MethodSymbolBuilder(Context, Module, this, name, attributes | MethodAttributes.PinvokeImpl, callingConvention, returnType ?? Context.ResolveCoreType("System.Void"), returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
            b.SetDllImportData(dllName, entryName, nativeCallConv, nativeCharSet);
            _methods.Add(b);
            _methodsCache = default;
            return b;
        }

        /// <summary>
        /// Adds a new property to the type, with the given name and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, TypeSymbol returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (parameterTypes.IsDefault)
                throw new ArgumentNullException(nameof(parameterTypes));

            return DefineProperty(name, attributes, default, returnType, [], [], parameterTypes, [], []);
        }

        /// <summary>
        /// Adds a new property to the type, with the given name, attributes, calling convention, and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, TypeSymbol returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            return DefineProperty(name, attributes, callingConvention, returnType, [], [], parameterTypes, [], []);
        }

        /// <summary>
        /// Adds a new property to the type, with the given name, property signature, and custom modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="returnRequiredCustomModifiers"></param>
        /// <param name="returnOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        /// <returns></returns>
        public PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, TypeSymbol returnType, ImmutableArray<TypeSymbol> returnRequiredCustomModifiers, ImmutableArray<TypeSymbol> returnOptionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers)
        {
            return DefineProperty(name, attributes, default, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
        }

        /// <summary>
        /// Adds a new property to the type, with the given name, calling convention, property signature, and custom modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnRequiredCustomModifiers"></param>
        /// <param name="returnOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        /// <returns></returns>
        public PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, TypeSymbol returnType, ImmutableArray<TypeSymbol> returnRequiredCustomModifiers, ImmutableArray<TypeSymbol> returnOptionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (returnRequiredCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(returnRequiredCustomModifiers));
            if (returnOptionalCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(returnOptionalCustomModifiers));
            if (parameterTypes.IsDefault)
                throw new ArgumentNullException(nameof(parameterTypes));
            if (parameterRequiredCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(parameterRequiredCustomModifiers));
            if (parameterOptionalCustomModifiers.IsDefault)
                throw new ArgumentNullException(nameof(parameterOptionalCustomModifiers));

            ThrowIfFrozen();
            var b = new PropertySymbolBuilder(Context, this, name, attributes, callingConvention, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
            _properties.Add(b);
            _propertiesCache = default;
            return b;
        }

        /// <summary>
        /// Adds a new event to the type, with the given name, attributes and event type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public EventSymbolBuilder DefineEvent(string name, EventAttributes attributes, TypeSymbol eventType)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            var b = new EventSymbolBuilder(Context, this, name, attributes, eventType);
            _events.Add(b);
            _eventsCache = default;
            return b;
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attributes, TypeSymbol? parent, ImmutableArray<TypeSymbol> interfaces)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (interfaces.IsDefault)
                throw new ArgumentNullException(nameof(interfaces));

            ThrowIfFrozen();
            var b = new TypeSymbolBuilder(Context, Module, name, attributes, parent, interfaces, PackingSize.Unspecified, -1, this);
            _nestedTypes.Add(b);
            _nestedTypesCache = default;
            return b;
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attributes, TypeSymbol? parent, PackingSize packSize, int typeSize)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            var b = new TypeSymbolBuilder(Context, Module, name, attributes, parent, [], packSize, typeSize, this);
            _nestedTypes.Add(b);
            _nestedTypesCache = default;
            return b;
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the packing size.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attributes, TypeSymbol? parent, PackingSize packSize)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            var b = new TypeSymbolBuilder(Context, Module, name, attributes, parent, [], packSize, -1, this);
            _nestedTypes.Add(b);
            _nestedTypesCache = default;
            return b;
        }

        /// <summary>
        /// Defines a nested type, given its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineNestedType(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            var b = new TypeSymbolBuilder(Context, Module, name, TypeAttributes.NestedPrivate, null, [], PackingSize.Unspecified, -1, this);
            _nestedTypes.Add(b);
            _nestedTypesCache = default;
            return b;
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attributes, TypeSymbol? parent)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            var b = new TypeSymbolBuilder(Context, Module, name, attributes, parent, [], PackingSize.Unspecified, -1, this);
            _nestedTypes.Add(b);
            _nestedTypesCache = default;
            return b;
        }

        /// <summary>
        /// Defines a nested type, given its name and attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attributes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            var b = new TypeSymbolBuilder(Context, Module, name, attributes, null, [], PackingSize.Unspecified, -1, this);
            _nestedTypes.Add(b);
            _nestedTypesCache = default;
            return b;
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the total size of the type, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attributes, TypeSymbol? parent, int typeSize)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            ThrowIfFrozen();
            var b = new TypeSymbolBuilder(Context, Module, name, attributes, parent, [], PackingSize.Unspecified, typeSize, this);
            _nestedTypes.Add(b);
            _nestedTypesCache = default;
            return b;
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes.Add(attribute);
        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var args = GenericArguments;
            for (int i = 0; i < args.Length; i++)
                if (args[i].ContainsGenericParameters)
                    args = args.SetItem(i, args[i].Specialize(context));

            return MakeGenericType(args);
        }

    }

}
