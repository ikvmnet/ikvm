namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    public enum IkvmReflectionSymbolState
    {

        /// <summary>
        /// Initial state of a symbol. No underlying object is available.
        /// </summary>
        Default,

        /// <summary>
        /// Initial state of an underlying object. Only basic properties have been initialized.
        /// </summary>
        Declared,

        /// <summary>
        /// Finished state of an underlying object. A finished symbol has had it's settings and children emitted.
        /// </summary>
        Finished,

        /// <summary>
        /// Export state of an underlying object. The symbol has been exported to its final IKVM reflection entity.
        /// </summary>
        Completed,

    }

}
