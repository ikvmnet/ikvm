using System.Collections.Immutable;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    interface ISymbolContext
    {

        /// <summary>
        /// Resolves the named type from the core assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeSymbol ResolveCoreType(string typeName);

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        AssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, ImmutableArray<CustomAttribute> attributes);

    }

}
