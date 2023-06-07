using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class RuntimeInvisibleTypeAnnotationsAttributeReader : AttributeReader<RuntimeInvisibleTypeAnnotationsAttributeRecord>
    {

        TypeAnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeInvisibleTypeAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeInvisibleTypeAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

        /// <summary>
        /// Gets the set of annotations described by this attribute.
        /// </summary>
        public TypeAnnotationReaderCollection Annotations => LazyGet(ref annotations, () => new TypeAnnotationReaderCollection(DeclaringClass, Record.Annotations));

    }

}
