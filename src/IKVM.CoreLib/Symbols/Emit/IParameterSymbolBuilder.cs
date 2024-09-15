namespace IKVM.CoreLib.Symbols.Emit
{

    interface IParameterSymbolBuilder : ISymbolBuilder<IParameterSymbol>
    {
        /// <summary>
        /// Sets the default value of the parameter.
        /// </summary>
        /// <param name="defaultValue"></param>
        void SetConstant(object? defaultValue);

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