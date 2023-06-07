using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler
{

    /// <summary>
    /// Describes a Java class.
    /// </summary>
    public abstract class JavaClassInfo
    {

        readonly JavaClassContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected JavaClassInfo(JavaClassContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the <see cref="JavaClassContext"/> that owns this class handle.
        /// </summary>
        public JavaClassContext Context => context;

        /// <summary>
        /// Gets the Java class name.
        /// </summary>
        public abstract JavaClassName Name { get; }

    }

}
