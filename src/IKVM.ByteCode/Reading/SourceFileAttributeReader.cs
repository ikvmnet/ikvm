using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public class SourceFileAttributeReader : AttributeReader<SourceFileAttributeRecord>
    {

        Utf8ConstantReader sourceFile;

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
        public Utf8ConstantReader SourceFile => LazyGet(ref sourceFile, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.SourceFileIndex));

    }

}
