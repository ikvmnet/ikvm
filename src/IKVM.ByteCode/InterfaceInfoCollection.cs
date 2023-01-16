using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Lazy init collection of method data.
    /// </summary>
    public sealed class InterfaceInfoCollection : IReadOnlyList<InterfaceInfo>, IReadOnlyDictionary<string, InterfaceInfo>, IEnumerable<InterfaceInfo>
    {

        readonly Class clazz;
        readonly InterfaceInfoRecord[] records;
        InterfaceInfo[] cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="records"></param>
        internal InterfaceInfoCollection(Class clazz, InterfaceInfoRecord[] records)
        {
            this.clazz = clazz ?? throw new ArgumentNullException(nameof(clazz));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <summary>
        /// Resolves the specified interface of the class from the records.
        /// </summary>
        /// <returns></returns>
        InterfaceInfo ResolveInterfaceInfo(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (cache == null)
                Interlocked.CompareExchange(ref cache, new InterfaceInfo[records.Length], null);

            // consult cache
            if (cache[index] is InterfaceInfo iface)
                return iface;

            iface = new InterfaceInfo(clazz, records[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref cache[index], iface, null);
            return cache[index];
        }

        /// <summary>
        /// Gets the interface at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InterfaceInfo this[int index] => ResolveInterfaceInfo(index);

        /// <summary>
        /// Gets the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public InterfaceInfo this[string name] => Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveInterfaceInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the count of interfaces.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets the names of the interfaces.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, InterfaceInfo>.Keys => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).Select(i => i.Name);

        /// <summary>
        /// Gets all of the interface values.
        /// </summary>
        IEnumerable<InterfaceInfo> IReadOnlyDictionary<string, InterfaceInfo>.Values => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo);

        /// <summary>
        /// Gets an enumerator over each interface.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<InterfaceInfo> GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).GetEnumerator();

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
        bool IReadOnlyDictionary<string, InterfaceInfo>.ContainsKey(string key) => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).Any(i => i.Name == key);

        /// <summary>
        /// Attempts to get the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out InterfaceInfo value)
        {
            value = Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveInterfaceInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault();
            return value != null;
        }

        /// <summary>
        /// Gets the list of the interfaces in the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<string, InterfaceInfo>> IEnumerable<KeyValuePair<string, InterfaceInfo>>.GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveInterfaceInfo).Select(i => new KeyValuePair<string, InterfaceInfo>(i.Name, i)).GetEnumerator();

    }

}
