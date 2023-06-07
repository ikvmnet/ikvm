using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public abstract class AttributeReader : ReaderBase
    {

        readonly AttributeInfoReader info;
        readonly AttributeRecord record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal AttributeReader(ClassReader declaringClass, AttributeInfoReader info, AttributeRecord record) :
            base(declaringClass)
        {
            this.info = info ?? throw new ArgumentNullException(nameof(info));
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the information about the attribute.
        /// </summary>
        public AttributeInfoReader Info => info;

        /// <summary>
        /// Gets the underlying record of the attribute.
        /// </summary>
        public AttributeRecord Record => record;

    }

    public abstract class AttributeReader<TRecord> : AttributeReader
        where TRecord : AttributeRecord
    {

        readonly TRecord record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal AttributeReader(ClassReader declaringClass, AttributeInfoReader info, TRecord record) :
            base(declaringClass, info, record)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the underlying data record of the attribute.
        /// </summary>
        public new TRecord Record => record;

    }

}
