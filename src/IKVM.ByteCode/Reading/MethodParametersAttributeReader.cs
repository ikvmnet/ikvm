using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodParametersAttributeReader : AttributeData<MethodParametersAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal MethodParametersAttributeReader(ClassReader declaringClass, AttributeInfoReader info, MethodParametersAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}