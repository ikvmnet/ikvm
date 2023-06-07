using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of annotation data.
    /// </summary>
    public sealed class AnnotationReaderCollection : LazyReaderList<AnnotationReader, AnnotationRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        public AnnotationReaderCollection(ClassReader declaringClass, AnnotationRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override AnnotationReader CreateReader(int index, AnnotationRecord record)
        {
            return new AnnotationReader(DeclaringClass, record);
        }

    }

}
