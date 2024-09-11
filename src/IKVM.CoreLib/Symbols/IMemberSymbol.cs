using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface IMemberSymbol : ISymbol, ICustomAttributeSymbolProvider
	{

		ITypeSymbol? DeclaringType { get; }

		MemberTypes MemberType { get; }

		int MetadataToken { get; }

		IModuleSymbol Module { get; }
		
		string Name { get; }

	}

}
