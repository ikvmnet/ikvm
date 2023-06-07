using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public class RuntimeVisibleParameterAnnotationsAttributeReader : AttributeReader<RuntimeVisibleParameterAnnotationsAttributeRecord>
    {

        ParameterAnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal RuntimeVisibleParameterAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeVisibleParameterAnnotationsAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the set of annotations described by this attribute.
        /// </summary>
        public IReadOnlyList<ParameterAnnotationReader> Parameters => LazyGet(ref annotations, () => new ParameterAnnotationReaderCollection(DeclaringClass, Record.Parameters));

    }

}