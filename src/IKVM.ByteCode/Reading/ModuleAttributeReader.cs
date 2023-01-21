using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ModuleAttributeReader : AttributeReader<ModuleAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public ModuleAttributeReader(ClassReader declaringClass, AttributeInfoReader info, ModuleAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
