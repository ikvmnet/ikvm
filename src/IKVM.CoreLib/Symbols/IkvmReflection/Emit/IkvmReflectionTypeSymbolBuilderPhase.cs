namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{
    public enum IkvmReflectionTypeSymbolBuilderPhase
    {

        /// <summary>
        /// Default phase.
        /// </summary>
        Default,

        /// <summary>
        /// TypeBuilder has been created.
        /// </summary>
        Created,

        /// <summary>
        /// Symbol declarations have been emitted (Field, Methods, Properties, Events).
        /// </summary>
        Declarations,

        /// <summary>
        /// Symbol implementations have been emitted (method bodies, overloads, etc).
        /// </summary>
        Implementations,

        /// <summary>
        /// Type instance has been created.
        /// </summary>
        Complete,

    }

}
