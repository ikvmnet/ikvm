namespace IKVM.ByteCode.Parsing
{

    internal abstract record RefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) :
        ConstantRecord
    {



    }

}
