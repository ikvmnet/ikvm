namespace IKVM.CoreLib.Symbols.Emit
{

    /// <summary>
    /// Represents a local variable within a method or constructor.
    /// </summary>
    interface ILocalBuilder
    {

        /// <summary>
        /// Gets a value indicating whether the object referred to by the local variable is pinned in memory.
        /// </summary>
        bool IsPinned { get; }

        /// <summary>
        /// Gets the zero-based index of the local variable within the method body.
        /// </summary>
        int LocalIndex { get; }

        /// <summary>
        /// Gets the type of the local variable.
        /// </summary>
        TypeSymbol LocalType { get; }

        /// <summary>
        /// Sets the name of this local variable.
        /// </summary>
        /// <param name="name"></param>
        void SetLocalSymInfo(string name);

        /// <summary>
        /// Sets the name and lexical scope of this local variable.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startOffset"></param>
        /// <param name="endOffset"></param>
        void SetLocalSymInfo(string name, int startOffset, int endOffset);


    }

}