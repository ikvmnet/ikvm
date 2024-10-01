using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionPropertySymbolBuilder : IReflectionSymbolBuilder, IReflectionMemberSymbolBuilder, IPropertySymbolBuilder, IReflectionPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyBuilder"/>.
        /// </summary>
        PropertyBuilder UnderlyingPropertyBuilder { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionParameterSymbolBuilder"/> for the given <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter);

    }

}
