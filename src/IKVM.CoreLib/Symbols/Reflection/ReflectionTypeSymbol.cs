using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionTypeSymbol : ReflectionMemberSymbol, ITypeSymbol
	{

		const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

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

		ReflectionTypeSymbol?[]? _asArray;
		ReflectionTypeSymbol? _asSZArray;
		ReflectionTypeSymbol? _asPointer;
		ReflectionTypeSymbol? _asByRef;

		Type[]? _genericParametersSource;
		ReflectionTypeSymbol?[]? _genericParameters;
		List<(Type[] Arguments, ReflectionTypeSymbol Symbol)>? _genericTypes;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		/// <param name="type"></param>
		public ReflectionTypeSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, Type type) :
			base(context, module, null, type)
		{
			Debug.Assert(module.ReflectionModule == type.Module);
			_type = type ?? throw new ArgumentNullException(nameof(type));
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		internal ReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
		{
			return (ReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionConstructorSymbol"/> cached for the type by ctor.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		internal ReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
		{
			return (ReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
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
					case ConstructorInfo c:
						Interlocked.CompareExchange(ref rec, new ReflectionConstructorSymbol(Context, ContainingModule, this, c), null);
						break;
					case MethodInfo m:
						Interlocked.CompareExchange(ref rec, new ReflectionMethodSymbol(Context, ContainingModule, this, m), null);
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
				Interlocked.CompareExchange(ref rec, new ReflectionFieldSymbol(Context, this, field), null);

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
				Interlocked.CompareExchange(ref rec, new ReflectionPropertySymbol(Context, this, property), null);

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
				Interlocked.CompareExchange(ref rec, new ReflectionEventSymbol(Context, this, @event), null);

			// this should never happen
			if (rec is not ReflectionEventSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the generic parameter type.
		/// </summary>
		/// <param name="genericTypeParameterType"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericTypeParameterType)
		{
			if (genericTypeParameterType is null)
				throw new ArgumentNullException(nameof(genericTypeParameterType));

			Debug.Assert(genericTypeParameterType.DeclaringType == _type);

			// initialize tables
			_genericParametersSource ??= _type.GetGenericArguments();
			_genericParameters ??= new ReflectionTypeSymbol?[_genericParametersSource.Length];

			// position of the parameter in the list
			var idx = genericTypeParameterType.GenericParameterPosition;

			// check that our list is long enough to contain the entire table
			if (_genericParameters.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _genericParameters[idx];
			if (rec == null)
				Interlocked.CompareExchange(ref rec, new ReflectionTypeSymbol(Context, ContainingModule, genericTypeParameterType), null);

			// this should never happen
			if (rec is not ReflectionTypeSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the generic parameter type.
		/// </summary>
		/// <param name="genericTypeArguments"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionTypeSymbol GetOrCreateGenericTypeSymbol(Type[] genericTypeArguments)
		{
			if (genericTypeArguments is null)
				throw new ArgumentNullException(nameof(genericTypeArguments));

			if (_type.IsGenericTypeDefinition == false)
				throw new InvalidOperationException();

			// initialize the available parameters
			_genericParametersSource ??= _type.GetGenericArguments();
			if (_genericParametersSource.Length != genericTypeArguments.Length)
				throw new InvalidOperationException();

			// initialize generic type map, and lock on it since we're potentially adding items
			_genericTypes ??= [];
			lock (_genericTypes)
			{
				// find existing entry
				foreach (var i in _genericTypes)
					if (i.Arguments.SequenceEqual(genericTypeArguments))
						return i.Symbol;

				// generate new symbol
				var sym = new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeGenericType(genericTypeArguments));
				_genericTypes.Add((genericTypeArguments, sym));
				return sym;
			}
		}

		/// <summary>
		/// Resolves the symbol for the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected internal override ReflectionTypeSymbol ResolveTypeSymbol(Type type)
		{
			if (type == _type)
				return this;
			else
				return base.ResolveTypeSymbol(type);
		}

		/// <summary>
		/// Resolves the symbol for the specified constructor.
		/// </summary>
		/// <param name="ctor"></param>
		/// <returns></returns>
		protected internal override ReflectionConstructorSymbol ResolveConstructorSymbol(ConstructorInfo ctor)
		{
			if (ctor.DeclaringType == _type)
				return GetOrCreateConstructorSymbol(ctor);
			else
				return base.ResolveConstructorSymbol(ctor);
		}

		/// <summary>
		/// Resolves the symbol for the specified method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		protected internal override ReflectionMethodSymbol ResolveMethodSymbol(MethodInfo method)
		{
			if (method.DeclaringType == _type)
				return GetOrCreateMethodSymbol(method);
			else
				return base.ResolveMethodSymbol(method);
		}

		/// <summary>
		/// Resolves the symbol for the specified field.
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		protected internal override ReflectionFieldSymbol ResolveFieldSymbol(FieldInfo field)
		{
			if (field.DeclaringType == _type)
				return GetOrCreateFieldSymbol(field);
			else
				return base.ResolveFieldSymbol(field);
		}

		/// <summary>
		/// Resolves the symbol for the specified field.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected internal override ReflectionPropertySymbol ResolvePropertySymbol(PropertyInfo property)
		{
			if (property.DeclaringType == _type)
				return GetOrCreatePropertySymbol(property);
			else
				return base.ResolvePropertySymbol(property);
		}

		/// <summary>
		/// Resolves the symbol for the specified event.
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		protected internal override ReflectionEventSymbol ResolveEventSymbol(EventInfo @event)
		{
			if (@event.DeclaringType == _type)
				return GetOrCreateEventSymbol(@event);
			else
				return base.ResolveEventSymbol(@event);
		}

		/// <summary>
		/// Gets the wrapped <see cref="Type"/>.
		/// </summary>
		internal Type ReflectionType => _type;

		public IAssemblySymbol Assembly => Context.GetOrCreateAssemblySymbol(_type.Assembly);

		public string? AssemblyQualifiedName => _type.AssemblyQualifiedName;

		public TypeAttributes Attributes => _type.Attributes;

		public ITypeSymbol? BaseType => _type.BaseType != null ? ResolveTypeSymbol(_type.BaseType) : null;

		public bool ContainsGenericParameters => _type.ContainsGenericParameters;

		public IMethodBaseSymbol? DeclaringMethod => _type.DeclaringMethod is MethodInfo m ? ResolveMethodSymbol(m) : null;

		public string? FullName => _type.FullName;

		public GenericParameterAttributes GenericParameterAttributes => _type.GenericParameterAttributes;

		public int GenericParameterPosition => _type.GenericParameterPosition;

		public ITypeSymbol[] GenericTypeArguments => ResolveTypeSymbols(_type.GenericTypeArguments);

		public bool HasElementType => _type.HasElementType;

		public bool IsAbstract => _type.IsAbstract;

		public bool IsArray => _type.IsArray;

		public bool IsAutoLayout => _type.IsAutoLayout;

		public bool IsByRef => _type.IsByRef;

		public bool IsClass => _type.IsClass;

		public bool IsConstructedGenericType => _type.IsConstructedGenericType;

		public bool IsEnum => _type.IsEnum;

		public bool IsExplicitLayout => _type.IsExplicitLayout;

		public bool IsGenericParameter => _type.IsGenericParameter;

		public bool IsGenericType => _type.IsGenericType;

		public bool IsGenericTypeDefinition => _type.IsGenericTypeDefinition;

		public bool IsInterface => _type.IsInterface;

		public bool IsLayoutSequential => _type.IsLayoutSequential;

		public bool IsNested => _type.IsNested;

		public bool IsNestedAssembly => _type.IsNestedAssembly;

		public bool IsNestedFamANDAssem => _type.IsNestedFamANDAssem;

		public bool IsNestedFamily => _type.IsNestedFamily;

		public bool IsNestedPrivate => _type.IsNestedPrivate;

		public bool IsNestedPublic => _type.IsNestedPublic;

		public bool IsNotPublic => _type.IsNotPublic;

		public bool IsPointer => _type.IsPointer;

		public bool IsPrimitive => _type.IsPrimitive;

		public bool IsPublic => _type.IsPublic;

		public bool IsSealed => _type.IsSealed;

		public bool IsSecurityCritical => _type.IsSecurityCritical;

		public bool IsSecuritySafeCritical => _type.IsSecuritySafeCritical;

		public bool IsSecurityTransparent => _type.IsSecurityTransparent;

#pragma warning disable SYSLIB0050 // Type or member is obsolete
		public bool IsSerializable => _type.IsSerializable;
#pragma warning restore SYSLIB0050 // Type or member is obsolete

		public bool IsValueType => _type.IsValueType;

		public bool IsVisible => _type.IsVisible;

		public string? Namespace => _type.Namespace;

		public IConstructorSymbol? TypeInitializer => _type.TypeInitializer is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;

		public int GetArrayRank()
		{
			return _type.GetArrayRank();
		}

		public IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types)
		{
			return _type.GetConstructor(bindingAttr, binder: null, UnpackTypeSymbols(types), modifiers: null) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
		}

		public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
		{
			return _type.GetConstructor(UnpackTypeSymbols(types)) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
		}

		public IConstructorSymbol[] GetConstructors()
		{
			return ResolveConstructorSymbols(_type.GetConstructors());
		}

		public IMemberSymbol[] GetDefaultMembers()
		{
			return ResolveMemberSymbols(_type.GetDefaultMembers());
		}

		public ITypeSymbol? GetElementType()
		{
			return _type.GetElementType() is Type t ? ResolveTypeSymbol(t) : null;
		}

		public string? GetEnumName(object value)
		{
			return _type.GetEnumName(value);
		}

		public string[] GetEnumNames()
		{
			return _type.GetEnumNames();
		}

		public ITypeSymbol GetEnumUnderlyingType()
		{
			return ResolveTypeSymbol(_type.GetEnumUnderlyingType());
		}

		public Array GetEnumValues()
		{
			return _type.GetEnumValues();
		}

		public IEventSymbol? GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public IEventSymbol? GetEvent(string name)
		{
			return _type.GetEvent(name) is { } f ? ResolveEventSymbol(f) : null;
		}

		public IEventSymbol[] GetEvents()
		{
			return ResolveEventSymbols(_type.GetEvents());
		}

		public IEventSymbol[] GetEvents(BindingFlags bindingAttr)
		{
			return ResolveEventSymbols(_type.GetEvents(bindingAttr));
		}

		public IFieldSymbol? GetField(string name)
		{
			return _type.GetField(name) is FieldInfo f ? ResolveFieldSymbol(f) : null;
		}

		public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
		{
			return _type.GetField(name, bindingAttr) is FieldInfo f ? ResolveFieldSymbol(f) : null;
		}

		public IFieldSymbol[] GetFields()
		{
			return ResolveFieldSymbols(_type.GetFields());
		}

		public IFieldSymbol[] GetFields(BindingFlags bindingAttr)
		{
			return ResolveFieldSymbols(_type.GetFields(bindingAttr));
		}

		public ITypeSymbol[] GetGenericArguments()
		{
			return ResolveTypeSymbols(_type.GetGenericArguments());
		}

		public ITypeSymbol[] GetGenericParameterConstraints()
		{
			return ResolveTypeSymbols(_type.GetGenericParameterConstraints());
		}

		public ITypeSymbol GetGenericTypeDefinition()
		{
			return ResolveTypeSymbol(_type.GetGenericTypeDefinition());
		}

		public ITypeSymbol? GetInterface(string name)
		{
			return _type.GetInterface(name) is { } i ? ResolveTypeSymbol(i) : null;
		}

		public ITypeSymbol? GetInterface(string name, bool ignoreCase)
		{
			return _type.GetInterface(name, ignoreCase) is { } i ? ResolveTypeSymbol(i) : null;
		}

		public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetInterfaces()
		{
			return ResolveTypeSymbols(_type.GetInterfaces());
		}

		public IMemberSymbol[] GetMember(string name)
		{
			return ResolveMemberSymbols(_type.GetMember(name));
		}

		public IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr)
		{
			return ResolveMemberSymbols(_type.GetMember(name, bindingAttr));
		}

		public IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			return ResolveMemberSymbols(_type.GetMember(name, type, bindingAttr));
		}

		public IMemberSymbol[] GetMembers(BindingFlags bindingAttr)
		{
			return ResolveMemberSymbols(_type.GetMembers(bindingAttr));
		}

		public IMemberSymbol[] GetMembers()
		{
			return ResolveMemberSymbols(_type.GetMembers());
		}

		public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr)
		{
			return _type.GetMethod(name, bindingAttr) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
		{
			return _type.GetMethod(name, UnpackTypeSymbols(types)) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetMethod(string name)
		{
			return _type.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types, ParameterModifier[]? modifiers)
		{
			return _type.GetMethod(name, UnpackTypeSymbols(types), modifiers) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol[] GetMethods(BindingFlags bindingAttr)
		{
			return ResolveMethodSymbols(_type.GetMethods(bindingAttr));
		}

		public IMethodSymbol[] GetMethods()
		{
			return ResolveMethodSymbols(_type.GetMethods());
		}

		public ITypeSymbol? GetNestedType(string name)
		{
			return _type.GetNestedType(name) is Type t ? ResolveTypeSymbol(t) : null;
		}

		public ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr)
		{
			return _type.GetNestedType(name, bindingAttr) is Type t ? ResolveTypeSymbol(t) : null;
		}

		public ITypeSymbol[] GetNestedTypes()
		{
			return ResolveTypeSymbols(_type.GetNestedTypes());
		}

		public ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return ResolveTypeSymbols(_type.GetNestedTypes(bindingAttr));
		}

		public IPropertySymbol[] GetProperties()
		{
			return ResolvePropertySymbols(_type.GetProperties());
		}

		public IPropertySymbol[] GetProperties(BindingFlags bindingAttr)
		{
			return ResolvePropertySymbols(_type.GetProperties(bindingAttr));
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types, ParameterModifier[]? modifiers)
		{
			var _returnType = returnType != null ? ((ReflectionTypeSymbol)returnType).ReflectionType : null;

			var _types = new Type[types.Length];
			for (int i = 0; i < types.Length; i++)
				_types[i] = ((ReflectionTypeSymbol)types[i]).ReflectionType;

			return _type.GetProperty(name, _returnType, _types, modifiers) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
		{
			var _types = new Type[types.Length];
			for (int i = 0; i < types.Length; i++)
				_types[i] = ((ReflectionTypeSymbol)types[i]).ReflectionType;

			return _type.GetProperty(name, _types) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
		{
			var _returnType = returnType != null ? ((ReflectionTypeSymbol)returnType).ReflectionType : null;

			var _types = new Type[types.Length];
			for (int i = 0; i < types.Length; i++)
				_types[i] = ((ReflectionTypeSymbol)types[i]).ReflectionType;

			return _type.GetProperty(name, _returnType, _types) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr)
		{
			return _type.GetProperty(name, bindingAttr) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name)
		{
			return _type.GetProperty(name) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
		{
			var _returnType = returnType != null ? ((ReflectionTypeSymbol)returnType).ReflectionType : null;

			return _type.GetProperty(name, _returnType) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public bool IsAssignableFrom(ITypeSymbol? c)
		{
			return _type.IsAssignableFrom(c != null ? ((ReflectionTypeSymbol)c).ReflectionType : null);
		}

		public bool IsEnumDefined(object value)
		{
			return _type.IsEnumDefined(value);
		}

		public bool IsSubclassOf(ITypeSymbol c)
		{
			return _type.IsSubclassOf(((ReflectionTypeSymbol)c).ReflectionType);
		}

		public ITypeSymbol MakeArrayType()
		{
			if (_asSZArray == null)
				Interlocked.CompareExchange(ref _asSZArray, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType()), null);

			return _asSZArray;
		}

		public ITypeSymbol MakeArrayType(int rank)
		{
			if (rank == 1)
				return MakeArrayType();

			if (_asArray == null)
				Interlocked.CompareExchange(ref _asArray, new ReflectionTypeSymbol?[32], null);

			ref var asArray = ref _asArray[rank];
			if (asArray == null)
				Interlocked.CompareExchange(ref asArray, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType(rank)), null);

			return asArray;
		}

		public ITypeSymbol MakeByRefType()
		{
			if (_asByRef == null)
				Interlocked.CompareExchange(ref _asByRef, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeByRefType()), null);

			return _asByRef;
		}

		public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol MakePointerType()
		{
			if (_asPointer == null)
				Interlocked.CompareExchange(ref _asPointer, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakePointerType()), null);

			return _asPointer;
		}

	}

}
