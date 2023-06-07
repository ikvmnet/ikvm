using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class RuntimeInvisibleParameterAnnotationsAttributeReader : AttributeReader<RuntimeInvisibleParameterAnnotationsAttributeRecord>
    {

        ParameterAnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        public RuntimeInvisibleParameterAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeInvisibleParameterAnnotationsAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the set of annotations described by this attribute.
        /// </summary>
        public IReadOnlyList<ParameterAnnotationReader> Annotations => LazyGet(ref annotations, () => new ParameterAnnotationReaderCollection(DeclaringClass, Record.Parameters));

    }

}
