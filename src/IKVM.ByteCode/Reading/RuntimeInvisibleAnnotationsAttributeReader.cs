using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class RuntimeInvisibleAnnotationsAttributeReader : AttributeReader<RuntimeInvisibleAnnotationsAttributeRecord>
    {

        AnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal RuntimeInvisibleAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeInvisibleAnnotationsAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the set of annotations described by this attribute.
        /// </summary>
        public AnnotationReaderCollection Annotations => LazyGet(ref annotations, () => new AnnotationReaderCollection(DeclaringClass, Record.Annotations));

    }

}