using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionPropertySymbol : IReflectionMemberSymbol, IPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyInfo"/>.
        /// </summary>
        PropertyInfo UnderlyingProperty { get; }

        /// <summary>
        /// Gets the underlying <see cref="PropertyInfo"/>.
        /// </summary>
        PropertyInfo UnderlyingRuntimeProperty { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionParameterSymbol"/> for the given <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionParameterSymbolBuilder"/> for the given <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter);

    }

}
