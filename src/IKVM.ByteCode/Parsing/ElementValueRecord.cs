using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    public abstract record class ElementValueRecord
    {

        public static bool TryReadElementValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryRead(out byte tag) == false)
                return false;

            return (char)tag switch
            {
                'B' or 'C' or 'D' or 'F' or 'I' or 'J' or 'S' or 'Z' or 's' => ElementConstantValueRecord.TryReadElementConstantValue(ref reader, out value),
                'e' => ElementEnumConstantValueRecord.TryReadElementEnumConstantValue(ref reader, out value),
                'c' => ElementClassInfoValueRecord.TryReadElementClassInfoValue(ref reader, out value),
                '@' => ElementAnnotationValueRecord.TryReadElementAnnotationValue(ref reader, out value),
                '[' => ElementArrayValueRecord.TryReadElementArrayValue(ref reader, out value),
                _ => throw new ByteCodeException($"Invalid annotation element value tag: '{(char)tag}'."),
            };
        }

    }

}
