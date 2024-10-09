namespace IKVM.CoreLib.Symbols.Emit
{

    interface IPropertySymbolBuilder : ISymbolBuilder<IPropertySymbol>, IMemberSymbolBuilder, IPropertySymbol
    {

        /// <summary>
        /// Sets the default value of this property.
        /// </summary>
        /// <param name="defaultValue"></param>
        void SetConstant(object? defaultValue);

        /// <summary>
        /// Sets the method that gets the property value.
        /// </summary>
        /// <param name="mdBuilder"></param>
        void SetGetMethod(IMethodSymbolBuilder mdBuilder);

        /// <summary>
        /// Sets the method that sets the property value.
        /// </summary>
        /// <param name="mdBuilder"></param>
        void SetSetMethod(IMethodSymbolBuilder mdBuilder);

        /// <summary>
        /// Adds one of the other methods associated with this property.
        /// </summary>
        /// <param name="mdBuilder"></param>
        void AddOtherMethod(IMethodSymbolBuilder mdBuilder);

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