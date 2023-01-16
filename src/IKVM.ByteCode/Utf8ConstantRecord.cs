using System.Buffers;

namespace IKVM.ByteCode
{

    public sealed record Utf8ConstantRecord(byte[] Value) : ConstantRecord;

}
