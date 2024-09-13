using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using MethodBase = IKVM.Reflection.MethodBase;
using ParameterInfo = IKVM.Reflection.ParameterInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

	abstract class IkvmReflectionMethodBaseSymbol : IkvmReflectionMemberSymbol, IMethodBaseSymbol
	{

		readonly MethodBase _method;

		ParameterInfo[]? _parametersSource;
		IkvmReflectionParameterSymbol?[]? _parameters;
		IkvmReflectionParameterSymbol? _returnParameter;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		/// <param name="type"></param>
		/// <param name="method"></param>
		public IkvmReflectionMethodBaseSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, IkvmReflectionTypeSymbol? type, MethodBase method) :
			base(context, module, type, method)
		{
			_method = method ?? throw new ArgumentNullException(nameof(method));
		}

		/// <summary>
		/// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		internal IkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
		{
			if (parameter is null)
				throw new ArgumentNullException(nameof(parameter));

			Debug.Assert(parameter.Member == _method);

			if (_parametersSource == null)
				Interlocked.CompareExchange(ref _parametersSource, _method.GetParameters().OrderBy(i => i.Position).ToArray(), null);
			if (_parameters == null)
				Interlocked.CompareExchange(ref _parameters, new IkvmReflectionParameterSymbol?[_parametersSource.Length], null);

			// index of current record
			var idx = parameter.Position;
			Debug.Assert(idx >= -1);
			Debug.Assert(idx < _parametersSource.Length);

			// check that our list is long enough to contain the entire table
			if (idx >= 0 && _parameters.Length < idx)
				throw new IndexOutOfRangeException();

			// if not yet created, create, allow multiple instances, but only one is eventually inserted
			ref var rec = ref _returnParameter;
			if (idx >= 0)
				rec = ref _parameters[idx];
			if (rec == null)
				Interlocked.CompareExchange(ref rec, new IkvmReflectionParameterSymbol(Context, this, parameter), null);

			// this should never happen
			if (rec is not IkvmReflectionParameterSymbol sym)
				throw new InvalidOperationException();

			return sym;
		}

		public System.Reflection.MethodAttributes Attributes => (System.Reflection.MethodAttributes)_method.Attributes;

		public System.Reflection.CallingConventions CallingConvention => (System.Reflection.CallingConventions)_method.CallingConvention;

		public bool ContainsGenericParameters => _method.ContainsGenericParameters;

		public bool IsAbstract => _method.IsAbstract;

		public bool IsAssembly => _method.IsAssembly;

		public bool IsConstructor => _method.IsConstructor;

		public bool IsFamily => _method.IsFamily;

		public bool IsFamilyAndAssembly => _method.IsFamilyAndAssembly;

		public bool IsFamilyOrAssembly => _method.IsFamilyOrAssembly;

		public bool IsFinal => _method.IsFinal;

		public bool IsGenericMethod => _method.IsGenericMethod;

		public bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition;

		public bool IsHideBySig => _method.IsHideBySig;

		public bool IsPrivate => _method.IsPrivate;

		public bool IsPublic => _method.IsPublic;

		public bool IsSpecialName => _method.IsSpecialName;

		public bool IsStatic => _method.IsStatic;

		public bool IsVirtual => _method.IsVirtual;

		public System.Reflection.MethodImplAttributes MethodImplementationFlags => (System.Reflection.MethodImplAttributes)_method.MethodImplementationFlags;

		public ITypeSymbol[] GetGenericArguments()
		{
			return ResolveTypeSymbols(_method.GetGenericArguments());
		}

		public System.Reflection.MethodImplAttributes GetMethodImplementationFlags()
		{
			return (System.Reflection.MethodImplAttributes)_method.GetMethodImplementationFlags();
		}

		public IParameterSymbol[] GetParameters()
		{
			return ResolveParameterSymbols(_method.GetParameters());
		}

	}

}