using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal abstract class ElementValueReader : ReaderBase
    {

        /// <summary>
        /// Resolves the given element value record to a reader type.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static ElementValueReader Resolve(ClassReader declaringClass, ElementValueRecord record) => record switch
        {
            ElementAnnotationValueRecord r => new ElementAnnotationValueReader(declaringClass, r),
            ElementArrayValueRecord r => new ElementArrayValueReader(declaringClass, r),
            ElementClassInfoValueRecord r => new ElementClassInfoValueReader(declaringClass, r),
            ElementConstantValueRecord r => new ElementConstantValueReader(declaringClass, r),
            ElementEnumConstantValueRecord r => new ElementEnumConstantValueReader(declaringClass, r),
            _ => throw new ByteCodeException("Cannot resolve element value reader."),
        };

        readonly ElementValueRecord record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ElementValueReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the underlying record.
        /// </summary>
        public ElementValueRecord Record => record;

    }

    internal abstract class ElementValueReader<TRecord> : ElementValueReader
        where TRecord : ElementValueRecord
    {

        readonly TRecord record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ElementValueReader(ClassReader declaringClass, TRecord record) :
            base(declaringClass, record)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the underlying record.
        /// </summary>
        public new TRecord Record => record;

    }

}
