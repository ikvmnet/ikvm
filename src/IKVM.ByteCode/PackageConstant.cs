namespace IKVM.ByteCode
{

    public sealed class PackageConstant : Constant<PackageConstantRecord>
    {

        Utf8Constant name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public PackageConstant(Class owner, PackageConstantRecord record) :
            base(owner, record)
        {

        }

        public Utf8Constant Name => name ??= DeclaringClass.ResolveConstant<Utf8Constant>(Record.NameIndex);

    }

}
