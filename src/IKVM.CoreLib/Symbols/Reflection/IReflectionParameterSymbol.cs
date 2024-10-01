using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionParameterSymbol : IReflectionSymbol, IParameterSymbol
    {

        /// <summary>
        /// Gets the module that resolved the parameter.
        /// </summary>
        IReflectionModuleSymbol ResolvingModule { get; }

        /// <summary>
        /// Gets the member that resolved the parameter.
        /// </summary>
        IReflectionMemberSymbol ResolvingMember { get; }

        /// <summary>
        /// Gets the underlying <see cref="ParameterInfo"/>.
        /// </summary>
        ParameterInfo UnderlyingParameter { get; }

    }

}
