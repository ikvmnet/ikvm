using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionFieldSymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionFieldSymbol>, IIkvmReflectionMemberSymbolBuilder, IFieldSymbolBuilder, IIkvmReflectionFieldSymbol
    {

        FieldBuilder UnderlyingFieldBuilder { get; }

    }

}
