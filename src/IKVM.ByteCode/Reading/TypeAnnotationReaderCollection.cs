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
    public sealed class TypeAnnotationReaderCollection : IReadOnlyList<TypeAnnotationReader>
    {

        readonly ClassReader ownerClass;
        readonly TypeAnnotationRecord[] records;

        TypeAnnotationReader[] cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ownerClass"></param>
        /// <param name="records"></param>
        public TypeAnnotationReaderCollection(ClassReader ownerClass, TypeAnnotationRecord[] records)
        {
            this.ownerClass = ownerClass ?? throw new ArgumentNullException(nameof(ownerClass));
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }
        /// <summary>
        /// Resolves the specified annotation of the class from the records.
        /// </summary>
        /// <returns></returns>
        TypeAnnotationReader ResolveAnnotation(int index)
        {
            if (index < 0 || index >= records.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (cache == null)
                Interlocked.CompareExchange(ref cache, new TypeAnnotationReader[records.Length], null);

            // consult cache
            if (cache[index] is TypeAnnotationReader reader)
                return reader;

            reader = new TypeAnnotationReader(ownerClass, records[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref cache[index], reader, null);
            return cache[index];
        }

        /// <summary>
        /// Gets the annotation at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TypeAnnotationReader this[int index] => ResolveAnnotation(index);

        /// <summary>
        /// Gets the count of annotations.
        /// </summary>
        public int Count => records.Length;

        /// <summary>
        /// Gets an enumerator over each annotation.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TypeAnnotationReader> GetEnumerator() => Enumerable.Range(0, records.Length).Select(ResolveAnnotation).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each annotation.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
