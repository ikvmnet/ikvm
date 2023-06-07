using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class ModuleAttributeReader : AttributeReader<ModuleAttributeRecord>
    {

        ModuleConstantReader name;
        Utf8ConstantReader version;

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

        /// <summary>
        /// Gets the module name.
        /// </summary>
        public ModuleConstantReader Name => LazyGet(ref name, () => DeclaringClass.Constants.Get<ModuleConstantReader>(Record.NameIndex));

        /// <summary>
        /// Gets the module version.
        /// </summary>
        public Utf8ConstantReader Version => LazyGet(ref version, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.VersionIndex));

    }

}
