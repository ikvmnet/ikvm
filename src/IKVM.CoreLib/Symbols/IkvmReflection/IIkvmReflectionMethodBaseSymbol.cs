using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionMethodBaseSymbol : IIkvmReflectionMemberSymbol, IMethodBaseSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodBase"/>.
        /// </summary>
        MethodBase UnderlyingMethodBase { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbol"/> for the given <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

    }

}
