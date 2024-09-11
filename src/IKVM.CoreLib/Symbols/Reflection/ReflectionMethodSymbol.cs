using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionMethodSymbol : ReflectionMethodBaseSymbol, IMethodSymbol
	{

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="method"></param>
		public ReflectionMethodSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, MethodInfo method) :
			base(context, type, method)
		{

		}

		public IParameterSymbol ReturnParameter => throw new NotImplementedException();

		public ITypeSymbol ReturnType => throw new NotImplementedException();

		public ICustomAttributeSymbolProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

		public IMethodSymbol GetBaseDefinition()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol GetGenericMethodDefinition()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments)
		{
			throw new NotImplementedException();
		}

	}

}