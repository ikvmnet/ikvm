namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides extension methods for working with a <see cref="ManagedType"/>.
    /// </summary>
    internal static class ManagedTypeExtensions
    {

        /// <summary>
        /// Gets the field, if any, on the type with the specified name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ManagedField? ResolveField(this ManagedType type, string name)
        {
            foreach (var f in type.Fields)
                if (f.Name == name)
                    return f;

            return null;
        }

    }

}
