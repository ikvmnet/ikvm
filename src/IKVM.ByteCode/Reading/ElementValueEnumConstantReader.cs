using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueEnumConstantReader : ElementValueReader<ElementValueEnumConstantValueRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementValueEnumConstantReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

    }

}