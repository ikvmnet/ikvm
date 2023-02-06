using System;

namespace IKVM.ByteCode.Writing
{
    /// <summary>
    /// Base class for a writer.
    /// </summary>
    internal abstract class BuilderBase
    {
        private readonly ClassBuilder declaringClass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BuilderBase(ClassBuilder declaringClass)
        {
            this.declaringClass = declaringClass ?? (this is ClassBuilder self ? self : throw new ArgumentNullException(nameof(declaringClass)));
        }

        /// <summary>
        /// Gets the class writer from which this entity is being written.
        /// </summary>
        protected ClassBuilder DeclaringClass => declaringClass;
    }

    /// <summary>
    /// Base class for a writer of a specific record type.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    internal abstract class BuilderBase<TRecord> : BuilderBase
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BuilderBase(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        /// <summary>
        /// Gets the underlying method being written.
        /// </summary>
        public abstract TRecord Build();
    }
}
