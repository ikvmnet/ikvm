namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationEmptyTargetRecord : TypeAnnotationTargetRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = new TypeAnnotationEmptyTargetRecord();
            return true;
        }

        public override int GetSize()
        {
            return 0;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            return true;
        }

    }

}