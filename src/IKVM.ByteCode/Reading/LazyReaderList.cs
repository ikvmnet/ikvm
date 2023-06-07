using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Base implementation of a list that generates reader instances on demand.
    /// </summary>
    /// <typeparam name="TReader"></typeparam>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class LazyReaderList<TReader, TRecord> : IReadOnlyList<TReader>
        where TReader : class
    {

        readonly ClassReader declaringClass;
        readonly TRecord[] records;
        readonly uint minIndex;

        TReader[] readers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        protected LazyReaderList(ClassReader declaringClass, TRecord[] records, uint minIndex = 0)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
            this.minIndex = minIndex;
        }

        /// <summary>
        /// Gets the underlying records that make up the list.
        /// </summary>
        protected IReadOnlyList<TRecord> Records => records;

        /// <summary>
        /// Creates the appropriate reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected abstract TReader CreateReader(int index, TRecord record);

        /// <summary>
        /// Resolves the specified reader at the given index.
        /// </summary>
        /// <returns></returns>
        TReader ResolveReader(int index)
        {
            if (index < minIndex || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (readers == null)
                Interlocked.CompareExchange(ref readers, new TReader[records.Length], null);

            // consult cache
            if (readers[index] is TReader reader)
                return reader;

            // atomic set, only one winner
            Interlocked.CompareExchange(ref readers[index], CreateReader(index, records[index]), null);
            return readers[index];
        }

        /// <summary>
        /// Gets the class that declared this list.
        /// </summary>
        public ClassReader DeclaringClass => declaringClass;

        /// <summary>
        /// Gets the reader at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TReader this[int index] => ResolveReader(index);

        /// <summary>
        /// Gets the count of readers.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets an enumerator over each reader.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TReader> GetEnumerator() => Enumerable.Range(0, records.Length).Select(i => this[i]).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each reader.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
