namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference of a type signature.
    /// </summary>
    public class ManagedByRefTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature refType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="refType"></param>
        public ManagedByRefTypeSignature(ManagedTypeSignature refType)
        {
            this.refType = refType;
        }

        /// <summary>
        /// Gets the type refered to as a reference.
        /// </summary>
        public ManagedTypeSignature RefType => refType;

    }

}
