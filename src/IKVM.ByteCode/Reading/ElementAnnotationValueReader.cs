using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementAnnotationValueReader : ElementValueReader<ElementAnnotationValueRecord>
    {

        AnnotationReader annotation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementAnnotationValueReader(ClassReader declaringClass, ElementAnnotationValueRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the annotation included with this element value.
        /// </summary>
        public AnnotationReader Annotation => ClassReader.LazyGet(ref annotation, () => new AnnotationReader(DeclaringClass, Record.Annotation));

    }

}