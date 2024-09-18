using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionMemberSymbolBuilder : IReflectionSymbolBuilder<IReflectionMemberSymbol>, IMemberSymbolBuilder, IReflectionMemberSymbol
    {

        /// <summary>
        /// Invoked when the type responsible for this builder is completed. Implementations should update their
        /// underlying type, and optionally dispatch the OnComplete invocation to downstream elements.
        /// </summary>
        void OnComplete();

    }

}
