using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionPropertySymbol : IIkvmReflectionMemberSymbol, IPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyInfo"/>.
        /// </summary>
        PropertyInfo UnderlyingProperty { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbol"/> for the given <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

    }

}
