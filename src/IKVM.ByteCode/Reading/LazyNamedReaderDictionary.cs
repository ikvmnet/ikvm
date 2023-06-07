using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Base implementation of a dictionary that generates reader instances on demand.
    /// </summary>
    /// <typeparam name="TReader"></typeparam>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class LazyNamedReaderDictionary<TReader, TRecord> : IReadOnlyDictionary<string, TReader>
        where TReader : class
    {

        readonly ClassReader declaringClass;
        readonly TRecord[] records;
        readonly int minIndex;

        TReader[] readers;
        string[] names;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        protected LazyNamedReaderDictionary(ClassReader declaringClass, TRecord[] records, int minIndex = 0)
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
        /// Gets the name for the given record.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected abstract string GetName(int index, TRecord record);

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
        /// Resolves the specified name at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        string ResolveName(int index)
        {
            if (index < minIndex || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (names == null)
                Interlocked.CompareExchange(ref names, new string[records.Length], null);

            // consult cache
            if (names[index] is string name)
                return name;

            // atomic set, only one winner
            Interlocked.CompareExchange(ref names[index], GetName(index, records[index]), null);
            return names[index];
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
        /// Gets the reader with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TReader this[string name] => Enumerable.Range(minIndex, records.Length).Where(i => ResolveName(i) == name).Select(ResolveReader).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the count of readers.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Attempts to get the value at the specified index, or returns the default value if out of range.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(int index, out TReader value)
        {
            value = default;

            if (index < minIndex || index >= records.Length)
                return false;

            value = this[index];
            return true;
        }

        /// <summary>
        /// Attempts to get the value at the specified index, or returns the default value if out of range.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string name, out TReader value) => (value = Enumerable.Range(minIndex, records.Length).Where(i => ResolveName(i) == name).Select(ResolveReader).FirstOrDefault()) != null;

        /// <summary>
        /// Returns <c>true</c> if the collection contains a reader with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) => Enumerable.Range(minIndex, records.Length).Any(i => ResolveName(i) == name);

        /// <summary>
        /// Gets an enumerator over each reader.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TReader> GetEnumerator() => Enumerable.Range(minIndex, records.Length).Select(i => this[i]).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each reader.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each reader.
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<string, TReader>> IEnumerable<KeyValuePair<string, TReader>>.GetEnumerator() => Enumerable.Range(minIndex, records.Length).Select(i => new KeyValuePair<string, TReader>(ResolveName(i), ResolveReader(i))).GetEnumerator();

        /// <summary>
        /// Gets all of the available keys.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, TReader>.Keys => Enumerable.Range(minIndex, records.Length).Select(ResolveName);

        /// <summary>
        /// Gets all of the available values.
        /// </summary>
        IEnumerable<TReader> IReadOnlyDictionary<string, TReader>.Values => Enumerable.Range(minIndex, records.Length).Select(ResolveReader);

        /// <summary>
        /// Attempts to retrieve the reader with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IReadOnlyDictionary<string, TReader>.TryGetValue(string key, out TReader value) => TryGet(key, out value);

        /// <summary>
        /// Returns <c>true</c> if the collection contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IReadOnlyDictionary<string, TReader>.ContainsKey(string key) => Contains(key);

    }

}
