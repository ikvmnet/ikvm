namespace IKVM.ByteCode.Writing
{
    internal abstract class FieldOrMethodBuilder<TRecord> : BuilderBase<TRecord>
    {
        protected FieldOrMethodBuilder(AccessFlag accessFlags, string name, string descriptor, ClassBuilder declaringClass) :
            base(declaringClass)
        {
            AccessFlags = accessFlags;
            NameIndex = declaringClass.AddUtf8(name);
            DescriptorIndex = declaringClass.AddUtf8(descriptor);

            Attributes = new AttributesBuilder(declaringClass);
        }

        protected AccessFlag AccessFlags { get; }

        protected ushort NameIndex { get; }

        protected ushort DescriptorIndex { get; }

        protected AttributesBuilder Attributes { get; set; }
    }
}
