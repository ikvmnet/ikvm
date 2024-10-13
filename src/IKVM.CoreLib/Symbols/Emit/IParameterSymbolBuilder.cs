namespace IKVM.CoreLib.Symbols.Emit
{

    interface IParameterSymbolBuilder : ISymbolBuilder<IParameterSymbol>, IParameterSymbol, ICustomAttributeProviderBuilder
    {

        /// <summary>
        /// Sets the default value of the parameter.
        /// </summary>
        /// <param name="defaultValue"></param>
        void SetConstant(object? defaultValue);

    }

}