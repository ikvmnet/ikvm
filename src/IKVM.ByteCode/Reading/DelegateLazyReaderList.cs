using System;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Concrete lazy list implementation that creates readers lazily based on a delegate.
    /// </summary>
    /// <typeparam name="TReader"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    internal sealed class DelegateLazyReaderList<TReader, TSource> : LazyReaderList<TReader, TSource>
        where TReader : class
    {

        readonly Func<int, TSource, TReader> create;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="sources"></param>
        /// <param name="create"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateLazyReaderList(ClassReader declaringClass, TSource[] sources, Func<int, TSource, TReader> create) :
            base(declaringClass, sources)
        {
            this.create = create ?? throw new ArgumentNullException(nameof(create));
        }

        /// <summary>
        /// Creates the new reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override TReader CreateReader(int index, TSource source) => create(index, source);

    }

}
