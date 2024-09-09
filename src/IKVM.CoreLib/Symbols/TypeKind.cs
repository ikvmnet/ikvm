namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Enumeration for possible kinds of type symbols.
    /// </summary>
    enum TypeKind : byte
    {

        /// <summary>
        /// Type's kind is undefined.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Type is an array type.
        /// </summary>
        Array = 1,

        /// <summary>
        /// Type is a class.
        /// </summary>
        Class = 2,

        /// <summary>
        /// Type is a delegate.
        /// </summary>
        Delegate = 3,

        /// <summary>
        /// Type is an enumeration.
        /// </summary>
        Enum = 5,

        /// <summary>
        /// Type is an error type.
        /// </summary>
        Error = 6,

        /// <summary>
        /// Type is an interface.
        /// </summary>
        Interface = 7,

        /// <summary>
        /// Type is a module.
        /// </summary>
        Module = 8,

        /// <summary>
        /// Type is a pointer.
        /// </summary>
        Pointer = 9,

        /// <summary>
        /// Type is a C# struct or VB Structure
        /// </summary>
        Struct = 10,

        /// <summary>
        /// Type is a C# struct or VB Structure
        /// </summary>
        Structure = 10,

        /// <summary>
        /// Type is a type parameter.
        /// </summary>
        TypeParameter = 11,

        /// <summary>
        /// Type is a function pointer.
        /// </summary>
        FunctionPointer = 13,

    }

}
