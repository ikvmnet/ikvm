using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class AnnotationReader : ReaderBase<AnnotationRecord>
    {

        string type;
        ElementValueKeyReaderCollection elements;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public AnnotationReader(ClassReader declaringClass, AnnotationRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the type of the annotation.
        /// </summary>
        public string Type => LazyGet(ref type, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.TypeIndex).Value);

        /// <summary>
        /// Gets the element values of the annotation.
        /// </summary>
        public ElementValueKeyReaderCollection Elements => LazyGet(ref elements, () => new ElementValueKeyReaderCollection(DeclaringClass, Record.Elements));

    }

}
