using System;

namespace IKVM.ByteCode.Writing
{
    /// <summary>
    /// Base class for a writer.
    /// </summary>
    internal abstract class WriterBase
    {
        private readonly ClassWriter declaringClass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected WriterBase(ClassWriter declaringClass)
        {
            this.declaringClass = declaringClass ?? (this is ClassWriter self ? self : throw new ArgumentNullException(nameof(declaringClass)));
        }

        /// <summary>
        /// Gets the class writer from which this entity is being written.
        /// </summary>
        public ClassWriter DeclaringClass => declaringClass;

    }

    /// <summary>
    /// Base class for a writer of a specific record type.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    internal abstract class WriterBase<TRecord> : WriterBase
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WriterBase(ClassWriter declaringClass) :
            base(declaringClass)
        {
        }

        /// <summary>
        /// Gets the underlying method being written.
        /// </summary>
        internal abstract TRecord Record { get; }
    }
}
