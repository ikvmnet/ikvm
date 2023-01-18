using System;
using System.Threading;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Base class for a reader.
    /// </summary>
    public abstract class ReaderBase
    {

        /// <summary>
        /// Gets the value at the given location or initializes it if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        internal static T LazyGet<T>(ref T location, Func<T> create)
            where T : class
        {
            if (create is null)
                throw new ArgumentNullException(nameof(create));

            if (location == null)
                Interlocked.CompareExchange(ref location, create(), null);

            return location;
        }

        readonly ClassReader declaringClass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ReaderBase(ClassReader declaringClass)
        {
            this.declaringClass = declaringClass ?? (this is ClassReader self ? self : throw new ArgumentNullException(nameof(declaringClass)));
        }

        /// <summary>
        /// Gets the class reader from which this entity is being read.
        /// </summary>
        public ClassReader DeclaringClass => declaringClass;

    }

    /// <summary>
    /// Base class for a reader of a specific record type.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class ReaderBase<TRecord> : ReaderBase
    {

        readonly TRecord record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReaderBase(ClassReader declaringClass, TRecord record) :
            base(declaringClass)
        {
            this.record = record;
        }

        /// <summary>
        /// Gets the underlying method being read.
        /// </summary>
        public TRecord Record => record;

    }
}
