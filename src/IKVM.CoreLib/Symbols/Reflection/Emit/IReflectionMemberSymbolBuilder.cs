using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionMemberSymbolBuilder : IReflectionSymbolBuilder, IMemberSymbolBuilder, IReflectionMemberSymbol
    {

        /// <summary>
        /// Gets the resolving <see cref="IReflectionModuleSymbolBuilder"/>.
        /// </summary>
        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder { get; }

        /// <summary>
        /// Invoked when the type responsible for this builder is completed. Implementations should update their
        /// underlying type, and optionally dispatch the OnComplete invocation to downstream elements.
        /// </summary>
        void OnComplete();

    }

}
