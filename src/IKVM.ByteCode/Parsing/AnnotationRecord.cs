using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public readonly record struct AnnotationRecord(ushort TypeIndex, ElementValuePairRecord[] Elements)
    {

        public static bool TryReadAnnotation(ref SequenceReader<byte> reader, out AnnotationRecord annotation)
        {
            annotation = default;

            if (reader.TryReadBigEndian(out ushort typeIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort pairCount) == false)
                return false;

            var elements = new ElementValuePairRecord[pairCount];
            for (int i = 0; i < pairCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (ElementValueRecord.TryReadElementValue(ref reader, out var elementValue) == false)
                    return false;

                elements[i] = new ElementValuePairRecord(nameIndex, elementValue);
            }

            annotation = new AnnotationRecord(typeIndex, elements);
            return true;
        }
    }

}
