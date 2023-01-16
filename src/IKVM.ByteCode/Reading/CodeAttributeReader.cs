using System.Collections.Generic;
using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class CodeAttributeReader : AttributeData<CodeAttributeRecord>
    {

        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal CodeAttributeReader(ClassReader declaringClass, AttributeInfoReader info, CodeAttributeRecord data) :
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
        public AttributeReaderCollection Attributes => ClassReader.LazyGet(ref attributes, () => new AttributeReaderCollection(DeclaringClass, Data.Attributes));

    }

}
