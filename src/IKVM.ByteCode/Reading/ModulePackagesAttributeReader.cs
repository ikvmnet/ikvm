using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ModulePackagesAttributeReader : AttributeReader<ModulePackagesAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal ModulePackagesAttributeReader(ClassReader declaringClass, AttributeInfoReader info, ModulePackagesAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
