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
        /// Gets the underlying <see cref="FieldInfo"/> used for IL emit operations.
        /// </summary>
        FieldInfo UnderlyingEmitField { get; }

        /// <summary>
        /// Gets the underlying <see cref="FieldInfo"/> used for IL emit operations against dynamic methods.
        /// </summary>
        FieldInfo UnderlyingDynamicEmitField { get; }

    }

}
