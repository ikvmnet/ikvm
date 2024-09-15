namespace IKVM.CoreLib.Symbols.Emit
{

    interface IFieldSymbolBuilder : ISymbolBuilder<IFieldSymbol>
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
