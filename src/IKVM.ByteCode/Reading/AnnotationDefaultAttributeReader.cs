using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class AnnotationDefaultAttributeReader : AttributeReader<AnnotationDefaultAttributeRecord>
    {

        ElementValueReader defaultValue;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal AnnotationDefaultAttributeReader(ClassReader declaringClass, AttributeInfoReader info, AnnotationDefaultAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        public ElementValueReader DefaultValue => ClassReader.LazyGet(ref defaultValue, () => ElementValueReader.Resolve(DeclaringClass, Record.DefaultValue));

    }

}