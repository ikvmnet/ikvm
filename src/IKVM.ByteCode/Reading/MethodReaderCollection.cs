using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of methods.
    /// </summary>
    public sealed class MethodReaderCollection : IReadOnlyList<MethodReader>, IReadOnlyDictionary<string, MethodReader>, IEnumerable<MethodReader>
    {

        readonly ClassReader ownerClass;
        readonly MethodInfoRecord[] records;
        MethodReader[] cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ownerClass"></param>
        /// <param name="records"></param>
        internal MethodReaderCollection(ClassReader ownerClass, MethodInfoRecord[] records)
        {
            this.ownerClass = ownerClass ?? throw new ArgumentNullException(nameof(ownerClass));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <summary>
        /// Resolves the specified method of the class from the records.
        /// </summary>
        /// <returns></returns>
        MethodReader ResolveMethodInfo(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (cache == null)
                Interlocked.CompareExchange(ref cache, new MethodReader[records.Length], null);

            // consult cache
            if (cache[index] is MethodReader method)
                return method;

            method = new MethodReader(ownerClass, records[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref cache[index], method, null);
            return cache[index];
        }

        /// <summary>
        /// Gets the method at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MethodReader this[int index] => ResolveMethodInfo(index);

        /// <summary>
        /// Gets the method with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MethodReader this[string name] => Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveMethodInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the count of methods.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets the names of the methods.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, MethodReader>.Keys => Enumerable.Range(0, records.Length).Select(ResolveMethodInfo).Select(i => i.Name);

        /// <summary>
        /// Gets all of the methods values.
        /// </summary>
        IEnumerable<MethodReader> IReadOnlyDictionary<string, MethodReader>.Values => Enumerable.Range(0, records.Length).Select(ResolveMethodInfo);

        /// <summary>
        /// Gets an enumerator over each method.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MethodReader> GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveMethodInfo).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each method.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns <c>true</c> if an method with the specified name exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IReadOnlyDictionary<string, MethodReader>.ContainsKey(string key) => Enumerable.Range(0, records.Length).Select(ResolveMethodInfo).Any(i => i.Name == key);

        /// <summary>
        /// Attempts to get the method with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out MethodReader value)
        {
            value = Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveMethodInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault();
            return value != null;
        }

        /// <summary>
        /// Gets the list of the items in the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<string, MethodReader>> IEnumerable<KeyValuePair<string, MethodReader>>.GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveMethodInfo).Select(i => new KeyValuePair<string, MethodReader>(i.Name, i)).GetEnumerator();

    }

}
