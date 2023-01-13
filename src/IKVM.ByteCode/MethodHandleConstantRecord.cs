namespace IKVM.ByteCode
{

    public class MethodHandleConstant : Constant
    {

        readonly ReferenceKind kind;
        readonly ushort referenceIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="referenceIndex"></param>
        public MethodHandleConstant(ReferenceKind kind, ushort referenceIndex)
        {
            this.kind = kind;
            this.referenceIndex = referenceIndex;
        }

        public ReferenceKind Kind => kind;

        public ushort ReferenceIndex => referenceIndex;

    }

}