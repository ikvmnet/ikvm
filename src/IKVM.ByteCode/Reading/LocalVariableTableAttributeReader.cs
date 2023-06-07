using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class LocalVariableTableAttributeReader : AttributeReader<LocalVariableTableAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal LocalVariableTableAttributeReader(ClassReader declaringClass, AttributeInfoReader info, LocalVariableTableAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
