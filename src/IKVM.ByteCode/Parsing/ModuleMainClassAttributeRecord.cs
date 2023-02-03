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

    }

}
