using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record MethodParametersAttributeRecord(MethodParametersAttributeParameterRecord[] Parameters) : AttributeRecord
    {

        public static bool TryReadMethodParametersAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var arguments = new MethodParametersAttributeParameterRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                    return false;

                arguments[i] = new MethodParametersAttributeParameterRecord(nameIndex, (AccessFlag)accessFlags);
            }

            attribute = new MethodParametersAttributeRecord(arguments);
            return true;
        }

    }

}