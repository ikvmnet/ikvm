using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler.Type
{

    /// <summary>
    /// Describes a class as viewed from Java.
    /// </summary>
    /// <remarks>
    /// This replaces the existing TypeWrapper.
    /// </remarks>
    internal abstract class JavaClassType : JavaType
    {

        readonly JavaTypeContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected JavaClassType(JavaTypeContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the contexxt that owns this class.
        /// </summary>
        public JavaTypeContext Context => context;

        /// <summary>
        /// Gets the Java class name.
        /// </summary>
        public abstract JavaClassName Name { get; }

        /// <summary>
        /// Gets the fields associated with this class.
        /// </summary>
        public abstract JavaField Fields { get; }

        /// <summary>
        /// Gets the method associated with this class.
        /// </summary>
        public abstract JavaMethod Methods { get;  }

    }

}
