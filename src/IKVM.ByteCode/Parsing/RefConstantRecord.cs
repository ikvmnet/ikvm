namespace IKVM.ByteCode.Parsing
{

    public abstract record RefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) :
        ConstantRecord
    {



    }

}
