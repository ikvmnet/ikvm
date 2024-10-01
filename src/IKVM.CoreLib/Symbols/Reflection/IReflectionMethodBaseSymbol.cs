using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionMethodBaseSymbol : IReflectionMemberSymbol, IMethodBaseSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/>.
        /// </summary>
        MethodBase UnderlyingMethodBase { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionParameterSymbol"/> for the given <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

    }

}
