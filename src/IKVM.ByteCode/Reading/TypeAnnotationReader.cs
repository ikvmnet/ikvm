using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class TypeAnnotationReader : ReaderBase<TypeAnnotationRecord>
    {

        string type;
        ElementValuePairReaderCollection elements;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public TypeAnnotationReader(ClassReader declaringClass, TypeAnnotationRecord record) :
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
        public ElementValuePairReaderCollection Elements => LazyGet(ref elements, () => new ElementValuePairReaderCollection(DeclaringClass, Record.Elements));

    }

}
