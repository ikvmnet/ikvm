using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionTypeSymbolBuilderState
    {

        public IkvmReflectionTypeSymbolBuilderPhase Phase = IkvmReflectionTypeSymbolBuilderPhase.Default;
        public TypeBuilder? Builder;
        public Type? Type;

    }

}
