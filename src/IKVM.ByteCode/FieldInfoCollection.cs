using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Lazy init collection of fields.
    /// </summary>
    public sealed class FieldInfoCollection : IReadOnlyList<FieldInfo>, IReadOnlyDictionary<string, FieldInfo>, IEnumerable<FieldInfo>
    {

        readonly Class clazz;
        readonly FieldInfoRecord[] records;
        FieldInfo[] cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="records"></param>
        internal FieldInfoCollection(Class clazz, FieldInfoRecord[] records)
        {
            this.clazz = clazz ?? throw new ArgumentNullException(nameof(clazz));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <summary>
        /// Resolves the specified method of the class from the records.
        /// </summary>
        /// <returns></returns>
        FieldInfo ResolveFieldInfo(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (cache == null)
                Interlocked.CompareExchange(ref cache, new FieldInfo[records.Length], null);

            // consult cache
            if (cache[index] is FieldInfo method)
                return method;

            method = new FieldInfo(clazz, records[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref cache[index], method, null);
            return cache[index];
        }

        /// <summary>
        /// Gets the field at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FieldInfo this[int index] => ResolveFieldInfo(index);

        /// <summary>
        /// Gets the mefield with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FieldInfo this[string name] => Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveFieldInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the count of fields.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets the names of the fields.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, FieldInfo>.Keys => Enumerable.Range(0, records.Length).Select(ResolveFieldInfo).Select(i => i.Name);

        /// <summary>
        /// Gets all of the field values.
        /// </summary>
        IEnumerable<FieldInfo> IReadOnlyDictionary<string, FieldInfo>.Values => Enumerable.Range(0, records.Length).Select(ResolveFieldInfo);

        /// <summary>
        /// Gets an enumerator over each field.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<FieldInfo> GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveFieldInfo).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each field.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns <c>true</c> if an field with the specified name exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IReadOnlyDictionary<string, FieldInfo>.ContainsKey(string key) => Enumerable.Range(0, records.Length).Select(ResolveFieldInfo).Any(i => i.Name == key);

        /// <summary>
        /// Attempts to get the field with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out FieldInfo value)
        {
            value = Enumerable.Range(0, records.Length).Select(i => new { Index = i, Info = ResolveFieldInfo(i) }).Where(i => i.Info.Name == name).Select(i => i.Info).FirstOrDefault();
            return value != null;
        }

        /// <summary>
        /// Gets the list of the items in the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<string, FieldInfo>> IEnumerable<KeyValuePair<string, FieldInfo>>.GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveFieldInfo).Select(i => new KeyValuePair<string, FieldInfo>(i.Name, i)).GetEnumerator();

    }

}
