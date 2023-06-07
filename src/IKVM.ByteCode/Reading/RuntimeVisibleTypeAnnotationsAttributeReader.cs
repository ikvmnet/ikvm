using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public class RuntimeVisibleTypeAnnotationsAttributeReader : AttributeReader<RuntimeVisibleTypeAnnotationsAttributeRecord>
    {

        TypeAnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleTypeAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeVisibleTypeAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

        /// <summary>
        /// Gets the set of annotations described by this attribute.
        /// </summary>
        public TypeAnnotationReaderCollection Annotations => LazyGet(ref annotations, () => new TypeAnnotationReaderCollection(DeclaringClass, Record.Annotations));

    }

}
