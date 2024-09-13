using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.Reflection;

using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using EventInfo = IKVM.Reflection.EventInfo;
using FieldInfo = IKVM.Reflection.FieldInfo;
using MethodBase = IKVM.Reflection.MethodBase;
using MethodInfo = IKVM.Reflection.MethodInfo;
using PropertyInfo = IKVM.Reflection.PropertyInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

	class IkvmReflectionTypeSymbol : IkvmReflectionMemberSymbol, ITypeSymbol
	{

		const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

		readonly Type _type;

		MethodBase[]? _methodsSource;
		int _methodsBaseRow;
		IkvmReflectionMethodBaseSymbol?[]? _methods;

		FieldInfo[]? _fieldsSource;
		int _fieldsBaseRow;
		IkvmReflectionFieldSymbol?[]? _fields;

		PropertyInfo[]? _propertiesSource;
		int _propertiesBaseRow;
		IkvmReflectionPropertySymbol?[]? _properties;

		EventInfo[]? _eventsSource;
		int _eventsBaseRow;
		IkvmReflectionEventSymbol?[]? _events;

		IkvmReflectionTypeSymbol?[]? _asArray;
		IkvmReflectionTypeSymbol? _asSZArray;
		IkvmReflectionTypeSymbol? _asPointer;
		IkvmReflectionTypeSymbol? _asByRef;

		Type[]? _genericParametersSource;
		IkvmReflectionTypeSymbol?[]? _genericParameters;
		List<(Type[] Arguments, IkvmReflectionTypeSymbol Symbol)>? _genericTypes;
		ReaderWriterLockSlim? _genericTypesLock;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		/// <param name="type"></param>
		public IkvmReflectionTypeSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, Type type) :
			base(context, module, null, type)
		{
			Debug.Assert(module.ReflectionObject == type.Module);
			_type = type ?? throw new ArgumentNullException(nameof(type));
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		internal IkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
		{
			return (IkvmReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionConstructorSymbol"/> cached for the type by ctor.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		internal IkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
		{
			return (IkvmReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		internal IkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
		{
			if (method is null)
				throw new ArgumentNullException(nameof(method));

			Debug.Assert(method.DeclaringType == _type);

			var hnd = MetadataTokens.MethodDefinitionHandle(method.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_methodsSource == null)
			{
				Interlocked.CompareExchange(ref _methodsSource, _type.GetConstructors(DefaultBindingFlags).Cast<MethodBase>().Concat(_type.GetMethods(DefaultBindingFlags)).OrderBy(i => i.MetadataToken).ToArray(), null);
				_methodsBaseRow = _methodsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_methodsSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			if (_methods == null)
				Interlocked.CompareExchange(ref _methods, new IkvmReflectionMethodBaseSymbol?[_methodsSource.Length], null);

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
						Interlocked.CompareExchange(ref rec, new IkvmReflectionConstructorSymbol(Context, ContainingModule, this, c), null);
						break;
					case MethodInfo m:
						Interlocked.CompareExchange(ref rec, new IkvmReflectionMethodSymbol(Context, ContainingModule, this, m), null);
						break;
				}
			}

			// this should never happen
			if (rec is not IkvmReflectionMethodBaseSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionFieldSymbol"/> cached for the type by field.
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal IkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
		{
			if (field is null)
				throw new ArgumentNullException(nameof(field));

			Debug.Assert(field.DeclaringType == _type);

			var hnd = MetadataTokens.FieldDefinitionHandle(field.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_fieldsSource == null)
			{
				Interlocked.CompareExchange(ref _fieldsSource, _type.GetFields(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray(), null);
				_fieldsBaseRow = _fieldsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_fieldsSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			if (_fields == null)
				Interlocked.CompareExchange(ref _fields, new IkvmReflectionFieldSymbol?[_fieldsSource.Length], null);

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
				Interlocked.CompareExchange(ref rec, new IkvmReflectionFieldSymbol(Context, this, field), null);

			// this should never happen
			if (rec is not IkvmReflectionFieldSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionPropertySymbol"/> cached for the type by property.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal IkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
		{
			if (property is null)
				throw new ArgumentNullException(nameof(property));

			Debug.Assert(property.DeclaringType == _type);

			var hnd = MetadataTokens.PropertyDefinitionHandle(property.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_propertiesSource == null)
			{
				Interlocked.CompareExchange(ref _propertiesSource, _type.GetProperties(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray(), null);
				_propertiesBaseRow = _propertiesSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_propertiesSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			if (_properties == null)
				Interlocked.CompareExchange(ref _properties, new IkvmReflectionPropertySymbol?[_propertiesSource.Length], null);

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
				Interlocked.CompareExchange(ref rec, new IkvmReflectionPropertySymbol(Context, this, property), null);

			// this should never happen
			if (rec is not IkvmReflectionPropertySymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionEventSymbol"/> cached for the type by event.
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal IkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
		{
			if (@event is null)
				throw new ArgumentNullException(nameof(@event));

			Debug.Assert(@event.DeclaringType == _type);

			var hnd = MetadataTokens.PropertyDefinitionHandle(@event.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source events
			if (_eventsSource == null)
			{
				Interlocked.CompareExchange(ref _eventsSource, _type.GetEvents(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray(), null);
				_eventsBaseRow = _eventsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(_eventsSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			if (_events == null)
				Interlocked.CompareExchange(ref _events, new IkvmReflectionEventSymbol?[_eventsSource.Length], null);

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
				Interlocked.CompareExchange(ref rec, new IkvmReflectionEventSymbol(Context, this, @event), null);

			// this should never happen
			if (rec is not IkvmReflectionEventSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the generic parameter type.
		/// </summary>
		/// <param name="genericTypeParameterType"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal IkvmReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericTypeParameterType)
		{
			if (genericTypeParameterType is null)
				throw new ArgumentNullException(nameof(genericTypeParameterType));

			Debug.Assert(genericTypeParameterType.DeclaringType == _type);

			// initialize tables
			if (_genericParametersSource == null)
				Interlocked.CompareExchange(ref _genericParametersSource, _type.GetGenericArguments(), null);
			if (_genericParameters == null)
				Interlocked.CompareExchange(ref _genericParameters, new IkvmReflectionTypeSymbol?[_genericParametersSource.Length], null);

			// position of the parameter in the list
			var idx = genericTypeParameterType.GenericParameterPosition;

			// check that our list is long enough to contain the entire table
			if (_genericParameters.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _genericParameters[idx];
			if (rec == null)
				Interlocked.CompareExchange(ref rec, new IkvmReflectionTypeSymbol(Context, ContainingModule, genericTypeParameterType), null);

			// this should never happen
			if (rec is not IkvmReflectionTypeSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the generic parameter type.
		/// </summary>
		/// <param name="genericTypeArguments"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal IkvmReflectionTypeSymbol GetOrCreateGenericTypeSymbol(Type[] genericTypeArguments)
		{
			if (genericTypeArguments is null)
				throw new ArgumentNullException(nameof(genericTypeArguments));

			if (_type.IsGenericTypeDefinition == false)
				throw new InvalidOperationException();

			// initialize the available parameters
			if (_genericParametersSource == null)
				Interlocked.CompareExchange(ref _genericParametersSource, _type.GetGenericArguments(), null);
			if (_genericParametersSource.Length != genericTypeArguments.Length)
				throw new InvalidOperationException();

			// initialize generic type map, and lock on it since we're potentially adding items
			if (_genericTypes == null)
				Interlocked.CompareExchange(ref _genericTypes, [], null);
			if (_genericTypesLock == null)
				Interlocked.CompareExchange(ref _genericTypesLock, new(), null);

			try
			{
				_genericTypesLock.EnterUpgradeableReadLock();

				// find existing entry
				foreach (var i in _genericTypes)
					if (i.Arguments.SequenceEqual(genericTypeArguments))
						return i.Symbol;

				try
				{
					_genericTypesLock.EnterWriteLock();

					// generate new symbol
					var sym = new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeGenericType(genericTypeArguments));
					_genericTypes.Add((genericTypeArguments, sym));
					return sym;
				}
				finally
				{
					_genericTypesLock.ExitWriteLock();
				}
			}
			finally
			{
				_genericTypesLock.ExitUpgradeableReadLock();
			}
		}

		/// <summary>
		/// Resolves the symbol for the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected internal override IkvmReflectionTypeSymbol ResolveTypeSymbol(Type type)
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
		protected internal override IkvmReflectionConstructorSymbol ResolveConstructorSymbol(ConstructorInfo ctor)
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
		protected internal override IkvmReflectionMethodSymbol ResolveMethodSymbol(MethodInfo method)
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
		protected internal override IkvmReflectionFieldSymbol ResolveFieldSymbol(FieldInfo field)
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
		protected internal override IkvmReflectionPropertySymbol ResolvePropertySymbol(PropertyInfo property)
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
		protected internal override IkvmReflectionEventSymbol ResolveEventSymbol(EventInfo @event)
		{
			if (@event.DeclaringType == _type)
				return GetOrCreateEventSymbol(@event);
			else
				return base.ResolveEventSymbol(@event);
		}

		/// <summary>
		/// Gets the wrapped <see cref="Type"/>.
		/// </summary>
		internal new Type ReflectionObject => _type;

		public IAssemblySymbol Assembly => Context.GetOrCreateAssemblySymbol(_type.Assembly);

		public string? AssemblyQualifiedName => _type.AssemblyQualifiedName;

		public System.Reflection.TypeAttributes Attributes => (System.Reflection.TypeAttributes)(int)_type.Attributes;

		public ITypeSymbol? BaseType => _type.BaseType != null ? ResolveTypeSymbol(_type.BaseType) : null;

		public bool ContainsGenericParameters => _type.ContainsGenericParameters;

		public IMethodBaseSymbol? DeclaringMethod => _type.DeclaringMethod is MethodInfo m ? ResolveMethodSymbol(m) : null;

		public string? FullName => _type.FullName;

		public System.Reflection.GenericParameterAttributes GenericParameterAttributes => (System.Reflection.GenericParameterAttributes)(int)_type.GenericParameterAttributes;

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

		public bool IsNestedFamORAssem => _type.IsNestedFamORAssem;

		public bool IsNestedFamily => _type.IsNestedFamily;

		public bool IsNestedPrivate => _type.IsNestedPrivate;

		public bool IsNestedPublic => _type.IsNestedPublic;

		public bool IsNotPublic => _type.IsNotPublic;

		public bool IsPointer => _type.IsPointer;

		public bool IsPrimitive => _type.IsPrimitive;

		public bool IsPublic => _type.IsPublic;

		public bool IsSealed => _type.IsSealed;

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

		public IConstructorSymbol? GetConstructor(System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
		{
			return _type.GetConstructor((BindingFlags)bindingAttr, binder: null, UnpackTypeSymbols(types), modifiers: null) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
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

		public IEventSymbol? GetEvent(string name, System.Reflection.BindingFlags bindingAttr)
		{
			return _type.GetEvent(name, (BindingFlags)bindingAttr) is { } f ? ResolveEventSymbol(f) : null;
		}

		public IEventSymbol? GetEvent(string name)
		{
			return _type.GetEvent(name) is { } f ? ResolveEventSymbol(f) : null;
		}

		public IEventSymbol[] GetEvents()
		{
			return ResolveEventSymbols(_type.GetEvents());
		}

		public IEventSymbol[] GetEvents(System.Reflection.BindingFlags bindingAttr)
		{
			return ResolveEventSymbols(_type.GetEvents((BindingFlags)bindingAttr));
		}

		public IFieldSymbol? GetField(string name)
		{
			return _type.GetField(name) is FieldInfo f ? ResolveFieldSymbol(f) : null;
		}

		public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
		{
			return _type.GetField(name, (BindingFlags)bindingAttr) is FieldInfo f ? ResolveFieldSymbol(f) : null;
		}

		public IFieldSymbol[] GetFields()
		{
			return ResolveFieldSymbols(_type.GetFields());
		}

		public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingAttr)
		{
			return ResolveFieldSymbols(_type.GetFields((BindingFlags)bindingAttr));
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

		public IMemberSymbol[] GetMember(string name, System.Reflection.BindingFlags bindingAttr)
		{
			return ResolveMemberSymbols(_type.GetMember(name, (BindingFlags)bindingAttr));
		}

		public IMemberSymbol[] GetMember(string name, System.Reflection.MemberTypes type, System.Reflection.BindingFlags bindingAttr)
		{
			return ResolveMemberSymbols(_type.GetMember(name, (MemberTypes)type, (BindingFlags)bindingAttr));
		}

		public IMemberSymbol[] GetMembers(System.Reflection.BindingFlags bindingAttr)
		{
			return ResolveMemberSymbols(_type.GetMembers((BindingFlags)bindingAttr));
		}

		public IMemberSymbol[] GetMembers()
		{
			return ResolveMemberSymbols(_type.GetMembers());
		}

		public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr)
		{
			return _type.GetMethod(name, (BindingFlags)bindingAttr) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
		{
			return _type.GetMethod(name, (BindingFlags)bindingAttr, null, UnpackTypeSymbols(types), null) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
		{
			return _type.GetMethod(name, UnpackTypeSymbols(types)) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetMethod(string name)
		{
			return _type.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
		{
			if (modifiers != null)
				throw new NotImplementedException();

			return _type.GetMethod(name, UnpackTypeSymbols(types), null) is { } m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingAttr)
		{
			return ResolveMethodSymbols(_type.GetMethods((BindingFlags)bindingAttr));
		}

		public IMethodSymbol[] GetMethods()
		{
			return ResolveMethodSymbols(_type.GetMethods());
		}

		public ITypeSymbol? GetNestedType(string name)
		{
			return _type.GetNestedType(name) is Type t ? ResolveTypeSymbol(t) : null;
		}

		public ITypeSymbol? GetNestedType(string name, System.Reflection.BindingFlags bindingAttr)
		{
			return _type.GetNestedType(name, (BindingFlags)bindingAttr) is Type t ? ResolveTypeSymbol(t) : null;
		}

		public ITypeSymbol[] GetNestedTypes()
		{
			return ResolveTypeSymbols(_type.GetNestedTypes());
		}

		public ITypeSymbol[] GetNestedTypes(System.Reflection.BindingFlags bindingAttr)
		{
			return ResolveTypeSymbols(_type.GetNestedTypes((BindingFlags)bindingAttr));
		}

		public IPropertySymbol[] GetProperties()
		{
			return ResolvePropertySymbols(_type.GetProperties());
		}

		public IPropertySymbol[] GetProperties(System.Reflection.BindingFlags bindingAttr)
		{
			return ResolvePropertySymbols(_type.GetProperties((BindingFlags)bindingAttr));
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
		{
			if (modifiers != null)
				throw new NotImplementedException();

			var _returnType = returnType != null ? ((IkvmReflectionTypeSymbol)returnType).ReflectionObject : null;
			return _type.GetProperty(name, _returnType, UnpackTypeSymbols(types), null) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
		{
			return _type.GetProperty(name, UnpackTypeSymbols(types)) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
		{
			var _returnType = returnType != null ? ((IkvmReflectionTypeSymbol)returnType).ReflectionObject : null;
			return _type.GetProperty(name, _returnType, UnpackTypeSymbols(types)) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, System.Reflection.BindingFlags bindingAttr)
		{
			return _type.GetProperty(name, (BindingFlags)bindingAttr) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name)
		{
			return _type.GetProperty(name) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
		{
			var _returnType = returnType != null ? ((IkvmReflectionTypeSymbol)returnType).ReflectionObject : null;

			return _type.GetProperty(name, _returnType) is { } p ? ResolvePropertySymbol(p) : null;
		}

		public bool IsAssignableFrom(ITypeSymbol? c)
		{
			return _type.IsAssignableFrom(c != null ? ((IkvmReflectionTypeSymbol)c).ReflectionObject : null);
		}

		public bool IsEnumDefined(object value)
		{
			return _type.IsEnumDefined(value);
		}

		public bool IsSubclassOf(ITypeSymbol c)
		{
			return _type.IsSubclassOf(((IkvmReflectionTypeSymbol)c).ReflectionObject);
		}

		public ITypeSymbol MakeArrayType()
		{
			if (_asSZArray == null)
				Interlocked.CompareExchange(ref _asSZArray, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType()), null);

			return _asSZArray;
		}

		public ITypeSymbol MakeArrayType(int rank)
		{
			if (rank == 1)
				return MakeArrayType();

			if (_asArray == null)
				Interlocked.CompareExchange(ref _asArray, new IkvmReflectionTypeSymbol?[32], null);

			ref var asArray = ref _asArray[rank];
			if (asArray == null)
				Interlocked.CompareExchange(ref asArray, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType(rank)), null);

			return asArray;
		}

		public ITypeSymbol MakeByRefType()
		{
			if (_asByRef == null)
				Interlocked.CompareExchange(ref _asByRef, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeByRefType()), null);

			return _asByRef;
		}

		public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol MakePointerType()
		{
			if (_asPointer == null)
				Interlocked.CompareExchange(ref _asPointer, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakePointerType()), null);

			return _asPointer;
		}

	}

}
