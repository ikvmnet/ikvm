namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Enumeration for kinds of scoped modifiers.
    /// </summary>
    enum ScopedKind : byte
    {
        /// <summary>
        /// Not scoped.
        /// </summary>
        None = 0,

        /// <summary>
        /// A ref scoped to the enclosing block or method.
        /// </summary>
        ScopedRef = 1,

        /// <summary>
        /// A value scoped to the enclosing block or method.
        /// </summary>
        ScopedValue = 2,

    }

}
