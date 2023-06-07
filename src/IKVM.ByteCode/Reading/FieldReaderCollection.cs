using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of fields.
    /// </summary>
    public sealed class FieldReaderCollection : LazyNamedReaderDictionary<FieldReader, FieldRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        internal FieldReaderCollection(ClassReader declaringClass, FieldRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new field reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override FieldReader CreateReader(int index, FieldRecord record)
        {
            return new FieldReader(DeclaringClass, record);
        }

        /// <summary>
        /// Gets the key for the specified record.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override string GetName(int index, FieldRecord record)
        {
            return DeclaringClass.Constants.Get<Utf8ConstantReader>(record.NameIndex).Value;
        }

    }

}
