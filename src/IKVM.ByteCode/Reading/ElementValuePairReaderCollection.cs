using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of element value data.
    /// </summary>
    internal sealed class ElementValueKeyReaderCollection : LazyNamedReaderDictionary<ElementValueReader, ElementValuePairRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        /// <param name="minIndex"></param>
        public ElementValueKeyReaderCollection(ClassReader declaringClass, ElementValuePairRecord[] records, int minIndex = 0) :
            base(declaringClass, records, minIndex)
        {

        }

        /// <summary>
        /// Creates a new reader for the given record at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override ElementValueReader CreateReader(int index, ElementValuePairRecord record)
        {
            return ElementValueReader.Resolve(DeclaringClass, record.Value);
        }

        /// <summary>
        /// Gets the key for the given record and the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override string GetKey(int index, ElementValuePairRecord record)
        {
            return DeclaringClass.ResolveConstant<Utf8ConstantReader>(record.NameIndex).Value;
        }

    }

}
