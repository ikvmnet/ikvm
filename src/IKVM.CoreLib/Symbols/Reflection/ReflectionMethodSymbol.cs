using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionMethodSymbol : ReflectionMethodBaseSymbol, IMethodSymbol
	{

		readonly MethodInfo _method;

		Type[]? _genericParametersSource;
		ReflectionTypeSymbol?[]? _genericParameters;
		List<(Type[] Arguments, ReflectionMethodSymbol Symbol)>? _genericTypes;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		/// <param name="type"></param>
		/// <param name="method"></param>
		public ReflectionMethodSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol? type, MethodInfo method) :
			base(context, module, type, method)
		{
			_method = method ?? throw new ArgumentNullException(nameof(method));
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the generic parameter type.
		/// </summary>
		/// <param name="genericParameterType"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericParameterType)
		{
			if (genericParameterType is null)
				throw new ArgumentNullException(nameof(genericParameterType));

			Debug.Assert(genericParameterType.DeclaringMethod == _method);

			// initialize tables
			_genericParametersSource ??= _method.GetGenericArguments();
			_genericParameters ??= new ReflectionTypeSymbol?[_genericParametersSource.Length];

			// position of the parameter in the list
			var idx = genericParameterType.GenericParameterPosition;

			// check that our list is long enough to contain the entire table
			if (_genericParameters.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _genericParameters[idx];
			if (rec == null)
				Interlocked.CompareExchange(ref rec, new ReflectionTypeSymbol(Context, ContainingModule, genericParameterType), null);

			// this should never happen
			if (rec is not ReflectionTypeSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		/// <summary>
		/// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the generic parameter type.
		/// </summary>
		/// <param name="genericMethodArguments"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionMethodSymbol GetOrCreateGenericTypeSymbol(Type[] genericMethodArguments)
		{
			if (genericMethodArguments is null)
				throw new ArgumentNullException(nameof(genericMethodArguments));

			if (_method.IsGenericMethodDefinition == false)
				throw new InvalidOperationException();

			// initialize the available parameters
			_genericParametersSource ??= _method.GetGenericArguments();
			if (_genericParametersSource.Length != genericMethodArguments.Length)
				throw new InvalidOperationException();

			// initialize generic type map, and lock on it since we're potentially adding items
			_genericTypes ??= [];
			lock (_genericTypes)
			{
				// find existing entry
				foreach (var i in _genericTypes)
					if (i.Arguments.SequenceEqual(genericMethodArguments))
						return i.Symbol;

				// generate new symbol
				var sym = new ReflectionMethodSymbol(Context, ContainingModule, ContainingType, _method.MakeGenericMethod(genericMethodArguments));
				_genericTypes.Add((genericMethodArguments, sym));
				return sym;
			}
		}

		public IParameterSymbol ReturnParameter => ResolveParameterSymbol(_method.ReturnParameter);

		public ITypeSymbol ReturnType => ResolveTypeSymbol(_method.ReturnType);

		public ICustomAttributeSymbolProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

		public IMethodSymbol GetBaseDefinition()
		{
			return ResolveMethodSymbol(_method.GetBaseDefinition());
		}

		public IMethodSymbol GetGenericMethodDefinition()
		{
			return ResolveMethodSymbol(_method.GetGenericMethodDefinition());
		}

		public IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments)
		{
			throw new NotImplementedException();
		}

	}

}