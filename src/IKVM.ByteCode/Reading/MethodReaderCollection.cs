using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of methods.
    /// </summary>
    public sealed class MethodReaderCollection : LazyNamedReaderDictionary<MethodReader, MethodRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        internal MethodReaderCollection(ClassReader declaringClass, MethodRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new method reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override MethodReader CreateReader(int index, MethodRecord record)
        {
            return new MethodReader(DeclaringClass, record);
        }

        /// <summary>
        /// Gets the name ofr the given record.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override string GetName(int index, MethodRecord record)
        {
            return DeclaringClass.Constants.Get<Utf8ConstantReader>(record.NameIndex).Value;
        }

    }

}
