using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of annotation data.
    /// </summary>
    internal sealed class ParameterAnnotationReaderCollection : IReadOnlyList<ParameterAnnotationReader>
    {

        readonly ClassReader ownerClass;
        readonly ParameterAnnotationRecord[] records;
        ParameterAnnotationReader[] cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ownerClass"></param>
        /// <param name="records"></param>
        public ParameterAnnotationReaderCollection(ClassReader ownerClass, ParameterAnnotationRecord[] records)
        {
            this.ownerClass = ownerClass ?? throw new ArgumentNullException(nameof(ownerClass));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }
        /// <summary>
        /// Resolves the specified annotation of the class from the records.
        /// </summary>
        /// <returns></returns>
        ParameterAnnotationReader ResolveAnnotation(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (cache == null)
                Interlocked.CompareExchange(ref cache, new ParameterAnnotationReader[records.Length], null);

            // consult cache
            if (cache[index] is ParameterAnnotationReader reader)
                return reader;

            reader = new ParameterAnnotationReader(ownerClass, records[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref cache[index], reader, null);
            return cache[index];
        }

        /// <summary>
        /// Gets the annotation at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ParameterAnnotationReader this[int index] => ResolveAnnotation(index);

        /// <summary>
        /// Gets the count of annotations.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets an enumerator over each annotation.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ParameterAnnotationReader> GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveAnnotation).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each annotation.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
