namespace IKVM.CoreLib.Symbols.Emit
{

    interface IFieldSymbolBuilder : ISymbolBuilder<IFieldSymbol>, IMemberSymbolBuilder, IFieldSymbol
    {

        /// <summary>
        /// Sets the default value of this field.
        /// </summary>
        /// <param name="defaultValue"></param>
        void SetConstant(object? defaultValue);

        /// <summary>
        /// Specifies the field layout.
        /// </summary>
        /// <param name="iOffset"></param>
        void SetOffset(int iOffset);

    }

}
