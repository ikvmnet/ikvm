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

    }

}