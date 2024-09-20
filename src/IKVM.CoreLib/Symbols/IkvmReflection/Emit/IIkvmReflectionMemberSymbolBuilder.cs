using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionMemberSymbolBuilder : IIkvmReflectionSymbolBuilder, IMemberSymbolBuilder, IIkvmReflectionMemberSymbol
    {

        /// <summary>
        /// Gets the resolving <see cref="IIkvmReflectionModuleSymbolBuilder"/>.
        /// </summary>
        public IIkvmReflectionModuleSymbolBuilder ResolvingModuleBuilder { get; }

        /// <summary>
        /// Invoked when the type responsible for this builder is completed. Implementations should update their
        /// underlying type, and optionally dispatch the OnComplete invocation to downstream elements.
        /// </summary>
        void OnComplete();

    }

}
