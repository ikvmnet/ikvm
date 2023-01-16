using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public abstract class AttributeReader
    {

        readonly ClassReader declaringClass;
        readonly AttributeInfoReader info;
        readonly AttributeRecord record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal AttributeReader(ClassReader declaringClass, AttributeInfoReader info, AttributeRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.info = info ?? throw new ArgumentNullException(nameof(info));
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the owning class of the attribute.
        /// </summary>
        protected ClassReader DeclaringClass => declaringClass;

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        public string Name => info.Name;

        /// <summary>
        /// Gets the underlying data record of the attribute.
        /// </summary>
        protected AttributeRecord Data => record;

    }

    public abstract class AttributeData<TRecord> : AttributeReader
        where TRecord : AttributeRecord
    {

        readonly TRecord data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal AttributeData(ClassReader declaringClass, AttributeInfoReader info, TRecord data) :
            base(declaringClass, info, data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Gets the underlying data record of the attribute.
        /// </summary>
        protected new TRecord Data => data;

    }

}
