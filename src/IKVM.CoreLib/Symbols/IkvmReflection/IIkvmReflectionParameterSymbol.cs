using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionParameterSymbol : IIkvmReflectionSymbol, IParameterSymbol
    {

        /// <summary>
        /// Gets the module that resolved the parameter.
        /// </summary>
        IIkvmReflectionModuleSymbol ResolvingModule { get; }

        /// <summary>
        /// Gets the method that resolved the parameter.
        /// </summary>
        IIkvmReflectionMethodBaseSymbol ResolvingMethod { get; }

        /// <summary>
        /// Gets the underlying <see cref="ParameterInfo"/>.
        /// </summary>
        ParameterInfo UnderlyingParameter { get; }

    }

}
