using System;
using System.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record CodeAttributeRecord(ushort MaxStack, ushort MaxLocals, ReadOnlyMemory<byte> Code, ExceptionHandlerRecord[] ExceptionTable, AttributeInfoRecord[] Attributes) : AttributeRecord
    {

        public static bool TryReadCodeAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort maxStack) == false)
                return false;
            if (reader.TryReadU2(out ushort maxLocals) == false)
                return false;
            if (reader.TryReadU4(out uint codeLength) == false)
                return false;
            if (reader.TryReadManyU1(codeLength, out ReadOnlySequence<byte> code) == false)
                return false;

            var codeBuffer = new byte[code.Length];
            code.CopyTo(codeBuffer);

            if (reader.TryReadU2(out ushort exceptionTableLength) == false)
                return false;

            var exceptionTable = new ExceptionHandlerRecord[(int)exceptionTableLength];
            for (int i = 0; i < exceptionTableLength; i++)
            {
                if (reader.TryReadU2(out ushort startOffset) == false)
                    return false;
                if (reader.TryReadU2(out ushort endOffset) == false)
                    return false;
                if (reader.TryReadU2(out ushort handlerOffset) == false)
                    return false;
                if (reader.TryReadU2(out ushort catchTypeIndex) == false)
                    return false;

                exceptionTable[i] = new ExceptionHandlerRecord(startOffset, endOffset, handlerOffset, catchTypeIndex);
            }

            if (ClassRecord.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            attribute = new CodeAttributeRecord(maxStack, maxLocals, codeBuffer, exceptionTable, attributes);
            return true;
        }

    }

}
