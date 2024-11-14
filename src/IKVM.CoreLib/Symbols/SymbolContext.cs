using System.Collections.Immutable;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    abstract class SymbolContext
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected SymbolContext()
        {
            DefaultBinder = new DefaultBinder(this);
        }

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
        /// Defines a dynamic assembly that has the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract AssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, ImmutableArray<CustomAttribute> attributes);

    }

}
