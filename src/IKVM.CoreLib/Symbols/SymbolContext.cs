using System.Collections.Immutable;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    public abstract class SymbolContext
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected SymbolContext()
        {
            DefaultBinder = new DefaultBinder(this);
        }

        /// <summary>
        /// Attempts to resolve the specified type definition. If the type definition is missing, <c>null</c> is returned.
        /// </summary>
        /// <param name="definitionTypeSymbol"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal abstract TypeDefinition? ResolveTypeSource(DefinitionTypeSymbol definitionTypeSymbol);

        /// <summary>
        /// Gets the binder.
        /// </summary>
        internal DefaultBinder DefaultBinder { get; }

        /// <summary>
        /// Resolves the named type from the core assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public abstract TypeSymbol ResolveCoreType(string typeName);

        /// <summary>
        /// Defines a new assembly that has the specified identity and attributes.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract AssemblySymbolBuilder DefineAssembly(AssemblyIdentity identity, ImmutableArray<CustomAttribute> attributes);

        /// <summary>
        /// Attempts to resolve the source of the assembly with the specified identity.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        internal abstract AssemblyDefinition? ResolveAssemblyDef(AssemblyIdentity identity);

    }

}
