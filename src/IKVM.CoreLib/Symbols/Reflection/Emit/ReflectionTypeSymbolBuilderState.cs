using System;
using System.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionTypeSymbolBuilderState
    {

        public ReflectionTypeSymbolBuilderPhase Phase = ReflectionTypeSymbolBuilderPhase.Default;
        public TypeBuilder? Builder;
        public Type? Type;

    }

}
