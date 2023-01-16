using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class LocalVariableTypeTableAttributeReader : AttributeData<LocalVariableTypeTableAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal LocalVariableTypeTableAttributeReader(ClassReader declaringClass, AttributeInfoReader info, LocalVariableTypeTableAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
