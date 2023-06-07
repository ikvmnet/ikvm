using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of constants.
    /// </summary>
    public sealed class ConstantReaderCollection : LazyReaderList<IConstantReader, ConstantRecord>
    {

        readonly ClassReader declaringClass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        internal ConstantReaderCollection(ClassReader declaringClass, ConstantRecord[] records) :
            base(declaringClass, records, 0)
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
        public TReader Get<TReader>(int index)
            where TReader : class, IConstantReader
        {
            if (index == 0)
                return null;

            try
            {
                return this[index] as TReader ?? throw new ByteCodeException($"Invalid constant resolution. Reader at index {index} is not a {typeof(TReader).Name}.");
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ByteCodeException($"Invalid constant resolution. Reader at index {index} is not valid.", e);
            }
        }

    }

}
