namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a managed method.
    /// </summary>
    public readonly struct ManagedMethodRef
    {

        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="managedType"></param>
        public ManagedMethodRef(int index)
        {
            this.index = index;
        }

        /// <summary>
        /// Gets the index of the method on the type.
        /// </summary>
        public int Index => index;

    }

}
