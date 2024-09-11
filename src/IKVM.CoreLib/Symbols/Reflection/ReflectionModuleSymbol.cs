using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

	/// <summary>
	/// Implementation of <see cref="IModuleSymbol"/> derived from System.Reflection.
	/// </summary>
	class ReflectionModuleSymbol : IModuleSymbol
	{

		readonly ReflectionSymbolContext _context;
		readonly Module _module;

		Type[]? _typesSource;
		int _typesBaseRow;
		ReflectionTypeSymbol?[]? _types;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ReflectionModuleSymbol(ReflectionSymbolContext context, Module module)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_module = module ?? throw new ArgumentNullException(nameof(module));
		}

		/// <summary>
		/// Gets the wrapped <see cref="Module"/>.
		/// </summary>
		internal Module ReflectionModule => _module;

		/// <summary>
		/// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the module by type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
		{
			if (type is null)
				throw new ArgumentNullException(nameof(type));

			Debug.Assert(type.Module == _module);

			// look up handle and row
			var hnd = MetadataTokens.TypeDefinitionHandle(type.MetadataToken);
			var row = MetadataTokens.GetRowNumber(hnd);

			// initialize source table
			if (_typesSource == null)
			{
				_typesSource = _module.GetTypes().OrderBy(i => i.MetadataToken).ToArray();
				_typesBaseRow = _typesSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_typesSource[0].MetadataToken)) : 0;
			}

			// initialize cache table
			_types ??= new ReflectionTypeSymbol?[_typesSource.Length];

			// index of current record is specified row - base
			var idx = row - _typesBaseRow;
			Debug.Assert(idx >= 0);
			Debug.Assert(idx < _typesSource.Length);

			// check that our type list is long enough to contain the entire table
			if (_types.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			if (_types[idx] == null)
				Interlocked.CompareExchange(ref _types[idx], new ReflectionTypeSymbol(_context, this, type), null);

			// this should never happen
			if (_types[idx] is not ReflectionTypeSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		public IAssemblySymbol Assembly => _context.GetOrCreateAssemblySymbol(_module.Assembly);

		public ImmutableArray<ICustomAttributeSymbol> CustomAttributes => throw new NotImplementedException();

		public string FullyQualifiedName => throw new NotImplementedException();

		public int MetadataToken => throw new NotImplementedException();

		public Guid ModuleVersionId => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();

		public string ScopeName => throw new NotImplementedException();

		public bool IsMissing => throw new NotImplementedException();

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
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

		public IFieldSymbol[] GetFields(BindingFlags bindingFlags)
		{
			throw new NotImplementedException();
		}

		public IFieldSymbol[] GetFields()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, ITypeSymbol[]? types, ParameterModifier[]? modifiers)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol[] GetMethods()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol[] GetMethods(BindingFlags bindingFlags)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol? GetType(string className)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol? GetType(string className, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetTypes()
		{
			throw new NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public bool IsResource()
		{
			throw new NotImplementedException();
		}

		public IFieldSymbol? ResolveField(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol? ResolveMember(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
		{
			throw new NotImplementedException();
		}

		public IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
		{
			throw new NotImplementedException();
		}

		public IMethodBaseSymbol? ResolveMethod(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public byte[] ResolveSignature(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public string ResolveString(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol ResolveType(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
		{
			throw new NotImplementedException();
		}
	}

}
