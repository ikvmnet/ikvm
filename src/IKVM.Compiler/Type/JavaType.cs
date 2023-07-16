using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler.Type
{

    /// <summary>
    /// Describes a type as viewed from Java.
    /// </summary>
    /// <remarks>
    /// This replaces the existing TypeWrapper.
    /// </remarks>
    internal abstract class JavaType
    {

        /// <summary>
        /// Gets the signature of the type.
        /// </summary>
        public abstract JavaTypeSignature Signature { get; }

    }

}
