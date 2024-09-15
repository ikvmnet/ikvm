namespace IKVM.CoreLib.Symbols.Emit
{

    interface IMethodSymbolBuilder : ISymbolBuilder<IMethodSymbol>
    {

        /// <summary>
        /// Sets the implementation flags for this method.
        /// </summary>
        /// <param name="attributes"></param>
        void SetImplementationFlags(System.Reflection.MethodImplAttributes attributes);

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

        /// <summary>
        /// Sets the parameter attributes and the name of a parameter of this method, or of the return value of this method. Returns a ParameterBuilder that can be used to apply custom attributes.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="attributes"></param>
        /// <param name="strParamName"></param>
        /// <returns></returns>
        IParameterSymbolBuilder DefineParameter(int position, System.Reflection.ParameterAttributes attributes, string? strParamName);

        /// <summary>
        /// Returns an ILGenerator for this method with a default Microsoft intermediate language (MSIL) stream size of 64 bytes.
        /// </summary>
        /// <returns></returns>
        IILGenerator GetILGenerator();

        /// <summary>
        /// Returns an ILGenerator for this method with the specified Microsoft intermediate language (MSIL) stream size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        IILGenerator GetILGenerator(int size);

        /// <summary>
        /// Set a custom attribute using a custom attribute builder.
        /// </summary>
        /// <param name="customBuilder"></param>
        void SetCustomAttribute(ICustomAttributeBuilder customBuilder);

        /// <summary>
        /// Sets a custom attribute using a specified custom attribute blob.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="binaryAttribute"></param>
        void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute);

    }

}
