using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueArrayReader : ElementValueReader<ElementValueArrayValueRecord>
    {

        ElementValueReaderCollection values;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementValueArrayReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

        public ElementValueReaderCollection Values => LazyGet(ref values, () => new ElementValueReaderCollection(DeclaringClass, ValueRecord.Values));

    }

}
