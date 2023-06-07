using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of method data.
    /// </summary>
    public sealed class InterfaceReaderCollection : LazyNamedReaderDictionary<InterfaceReader, InterfaceRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        internal InterfaceReaderCollection(ClassReader declaringClass, InterfaceRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new interface reader.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override InterfaceReader CreateReader(int index, InterfaceRecord record)
        {
            return new InterfaceReader(DeclaringClass, record);
        }

        /// <summary>
        /// Gets the name of the interface.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override string GetName(int index, InterfaceRecord record)
        {
            return DeclaringClass.Constants.Get<ClassConstantReader>(record.ClassIndex).Name.Value;
        }

    }

}
