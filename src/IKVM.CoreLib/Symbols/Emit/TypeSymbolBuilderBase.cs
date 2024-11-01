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

    abstract class TypeSymbolBuilderBase<TContext> : ITypeSymbolBuilder
        where TContext : ISymbolContext
    {

        const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        readonly TContext _context;
        readonly IModuleSymbolBuilder _module;

        readonly TypeAttributes _attributes;
        readonly string _namespace;
        readonly string _name;
        readonly ITypeSymbol? _declaringType;
        readonly PackingSize _packingSize;
        readonly int _typeSize;
        ITypeSymbol? _parentType;
        IConstructorSymbol? _typeInitializer = null;
        ImmutableList<ITypeSymbol> _genericTypeParameters = ImmutableList<ITypeSymbol>.Empty;
        ImmutableList<ITypeSymbol> _interfaces = ImmutableList<ITypeSymbol>.Empty;
        ImmutableList<IFieldSymbol> _fields = ImmutableList<IFieldSymbol>.Empty;
        ImmutableList<IConstructorSymbol> _constructors = ImmutableList<IConstructorSymbol>.Empty;
        ImmutableList<IMethodSymbol> _methods = ImmutableList<IMethodSymbol>.Empty;
        ImmutableList<(IMethodSymbol, IMethodSymbol)> _methodOverrides = ImmutableList<(IMethodSymbol, IMethodSymbol)>.Empty;
        ImmutableList<IPropertySymbol> _properties = ImmutableList<IPropertySymbol>.Empty;
        ImmutableList<IEventSymbol> _events = ImmutableList<IEventSymbol>.Empty;
        ImmutableList<CustomAttribute> _customAttributes = ImmutableList<CustomAttribute>.Empty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="attributes"></param>
        /// <param name="name"></param>
        /// <param name="packingSize"></param>
        /// <param name="typeSize"></param>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TypeSymbolBuilderBase(TContext context, IModuleSymbolBuilder module, TypeAttributes attributes, string name, ITypeSymbol? declaringType, PackingSize packingSize, int typeSize)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _attributes = attributes;
            _declaringType = declaringType;
            _packingSize = packingSize;
            _typeSize = typeSize;

            int iLast = name.LastIndexOf('.');
            if (iLast <= 0)
            {
                // no name space
                _namespace = string.Empty;
                _name = name;
            }
            else
            {
                // split the name space
                _namespace = name.Substring(0, iLast);
                _name = name.Substring(iLast + 1);
            }
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
            return (IConstructorSymbolBuilder)(_typeInitializer = CreateTypeInitializerBuilder());
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

        /// <inheritdoc />
        public TypeAttributes Attributes => _attributes;

        /// <inheritdoc />
        public IAssemblySymbol Assembly => _module.Assembly;

        /// <inheritdoc />
        public IMethodBaseSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public string? AssemblyQualifiedName => System.Reflection.Assembly.CreateQualifiedName(Assembly.FullName, FullName);

        /// <inheritdoc />
        public string? FullName => string.IsNullOrEmpty(_namespace) ? _name : _namespace + "." + _name;

        /// <inheritdoc />
        public string? Namespace => _namespace;

        /// <inheritdoc />
        public TypeCode TypeCode => TypeCode.Object;

        /// <inheritdoc />
        public ITypeSymbol? BaseType => _parentType;

        /// <inheritdoc />
        public bool ContainsGenericParameters => _genericTypeParameters.IsEmpty == false;

        /// <inheritdoc />
        public GenericParameterAttributes GenericParameterAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public int GenericParameterPosition => throw new NotImplementedException();

        /// <inheritdoc />
        public IImmutableList<ITypeSymbol> GenericTypeArguments => _genericTypeParameters;

        /// <inheritdoc />
        public bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public bool IsGenericType => _genericTypeParameters.Count > 0;

        /// <inheritdoc />
        public bool IsGenericTypeDefinition => IsGenericType;

        /// <inheritdoc />
        public bool IsGenericParameter => false;

        /// <inheritdoc />
        public bool IsAutoLayout => (_attributes & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout;

        /// <inheritdoc />
        public bool IsExplicitLayout => (_attributes & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;

        /// <inheritdoc />
        public bool IsLayoutSequential => (_attributes & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;

        /// <inheritdoc />
        public bool HasElementType => false;

        /// <inheritdoc />
        public bool IsClass => (_attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Class && !IsValueType;

        /// <inheritdoc />
        public bool IsValueType => IsSubclassOf(Context.ResolveCoreType("System.ValueType"));

        /// <inheritdoc />
        public bool IsInterface => (_attributes & TypeAttributes.Interface) != 0;

        /// <inheritdoc />
        public bool IsPrimitive => false;

        /// <inheritdoc />
        public bool IsSZArray => false;

        /// <inheritdoc />
        public bool IsArray => false;

        /// <inheritdoc />
        public bool IsEnum => IsSubclassOf(Context.ResolveCoreType("System.Enum"));

        /// <inheritdoc />
        public bool IsPointer => false;

        /// <inheritdoc />
        public bool IsFunctionPointer => false;

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public bool IsByRef => false;

        /// <inheritdoc />
        public bool IsAbstract => (_attributes & TypeAttributes.Abstract) != 0;

        /// <inheritdoc />
        public bool IsSealed => (_attributes & TypeAttributes.Sealed) != 0;

        /// <inheritdoc />
        public bool IsVisible => GetIsVisible();

        bool GetIsVisible()
        {
            if (IsGenericParameter)
                return true;

            if (HasElementType)
                return GetElementType()!.IsVisible;

            var type = (ITypeSymbol)this;
            while (type.IsNested)
            {
                if (!type.IsNestedPublic)
                    return false;

                // this should be null for non-nested types.
                type = type.DeclaringType!;
            }

            // Now "type" should be a top level type
            if (!type.IsPublic)
                return false;

            if (IsGenericType && !IsGenericTypeDefinition)
            {
                foreach (var t in GetGenericArguments())
                {
                    if (!t.IsVisible)
                        return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public bool IsPublic => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public;

        /// <inheritdoc />
        public bool IsNotPublic => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic;

        /// <inheritdoc />
        public bool IsNested => DeclaringType != null;

        /// <inheritdoc />
        public bool IsNestedAssembly => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly;

        /// <inheritdoc />
        public bool IsNestedFamANDAssem => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem;

        /// <inheritdoc />
        public bool IsNestedFamily => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily;

        /// <inheritdoc />
        public bool IsNestedFamORAssem => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamORAssem;

        /// <inheritdoc />
        public bool IsNestedPrivate => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate;

        /// <inheritdoc />
        public bool IsNestedPublic => (_attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic;

        /// <inheritdoc />
        public bool IsSerializable => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsSignatureType => false;

        /// <inheritdoc />
        public bool IsSpecialName => (_attributes & TypeAttributes.SpecialName) != 0;

        /// <inheritdoc />
        public IConstructorSymbol? TypeInitializer => throw new NotImplementedException();

        /// <inheritdoc />
        public IModuleSymbol Module => _module;

        /// <inheritdoc />
        public ITypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public MemberTypes MemberType => throw new NotImplementedException();

        /// <inheritdoc />
        public int MetadataToken => throw new NotImplementedException();

        /// <inheritdoc />
        public string Name => _name;

        /// <inheritdoc />
        public bool IsMissing => false;

        /// <inheritdoc />
        public bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsComplete => throw new NotImplementedException();

        /// <inheritdoc />
        public int GetArrayRank()
        {
            return 0;
        }

        /// <inheritdoc />
        public IImmutableList<IMemberSymbol> GetDefaultMembers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetElementType()
        {
            return null;
        }

        /// <inheritdoc />
        public string? GetEnumName(object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<string> GetEnumNames()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol GetEnumUnderlyingType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<ITypeSymbol> GetGenericArguments()
        {
            return _genericTypeParameters;
        }

        /// <inheritdoc />
        public IImmutableList<ITypeSymbol> GetGenericParameterConstraints()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol GetGenericTypeDefinition()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<ITypeSymbol> GetInterfaces(bool inherit = true)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<IConstructorSymbol> GetDeclaredConstructors()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(BindingFlags bindingFlags, IImmutableList<ITypeSymbol> types)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<IConstructorSymbol> GetConstructors()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<IConstructorSymbol> GetConstructors(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<IFieldSymbol> GetFields()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<IFieldSymbol> GetFields(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IImmutableList<IMethodSymbol> GetDeclaredMethods()
        {
            return _methods;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return SymbolUtil.SelectMethod(this, DefaultBindingFlags, name, null, null);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags, IImmutableList<ITypeSymbol>? types, IImmutableList<ParameterModifier>? modifiers)
        {
            return SymbolUtil.SelectMethod(this, bindingFlags, name, types, modifiers);
        }

        public IMethodSymbol? GetMethod(string name, IImmutableList<ITypeSymbol> types)
        {
            return SymbolUtil.SelectMethod(this, DefaultBindingFlags, null, types, null);
        }

        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags)
        {
            return SymbolUtil.SelectMethod(this, bindingFlags, null, null, null);
        }

        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags, IImmutableList<ITypeSymbol> types)
        {
            return SymbolUtil.SelectMethod(this, bindingFlags, null, types, null);
        }

        public IMethodSymbol? GetMethod(string name, BindingFlags bindingFlags, CallingConventions callConvention, IImmutableList<ITypeSymbol> types, IImmutableList<ParameterModifier> modifiers)
        {
            return SymbolUtil.SelectMethod(this, bindingFlags, null, types, modifiers);
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
