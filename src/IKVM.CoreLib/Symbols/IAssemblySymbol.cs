using System.Collections.Generic;
using System.Reflection;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    interface IAssemblySymbol : ISymbol, ICustomAttributeProvider
    {

        IEnumerable<ITypeSymbol> DefinedTypes { get; }

        IMethodSymbol? EntryPoint { get; }

        IEnumerable<ITypeSymbol> ExportedTypes { get; }

        string? FullName { get; }

        string ImageRuntimeVersion { get; }

        IModuleSymbol ManifestModule { get; }

        IEnumerable<IModuleSymbol> Modules { get; }

        ITypeSymbol[] GetExportedTypes();

        IModuleSymbol? GetModule(string name);

        IModuleSymbol[] GetModules();

        IModuleSymbol[] GetModules(bool getResourceModules);

        AssemblyName GetName();

        AssemblyName GetName(bool copiedName);

        AssemblyName[] GetReferencedAssemblies();

        ITypeSymbol? GetType(string name, bool throwOnError);

        ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase);

        ITypeSymbol? GetType(string name);

        ITypeSymbol[] GetTypes();

    }

}