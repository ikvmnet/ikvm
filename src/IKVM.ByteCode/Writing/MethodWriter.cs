using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class MethodWriter : FieldOrMethodWriter<MethodRecord>
    {
        private readonly AccessFlag accessFlags;
        private readonly ushort nameIndex;
        private readonly ushort descriptorIndex;

        public MethodWriter(AccessFlag accessFlags, string name, string descriptor, ClassWriter declaringClass) :
            base(declaringClass)
        {
            this.accessFlags = accessFlags;
            this.nameIndex = declaringClass.AddUtf8(name);
            this.descriptorIndex = declaringClass.AddUtf8(descriptor);
        }

        internal override MethodRecord Record => new
            (
                AccessFlags: accessFlags,
                NameIndex: nameIndex,
                DescriptorIndex: descriptorIndex,
                Attributes: new AttributeInfoRecord[0]
            );
    }
}
