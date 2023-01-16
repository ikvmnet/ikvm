using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class PackageConstantReader : Constant<PackageConstantRecord>
    {

        Utf8ConstantReader name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public PackageConstantReader(ClassReader owner, PackageConstantRecord record) :
            base(owner, record)
        {

        }

        public Utf8ConstantReader Name => name ??= DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex);

    }

}
