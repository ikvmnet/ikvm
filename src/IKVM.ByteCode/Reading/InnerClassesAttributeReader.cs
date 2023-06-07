using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class InnerClassesAttributeReader : AttributeReader<InnerClassesAttributeRecord>
    {

        IReadOnlyList<InnerClassesAttributeItemReader> items;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        public InnerClassesAttributeReader(ClassReader declaringClass, AttributeInfoReader info, InnerClassesAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the items on the attribute.
        /// </summary>
        public IReadOnlyList<InnerClassesAttributeItemReader> Items => LazyGet(ref items, () => new DelegateLazyReaderList<InnerClassesAttributeItemReader, InnerClassesAttributeItemRecord>(DeclaringClass, Record.Items, (_, record) => new InnerClassesAttributeItemReader(DeclaringClass, record)));

    }

}
