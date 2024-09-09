namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents the different kinds of type parameters.
    /// </summary>
    enum TypeParameterKind
    {

        /// <summary>
        /// Type parameter of a named type. For example: <c>T</c> in <c><![CDATA[List<T>]]></c>.
        /// </summary>
        Type = 0,

        /// <summary>
        /// Type parameter of a method. For example: <c>T</c> in <c><![CDATA[void M<T>()]]></c>.
        /// </summary>
        Method = 1,

        /// <summary>
        /// Type parameter in a <c>cref</c> attribute in XML documentation comments. For example: <c>T</c> in <c><![CDATA[<see cref="List{T}"/>]]></c>.
        /// </summary>
        Cref = 2,

    }

}
