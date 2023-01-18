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
        readonly ConstantOverride[] overrides;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        /// <param name="overrides"></param>
        internal ConstantReaderCollection(ClassReader declaringClass, ConstantRecord[] records, ConstantOverride[] overrides = null) :
            base(declaringClass, records, 1)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.overrides = overrides;
        }

        /// <summary>
        /// Creates a new reader for the given record.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override IConstantReader CreateReader(int index, ConstantRecord record)
        {
            return record is not null ? ConstantReader.Read(declaringClass, record, overrides != null && overrides.Length >= index ? overrides[index] : null) : null;
        }

    }

}
