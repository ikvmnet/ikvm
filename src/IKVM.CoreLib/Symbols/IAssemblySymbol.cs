using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface IAssemblySymbol : ISymbol, ICustomAttributeSymbolProvider
	{

		ImmutableArray<ICustomAttributeSymbol> CustomAttributes { get; }

		ImmutableArray<ITypeSymbol> DefinedTypes { get; }

		IMethodSymbol? EntryPoint { get; }

		ImmutableArray<ITypeSymbol> ExportedTypes { get; }

		string? FullName { get; }

		string ImageRuntimeVersion { get; }

		IModuleSymbol ManifestModule { get; }

		ImmutableArray<IModuleSymbol> Modules { get; }

		ImmutableArray<ICustomAttributeSymbol> GetCustomAttributesData();

		ImmutableArray<ITypeSymbol> GetExportedTypes();

		ImmutableArray<ITypeSymbol> GetForwardedTypes();

		IModuleSymbol? GetModule(string name);

		ImmutableArray<IModuleSymbol> GetModules();

		ImmutableArray<IModuleSymbol> GetModules(bool getResourceModules);

		AssemblyName GetName();

		AssemblyName GetName(bool copiedName);

		ImmutableArray<AssemblyName> GetReferencedAssemblies();

		ITypeSymbol? GetType(string name, bool throwOnError);

		ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase);

		ITypeSymbol? GetType(string name);

		ImmutableArray<ITypeSymbol> GetTypes();

	}

}