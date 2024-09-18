using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionFieldSymbolBuilder : IReflectionSymbolBuilder<IReflectionFieldSymbol>, IReflectionMemberSymbolBuilder, IFieldSymbolBuilder, IReflectionFieldSymbol
    {

        FieldBuilder UnderlyingFieldBuilder { get; }

    }

}
