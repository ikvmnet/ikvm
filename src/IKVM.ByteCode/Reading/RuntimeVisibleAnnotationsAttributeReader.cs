using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public class RuntimeVisibleAnnotationsAttributeReader : AttributeReader<RuntimeVisibleAnnotationsAttributeRecord>
    {

        AnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeVisibleAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

        /// <summary>
        /// Gets the set of annotations described by this attribute.
        /// </summary>
        public AnnotationReaderCollection Annotations => LazyGet(ref annotations, () => new AnnotationReaderCollection(DeclaringClass, Record.Annotations));

    }

}