namespace IKVM.CoreLib.Symbols.Reflection
{

    public enum ReflectionSymbolState
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
        /// Complete state of an underlying object. A completed symbol has been fully completed.
        /// </summary>
        Completed,

    }

}
