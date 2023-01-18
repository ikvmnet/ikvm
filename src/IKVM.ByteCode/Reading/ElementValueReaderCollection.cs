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
    public sealed class ElementValueReaderCollection : IReadOnlyList<ElementValueReader>
    {

        readonly ClassReader ownerClass;
        readonly ElementValueRecord[] records;
        ElementValueReader[] readers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ownerClass"></param>
        /// <param name="records"></param>
        public ElementValueReaderCollection(ClassReader ownerClass, ElementValueRecord[] records)
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
            Interlocked.CompareExchange(ref readers[index], ResolveRecord(records[index]), null);
            return readers[index];
        }

        /// <summary>
        /// Resolves the given element value record to a reader type.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        ElementValueReader ResolveRecord(ElementValueRecord record) => record switch
        {
            ElementAnnotationValueRecord r => new ElementAnnotationValueReader(ownerClass, r),
            ElementArrayValueRecord r => new ElementArrayValueReader(ownerClass, r),
            ElementClassInfoValueRecord r => new ElementClassInfoValueReader(ownerClass, r),
            ElementConstantValueRecord r => new ElementConstantValueReader(ownerClass, r),
            ElementEnumConstantValueRecord r => new ElementEnumConstantValueReader(ownerClass, r),
            _ => throw new ByteCodeException("Cannot resolve element value reader."),
        };

        /// <summary>
        /// Gets the element value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ElementValueReader this[int index] => ResolveValue(index);

        /// <summary>
        /// Gets the count of element values.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets an enumerator over each element value.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ElementValueReader> GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveValue).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each attribute.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
