using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionFieldSymbol : IReflectionMemberSymbol, IFieldSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="FieldInfo"/>.
        /// </summary>
        FieldInfo UnderlyingField { get; }

        /// <summary>
        /// Gets the underlying <see cref="FieldInfo"/>.
        /// </summary>
        FieldInfo UnderlyingRuntimeField { get; }

    }

}
