namespace IKVM.ByteCode.Parsing
{
    internal sealed record ModuleMainClassAttributeRecord(ushort MainClassIndex) : AttributeRecord
    {
        public static bool TryReadModuleMainClassAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort mainClassIndex) == false)
                return false;

            attribute = new ModuleMainClassAttributeRecord(mainClassIndex);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(MainClassIndex) == false)
                return false;

            return true;
        }
    }
}
