using IKVM.ByteCode.Syntax;
using IKVM.Compiler.Managed.Reader;

namespace IKVM.Compiler.Type
{

    /// <summary>
    /// Describes a Java type sources from an existing managed type.
    /// </summary>
    /// <remarks>
    /// This replaces the existing CompiledTypeWrapper.
    /// </remarks>
    internal class JavaManagedType : JavaType
    {

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public JavaManagedType(JavaTypeContext context, ManagedType type) :
            base(context)
        {
            this.type = type;
        }

        public override JavaClassName Name => throw new System.NotImplementedException();

    }

}
