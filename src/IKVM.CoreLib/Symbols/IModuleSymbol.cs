using System;
using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

	interface IModuleSymbol : ISymbol, ICustomAttributeSymbolProvider
	{

		IAssemblySymbol Assembly { get; }

		string FullyQualifiedName { get; }

		int MetadataToken { get; }

		Guid ModuleVersionId { get; }

		string Name { get; }

		string ScopeName { get; }

		IFieldSymbol? GetField(string name);

		IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr);

		IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingFlags);

		IFieldSymbol[] GetFields();

		IMethodSymbol? GetMethod(string name);

		IMethodSymbol? GetMethod(string name, ITypeSymbol[] types);

		IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.Binder? binder, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers);

		IMethodSymbol[] GetMethods();

		IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingFlags);

		ITypeSymbol? GetType(string className);

		ITypeSymbol? GetType(string className, bool ignoreCase);

		ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase);

		ITypeSymbol[] GetTypes();

		bool IsResource();

		IFieldSymbol? ResolveField(int metadataToken);

		IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

		IMemberSymbol? ResolveMember(int metadataToken);

		IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

		IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

		IMethodBaseSymbol? ResolveMethod(int metadataToken);

		byte[] ResolveSignature(int metadataToken);

		string ResolveString(int metadataToken);

		ITypeSymbol ResolveType(int metadataToken);

		ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

	}

}
