using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public readonly record struct TypeAnnotationRecord(TypeAnnotationTargetRecord Target, TypePathRecord TargetPath, ushort TypeIndex, ElementValuePairRecord[] Elements)
    {

        public static bool TryReadTypeAnnotation(ref SequenceReader<byte> reader, out TypeAnnotationRecord annotation)
        {
            annotation = default;

            if (reader.TryRead(out byte targetType) == false)
                return false;
            if (TypeAnnotationTargetRecord.TryReadTypeAnnotationTarget(ref reader, targetType, out var target) == false)
                return false;
            if (TypePathRecord.TryReadTypePath(ref reader, out var targetPath) == false)
                return false;
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

            annotation = new TypeAnnotationRecord(target, targetPath, typeIndex, elements);
            return true;
        }

    }

}
