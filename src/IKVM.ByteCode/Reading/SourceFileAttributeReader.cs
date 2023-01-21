using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal class SourceFileAttributeReader : AttributeReader<SourceFileAttributeRecord>
    {

        string sourceFile;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal SourceFileAttributeReader(ClassReader declaringClass, AttributeInfoReader info, SourceFileAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the path to the source file.
        /// </summary>
        public string SourceFile => ClassReader.LazyGet(ref sourceFile, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.SourceFileIndex).Value);

    }

}
