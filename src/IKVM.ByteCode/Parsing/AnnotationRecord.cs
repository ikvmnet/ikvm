using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Writing;

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
                if (ElementValuePairRecord.TryRead(ref reader, out var element) == false)
                    return false;

                elements[i] = element;
            }

            annotation = new AnnotationRecord(typeIndex, elements);
            return true;
        }

        public int GetSize()
        {
            var size = 0;
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
            if (writer.TryWrite(TypeIndex) == false)
                return false;
            if (writer.TryWrite((ushort)Elements.Length) == false)
                return false;

            foreach (var element in Elements)
                if (element.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }

}
