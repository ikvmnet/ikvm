using System;

namespace IKVM.Compiler.Managed.Reflection
{

    internal abstract class ReflectionEntityInfo
    {

        readonly ReflectionContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected ReflectionEntityInfo(ReflectionContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the context that resulted in the loading of the entity.
        /// </summary>
        public ReflectionContext Context => context;

    }

}
