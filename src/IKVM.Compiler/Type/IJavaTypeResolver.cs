namespace IKVM.Compiler.Type
{

    /// <summary>
    /// Provides a mechanism for a <see cref="JavaTypeContext"/> to resolve external <see cref="JavaType"/> instances.
    /// </summary>
    internal interface IJavaTypeResolver
    {

        /// <summary>
        /// Attempts to resolve a <see cref="JavaType"/> for the given type name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        JavaType? Resolve(string typeName);

    }

}
