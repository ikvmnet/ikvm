using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    public sealed record RuntimeVisibleTypeAnnotationsAttributeRecord(TypeAnnotationRecord[] Annotations) : AttributeRecord
    {

        public static bool TryReadRuntimeVisibleTypeAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var annotations = new TypeAnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TypeAnnotationRecord.TryReadTypeAnnotation(ref reader, out var annotation) == false)
                    return false;

                annotations[i] = annotation;
            }

            attribute = new RuntimeVisibleTypeAnnotationsAttributeRecord(annotations);
            return true;
        }

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            var size = 0;
            size += sizeof(ushort);

            foreach (var element in Annotations)
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
            if (writer.TryWrite((ushort)Annotations.Length) == false)
                return false;

            foreach (var record in Annotations)
                if (record.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }

}