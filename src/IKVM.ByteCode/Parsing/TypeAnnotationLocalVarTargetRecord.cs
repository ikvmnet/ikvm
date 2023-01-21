namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationLocalVarTargetRecord(TypeAnnotationLocalVarTargetItemRecord[] Items) : TypeAnnotationTargetRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU2(out ushort length) == false)
                return false;

            var items = new TypeAnnotationLocalVarTargetItemRecord[length];
            for (int i = 0; i < length; i++)
                if (TypeAnnotationLocalVarTargetItemRecord.TryRead(ref reader, out items[i]) == false)
                    return false;

            targetInfo = new TypeAnnotationLocalVarTargetRecord(items);
            return true;
        }

        /// <summary>
        /// Gets the size of the record if written.
        /// </summary>
        /// <returns></returns>
        public override int GetSize()
        {
            var length = 0;
            length += sizeof(ushort);

            foreach (var item in Items)
                length += item.GetSize();

            return length;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)Items.Length) == false)
                return false;

            foreach (var item in Items)
                if (item.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }

}