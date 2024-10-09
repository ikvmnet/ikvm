using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionFieldSymbol : IIkvmReflectionMemberSymbol, IFieldSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="FieldInfo"/>.
        /// </summary>
        FieldInfo UnderlyingField { get; }

    }

}
