using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface IParameterSymbol : ISymbol, ICustomAttributeSymbolProvider
	{

		ParameterAttributes Attributes { get; }

		object? DefaultValue { get; }

		bool HasDefaultValue { get; }

		bool IsIn { get; }

		bool IsLcid { get; }

		bool IsOptional { get; }

		bool IsOut { get; }

		bool IsRetval { get; }

		IMemberSymbol Member { get; }

		int MetadataToken { get; }

		string? Name { get; }

		ITypeSymbol ParameterType { get; }

		int Position { get; }

	}

}
