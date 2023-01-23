using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueAnnotationReader : ElementValueReader<ElementValueAnnotationValueRecord>
    {

        AnnotationReader annotation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementValueAnnotationReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the annotation included with this element value.
        /// </summary>
        public AnnotationReader Annotation => LazyGet(ref annotation, () => new AnnotationReader(DeclaringClass, ValueRecord.Annotation));

    }

}