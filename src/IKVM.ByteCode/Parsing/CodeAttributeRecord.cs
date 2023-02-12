using System;
using System.Buffers;

namespace IKVM.ByteCode.Parsing
{
    internal sealed record CodeAttributeRecord(ushort MaxStack, ushort MaxLocals, byte[] Code, ExceptionHandlerRecord[] ExceptionTable, AttributeInfoRecord[] Attributes) : AttributeRecord
    {
        public const string Name = "Code";

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

            var exceptionTable = new ExceptionHandlerRecord[exceptionTableLength];
            for (int i = 0; i < exceptionTableLength; i++)
            {
                if (ExceptionHandlerRecord.TryRead(ref reader, out var j) == false)
                    return false;

                exceptionTable[i] = j;
            }

            if (ClassRecord.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            attribute = new CodeAttributeRecord(maxStack, maxLocals, codeBuffer, exceptionTable, attributes);
            return true;
        }

        public override int GetSize()
        {
            var size = 0;
            size += sizeof(ushort);
            size += sizeof(ushort);
            size += sizeof(uint);
            size += Code.Length * sizeof(byte);

            size += sizeof(ushort);

            foreach (var exceptionHandler in ExceptionTable)
                size += exceptionHandler.GetSize();

            size += sizeof(ushort);

            for (int i = 0; i < Attributes.Length; i++)
                size += Attributes[i].GetSize();

            return size;
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(MaxStack) == false)
                return false;
            if (writer.TryWriteU2(MaxLocals) == false)
                return false;
            if (writer.TryWriteU4((uint)Code.Length) == false)
                return false;
            if (writer.TryWriteManyU1(Code) == false)
                return false;

            if (writer.TryWriteU2((ushort)ExceptionTable.Length) == false)
                return false;

            foreach(var exceptionHandler in ExceptionTable)
                if (exceptionHandler.TryWrite(ref writer) == false)
                    return false;

            if (ClassRecord.TryWriteAttributes(ref writer, Attributes) == false)
                return false;

            return true;
        }
    }
}
