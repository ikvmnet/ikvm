namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Thrown when an exception occurs while resolving a managed entity.
    /// </summary>
    internal class ManagedResolveException : ManagedException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ManagedResolveException(string? message) :
            base(message)
        {

        }

    }

}
