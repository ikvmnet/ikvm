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
        /// Gets the member that resolved the parameter.
        /// </summary>
        IIkvmReflectionMemberSymbol ResolvingMember { get; }

        /// <summary>
        /// Gets the underlying <see cref="ParameterInfo"/>.
        /// </summary>
        ParameterInfo UnderlyingParameter { get; }

    }

}
