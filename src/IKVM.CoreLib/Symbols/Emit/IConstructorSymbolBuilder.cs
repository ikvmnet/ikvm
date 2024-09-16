using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    interface IConstructorSymbolBuilder : ISymbolBuilder<IConstructorSymbol>
    {

        /// <summary>
        /// Sets the method implementation flags for this constructor.
        /// </summary>
        /// <param name="attributes"></param>
        void SetImplementationFlags(MethodImplAttributes attributes);

        /// <summary>
        /// Defines a parameter of this constructor.
        /// </summary>
        /// <param name="iSequence"></param>
        /// <param name="attributes"></param>
        /// <param name="strParamName"></param>
        /// <returns></returns>
        IParameterSymbolBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName);

        /// <summary>
        /// Gets an ILGenerator object, with the specified MSIL stream size, that can be used to build a method body for this constructor.
        /// </summary>
        /// <param name="streamSize"></param>
        /// <returns></returns>
        IILGenerator GetILGenerator(int streamSize);

        /// <summary>
        /// Gets an ILGenerator for this constructor.
        /// </summary>
        /// <returns></returns>
        IILGenerator GetILGenerator();

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