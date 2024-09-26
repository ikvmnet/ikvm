namespace IKVM.CoreLib.Symbols.Emit
{

    interface IMethodSymbolBuilder : ISymbolBuilder<IMethodSymbol>, IMethodBaseSymbolBuilder, IMethodSymbol
    {

        /// <summary>
        /// Sets the number and types of parameters for a method.
        /// </summary>
        /// <param name="parameterTypes"></param>
        void SetParameters(params ITypeSymbol[] parameterTypes);

        /// <summary>
        /// Sets the return type of the method.
        /// </summary>
        /// <param name="returnType"></param>
        void SetReturnType(ITypeSymbol? returnType);

        /// <summary>
        /// Sets the method signature, including the return type, the parameter types, and the required and optional custom modifiers of the return type and parameter types.
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="returnTypeRequiredCustomModifiers"></param>
        /// <param name="returnTypeOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterTypeRequiredCustomModifiers"></param>
        /// <param name="parameterTypeOptionalCustomModifiers"></param>
        void SetSignature(ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers);

        /// <summary>
        /// Sets the number of generic type parameters for the current method, specifies their names, and returns an array of GenericTypeParameterBuilder objects that can be used to define their constraints.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names);

    }

}
