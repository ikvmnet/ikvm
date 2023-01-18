using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class PackageConstantReader : ConstantReader<PackageConstantRecord, PackageConstantOverride>
    {

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public PackageConstantReader(ClassReader owner, PackageConstantRecord record, PackageConstantOverride @override = null) :
            base(owner, record, @override)
        {

        }

        /// <summary>
        /// Gest the name of this package.
        /// </summary>
        public string Name => LazyGet(ref name, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex).Value);

    }

}
