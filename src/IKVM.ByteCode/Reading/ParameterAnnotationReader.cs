using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class ParameterAnnotationReader : ReaderBase<ParameterAnnotationRecord>
    {

        AnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ParameterAnnotationReader(ClassReader declaringClass, ParameterAnnotationRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the element values of the annotation.
        /// </summary>
        public AnnotationReaderCollection Annotations => LazyGet(ref annotations, () => new AnnotationReaderCollection(DeclaringClass, Record.Annotations));

    }

}
