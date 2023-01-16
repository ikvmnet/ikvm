using System.Buffers;

namespace IKVM.ByteCode
{

    public sealed record CodeAttributeDataRecord(ushort MaxStack, ushort MaxLocals, byte[] Code, ExceptionHandlerRecord[] ExceptionTable, AttributeInfoRecord[] Attributes) : AttributeDataRecord;

}
