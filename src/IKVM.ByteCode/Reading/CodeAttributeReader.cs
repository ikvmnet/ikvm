using System;
using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class CodeAttributeReader : AttributeReader<CodeAttributeRecord>
    {

        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal CodeAttributeReader(ClassReader declaringClass, AttributeInfoReader info, CodeAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        public ushort MaxStack => Record.MaxStack;

        public ushort MaxLocals => Record.MaxLocals;

        /// <summary>
        /// Gets the byte code.
        /// </summary>
        public ReadOnlyMemory<byte> Code => Record.Code;

        public IReadOnlyList<ExceptionHandlerRecord> ExceptionTable => Record.ExceptionTable;

        /// <summary>
        /// Gets the set of attributes applied to this attribute.
        /// </summary>
        public AttributeReaderCollection Attributes => LazyGet(ref attributes, () => new AttributeReaderCollection(DeclaringClass, Record.Attributes));

    }

}
