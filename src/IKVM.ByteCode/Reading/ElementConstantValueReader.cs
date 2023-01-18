using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ElementConstantValueReader : ElementValueReader<ElementConstantValueRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementConstantValueReader(ClassReader declaringClass, ElementConstantValueRecord record) :
            base(declaringClass, record)
        {

        }

    }

}