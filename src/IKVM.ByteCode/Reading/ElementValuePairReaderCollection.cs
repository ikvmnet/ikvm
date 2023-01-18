using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of attribute data.
    /// </summary>
    internal sealed class ElementValuePairReaderCollection : IReadOnlyDictionary<string, ElementValueReader>
    {

        readonly ClassReader ownerClass;
        readonly ElementValuePairRecord[] records;
        string[] names;
        ElementValueReader[] readers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ownerClass"></param>
        /// <param name="records"></param>
        public ElementValuePairReaderCollection(ClassReader ownerClass, ElementValuePairRecord[] records)
        {
            this.ownerClass = ownerClass ?? throw new ArgumentNullException(nameof(ownerClass));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <summary>
        /// Resolves the element value at the given index.
        /// </summary>
        /// <returns></returns>
        ElementValueReader ResolveValue(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (readers == null)
                Interlocked.CompareExchange(ref readers, new ElementValueReader[records.Length], null);

            // consult cache
            if (readers[index] is ElementValueReader attribute)
                return attribute;

            // atomic set, only one winner
            Interlocked.CompareExchange(ref readers[index], ElementValueReader.Resolve(ownerClass, records[index].Value), null);
            return readers[index];
        }

        /// <summary>
        /// Resolves the elmeent name at the given index.
        /// </summary>
        /// <returns></returns>
        string ResolveName(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (names == null)
                Interlocked.CompareExchange(ref names, new string[records.Length], null);

            // consult cache
            if (names[index] is string name)
                return name;

            // atomic set, only one winner
            Interlocked.CompareExchange(ref names[index], ownerClass.ResolveConstant<Utf8ConstantReader>(records[index].NameIndex).Value, null);
            return names[index];
        }

        /// <summary>
        /// Gets the element value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ElementValueReader this[int index] => ResolveValue(index);

        /// <summary>
        /// Gets the element value with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ElementValueReader this[string name] => Enumerable.Range(0, records.Length).Where(i => ResolveName(i) == name).Select(ResolveValue).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the count of element values.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets the names of the element values.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, ElementValueReader>.Keys => Enumerable.Range(0, records.Length).Select(ResolveName);

        /// <summary>
        /// Gets all of the element values.
        /// </summary>
        IEnumerable<ElementValueReader> IReadOnlyDictionary<string, ElementValueReader>.Values => Enumerable.Range(0, records.Length).Select(ResolveValue);

        /// <summary>
        /// Gets an enumerator over each element value.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, ElementValueReader>> GetEnumerator() => Enumerable.Range(0, records.Length).Select(i => new KeyValuePair<string, ElementValueReader>(ResolveName(i), ResolveValue(i))).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each attribute.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns <c>true</c> if an attribute with the specified name exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IReadOnlyDictionary<string, ElementValueReader>.ContainsKey(string key) => Enumerable.Range(0, records.Length).Select(ResolveName).Contains(key);

        /// <summary>
        /// Attempts to get the attribute with the specified name.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out ElementValueReader value)
        {
            value = Enumerable.Range(0, records.Length).Where(i => ResolveName(i) == key).Select(ResolveValue).FirstOrDefault();
            return value != null;
        }

    }

}
