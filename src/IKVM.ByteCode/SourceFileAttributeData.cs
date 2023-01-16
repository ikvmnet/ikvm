namespace IKVM.ByteCode
{

    public class SourceFileAttributeData : AttributeData<SourceFileAttributeDataRecord>
    {

        string sourceFile;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SourceFileAttributeData(Class declaringClass, AttributeInfo info, SourceFileAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

        /// <summary>
        /// Gets the path to the source file.
        /// </summary>
        public string SourceFile => Class.LazyGet(ref sourceFile, () => DeclaringClass.ResolveConstant<Utf8Constant>(Data.SourceFileIndex).Value);

    }

}
