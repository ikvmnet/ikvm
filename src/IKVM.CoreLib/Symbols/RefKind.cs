namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Denotes the kind of reference.
    /// </summary>
    enum RefKind : byte
    {

        /// <summary>
        /// Indicates a "value" parameter or return type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates a "ref" parameter or return type.
        /// </summary>
        Ref = 1,

        /// <summary>
        /// Indicates an "out" parameter.
        /// </summary>
        Out = 2,

        /// <summary>
        /// Indicates an "in" parameter.
        /// </summary>
        In = 3,

        /// <summary>
        /// Indicates a "ref readonly" return type.
        /// </summary>
        RefReadOnly = 3,

        /// <summary>
        /// Indicates a "ref readonly" parameter.
        /// </summary>
        RefReadOnlyParameter = 4,

    }

}
