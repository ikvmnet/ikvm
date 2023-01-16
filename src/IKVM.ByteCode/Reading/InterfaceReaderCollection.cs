using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of method data.
    /// </summary>
    public sealed class InterfaceReaderCollection : IReadOnlyList<InterfaceReader>, IReadOnlyDictionary<string, InterfaceReader>, IEnumerable<InterfaceReader>
    {

        readonly ClassReader ownerClass;
        readonly InterfaceInfoRecord[] records;
        InterfaceReader[] cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ownerClass"></param>
        /// <param name="records"></param>
        internal InterfaceReaderCollection(ClassReader ownerClass, InterfaceInfoRecord[] records)
        {
            this.ownerClass = ownerClass ?? throw new ArgumentNullException(nameof(ownerClass));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <summary>
        /// Resolves the specified interface of the class from the records.
        /// </summary>
        /// <returns></returns>
        InterfaceReader ResolveInterfaceInfo(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (cache == null)
                Interlocked.CompareExchange(ref cache, new InterfaceReader[records.Length], null);

            // consult cache
            if (cache[index] is InterfaceReader iface)
                return iface;

            iface = new InterfaceReader(ownerClass, records[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref cache[index], iface, null);
            return cache[index];
        }

        /// <summary>
        /// Gets the interface at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InterfaceReader this[int index] => ResolveInterfaceInfo(index);

        /// <summary>
        /// Gets the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public InterfaceReader this[string name] => Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveInterfaceInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the count of interfaces.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets the names of the interfaces.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, InterfaceReader>.Keys => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).Select(i => i.Name);

        /// <summary>
        /// Gets all of the interface values.
        /// </summary>
        IEnumerable<InterfaceReader> IReadOnlyDictionary<string, InterfaceReader>.Values => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo);

        /// <summary>
        /// Gets an enumerator over each interface.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<InterfaceReader> GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each interface.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns <c>true</c> if an interface with the specified name exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IReadOnlyDictionary<string, InterfaceReader>.ContainsKey(string key) => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).Any(i => i.Name == key);

        /// <summary>
        /// Attempts to get the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out InterfaceReader value)
        {
            value = Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveInterfaceInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault();
            return value != null;
        }

        /// <summary>
        /// Gets the list of the interfaces in the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<string, InterfaceReader>> IEnumerable<KeyValuePair<string, InterfaceReader>>.GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).Select(i => new KeyValuePair<string, InterfaceReader>(i.Name, i)).GetEnumerator();

    }

}
