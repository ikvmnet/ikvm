namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference of a type signature.
    /// </summary>
    public class ManagedPointerTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature pointerType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="pointerType"></param>
        public ManagedPointerTypeSignature(ManagedTypeSignature pointerType)
        {
            this.pointerType = pointerType;
        }

        /// <summary>
        /// Gets the type refered to by the pointer.
        /// </summary>
        public ManagedTypeSignature PointerType => pointerType;

    }

}
