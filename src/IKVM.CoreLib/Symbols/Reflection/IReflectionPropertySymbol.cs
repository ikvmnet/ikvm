using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionPropertySymbol : IReflectionMemberSymbol, IPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyInfo"/>.
        /// </summary>
        PropertyInfo UnderlyingProperty { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionParameterSymbol"/> for the given <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

    }

}
