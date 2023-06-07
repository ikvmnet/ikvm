using System.Collections.Concurrent;

using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler
{

    /// <summary>
    /// Represents a context which holds and provides resolution for multiple Java types.
    /// </summary>
    public abstract class JavaClassContext
    {

        readonly ConcurrentDictionary<JavaClassName, JavaClassInfo> classes = new();

        /// <summary>
        /// Resolves the specified <see cref="JavaClassName"/>.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public JavaClassInfo Resolve(JavaClassName className) => classes.GetOrAdd(className, ResolveCore);

        /// <summary>
        /// Implements the resolution of the specified <see cref="JavaClassName"/>.
        /// This method may be invoked multiple times simultaniously, and only one result will be utilized.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        protected abstract JavaClassInfo ResolveCore(JavaClassName className);

    }

}
