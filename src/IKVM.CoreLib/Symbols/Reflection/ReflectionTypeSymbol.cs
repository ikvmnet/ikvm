using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionTypeSymbol : ITypeSymbol
	{

		/// <summary>
		/// Determines whether this <see cref="System.Type"/> is a type definition. That is, it is not a derived generic type or array or pointer.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		static bool IsTypeDefinitionInternal(Type type)
		{
#if NET
			return type.IsTypeDefinition;
#else
			return type.IsArray == false && type.IsPointer == false;
#endif
		}

		const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

		readonly ReflectionSymbolContext _context;
		readonly ReflectionModuleSymbol _module;
		readonly Type _type;

		MethodBase[]? _methodsSource;
		int _methodsBaseRow;
		ReflectionMethodBaseSymbol?[]? _methods;

		FieldInfo[]? _fieldsSource;
		int _fieldsBaseRow;
		ReflectionFieldSymbol?[]? _fields;

		PropertyInfo[]? _propertiesSource;
		int _propertiesBaseRow;
		ReflectionPropertySymbol?[]? _properties;

		EventInfo[]? _eventsSource;
		int _eventsBaseRow;
		ReflectionEventSymbol?[]? _events;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		/// <param name="type"></param>
		public ReflectionTypeSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, Type type)
		{
			Debug.Assert(module.ReflectionModule == type.Module);
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_module = module ?? throw new ArgumentNullException(nameof(module));
			_type = type ?? throw new ArgumentNullException(nameof(type));
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
		{
			return (ReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionConstructorSymbol"/> cached for the type by ctor.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
		{
			return (ReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
		{
			if (method is null)
				throw new ArgumentNullException(nameof(method));

			Debug.Assert(method.DeclaringType == _type);

			var hnd = MetadataTokens.MethodDefinitionHandle(method.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_methodsSource == null)
			{
				_methodsSource = _type.GetConstructors(DefaultBindingFlags).Cast<MethodBase>().Concat(_type.GetMethods(DefaultBindingFlags)).OrderBy(i => i.MetadataToken).ToArray();
				_methodsBaseRow = _methodsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_methodsSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			_methods ??= new ReflectionMethodSymbol?[_methodsSource.Length];

			// index of current record is specified row - base
			var idx = row - _methodsBaseRow;
			Debug.Assert(idx >= 0);
			Debug.Assert(idx < _methodsSource.Length);

			// check that our list is long enough to contain the entire table
			if (_methods.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _methods[idx];
			if (rec == null)
			{
				switch (method)
				{
					case ConstructorInfo ctor:
						Interlocked.CompareExchange(ref rec, new ReflectionConstructorSymbol(_context, this, ctor), null);
						break;
					case MethodInfo meth:
						Interlocked.CompareExchange(ref rec, new ReflectionMethodSymbol(_context, this, meth), null);
						break;
				}
			}

			// this should never happen
			if (rec is not ReflectionMethodBaseSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionFieldSymbol"/> cached for the type by field.
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
		{
			if (field is null)
				throw new ArgumentNullException(nameof(field));

			Debug.Assert(field.DeclaringType == _type);

			var hnd = MetadataTokens.FieldDefinitionHandle(field.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_fieldsSource == null)
			{
				_fieldsSource = _type.GetFields(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray();
				_fieldsBaseRow = _fieldsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_fieldsSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			_fields ??= new ReflectionFieldSymbol?[_fieldsSource.Length];

			// index of current field is specified row - base
			var idx = row - _fieldsBaseRow;
			Debug.Assert(idx >= 0);
			Debug.Assert(idx < _fieldsSource.Length);

			// check that our list is long enough to contain the entire table
			if (_fields.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _fields[idx];
			if (rec == null)
				Interlocked.CompareExchange(ref rec, new ReflectionFieldSymbol(_context, this, field), null);

			// this should never happen
			if (rec is not ReflectionFieldSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionPropertySymbol"/> cached for the type by property.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
		{
			if (property is null)
				throw new ArgumentNullException(nameof(property));

			Debug.Assert(property.DeclaringType == _type);

			var hnd = MetadataTokens.PropertyDefinitionHandle(property.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_propertiesSource == null)
			{
				_propertiesSource = _type.GetProperties(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray();
				_propertiesBaseRow = _propertiesSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_propertiesSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			_properties ??= new ReflectionPropertySymbol?[_propertiesSource.Length];

			// index of current property is specified row - base
			var idx = row - _propertiesBaseRow;
			Debug.Assert(idx >= 0);
			Debug.Assert(idx < _propertiesSource.Length);

			// check that our list is long enough to contain the entire table
			if (_properties.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _properties[idx];
			if (rec == null)
				Interlocked.CompareExchange(ref rec, new ReflectionPropertySymbol(_context, this, property), null);

			// this should never happen
			if (rec is not ReflectionPropertySymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionEventSymbol"/> cached for the type by event.
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
		{
			if (@event is null)
				throw new ArgumentNullException(nameof(@event));

			Debug.Assert(@event.DeclaringType == _type);

			var hnd = MetadataTokens.PropertyDefinitionHandle(@event.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_eventsSource == null)
			{
				_eventsSource = _type.GetEvents(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray();
				_eventsBaseRow = _eventsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(_eventsSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			_events ??= new ReflectionEventSymbol?[_eventsSource.Length];

			// index of current event is specified row - base
			var idx = row - _eventsBaseRow;
			Debug.Assert(idx >= 0);
			Debug.Assert(idx < _eventsSource.Length);

			// check that our list is long enough to contain the entire table
			if (_events.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _events[idx];
			if (rec == null)
				Interlocked.CompareExchange(ref rec, new ReflectionEventSymbol(_context, this, @event), null);

			// this should never happen
			if (rec is not ReflectionEventSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets the wrapped <see cref="Type"/>.
		/// </summary>
		internal Type ReflectionType => _type;

		public IAssemblySymbol Assembly => _context.GetOrCreateAssemblySymbol(_type.Assembly);

		public string? AssemblyQualifiedName => _type.AssemblyQualifiedName;

		public TypeAttributes Attributes => _type.Attributes;

		public ITypeSymbol? BaseType => _type.BaseType != null ? _context.GetOrCreateTypeSymbol(_type.BaseType) : null;

		public bool ContainsGenericParameters => _type.ContainsGenericParameters;

		public IMethodBaseSymbol? DeclaringMethod => throw new NotImplementedException();

		public string? FullName => throw new NotImplementedException();

		public GenericParameterAttributes GenericParameterAttributes => throw new NotImplementedException();

		public int GenericParameterPosition => throw new NotImplementedException();

		public ImmutableArray<ITypeSymbol> GenericTypeArguments => throw new NotImplementedException();

		public bool HasElementType => throw new NotImplementedException();

		public bool IsAbstract => throw new NotImplementedException();

		public bool IsArray => throw new NotImplementedException();

		public bool IsAutoLayout => throw new NotImplementedException();

		public bool IsByRef => throw new NotImplementedException();

		public bool IsByRefLike => throw new NotImplementedException();

		public bool IsClass => throw new NotImplementedException();

		public bool IsConstructedGenericType => throw new NotImplementedException();

		public bool IsEnum => throw new NotImplementedException();

		public bool IsExplicitLayout => throw new NotImplementedException();

		public bool IsFunctionPointer => throw new NotImplementedException();

		public bool IsGenericMethodParameter => throw new NotImplementedException();

		public bool IsGenericParameter => throw new NotImplementedException();

		public bool IsGenericType => throw new NotImplementedException();

		public bool IsInterface => throw new NotImplementedException();

		public bool IsLayoutSequential => throw new NotImplementedException();

		public bool IsNested => throw new NotImplementedException();

		public bool IsNestedAssembly => throw new NotImplementedException();

		public bool IsNestedFamANDAssem => throw new NotImplementedException();

		public bool IsNestedFamily => throw new NotImplementedException();

		public bool IsNestedPrivate => throw new NotImplementedException();

		public bool IsNestedPublic => throw new NotImplementedException();

		public bool IsNotPublic => throw new NotImplementedException();

		public bool IsPointer => throw new NotImplementedException();

		public bool IsPrimitive => throw new NotImplementedException();

		public bool IsPublic => throw new NotImplementedException();

		public bool IsSealed => throw new NotImplementedException();

		public bool IsSecurityCritical => throw new NotImplementedException();

		public bool IsSecuritySafeCritical => throw new NotImplementedException();

		public bool IsSecurityTransparent => throw new NotImplementedException();

		public bool IsSerializable => throw new NotImplementedException();

		public bool IsSignatureType => throw new NotImplementedException();

		public bool IsSZArray => throw new NotImplementedException();

		public bool IsTypeDefinition => throw new NotImplementedException();

		public bool IsUnmanagedFunctionPointer => throw new NotImplementedException();

		public bool IsValueType => throw new NotImplementedException();

		public bool IsVariableBoundArray => throw new NotImplementedException();

		public bool IsVisible => throw new NotImplementedException();

		public string? Namespace => throw new NotImplementedException();

		public IConstructorSymbol? TypeInitializer => throw new NotImplementedException();

		public ImmutableArray<ICustomAttributeSymbol> CustomAttributes => throw new NotImplementedException();

		public ITypeSymbol? DeclaringType => throw new NotImplementedException();

		public MemberTypes MemberType => throw new NotImplementedException();

		public int MetadataToken => throw new NotImplementedException();

		public IModuleSymbol Module => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();

		public bool IsMissing => throw new NotImplementedException();

		public int GetArrayRank()
		{
			throw new NotImplementedException();
		}

		public TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotImplementedException();
		}

		public IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IConstructorSymbol[] GetConstructors()
		{
			throw new NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol[] GetDefaultMembers()
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

		public string[] GetEnumNames()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol GetEnumUnderlyingType()
		{
			throw new NotImplementedException();
		}

		public Array GetEnumValues()
		{
			throw new NotImplementedException();
		}

		public Array GetEnumValuesAsUnderlyingType()
		{
			throw new NotImplementedException();
		}

		public IEventSymbol? GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IEventSymbol? GetEvent(string name)
		{
			throw new NotImplementedException();
		}

		public IEventSymbol[] GetEvents()
		{
			throw new NotImplementedException();
		}

		public IEventSymbol[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IFieldSymbol? GetField(string name)
		{
			throw new NotImplementedException();
		}

		public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IFieldSymbol[] GetFields()
		{
			throw new NotImplementedException();
		}

		public IFieldSymbol[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetFunctionPointerCallingConventions()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetFunctionPointerParameterTypes()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol GetFunctionPointerReturnType()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetGenericArguments()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetGenericParameterConstraints()
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

		public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetInterfaces()
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol[] GetMember(string name)
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol[] GetMembers()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, ParameterModifier[]? modifiers)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types, ParameterModifier[]? modifiers)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol[] GetMethods()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol? GetNestedType(string name)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetNestedTypes()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetOptionalCustomModifiers()
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol[] GetProperties()
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types, ParameterModifier[]? modifiers)
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol? GetProperty(string name)
		{
			throw new NotImplementedException();
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetRequiredCustomModifiers()
		{
			throw new NotImplementedException();
		}

		public bool IsAssignableFrom(ITypeSymbol? c)
		{
			throw new NotImplementedException();
		}

		public bool IsAssignableTo(ITypeSymbol? targetType)
		{
			throw new NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public bool IsEnumDefined(object value)
		{
			throw new NotImplementedException();
		}

		public bool IsSubclassOf(ITypeSymbol c)
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

		public ITypeSymbol MakeByRefType()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol MakePointerType()
		{
			throw new NotImplementedException();
		}

	}

}
