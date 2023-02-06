namespace IKVM.ByteCode.Writing
{
    internal abstract class RecordsBuilder<TRecord> : BuilderBase<TRecord[]>
    {
        protected RecordsBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        { 
        }
    }
}
