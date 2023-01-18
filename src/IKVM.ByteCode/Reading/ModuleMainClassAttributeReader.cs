using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ModuleMainClassAttributeReader : AttributeReader<ModuleMainClassAttributeRecord>
    {

        /// <summary>
        /// initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public ModuleMainClassAttributeReader(ClassReader declaringClass, AttributeInfoReader info, ModuleMainClassAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
