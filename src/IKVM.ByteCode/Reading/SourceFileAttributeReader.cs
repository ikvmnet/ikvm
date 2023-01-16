using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class SourceFileAttributeReader : AttributeData<SourceFileAttributeRecord>
    {

        string sourceFile;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SourceFileAttributeReader(ClassReader declaringClass, AttributeInfoReader info, SourceFileAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

        /// <summary>
        /// Gets the path to the source file.
        /// </summary>
        public string SourceFile => ClassReader.LazyGet(ref sourceFile, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Data.SourceFileIndex).Value);

    }

}
