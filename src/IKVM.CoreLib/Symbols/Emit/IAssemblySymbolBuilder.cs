namespace IKVM.CoreLib.Symbols.Emit
{

    interface IAssemblySymbolBuilder : ISymbolBuilder<IAssemblySymbol>
    {

        /// <summary>
        /// Defines a named module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IModuleSymbolBuilder DefineModule(string name);

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
