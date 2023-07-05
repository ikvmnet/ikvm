namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides an interface for a managed type to call back into its loader.
    /// </summary>
    public interface IManagedTypeContext
    {

        /// <summary>
        /// Invoked when the first access to a managed type occurs.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ManagedTypeData LoadType(ManagedType type);

    }

}
