using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class SourceDebugExtensionAttributeReader : AttributeReader<SourceDebugExtensionAttributeRecord>
    {

        /// <summary>
        /// Initalizes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SourceDebugExtensionAttributeReader(ClassReader declaringClass, AttributeInfoReader info, SourceDebugExtensionAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
