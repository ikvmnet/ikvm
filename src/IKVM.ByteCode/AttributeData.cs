using System;

namespace IKVM.ByteCode
{

    public abstract class AttributeData
    {

        readonly Class declaringClass;
        readonly AttributeInfo info;
        readonly AttributeDataRecord data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal AttributeData(Class declaringClass, AttributeInfo info, AttributeDataRecord data)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.info = info ?? throw new ArgumentNullException(nameof(info));
            this.data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Gets the owning class of the attribute.
        /// </summary>
        protected Class DeclaringClass => declaringClass;

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        public string Name => info.Name;

        /// <summary>
        /// Gets the underlying data record of the attribute.
        /// </summary>
        protected AttributeDataRecord Data => data;

    }

    public abstract class AttributeData<TRecord> : AttributeData
        where TRecord : AttributeDataRecord
    {

        readonly TRecord data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal AttributeData(Class declaringClass, AttributeInfo info, TRecord data) :
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
