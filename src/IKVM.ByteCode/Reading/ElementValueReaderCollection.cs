using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of attribute data.
    /// </summary>
    internal sealed class ElementValueReaderCollection : LazyReaderList<ElementValueReader, ElementValueRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="delcaringClass"></param>
        /// <param name="records"></param>
        public ElementValueReaderCollection(ClassReader delcaringClass, ElementValueRecord[] records) :
            base(delcaringClass, records)
        {

        }

        protected override ElementValueReader CreateReader(int index, ElementValueRecord record)
        {
            return ElementValueReader.Resolve(DeclaringClass, record);
        }

    }

}
