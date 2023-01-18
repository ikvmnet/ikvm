using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    public readonly record struct TypeAnnotationRecord(TypeAnnotationTargetRecord Target, TypePathRecord TargetPath, ushort TypeIndex, ElementValuePairRecord[] Elements)
    {

        public static bool TryReadTypeAnnotation(ref SequenceReader<byte> reader, out TypeAnnotationRecord annotation)
        {
            annotation = default;

            if (TypeAnnotationTargetRecord.TryReadTypeAnnotationTarget(ref reader, out var target) == false)
                return false;
            if (TypePathRecord.TryRead(ref reader, out var targetPath) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort typeIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort pairCount) == false)
                return false;

            var elements = new ElementValuePairRecord[pairCount];
            for (int i = 0; i < pairCount; i++)
            {
                if (ElementValuePairRecord.TryRead(ref reader, out var element) == false)
                    return false;

                elements[i] = element;
            }

            annotation = new TypeAnnotationRecord(target, targetPath, typeIndex, elements);
            return true;
        }

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            var size = 0;
            size += Target.GetSize();
            size += TargetPath.GetSize();
            size += sizeof(ushort);
            size += sizeof(ushort);

            foreach (var element in Elements)
                size += element.GetSize();

            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (Target.TryWrite(ref writer) == false)
                return false;
            if (TargetPath.TryWrite(ref writer) == false)
                return false;
            if (writer.TryWrite(TypeIndex) == false)
                return false;
            if (writer.TryWrite((ushort)Elements.Length) == false)
                return false;

            foreach (var record in Elements)
                if (record.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }

}
