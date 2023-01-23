using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of constants.
    /// </summary>
    internal sealed class ConstantReaderCollection : LazyReaderList<IConstantReader, ConstantRecord>
    {

        readonly ClassReader declaringClass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        /// <param name="overrides"></param>
        internal ConstantReaderCollection(ClassReader declaringClass, ConstantRecord[] records) :
            base(declaringClass, records, 1)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
        }

        /// <summary>
        /// Creates a new reader for the given record.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override IConstantReader CreateReader(int index, ConstantRecord record)
        {
            return record is not null ? ConstantReader.Read(declaringClass, (ushort)index, record) : null;
        }

        /// <summary>
        /// Attempts to get the constant reader at the specified index.
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <param name="index"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool TryGet<TReader>(int index, out TReader reader)
            where TReader : class, IConstantReader
        {
            if (base.TryGet(index, out var reader2))
            {
                if (reader2 is not TReader reader3)
                    throw new ByteCodeException($"Invalid constant resolution. Reader at index {index} is not a {typeof(TReader).Name}.");

                reader = reader3;
                return true;
            }

            reader = null;
            return false;
        }
    }

}
