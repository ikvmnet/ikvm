using System.Buffers;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    public abstract record VerificationTypeInfoRecord
    {

        public static bool TryReadVerificationTypeInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort tag) == false)
                return false;

            switch (tag)
            {
                case 0:
                    return TryReadTopVariableInfo(ref reader, out record);
                case 1:
                    return TryReadIntegerVariableInfo(ref reader, out record);
                case 2:
                    return TryReadFloatVariableInfo(ref reader, out record);
                case 3:
                    return TryReadDoubleVariableInfo(ref reader, out record);
                case 4:
                    return TryReadLongVariableInfo(ref reader, out record);
                case 5:
                    return TryReadNullVariableInfo(ref reader, out record);
                case 6:
                    return TryReadUnitializedThisVariableInfo(ref reader, out record);
                case 7:
                    return TryReadObjectVariableInfo(ref reader, out record);
                case 8:
                    return TryReadUnitializedVariableInfo(ref reader, out record);
                default:
                    throw new ByteCodeException($"Invalid verification info tag: '{tag}'.");
            }
        }

        static bool TryReadTopVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new TopVariableInfoRecord();
            return true;
        }

        static bool TryReadIntegerVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new IntegerVariableInfoRecord();
            return true;
        }

        static bool TryReadFloatVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new FloatVariableInfoRecord();
            return true;
        }

        static bool TryReadDoubleVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new DoubleVariableInfoRecord();
            return true;
        }

        static bool TryReadLongVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new LongVariableInfoRecord();
            return true;
        }

        static bool TryReadNullVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new NullVariableInfoRecord();
            return true;
        }

        static bool TryReadUnitializedThisVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new UninitalizedThisVariableInfoRecord();
            return true;
        }

        static bool TryReadObjectVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;

            record = new ObjectVariableInfoRecord(classIndex);
            return true;
        }

        static bool TryReadUnitializedVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort offset) == false)
                return false;

            record = new UninitializedVariableInfoRecord(offset);
            return true;
        }

    }

}
