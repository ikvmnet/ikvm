namespace IKVM.ByteCode.Writing
{
    internal abstract class FieldOrMethodWriter<TRecord> : WriterBase<TRecord>
    {
        protected FieldOrMethodWriter(ClassWriter declaringClass) :
            base(declaringClass)
        {
            Attributes = new AttributeWriterCollection(declaringClass);
        }

        public AttributeWriterCollection Attributes { get; }
    }
}
