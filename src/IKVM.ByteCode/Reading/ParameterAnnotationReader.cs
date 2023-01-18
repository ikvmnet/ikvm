using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ParameterAnnotationReader
    {

        readonly ClassReader declaringClass;
        readonly ParameterAnnotationRecord record;

        AnnotationReaderCollection annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ParameterAnnotationReader(ClassReader declaringClass, ParameterAnnotationRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the class that declared this annotation.
        /// </summary>
        public ClassReader DeclaringClass => declaringClass;

        /// <summary>
        /// Gets the underlying record being read.
        /// </summary>
        public ParameterAnnotationRecord Record => record;

        /// <summary>
        /// Gets the element values of the annotation.
        /// </summary>
        public AnnotationReaderCollection Annotations => ClassReader.LazyGet(ref annotations, () => new AnnotationReaderCollection(declaringClass, record.Annotations));

    }

}
