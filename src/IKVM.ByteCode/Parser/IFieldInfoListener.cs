namespace IKVM.ByteCode.Parser
{

    public interface IFieldInfoListener
    {

        void AcceptAccessFlags(AccessFlag flags);

        void AcceptNameIndex(ConstantPoolIndex index);

        void AcceptDescriptorIndex(ConstantPoolIndex index);

        void AcceptAttributesCount(int count);

        void AcceptAttribute(int index);

    }

}