using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal abstract class AttributeBuilder<TRecord> : BuilderBase<TRecord>
        where TRecord : AttributeRecord
    {
        protected AttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public abstract string Name { get; }
    }
}
