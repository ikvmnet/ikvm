namespace IKVM.ByteCode.Parsing
{
    internal sealed record ModulePackagesAttributeRecord(ushort[] Packages) : AttributeRecord
    {
        public static bool TryReadModulePackagesAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var packages = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadU2(out ushort packageIndex) == false)
                    return false;

                packages[i] = packageIndex;
            }

            attribute = new ModulePackagesAttributeRecord(packages);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort) + Packages.Length * sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)Packages.Length) == false)
                return false;

            foreach (var packageIndex in Packages)
                if (writer.TryWriteU2(packageIndex) == false)
                    return false;

            return true;
        }
    }
}