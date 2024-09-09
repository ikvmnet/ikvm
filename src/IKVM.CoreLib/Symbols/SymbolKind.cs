namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Specifies the possible kinds of symbols.
    /// </summary>
    public enum SymbolKind
    {

        /// <summary>
        /// Symbol is an array type.
        /// </summary>
        ArrayType = 1,

        /// <summary>
        /// Symbol is an assembly.
        /// </summary>
        Assembly = 2,

        /// <summary>
        /// Symbol that represents an error 
        /// </summary>
        ErrorType = 4,

        /// <summary>
        /// Symbol is an Event.
        /// </summary>
        Event = 5,

        /// <summary>
        /// Symbol is a field.
        /// </summary>
        Field = 6,

        /// <summary>
        /// Symbol is a label.
        /// </summary>
        Label = 7,

        /// <summary>
        /// Symbol is a local.
        /// </summary>
        Local = 8,

        /// <summary>
        /// Symbol is a method.
        /// </summary>
        Method = 9,

        /// <summary>
        /// Symbol is a netmodule.
        /// </summary>
        NetModule = 10,

        /// <summary>
        /// Symbol is a named type (e.g. class).
        /// </summary>
        NamedType = 11,

        /// <summary>
        /// Symbol is a namespace.
        /// </summary>
        Namespace = 12,

        /// <summary>
        /// Symbol is a parameter.
        /// </summary>
        Parameter = 13,

        /// <summary>
        /// Symbol is a pointer type.
        /// </summary>
        PointerType = 14,

        /// <summary>
        /// Symbol is a property.
        /// </summary>
        Property = 15,

        /// <summary>
        /// Symbol is a type parameter.
        /// </summary>
        TypeParameter = 17,

        /// <summary>
        /// Symbol represents a function pointer type
        /// </summary>
        FunctionPointerType = 20,

    }

}
