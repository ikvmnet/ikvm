using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class BootstrapMethodsAttributeReader : AttributeData<BootstrapMethodsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal BootstrapMethodsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, BootstrapMethodsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}