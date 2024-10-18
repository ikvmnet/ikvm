using System.Collections.Immutable;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    interface ISymbolContext
    {

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collectable"></param>
        /// <param name="saveable"></param>
        /// <returns></returns>
        IAssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, bool collectable = true, bool saveable = false);

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="collectable"></param>
        /// <param name="saveable"></param>
        /// <returns></returns>
        IAssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, ImmutableArray<CustomAttribute> attributes, bool collectable = true, bool saveable = false);
    }

}
