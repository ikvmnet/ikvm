using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ElementArrayValueReader : ElementValueReader<ElementArrayValueRecord>
    {

        ElementValueReaderCollection values;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementArrayValueReader(ClassReader declaringClass, ElementArrayValueRecord record) :
            base(declaringClass, record)
        {

        }

        public ElementValueReaderCollection Values => ClassReader.LazyGet(ref values, () => new ElementValueReaderCollection(DeclaringClass, Record.Values));

    }

}