using System.Collections.Generic;

namespace IKVM.ByteCode
{

    public sealed class CodeAttributeData : AttributeData<CodeAttributeDataRecord>
    {

        AttributeDataCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal CodeAttributeData(Class declaringClass, AttributeInfo info, CodeAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

        public ushort MaxStack => Data.MaxStack;

        public ushort MaxLocals => Data.MaxLocals;

        /// <summary>
        /// Gets the byte code.
        /// </summary>
        public byte[] Code => Data.Code;

        public IReadOnlyList<ExceptionHandlerRecord> ExceptionTable => Data.ExceptionTable;

        /// <summary>
        /// Gets the set of attributes applied to this attribute.
        /// </summary>
        public AttributeDataCollection Attributes => Class.LazyGet(ref attributes, () => new AttributeDataCollection(DeclaringClass, Data.Attributes));

    }

}
