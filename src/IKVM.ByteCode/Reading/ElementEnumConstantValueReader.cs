using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementEnumConstantValueReader : ElementValueReader<ElementEnumConstantValueRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementEnumConstantValueReader(ClassReader declaringClass, ElementEnumConstantValueRecord record) :
            base(declaringClass, record)
        {

        }

    }

}