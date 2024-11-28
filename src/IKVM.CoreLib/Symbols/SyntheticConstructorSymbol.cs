using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SyntheticConstructorSymbol : SyntheticMethodSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConventions"></param>
        /// <param name="parameters"></param>
        public SyntheticConstructorSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol declaringType, MethodAttributes attributes, CallingConventions callingConventions, ImmutableArray<ParameterSymbol> parameters) :
            base(context, module, declaringType, (attributes & MethodAttributes.Static) != 0 ? ConstructorInfo.TypeConstructorName : ConstructorInfo.ConstructorName, attributes | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, callingConventions, null, parameters)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConventions"></param>
        /// <param name="parameters"></param>
        public SyntheticConstructorSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol declaringType, MethodAttributes attributes, CallingConventions callingConventions, ImmutableArray<TypeSymbol> parameters) :
            base(context, module, declaringType, (attributes & MethodAttributes.Static) != 0 ? ConstructorInfo.TypeConstructorName : ConstructorInfo.ConstructorName, attributes | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, callingConventions, null, parameters)
        {

        }

    }

}
