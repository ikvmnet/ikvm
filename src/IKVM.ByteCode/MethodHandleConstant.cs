namespace IKVM.ByteCode
{

    public sealed class MethodHandleConstant : Constant<MethodHandleConstantRecord>
    {

        Constant reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public MethodHandleConstant(Class owner, MethodHandleConstantRecord record) :
            base(owner, record)
        {

        }

        public ReferenceKind Kind => Record.Kind;

        public Constant Reference => reference ??= DeclaringClass.ResolveConstant<Constant>(Record.ReferenceIndex);

    }

}
